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
using PushNotification.Web.Models;
using StrangerDetection.Validators;
using StrangerDetection.UnitOfWorks;

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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IOptions<AppSetting> appSetting, StrangerDetectionContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var user = _unitOfWork.AccountsRepository.Authenticate(reqObj);
            if (user == null) return null;
            var token = generateJwtToken(user);
            user.IsLogin = true;
            _unitOfWork.Save().Wait();
            return new AuthenticationResponse(user, token);

        }

        //Create Account
        public bool CreateAccount(CreateAccountRequest reqObj)
        {
            if (UserValidator.ValidateRequestObjForCreate(reqObj) && !IsUsernameExisted(reqObj.Username))
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
               // _unitOfWork.AccountsRepository.Create(account);
                context.TblAccounts.Add(account);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        private bool IsUsernameExisted(string username)
        {
            TblAccount account = GetByUsername(username);
            if (account != null)
            {
                return true;
            }
            return false;
        }

        //update account
        public bool UpdateAccount(UpdateAccountRequest obj)
        {
            if (UserValidator.ValidateRequestObjForUpdate(obj))
            {
                TblAccount account = context.TblAccounts.AsQueryable().Where(x => x.Username.Equals(obj.Username)).FirstOrDefault();
                if (account != null)
                {
                    account.ProfileImageName = obj.Image;
                    account.Password = obj.Password;
                    account.Name = obj.Fullname;
                    account.Address = obj.Address;
                    account.IdentificationCardFrontImageName = obj.FrontIdentity;
                    account.IdentificationCardBackImageName = obj.BackIdentity;
                    context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public List<GetAccountsResponse> GetAllFullnameAndImage()
        {
            List<GetAccountsResponse> result = null;
            List<TblAccount> accountList = context.TblAccounts.AsQueryable().ToList();
            if (accountList != null)
            {
                foreach (TblAccount account in accountList)
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
            TblAccount account = context.TblAccounts.AsQueryable().Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (account != null)
            {
                GetAccountResponse resObj = new GetAccountResponse(account.Username, account.ProfileImageName, account.RoleId, account.Password,
                account.Name, account.Address, account.IdentificationCardFrontImageName, account.IdentificationCardBackImageName);
                return resObj;
            }
            return null;
        }

        //Delete account
        public bool DeleteAccount(string username)
        {
            TblAccount account = context.TblAccounts.AsQueryable().Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (account != null)
            {
                context.TblAccounts.Remove(account);
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
            TblAccount account = context.TblAccounts.AsQueryable().Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (account != null)
            {
                account.IsLogin = false;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void RegisterPushFCM(User user)
        {
            TblAccount acc = new TblAccount
            {
                Name = user.UserName,
                IdentificationCardBackImageName = user.Token
            };
            context.TblAccounts.Add(acc);
            context.SaveChanges();
        }
    }
}