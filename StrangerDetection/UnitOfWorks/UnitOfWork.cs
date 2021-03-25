using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using StrangerDetection.Helpers;
using StrangerDetection.Models;
using StrangerDetection.Repositories.Encoding;
using StrangerDetection.Repositories.impl;
using StrangerDetection.Repositories.KnownPerson;

namespace StrangerDetection.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly StrangerDetectionContext _strangerDetectionContext;
        private readonly IOptions<AppSetting> _appSettings;
        private AccountsRepository _accountsRepository;
        private KnownPersonsRepository _knownPersonsRepository;
        private EncodingRepository _encodingRepository;

        public UnitOfWork(StrangerDetectionContext context, IOptions<AppSetting> options,AccountsRepository accountsRepository,
            KnownPersonsRepository knownPersonsRepository, EncodingRepository encodingRepository)
        {
            _strangerDetectionContext = context;
            _appSettings = options;
                     _accountsRepository=accountsRepository;
         _knownPersonsRepository =knownPersonsRepository;
            _encodingRepository = encodingRepository;
    }

        public AccountsRepository AccountsRepository
        {
            get
            {
                if (_accountsRepository == null)
                {
                    _accountsRepository = new AccountsRepository(_strangerDetectionContext);
                }

                return _accountsRepository;
            }
        }

        public KnownPersonsRepository KnownPersonsRepository
        {
            get
            {
                if (_knownPersonsRepository == null)
                {
                    _knownPersonsRepository = new KnownPersonsRepository(_strangerDetectionContext);
                }

                return _knownPersonsRepository;
            }
        }

        public EncodingRepository EncodingRepository
        {
            get
            {
                if (_encodingRepository == null)
                {
                    _encodingRepository = new EncodingRepository(_strangerDetectionContext);
                }

                return _encodingRepository;
            }
        }

        public async Task<int> Save()
        {
            return await _strangerDetectionContext.SaveChangesAsync();
        }

        public IDbContextTransaction GetTransaction()
        {
            return _strangerDetectionContext.Database.BeginTransaction();
        }
    }
}