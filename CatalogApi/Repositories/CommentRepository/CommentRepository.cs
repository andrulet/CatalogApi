using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories.CommentRepository;

public class CommentRepository : ICommentRepository
{
    private readonly CatalogContext _context;
    private readonly DbSet<Comment> _table;

    public CommentRepository(CatalogContext catalogContext)
    {
        _context = catalogContext;
        _table = _context.Set<Comment>();
    }
    public IEnumerable<Comment> GetAll()
    {
        return _table.ToList();
    }

    public Comment GetById(int id)
    {
        return _table.Find(id);
    }

    public void Insert(Comment obj)
    {
        _table.Add(obj);
    }

    public void Update(Comment obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Comment existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}