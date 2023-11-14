using Hikers_Diary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.DTO
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int PosterId { get; set; }
        public string? PostContent { get; set; }
        public DateTime DatePosted { get; set; }
        public string? Map { get; set; }  
        public string? MapUrl { get; set; }
    }
}
