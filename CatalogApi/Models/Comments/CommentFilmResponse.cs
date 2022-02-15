using System;
using CatalogApi.Entities;

namespace CatalogApi.Models.Comments
{
    public class CommentFilmResponse
    {
        public int CommentId { get;}
        public string Content { get;}
        public DateTime DateCreate { get;}
        public string FirstName { get;}

        public CommentFilmResponse(Comment comment)
        {
            CommentId = comment.Id;
            FirstName = comment.User.FirstName;
            Content = comment.Content;
            DateCreate = comment.DateCreate;
        }
    }
}