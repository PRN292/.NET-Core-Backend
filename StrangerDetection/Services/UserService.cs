using StrangerDetection.Models.Responses;
using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrangerDetection.Helpers;
using Microsoft.Extensions.Options;
using StrangerDetection.Models;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace StrangerDetection.Services
{
    public interface IUserService
    {
        AuthenticationResponse Authenticate(AuthenticattionRequest model);
        public TblAccount GetByUsername(string username);
        public List<TblAccount> GetAll();
        bool LogoutUser(string username);
    }
    public class UserService : IUserService
    {
        private readonly AppSetting appSetting;
        private readonly StrangerDetectionContext context;

        public UserService(IOptions<AppSetting> appSetting, StrangerDetectionContext context)  
        {
            this.appSetting = appSetting.Value;
            this.context = context;
        }
        public AuthenticationResponse Authenticate(AuthenticattionRequest model)
        {
            var user = context.TblAccounts.Where(account =>
            account.Username.Equals(model.Username) && account.Password.Equals(model.Password))
                .FirstOrDefault();
            if (user == null) return null;
            var token = generateJwtToken(user);
            user.IsLogin = true;
            context.SaveChanges();
            return new AuthenticationResponse(user, token);
            
        }

        private string generateJwtToken(TblAccount account)
        {
            var toenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", account.Username) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = toenHandler.CreateToken(tokenDescriptor);
            return toenHandler.WriteToken(token);
        }

        public TblAccount GetByUsername(string username)
        {
            return context.TblAccounts.Where(x => x.Username.Equals(username)).FirstOrDefault();
        }

        public List<TblAccount> GetAll()
        {
            return context.TblAccounts.ToList();
        }

        public bool LogoutUser(string username)
        {
            var account = this.GetByUsername(username);
            if (account != null)
            {
                account.IsLogin = false;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateAccount(AccountRequest model)
        {
            var account = this.GetByUsername(model.Username);
            if (account != null)
            {
                account.Address = model.Address;
                account.IdentificationCardBackImageName = model
            }
        }
    }
}
