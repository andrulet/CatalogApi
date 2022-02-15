namespace CatalogApi.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int ValueRating { get; set; }
        
        public int FilmId { get; set; }
        public Film Film { get; set; }
        
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}