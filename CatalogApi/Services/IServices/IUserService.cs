using System.Collections.Generic;
using CatalogApi.Entities;
using CatalogApi.Models.Users;

namespace CatalogApi.Services.IServices;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);

    AuthenticateResponse Register(RegisterRequest registerModelUser);

    User GetById(int id);

    IEnumerable<User> GetAll();
}