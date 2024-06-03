using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Model;

namespace POS.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task SoftDeleteCategory(Category category);
        Task ParamentDeleteCategory(Category category);
    }
}