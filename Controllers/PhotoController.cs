using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoController(IPhotoRepository photoRepository)
        {
           _photoRepository = photoRepository;
        }
        [HttpGet("{photoId}")]
        public IActionResult GetPhotoById(int photoId)
        {
            var photo = _photoRepository.GetPhotoById(photoId);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        [HttpGet("post/{postId}")]
        public IActionResult GetPhotosByPost(int postId)
        {
            var photos = _photoRepository.GetPhotosByPost(postId);
            return Ok(photos);
        }

        [HttpPost]
        public IActionResult AddPhotos([FromBody] IEnumerable<PhotoDto> photos,int id)
        {
            _photoRepository.AddPhotos(photos, id);
            return Ok();
        }

        [HttpPut("{photoId}")]
        public IActionResult UpdatePhoto(int photoId,[FromBody] IEnumerable <PhotoDto> photos)
        {

            _photoRepository.UpdatePhoto(photos,photoId);
            return NoContent();
        }

        [HttpDelete("{photoId}")]
        public IActionResult DeletePhoto(int photoId)
        {
            _photoRepository.DeletePhoto(photoId);
            return NoContent();
        }

    }
}
