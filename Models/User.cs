using System.ComponentModel.DataAnnotations;

namespace HikersDiary_Web.Model
{
    public class User
    {
        
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Profile_Pic { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Followings { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> SentNotifications { get; set; }
        public ICollection<Notification> ReceivedNotifications { get; set; }
    }
}
