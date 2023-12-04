using System;
using GloboTicket.Integration.Messages;

namespace GloboTicket.Services.Payment.Messages
{
    public class OrderPaymentUpdateMessage: IntegrationBaseMessage
    {
        public Guid OrderId { get; set; }
        public bool PaymentSuccess { get; set; }
    }
}
