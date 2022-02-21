using System.Collections.Generic;

namespace CatalogApi.Repositories;

public interface IRepository<T> where T: class
{
    IEnumerable<T> GetAll();
    T GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
    void Save();
}