using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POS.Data;
using POS.IService;
using POS.IService.Services;
using POS.Model;
using POS.Model.DTO;
using POS.Model.ViewModels;

namespace POS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, MyDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        
        [HttpPut("{id}")]
        public ActionResult PutCategory(int id, CategoryViewModel categoryViewModel)
        {
            
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound("no category found with that id;");
            }

            if (categoryViewModel == null)
            {
                var categoryJson = JsonSerializer.Serialize(category);
                var jsonResult = new ContentResult 
                {
                    Content = categoryJson,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                return jsonResult;
            }

            category.Name = categoryViewModel.Name;
            category.IsDelete = categoryViewModel.IsDelete;

            _context.Categories.Update(category);
            _context.SaveChanges();

            return Ok(category);

        }

       // POST: api/Categories
        [HttpPost]
        public ActionResult<Category> AddCategory(CategoryViewModel categoryInput)
        {
            if (categoryInput == null)
            {
                return BadRequest("Category data is null.");
            }

            // Create a new category with the provided name
            var category = new Category
            {
                Name = categoryInput.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Additional actions for updating, deleting, and retrieving categories can be added here

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                Name = category.Name,
                IsDelete = category.IsDelete,
            };
            

            return Ok(viewModel);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _context.Categories
                .Where(c => c.IsDelete == false)
                .ToList();
            return Ok(categories);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound("Category not found with that id!");
            }

            category.IsDelete = true;
            
            _context.SaveChanges();
            return Ok("Category deleted successfully");
        }

        [HttpPost("{id}/restore")]
        public ActionResult RestoreCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            if (!category.IsDelete)
            {
                return BadRequest("Category is not Deleted.");
            }

            category.IsDelete = false;

            _context.SaveChanges();

            return Ok("Category restored successfully");
        }

        [HttpDelete("{id}/permanently")]
        public ActionResult PermanentlyDeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null )
            {
                return BadRequest("Category is not found");
            }

            if (!category.IsDelete)
            {
                return BadRequest("Category is not Deleted.");
            }

            
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok("Category is deleted permanently");

        }
        

    //! with CategoryDto 
        [HttpGet("dto/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            
            if (category == null )
            {
                return NotFound("no category with that id");
            }

            // var viewModel = _mapper.Map<CategoryViewModel>(category);
            var viewModel = new CategoryViewModel 
            {
                Name = category.Name,
                IsDelete = category.IsDelete,
            };

            return Ok(viewModel);
        }

        [HttpGet("dto")]
        public async Task<ActionResult<CategoryDto>> GetAllCategoriesWithDto()
        {
            var categories = await _categoryService.GetAllCategories();

            if (categories == null)
            {
                return BadRequest("something wrong");
            }

            return Ok(categories);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> AddCategoryWithDto([FromBody]CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(categoryViewModel);
            }

            var category = _mapper.Map<CategoryDto>(categoryViewModel);
            await _categoryService.AddCategory(category);

            return CreatedAtAction(nameof(GetCategoryById), new {id = category.Id }, categoryViewModel);
            // return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdateCategoryWithDto(int id , [FromBody]CategoryViewModel categoryViewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound("Category not found!");
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryViewModel);
            categoryDto.Id = id;
           
            await _categoryService.UpdateCategory(categoryDto);

            return CreatedAtAction(nameof(GetCategoryById), new {id = categoryDto.Id}, categoryViewModel);
        }

        [HttpDelete("dto/{id}")]
        public async Task<ActionResult> SoftDeleteCategoryWithDto(int id, [FromBody]CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryService.GetCategoryById(id);
            if (existingCategory == null )
            {
                return NotFound("Category Not Found!");
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryViewModel);
            categoryDto.Id = id;
            if (!categoryDto.IsDelete)
            {
                return BadRequest("Is Deleted must be true for deletion ");
            }
            

            await _categoryService.SoftDeleteCategory(categoryDto);

            return Ok("Category is SoftDeleted!");
        }

        [HttpDelete("dto/delete/{id}")]
        public async Task<ActionResult> ParamentDeleteCategoryWithDto(int id, [FromBody]CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryService.GetCategoryById(id);
            if (existingCategory == null )
            {
                return NotFound("Category Not Found!");
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryViewModel);
            categoryDto.Id = id;

            await _categoryService.ParamentDeleteCategory(categoryDto);

            return Ok("Category is Parament Deleted!");
        }
    }   
}
    

        

