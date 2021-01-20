using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Helpers;
using Praca_dyplomowa.Context;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Praca_dyplomowa.Models;
using Newtonsoft.Json;

namespace Praca_dyplomowa.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User AuthenticateGoogle(string email, string googleId, string googleToken);
        // bool RevokeToken(string token, string ipAddress);
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private ProgramContext _context;
        
        public UserService(ProgramContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public User AuthenticateGoogle(string email, string googleId, string googleToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(googleToken))
                return null;

            GoogleResponseModel googleResponse = CheckGoogleToken(googleToken);

            if (googleResponse.Status == false)
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserGoogleId.Equals(googleResponse.Sub));
            if (user == null)
            {
                var userWithSameEmail = _context.Users.Where(x => x.Email.Equals(email));
                if (userWithSameEmail == null)
                {
                    try
                    {
                        User newUser = new User
                        {
                            Email = googleResponse.Email,
                            FirstName = googleResponse.Given_name,
                            LastName = googleResponse.Family_name,
                            UserGoogleId = googleResponse.Sub
                        };
                        _context.Users.Add(newUser);
                        _context.SaveChanges();
                        return newUser;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            else
                return user;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Wymagane jest wprowadzenie hasła");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Nazwa uzytkownika " + user.UserName + " jest już zajęta.");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.UserId);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.UserName) && userParam.UserName != user.UserName)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException("UserName " + userParam.UserName + " is already taken");

                user.UserName = userParam.UserName;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Tworznie hash SHA512 dla nowego użytkownika
        /// </summary>
        /// <param name="password">Password inserted during registering in browser</param>
        /// <param name="passwordHash">Generated Hash [out]</param>
        /// <param name="passwordSalt">Generated Salt [out]</param>
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
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
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
    
        private static GoogleResponseModel CheckGoogleToken(string googleToken)
        {
            HttpClient httpClient = new HttpClient();
            var URL = "https://oauth2.googleapis.com/tokeninfo?id_token=" + googleToken;

            try
            {
                Task<string> result = httpClient.GetStringAsync(URL);
                string checkResult = result.Result; 
                GoogleResponseModel googleResponseModel = JsonConvert.DeserializeObject<GoogleResponseModel>(checkResult);
                googleResponseModel.Status = true;
                httpClient.Dispose();
                return googleResponseModel;
            }
            catch (Exception)
            {
                GoogleResponseModel googleResponseModel = new GoogleResponseModel { Status = false };
                httpClient.Dispose();
                return googleResponseModel;
            }
        }
    }
}
