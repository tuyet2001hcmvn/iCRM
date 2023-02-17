using ISD.API.Repositories.Services.MasterData;
using ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers
{
    [Route("api/MasterData/[controller]")]
    [ApiController]
    public class FavoriteReportsController : ControllerBase
    {
        private readonly IFavoriteReportService _favoriteReportService;
        public FavoriteReportsController(IFavoriteReportService favoriteReportService)
        {

            _favoriteReportService = favoriteReportService;
        }
        [HttpGet("Search")]
        public IActionResult GetAllReportWithPermission([FromQuery] Guid accountId, [FromQuery] string reportName, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var res = _favoriteReportService.Search(accountId, reportName, pageIndex, pageSize, out totalRow);
            var data = new
            {
                totalRow = totalRow,
                list = res
            };
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = data
            });
        }
        [HttpGet("{accountId}")]
        public IActionResult GetFavoriteReport([FromRoute] Guid accountId)
        {
            var res = _favoriteReportService.GetFavoriteReport(accountId);
            var data = new
            {             
                list = res
            };
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = data
            });
        }
        [HttpPost]
        public IActionResult Create([FromBody] FavoriteReportCreateViewModel obj)
        {
            _favoriteReportService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully"
            });
        }
        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] FavoriteReportDeleteViewModel obj)
        {
            _favoriteReportService.Delete(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Delete successfully"
            });
        }
    }
}
