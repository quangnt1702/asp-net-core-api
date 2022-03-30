using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menu
        // [HttpGet]
        // public ActionResult<IEnumerable<MenuViewModel>> GetMenus()
        // {
        //     return Ok(_menuService.GetAllMenus());
        // }

        // GET: api/Menu
        [HttpGet]
        public ActionResult<BaseResponseViewModel<MenuViewModel>> GetMenus([FromQuery] MenuViewModel menuViewModel,
            [FromQuery] PagingModel paging, [FromQuery] string[] fields)
        {
            return Ok(_menuService.GetAllMenus(menuViewModel, paging, fields));
        }

        // GET: api/Menu/5
        [HttpGet("{id}")]
        public ActionResult<MenuViewModel> GetMenu(int id)
        {
            return Ok(_menuService.GetMenuById(id));
        }

        // PUT: api/Menu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutMenu(int id, UpdateMenuRequestModel menuRequestModel)
        {
            return Ok(_menuService.UpdateMenu(id, menuRequestModel));
        }

        // POST: api/Menu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<MenuViewModel> PostMenu(CreateMenuRequestModel menuRequestModel)
        {
            var menu = _menuService.CreateMenu(menuRequestModel);
            return Created("", menu);
        }

        // DELETE: api/Menu/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMenu(int id)
        {
            _menuService.DeleteMenu(id);

            return Ok(id);
        }

        // POST: api/menu/5/product
        [HttpPost("{id}/products")]
        public IActionResult AddProduct(int id, ProductInMenuRequestModel productInMenuRequestModel)
        {
            _menuService.AddProductToMenu(id, productInMenuRequestModel);
            return Ok();
        }

        // GET: api/menu/5/product
        // [HttpGet("{id}/products")]
        // public ActionResult<IEnumerable<ProductInMenuModel>> GetProducts(int id)
        // {
        //     return Ok(_menuService.GetProductsInMenu(id));
        // }


        // GET: api/menu/5/product
        [HttpGet("{id}/products")]
        public ActionResult<IEnumerable<ProductInMenuModel>> GetProducts(int id,
            [FromQuery] ProductViewModel productViewModel, [FromQuery] PagingModel paging, [FromQuery] string[] fields)
        {
            return Ok(_menuService.GetProductsInMenu(id, productViewModel, paging, fields));
        }

        // DELETE: api/menu/5/product
        [HttpDelete("{id}/products")]
        public IActionResult DeleteProductInMenu(int id, ProductInMenuRequestModel productInMenuRequestModel)
        {
            _menuService.RemoveProductToMenu(id, productInMenuRequestModel);

            return Ok();
        }
    }
}