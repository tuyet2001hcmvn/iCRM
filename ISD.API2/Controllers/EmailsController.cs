using ISD.API.Repositories.Services.Marketing;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        public EmailsController(IEmailService emailService, IWebHostEnvironment env)
        {
            _emailService = emailService;
            _env = env;
        }
        [HttpGet("TrackingOpenedEmail/{id}")]
        public IActionResult TrackingOpenedEmail([FromRoute] string id)
        {
            var emailId = Guid.Parse(id.Replace(".png", ""));
            if (emailId != Guid.Empty)
            {
                _emailService.TrackingOpened(emailId);
            }
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload", "Images", "1.jpg");
            //var image = System.IO.File.OpenRead(path);
            string trackingPixel = @"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVQI12P4DwQACfsD/WMmxY8AAAAASUVORK5CYII=";
            return File(Convert.FromBase64String(trackingPixel), "image/png");
        }
        [HttpGet("Unsubscribe/{id}")]
        public IActionResult Unsubscribe([FromRoute] Guid id)
        {
            _emailService.TrackingOpened(id);
             _emailService.Unsubscribe(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Unsubcribe successfully"
            });
        }
    }
}
