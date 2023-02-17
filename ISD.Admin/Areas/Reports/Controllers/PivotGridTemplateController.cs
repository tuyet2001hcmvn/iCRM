using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class PivotGridTemplateController : BaseController
    {

        public ActionResult Index(Guid? TemplateId = null) {

            SearchResultTemplateModel Template = new SearchResultTemplateModel();
            List<FieldSettingModel> fieldSettingList = (List<FieldSettingModel>)Session[CurrentUser.AccountId + "Layout"];
            if (fieldSettingList != null)
            {
                var FieldNameList = fieldSettingList.Where(x => x.Visible == true && x.PivotArea != 2).OrderBy(x => x.AreaIndex).Select(x => new { x.FieldName, x.Caption }).ToList();
                ViewBag.OrderBy = new SelectList(FieldNameList, "FieldName", "Caption");
            }
            else
            {
                if (TemplateId != null && TemplateId != Guid.Empty)
                {
                    Template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(TemplateId.Value);
                    List<FieldSettingModel> pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(TemplateId.Value);
                    var FieldNameList = pivotSetting.Where(x => x.Visible == true && x.PivotArea != 2).OrderBy(x => x.AreaIndex).Select(x => new { x.FieldName, x.Caption }).ToList();
                    ViewBag.OrderBy = new SelectList(FieldNameList, "FieldName", "Caption", Template.OrderBy);
                }
            }
           

            return PartialView("~/Areas/Reports/Views/_SaveTemplatePopup.cshtml", Template);
        }

        public JsonResult Create(string templateName, string pageUrl,string parameter, bool isSystem, bool IsDefault, string orderBy = null, string typeSort = null)
        {
            Guid pageId = Guid.Empty;
            #region PageId
            if (string.IsNullOrEmpty(parameter))
            {
                pageId = GetPageId(pageUrl);
            }
            else
            {
                pageId = GetPageId(pageUrl, parameter);
            }
            #endregion
            List<FieldSettingModel> settings = (List<FieldSettingModel>)(Session[CurrentUser.AccountId + "Layout"]);
            string LayoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            Session.Remove(CurrentUser.AccountId + "Layout");
            Session.Remove(CurrentUser.AccountId + "LayoutConfigs");
            //Nếu setting rỗng báo lỗi
            if (settings == null || settings.Count <= 0)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Session time out"
                });
            }
            //nếu page id không tồn tại báo lỗi
            if (pageId == null || pageId == Guid.Empty)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Page url invalid"
                });
            }
            _unitOfWork.PivotGridTemplateRepository.Create(templateName,IsDefault, isSystem, CurrentUser.AccountId, pageId, settings, LayoutConfigs, orderBy, typeSort);
            _context.SaveChanges();
            return Json(new
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true
            });

        }
        public JsonResult Edit(Guid templateId, string templateName , bool isDefault, string orderBy = null, string typeSort = null)
        {
            List<FieldSettingModel> settings = (List<FieldSettingModel>)(Session[CurrentUser.AccountId + "Layout"]);
            Session.Remove(CurrentUser.AccountId + "Layout");
            string LayoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            Session.Remove(CurrentUser.AccountId + "LayoutConfigs");
            if (settings == null || settings.Count <= 0)
            {
                settings = new List<FieldSettingModel>();
            }
            _unitOfWork.PivotGridTemplateRepository.Update(templateId, templateName,isDefault, settings, LayoutConfigs, orderBy, typeSort);
            _context.SaveChanges();
            return Json(new
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true
            });

        }
        public JsonResult Delete(Guid templateId)
        {
            if (templateId != Guid.Empty)
            {
                _unitOfWork.PivotGridTemplateRepository.Delete(templateId);
                _context.SaveChanges();

            }
            return Json(new
            {
                Code = System.Net.HttpStatusCode.NoContent,
                Success = true
            });
        }
    }
}