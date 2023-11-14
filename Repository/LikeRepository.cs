using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MyDbContext _context;
        private readonly INotificationRepository _notificationRepository;
        public LikeRepository(MyDbContext context, INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public void AddLike(LikeDto likedto)
        {
            var like = MapDtoToEntity(likedto);
            _context.Like.Add(like);
            _context.SaveChanges();

            int receiverId = _context.Post.Find(likedto.PostId).Poster_Id;

            _notificationRepository.AddNotification(likedto.LikerId,receiverId,"like");
        }

        public void DeleteLike(int postId, int userId)
        {
            var like = _context.Like.FirstOrDefault(b=>b.Lpost_Id == postId && b.Liker_Id == userId);
            if (like != null)
            {
                _context.Like.Remove(like);
                _context.SaveChanges();
            }
        }

        public LikeDto GetLikeById(int likeId)
        {
          var like= _context.Like.FirstOrDefault(l => l.Like_Id == likeId);
            return MapEntityToDto(like);
        }

        public IEnumerable<LikeDto> GetLikeByPost(int postId)
        {
            var like = _context.Like.Where(f=>f.Lpost_Id == postId);
            return like.Select(MapEntityToDto);  
        }

        public IEnumerable<LikeDto> GetLikeByUser(int likerId)
        {
            var like= _context.Like.Where(b=>b.Liker_Id == likerId);
            return like.Select(MapEntityToDto);
        }

        public bool isLiked(int postId,int userId)
        {
            return _context.Like.Any(p=>p.Lpost_Id == postId && p.Liker_Id==userId);
        }

        private LikeDto MapEntityToDto(Like entity)
        {
            if (entity == null)
                return null;

            return new LikeDto
            {
                LikeId = entity.Like_Id,
                PostId = entity.Lpost_Id,
                LikerId = entity.Liker_Id,
            };
        }

        private Like MapDtoToEntity(LikeDto dto)
        {
            return new Like
            {
                Like_Id = dto.LikeId,
                Liker_Id = dto.LikerId,
                Lpost_Id = dto.PostId
            };
        }
    }
}
