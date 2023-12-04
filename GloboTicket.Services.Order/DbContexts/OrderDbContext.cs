using GloboTicket.Services.Ordering.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Services.Ordering.DbContexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        
    }
}
