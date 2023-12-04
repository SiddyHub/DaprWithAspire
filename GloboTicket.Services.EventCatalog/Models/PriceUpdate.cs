using System;

namespace GloboTicket.Services.EventCatalog.Models
{
    public class PriceUpdate
    {
        public Guid EventId { get; set; }
        public int Price { get; set; }
    }
}
