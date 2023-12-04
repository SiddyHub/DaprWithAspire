using Dapr;
using Dapr.Client;
using GloboTicket.Services.Payment.Messages;
using GloboTicket.Services.Payment.Model;
using GloboTicket.Services.Payment.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.Payment.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        private readonly IExternalGatewayPaymentService externalGatewayPaymentService;
        private readonly DaprClient daprClient;

        public PaymentController(IExternalGatewayPaymentService externalGatewayPaymentService, DaprClient daprClient)
        {
            this.externalGatewayPaymentService = externalGatewayPaymentService;
            this.daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        [HttpPost("", Name = "PaymentOrder")]
        [Topic("pubsub", "orderpaymentrequestmessage")]
        public async Task<IActionResult> Pay(OrderPaymentRequestMessage orderPaymentRequestMessage)
        {
            PaymentInfo paymentInfo = new PaymentInfo
            {
                CardNumber = orderPaymentRequestMessage.CardNumber,
                CardName = orderPaymentRequestMessage.CardName,
                CardExpiration = orderPaymentRequestMessage.CardExpiration,
                Total = orderPaymentRequestMessage.Total
            };

            var result = await externalGatewayPaymentService.PerformPayment(paymentInfo);

            //send payment result to order service via service bus
            OrderPaymentUpdateMessage orderPaymentUpdateMessage = new OrderPaymentUpdateMessage
            {
                PaymentSuccess = result,
                OrderId = orderPaymentRequestMessage.OrderId
            };

            try
            {
                await daprClient.PublishEventAsync("pubsub", "orderpaymentupdatedmessage", orderPaymentUpdateMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok();
        }
    }
}
