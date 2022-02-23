using CatalogApi.Entities;

namespace CatalogApi.Repositories.FilmRepository;

public interface IFilmRepository : IRepository<Film>
{
    void LoadAllComments();
}