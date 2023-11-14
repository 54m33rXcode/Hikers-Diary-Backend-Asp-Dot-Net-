using Hikers_Diary.Models;

namespace HikersDiary_Web.Model
{
    public class Post
    {

        public int Post_Id { get; set; }
        public int Poster_Id { get; set; }
        public string? Post_Content { get; set; }
        public DateTime Date_Posted { get; set; }           
        public string? Map { get; set; }

        public User? Poster { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Photo>? Photoes { get; set; }
        public ICollection<Category>? Categories { get; set; }   
        public ICollection<Hashtag>? Hashtags { get; set; }
    }   

}
