using StrangerDetection.Models.Responses;
using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using StrangerDetection.Helpers;
using Microsoft.Extensions.Options;
using StrangerDetection.Models;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using StrangerDetection.Validators;

namespace StrangerDetection.Services
{
    public interface IUserService
    {
        //login
        AuthenticationResponse Authenticate(AuthenticationRequest requestObj);
        //search by name
        GetAccountResponse SearchAccountByUsername(string username);
        //Get tblAccount by name
        TblAccount GetByUsername(string username);
        //Get all accounts
        List<TblAccount> GetAll();
        //Log out
        bool LogoutUser(string username);
        //Create account
        bool CreateAccount(CreateAccountRequest requestObj);
        //Get fullname and image of all accounts
        List<GetAccountsResponse> GetAllFullnameAndImage();
        //Update account
        bool UpdateAccount(UpdateAccountRequest requestObj);
        //Delete Account
        bool DeleteAccount(string username);
    }
    public class UserService : IUserService
    {
        private readonly AppSetting appSetting;
        private readonly StrangerDetectionContext context;
        private readonly UserValidator validator;

        public UserService(IOptions<AppSetting> appSetting, StrangerDetectionContext context)
        {
            this.appSetting = appSetting.Value;
            this.context = context;
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

        //login
        public AuthenticationResponse Authenticate(AuthenticationRequest reqObj)
        {
            var user = context.TblAccounts.AsQueryable().Where(account =>
            account.Username.Equals(reqObj.Username) && account.Password.Equals(reqObj.Password))
                .FirstOrDefault();
            if (user == null) return null;
            var token = generateJwtToken(user);
            user.IsLogin = true;
            context.SaveChanges();
            return new AuthenticationResponse(user, token);

        }

        //Create Account
        public bool CreateAccount(CreateAccountRequest reqObj)
        {
            if (validator.ValidationRequestObjForCreateAccount(reqObj))
            {
                TblAccount account = new TblAccount
                {
                    Username = reqObj.Username,
                    Password = reqObj.Password,
                    Address = reqObj.Address,
                    Name = reqObj.FullName,
                    IdentificationCardFrontImageName = reqObj.FrontIdentityImage,
                    IdentificationCardBackImageName = reqObj.BackIdentityImage,
                    ProfileImageName = reqObj.Image,
                    RoleId = reqObj.RoleID
                };
                context.TblAccounts.Add(account);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        //update account
        public bool UpdateAccount(UpdateAccountRequest obj)
        {
            var query = (from x in context.TblAccounts.AsQueryable() where x.Username == obj.Username select x).First();
            if (query != null)
            {
                query.ProfileImageName = obj.Image;
                query.Password = obj.Password;
                query.Name = obj.Fullname;
                query.Address = obj.Address;
                query.IdentificationCardFrontImageName = obj.FrontIdentity;
                query.IdentificationCardBackImageName = obj.BackIdentity;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<GetAccountsResponse> GetAllFullnameAndImage()
        {
            List<GetAccountsResponse> result = null;
            List<TblAccount> query = (from x in context.TblAccounts.AsQueryable() select x).ToList();
            if (query != null)
            {
                foreach (TblAccount account in query)
                {
                    string fullname = account.Name;
                    string image = account.ProfileImageName;
                    GetAccountsResponse resObj = new GetAccountsResponse(fullname, image);
                    if (result == null)
                    {
                        result = new List<GetAccountsResponse>();
                    }
                    result.Add(resObj);
                }
                return result;
            }
            return null;
        }

        //Get account by username => return TblAccount
        public TblAccount GetByUsername(string username)
        {
            TblAccount account = context.TblAccounts.AsQueryable().Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (account != null)
            {
                return account;
            }
            return null;
        }
        //get account by username => return GetAccountResponse
        public GetAccountResponse SearchAccountByUsername(string username)
        {
            var query = (from x in context.TblAccounts.AsQueryable() where x.Username == username select x).First();
            if (query != null)
            {
                GetAccountResponse resObj = new GetAccountResponse(query.Username, query.ProfileImageName, query.RoleId, query.Password,
                query.Name, query.Address, query.IdentificationCardFrontImageName, query.IdentificationCardBackImageName);
                return resObj;
            }
            return null;
        }

        //Delete account
        public bool DeleteAccount(string username)
        {
            var query = (from x in context.TblAccounts.AsQueryable() where x.Username == username select x).First();
            if (query != null)
            {
                context.TblAccounts.Remove(query);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        //get all accounts
        public List<TblAccount> GetAll()
        {
            return context.TblAccounts.ToList();
        }

        //log out 
        public bool LogoutUser(string username)
        {
            var query = (from x in context.TblAccounts.AsQueryable() where x.Username == username && x.IsLogin == true select x).First();
            if (query != null)
            {
                query.IsLogin = false;
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}