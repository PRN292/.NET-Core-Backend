using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Services
{

    public interface IEncdodingService
    {
        public bool DeleteEncoding(string ID);
        public bool CreateEncoding(string user_email, string image);

    }
    public class EncodingService: IEncdodingService
    {
        private readonly StrangerDetectionContext context;

        public EncodingService(StrangerDetectionContext context)
        {
            this.context = context;
        }

        public bool CreateEncoding(string user_email, string image)
        {
            string uuid = System.Guid.NewGuid().ToString();
            //TODO: call Python GRPC server and create encdoing
            //and get image_name and save it to database
            //Hello Sonw
            TblEncoding encoding = new TblEncoding
            {
                Id = uuid,
                ImageName = image,
                //KnownPersonEmail = user_email
            };
            context.Add<TblEncoding>(encoding);
            context.SaveChanges();
            return true;
        }

        public bool DeleteEncoding(string ID)
        {
            TblEncoding encoding = context.TblEncodings.FirstOrDefault(e => e.Id.Equals(ID));
            if (encoding != null)
            {
                //TODO: call Python GRPC server and delete encoding
                context.Remove(encoding);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
