using StrangerDetection.Models;

namespace StrangerDetection.Repositories.KnownPerson
{
    public class KnownPersonsRepository : GenericRepository<TblKnownPerson>, IKnownPersonsRepository
    {
        private readonly StrangerDetectionContext _strangerDetectionContext;

        public KnownPersonsRepository(StrangerDetectionContext context)
        {
            _strangerDetectionContext = context;
        }
    }
}