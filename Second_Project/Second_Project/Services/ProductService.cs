using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Second_Project.Commons;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Utils;
using Second_Project.ViewModels;

namespace Second_Project.Services
{
    public interface IProductService
    {
        List<ProductViewModel> GetAllProducts();
        ProductViewModel GetProductById(int productId);
        ProductViewModel CreateProduct(CreateProductRequestModel productRequestModel);
        ProductViewModel UpdateProduct(int productId, UpdateProductRequestModel productRequestModel);
        int DeleteProduct(int productId);
        bool ProductExists(int id);
        List<ProductViewModel> GetProductsByCategory(int categoryId);

        BaseResponseViewModel<ProductViewModel> GetsAllProducts(ProductViewModel productViewModel, PagingModel paging,
            string[] fields);

        BaseResponseViewModel<ProductViewModel> GetProductsByCategory(int categoryId,
            ProductViewModel productViewModel, PagingModel paging, string[] fields);
    }

    public class ProductService : IProductService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly IMapper _mapper;

        public ProductService(ProductDBContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext;
            _mapper = mapper;
        }

        public BaseResponseViewModel<ProductViewModel> GetsAllProducts(ProductViewModel productViewModel,
            PagingModel paging, string[] fields)
        {
            var listProducts = _productDbContext.Products.Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter<ProductViewModel>(productViewModel);
            var pagingProducts = listProducts.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
            return new BaseResponseViewModel<ProductViewModel>()
            {
                Metadata = new PagingMetadata()
                {
                    Page = paging.Page,
                    Size = paging.Size,
                    Total = listProducts.Count()
                },
                Data = pagingProducts
            };
        }


        public List<ProductViewModel> GetAllProducts()
        {
            var listProductViews = new List<ProductViewModel>();
            var listProducts = _productDbContext.Products.Include(p => p.Category).ToList();
            foreach (var product in listProducts)
            {
                listProductViews.Add(_mapper.Map<ProductViewModel>(product));
            }

            return listProductViews;
        }

        public ProductViewModel GetProductById(int productId)
        {
            if (!ProductExists(productId))
            {
                throw new KeyNotFoundException();
            }

            Product product = _productDbContext.Products.Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == productId);
            return _mapper.Map<ProductViewModel>(product);
        }


        public ProductViewModel CreateProduct(CreateProductRequestModel productRequestModel)
        {
            var product = _mapper.Map<Product>(productRequestModel);
            _productDbContext.Add(product);
            _productDbContext.SaveChanges();
            var addedProduct = _productDbContext.Products.Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == product.ProductId);
            return _mapper.Map<ProductViewModel>(addedProduct);
            ;
        }

        public ProductViewModel UpdateProduct(int productId, UpdateProductRequestModel productRequestModel)
        {
            if (!ProductExists(productId))
            {
                throw new KeyNotFoundException();
            }

            Product product = _productDbContext.Products.Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _mapper.Map<UpdateProductRequestModel, Product>(productRequestModel, product);
                _productDbContext.Products.Update(product);
                _productDbContext.SaveChanges();
            }

            var updatedProduct = _productDbContext.Products.Include(x => x.Category)
                .SingleOrDefault(p => p.ProductId == productId);
            return _mapper.Map<ProductViewModel>(updatedProduct);
        }

        public int DeleteProduct(int productId)
        {
            Product product = _productDbContext.Products.SingleOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.IsDelete = true;
                _productDbContext.Update(product);
                _productDbContext.SaveChanges();
            }

            return productId;
        }

        public bool ProductExists(int id)
        {
            return _productDbContext.Products.Any(e => e.ProductId == id);
        }

        public List<ProductViewModel> GetProductsByCategory(int categoryId)
        {
            Category category = _productDbContext.Categories.Include("Products")
                .SingleOrDefault(c => c.CategoryId == categoryId);
            List<Product> products = category?.Products.ToList();
            var listView = new List<ProductViewModel>();
            if (products != null)
                foreach (var product in products)
                {
                    listView.Add(_mapper.Map<ProductViewModel>(product));
                }

            return listView;
        }


        public BaseResponseViewModel<ProductViewModel> GetProductsByCategory(int categoryId,
            ProductViewModel productViewModel, PagingModel paging, string[] fields)
        {
            Category category = _productDbContext.Categories.Include("Products")
                .SingleOrDefault(c => c.CategoryId == categoryId);
            if (category==null)
            {
                throw new KeyNotFoundException();
            }
            List<Product> products = category?.Products.ToList();
            var pagingView = products?.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
            var listView = new List<ProductViewModel>();
            if (pagingView != null)
                foreach (var product in pagingView)
                {
                    listView.Add(_mapper.Map<ProductViewModel>(product));
                }

            return new BaseResponseViewModel<ProductViewModel>()
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
    }
}