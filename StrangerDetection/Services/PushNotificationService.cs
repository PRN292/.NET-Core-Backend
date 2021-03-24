using CorePush.Google;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using StrangerDetection.Services;

namespace PushNotification.Web
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly UserService userService;
        private readonly string _serverKey;

        public PushNotificationService(IConfiguration configuration, UserService userService)
        {
            _serverKey = configuration.GetSection("FCM:ServerKey").Value;
            this.userService = userService;
        }

        Task IPushNotificationService.RegisterForPush(string username, string token)
        {
            throw new System.NotImplementedException();
        }

        Task IPushNotificationService.SendNotification(PushNotificationItem notification)
        {
            throw new System.NotImplementedException();
        }

        //public async Task RegisterForPush(string username, string token)
        //{
        //    userService.RegisterPushFCM(new Models.User { UserName = username, Token = token });

        //    if (token != null)
        //    {
        //        await SendNotification(new PushNotificationItem()
        //        {
        //            Title = "Notification Test",
        //            Body = "Successfuly registered for push notifications"
        //        });
        //    }
        //}

        //public async Task SendNotification(PushNotificationItem notification)
        //{
        //    var users = userService.GetAll();

        //    foreach (var token in users)
        //    {
        //        using (var fcm = new FcmSender(null, null))
        //        {
        //            await fcm.SendAsync(token.IdentificationCardBackImageName,//token
        //                 new
        //                 {
        //                     notification = new
        //                     {
        //                         title = notification.Title,
        //                         body = notification.Body
        //                     },
        //                 });
        //        }
        //    }
        //}
    }
}
