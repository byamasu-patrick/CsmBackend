using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewDto, ProductReview>().ReverseMap();
        }
    }
}
