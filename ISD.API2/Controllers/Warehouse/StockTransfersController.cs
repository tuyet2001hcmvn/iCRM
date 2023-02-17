using ISD.API.Repositories.Services;
using ISD.API.ViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers.Warehouse
{
    [Route("api/Warehouse/[controller]")]
    [ApiController]
    public class StockTransfersController : ControllerBase
    {
        private readonly ITransferService _stockTransferService;
        public StockTransfersController(ITransferService stockTransferService)
        {
            _stockTransferService = stockTransferService;
        }

        [HttpPost]
        public IActionResult CreateStockTransferByRequest([FromBody] StockTransferCreateViewModel obj)
        {
            _stockTransferService.CreateByRequest(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully",
                Data = obj

            });

        }
    }
}
