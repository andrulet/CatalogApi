using CatalogApi.Entities;

namespace CatalogApi.Repositories.CollectionRepository;

public interface ICollectionRepository : IRepository<Collection>
{
    void LoadAllUserInfoForCollections();
    void LoadAllFilmInfoInCollections();
}