using Hikers_Diary.DTO;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface IPhotoRepository
    {
            PhotoDto GetPhotoById(int photoId);
            IEnumerable<PhotoDto> GetPhotosByPost(int postId);
            void AddPhotos(IEnumerable<PhotoDto> photos, int postId);
            void UpdatePhoto(IEnumerable<PhotoDto> photos, int postId);
            void DeletePhoto(int postId);

    }
}
