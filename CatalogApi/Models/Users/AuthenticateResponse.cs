using System;
using CatalogApi.Entities;

namespace CatalogApi.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get;}
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDay { get; }
        public string Token { get; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            BirthDay = user.BirthDay;
            Token = token;
        }
    }
}