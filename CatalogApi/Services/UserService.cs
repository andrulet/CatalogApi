using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;

namespace CatalogApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest, string ipAdress);

        AuthenticateResponse Register(RegisterRequest registerModelUser, string ipAdress);

        User GetById(int id);

        IEnumerable<User> GetAll();

        void RevokeToken(string token, string ipAdress);
    }

    public class UserService : IUserService
    {
        private IJwtUtils _jwtUtils;
        private CatalogContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
         

        public UserService(CatalogContext context, IConfiguration configuration, IMapper mapper, IJwtUtils jwtUtils)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest authenticateModel, string ipAdress)
        {
            if (string.IsNullOrEmpty(authenticateModel.Email) || string.IsNullOrEmpty(authenticateModel.Password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == authenticateModel.Email);
            
            if (user == null || !VerifyPasswordHash(authenticateModel.Password, user.PasswordHash, user.PasswordSalt))
                return null;
            
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAdress);
            user.RefreshTokens.Add(refreshToken);
            
            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);
            
            _context.Update(user);
            _context.SaveChanges();
            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse Register(RegisterRequest model,string ipAdress)
        {            
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("Username '" + model.Email + "' is already taken");

            var user = _mapper.Map<User>(model);
            CreatePasswordHash(model.Password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAdress);
            user.RefreshTokens.Add(refreshToken);
            RemoveOldRefreshTokens(user);
            _context.Users.Add(user);
            _context.SaveChanges();
            
            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }
        
        public void RevokeToken(string token, string ipAddress)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _context.Update(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }
        
        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        // helper methods
        private User GetUserByRefreshToken(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new AppException("Invalid token");

            return user;
        }
        
        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
        
        private void RemoveOldRefreshTokens(User user)
        {
            var z = Convert.ToInt32(_configuration.GetSection("AppSettings:RefreshTokenTTL").Value);
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x => 
                !x.IsActive && 
                x.Created.AddDays(z) <= DateTime.UtcNow);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
