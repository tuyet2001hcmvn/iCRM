using ISD.API.Repositories.Services.MasterData;
using ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ISD.API2.Controllers
{
    [Route("api/MasterData/[controller]")]
    [ApiController]
    public class QuestionBanksController : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService;
        private readonly IWebHostEnvironment _env;
        public QuestionBanksController(IQuestionBankService questionBankService, IWebHostEnvironment env)
        {
            _questionBankService = questionBankService;
            _env = env;
        }
        [HttpGet("QuestionCategory")]
        public IActionResult GetQuestionCategoriesAvailable()
        {
            var res = _questionBankService.GetQuestionCategoriesAvailable();
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpGet("Department")]
        public IActionResult GetDepartment()
        {
            var res = _questionBankService.GetDepartment();
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var res = _questionBankService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
        [HttpPost]
        public IActionResult Create(QuestionCreateViewModel obj)
        {
            var res = _questionBankService.Create(obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message="Create successfully",
                Data = res
            });
        }
        [HttpPost("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] QuestionEditViewModel obj)
        {
            _questionBankService.Update(id,obj);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? questionBankCode, [FromQuery] string question, [FromQuery] string questionCategoryId, [FromQuery] bool? actived, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            Guid questionCategoryIdGuid = new Guid();
            if (questionCategoryId==""|| questionCategoryId == null)
            {
                questionCategoryIdGuid = Guid.Empty;
            }
            else
            {
                questionCategoryIdGuid = Guid.Parse(questionCategoryId);
            }
            int totalRow = 0;
            var res = _questionBankService.Search(questionBankCode, question, questionCategoryIdGuid, actived, pageIndex, pageSize, out totalRow);
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
    }
}
