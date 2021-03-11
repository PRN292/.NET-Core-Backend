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
        AuthenticationResponse Authenticate(AuthenticationRequest model);
        public TblAccount GetByUsername(string username);
        public List<TblAccount> GetAll();
        public bool LogoutUser(string username);

        public bool CreateAccount(CreateAccountRequest model);

        public List<GetAllAccountResponse> GetAllFullnameAndImage();
        object GetAnAccount(string username);

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

        //authenticate method
        public AuthenticationResponse Authenticate(AuthenticationRequest model)
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

        //Create Account
        public bool CreateAccount(CreateAccountRequest model)
        {
            if (ValidationRequestObj(model))
            {
                TblAccount account = new TblAccount
                {
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.FullName,
                    IdentificationCardFrontImageName = model.FrontIdentityImage,
                    IdentificationCardBackImageName = model.BackIdentityImage,
                    ProfileImageName = model.Image,
                    RoleId = model.RoleID,
                    KnownPersonId = model.KnowPersonId
                };
                context.SaveChanges();
                return true;
            }
            return false;
        }

        //Get all accounts
        public List<TblAccount> GetAll()
        {
            return context.TblAccounts.ToList();
        }

        //Get fullname and images of all accounts
        public List<GetAllAccountResponse> GetAllFullnameAndImage()
        {
            List<GetAllAccountResponse> resultList = null;
            List<TblAccount> accountList = GetAll();
            foreach (TblAccount account in accountList)
            {
                if (resultList == null)
                {
                    resultList = new List<GetAllAccountResponse>();
                }
                string fullname = account.Name;
                string image = account.ProfileImageName;
                GetAllAccountResponse resObj = new GetAllAccountResponse(fullname, image);
                resultList.Add(resObj);
            }
            return resultList;
        }

        //Get an account
        public object GetAnAccount(string username)
        {
            return null;
        }

        //Get account by username
        public TblAccount GetByUsername(string username)
        {
            return context.TblAccounts.Where(x => x.Username.Equals(username)).FirstOrDefault();
        }

        //logout
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

        //generate jwt token
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

        //validation
        private bool ValidationRequestObj(CreateAccountRequest model)
        {
            if (model.Username.Trim().Length == 0 || model.Password.Trim().Length == 0 ||
                model.FrontIdentityImage.Trim().Length == 0 ||
                model.BackIdentityImage.Trim().Length == 0 ||
                model.Image.Trim().Length == 0 ||
                model.KnowPersonId.Trim().Length == 0) //except roleID (int)
            {
                return false;
            }
            return true;
        }


        //public bool UpdateAccount(AccountRequest model)
        //{
        //    var account = this.GetByUsername(model.Username);
        //    if (account != null)
        //    {
        //        account.Address = model.Address;
        //        account.IdentificationCardBackImageName = model
        //    }
        //}
    }
}
