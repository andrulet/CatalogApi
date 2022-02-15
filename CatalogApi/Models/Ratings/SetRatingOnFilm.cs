using CatalogApi.Helpers;

namespace CatalogApi.Models.Rating
{
    public class SetRatingOnFilm
    {
        public int UserId { get; set; }
        public int FilmId { get; set; }
        public int ValueRating { get; set; }
    }
}