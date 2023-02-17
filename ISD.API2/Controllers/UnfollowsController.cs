using ISD.API.Repositories.Services.Marketing;
using ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class UnfollowsController : ControllerBase
    {
        private readonly IUnfollowService _unfollowService;
        public UnfollowsController(IUnfollowService unfollowService)
        {
            _unfollowService = unfollowService;
        }
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] string email, [FromQuery] string companyCode, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            List<UnfollowViewModel> list = _unfollowService.Search(email,companyCode, pageIndex, pageSize);
            int totalRow = _unfollowService.GetTotalRow(email,companyCode);
            var data = new
            {
                totalRow = totalRow,
                list = list
            };
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = data,
                IsSuccess = true
            });

        }
        [HttpPost]
        public IActionResult Create([FromBody] UnfollowCreateViewModel obj)
        {
             _unfollowService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully"

            });

        }
        [HttpPost("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _unfollowService.Delete(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Delete successfully"

            });

        }
    }
}
