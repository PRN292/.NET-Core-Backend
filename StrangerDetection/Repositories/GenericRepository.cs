using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StrangerDetection.Helpers;
using StrangerDetection.Models;

namespace StrangerDetection.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StrangerDetectionContext _dbContext;

        public GenericRepository()
        {
        }

        public GenericRepository(StrangerDetectionContext dbContext,
            IOptions<AppSetting> appSettings)
        {
            _dbContext = dbContext;
            AppSetting = appSettings.Value;
        }

        public AppSetting AppSetting { get; }


        public async Task<T> GetById(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Create(T obj)
        {
            _dbContext.Set<T>().Add(obj);
        }

        public async Task<List<T>> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public void Remove(object id)
        {
            var entity = GetById(id).Result;
            _dbContext.Set<T>().Remove(entity);
        }

        public T Update(T obj)
        {
            return _dbContext.Set<T>().Update(obj).Entity;
        }
    }
}