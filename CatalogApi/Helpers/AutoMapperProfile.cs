using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Models.Users;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            CreateMap<RegisterModel, User>();
            CreateMap<User, UserModel>();
        }
    }
}