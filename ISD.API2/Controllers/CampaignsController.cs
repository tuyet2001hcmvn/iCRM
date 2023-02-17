using ISD.API.Repositories.Services.Marketing;
using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
      
        private readonly ICampaignService _campaignService;
        private readonly IWebHostEnvironment _env;
        public CampaignsController(ICampaignService campaignService, IWebHostEnvironment env)
        {
            _campaignService = campaignService;
            _env = env;
        }
        /// <summary>
        /// create new campaign
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] CampaignCreateViewModel obj)
        {
            var res = _campaignService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully",
                Data = res
            });
        }
        /// <summary>
        /// get campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var res = _campaignService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpGet("Status")]
        public IActionResult GetAllStatus()
        {
            var res = _campaignService.GetCampaignStatus();
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        /// <summary>
        /// update campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IActionResult Update([FromRoute] Guid id,[FromBody] CampaignEditViewModel obj)
        {
            _campaignService.Update(id,obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message="Update successfully"
            });
        }
        /// <summary>
        /// search campaign
        /// </summary>
        /// <param name="campaignCode"></param>
        /// <param name="campaignName"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? campaignCode, [FromQuery] string campaignName, [FromQuery] string status, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string Type)
        {
            var res = _campaignService.Search(campaignCode,campaignName,status,pageIndex,pageSize, Type);
            int totalRow = _campaignService.GetTotalRowSearch(campaignCode, campaignName, status, Type);
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
        [HttpGet("Report/{id}")]
        public IActionResult GetReport([FromRoute] Guid id)
        {
            var res = _campaignService.GetReportForCampiagn(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        ////[EnableCors("TrackingEmailPolicy")]
        //[HttpGet("TrackingOpenedEmail/{id}")]
        //public IActionResult TrackingOpenedEmail(string id)
        //{
        //    var emailId = Guid.Parse(id.Replace(".png", ""));
        //    if (emailId != Guid.Empty)
        //    {
        //        _campaignService.EmailIsOpened(emailId);
        //    }
        //    //var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload", "Images", "1.jpg");
        //    //var image = System.IO.File.OpenRead(path);
        //    string trackingPixel = @"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVQI12P4DwQACfsD/WMmxY8AAAAASUVORK5CYII=";

        //    return File(Convert.FromBase64String(trackingPixel), "image/png");
        //}
        
    }
}
