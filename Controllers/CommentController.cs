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
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id)
        {
            var res = _commentRepository.GetComment(id);
            if (res == null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("post/{postId}")]
        public IActionResult GetCommentByPost(int postId) {
            var comment = _commentRepository.GetCommentByPost(postId);
            return Ok(comment);
        }
        [HttpGet("user/{userId}")]
        public IActionResult GetCommentByUser(int userId)
        {
            var comment = _commentRepository.GetCommentByUser(userId);
            return Ok(comment);
        }
        [HttpGet("reply/{replyId}")]
        public IActionResult GetReplyComment(int replyId)
        {
            var reply = _commentRepository.GetReplyByParent(replyId);
            return Ok(reply);
        }

        [HttpPost]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            _commentRepository.AddComment(comment);
            return Ok();
        }
        [HttpPost("{commentId}/reply")]
        public IActionResult AddReply(int commentId, [FromBody] CommentDto comment)
        {
            _commentRepository.AddReply(commentId, comment);
            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentDto comment)
        {
            if (id != comment.CommentId)
                return BadRequest();

            _commentRepository.UpdateComment(comment);
            return Ok();
        }
      /*  [HttpPut("reply/{id}")]
        public IActionResult UpdateReply(int id, [FromBody] CommentDto replyDto)
        {
            if (id != replyDto.CommentId)
                return BadRequest();

            _commentRepository.UpdateReply(replyDto);
            return Ok();
        }*/

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            _commentRepository.DeleteComment(id);
            return Ok();
        }

  

    }
}
