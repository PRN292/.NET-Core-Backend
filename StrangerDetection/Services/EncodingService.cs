using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcImage;
using StrangerDetection.Validators;

namespace StrangerDetection.Services
{

    public interface IEncodingService
    {
        public bool DeleteEncoding(string ID);
        public bool CreateEncoding(string user_email, string image);

    }
    public class EncodingService: IEncodingService
    {
        private readonly StrangerDetectionContext context;

        private readonly GRPCClient gRPCClient;

        public EncodingService(StrangerDetectionContext context, GRPCClient gRPCClient)
        {
            this.context = context;
            this.gRPCClient = gRPCClient;
        }

        public bool CreateEncoding(string user_email, string image)
        {
            if (EncodingValidator.ValidateCreateEncodingRequest(user_email, image))
            {
                string uuid = System.Guid.NewGuid().ToString();
                CreateEncodingReply grpcReply = gRPCClient.CreateEncoding(user_email, image);
                TblEncoding encoding = new TblEncoding
                {
                    Id = uuid,
                    ImageName = grpcReply.ImageName,
                    KnownPersonEmail = user_email
                };
                context.Add<TblEncoding>(encoding);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteEncoding(string ID)
        {
            TblEncoding encoding = context.TblEncodings.FirstOrDefault(e => e.Id.Equals(ID));
            if (encoding != null)
            {
                gRPCClient.DeleteEncoding(encoding.ImageName);
                context.Remove(encoding);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
