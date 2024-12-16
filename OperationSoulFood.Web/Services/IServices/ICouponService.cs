using OperationSoulFood.Web.Models;

namespace OperationSoulFood.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponsAsync(CouponDto coupon);
        Task<ResponseDto?> UpdateCouponsAsync(CouponDto coupon);
        Task<ResponseDto?> DeleteCouponsAsync(int id);
    }
}
