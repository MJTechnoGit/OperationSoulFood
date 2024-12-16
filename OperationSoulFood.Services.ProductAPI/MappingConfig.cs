using AutoMapper;
using OperationSoulFood.Services.ProductAPI.Models;
using OperationSoulFood.Services.ProductAPI.Models.Dto;

namespace OperationSoulFood.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();                             
            });

            return mappingConfig;
        }
    }
}
