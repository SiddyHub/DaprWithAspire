using Dapr.Client;
using GloboTicket.Services.Ordering.Messages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Services.Ordering.Messaging
{
    public class EmailSender
    {
        private readonly DaprClient daprClient;
        private readonly ILogger<EmailSender> logger;

        public EmailSender(DaprClient daprClient, ILogger<EmailSender> logger)
        {
            this.daprClient = daprClient;
            this.logger = logger;
        }

        public async Task SendEmailForOrder(BasketCheckoutMessage order)
        {
            logger.LogInformation($"Received a new order for {order.Email}");

            logger.LogInformation($"Sending email");
            var metadata = new Dictionary<string, string>
            {
                ["emailFrom"] = "noreply@globoticket.shop",
                ["emailTo"] = order.Email,
                ["subject"] = $"Thank you for your order"
            };
            var body = $"<h2>Your order has been received</h2>"
                + "<p>Your tickets are on the way!</p>";
            await daprClient.InvokeBindingAsync("sendmail", "create",
                body, metadata);
        }
    }
}