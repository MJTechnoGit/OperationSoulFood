using AutoMapper;
using OperationSoulFood.Services.CouponAPI.Models;
using OperationSoulFood.Services.CouponAPI.Models.Dto;

namespace OperationSoulFood.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });

            return mappingConfig;
        
        }



    
    }
}
