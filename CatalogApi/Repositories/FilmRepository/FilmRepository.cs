using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories.FilmRepository;

public class FilmRepository : IFilmRepository
{
    private readonly CatalogContext _context;
    private readonly DbSet<Film> _table;

    public FilmRepository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<Film>();
    }
    public IEnumerable<Film> GetAll()
    {
        return _table.ToList();
    }

    public Film GetById(int id)
    {
        return _table.Find(id);
    }

    public void Insert(Film obj)
    {
        _table.Add(obj);
    }

    public void Update(Film obj)
    {
        Delete(obj.Id);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Film existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void LoadAllComments()
    {
        _context.Comments.Include(u => u.User).Load();
    }
}