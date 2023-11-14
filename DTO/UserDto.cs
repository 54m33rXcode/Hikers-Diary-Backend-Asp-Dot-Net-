using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hikers_Diary.DTO
{
    public class UserDto
    {
      public int UserId { get; set; }
      public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Profile_Pic { get; set; }
        public IFormFile? Profile_Pic_File { get; set; }

    }
}
