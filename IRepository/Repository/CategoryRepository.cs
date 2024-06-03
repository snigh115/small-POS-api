using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Model;
using POS.Repository;

namespace POS.IRepository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;

        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                _context.Entry(existingCategory).State = EntityState.Detached;
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteCategory(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory != null )
            {
                _context.Entry(existingCategory).State = EntityState.Detached;
            }
                _context.Categories.Update(category);
                await _context.SaveChangesAsync(); ;
            

        }

        public async Task ParamentDeleteCategory(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory != null )
            {
                _context.Entry(existingCategory).State = EntityState.Detached;
            }

            if (category.IsDelete)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            
            
        }
    }
}