using ISD.API.Repositories.Services.Utilities;
using ISD.API.ViewModels.Responses;
using ISD.API.ViewModels.UtilitiesViewModel.SearchTemplateViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers.Utilities
{
    [Route("api/Utilities/[controller]")]
    [ApiController]
    public class SearchTemplatesController : ControllerBase
    {
        private readonly ISearchTemplateService _searchTemplateService;
        public SearchTemplatesController(ISearchTemplateService searchTemplateService)
        {
            _searchTemplateService = searchTemplateService;
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var res = _searchTemplateService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpGet]
        public IActionResult GetByAccount([FromQuery] Guid accountId, [FromQuery] Guid pageId)
        {
            var res = _searchTemplateService.GetByAccount(accountId, pageId);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpPost]
        public IActionResult Create(SearchTemplateCreateViewModel obj)
        {
            var res = _searchTemplateService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Lưu mẫu tìm kiếm thành công.",
                Data = res
            });
        }
        [HttpPost("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] SearchTemplateEditViewModel obj)
        {
            _searchTemplateService.Update(id, obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromQuery] Guid searchTemplateId)
        {
            _searchTemplateService.Delete(searchTemplateId);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Xoá mẫu tìm kiếm thành công"
            });
        }
    }
}
