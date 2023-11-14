using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MyDbContext _context;

        public PostController(IPostRepository postRepository,IPhotoRepository photoRepository,ICategoryRepository categoryRepository, IWebHostEnvironment hostingEnvironment,MyDbContext context )
        {
            _postRepository = postRepository;
            _photoRepository = photoRepository;
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllPost()
        {
            var posts = _postRepository.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet ("{postId}")]
        public IActionResult getPostById(int postId) {
            var post = _postRepository.getByPostId(postId);
            return Ok(post);
        }

        [HttpGet("byposter/{posterId}")]
        public IActionResult GetPostsByPosterId(int posterId)
        {
            var posts = _postRepository.GetPostsByPosterId(posterId);
            return Ok(posts);
        }

        [HttpGet("following/{followerId}")]
        public IActionResult GetPostsByFollowing(int followerId)
        {
            var posts = _postRepository.GetPostByFollowing(followerId);
            return Ok(posts);
        }
        [HttpGet("category/categoryName")]
        public IActionResult GetPostByCategoryName(string categoryName)
        {
            var posts = _postRepository.GetPostByCategoryName (categoryName);
            return Ok(posts);
        }

        [HttpGet("search/{search}")]
        public IActionResult searchPostByKeyword(string keyword)
        {
            var posts = _postRepository.SearchPostByKeyword(keyword);
            return Ok(posts);
        }

        [HttpPost("addPost")]
        public IActionResult addpost([FromBody]PostDto post) {
            _postRepository.AddPost(post);
            return CreatedAtAction(nameof(getPostById), new { postId = post.PostId},post);
        }

        [HttpPost("createPost")]   
        public IActionResult CreatePostWithPhotosAndCategories([FromBody] CreatePostDto postData)
        {
            Console.WriteLine("PostData: ", postData);

            if (postData.Post == null)
                    {
                        return BadRequest("Post data is missing.");
                    }

                    var post = postData.Post;
                    var postId = _postRepository.AddPost(post);

                    if (post.PostId == 0)
                    {
                        return BadRequest("Failed to add the post.");
                    }


            if (postData.Photos != null)
            {
                var photos = postData.Photos;
                _photoRepository.AddPhotos(photos, postId);
            }


            if (postData.Categories != null)
                    {
                        var categories = postData.Categories;
                        _categoryRepository.AddCategories(categories, postId);
                    }

                    return Ok();
        }


        [HttpPut("{postId}")]
        public IActionResult updatepost(int postId,[FromBody]PostDto post) {
            post.PostId = postId;
            _postRepository.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            _postRepository.DeletePost(postId);
            return NoContent();
        }


        [HttpGet("download-map")]
        public IActionResult DownloadMap(string uniqueFileName)
        {
            if (!string.IsNullOrEmpty(uniqueFileName))
            {
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Maps");
                string filePath = Path.Combine(folderPath, uniqueFileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/octet-stream", uniqueFileName);
                }
            }

            return NotFound();
        }
}
}
