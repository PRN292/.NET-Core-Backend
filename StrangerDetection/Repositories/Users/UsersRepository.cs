using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StrangerDetection.Helpers;
using StrangerDetection.Models;

namespace StrangerDetection.Repositories.impl
{
    public class UsersRepository : IUsersRepository
    {
        private readonly StrangerDetectionContext _strangerDetectionContext;

        public UsersRepository(StrangerDetectionContext dbContext,
                                IOptions<AppSetting> appSetting)
        {
            _strangerDetectionContext = dbContext;
            AppSetting = appSetting.Value;
        }
        
        public AppSetting AppSetting { get; }

        public Task<TblAccount> GetUserByUsername(string username)
        {
            return _strangerDetectionContext.TblAccounts.FindAsync(username).AsTask();
        }
    }
}