using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PhotoRepository(MyDbContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public PhotoDto GetPhotoById(int photoId)
        {
            var res= _context.Photo.FirstOrDefault(p => p.Photo_Id == photoId);
            return MapEntityToDto(res);
        }

        public IEnumerable<PhotoDto> GetPhotosByPost(int postId)
        {
            var photo = _context.Photo.Where(p => p.Ppost_Id == postId).ToList();
            return photo.Select(MapEntityToDto);
        }

        public void AddPhotos(IEnumerable<PhotoDto> photos, int postId)
        {
            if (!_context.Post.Any(p => p.Post_Id == postId))
            {
                throw new ArgumentException($"Invalid postId: {postId}. Post does not exist.");
            }
            var photoEntities = photos.Select(dto => MapDtoToEntity(dto, postId));

            _context.Photo.AddRange(photoEntities);
            _context.SaveChanges();
        }

        public void UpdatePhoto(IEnumerable<PhotoDto> photos, int postId)
        {
            var existingPhotos = _context.Photo.Where(p => p.Ppost_Id == postId);
            foreach (var photo in existingPhotos)
            {
                if (!string.IsNullOrEmpty(photo.Photo_Url))
                {
                    var previousPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), photo.Photo_Url.TrimStart('/'));
                    if (File.Exists(previousPhotoPath))
                    {
                        File.Delete(previousPhotoPath);
                    }
                }
                _context.Photo.Remove(photo);
            }

            
            foreach (var photo in photos)
            {
                _context.Photo.Add(MapDtoToEntity(photo, postId));
            }

            _context.SaveChanges();
        }

        public void DeletePhoto(int postId)
        {
            var existingPhotos = _context.Photo.Where(p => p.Ppost_Id == postId);
            foreach (var photo in existingPhotos)
            {
                if (!string.IsNullOrEmpty(photo.Photo_Url))
                {
                    var previousPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), photo.Photo_Url.TrimStart('/'));
                    if (File.Exists(previousPhotoPath))
                    {
                        File.Delete(previousPhotoPath);
                    }
                }
                _context.Photo.Remove(photo);
            }
            _context.SaveChanges();
        }

        private PhotoDto MapEntityToDto(Photo entity)
        {
            if (entity == null)
                return null;

            return new PhotoDto
            {
                photoId = entity.Photo_Id,
                postId = entity.Ppost_Id,
                photoUrl = GetPhoto(entity.Photo_Url)
            };
        }

        private Photo MapDtoToEntity(PhotoDto dto, int postId)
        {
            return new Photo
            {
                Ppost_Id = postId,
                Photo_Url = SavePhoto(dto.photoUrl),
            };
        }

        public string SavePhoto(string base64Data)
        {
            if (!string.IsNullOrEmpty(base64Data))
            {
                string extension = ".jpg";

                if (base64Data.StartsWith("data:image/png"))
                {
                    extension = ".png";
                }
                else if (base64Data.StartsWith("data:image/jpeg"))
                {
                    extension = ".jpg";
                }
         

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Photograph");
                string destinationPath = Path.Combine(folderPath, uniqueFileName);

                byte[] fileBytes = Convert.FromBase64String(base64Data);

                using (var stream = new FileStream(destinationPath, FileMode.Create))
                {
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }

                return $"/Photograph/{uniqueFileName}";
            }

            return null;
        }

        public string GetPhoto(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                string baseUrl = request.Scheme + "://" + request.Host.Value;
                string imageUrl = baseUrl + fileName;

                return imageUrl;
            }

            return null;
        }
    }
}
