using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyDbContext _context;
        private readonly INotificationRepository _notificationRepository;
        public CommentRepository(MyDbContext context, INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public void AddComment(CommentDto comment)
        {
            var addcomment = MapDtoToEntity(comment);
            _context.Comment.Add(addcomment);
            _context.SaveChanges();


            int receiverId = _context.Post.Find(comment.PostId).Poster_Id;

            _notificationRepository.AddNotification(comment.CommentorId, receiverId, "comment");
        }

        public void AddReply(int commentId,CommentDto reply)
        {
            var parentCmt = _context.Comment.FirstOrDefault(b => b.C_Id == reply.CommentId);
            if (parentCmt != null) {
                var rply = new Comment
                {
                    Cpost_Id = reply.PostId,
                    Commentor_Id = reply.CommentorId,
                    Comment_Content = reply.Content
                };
             rply.Parent_Cid = commentId;
            _context.Comment.Add(rply);
            _context.SaveChanges();
            }
           
        }

        public void DeleteComment(int id)
        {
            var comment = _context.Comment.FirstOrDefault(b=>b.C_Id == id);
            if (comment == null)
            {
                return;
            }

            _context.Comment.Remove(comment);   
            _context.SaveChanges();
        }

        public CommentDto GetComment(int id)
        {
            var comment = _context.Comment.FirstOrDefault(f => f.C_Id == id);
            return MapEntityToDto(comment);
        }

        public IEnumerable<CommentDto> GetCommentByPost(int PostId)
        {
           var comment = _context.Comment.Where(f=>f.Cpost_Id == PostId);
            return comment.Select(MapEntityToDto);
        }

        public IEnumerable<CommentDto> GetCommentByUser(int commentorId)
        {
            var comment = _context.Comment.Where(f=>f.Commentor_Id == commentorId);
            return comment.Select(MapEntityToDto);
        }

        public IEnumerable<CommentDto> GetReplyByParent(int ParentId)
        {
            var comment = _context.Comment.Where(f=>f.Parent_Cid == ParentId);  
            return comment.Select(MapEntityToDto);  
        }

        public void UpdateComment(CommentDto comment)
        {
          var existing_comment = _context.Comment.FirstOrDefault(b=>b.C_Id == comment.CommentId);
            if (existing_comment != null)
            {
                existing_comment.Comment_Content = comment.Content;
            }
            _context.Comment.Update(existing_comment);
            _context.SaveChanges();
        }

       /* public void UpdateReply(CommentDto reply)
        {
            var existingReply = _context.Comment.FirstOrDefault(b => b.Parent_Cid == reply.ReplyId);
            if (existingReply != null)
            {
                existingReply.Comment_Content= reply.Content;
            }
            _context.Comment.Update(existingReply);
            _context.SaveChanges();
        }*/

        private CommentDto MapEntityToDto(Comment entity)
        {
            if (entity == null)
                return null;
            return new CommentDto
            {
                CommentId = entity.C_Id,
                PostId = entity.Cpost_Id,
                CommentorId = entity.Commentor_Id,
                Content = entity.Comment_Content,
                ReplyId = entity.Parent_Cid
            };
        }

        private Comment MapDtoToEntity(CommentDto dto)
        {
            return new Comment
            {
                C_Id = dto.CommentId,
                Cpost_Id = dto.PostId,
                Commentor_Id = dto.CommentorId,
                Comment_Content = dto.Content,
                Parent_Cid = dto.ReplyId
            };
        }
    }
}
