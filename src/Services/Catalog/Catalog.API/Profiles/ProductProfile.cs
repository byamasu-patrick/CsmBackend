using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>().ReverseMap();
        }
    }
}
