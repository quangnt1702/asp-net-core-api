using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Utils;
using Second_Project.ViewModels;

namespace Second_Project.Services
{
    public interface ICategoryService
    {
        List<CategoryViewModel> GetAllCategories();
        CategoryViewModel GetCategoryById(int categoryId);
        CategoryViewModel CreateCategory(CreateCategoryRequestModel categoryRequestModel);
        CategoryViewModel UpdateCategory(int categoryId, UpdateCategoryRequestModel categoryRequestModel);
        int DeleteCategory(int categoryId);
        bool CategoryExists(int id);

        BaseResponseViewModel<CategoryViewModel> GetAllCategories(CategoryViewModel categoryViewModel,
            PagingModel paging, string[] fields);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly IMapper _mapper;

        public CategoryService(ProductDBContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext;
            _mapper = mapper;
        }


        public BaseResponseViewModel<CategoryViewModel> GetAllCategories(CategoryViewModel categoryViewModel,
            PagingModel paging, string[] fields)
        {
            var categories = _productDbContext.Categories
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter<CategoryViewModel>(categoryViewModel);
            var pagingCategories = categories.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
         
            return new BaseResponseViewModel<CategoryViewModel>()
            {
                Metadata = new PagingMetadata()
                {
                    Page = paging.Page,
                    Size = paging.Size,
                    Total = categories.Count()
                },
                Data = pagingCategories
            };
        }

        public List<CategoryViewModel> GetAllCategories()
        {
            var listCategories = _productDbContext.Categories.ToList();
            var listCategoryViewModels = new List<CategoryViewModel>();
            foreach (var category in listCategories)
            {
                listCategoryViewModels.Add(_mapper.Map<CategoryViewModel>(category));
            }

            return listCategoryViewModels;
        }

        public CategoryViewModel GetCategoryById(int categoryId)
        {
            if (!CategoryExists(categoryId))
            {
                throw new KeyNotFoundException();
            }
            var category = _productDbContext.Categories.SingleOrDefault(c => c.CategoryId == categoryId);
            return _mapper.Map<CategoryViewModel>(category);
        }

        public CategoryViewModel CreateCategory(CreateCategoryRequestModel categoryRequestModel)
        {
            Category category = _mapper.Map<Category>(categoryRequestModel);
            _productDbContext.Categories.Add(category);
            _productDbContext.SaveChanges();

            return _mapper.Map<CategoryViewModel>(category);
        }

        public CategoryViewModel UpdateCategory(int categoryId, UpdateCategoryRequestModel categoryRequestModel)
        {
            if (!CategoryExists(categoryId))
            {
                throw new KeyNotFoundException();
            }
            Category category = _productDbContext.Categories.SingleOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                category.CategoryName = categoryRequestModel.CategoryName;
                _productDbContext.Update(category);
                _productDbContext.SaveChanges();
            }

            return _mapper.Map<CategoryViewModel>(category);
        }

        public int DeleteCategory(int categoryId)
        {
            Category category = _productDbContext.Categories.SingleOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                _productDbContext.Remove(category);
                _productDbContext.SaveChanges();
            }

            return categoryId;
        }

        public bool CategoryExists(int id)
        {
            return _productDbContext.Categories.Any(e => e.CategoryId == id);
        }
    }
}