using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi_Poc.Helpers;
using WebApi_Poc.Model;

namespace WebApi_Poc.Services
{
    public interface IUserService
    {
        User GetTokenAndUserInfo(string username, string password);
        bool SaveUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private List<User> _users = new List<User>
           {
               new User{Id=1,FirstName="Rakesh",LastName="Jha",Username="poc",Password="poc" },
               new User{Id=1,FirstName="Manju",LastName="Mallikarjuna",Username="poc",Password="poc" }
           };

        public UserService(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }

        public User GetTokenAndUserInfo(string username, string password)
        {
            var user = _users.FirstOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public bool SaveUser(User userInfo)
        {
            return true;
        }
    }
}
