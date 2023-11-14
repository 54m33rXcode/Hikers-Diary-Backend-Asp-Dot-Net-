using Hikers_Diary.DTO;
using Hikers_Diary.Models;

namespace Hikers_Diary.Interfaces
{
    public  interface ICategoryRepository
    {
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int categoryId);
        IEnumerable<CategoryDto> GetCategoriesByPost(int postId);
        public void AddCategories(IEnumerable<CategoryDto> categories, int postId);
        public void UpdateCategories(IEnumerable<CategoryDto> categories, int postId);
        public void DeleteCategories(int postId);   
    }
}
