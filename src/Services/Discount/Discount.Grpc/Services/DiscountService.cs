using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbcontext,ILogger<DiscountService> logger):DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)
                coupon = new Models.Coupon
                {
                    ProductName = "No Discount",
                    Description = "No Discount Desc",
                    Amount = 0
                };
            logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
           
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon=request.Coupon.Adapt<Models.Coupon>();
            if(coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));
            dbcontext.Coupons.Add(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if(coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));
            dbcontext.Coupons.Update(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon=await dbcontext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if(coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));
            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();
            logger.Adapt<ILogger>().LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);
            var couponModel=coupon.Adapt<CouponModel>();    
            return new DeleteDiscountResponse
            {
                Success = true
            };
        }
    }
}
