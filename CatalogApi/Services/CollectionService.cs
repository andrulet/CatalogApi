using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models;
using CatalogApi.Models.Collections;
using CatalogApi.Models.Films;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services;

public interface ICollectionService
{
    void Create(CollectionCreateRequest request);
    void Delete(int collectionId);
    IEnumerable<CollectionInfoResponse> GetAll(int userId);
    void AddFilmIntoCollection(FilmAddIntoCollectionRequest response);
    CollectionInfoResponse EditCollection(int collectionId,CollectionEditRequest request);
    IEnumerable<FilmInfoResponse> GetCollectionInfo(int collectionId);
}

public class CollectionService : ICollectionService
{
    private readonly CatalogContext _context;
    private readonly IMapper _mapper;
    private readonly IRatingService _ratingService;
    
    public CollectionService(CatalogContext context, IMapper mapper, IRatingService ratingService)
    {
        _context = context;
        _mapper = mapper;
        _ratingService = ratingService;
    }


    public void Create(CollectionCreateRequest request)
    {
        if (request == null)
            throw new AppException(nameof(request) + "is null");
        var collection = _mapper.Map<Collection>(request);
        collection.User = _context.Users.Find(collection.UserId);
        _context.Collections.Add(collection);
        _context.SaveChanges();
    }

    public void Delete(int collectionId)
    {
        _context.Collections.Remove(GetById(collectionId));
        _context.SaveChanges();
    }

    public IEnumerable<CollectionInfoResponse> GetAll(int userId)
    {
        
        var collections = _context.Collections.Where(x => x.IsPrivate == false | (x.IsPrivate == true && x.UserId == userId))
            .Include(u=>u.User);
        return collections.Select(x => new CollectionInfoResponse(x));
    }

    public void AddFilmIntoCollection(FilmAddIntoCollectionRequest response)
    {
        var film = _context.Films.Find(response.FilmId);
        if (_context.Collections.Find(response.CollectionId) == null)
            throw new AppException($"Incorrect {nameof(response.CollectionId)}");
        if (_context.Collections.Include(f => f.Films).Any(x => x.Id == response.FilmId))
            throw new AppException($"This collection include this film {nameof(response.CollectionId)}");
        _context.Collections.Find(response.CollectionId)?.Films.Add(film);
        _context.SaveChanges();
    }

    public CollectionInfoResponse EditCollection(int collectionId,CollectionEditRequest request)
    {
        Collection collection;
        if (request == null | (collection = _context.Collections.Include(c =>c.User).FirstOrDefault(x=>x.Id == collectionId)) == null)
            throw new AppException("Bad request");
        collection.Title = request.Title;
        collection.IsPrivate = request.IsPrivate;
        _context.SaveChanges();
        return new CollectionInfoResponse(collection);
    }

    public IEnumerable<FilmInfoResponse> GetCollectionInfo(int collectionId)
    {
        if (GetById(collectionId) == null)
            throw new AppException("Incorrect Id = " + collectionId);
        _context.Collections.Include(c =>c.Films).Load();
        var films = GetById(collectionId).Films;
        return films.Select(x => new FilmInfoResponse(x, _ratingService.GetRatingByFilm(x.Id)));
    }

    private Collection GetById(int id)
    {
        var collection = _context.Collections.Find(id);
        if (collection == null)
            throw new AppException("Incorrect collection id");
        return collection;
    }
}