using ISD.API.Repositories.Services;
using ISD.API.ViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Controllers.Customer
{
    [Route("api/Customer/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("RequestCreateECC")]
        public IActionResult RequestCreateECC([FromForm] EmailViewModel emailViewModel)
        {
            //if(emailViewModel.Attachments != null && emailViewModel.Attachments.Count()>0)
            //{
            //    _profileService.RequestCreateEcc(attachments.Files[0]);
            //}
            _profileService.RequestCreateEcc(emailViewModel);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Gửi yêu cầu thành công",
             

            });
        }
        [HttpGet("EmailConfig")]
        public IActionResult GetEmailConfig([FromQuery] string senderName)
        {
          
            var res = _profileService.GetEmailConfig(senderName);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res

            });
        }
    }
}
