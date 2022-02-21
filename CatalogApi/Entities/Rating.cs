namespace CatalogApi.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int ValueRating { get; set; }
        
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}