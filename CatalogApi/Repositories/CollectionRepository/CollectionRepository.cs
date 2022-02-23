using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories.CollectionRepository;

public class CollectionRepository : ICollectionRepository
{
    private readonly CatalogContext _context;
    private readonly DbSet<Collection> _table;

    public CollectionRepository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<Collection>();
    }
    public IEnumerable<Collection> GetAll()
    {
        return _table.ToList();
    }

    public Collection GetById(int id)
    {
        return _table.Find(id);
    }

    public void Insert(Collection obj)
    {
        _table.Add(obj);
    }

    public void Update(Collection obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Collection existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
    
    public void LoadAllUserInfoForCollections()
    {
        _context.Collections.Include(u=>u.User).Load();
    }
	
    public void LoadAllFilmInfoInCollections()
    {
        _context.Collections.Include(f => f.Films).Load();
    }
}