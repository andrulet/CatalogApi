using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Collections;
using CatalogApi.Models.Films;
using CatalogApi.Repositories.CollectionRepository;
using CatalogApi.Services.IServices;

namespace CatalogApi.Services;



public class CollectionService : ICollectionService
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IMapper _mapper;
    private readonly IRatingService _ratingService;
    private readonly IFilmService _filmService;
    private readonly IUserService _userService;
    
    public CollectionService(ICollectionRepository collectionRepository, IMapper mapper, IRatingService ratingService, IFilmService filmService, IUserService userService)
    {
        _collectionRepository = collectionRepository;
        _mapper = mapper;
        _filmService = filmService;
        _ratingService = ratingService;
        _userService = userService;
    }


    public void Create(CollectionCreateRequest request)
    {
        if (request == null)
            throw new AppException(nameof(request) + "is null");
        var collection = _mapper.Map<Collection>(request);
        collection.User = _userService.GetById(request.UserId);
        _collectionRepository.Insert(collection);
        _collectionRepository.Save();
    }

    public void Delete(int collectionId)
    {
        _collectionRepository.Delete(collectionId);
        _collectionRepository.Save();
    }

    public IEnumerable<CollectionInfoResponse> GetAll(int userId)
    {
        _collectionRepository.LoadAllUserInfoForCollections();
        var collections = _collectionRepository.GetAll().Where(x => x.IsPrivate == false | (x.IsPrivate == true && x.UserId == userId));
        return collections.Select(x => new CollectionInfoResponse(x));
    }

    public void AddFilmIntoCollection(FilmAddIntoCollectionRequest response)
    {
        var film = _filmService.GetById(response.FilmId);
        if (_collectionRepository.GetById(response.CollectionId) == null)
            throw new AppException($"Incorrect {nameof(response.CollectionId)}");
        _collectionRepository.LoadAllFilmInfoInCollections();
        if (_collectionRepository.GetAll().Any(x => x.Films.Any(z=>z.Id == response.FilmId)))
            throw new AppException($"This collection include this film {nameof(response.CollectionId)}");
        _collectionRepository.GetById(response.CollectionId)?.Films.Add(film);
        _collectionRepository.Save();
    }

    public CollectionInfoResponse EditCollection(int collectionId,CollectionEditRequest request)
    {
        Collection collection;
        
        if (request == null | (collection = _collectionRepository.GetAll().FirstOrDefault(x=>x.Id == collectionId)) == null)
            throw new AppException("Bad request");
        collection.Title = request.Title;
        collection.IsPrivate = request.IsPrivate;
        _collectionRepository.Update(collection);
        _collectionRepository.Save();
        _collectionRepository.LoadAllUserInfoForCollections();
        return new CollectionInfoResponse(collection);
    }

    public IEnumerable<FilmInfoResponse> GetCollectionInfo(int collectionId)
    {
        if (GetById(collectionId) == null)
            throw new AppException("Incorrect Id = " + collectionId);
        _collectionRepository.LoadAllFilmInfoInCollections();
        var films = GetById(collectionId).Films;
        return films.Select(x => new FilmInfoResponse(x, _ratingService.GetRatingByFilm(x.Id)));
    }

    private Collection GetById(int id)
    {
        var collection = _collectionRepository.GetById(id);
        if (collection == null)
            throw new AppException("Incorrect collection id");
        return collection;
    }
}