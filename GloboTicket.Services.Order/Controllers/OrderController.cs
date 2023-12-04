using Dapr;
using Dapr.Client;
using GloboTicket.Services.Ordering.Entities;
using GloboTicket.Services.Ordering.Messages;
using GloboTicket.Services.Ordering.Messaging;
using GloboTicket.Services.Ordering.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GloboTicket.Services.Ordering.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly DaprClient daprClient;
        private readonly EmailSender emailSender;

        public OrderController(IOrderRepository orderRepository, DaprClient daprClient, EmailSender emailSender)
        {
            _orderRepository = orderRepository;
            this.daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            this.emailSender = emailSender;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> List(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersForUser(userId);
            return Ok(orders);
        }

        [HttpPost("submitorder")]
        [Topic("pubsub", "checkoutmessage")]
        public async Task<IActionResult> Submit(BasketCheckoutMessage basketCheckoutMessage)
        {
            Guid orderId = Guid.NewGuid();

            Order order = new Order
            {
                UserId = basketCheckoutMessage.UserId,
                Id = orderId,
                OrderPaid = false,
                OrderPlaced = DateTime.Now,
                OrderTotal = basketCheckoutMessage.BasketTotal
            };

            await _orderRepository.AddOrder(order);
            await emailSender.SendEmailForOrder(basketCheckoutMessage);

            OrderPaymentRequestMessage orderPaymentRequestMessage = new OrderPaymentRequestMessage
            {
                CardExpiration = basketCheckoutMessage.CardExpiration,
                CardName = basketCheckoutMessage.CardName,
                CardNumber = basketCheckoutMessage.CardNumber,
                OrderId = orderId,
                Total = basketCheckoutMessage.BasketTotal
            };

            try
            {
                await daprClient.PublishEventAsync("pubsub", "orderpaymentrequestmessage", orderPaymentRequestMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok();
        }

        [HttpPost("updateorder")]
        [Topic("pubsub", "orderpaymentupdatedmessage")]
        public async Task<IActionResult> UpdateOrder (OrderPaymentUpdateMessage orderPaymentUpdateMessage)
        {
            await _orderRepository.UpdateOrderPaymentStatus(orderPaymentUpdateMessage.OrderId, orderPaymentUpdateMessage.PaymentSuccess);
            return Ok();
        }
    }
}
