using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcImage;

namespace StrangerDetection.Services
{
    public class GRPCClient
    {
        string serverAddr = "http://localhost:50052";
        private ProcessImage.ProcessImageClient client;
        private GrpcChannel grpcChannel;

        public GRPCClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            grpcChannel = GrpcChannel.ForAddress(serverAddr);
            client = new ProcessImage.ProcessImageClient(grpcChannel);
        }

        public CreateEncodingReply CreateEncoding(string user_email, string image_b64_string)
        {
            CreateEncodingRequest request = new CreateEncodingRequest
            {
                Image = image_b64_string,
                UserEmail = user_email
            };
            CreateEncodingReply reply = client.CreateEncoding(request);
            return reply;
        }

        public int DeleteEncoding(string image_name)
        {
            DeleteEncodingRequest request = new DeleteEncodingRequest { ImageName = image_name };
            DeleteEncodingReply reply = client.DeleteEncoding(request);
            return reply.Count;
        }
    }
}
