using System;

namespace CatalogApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}