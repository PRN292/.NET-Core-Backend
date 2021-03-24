using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StrangerDetection.Helpers;
using StrangerDetection.Models;
using StrangerDetection.Models.Requests;

namespace StrangerDetection.Repositories.impl
{
    public class AccountsRepository : GenericRepository<TblAccount>, IAccountsRepository
    {
        private readonly StrangerDetectionContext _strangerDetectionContext;

        public AccountsRepository(StrangerDetectionContext context)
        {
            _strangerDetectionContext = context;
        }

        public Task<TblAccount> GetUserByUsername(string username)
        {
            return GetById(username);
        }

        public TblAccount Authenticate(AuthenticationRequest reqObj)
        {
           return _strangerDetectionContext.TblAccounts.AsQueryable().Where(account =>
            account.Username.Equals(reqObj.Username) && account.Password.Equals(reqObj.Password))
                .FirstOrDefault();
        }

        public TblAccount GetAccountByUsername(string username)
        {
            return _strangerDetectionContext.TblAccounts.AsQueryable().Where(x => x.Username.Equals(username)).FirstOrDefault();
        }


    }
}