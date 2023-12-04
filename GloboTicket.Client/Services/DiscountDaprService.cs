using Dapr.Client;
using GloboTicket.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public class DiscountDaprService : IDiscountService
    {
        private readonly DaprClient daprClient;

        public DiscountDaprService(DaprClient daprClient)
        {
            this.daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<Coupon> GetCouponByCode(string code)
        {            
            if (code == string.Empty)
                return null;           

            var coupon = new Coupon();
            var data = new GloboTicket.Grpc.GetCouponByIdRequest { CouponId = code };
            var result = await daprClient.InvokeMethodGrpcAsync<GloboTicket.Grpc.GetCouponByIdRequest, GloboTicket.Grpc.Coupon>("discountgrpc", "GetCouponByCode", data);
            if (result != null)
            {
                coupon.AlreadyUsed = result.AlreadyUsed;
                coupon.Amount = result.Amount;
                coupon.Code = result.Code;
                coupon.CouponId = Guid.Parse(result.CouponId);
            }
            return coupon;
        }

        public async Task<Coupon> GetCouponById(Guid couponId)
        {            
            var coupon = new Coupon();
            var data = new GloboTicket.Grpc.GetCouponByIdRequest { CouponId = couponId.ToString() };
            var result = await daprClient.InvokeMethodGrpcAsync<GloboTicket.Grpc.GetCouponByIdRequest, GloboTicket.Grpc.Coupon>("discountgrpc", "GetCouponById", data);
            if (result != null)
            {
                coupon.AlreadyUsed = result.AlreadyUsed;
                coupon.Amount = result.Amount;
                coupon.Code = result.Code;
                coupon.CouponId = Guid.Parse(result.CouponId);
            }
            return coupon;
        }

        public async Task UseCoupon(Guid couponId)
        {            
            var data = new GloboTicket.Grpc.GetCouponByIdRequest { CouponId = couponId.ToString() };
            await daprClient.InvokeMethodGrpcAsync<GloboTicket.Grpc.GetCouponByIdRequest>("discountgrpc", "UseCoupon", data);
        }
    }
}
