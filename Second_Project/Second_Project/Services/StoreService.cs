using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Second_Project.Commons;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.Utils;
using Second_Project.ViewModels;

namespace Second_Project.Services
{
    public interface IStoreService
    {
        List<StoreViewModel> GetAllStores();
        StoreViewModel GetStoreById(int storeId);
        StoreViewModel CreateStore(CreateStoreRequestModel storeRequestModel);
        StoreViewModel UpdateStore(int storeId, UpdateStoreRequestModel storeRequestModel);
        int DeleteStore(int storeId);
        bool StoreExists(int id);

        BaseResponseViewModel<StoreViewModel> GetAllStores(StoreViewModel storeViewModel, PagingModel paging,
            string[] fields);
    }

    public class StoreService : IStoreService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly IMapper _mapper;

        public StoreService(ProductDBContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext;
            _mapper = mapper;
        }

        public List<StoreViewModel> GetAllStores()
        {
            var listStores = _productDbContext.Stores.ToList();
            var listView = new List<StoreViewModel>();
            foreach (var store in listStores)
            {
                listView.Add(_mapper.Map<StoreViewModel>(store));
            }

            return listView;
        }

        public BaseResponseViewModel<StoreViewModel> GetAllStores(StoreViewModel storeViewModel, PagingModel paging,
            string[] fields)
        {
            var listStores = _productDbContext.Stores.ProjectTo<StoreViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter<StoreViewModel>(storeViewModel);
            var pagingStores = listStores.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();

            return new BaseResponseViewModel<StoreViewModel>()
            {
                Metadata = new PagingMetadata()
                {
                    Page = paging.Page,
                    Size = paging.Size,
                    Total = listStores.Count()
                },
                Data = pagingStores
            };
        }

        public StoreViewModel GetStoreById(int storeId)
        {
            if (!StoreExists(storeId))
            {
                throw new KeyNotFoundException();
            }

            var store = _productDbContext.Stores.SingleOrDefault(s => s.StoreId == storeId);
            return _mapper.Map<StoreViewModel>(store);
        }

        public StoreViewModel CreateStore(CreateStoreRequestModel storeRequestModel)
        {
            Store store = _mapper.Map<Store>(storeRequestModel);
            _productDbContext.Stores.Add(store);
            _productDbContext.SaveChanges();
            return _mapper.Map<StoreViewModel>(store);
        }

        public StoreViewModel UpdateStore(int storeId, UpdateStoreRequestModel storeRequestModel)
        {
            var store = _productDbContext.Stores.AsNoTracking().SingleOrDefault(s => s.StoreId == storeId);
            if (store != null)
            {
                store = _mapper.Map<Store>(storeRequestModel);
                store.StoreId = storeId;
                _productDbContext.Update(store);
                _productDbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }

            return _mapper.Map<StoreViewModel>(store);
        }

        public int DeleteStore(int storeId)
        {
            var store = _productDbContext.Stores.SingleOrDefault(s => s.StoreId == storeId);
            if (store != null)
            {
                _productDbContext.Stores.Remove(store);
                _productDbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }

            return storeId;
        }

        public bool StoreExists(int id)
        {
            return _productDbContext.Stores.Any(s => s.StoreId == id);
        }
    }
}