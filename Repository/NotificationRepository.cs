using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.SignalR;

namespace Hikers_Diary.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MyDbContext _context;
        
        public NotificationRepository(MyDbContext context)
        {
            _context = context;
 
        }

        public void AddNotification(int senderId, int receiverId, string notifyType)
        {
            string Noti_Msg;

            String Sender = _context.User.Find(senderId).UserName;

            if(notifyType == "like")
            {
                 Noti_Msg = $"{Sender} liked your post";
            }

            else if(notifyType == "comment")
            {
                Noti_Msg = $"{Sender} commented on your post";
            }
            else
            {
                Noti_Msg = $"{Sender} followed you";
            }

            var notification = new Notification
            {
                Sender_Id = senderId,
                Receiver_Id = receiverId,
                Notify_Type = notifyType,
                Notification_Message = Noti_Msg,
            };

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }
        public IEnumerable<string> SendNotification(int receiverId)
        {
            var message = _context.Notification.Where(s=>s.Receiver_Id == receiverId)
                .Select(m=>m.Notification_Message).ToList();
            return message;
        }

    }
}
