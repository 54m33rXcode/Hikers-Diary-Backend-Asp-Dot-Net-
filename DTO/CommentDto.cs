namespace Hikers_Diary.DTO
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int CommentorId{ get; set; }
        public int? ReplyId { get; set; }
        public string? Content { get; set; }
        
    }
}
