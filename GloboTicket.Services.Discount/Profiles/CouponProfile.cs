using AutoMapper;
using GloboTicket.Services.Discount.Entities;
using GloboTicket.Services.Discount.Models;

namespace GloboTicket.Services.Discount.Profiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }
    }
}
