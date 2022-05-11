using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResoLogTool;
using Second_Project.Commons;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Services;
using Second_Project.ViewModels;
using Product = Second_Project.Models.Product;

namespace Second_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // // GET: api/Product
        // [HttpGet]
        // public ActionResult<IEnumerable<ProductViewModel>> GetProducts()
        // {
        //     return Ok(_productService.GetAllProducts());
        // }

        // GET: api/Product
        [HttpGet]
        public ActionResult<BaseResponseViewModel<ProductViewModel>> GetProducts(
            [FromQuery] ProductViewModel productViewModel, [FromQuery] PagingModel paging,
            [FromQuery] string[] fields = null)
        {
            return Ok(_productService.GetsAllProducts(productViewModel, paging, fields));
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public ActionResult<ProductViewModel> GetProduct(int id)
        {
            var product = _productService.GetProductById(id);
            return Ok(product);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<ProductViewModel> PutProduct(int id, UpdateProductRequestModel productRequestModel)
        {
            var product = _productService.UpdateProduct(id, productRequestModel);

            return Ok(product);
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<ProductViewModel> PostProduct(CreateProductRequestModel productRequestModel)
        {
            var product = _productService.CreateProduct(productRequestModel);

            return Created("", product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);

            return Ok(id);
        }
    }
}