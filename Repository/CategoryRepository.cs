using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Models;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;
        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            var category = _context.Category.ToList();
            return category.Select(MapEntityToDto);
        }

        public CategoryDto GetCategoryById(int categoryId)
        {
            var category =  _context.Category.FirstOrDefault(c => c.Category_Id == categoryId);
            return MapEntityToDto(category);
        }

        public IEnumerable<CategoryDto> GetCategoriesByPost(int postId)
        {
            var category = _context.Category.Where(c => c.Catpost_Id == postId).ToList();
            return category.Select(MapEntityToDto); 
        }

        public void AddCategories(IEnumerable<CategoryDto> categories, int postId)
        {
            if (!_context.Post.Any(p => p.Post_Id == postId))
            {
                throw new ArgumentException($"Invalid postId: {postId}. Post does not exist.");
            }
            var categoryEntities = categories.Select(dto => MapDtoToEntity(dto, postId));

            _context.Category.AddRange(categoryEntities);
            _context.SaveChanges();
           
        }

        public void UpdateCategories(IEnumerable<CategoryDto> categories, int postId)
        {
            var existingcategory = _context.Category.Where(b => b.Catpost_Id == postId);
            _context.RemoveRange(existingcategory);
            foreach (var category in categories)
            {
                    var categoryEntities = MapDtoToEntity(category, postId);
                   _context.Category.Add(categoryEntities);
   
            }
            _context.SaveChanges();
        }

        public void DeleteCategories(int postId)
        {
            var existingcategory = _context.Category.Where(b => b.Catpost_Id == postId);
            _context.RemoveRange(existingcategory);
            _context.SaveChanges();
        }

        private CategoryDto MapEntityToDto(Category entity)
        {
            if(entity == null)
            {
                return null;
            }
            return new CategoryDto
            {
                categoryId = entity.Category_Id,
                postId = entity.Catpost_Id,
                categoryName = entity.Category_Name,
            };
        }

        private Category MapDtoToEntity(CategoryDto dto, int postId)
        {
            return new Category
            {
               
                Catpost_Id = postId,
                Category_Name = dto.categoryName,
            };
        }

    }
}
