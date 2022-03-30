using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Services;
using Second_Project.ViewModels;

namespace Second_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        // GET: api/Category
        // [HttpGet]
        // public ActionResult<IEnumerable<CategoryViewModel>> GetCategories()
        // {
        //     return Ok(_categoryService.GetAllCategories());
        // }

        // GET: api/Category
        [HttpGet]
        public ActionResult<BaseResponseViewModel<CategoryViewModel>> GetCategories(
            [FromQuery] CategoryViewModel categoryViewModel, [FromQuery] PagingModel paging,
            [FromQuery] string[] fields = null)
        {
            return Ok(_categoryService.GetAllCategories(categoryViewModel, paging, fields));
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public ActionResult<CategoryViewModel> GetCategory(int id)
        {
            return Ok(_categoryService.GetCategoryById(id));
        }

        // GET: api/category/5/product
        // [HttpGet("{id}/products")]
        // public ActionResult<List<ProductViewModel>> GetProductByCategory(int id)
        // {
        //     if (!_categoryService.CategoryExists(id))
        //     {
        //         return NotFound();
        //     }
        //
        //     return Ok(_productService.GetProductsByCategory(id));
        // }

        [HttpGet("{id}/products")]
        public ActionResult<BaseResponseViewModel<ProductViewModel>> GetProductByCategory(int id,
            [FromQuery] ProductViewModel productViewModel, [FromQuery] PagingModel paging,
            [FromQuery] string[] fields = null)
        {
            if (!_categoryService.CategoryExists(id))
            {
                return NotFound();
            }

            return Ok(_productService.GetProductsByCategory(id,productViewModel,paging,fields));
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public ActionResult<CategoryViewModel> PutCategory(int id, UpdateCategoryRequestModel categoryRequestModel)
        {
            return Ok(_categoryService.UpdateCategory(id, categoryRequestModel));
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult<CategoryViewModel> PostCategory(CreateCategoryRequestModel categoryRequestModel)
        {
            var category = _categoryService.CreateCategory(categoryRequestModel);

            return Created("", category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return Ok(id);
        }
    }
}