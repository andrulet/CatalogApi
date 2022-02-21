using System;

namespace CatalogApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}