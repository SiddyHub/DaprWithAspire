using System;
using GloboTicket.Services.Discount.Entities;
using System.Threading.Tasks;

namespace GloboTicket.Services.Discount.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetCouponByCode(string couponCode);
        Task UseCoupon(Guid couponId);
        Task<Coupon> GetCouponById(Guid couponId);
    }
}
