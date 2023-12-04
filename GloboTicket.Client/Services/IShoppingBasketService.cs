using GloboTicket.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public interface IShoppingBasketService
    {
        Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine);
        Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId);
        Task<Basket> GetBasket(Guid basketId);
        Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate);
        Task UpdateLinePrices(Guid basketId, PriceUpdate priceUpdate);
        Task RemoveLine(Guid basketId, Guid lineId);
        Task ClearBasket(Guid currentBasketId);
        Task ApplyCouponToBasket(Guid basketId, CouponForUpdate couponForUpdate);
        Task Checkout(Guid basketId, BasketForCheckout basketForCheckout);
    }
}
