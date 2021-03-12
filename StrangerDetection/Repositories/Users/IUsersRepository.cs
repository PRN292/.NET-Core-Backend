using System.Threading.Tasks;
using StrangerDetection.Helpers;
using StrangerDetection.Models;

namespace StrangerDetection.Repositories.impl
{
    public interface IUsersRepository
    {
        public AppSetting AppSetting { get; }
        public Task<TblAccount> GetUserByUsername(string username);
        
    }
}