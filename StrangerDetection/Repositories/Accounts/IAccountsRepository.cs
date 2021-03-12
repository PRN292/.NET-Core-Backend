using System.Threading.Tasks;
using StrangerDetection.Helpers;
using StrangerDetection.Models;

namespace StrangerDetection.Repositories.impl
{
    public interface IAccountsRepository
    {
        public Task<TblAccount> GetUserByUsername(string username);
        
    }
}