using AutoMapper;
using OperationSoulFood.Services.ShoppingCartAPI.Models;
using OperationSoulFood.Services.ShoppingCartAPI.Models.Dto;

namespace OperationSoulFood.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
