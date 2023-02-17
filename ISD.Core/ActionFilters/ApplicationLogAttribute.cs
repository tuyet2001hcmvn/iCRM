using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ISD.Core.ActionFilters
{
    /// <summary>
    /// Tạo bởi Tiến LX
    /// Tạo lúc 22:07 04 tháng 6, 2019
    /// 
    /// Hướng dẫn sử dụng: cái nào muốn log ra thì thêm filter đằng trên
    /// Ví dụ: [ApplicationLog("Edit Store {id}")] thì nó sẽ tự log vô bảng ApplicationLog
    /// {id} là đối số truyền vào của Controller đó
    /// 
    /// Chi tiết: vào ISD.Admin - StyleGuideController để test thử
    /// </summary>
    public class ApplicationLogAttribute : ActionFilterAttribute
    {
        public string Description { get; set; }
        private IDictionary<string, object> _parameters;
        public EntityDataContext _context { get; set; }
        public AppUserPrincipal CurrentUser { get; set; }

        public ApplicationLogAttribute(string description)
        {
            Description = description;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _parameters = filterContext.ActionParameters;
            _context = ((ISD.Core.BaseController)filterContext.Controller)._context; // Lưu _context của BaseController
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var description = Description;

            CurrentUser = new AppUserPrincipal(filterContext.HttpContext.User as ClaimsPrincipal);

            foreach (var kvp in _parameters)
            {
                // Xử lý log data ở đây, có thể chỉnh lại cũng được
                description = description.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
            }

            _context.ApplicationLog.Add(new ApplicationLog()
            {
                ApplicationLogId = Guid.NewGuid(),
                PerformedBy_AccountId = CurrentUser.AccountId,
                PerformedAt = DateTime.Now,
                Action = filterContext.ActionDescriptor.ActionName,
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Description = description
            });

            _context.SaveChanges();
        }
    }
}