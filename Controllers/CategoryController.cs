using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet("post/{postId}")]
        public IActionResult GetCategoriesByPost(int postId)
        {
            var categories = _categoryRepository.GetCategoriesByPost(postId);
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult AddCategories(IEnumerable<CategoryDto> categories,int id)
        {
            _categoryRepository.AddCategories(categories,id);
            return Ok();
        }
        [HttpPut ("{id}")]
        public IActionResult UpdateCategory(int id, IEnumerable<CategoryDto> categories)
        {
            _categoryRepository.UpdateCategories(categories, id);
            return Ok();    
        }
    }
}
