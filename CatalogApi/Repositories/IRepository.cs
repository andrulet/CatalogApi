using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CatalogApi.Repositories;

public interface IRepository<T> where T: class
{
    IEnumerable<T> GetAll();
    T GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
    void Save();
    void LoadAllComments();
    void LoadAllUserInfoForCollections();
    void LoadAllFilmInfoInCollections();
}