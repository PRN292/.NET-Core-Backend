using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StrangerDetection.Helpers;
using StrangerDetection.Models;

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
    }
}