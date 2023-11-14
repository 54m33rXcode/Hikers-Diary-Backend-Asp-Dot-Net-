using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Repository;
using HikersDiary_Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombinedPostController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;


        public CombinedPostController(IUserRepository userRepository, IPostRepository postRepository, IPhotoRepository photoRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _photoRepository = photoRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        [HttpPut("UpdatePost/{postId}")]
        public IActionResult UpdatePost(int postId, [FromBody] CreatePostDto UpdataData)
        {
            if (UpdataData.Post != null && UpdataData.Post.PostId == postId)
            {
                _postRepository.UpdatePost(UpdataData.Post);
            }


            if (UpdataData.Photos != null)
            {
                 _photoRepository.UpdatePhoto(UpdataData.Photos,postId);        
            }

            if (UpdataData.Categories != null)
            {
                _categoryRepository.UpdateCategories(UpdataData.Categories, postId);
            }


            return Ok();
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserPost(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var posts = _postRepository.GetPostsByPosterId(userId);
            if (posts != null)
            {
                var fetchPostDtos = new List<FetchPostDto>();

                foreach (var post in posts)
                {
                    var photos = _photoRepository.GetPhotosByPost(post.PostId);
                    var categories = _categoryRepository.GetCategoriesByPost(post.PostId);

                    var fetchPostDto = new FetchPostDto
                    {
                        User = user,
                        Post = post,
                        Photos = photos,
                        Categories = categories
                    };

                    fetchPostDtos.Add(fetchPostDto);
                }

                return Ok(fetchPostDtos);
            }


            return BadRequest();

        }

        [HttpGet]
        public IActionResult GetAllCombinedPost()
        {
            var posts = _postRepository.GetAllPosts();
            if (posts != null)
            {
                var fetchPostDtos = new List<FetchPostDto>();
                foreach (var post in posts)
                {
                    var user = _userRepository.GetUserById(post.PosterId);
                    var photos = _photoRepository.GetPhotosByPost(post.PostId);
                    var categories = _categoryRepository.GetCategoriesByPost(post.PostId);
                    var fetchPostDto = new FetchPostDto
                    {
                        User = user,
                        Post = post,
                        Photos = photos,
                        Categories = categories
                    };

                    fetchPostDtos.Add(fetchPostDto);
                }
                return Ok(fetchPostDtos);
            }
            return BadRequest();
        }

        [HttpGet("category/{categoryName}")]
        public IActionResult GetByCategoryName(string categoryName)
        {
            var posts = _postRepository.GetPostByCategoryName(categoryName);
            if (posts != null)
            {
                var fetchPostDtos = new List<FetchPostDto>();
                foreach (var post in posts)
                {
                    var user = _userRepository.GetUserById(post.PosterId);
                    var photos = _photoRepository.GetPhotosByPost(post.PostId);
                    var categories = _categoryRepository.GetCategoriesByPost(post.PostId);
                    var fetchPostDto = new FetchPostDto
                    {
                        User = user,
                        Post = post,
                        Photos = photos,
                        Categories = categories
                    };

                    fetchPostDtos.Add(fetchPostDto);
                }
                return Ok(fetchPostDtos);
            }
            return BadRequest();

        }
        [HttpGet("following/{userId}")]
        public IActionResult GetPostByFollowing(int userId)
        {
            var posts = _postRepository.GetPostByFollowing(userId);
            if (posts != null)
            {
                var fetchPostDtos = new List<FetchPostDto>();
                foreach (var post in posts)
                {
                    var user = _userRepository.GetUserById(post.PosterId);
                    var photos = _photoRepository.GetPhotosByPost(post.PostId);
                    var categories = _categoryRepository.GetCategoriesByPost(post.PostId);
                    var fetchPostDto = new FetchPostDto
                    {
                        User = user,
                        Post = post,
                        Photos = photos,
                        Categories = categories
                    };

                    fetchPostDtos.Add(fetchPostDto);
                }
                return Ok(fetchPostDtos);
            }
            return BadRequest();

        }

        [HttpGet("hashtag/{tagName}")]
        public IActionResult GetByHashTagName(string tagName)
        {
            var posts = _postRepository.GetPostByHashTag(tagName);
            if (posts != null)
            {
                var fetchPostDtos = new List<FetchPostDto>();
                foreach (var post in posts)
                {
                    var user = _userRepository.GetUserById(post.PosterId);
                    var photos = _photoRepository.GetPhotosByPost(post.PostId);
                    var categories = _categoryRepository.GetCategoriesByPost(post.PostId);
                    var fetchPostDto = new FetchPostDto
                    {
                        User = user,
                        Post = post,
                        Photos = photos,
                        Categories = categories
                    };

                    fetchPostDtos.Add(fetchPostDto);
                }
                return Ok(fetchPostDtos);
            }
            return BadRequest();

        }

        [HttpGet("comments/{postId}")]
        public IActionResult GetPostComments(int postId)
        {
            List<CommentDto> comments = _commentRepository.GetCommentByPost(postId).ToList();

            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }

            List<UserNCommentDto> userNComments = new List<UserNCommentDto>();

            foreach (var comment in comments)
            {
                UserDto commentor = _userRepository.GetUserById(comment.CommentorId);
                UserNCommentDto userNComment = new UserNCommentDto
                {
                    User = commentor,
                    Comment = comment
                };

                userNComments.Add(userNComment);
            }

            return Ok(userNComments);
        }

        [HttpGet("reply/{parentId}")]
        public IActionResult GetRepliedComments(int parentId)
        {
            List<CommentDto> comments = _commentRepository.GetReplyByParent(parentId).ToList();

            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }

            List<UserNCommentDto> userNComments = new List<UserNCommentDto>();

            foreach (var comment in comments)
            {
                UserDto commentor = _userRepository.GetUserById(comment.CommentorId);
                UserNCommentDto userNComment = new UserNCommentDto
                {
                    User = commentor,
                    Comment = comment
                };

                userNComments.Add(userNComment);
            }

            return Ok(userNComments);
        }
        [HttpDelete ("{postId}")]
        public IActionResult CombinedPostDelete(int postId)
        {
             _postRepository.DeletePost(postId);
            _photoRepository.DeletePhoto(postId);
            _categoryRepository.DeleteCategories(postId);
            return Ok();
        }
    }
}
