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
using StrangerDetection.Validators;

namespace StrangerDetection.Services
{
    public interface IUserService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest requestObj);
        GetAccountResponse SearchAccountByUsername(string username);
        TblAccount GetByUsername(string username);
        List<TblAccount> GetAll();
        bool LogoutUser(string username);

        bool CreateAccount(CreateAccountRequest requestObj);

<<<<<<< HEAD
        List<GetAccountsResponse> GetAllFullnameAndImage();

        bool UpdateAccount(UpdateAccountRequest requestObj);

        bool DeleteAccount(string username);
=======
        public List<AccountResponse> GetAllFullnameAndImage();
        object GetAnAccount(string username);
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a

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

        //authenticate method
        public AuthenticationResponse Authenticate(AuthenticationRequest reqObj)
        {
            var user = context.TblAccounts.Where(account =>
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
<<<<<<< HEAD
                    Username = reqObj.Username,
                    Password = reqObj.Password,
                    Address = reqObj.Address,
                    Name = reqObj.FullName,
                    IdentificationCardFrontImageName = reqObj.FrontIdentityImage,
                    IdentificationCardBackImageName = reqObj.BackIdentityImage,
                    ProfileImageName = reqObj.Image,
                    RoleId = reqObj.RoleID
=======
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.FullName,
                    IdentificationCardFrontImageName = model.FrontIdentityImage,
                    IdentificationCardBackImageName = model.BackIdentityImage,
                    ProfileImageName = model.Image,
                    RoleId = model.RoleID
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a
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
            var query = (from x in context.TblAccounts where x.Username == obj.Username select x).First();
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

        //Get all accounts
        public List<TblAccount> GetAll()
        {
            return context.TblAccounts.ToList();
        }

        //Get fullname and images of all accounts
<<<<<<< HEAD
        public List<GetAccountsResponse> GetAllFullnameAndImage()
        {
            List<GetAccountsResponse> result = null;
            List<TblAccount> query = (from x in context.TblAccounts select x).ToList();
            if (query != null)
=======
        public List<AccountResponse> GetAllFullnameAndImage()
        {
            List<AccountResponse> resultList = null;
            List<TblAccount> accountList = GetAll();
            foreach (TblAccount account in accountList)
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a
            {
                foreach (TblAccount account in query)
                {
<<<<<<< HEAD
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
=======
                    resultList = new List<AccountResponse>();
                }
                string fullname = account.Name;
                string image = account.ProfileImageName;
                AccountResponse resObj = new AccountResponse(fullname, image);
                resultList.Add(resObj);
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a
            }
            return null;
            //List<GetAccountsResponse> resultList = null;
            //List<TblAccount> accountList = GetAll();
            //foreach (TblAccount account in accountList)
            //{
            //    if (resultList == null)
            //    {
            //        resultList = new List<GetAccountsResponse>();
            //    }
            //    string fullname = account.Name;
            //    string image = account.ProfileImageName;
            //    GetAccountsResponse resObj = new GetAccountsResponse(fullname, image);
            //    resultList.Add(resObj);
            //}
            //return resultList;
        }


        //Get account by username
        public TblAccount GetByUsername(string username)
        {
            TblAccount account = context.TblAccounts.Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (account != null)
            {
                return account;
            }
            return null;
        }

        public GetAccountResponse SearchAccountByUsername(string username)
        {
            var query = (from x in context.TblAccounts where x.Username == username select x).First();
            if (query != null)
            {
                GetAccountResponse resObj = new GetAccountResponse(query.Username, query.ProfileImageName, query.RoleId, query.Password,
                query.Name, query.Address, query.IdentificationCardFrontImageName, query.IdentificationCardBackImageName);
                return resObj;
            }
            return null;
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

        //Delete account
        public bool DeleteAccount(string username)
        {
<<<<<<< HEAD
            var query = (from x in context.TblAccounts where x.Username == username select x).First();
            if (query != null)
=======
            if (model.Username.Trim().Length == 0 || model.Password.Trim().Length == 0 ||
                model.FrontIdentityImage.Trim().Length == 0 ||
                model.BackIdentityImage.Trim().Length == 0 ||
                model.Image.Trim().Length == 0
                ) //except roleID (int)
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a
            {
                context.TblAccounts.Remove(query);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
