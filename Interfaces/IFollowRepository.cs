using Hikers_Diary.DTO;
using HikersDiary_Web.Model;
using System.Collections;

namespace Hikers_Diary.Interfaces
{
    public interface IFollowRepository
    {
       
        ICollection <UserDto> getFollower(int userId);  
        ICollection<UserDto> getFollowing(int userId);
        bool IsFollower(int followerId, int followingId);
        void FollowUser(int followerId, int followingId);
        void UnfollowUser(int followerId,int followingId);  
    }
}
