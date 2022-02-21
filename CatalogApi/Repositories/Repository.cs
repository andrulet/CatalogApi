using System.Collections.Generic;
using System.Linq;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly CatalogContext _context;
    private readonly DbSet<T> _table;

    public Repository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<T>();
    }
    public IEnumerable<T> GetAll()
    {
        return _table.AsNoTracking().ToList();
    }

    public T GetById(object id)
    {
        return _table.Find(id);
    }

    public void Insert(T obj)
    {
        _table.Add(obj);
    }

    public void Update(T obj)
    {
        //_table.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        T existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}