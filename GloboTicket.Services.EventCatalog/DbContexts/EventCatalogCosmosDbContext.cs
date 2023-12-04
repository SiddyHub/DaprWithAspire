using GloboTicket.Services.EventCatalog.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.EventCatalog.DbContexts
{
    public class EventCatalogCosmosDbContext : DbContext
    {
        public EventCatalogCosmosDbContext(DbContextOptions<EventCatalogCosmosDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Events");

            modelBuilder.Entity<Event>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Event>()
                .HasPartitionKey(o => o.CategoryId);

            modelBuilder.Entity<Category>()
                .ToContainer("Categories");

            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            var CategoryConcert = new Category
            {
                CategoryId = concertGuid,
                Name = "Concerts"
            };
            modelBuilder.Entity<Category>().HasData(CategoryConcert);

            var CategoryMusicals = new Category
            {
                CategoryId = musicalGuid,
                Name = "Musicals"
            };
            modelBuilder.Entity<Category>().HasData(CategoryMusicals);

            var CategoryPlays = new Category
            {
                CategoryId = playGuid,
                Name = "Plays"
            };
            modelBuilder.Entity<Category>().HasData(CategoryPlays);

            var CategoryConferences = new Category
            {
                CategoryId = conferenceGuid,
                Name = "Conferences"
            };
            modelBuilder.Entity<Category>().HasData(CategoryConferences);

            var Venue1 = new Venue
            {
                Name = "Massey Hall",
                City = "Toronto",
                Country = "Canada"
            };

            var Venue2 = new Venue
            {
                Name = "L'Olympia",
                City = "Montreal",
                Country = "Canada"
            };

            var Venue3 = new Venue
            {
                Name = "Commodore Ballroom",
                City = "Vancouver",
                Country = "Canada"
            };

            DateTime eventDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                Name = "John Egbert Live",
                Price = 65,
                Artist = "John Egbert",
                Date = eventDate.AddMonths(6),
                Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",

                CategoryId = CategoryConcert.CategoryId.ToString(),
                CategoryName = CategoryConcert.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                Name = "The State of Affairs: Michael Live!",
                Price = 85,
                Artist = "Michael Johnson",
                Date = eventDate.AddMonths(9),
                Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/michael.jpg",

                CategoryId = CategoryConcert.CategoryId.ToString(),
                CategoryName = CategoryConcert.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                Name = "Clash of the DJs",
                Price = 85,
                Artist = "DJ 'The Mike'",
                Date = eventDate.AddMonths(4),
                Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/dj.jpg",
                CategoryId = CategoryConcert.CategoryId.ToString(),
                CategoryName = CategoryConcert.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                Name = "Spanish guitar hits with Manuel",
                Price = 25,
                Artist = "Manuel Santinonisi",
                Date = eventDate.AddMonths(4),
                Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/guitar.jpg",
                CategoryId = CategoryConcert.CategoryId.ToString(),
                CategoryName = CategoryConcert.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
                Name = "Techorama 2021",
                Price = 400,
                Artist = "Many",
                Date = eventDate.AddMonths(10),
                Description = "The best tech conference in the world",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/conf.jpg",
                CategoryId = CategoryConferences.CategoryId.ToString(),
                CategoryName = CategoryConferences.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                Name = "To the Moon and Back",
                Price = 135,
                Artist = "Nick Sailor",
                Date = eventDate.AddMonths(8),
                Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
                CategoryId = CategoryMusicals.CategoryId.ToString(),
                CategoryName = CategoryMusicals.Name,
                VenueName = Venue1.Name,
                VenueCity = Venue1.City,
                VenueCountry = Venue1.Country
            });
        }
    }
}
