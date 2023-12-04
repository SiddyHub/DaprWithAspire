using AutoMapper;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using GloboTicket.Grpc;
using GloboTicket.Services.Discount.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.Discount.Services
{
    public class AppCallbackService : AppCallback.AppCallbackBase
    {
        private readonly IMapper mapper;
        private readonly ICouponRepository couponRepository;

        public AppCallbackService(IMapper mapper, ICouponRepository couponRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        }

        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            switch (request.Method)
            {
                case "GetCouponById":
                    {
                        //var dataString = request.Data.Value.ToStringUtf8();
                        var input = request.Data.Unpack<GetCouponByIdRequest>();
                        var coupon = await couponRepository.GetCouponById(Guid.Parse(input.CouponId));
                        var couponResponse = new Coupon();
                        couponResponse.Code = coupon.Code;
                        couponResponse.AlreadyUsed = coupon.AlreadyUsed;
                        couponResponse.Amount = coupon.Amount;
                        couponResponse.CouponId = coupon.CouponId.ToString();                        
                        response.Data = Any.Pack(couponResponse);
                    }
                    break;
                case "GetCouponByCode":
                    {                        
                        var input = request.Data.Unpack<GetCouponByIdRequest>();
                        var coupon = await couponRepository.GetCouponByCode(input.CouponId);
                        var couponResponse = new Coupon();
                        couponResponse.Code = coupon.Code;
                        couponResponse.AlreadyUsed = coupon.AlreadyUsed;
                        couponResponse.Amount = coupon.Amount;
                        couponResponse.CouponId = coupon.CouponId.ToString();                        
                        response.Data = Any.Pack(couponResponse);
                    }
                    break;
                case "UseCoupon":
                    {                        
                        var input = request.Data.Unpack<GetCouponByIdRequest>();
                        await couponRepository.UseCoupon(Guid.Parse(input.CouponId));                        
                    }
                    break;
                default:
                    Console.WriteLine("Method not supported");
                    break;
            }
            return response;
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListTopicSubscriptionsResponse());
        }

        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return base.ListInputBindings(request, context);
        }
    }
}
