using ISD.API.Repositories.Services;
using ISD.API.ViewModels;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class TemplateAndGiftTargetGroupsController : ControllerBase
    {
        private readonly ITemplateAndGiftTargetGroupService _templateAndGiftTargetGroupService;
        public TemplateAndGiftTargetGroupsController(ITemplateAndGiftTargetGroupService templateAndGiftTargetGroupService)
        {
            _templateAndGiftTargetGroupService = templateAndGiftTargetGroupService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TargetGroupCreateViewModel obj)
        {
            var res = _templateAndGiftTargetGroupService.Create(obj);

            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res,
                Message = "Create successfully"

            });

        }
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? targetGroupCode, [FromQuery] string targetGroupName, [FromQuery] bool? actived, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var list = _templateAndGiftTargetGroupService.Search(targetGroupCode, targetGroupName, actived, pageIndex, pageSize, out totalRow);

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
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var res = _templateAndGiftTargetGroupService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = res,
                IsSuccess = true
            });

        }
        [HttpGet]
        public IActionResult GetAll([FromRoute] Guid id)
        {
            var res = _templateAndGiftTargetGroupService.GetAll();
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = res,
                IsSuccess = true
            });

        }
        [HttpGet("Members/{id}")]
        public IActionResult GetMember([FromRoute] Guid id, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var members = _templateAndGiftTargetGroupService.GetMember(id, pageIndex, pageSize, out totalRow);

            var data = new
            {
                totalRow = totalRow,
                list = members
            };
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = data
            });
        }
        [HttpPost("Members/{id}")]
        public IActionResult ImportMember([FromForm] IFormCollection file, [FromRoute] Guid id)
        {
            if (file == null || file.Files.Count == 0)
            {
                return Ok(new ApiResponse
                {
                    Code = 400,
                    IsSuccess = false,
                    Message = "Import failed!"
                });
            }
            _templateAndGiftTargetGroupService.Import(file.Files[0], id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Import successfully!"
            });
        }
        [HttpPost("{id}")]
        public IActionResult Update([FromBody] TargetGroupEditViewModel obj, [FromRoute] Guid id)
        {
            _templateAndGiftTargetGroupService.Update(obj, id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }
    }
}
