using ISD.Core;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using ISD.ViewModels.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class SendMailReportController : BaseController
    {
        // GET: RegisterReceiveNews
        public ActionResult Index(Guid? CampaignId = null)
        {
            ViewBag.PageId = GetPageId("/Reports/SendMailReport");
            CreateViewBag(CampaignId);
            return View();
        }
        public ActionResult _Search(SendMailSearchViewModel viewModel)
        {
            return ExecuteSearch(()=> {
                var result = _unitOfWork.CampaignRepository.MemberOfTargetGroupInCampaign(viewModel);

                return PartialView(result);
            });
        }

       
        public ActionResult ExportExcel(SendMailSearchViewModel viewModel)
        {
            List<MemberOfTargetGroupInCampaignExcelModel> result = new List<MemberOfTargetGroupInCampaignExcelModel>();
            result = (from s in _unitOfWork.CampaignRepository.MemberOfTargetGroupInCampaign(viewModel)
                      select new MemberOfTargetGroupInCampaignExcelModel()
                      {                        
                          Campaing = s.CampainName,
                          Type = s.Type,
                          ProfileCode = s.ProfileCode??null,
                          ProfileName = s.ProfileName,
                          Phone = s.Phone,
                          Email = s.Email
                      }).ToList();
         
            return Export(result);
        }


        public ActionResult Export(List<MemberOfTargetGroupInCampaignExcelModel> viewModel)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "THÔNG TIN KHÁCH HÀNG CỦA CHIẾN DỊCH";

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "Campaing", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Type", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });
           
            // columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false });
            #endregion

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        #region Helper

        #endregion
        private void CreateViewBag(Guid? CampaignId = null)
        {
            if (CampaignId != null && CampaignId != Guid.Empty)
            {
                var campaign = _unitOfWork.CampaignRepository.GetBy(CampaignId);
                if (campaign != null)
                {
                    ViewBag.CampaignId = campaign.Id;
                    ViewBag.CampaignName = campaign.CampaignName;
                }
             
            }
            
        }

        /// <summary>
        /// Lấy danh sách chiến dịch(dùng cho autocomplete)
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCampaign(string search, string type = null)
        {
            var result = new List<ISDSelectItem>();
            result = (from a in _unitOfWork.CampaignRepository.GetAll()
                      where (string.IsNullOrEmpty(type) || a.Type == type)
                      && (search == null || a.CampaignName.Contains(search))
                      orderby a.CampaignCode
                      select new ISDSelectItem()
                      {
                          value = a.Id,
                          text = a.CampaignName
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}