using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        public FollowRepository(MyDbContext context,IHttpContextAccessor httpContextAccessor,INotificationRepository notificationRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
        }

      
        public ICollection<UserDto> getFollower(int userId)
        {
            var followers = _context.Follow
                .Where(f=>f.Following_Id == userId)
                .Select(f=>f.Follower)
                .ToList();

            var userDtos = followers.Select(user => new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Profile_Pic = GetProfilePic(user.Profile_Pic)
            }).ToList();

            return userDtos;
        }

        public ICollection<UserDto> getFollowing(int userId)
        {
            var following =  _context.Follow
                .Where(f => f.Follower_Id == userId)
                .Select(f => f.Following)
                .ToList();

            var userDtos = following.Select(user => new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Profile_Pic = GetProfilePic(user.Profile_Pic)
            }).ToList();

            return userDtos;
        }

        public void FollowUser(int followerId, int followingId)
        {

            if (!_context.Follow.Any(f=>f.Follower_Id==followerId && f.Following_Id == followingId))
            {
                var follow = new Follow
                {
                    Follower_Id = followerId,
                    Following_Id = followingId
                };

                _context.Follow.Add(follow);
                _context.SaveChanges();

                _notificationRepository.AddNotification(followerId, followingId, "follow");
            }
        }


        public void UnfollowUser(int followerId, int followingId)
        {
           var follow = _context.Follow.FirstOrDefault(f=>f.Follower_Id == followerId && f.Following_Id == followingId);

            if(follow !=  null)
            {
                _context.Follow.Remove(follow);
                _context.SaveChanges();
            }
        }

        public bool IsFollower(int followerId, int followingId)
        {
            return _context.Follow.Any(f => f.Follower_Id == followerId && f.Following_Id == followingId);
        }

        public string GetProfilePic(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                string baseUrl = request.Scheme + "://" + request.Host.Value;
                string imageUrl = baseUrl + fileName;

                return imageUrl;
            }

            return null;
        }
    }
}
