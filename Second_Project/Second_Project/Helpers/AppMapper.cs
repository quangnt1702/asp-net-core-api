using AutoMapper;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.ViewModels;

namespace Second_Project.Helpers
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<Menu, MenuViewModel>().ReverseMap();
            CreateMap<Menu, CreateMenuRequestModel>().ReverseMap();
            CreateMap<Menu, UpdateMenuRequestModel>().ReverseMap();

            CreateMap<Store, StoreViewModel>().ReverseMap();
            CreateMap<Store, CreateStoreRequestModel>().ReverseMap();
            CreateMap<Store, UpdateStoreRequestModel>().ReverseMap();

            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, CreateProductRequestModel>().ReverseMap();
            CreateMap<UpdateProductRequestModel, Product>().ReverseMap();
            CreateMap<ProductInMenuModel, Product>().ReverseMap();

            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CreateCategoryRequestModel>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequestModel>().ReverseMap();
        }
    }
}