using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Users;
using CatalogApi.Models.Films;

namespace CatalogApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            CreateMap<RegisterModelUser, User>();
            CreateMap<User, UserResponse>();
            CreateMap<CreateModelFilm, Film>();
            CreateMap<EditModelFilm, Film>();
            CreateMap<CreateCommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<EditCommentRequest, Comment>();
            CreateMap<Comment, CommentFilmResponse>();
        }
    }
}