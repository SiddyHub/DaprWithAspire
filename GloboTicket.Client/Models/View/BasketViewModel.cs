using System;
using System.Collections.Generic;

namespace GloboTicket.Web.Models.View
{
    public class BasketViewModel
    {
        public Guid BasketId { get; set; }
        public List<BasketLineViewModel> BasketLines { get; set; }
        public int ShoppingCartTotal { get; set; }
        public string Code { get; set; }
        public int Discount { get; set; }
    }
}
