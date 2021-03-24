using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using StrangerDetection.Repositories.Encoding;
using StrangerDetection.Repositories.impl;
using StrangerDetection.Repositories.KnownPerson;

namespace StrangerDetection.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public AccountsRepository AccountsRepository { get; }
        public KnownPersonsRepository KnownPersonsRepository { get; }

        public EncodingRepository EncodingRepository { get; }

        Task<int> Save();

        IDbContextTransaction GetTransaction();
    }
}