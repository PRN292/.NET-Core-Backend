using System.Threading.Tasks;

namespace StrangerDetection.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(object id);
        void Create(T obj);
    }
}