using Hikers_Diary.DTO;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface ICommentRepository
    {
        CommentDto GetComment(int id);
        IEnumerable<CommentDto> GetCommentByPost(int PostId);
        IEnumerable<CommentDto> GetCommentByUser(int commentorId);
        public void AddComment(CommentDto comment);
        public void UpdateComment(CommentDto comment);
        public void DeleteComment(int id);

        IEnumerable<CommentDto> GetReplyByParent(int ParentId);
        public void AddReply(int commentId,CommentDto reply);
        /*public void UpdateReply(CommentDto reply);*/
    

    }
}
