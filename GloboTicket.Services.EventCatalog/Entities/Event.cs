using System;
using System.ComponentModel.DataAnnotations;

namespace GloboTicket.Services.EventCatalog.Entities
{
    public class Event
    {
        [Required]
        public Guid EventId { get; set; }
        public string Name { get; set; }

        // CategoryId is now the partitionKey used in Cosmos Db
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int Price { get; set; }
        public string Artist { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string VenueName { get; set; }
        public string VenueCity { get; set; }
        public string VenueCountry { get; set; }

    }
}
