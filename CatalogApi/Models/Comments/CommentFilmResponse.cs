using System;

namespace CatalogApi.Models.Comments
{
    public class CommentFilmResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
    }
}