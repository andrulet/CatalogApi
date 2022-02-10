using System;

namespace CatalogApi.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        
        public int Userid { get; set; }
        
        public int FilmId { get; set; }
        
        public int ValueRating { get; set; }
    }
}