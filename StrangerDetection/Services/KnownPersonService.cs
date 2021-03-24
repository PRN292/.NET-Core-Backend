using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StrangerDetection.Validators;

namespace StrangerDetection.Services
{
    public interface IKnownPersonService
    {
        public List<TblKnownPerson> GetAllKnowPerson();

        public TblKnownPerson GetKnownPersonByEmail(string email);

        public bool UpdateKnownPerson(TblKnownPerson person);

        public bool CreateKnownPerson(TblKnownPerson person);

        public bool DeleteKnownPerson(string enail);
    }
    public class KnownPersonService : IKnownPersonService
    {
        private readonly StrangerDetectionContext context;

        public KnownPersonService(StrangerDetectionContext context)
        {
            this.context = context;
        }
        public bool CreateKnownPerson(TblKnownPerson person)
        {
            //NOTE: moved validation to KnowPersonsController
            //NOTE: add validate duplicate knowperson email
            bool result = IsEmailExisted(person);
            if (result)
            {
                this.context.Add<TblKnownPerson>(person);
                int row = context.SaveChanges();
                if (row > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsEmailExisted(TblKnownPerson person)
        {
            List<TblKnownPerson> knowPersonList = GetAllKnowPerson();
            foreach (TblKnownPerson existedPerson in knowPersonList)
            {
                if (person.Email.Equals(existedPerson.Email))
                {
                    return false;
                }
            }
            return true;
        }

        public bool DeleteKnownPerson(string email)
        {
            TblKnownPerson exsitedPerson = context.TblKnownPeople.FirstOrDefault(p => p.Email.Equals(email));
            if (exsitedPerson != null)
            {
                context.Remove(exsitedPerson);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TblKnownPerson> GetAllKnowPerson()
        {
            return context.TblKnownPeople.Include(p => p.TblEncodings).ToList();
        }

        public TblKnownPerson GetKnownPersonByEmail(string email)
        {
            return context.TblKnownPeople.AsQueryable()
                .Where(p => p.Email.Equals(email))
                .Include(p => p.TblEncodings)
                .FirstOrDefault();
        }

        public bool UpdateKnownPerson(TblKnownPerson person)
        {
            //NOTE: moved validation to KnowsPer
            TblKnownPerson exsitedPerson = context.TblKnownPeople.FirstOrDefault(p => p.Email.Equals(person.Email));

            exsitedPerson.Address = person.Address;
            exsitedPerson.Email = person.Email;
            exsitedPerson.Name = person.Name;
            exsitedPerson.PhoneNumber = person.PhoneNumber;
            int row = context.SaveChanges();
            if (row > 0)
            {
                return true;
            }
            return false;
        }
    }
}
