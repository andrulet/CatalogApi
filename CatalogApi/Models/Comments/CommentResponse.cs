using System;

namespace CatalogApi.Models.Comments
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
    }
}