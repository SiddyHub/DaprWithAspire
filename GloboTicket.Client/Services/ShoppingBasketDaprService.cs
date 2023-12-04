using Dapr.Client;
using GloboTicket.Web.Messages;
using GloboTicket.Web.Models;
using GloboTicket.Web.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public class ShoppingBasketDaprService : IShoppingBasketService
    {
        private readonly DaprClient daprClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly Settings settings;
        private readonly ILogger<ShoppingBasketDaprService> logger;
        private readonly IEventCatalogService eventCatalogService;
        private const string stateStoreName = "shopstate";

        public ShoppingBasketDaprService(DaprClient daprClient, Settings settings, IHttpContextAccessor httpContextAccessor, 
            ILogger<ShoppingBasketDaprService> logger,
            IEventCatalogService eventCatalogService)
        {
            this.daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventCatalogService = eventCatalogService ?? throw new ArgumentNullException(nameof(eventCatalogService));
        }

        public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLineForCreation)
        {            
            logger.LogInformation($"ADD TO BASKET {basketId}");
            var basket = await GetBasketFromStateStore(basketId);
            var @event = await GetEventFromStateStore(basketLineForCreation.EventId);

            var basketLine = new BasketLine()
            {
                EventId = basketLineForCreation.EventId,
                TicketAmount = basketLineForCreation.TicketAmount,
                Event = @event,
                BasketId = basket.BasketId,
                BasketLineId = Guid.NewGuid(),
                Price = basketLineForCreation.Price
            };            
            basket.Lines.Add(basketLine);            
            logger.LogInformation($"SAVING BASKET {basket.BasketId}");
            await SaveBasketToStateStore(basket);
            daprClient.CreateInvokeMethodRequest(HttpMethod.Post, "shoppingbasket", $"/api/baskets/{basket.UserId}/basketlines", basketLineForCreation);
            return basketLine;
        }

        public async Task<Basket> GetBasket(Guid basketId)
        {
            if (basketId == Guid.Empty)
                return null;            

            logger.LogInformation($"GET BASKET {basketId}");
            var basket = await GetBasketFromStateStore(basketId);
            return new Basket() { BasketId = basketId, NumberOfItems = basket.Lines.Count, UserId = basket.UserId, CouponId = basket.CouponId };
        }

        public async Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId)
        {
            if (basketId == Guid.Empty)
                return new BasketLine[0];            

            var basket = await GetBasketFromStateStore(basketId);
            return basket.Lines;
        }

        public async Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate)
        {                       
            var basket = await GetBasketFromStateStore(basketId);
            var index = basket.Lines.FindIndex(bl => bl.BasketLineId == basketLineForUpdate.LineId);
            basket.Lines[index].TicketAmount = basketLineForUpdate.TicketAmount;
            await SaveBasketToStateStore(basket);
        }

        public async Task UpdateLinePrices(Guid basketId, PriceUpdate priceUpdate)
        {
            var basket = await GetBasketFromStateStore(basketId);
            var basketLinesToUpdate = basket.Lines.Where(bl => bl.EventId == priceUpdate.EventId);

            foreach(var line in basketLinesToUpdate)
            {            
                line.Price = priceUpdate.Price;
            }
            await SaveBasketToStateStore(basket);
        }

        public async Task RemoveLine(Guid basketId, Guid lineId)
        {                               
            var basket = await GetBasketFromStateStore(basketId);
            var index = basket.Lines.FindIndex(bl => bl.BasketLineId == lineId);
            if (index >= 0)
            {
                await daprClient.InvokeMethodAsync(HttpMethod.Delete, "shoppingbasket", $"/api/baskets/{basket.UserId}/basketLines/{basket.Lines[index].EventId}");
                basket.Lines.RemoveAt(index);
            }
            await SaveBasketToStateStore(basket);
        }

        private async Task SaveBasketToStateStore(StateStoreBasket basket)
        {
            var key = $"basket-{basket.BasketId}";
            await daprClient.SaveStateAsync(stateStoreName, key, basket);
            logger.LogInformation($"Created new basket in state store {key}");
        }

        private async Task SaveEventToStateStore(Event @event)
        {
            var key = $"event-{@event.EventId}";
            logger.LogInformation($"Saving event to state store {key}");
            await daprClient.SaveStateAsync(stateStoreName, key, @event);
        }


        private async Task<StateStoreBasket> GetBasketFromStateStore(Guid basketId)
        {
            var key = $"basket-{basketId}";
            var basket = await daprClient.GetStateAsync<StateStoreBasket>(stateStoreName, key);
            if (basket == null)
            {
                if (basketId == Guid.Empty) basketId = Guid.NewGuid();                
                basket = new StateStoreBasket();
                basket.BasketId = basketId;
                // basket.UserId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                basket.UserId = settings.UserId;
                basket.Lines = new List<BasketLine>();                
                await SaveBasketToStateStore(basket);
            }
            return basket;
        }

        private async Task<Event> GetEventFromStateStore(Guid eventId)
        {
            var key = $"event-{eventId}";
            var @event = await daprClient.GetStateAsync<Event>(stateStoreName, key);

            if (@event != null)
            {
                logger.LogInformation("Using cached event");
            }
            else
            {
                @event = await eventCatalogService.GetEvent(eventId);
                await SaveEventToStateStore(@event);
            }
            return @event;
        }

        public async Task ClearBasket(Guid basketId)
        {
            var basket = await GetBasketFromStateStore(basketId);
            basket.Lines.Clear();
            basket.CouponId = null;            
            await SaveBasketToStateStore(basket);
        }

        public async Task Checkout(Guid basketId, BasketForCheckout basketForCheckout)
        {          
            var basket = await GetBasketFromStateStore(basketId);

            var basketCheckoutMessage = new BasketCheckoutMessage();
            basketCheckoutMessage.BasketLines = new List<BasketLineMessage>();
            int total = 0;

            basketCheckoutMessage.UserId = basketForCheckout.UserId;
            basketCheckoutMessage.CardExpiration = basketForCheckout.CardExpiration;
            basketCheckoutMessage.CardName = basketForCheckout.CardName;
            basketCheckoutMessage.CardNumber = basketForCheckout.CardNumber;
            basketCheckoutMessage.Email = basketForCheckout.Email;

            foreach (var b in basket.Lines)
            {
                var basketLineMessage = new BasketLineMessage
                {
                    BasketLineId = b.BasketLineId,
                    Price = b.Price,
                    TicketAmount = b.TicketAmount
                };

                total += b.Price * b.TicketAmount;

                basketCheckoutMessage.BasketLines.Add(basketLineMessage);
            }

            //apply discount by talking to the discount service
            Coupon coupon = new Coupon();

            if (basket.CouponId.HasValue)
            {
                var data = new GloboTicket.Grpc.GetCouponByIdRequest { CouponId = basket.CouponId.Value.ToString() };
                var result = await daprClient.InvokeMethodGrpcAsync<GloboTicket.Grpc.GetCouponByIdRequest, GloboTicket.Grpc.Coupon>("discountgrpc", "GetCouponById", data);
                if (result != null)
                {
                    coupon.AlreadyUsed = result.AlreadyUsed;
                    coupon.Amount = result.Amount;
                    coupon.Code = result.Code;
                    coupon.CouponId = Guid.Parse(result.CouponId);
                }
            }

            if (coupon != null)
            {
                basketCheckoutMessage.BasketTotal = total - coupon.Amount;
            }
            else
            {
                basketCheckoutMessage.BasketTotal = total;
            }

            try
            {
                await daprClient.PublishEventAsync("pubsub", "checkoutmessage", basketCheckoutMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await ClearBasket(basketId);            
        }

        public async Task ApplyCouponToBasket(Guid basketId, CouponForUpdate couponForUpdate)
        {
            var key = $"basket-{basketId}";
            var basket = await daprClient.GetStateAsync<StateStoreBasket>(stateStoreName, key);
            if (basket != null)
            {
                basket.CouponId = couponForUpdate.CouponId;
            }
            await SaveBasketToStateStore(basket);            
        }
    }
}
