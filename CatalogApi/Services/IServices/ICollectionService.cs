using System.Collections.Generic;
using CatalogApi.Models.Collections;
using CatalogApi.Models.Films;

namespace CatalogApi.Services.IServices;

public interface ICollectionService
{
    void Create(CollectionCreateRequest request);
    void Delete(int collectionId);
    IEnumerable<CollectionInfoResponse> GetAll(int userId);
    void AddFilmIntoCollection(FilmAddIntoCollectionRequest response);
    CollectionInfoResponse EditCollection(int collectionId,CollectionEditRequest request);
    IEnumerable<FilmInfoResponse> GetCollectionInfo(int collectionId);
}