using System.Threading.Tasks;
using GloboTicket.Services.Marketing.Entities;

namespace GloboTicket.Services.Marketing.Repositories
{
    public interface IBasketChangeEventRepository
    {
        Task AddBasketChangeEvent(BasketChangeEvent basketChangeEvent);
    }
}