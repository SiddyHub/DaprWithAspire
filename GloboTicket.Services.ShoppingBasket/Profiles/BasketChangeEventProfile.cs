using AutoMapper;
using GloboTicket.Services.ShoppingBasket.Entities;
using GloboTicket.Services.ShoppingBasket.Models;

namespace GloboTicket.Services.ShoppingBasket.Profiles
{
    public class BasketChangeEventProfile: Profile
    {
        public BasketChangeEventProfile()
        {
            CreateMap<BasketChangeEvent, BasketChangeEventForPublication>().ReverseMap();
        }
    }
}
