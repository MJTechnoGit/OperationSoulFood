using OperationSoulFood.Services.ShoppingCartAPI.Services.IServices;
using OperationSoulFood.Services.ShoppingCartAPI.Models.Dto;
using Newtonsoft.Json;

namespace OperationSoulFood.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        { 
            _httpClientFactory = clientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }

            return new CouponDto();
        }
    }

}

