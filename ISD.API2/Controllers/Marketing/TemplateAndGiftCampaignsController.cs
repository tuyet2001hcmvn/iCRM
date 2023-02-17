using ISD.API.Repositories.Services;
using ISD.API.ViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers.Marketing
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class TemplateAndGiftCampaignsController : ControllerBase
    {
        private readonly ITemplateAndGiftCampaignService _templateAndGiftCampaignService;
        private readonly ITemplateAndGiftMemberAddressService _templateAndGiftMemberAddressService;
        public TemplateAndGiftCampaignsController(ITemplateAndGiftCampaignService templateAndGiftCampaignService, ITemplateAndGiftMemberAddressService templateAndGiftMemberAddressService)
        {
            _templateAndGiftCampaignService = templateAndGiftCampaignService;
            _templateAndGiftMemberAddressService = templateAndGiftMemberAddressService;
        }
        [HttpPost]
        public IActionResult Create([FromBody] TemplateAndGiftCampaignCreateModel obj)
        {
            var res = _templateAndGiftCampaignService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully",
                Data = res
            });
        }
        [HttpPost("{id}")]
        public IActionResult Update([FromBody] TemplateAndGiftCampaignEditModel obj, [FromRoute] Guid id)
        {
            _templateAndGiftCampaignService.Update(obj, id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? templateAndGiftCampaignCode, [FromQuery] string templateAndGiftCampaignName, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var list = _templateAndGiftCampaignService.Search(templateAndGiftCampaignCode, templateAndGiftCampaignName, pageIndex, pageSize, out totalRow);

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
            var res = _templateAndGiftCampaignService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = res,
                IsSuccess = true
            });

        }
        [HttpGet("Members")]
        public IActionResult GetMemberByCampaign([FromQuery] Guid campaignId, [FromQuery] string profileCode, [FromQuery] string profileName, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow;
            var list = _templateAndGiftCampaignService.GetMemberByCampaign(campaignId, profileCode, profileName, pageIndex, pageSize, out totalRow);

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
        [HttpGet("MemberAddress")]
        public IActionResult GetMemberByCampaign([FromQuery] Guid campaignId, [FromQuery] Guid profileId)
        {
          var res  = _templateAndGiftCampaignService.GetMember(campaignId, profileId);
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = res,
                IsSuccess = true
            });

        }
        [HttpPost("MemberAddress")]
        public IActionResult CreateMemberAddress([FromBody] TemplateAndGiftMemberAddressCreateModel obj)
        {
            _templateAndGiftMemberAddressService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                Data = obj,
                IsSuccess = true,
                Message = "Update successfully"
            });

        }
    }
}
