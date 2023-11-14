using Hikers_Diary.Interfaces;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowRepository _followRepository;

        public FollowController(IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }

        [HttpGet]
        [Route("user/{userId}/followers")]
        public IActionResult GetFollower(int userId) {
            var followers = _followRepository.getFollower(userId);
            return followers == null ? NotFound() : Ok(followers);
        }

        [HttpGet]
        [Route("user/{userId}/followings")]
        public IActionResult GetFollowing(int userId)
        {
            var followings = _followRepository.getFollowing(userId);
            return followings == null ? NotFound() : Ok(followings);    
        }

        [HttpGet]
        [Route("user/{followerId}/isfollower/{followingId}")]
        public IActionResult IsFollower(int followerId, int followingId)
        {
            var isFollower = _followRepository.IsFollower(followerId, followingId);
            return Ok(isFollower);
        }

        [HttpPost]
        [Route("user/{followerId}/follow/{followingId}")]
        public IActionResult Follow(int followId,int Following_Id)
        {
            _followRepository.FollowUser(followId, Following_Id);
            return Ok();
        }

        [HttpDelete]
        [Route("user/{followerId}/unfollow/{followingId}")]
        public IActionResult Unfollow(int followId,int followingId) {
            _followRepository.UnfollowUser(followId, followingId);
            return Ok();
        }
      
    }
}
