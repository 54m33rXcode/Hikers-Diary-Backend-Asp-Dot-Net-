using HikersDiary_Web.Model;

namespace Hikers_Diary.Models
{
    public class Category
    {
        public int Category_Id { get; set; }
        public int Catpost_Id { get; set; }
        public string? Category_Name { get; set; }
        public Post? Post { get; set; }
    }
}
