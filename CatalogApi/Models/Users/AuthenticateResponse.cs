using System;
using CatalogApi.Entities;
using Newtonsoft.Json;

namespace CatalogApi.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get;}
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDay { get; }
        public string JwtToken { get; }
        
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }


        public AuthenticateResponse(User user,string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            BirthDay = user.BirthDay;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
