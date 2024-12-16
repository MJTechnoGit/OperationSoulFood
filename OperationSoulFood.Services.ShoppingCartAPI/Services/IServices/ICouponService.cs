using OperationSoulFood.Services.ShoppingCartAPI.Models.Dto;

namespace OperationSoulFood.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
