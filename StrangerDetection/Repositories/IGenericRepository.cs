using System.Collections.Generic;
using System.Threading.Tasks;
using StrangerDetection.Helpers;

namespace StrangerDetection.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(object id);
        void Create(T obj);
        void Remove(object id);
        T Update(T obj);
        Task<List<T>> GetAll();
    }
}