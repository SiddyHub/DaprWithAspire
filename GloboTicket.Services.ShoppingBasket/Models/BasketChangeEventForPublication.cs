using GloboTicket.Services.ShoppingBasket.Entities;
using System;

namespace GloboTicket.Services.ShoppingBasket.Models
{
    public class BasketChangeEventForPublication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTimeOffset InsertedAt { get; set; }
        public BasketChangeTypeEnum BasketChangeType { get; set; }
    }
}
