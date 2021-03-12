using System.Threading.Tasks;
using StrangerDetection.Models;

namespace StrangerDetection.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StrangerDetectionContext _dbContext;

        public GenericRepository(StrangerDetectionContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<T> GetById(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Create(T obj)
        {
            _dbContext.Set<T>().Add(obj);
        }
        
    }
}