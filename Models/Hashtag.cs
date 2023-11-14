using HikersDiary_Web.Model;

namespace Hikers_Diary.Models
{
    public class Hashtag
    {
        public int Tag_Id { get; set; } 
        public int Tpost_Id { get; set; }
        public string? Tag_Name { get; set; }
        public Post Post { get; set; }
    }
}
