using CatalogApi.Models.Rating;

namespace CatalogApi.Services.IServices;

public interface IRatingService
{
    void SetRating(SetRatingOnFilm request);
    double GetRatingByFilm(int filmId);
}