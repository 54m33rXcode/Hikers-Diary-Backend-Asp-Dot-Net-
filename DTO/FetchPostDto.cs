namespace Hikers_Diary.DTO
{
    public class FetchPostDto
    {
        public UserDto? User { get; set; }
        public PostDto? Post { get; set; }
        public IEnumerable<PhotoDto>? Photos { get; set; }
        public IEnumerable<CategoryDto>? Categories { get; set; }
    }
}
