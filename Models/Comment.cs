namespace HikersDiary_Web.Model
{
    public class Comment
    {
        public int C_Id { get; set; }
        public int Cpost_Id { get; set; }
        public int Commentor_Id { get; set; }
        public string? Comment_Content { get; set; }
        public int? Parent_Cid { get; set; }

        public Post Post { get; set; }
        public User Commentor { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment>? ChildComments { get; set; }
    }
}
