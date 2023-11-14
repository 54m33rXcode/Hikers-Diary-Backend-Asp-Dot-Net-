using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Models;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace Hikers_Diary.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly MyDbContext _context;
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostRepository(MyDbContext context, IHashtagRepository hashtagRepository, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hashtagRepository = hashtagRepository;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public int AddPost(PostDto post)
        {
            var newPost = new Post
            {
                Poster_Id = post.PosterId,
                Post_Content = post.PostContent,
                Date_Posted = DateTime.Now,
                Map = SaveMap(post.Map),
            };

            _context.Post.Add(newPost);
            _context.SaveChanges();

            post.PostId = newPost.Post_Id; 

            _hashtagRepository.ExtractAndInsertHashtags(post);
            return post.PostId;
        }

        public void DeletePost(int postId)
        {
         var post = _context.Post.FirstOrDefault(b=>b.Post_Id == postId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }
            _context.Post.Remove(post);
            _context.SaveChanges();        
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var posts = _context.Post
                 .AsEnumerable()
                .Select(p => new PostDto
                {
                    PostId = p.Post_Id,
                    PosterId = p.Poster_Id,
                    PostContent = p.Post_Content,
                    DatePosted = p.Date_Posted,
                    MapUrl = GenerateMapUrl(p.Map)
                }).ToList();
            return posts;
        }

        public PostDto getByPostId(int postId)
        {
            var posts=_context.Post.FirstOrDefault(s => s.Post_Id == postId);

          /*  if (posts == null)
                return null;*/

            return new PostDto
            {
                PostId = posts.Post_Id,
                PosterId = posts.Poster_Id,
                PostContent = posts.Post_Content ??  "",
                DatePosted= posts.Date_Posted,
                MapUrl = GenerateMapUrl(posts.Map)
            };
        }

        public IEnumerable<PostDto> GetPostsByPosterId(int posterId)
        {
            var post= _context.Set<Post>()
                 .AsEnumerable()
                .Where(p => p.Poster_Id == posterId)
                .Select(b=> new PostDto
                {
                    PostId=b.Post_Id,
                    PosterId= b.Poster_Id,
                    PostContent= b.Post_Content,
                    DatePosted = b.Date_Posted, 
                    MapUrl =GenerateMapUrl (b.Map) 
                }).ToList();
            return post;    

        }
        public IEnumerable<PostDto> SearchPostByKeyword(string keyword)
        {
            var post= _context.Post.Where(p=>p.Post_Content.Contains(keyword))
                .Select(p => new PostDto
                {
                    PostId = p.Post_Id,
                    PosterId = p.Poster_Id,
                    PostContent = p.Post_Content,
                    DatePosted = p.Date_Posted,
                    Map = p.Map
                })
                .ToList();
            return post;
        }
        public IEnumerable<PostDto> GetPostByCategoryName(string CategoryName)
        {
            var posts = _context.Post
                .Where(b => _context.Category.Any(s => s.Category_Name == CategoryName && s.Catpost_Id == b.Post_Id))
                 .ToList()
                 .Select(p => new PostDto
                 {
                     PostId = p.Post_Id,
                     PosterId = p.Poster_Id,
                     PostContent = p.Post_Content,
                     DatePosted = p.Date_Posted,    
                     MapUrl =GenerateMapUrl(p.Map)
                 })
            .ToList();
            return posts;
        }
        public IEnumerable<PostDto> GetPostByFollowing(int followerId)
        {
            var posts = _context.Post   
               .Where(s => _context.Follow.Any(b => b.Follower_Id == followerId && b.Following_Id == s.Poster_Id))
                .ToList()
                 .Select(p => new PostDto
                 {
                     PostId = p.Post_Id,
                     PosterId = p.Poster_Id,
                     PostContent = p.Post_Content,
                     DatePosted= p.Date_Posted, 
                     MapUrl = GenerateMapUrl(p.Map)
                 })
            .ToList();

            return posts;
        }

        public IEnumerable<PostDto> GetPostByHashTag(string hashTag)
        {
            var posts = _context.Post
                .Where(s=> _context.Hashtag.Any(b=> b.Tag_Name == hashTag && b.Tpost_Id == s.Post_Id))
                 .ToList()
                .Select(p=> new PostDto
                {
                    PostId = p.Post_Id,
                    PosterId = p.Poster_Id,
                    PostContent = p.Post_Content,
                    DatePosted = p.Date_Posted,
                    MapUrl = GenerateMapUrl(p.Map) 
                })
                .ToList();
            return posts;
        }
      

        public void UpdatePost(PostDto post)
        {
            var existingPost = _context.Post.FirstOrDefault(b => b.Post_Id == post.PostId);
            if (existingPost != null)
            {
                existingPost.Post_Content = post.PostContent;
                existingPost.Date_Posted = post.DatePosted;
                existingPost.Map = SaveMap(post.Map);
                existingPost.Date_Posted = DateTime.Now;    
            }
            _context.Post.Update(existingPost); 
            _context.SaveChanges();
        }

        private string SaveMap(string base64Data)
        {
            if (!string.IsNullOrEmpty(base64Data))
            {
                string extension = ".gpx";

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Maps");
                string destinationPath = Path.Combine(folderPath, uniqueFileName);

                byte[] fileBytes = Convert.FromBase64String(base64Data);

                using (var stream = new FileStream(destinationPath, FileMode.Create))
                {
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }

                return $"/Maps/{uniqueFileName}";
            }

            return null;
        }

           public string GenerateMapUrl(string mapFileName)
          {
            if (!string.IsNullOrEmpty(mapFileName))
            {
              
                string[] parts = mapFileName.Split('/');
                if (parts.Length > 1)
                {
                    return parts[parts.Length - 1];
                }
            }

            return null;
        }                 
    }
}
