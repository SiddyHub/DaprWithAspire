using GloboTicket.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public class StateStoreBasket
    {
        public Guid BasketId { get; set; }
        public List<BasketLine> Lines { get; set; } = new List<BasketLine>();        
        public Guid UserId { get; set; }
        public Guid? CouponId { get; set; }
    }
}
