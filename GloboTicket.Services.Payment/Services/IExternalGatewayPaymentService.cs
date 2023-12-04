using GloboTicket.Services.Payment.Model;
using System.Threading.Tasks;

namespace GloboTicket.Services.Payment.Services
{
    public interface IExternalGatewayPaymentService
    {
        Task<bool> PerformPayment(PaymentInfo paymentInfo);
    }
}
