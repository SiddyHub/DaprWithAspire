using GloboTicket.Services.Discount.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GloboTicket.Services.Discount.DbContexts
{
    public class DiscountDbContext: DbContext
    {
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = Guid.NewGuid(),
                Code = "BeNice",
                Amount = 10,
               AlreadyUsed = false
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = Guid.NewGuid(),
                Code = "Awesome",
                Amount = 20,
                AlreadyUsed = false
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = Guid.NewGuid(),
                Code = "AlmostFree",
                Amount = 100,
                AlreadyUsed = false
            });

        }
    }
}
