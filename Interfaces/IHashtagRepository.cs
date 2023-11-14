using Hikers_Diary.DTO;

namespace Hikers_Diary.Interfaces
{
    public interface IHashtagRepository
    {
        public void ExtractAndInsertHashtags(PostDto post);
    }
}
