using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface INotificationRepository
    {
        void AddNotification(int senderId, int receiverId, string notifyType);
        IEnumerable<string> SendNotification(int receiverId);
    }
}
