using ISD.API.Repositories.Services.Marketing;
using ISD.API.ViewModels;
using ISD.API.ViewModels.MarketingViewModels.MemberOfTargetGroupViewModels;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace ISD.API2.Controllers
{
    [Route("api/Marketing/[controller]")]
    [ApiController]
    public class TargetGroupsController : ControllerBase
    {
        private readonly ITargetGroupService _targetGroupService;
        private readonly IMemberOfTargetGroupService _memberOfTargetGroupService;   
        public TargetGroupsController(ITargetGroupService targetGroupService, IMemberOfTargetGroupService memberOfTargetGroupService)
        {
            _targetGroupService = targetGroupService;
            _memberOfTargetGroupService = memberOfTargetGroupService;
        }
        /// <summary>
        /// create new target group
        /// </summary>
        /// <param name="obj">user input value</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] TargetGroupCreateViewModel obj)
        {
            TargetGroupViewViewModel res = _targetGroupService.Create(obj);

            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res,
                Message="Create successfully"
                
            });

        }
        /// <summary>
        /// update target group by id
        /// </summary>
        /// <param name="obj">user input value</param>
        /// <param name="id">target group id want to update</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IActionResult Update([FromBody] TargetGroupEditViewModel obj, [FromRoute] Guid id)
        {
            _targetGroupService.Update(obj, id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Update successfully"
            });
        }
        /// <summary>
        /// get target group by id 
        /// </summary>
        /// <param name="id">target group id want to get</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            TargetGroupViewViewModel res = _targetGroupService.GetById(id);
            return Ok(new ApiResponse
            {
                Code = 200,
                Data =res,
                IsSuccess = true
            });

        }
        /// <summary>
        /// search target group 
        /// </summary>
        /// <param name="targetGroupCode">get equal or get all</param>
        /// <param name="targetGroupName">get contain or get all</param>
        /// <param name="actived">get equal or get all</param>
        /// <param name="pageIndex">index of page want to get</param>
        /// <param name="pageSize">number of row want to get</param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] int? targetGroupCode, [FromQuery] string targetGroupName, [FromQuery] bool? actived, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string Type)
        {
            List<TargetGroupViewViewModel> list = _targetGroupService.Search(targetGroupCode, targetGroupName, actived, pageIndex, pageSize, Type);
            int totalRow = _targetGroupService.GetTotalRow(targetGroupCode, targetGroupName, actived, Type);
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
        /// import member of target group from excel file
        /// </summary>
        /// <param name="file">excel file member</param>
        /// <param name="id">target group id want to import</param>
        /// <returns></returns>
        [HttpPost("Members/{id}")]
        public IActionResult ImportMember([FromForm] IFormCollection file, [FromRoute] Guid id, [FromQuery] string type = null)
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
            //string mapPath = Path.Combine(_hostingEnvironment.ContentRootPath, "~\\Upload\\ImportExcel\\");
            _memberOfTargetGroupService.Import(file.Files[0], id, type);

            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Import successfully!"
            });
        }
        [HttpPost("ExternalMembers/{id}")]
        public IActionResult ImportExternalMember([FromForm] IFormCollection file, [FromRoute] Guid id)
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
            //string mapPath = Path.Combine(_hostingEnvironment.ContentRootPath, "~\\Upload\\ImportExcel\\");
            _memberOfTargetGroupService.ImportExternal(file.Files[0], id);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Message = "Import successfully!"
            });
        }
        /// <summary>
        /// get members of target group by target group id
        /// </summary>
        /// <param name="id">target group id want to get member</param>
        /// <param name="hasEmail">get member has email or get all</param>
        /// <param name="distinctEmail"></param>
        /// <param name="pageIndex">index of page want to get</param>
        /// <param name="pageSize">number of row want to get</param>
        /// <returns></returns>
        [HttpGet("Members/{id}")]
        public IActionResult GetMember([FromRoute] Guid id, [FromQuery] bool hasEmail, [FromQuery] bool distinctEmail, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow = 0;
            List<MemberOfTargetGroupViewViewModel> members = _memberOfTargetGroupService.GetMemberOfTargetGroupById(id, hasEmail, distinctEmail, pageIndex, pageSize, out totalRow);
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
        [HttpGet("ExternalMembers/{id}")]
        public IActionResult GetMember([FromRoute] Guid id,  [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            int totalRow = 0;
            List<MemberOfTargetGroupViewViewModel> members = _memberOfTargetGroupService.GetExternalMemberOfTargetGroupById(id, pageIndex, pageSize, out totalRow);
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
        /// <summary>
        /// download excel template for import members
        /// </summary>
        /// <returns></returns>
        [HttpGet("Members/Download")]
        public IActionResult Download()
        {
            IFileProvider provider = new PhysicalFileProvider("E:\\AC_CRM\\crm-webapi.ancuong.com\\Upload\\ImportExcel");          
            IFileInfo fileInfo = provider.GetFileInfo("ImportMember.xlsx");
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.ms-excel";
            return File(readStream, mimeType, "ImportMemberTemplate.xlsx");
        }
        /// <summary>
        /// get all available target group 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Gets([FromQuery] string Type)
        {
            var res = _targetGroupService.GetAll(Type);
            return Ok(new ApiResponse
            {
                Code = 200,
                IsSuccess = true,
                Data = res
            });
        }
    }
}
