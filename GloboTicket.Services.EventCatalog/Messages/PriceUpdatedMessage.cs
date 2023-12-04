using GloboTicket.Integration.Messages;
using System;

namespace GloboTicket.Services.EventCatalog.Messages
{
    public class PriceUpdatedMessage: IntegrationBaseMessage
    {
        public Guid EventId { get; set; }
        public int Price { get; set; }
    }
}
