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
using CatalogApi.Models;
using CatalogApi.Repositories;

namespace CatalogApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);

        AuthenticateResponse Register(RegisterRequest registerModelUser);

        User GetById(int id);

        IEnumerable<User> GetAll();

    }

    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        private readonly IRepository<User> _userRepository;

        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest)
        {
            if (string.IsNullOrEmpty(authenticateRequest.Email) || string.IsNullOrEmpty(authenticateRequest.Password))
                return null;

            var user = _userRepository.GetAll().SingleOrDefault(x => x.Email == authenticateRequest.Email);
            
            if (user == null)
                return null;
            
            if (!VerifyPasswordHash(authenticateRequest.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public AuthenticateResponse Register(RegisterRequest request)
        {            
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new AppException("Password is required");

            if (_userRepository.GetAll().Any(x => x.Email == request.Email))
                throw new AppException("Username '" + request.Email + "' is already taken");

            CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            var token = GenerateJwtToken(user);
            
            _userRepository.Insert(user);
            _userRepository.Save();

            return new AuthenticateResponse(user, token);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }
        
        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        // helper methods        

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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