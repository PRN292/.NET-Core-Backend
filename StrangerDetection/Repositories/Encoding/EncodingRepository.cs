using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Repositories.Encoding
{
    public class EncodingRepository : GenericRepository<TblEncoding>, IEncodingRepository
    {
        private readonly StrangerDetectionContext _strangerDetectionContext;

        public EncodingRepository(StrangerDetectionContext context)
        {
            _strangerDetectionContext = context;
        }
    }
}
