using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using POS.Model;
using POS.Model.DTO;
using POS.Repository;

namespace POS.IService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService (ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository =categoryRepository ?? throw new ArgumentNullException(nameof(_categoryRepository));
            _mapper =mapper ?? throw new ArgumentNullException() ?? throw new ArgumentNullException(nameof(_mapper));   
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDto>(category);
                     
        }

        public async Task AddCategory(CategoryDto categoryDto)
        {
            var category =  _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddCategory(category);
        }

        public async Task UpdateCategory(CategoryDto categoryDto)
        {
            var category =  _mapper.Map<Category>(categoryDto);
            await _categoryRepository.UpdateCategory(category);
            
        }

        public async Task SoftDeleteCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.SoftDeleteCategory(category);
        }

        public async Task ParamentDeleteCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.ParamentDeleteCategory(category);
        }

    }
}