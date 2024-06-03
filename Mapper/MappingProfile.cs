using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using POS.Model;
using POS.Model.DTO;
using POS.Model.ViewModels;

namespace POS.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<ProductDto, Product>();

            CreateMap<CategoryDto, CategoryViewModel>()
                .ReverseMap()
                .ForMember( dest => dest.Id, opt => opt.Ignore()); //? Ignore Id during reverse mapping from ViewModel to DTO
        }
    }
}