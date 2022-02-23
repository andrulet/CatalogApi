using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories.UserRepository;

public class UserRepository: IUserRepository
{
    private readonly CatalogContext _context;
    private readonly DbSet<User> _table;

    public UserRepository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<User>();
    }
    public IEnumerable<User> GetAll()
    {
        return _table.ToList();
    }

    public User GetById(int id)
    {
        return _table.Find(id);
    }

    public void Insert(User obj)
    {
        _table.Add(obj);
    }

    public void Update(User obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        User existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}