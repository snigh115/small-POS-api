using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Model;
using POS.Model.DTO;

namespace POS.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task AddCategory(CategoryDto categoryDto);
        Task UpdateCategory(CategoryDto categoryDto);
        Task SoftDeleteCategory(CategoryDto categoryDto);
        Task ParamentDeleteCategory(CategoryDto categoryDto);
    }
}