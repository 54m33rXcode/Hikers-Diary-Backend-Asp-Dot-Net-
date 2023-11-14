namespace Hikers_Diary.DTO
{
    public class CreatePostDto
    {
        public PostDto? Post { get; set; }
        public IEnumerable<PhotoDto>? Photos { get; set; }
        public IEnumerable<CategoryDto>? Categories { get; set; }
    }
}
