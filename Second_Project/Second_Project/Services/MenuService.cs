using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Second_Project.Commons;
using Second_Project.Helpers;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Utils;
using Second_Project.ViewModels;

namespace Second_Project.Services
{
    public interface IMenuService
    {
        List<MenuViewModel> GetAllMenus();
        MenuViewModel GetMenuById(int menuId);
        MenuViewModel CreateMenu(CreateMenuRequestModel menuRequestModel);
        MenuViewModel UpdateMenu(int menuId, UpdateMenuRequestModel menuRequestModel);
        int DeleteMenu(int menuId);
        bool MenuExits(int menuId);
        List<ProductInMenuModel> GetProductsInMenu(int menuId);
        void AddProductToMenu(int menuId, ProductInMenuRequestModel productInMenuRequestModel);
        bool CheckProductInMenuExist(int menuId, int productId);
        void RemoveProductToMenu(int menuId, ProductInMenuRequestModel productInMenuRequestModel);

        BaseResponseViewModel<MenuViewModel> GetAllMenus(MenuViewModel menuViewModel, PagingModel paging,
            string[] fields);

        BaseResponseViewModel<ProductInMenuModel> GetProductsInMenu(int menuId,
            ProductViewModel productViewModel, PagingModel paging, string[] fields);
    }

    public class MenuService : IMenuService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly IMapper _mapper;

        #region Menu

        public MenuService(ProductDBContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext;
            _mapper = mapper;
        }

        public List<MenuViewModel> GetAllMenus()
        {
            var listMenus = _productDbContext.Menus.Include("Store").ToList();
            var listView = new List<MenuViewModel>();

            foreach (var menu in listMenus)
            {
                listView.Add(_mapper.Map<MenuViewModel>(menu));
            }

            return listView;
        }

        public BaseResponseViewModel<MenuViewModel> GetAllMenus(MenuViewModel menuViewModel, PagingModel paging,
            string[] fields)
        {
            var listMenus = _productDbContext.Menus.Include("Store")
                .ProjectTo<MenuViewModel>(_mapper.ConfigurationProvider).DynamicFilter<MenuViewModel>(menuViewModel);
            var pagingMenus = listMenus.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
            return new BaseResponseViewModel<MenuViewModel>()
            {
                Metadata = new PagingMetadata()
                {
                    Page = paging.Page,
                    Size = paging.Size,
                    Total = listMenus.Count()
                },
                Data = pagingMenus
            };
        }

        public MenuViewModel GetMenuById(int menuId)
        {
            if (!MenuExits(menuId))
            {
                throw new KeyNotFoundException();
            }

            var menu = _productDbContext.Menus.Include("Store").SingleOrDefault(m => m.MenuId == menuId);
            return _mapper.Map<MenuViewModel>(menu);
        }

        public MenuViewModel CreateMenu(CreateMenuRequestModel menuRequestModel)
        {
            Menu menu = _mapper.Map<Menu>(menuRequestModel);
            _productDbContext.Menus.Add(menu);
            _productDbContext.SaveChanges();
            var addedMenu = _productDbContext.Menus.Include("Store").SingleOrDefault(m => m.MenuId == menu.MenuId);
            return _mapper.Map<MenuViewModel>(addedMenu);
        }

        public MenuViewModel UpdateMenu(int menuId, UpdateMenuRequestModel menuRequestModel)
        {
            var menu = _productDbContext.Menus.Include("Store").SingleOrDefault(m => m.MenuId == menuId);
            if (menu != null)
            {
                menu.MenuName = menuRequestModel.MenuName;
                menu.StoreId = menuRequestModel.StoreId;
                _productDbContext.Update(menu);
                _productDbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }

            var updatedMenu = _productDbContext.Menus.Include("Store").SingleOrDefault(m => m.MenuId == menu.MenuId);
            return _mapper.Map<MenuViewModel>(updatedMenu);
        }

        public int DeleteMenu(int menuId)
        {
            var menu = _productDbContext.Menus.SingleOrDefault(m => m.MenuId == menuId);
            if (menu != null)
            {
                _productDbContext.Menus.Remove(menu);
                _productDbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }

            return menuId;
        }

        public bool MenuExits(int menuId)
        {
            return _productDbContext.Menus.Any(m => m.MenuId == menuId);
        }

        #endregion

        #region Product

        public List<ProductInMenuModel> GetProductsInMenu(int menuId)
        {
            var products = _productDbContext.MenuProducts.Where(m => m.MenuId == menuId).Select(m => m.Product);
            var listView = new List<ProductInMenuModel>();
            foreach (var product in products)
            {
                listView.Add(_mapper.Map<ProductInMenuModel>(product));
            }

            return listView;
        }

        public BaseResponseViewModel<ProductInMenuModel> GetProductsInMenu(int menuId,
            ProductViewModel productViewModel, PagingModel paging, string[] fields)
        {
            var products = _productDbContext.MenuProducts.Where(m => m.MenuId == menuId).Select(m => m.Product);
            var pagingProducts = products.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
            var listView = new List<ProductInMenuModel>();
            foreach (var product in products)
            {
                listView.Add(_mapper.Map<ProductInMenuModel>(product));
            }

            return new BaseResponseViewModel<ProductInMenuModel>()
            {
                Metadata = new PagingMetadata()
                {
                    Page = paging.Page,
                    Size = paging.Size,
                    Total = products.Count()
                },
                Data = listView
            };
        }

        public void AddProductToMenu(int menuId, ProductInMenuRequestModel productInMenuRequestModel)
        {
            if (CheckProductInMenuExist(menuId, productInMenuRequestModel.ProductId))
            {
                throw new AppException("Product have been already in menu");
            }

            MenuProduct menuProduct = new MenuProduct();
            menuProduct.MenuId = menuId;
            menuProduct.ProductId = productInMenuRequestModel.ProductId;
            _productDbContext.MenuProducts.Add(menuProduct);
            _productDbContext.SaveChanges();
        }

        public void RemoveProductToMenu(int menuId, ProductInMenuRequestModel productInMenuRequestModel)
        {
            MenuProduct menuProduct = _productDbContext.MenuProducts.SingleOrDefault(m =>
                m.MenuId == menuId && m.ProductId == productInMenuRequestModel.ProductId);
            if (menuProduct != null) _productDbContext.MenuProducts.Remove(menuProduct);
            else
            {
                throw new KeyNotFoundException();
            }

            _productDbContext.SaveChanges();
        }


        public bool CheckProductInMenuExist(int menuId, int productId)
        {
            return _productDbContext.MenuProducts.Any(o => o.MenuId == menuId && o.ProductId == productId);
        }

        #endregion
    }
}