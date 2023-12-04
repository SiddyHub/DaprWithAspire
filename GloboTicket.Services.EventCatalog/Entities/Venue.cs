using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.EventCatalog.Entities
{
    public class Venue
    {
        [Required]
        //public Guid VenueId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
