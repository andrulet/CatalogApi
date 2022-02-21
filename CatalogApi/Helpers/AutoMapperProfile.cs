using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Models.Collections;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Users;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;

namespace CatalogApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            CreateMap<RegisterRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<CreateModelFilm, Film>();
            CreateMap<EditModelFilm, Film>();
            CreateMap<CreateCommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<EditCommentRequest, Comment>();
            CreateMap<SetRatingOnFilm, Rating>();
            CreateMap<CollectionCreateRequest, Collection>();
            CreateMap<Collection, CollectionInfoResponse>();
        }
    }
}