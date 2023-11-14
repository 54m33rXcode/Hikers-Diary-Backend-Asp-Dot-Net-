using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpGet("{likeId}")]
        public IActionResult GetLike(int likeId)
        {
            var like = _likeRepository.GetLikeById(likeId);

            if(like == null)
            {
                return NotFound();
            }
            return Ok(like);
        }
        [HttpGet ("post/{postId}")]
            public IActionResult GetLikeByPost(int postId)
        {
            var like = _likeRepository.GetLikeByPost(postId);
            return Ok(like);
        }

        [HttpGet ("user/{userId}")]
        public IActionResult GetLikeByUser(int userId)
        {
            var like = _likeRepository.GetLikeByUser(userId);
            return Ok(like);
        }

        [HttpGet("{postId}/likedBy/{userId}")]
        public IActionResult isLiked(int postId,int userId)
        {
            var liked = _likeRepository.isLiked(postId, userId);
            return Ok(liked);
        }

        [HttpPost]
        public IActionResult addLike(LikeDto like)
        {
            _likeRepository.AddLike(like);
            return Ok();
        }
        [HttpDelete ("{postId}/{userId}")]
        public IActionResult deleteLike(int postId,int userId)
        {
            _likeRepository.DeleteLike(postId,userId);
            return Ok();
        }

    }
}
