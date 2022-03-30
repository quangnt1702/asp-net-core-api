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
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: api/Store
        // [HttpGet]
        // public ActionResult<IEnumerable<StoreViewModel>> GetStores()
        // {
        //     return Ok(_storeService.GetAllStores());
        // }


        // GET: api/Store
        [HttpGet]
        public ActionResult<BaseResponseViewModel<StoreViewModel>> GetStores([FromQuery] StoreViewModel storeViewModel,
            [FromQuery] PagingModel paging, [FromQuery] string[] fields = null)
        {
            return Ok(_storeService.GetAllStores(storeViewModel, paging, fields));
        }

        // GET: api/Store/5
        [HttpGet("{id}")]
        public ActionResult<StoreViewModel> GetStore(int id)
        {
            return _storeService.GetStoreById(id);
        }

        // PUT: api/Store/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutStore(int id, UpdateStoreRequestModel storeRequestModel)
        {
            return Ok(_storeService.UpdateStore(id, storeRequestModel));
        }

        // POST: api/Store
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<StoreViewModel> PostStore(CreateStoreRequestModel storeRequestModel)
        {
            var store = _storeService.CreateStore(storeRequestModel);

            return Created("", store);
        }

        // DELETE: api/Store/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            _storeService.DeleteStore(id);

            return Ok(id);
        }
    }
}