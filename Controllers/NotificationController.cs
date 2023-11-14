using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Repository;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpPost]
        public IActionResult NotificationTest(int senderId, int receiverId, string notifyType)
        {
            _notificationRepository.AddNotification(senderId, receiverId, notifyType);
            return Ok("sending");
        }
        [HttpGet]
        public IActionResult SendMessage(int receiverId)
        {
           var msg = _notificationRepository.SendNotification(receiverId);
            return Ok(msg);
        }
    }
}

