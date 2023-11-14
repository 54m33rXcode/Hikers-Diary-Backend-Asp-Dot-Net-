using Hikers_Diary.DTO;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<PostDto> GetAllPosts();
        PostDto getByPostId(int postId);
        IEnumerable<PostDto> SearchPostByKeyword(string keyword);
        IEnumerable<PostDto> GetPostsByPosterId(int posterId);
        IEnumerable<PostDto> GetPostByFollowing(int followerId);
        IEnumerable<PostDto>GetPostByCategoryName(string categoryName);
        IEnumerable<PostDto> GetPostByHashTag(string hashTag);
        
        public int AddPost(PostDto post); 
        public void UpdatePost(PostDto post);  
        public void DeletePost(int postId); 

    }
}
