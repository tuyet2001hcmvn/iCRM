using ISD.API.Repositories.Services.Marketing;
using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IStoreService _storeService;
        private readonly IEmailAccountService _emailAccountService;
        public ContentsController(IContentService contentService, IStoreService storeService, IEmailAccountService emailAccountService)
        {
            _contentService = contentService;
            _storeService = storeService;
            _emailAccountService = emailAccountService;
        }
        /// <summary>
        /// create new content
        /// </summary>
        /// <param name="contentCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ContentCreateViewModel contentCreate)
        {
            var res= _contentService.Create(contentCreate);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Create successfully",
                Data = res
            });
        }
        /// <summary>
        /// update content by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        
        [HttpPost("{id}")]
        public IActionResult Update([FromRoute] Guid id,[FromBody] ContentEditViewModel obj)
        {
             _contentService.Update(id,obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }
        /// <summary>
        /// get all available stores
        /// </summary>
        /// <returns></returns>
        [HttpGet("Stores")]
        public IActionResult GetStore()
        {
            var res = _storeService.GetStores();
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        /// <summary>
        /// get content by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetContentById([FromRoute] Guid id)
        {
            var res = _contentService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        /// <summary>
        /// get all email account
        /// </summary>
        /// <returns></returns>
        [HttpGet("EmailAccounts")]
        public IActionResult GetFromEmailAccount()
        {
            var res = _emailAccountService.GetEmails();
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        /// <summary>
        /// search content
        /// </summary>
        /// <param name="contentCode"></param>
        /// <param name="contentName"></param>
        /// <param name="actived"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? contentCode, [FromQuery] string contentName, [FromQuery] bool? actived, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string Type)
        {
            List<ContenViewViewModel> list = _contentService.Search(contentCode, contentName, actived, pageIndex, pageSize, Type);
            int totalRow = _contentService.GetTotalRow(contentCode, contentName, actived, Type);
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
        /// <summary>
        /// get all available content
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Gets([FromQuery] string Type)
        {
            var res = _contentService.GetAll(Type);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
    }
}
