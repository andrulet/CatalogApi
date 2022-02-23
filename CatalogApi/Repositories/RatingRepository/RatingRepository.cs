using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories.RatingRepository;

public class RatingRepository : IRatingRepository
{
    private readonly CatalogContext _context;
    private readonly DbSet<Rating> _table;

    public RatingRepository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<Rating>();
    }
    public IEnumerable<Rating> GetAll()
    {
        return _table.ToList();
    }

    public Rating GetById(int id)
    {
        return _table.Find(id);
    }

    public void Insert(Rating obj)
    {
        _table.Add(obj);
    }

    public void Update(Rating obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Rating existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}