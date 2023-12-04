using GloboTicket.Web.Models.Api;
using System.Collections.Generic;

namespace GloboTicket.Web.Models.View
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
