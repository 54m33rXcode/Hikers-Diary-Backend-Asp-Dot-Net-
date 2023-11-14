using Hikers_Diary.DTO;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface ILikeRepository
    {
        LikeDto GetLikeById(int likeId);
        IEnumerable<LikeDto> GetLikeByPost(int postId);
        IEnumerable<LikeDto> GetLikeByUser(int likerId);
        public void AddLike(LikeDto like);
        public void DeleteLike(int postId, int userId);
        public bool isLiked(int postId, int userId);


    }
}
