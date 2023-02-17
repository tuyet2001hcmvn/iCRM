using ISD.API.Repositories.Services;
using ISD.API.ViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers
{
    [Route("api/Warehouse/[controller]")]
    [ApiController]
    public class StockTransferRequestsController : ControllerBase
    {
        private readonly IStockTransferRequestService _stockTransferRequestService;
        public StockTransferRequestsController(IStockTransferRequestService stockTransferRequestService)
        {
            _stockTransferRequestService = stockTransferRequestService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] StockTransferRequestCreateViewModel obj)
        {
           var res = _stockTransferRequestService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully",
                Data = res

            });

        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var res = _stockTransferRequestService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpPost("{id}")]
        public IActionResult Edit([FromRoute] Guid id, [FromBody] StockTransferRequestUpdateViewModel obj)
        {
            _stockTransferRequestService.Update(id,obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully",
                Data = id
            });
        }
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] Guid? companyId, [FromQuery] Guid? storeId, [FromQuery] int? stockTransferRequestCode, [FromQuery] string salesEmployeeCode, [FromQuery] Guid? fromStock, [FromQuery] Guid? toStock, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] bool? actived, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var list = _stockTransferRequestService.Search(companyId, storeId, stockTransferRequestCode,salesEmployeeCode, fromStock, toStock,fromDate, toDate, actived, pageIndex, pageSize, out totalRow);
           
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

        [HttpPost("Delete")]
        public IActionResult Delete([FromQuery] Guid id)
        {
            _stockTransferRequestService.DeleteDetail(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Delete successfully"
            });
        }
    }
}
