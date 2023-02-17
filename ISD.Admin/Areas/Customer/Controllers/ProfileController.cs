using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using ISD.Repositories.Excel;
using System.Data;
using System.Transactions;
using System.Web.Configuration;
using ISD.Repositories.Infrastructure.Extensions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace Customer.Controllers
{
    [AllowAnonymous]
    public class ProfileController : BaseController
    {
        const string controllerCode = ConstExcelController.Profile;
        const int startIndex = 10;
        private string viewMode = WebConfigurationManager.AppSettings["ViewExtens"].ToString();

        // GET: Profile
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index(string Type = null, string phoneNumber = "")
        {
            ViewBag.Actived = true;
            //Title
            string pageUrl = "/Customer/Profile";
            var parameter = string.Empty;
            if (Type == ConstProfileType.Account)
            {
                parameter = "?Type=Account";
            }
            else if (Type == ConstProfileType.Contact)
            {
                parameter = "?Type=Contact";
            }
            else if (Type == ConstProfileType.Lead)
            {
                parameter = "?Type=Lead";
            }
            else if (Type == ConstProfileType.Opportunity)
            {
                parameter = "?Type=Opportunity";
            }
            else if (Type == ConstProfileType.Competitor)
            {
                parameter = "?Type=Competitor";
            }
            var title = (from p in _context.PageModel
                         where p.PageUrl == pageUrl
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;

            ViewBag.PageId = GetPageId(pageUrl, parameter);
            CreateViewBagSearch(ProfileType: Type);

            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion

            //var ProVM = new ProfileViewModel
            //{
            //    Phone = phoneNumber
            //};
            ViewBag.Phone = phoneNumber;

            #region CreateRequestTime
            var currentDate = DateTime.Now;
            ViewBag.CreateRequestTimeFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
            ViewBag.CreateRequestTimeTo = currentDate;
            #endregion

            #region Current user email
            ViewBag.CurrentUserEmail = _context.SalesEmployeeModel.Where(p => p.SalesEmployeeCode == CurrentUser.EmployeeCode).Select(p => p.Email).FirstOrDefault();
            #endregion

            return View();
        }
        public ActionResult _Search(ProfileSearchViewModel searchViewModel)
        {
            //Session["frmSearchProfile"] = searchViewModel;
            return ExecuteSearch(() =>
            {
                ProfileRepository repo = new ProfileRepository(_context);
                var profiles = repo.Search(searchViewModel);

                ViewBag.Type = searchViewModel.Type;

                return PartialView(profiles);
            });
        }

        public ActionResult ExportEmail(ProfileSearchViewModel searchViewModel)
        {
            ProfileRepository repo = new ProfileRepository(_context);
            var profiles = repo.SearchQueryProfileEmail(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode);
            return ExportEmailExcel(profiles);
        }

        public ActionResult ExportProfileForProductPromotion(ProfileSearchViewModel searchViewModel)
        {
            ProfileRepository repo = new ProfileRepository(_context);
            var profiles = repo.SearchQueryProfileForProductPromotion(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode);
            return ExportProfileForProductPromotionExcel(profiles);
        }

        public FileContentResult ExportProfileForProductPromotionExcel(List<ProfileSearchResultViewModel> profile)
        {
            //#region //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Customer_Profiles);
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
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

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileId", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileName", isAllowedToEdit = true });
            #endregion //Columns to take
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));
            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult ExportEmailExcel(List<ProfileSearchResultViewModel> profile)
        {
            //#region //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Customer_Emails);
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
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

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileId", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Email", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Telephone", isAllowedToEdit = true });
            #endregion //Columns to take
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));
            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        public ActionResult _PaggingServerSide(DatatableViewModel model, ProfileSearchViewModel searchViewModel, List<Guid> ProvinceId)
        {
            //try
            //{
                // action inside a standard controller
                //90318
                int filteredResultsCount;
                //10
                int totalResultsCount = model.length;

                #region //Create Date
                if (searchViewModel.CreateCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.CreateFromDate = fromDate;
                    searchViewModel.CreateToDate = toDate;
                }
                #endregion

                searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
                searchViewModel.ProvinceIdList = ProvinceId;

                //Page Size 
                searchViewModel.PageSize = model.length;

                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;

                var res = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);
                if (res != null && res.Count() > 0)
                {
                    string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                        if (!string.IsNullOrEmpty(item.Address) && item.Address.StartsWith(","))
                        {
                            item.Address = item.Address.Remove(0, 1).Trim();
                        }
                    }
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = res
                });
            //}
            //catch //(Exception ex)
            //{
            //    return Json(null);
            //}
        }


        public ActionResult SearchContactDeleted(DatatableViewModel model, ProfileSearchViewModel searchViewModel)
        {
            try
            {
                int filteredResultsCount;
                //10
                int totalResultsCount = model.length;
                ProfileRepository repo = new ProfileRepository(_context);

                //Page Size 
                searchViewModel.PageSize = model.length;

                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;

                var res = repo.SearchQueryContacDeleted(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);
                if (res != null && res.Count() > 0)
                {
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                    }
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = res
                });
            }
            catch// (Exception ex)
            {
                return Json(null);
            }
        }
        #endregion

        //GET: /Profile/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var profile = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == id);
            if (profile != null)
            {
                ProfileDetailViewModel profileView = new ProfileDetailViewModel();

                #region Lấy Name các thông tin là ID, CODE
                var customerType = _context.CatalogModel.FirstOrDefault(p => p.CatalogTypeCode == ConstCatalogType.CustomerType && p.CatalogCode == profile.CustomerTypeCode);
                if (customerType == null)
                {
                    profileView.CustomerTypeName = "";
                }
                else
                {
                    profileView.CustomerTypeName = customerType.CatalogText_vi;
                }
                //7. Tỉnh/Thành phố
                var province = _context.ProvinceModel.FirstOrDefault(p => p.ProvinceId == profile.ProvinceId);
                if (province == null)
                {
                    profileView.ProvinceName = "";
                }
                else
                {

                    profileView.ProvinceName = province.ProvinceName;
                }
                //8. Quận/Huyện
                var _districtRepository = new DistrictRepository(_context);
                var district = _districtRepository.Find(profile.DistrictId);
                if (district == null)
                {
                    profileView.DistrictName = "";
                }
                else
                {
                    profileView.DistrictName = district.DistrictName;
                }
                #endregion
                #region map profile to view
                //1. GUID
                //profileView.ProfileId = profile.ProfileId;
                //2. Loại
                profileView.CustomerTypeCode = profile.CustomerTypeCode;

                //3. Ho va Tên|Tên công ty
                profileView.ProfileName = profile.ProfileName;
                //4. Số điện thoại
                profileView.Phone = profile.Phone;
                //5. Email
                profileView.Email = profile.Email;
                //6. Địa chỉ
                profileView.Address = profile.Address;


                //9. Ghi chú
                profileView.Note = profile.Note;
                //10. Hình ảnh
                profileView.ImageUrl = profile.ImageUrl;
                //11. Nhân viên tạo
                profileView.CreateByEmployee = profile.CreateByEmployee;
                //12. Tạo tại công ty
                profileView.CreateAtCompany = profile.CreateAtCompany;
                //13. Tạo tại cửa hàng
                profileView.CreateAtSaleOrg = profile.CreateAtSaleOrg;
                //14. CreateBy
                profileView.CreateBy = profile.CreateBy;
                //15. Thời gian tạo
                profileView.CreateTime = profile.CreateTime;
                //16. CreateBy
                profileView.LastEditBy = profile.LastEditBy;
                //17. Thời gian tạo
                profileView.LastEditTime = profile.LastEditTime;
                //18. Actived
                profileView.Actived = profile.Actived;
                #endregion
                #region map bussiness or customer
                //Bussiness
                if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                {
                    var proBussiness = _context.ProfileBAttributeModel.FirstOrDefault(p => p.ProfileId == id);
                    if (proBussiness == null)
                    {
                        return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_Profiles.ToLower()) });
                    }

                    //Lấy Name Career vả category
                    //6.Ngành nghề
                    var catalogCareer = _context.CatalogModel.FirstOrDefault(p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer && p.CatalogCode == proBussiness.CustomerCareerCode);
                    if (catalogCareer == null)
                    {
                        profileView.CustomerCareerName = "";
                    }
                    else
                    {
                        profileView.CustomerCareerName = catalogCareer.CatalogText_vi;
                    }
                    //end

                    //2. Mã số thuế
                    profileView.TaxNo = proBussiness.TaxNo;
                    //3. Người liên hệ
                    profileView.ContactName = proBussiness.ContactName;
                    //4.Vị trí
                    profileView.PositionB = proBussiness.Position;
                }
                //Customer
                if (profile.CustomerTypeCode == ConstCustomerType.Customer)
                {
                    var proCustomer = _context.ProfileCAttributeModel.FirstOrDefault(p => p.ProfileId == id);
                    if (proCustomer == null)
                    {
                        return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_Profiles.ToLower()) });
                    }
                    //Get Name company
                    var companyOfCustomer = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == proCustomer.CompanyId);
                    //2. Công ty
                    if (companyOfCustomer == null)
                    {
                        profileView.CompanyName = "";
                    }
                    else
                    {
                        profileView.CompanyName = companyOfCustomer.ProfileName;
                    }

                    //3.Chức vụ
                    profileView.PositionC = proCustomer.Position;
                }
                #endregion
                return View(profileView);
            }
            return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_Profiles.ToLower()) });
        }
        #endregion

        //GET: /Profile/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create(string Type, string phoneNumber = "")
        {
            var profileVM = new ProfileViewModel()
            {
                ProfileTypeCode = Type,
                Actived = true,
                VisitDate = DateTime.Now,
                IsAnCuongAccessory = false,
                IsThiCong = false,
                Phone = phoneNumber
            };
            var store = _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == CurrentUser.SaleOrg);
            CreateViewBag(CustomerTypeCode: profileVM.CustomerTypeCode, isForeignCustomer: profileVM.isForeignCustomer, ProfileType: Type, isEditMode: false, SaleOrg: CurrentUser.SaleOrg);
            ViewBag.EmployeeCode = CurrentUser.EmployeeCode;
            //Title
            var title = (from p in _context.PageModel
                         where p.PageUrl == "/Customer/Profile" && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = LanguageResource.Create + " " + title;
            //Profile Config
            var configList = GetProfileConfig(Type, true);

            return View(profileVM);
        }
        //POST: Create
        [HttpPost]
        public JsonResult Create(ProfileViewModel profileVM, HttpPostedFileBase FileUpload, List<string> Phone, List<string> Email, List<string> HandoverFurnitureList, List<string> OpportunityStatusTypeList, List<PersonInChargeViewModel> personInChargeList, List<PersonInChargeNotRequiredViewModel> personInCharge2List, List<ProfileGroupCreateViewModel> profileGroupList, List<Profile_Catalog_MappingViewModel> CompetitorAreaList, List<Profile_Catalog_MappingViewModel> DistributionIndustry)
        {
            return ExecuteContainer(() =>
            {
                List<string> errorList = new List<string>();
                //Type
                string redirectUrl = string.Format("/Customer/Profile?Type={0}", profileVM.ProfileTypeCode);

                //Check MST
                if (!string.IsNullOrEmpty(profileVM.TaxNo))
                {
                    var existsTaxNo = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Account && p.TaxNo == profileVM.TaxNo).FirstOrDefault();
                    if (existsTaxNo != null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = "Mã số thuế đã tồn tại. Vui lòng kiểm tra lại.",
                        });
                    }
                }
                //Check yêu cầu tạo mã ECC
                if (profileVM.isCreateRequest == true && (string.IsNullOrEmpty(profileVM.Manager) || string.IsNullOrEmpty(profileVM.DebsEmployee)))
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn đầy đủ thông tin \"Nhân viên quản lý\" và \"Nhân viên công nợ\".",
                    });
                }
                ProfileModel modelAdd = new ProfileModel()
                {
                    ProfileId = Guid.NewGuid()
                };
                if (FileUpload != null)
                {
                    modelAdd.ImageUrl = Upload(FileUpload, "Profile");
                }

                //Dự án
                //Thời gian bắt đầu
                if (profileVM.StartMonth.HasValue && profileVM.StartYear.HasValue)
                {
                    profileVM.Date1 = new DateTime(profileVM.StartYear.Value, profileVM.StartMonth.Value, 1);
                }
                //Thời gian kết thúc
                if (profileVM.EndMonth.HasValue && profileVM.EndYear.HasValue)
                {
                    profileVM.Date2 = new DateTime(profileVM.EndYear.Value, profileVM.EndMonth.Value, 1);
                    //Từ thời gian kết thúc tính thời gian hoàn thiện => quý, năm
                    profileVM.Text5 = GetQuarter(profileVM.Date2.Value).ToString();
                    profileVM.Text4 = profileVM.EndYear.ToString();
                }
                //Create profile and details
                _unitOfWork.ProfileRepository.CreateProfile(profileVM, modelAdd, Phone, Email, HandoverFurnitureList, OpportunityStatusTypeList, personInChargeList, personInCharge2List, profileGroupList, CurrentUser.AccountId, CurrentUser.CompanyCode, CurrentUser.EmployeeCode, out errorList, CompetitorAreaList, DistributionIndustry);

                //Return errors
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = errorList
                    });
                }
                _context.SaveChanges();
                #region sync_Profile_ExtendInfo_To_Table
                _unitOfWork.ProfileRepository.sync_Profile_ExtendInfo_To_Table(modelAdd.ProfileId);
                #endregion
                var title = (from p in _context.PageModel
                             where p.PageUrl == "/Customer/Profile" && p.Parameter.Contains(profileVM.ProfileTypeCode)
                             select p.PageName).FirstOrDefault();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, title),
                    RedirectUrl = redirectUrl,
                    Id = modelAdd.ProfileId
                });
            });
        }
        #endregion

        //GET: /Profile/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id, string ProfileType = null)
        {
            //Some design change in Edit Mode
            ViewBag.isEditMode = true;
            ViewBag.ViewExtens = viewMode;

            bool error = false;
            var profileView = _unitOfWork.ProfileRepository.GetProfileDetails(id, CurrentUser.CompanyCode, viewMode, out error);
            //Check phân quyền
            if (error == false && (ProfileType == ConstProfileType.Opportunity || ProfileType == ConstProfileType.Lead))
            {
                var User = _unitOfWork.AccountRepository.GetBy(CurrentUser.AccountId);
                if (User.isViewOpportunity != true)
                {
                    var data = (from p in _context.PersonInChargeModel
                                join a in _context.AccountModel on p.SalesEmployeeCode equals a.EmployeeCode
                                where a.AccountId == CurrentUser.AccountId && (p.SalesEmployeeType == 4 || p.SalesEmployeeType == 5) && p.ProfileId == profileView.ProfileId
                                select p.ProfileId);
                    if (data == null || data.Count() < 1)
                    {
                        error = true;
                    }
                }
                
            }
            #region ViewBag
            if (error == false)
            {
                //Nội thất bàn giao
                var HandoverFurniture = _context.Profile_Opportunity_MaterialModel.Where(p => p.ProfileId == id && p.MaterialType == ConstMaterialType.NoiThatBanGiao)
                                                                                    .Select(p => p.MaterialCode)
                                                                                    .ToList();
                profileView.ProfileTypeCode = !string.IsNullOrEmpty(ProfileType) ? ProfileType : profileView.ProfileTypeCode;
                //Phân loại đối thủ
                var CompetitorIndustry = _context.Competitor_Industry_Mapping.Where(p => p.CompetitorId == id).Select(p => p.IndustryCode).ToList();
                //Thời gian
                int? StartMonth = profileView.Date1.HasValue ? (int?)profileView.Date1.Value.Month : null;
                int? StartYear = profileView.Date1.HasValue ? (int?)profileView.Date1.Value.Year : null;
                int? EndMonth = profileView.Date2.HasValue ? (int?)profileView.Date2.Value.Month : null;
                int? EndYear = profileView.Date2.HasValue ? (int?)profileView.Date2.Value.Year : null;
                //Loại hình
                var OpportunityType = _context.ProfileTypeModel.Where(p => p.ProfileId == id).Select(p => p.CustomerTypeCode).ToList();
                var OpportunityStatusTypeList = _context.Profile_OpportunityStatus_Mapping.Where(p => p.ProfileId == id && p.MappingType == ConstCatalogType.Opportunity_Status)
                                                                                    .Select(p => p.CatalogCode)
                                                                                    .ToList();
                CreateViewBag(
                    CustomerTypeCode: profileView.CustomerTypeCode,
                    ProvinceId: profileView.ProvinceId,
                    DistrictId: profileView.DistrictId,
                    CustomerGroupCode: profileView.CustomerGroupCode,
                    CustomerCareerCode: profileView.CustomerCareerCode,
                    isForeignCustomer: profileView.isForeignCustomer,
                    DayOfBirth: profileView.DayOfBirth,
                    MonthOfBirth: profileView.MonthOfBirth,
                    CompanyId: profileView.CompanyId,
                    ProfileType: profileView.ProfileTypeCode,
                    profileId: profileView.ProfileId,
                    WardId: profileView.WardId,
                    SaleOfficeCode: profileView.SaleOfficeCode,
                    Age: profileView.Age,
                    CustomerSourceCode: profileView.CustomerSourceCode,
                    isEditMode: true,
                    SaleOrg: profileView.CreateAtSaleOrg,
                    Title: profileView.Title,
                    Position: profileView.ProfileContactPosition,
                    AddressTypeCode: profileView.AddressTypeCode,
                    DepartmentCode: profileView.DepartmentCode,
                    ProjectStatusCode: profileView.ProjectStatusCode,
                    QualificationLevelCode: profileView.QualificationLevelCode,
                    ProjectSourceCode: profileView.ProjectSourceCode,
                    isCreateRequest: profileView.isCreateRequest,
                    PaymentMethodCode: profileView.PaymentMethodCode,
                    PartnerFunctionCode: profileView.PartnerFunctionCode,
                    CurrencyCode: profileView.CurrencyCode,
                    TaxClassificationCode: profileView.TaxClassificationCode,
                    Manager: profileView.Manager,
                    DebsEmployee: profileView.DebsEmployee,
                    PaymentTermCode: profileView.PaymentTermCode,
                    ReconcileAccountCode: profileView.ReconcileAccountCode,
                    CustomerAccountAssignmentGroupCode: profileView.CustomerAccountAssignmentGroupCode,
                    CompleteQuarter: profileView.CompleteQuarter,
                    CompleteYear: profileView.CompleteYear,
                    HandoverFurniture: HandoverFurniture,
                    Dropdownlist7: profileView.Dropdownlist7,
                    Dropdownlist8: profileView.Dropdownlist8,
                    CompetitorIndustryList: CompetitorIndustry,
                    StartMonth: StartMonth,
                    StartYear: StartYear,
                    EndMonth: EndMonth,
                    EndYear: EndYear,
                    OpportunityType: OpportunityType,
                    OpportunityStatusTypeList: OpportunityStatusTypeList
                    );

                //Profile Config
                var configList = GetProfileConfig(profileView.ProfileTypeCode, true, profileView);
                //Xem doanh số
                var isViewRevenue = _context.AccountModel.Where(p => p.AccountId == CurrentUser.AccountId)
                                            .Select(p => p.isViewRevenue).FirstOrDefault();
                ViewBag.isViewRevenue = isViewRevenue;
                //Liên hệ => nếu liên hệ thuộc KH đồng bộ từ ECC => khóa các trường đồng bộ
                //[2021-08-18] - Update: Bỏ đồng bộ liên hệ, cho phép chỉnh sửa, xóa liên hệ trên web
                //if (!string.IsNullOrEmpty(profileView.ProfileTypeCode) && profileView.ProfileTypeCode == ConstCustomerType.Contact)
                //{
                //    var ProfileForeignCode = _context.ProfileModel.Where(p => p.ProfileId == profileView.ReferenceProfileId).Select(p => p.ProfileForeignCode).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(ProfileForeignCode))
                //    {
                //        ViewBag.ProfileForeignCode = ProfileForeignCode;
                //    }
                //}

                return View(profileView);
            }
            #endregion
            return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_Profiles.ToLower()) });
        }
        //POST: Edit
        [HttpPost]
        public JsonResult Edit(ProfileViewModel profileViewModel, HttpPostedFileBase FileUpload, List<string> Phone, List<string> Email, List<string> HandoverFurnitureList,List<string> OpportunityStatusTypeList, List<PersonInChargeViewModel> personInChargeList, List<PersonInChargeNotRequiredViewModel> personInCharge2List, List<ProfileGroupCreateViewModel> profileGroupList, List<Profile_Catalog_MappingViewModel> CompetitorAreaList, List<Profile_Catalog_MappingViewModel> DistributionIndustry)
        {
            return ExecuteContainer(() =>
            {
                string redirectUrl = string.Empty;
                List<string> errorList = new List<string>();
                //Get profile in database
                var profile = _context.ProfileModel.Find(profileViewModel.ProfileId);
                if (profile != null)
                {
                    //Nếu set ngưng hoạt động thì set field MST = NULL => tránh tạo mới KH sẽ trùng MST cũ này và không cần check các field khác
                    if (profileViewModel.Actived == false)
                    {
                        profile.TaxNo = null;
                        profile.Actived = profileViewModel.Actived;

                        #region Cập nhật liên hệ theo khách hàng
                        if (profile.Actived == false)
                        {
                            var contactInDb = _context.ProfileContactAttributeModel.Where(p => p.CompanyId == profile.ProfileId);
                            if (contactInDb != null && contactInDb.Count() > 0)
                            {
                                foreach (var item in contactInDb)
                                {
                                    var profileContact = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == item.ProfileId);
                                    if (profileContact != null)
                                    {
                                        profileContact.Actived = false;
                                        _context.Entry(profileContact).State = EntityState.Modified;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Check MST
                        if (!string.IsNullOrEmpty(profileViewModel.TaxNo) && string.IsNullOrEmpty(profile.ProfileForeignCode))
                        {
                            var existsTaxNo = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Account && p.TaxNo == profileViewModel.TaxNo && p.ProfileId != profile.ProfileId && p.Actived == true).FirstOrDefault();
                            if (existsTaxNo != null)
                            {
                                return Json(new
                                {
                                    Code = System.Net.HttpStatusCode.NotModified,
                                    Success = false,
                                    Data = "Mã số thuế đã tồn tại. Vui lòng kiểm tra lại.",
                                });
                            }
                        }
                        //Check yêu cầu tạo mã ECC
                        if (profileViewModel.isCreateRequest == true && (string.IsNullOrEmpty(profileViewModel.Manager) || string.IsNullOrEmpty(profileViewModel.DebsEmployee)))
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = "Vui lòng chọn đầy đủ thông tin \"Nhân viên quản lý\" và \"Nhân viên công nợ\".",
                            });
                        }

                        if (FileUpload != null)
                        {
                            profile.ImageUrl = Upload(FileUpload, "Profile");
                        }

                        //Dự án
                        //Thời gian bắt đầu
                        if (profileViewModel.StartMonth.HasValue && profileViewModel.StartYear.HasValue)
                        {
                            profileViewModel.Date1 = new DateTime(profileViewModel.StartYear.Value, profileViewModel.StartMonth.Value, 1);
                        }
                        //Thời gian kết thúc
                        if (profileViewModel.EndMonth.HasValue && profileViewModel.EndYear.HasValue)
                        {
                            profileViewModel.Date2 = new DateTime(profileViewModel.EndYear.Value, profileViewModel.EndMonth.Value, 1);
                            //Từ thời gian kết thúc tính thời gian hoàn thiện => quý, năm
                            profileViewModel.Text5 = GetQuarter(profileViewModel.Date2.Value).ToString();
                            profileViewModel.Text4 = profileViewModel.EndYear.ToString();
                        }
                        //Update profile and details
                        _unitOfWork.ProfileRepository.UpdateProfile(profileViewModel, profile, Phone, Email, HandoverFurnitureList, OpportunityStatusTypeList, personInChargeList, personInCharge2List, profileGroupList, CurrentUser.AccountId, CurrentUser.CompanyCode, CurrentUser.EmployeeCode, out errorList, CompetitorAreaList, DistributionIndustry);
                    }

                    redirectUrl = string.Format("/Customer/Profile?Type={0}", profileViewModel.ProfileTypeCode);
                    //Return errors
                    if (errorList != null && errorList.Count > 0)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = errorList
                        });
                    }
                    _context.SaveChanges();
                }
                string pageUrl = "/Customer/Profile";
                var title = (from p in _context.PageModel
                             where p.PageUrl == pageUrl &&
                             p.Parameter.Contains(profileViewModel.ProfileTypeCode)
                             select p.PageName).FirstOrDefault();
                #region sync_Profile_ExtendInfo_To_Table
                _unitOfWork.ProfileRepository.sync_Profile_ExtendInfo_To_Table(profileViewModel.ProfileId);
                #endregion
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, title),
                    RedirectUrl = redirectUrl,
                    RedirectEditUrl = string.Format("/Customer/Profile/Edit/?id={0}&ProfileType={1}", profileViewModel.ProfileId, profileViewModel.ProfileTypeCode)
                });
            });
        }

        [AllowAnonymous]
        public ActionResult Detail(Guid id, string CompanyCode)
        {
            //Some design change in Edit Mode
            ViewBag.CompanyCode = CompanyCode;
            var profileView = _unitOfWork.ProfileRepository.GetById(id);
            return View(profileView);
        }
        public ActionResult ConvertToOpportunity(Guid ProfileId)
        {
            return ExecuteContainer(() =>
            {
                var profile = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
                if (profile != null)
                {
                    profile.CustomerTypeCode = ConstProfileType.Opportunity;
                    _context.Entry(profile).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = "Chuyển thành Update dự án thành công!",
                    RedirectEditUrl = string.Format("/Customer/Profile/Edit/?id={0}&ProfileType={1}", ProfileId, ConstProfileType.Opportunity)
                });
            });
        }
        #endregion

        #region CreateViewBag, Helper
        public int GetQuarter(DateTime date)
        {
            if (date.Month <= 3)
                return 1;
            else if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else
                return 4;
        }
        //ViewBag Create and Edit, Search
        public void CreateViewBag(string CustomerTypeCode = null, Guid? ProvinceId = null, Guid? DistrictId = null, string CustomerGroupCode = null, string CustomerCareerCode = null, bool? isForeignCustomer = null, int? DayOfBirth = null, int? MonthOfBirth = null, Guid? CompanyId = null, string ProfileType = null, Guid? profileId = null, Guid? WardId = null, string SaleOfficeCode = null, string Age = null, string CustomerSourceCode = null, bool? isEditMode = null, string SaleOrg = null, string Title = null, string Position = null, string DepartmentCode = null, Guid? StoreId = null, string AddressTypeCode = null, string ProjectStatusCode = null, string QualificationLevelCode = null, string ProjectSourceCode = null, bool? isCreateRequest = null, string PaymentMethodCode = null, string PartnerFunctionCode = null, string CurrencyCode = null, string TaxClassificationCode = null, string Manager = null, string DebsEmployee = null, string PaymentTermCode = null, string CustomerAccountAssignmentGroupCode = null, string ReconcileAccountCode = null, string CompleteYear = null, string CompleteQuarter = null, List<string> HandoverFurniture = null, string Dropdownlist7 = null, string Dropdownlist8 = null, List<string> CompetitorIndustryList = null, int? StartMonth = null, int? StartYear = null, int? EndMonth = null, int? EndYear = null, List<string> OpportunityType = null, List<string> OpportunityStatusTypeList =  null, string LeadUnit = null)
        {
            var _catalogRepository = new CatalogRepository(_context);

            ViewBag.Type = ProfileType;

            #region //isForeignCustomer (Trong nước|Quốc tế)
            var ForeignList = new List<SelectListItem>() {
                new SelectListItem() {
                     Text = LanguageResource.Domestic,
                     Value = false.ToString()
                },
                new SelectListItem() {
                     Text = LanguageResource.Foreign,
                     Value = true.ToString()
                }
            };
            ViewBag.isForeignCustomer = new SelectList(ForeignList, "Value", "Text", isForeignCustomer);
            #endregion

            #region //Get list CustomerType (Phân loại KH: Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.CatalogCode != ConstCustomerType.Contact
                && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi", CustomerTypeCode);

            //Loại hình đối thủ
            var CompetitorTypeList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Competitor_Field
                                                           && p.Actived == true)
                                                           .OrderBy(p => p.OrderIndex)
                                                           .ToList();
            if (ProfileType == ConstProfileType.Competitor)
            {
                ViewBag.Dropdownlist8 = new SelectList(CompetitorTypeList, "CatalogCode", "CatalogText_vi", Dropdownlist8);
            }
            #endregion

           

            #region //Get list Age (Độ tuổi)
            var ageList = _catalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi", Age);
            #endregion

            #region //Get list Other Phone (Số điện thoại khách)
            var morePhoneList = _context.ProfilePhoneModel.Where(p => p.ProfileId == profileId).ToList();
            List<string> phone = new List<string>();
            foreach (var item in morePhoneList)
            {
                phone.Add(item.PhoneNumber);
            }
            ViewBag.Phones = phone;
            #endregion

            #region //Get list Other Email (email khách hàng)
            var moreEmailList = _context.ProfileEmailModel.Where(p => p.ProfileId == profileId).ToList();
            List<string> email = new List<string>();
            foreach (var item in moreEmailList)
            {
                email.Add(item.Email);
            }
            ViewBag.Emails = email;
            #endregion

            #region //Get list Sale Office (Khu vực)
            //var saleOfficeList = _catalogRepository.GetBy(ConstCatalogType.SaleOffice);
            var saleOfficeList = _catalogRepository.GetSaleOffice(isForeignCustomer);
            //ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi", SaleOfficeCode);
            //if (string.IsNullOrEmpty(SaleOfficeCode))
            //{
            //    saleOfficeList = new List<CatalogViewModel>();
            //}
            if (ProfileType == ConstProfileType.Competitor || ProfileType == ConstProfileType.Lead)
            {
                saleOfficeList = _catalogRepository.GetSaleOffice(false).Where(p => p.CatalogCode != "9999").ToList();
            }else if(ProfileType == ConstProfileType.Opportunity)
            {
                saleOfficeList = _catalogRepository.GetOpportunityRegion(false).Where(p => p.CatalogCode != "9999").ToList();
            }
            
            ViewBag.RequiredSaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi", SaleOfficeCode);
            #endregion

            #region //Get list Province (Tỉnh/Thành phố)
            var _provinceRepository = new ProvinceRepository(_context);
            var provinceList = _provinceRepository.GetAll().OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội");
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);
            ViewBag.ProvinceIdSearchList = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
            var provinceAreaList = _context.ProvinceModel.Where(p => p.Actived == true)
                                           .Select(p => new ProvinceViewModel()
                                           {
                                               ProvinceId = p.ProvinceId,
                                               ProvinceCode = p.ProvinceCode,
                                               ProvinceName = p.ProvinceName,
                                               Area = p.Area,
                                               OrderIndex = p.OrderIndex
                                           }).ToList();

            if (!string.IsNullOrEmpty(SaleOfficeCode))
            {
                //int SaleOffice = int.Parse(SaleOfficeCode);
                string SaleOffice = SaleOfficeCode;

                //List<int> areaList = new List<int>();
                //areaList.Add(SaleOffice);

                //if (provinceAreaList != null && provinceAreaList.Count > 0)
                //{
                //    foreach (var item in provinceAreaList)
                //    {
                //        if (!areaList.Contains((int)item.Area))
                //        {
                //            areaList.Add((int)item.Area);
                //        }
                //    }
                //}
                //areaList = areaList.OrderBy(p => p != SaleOffice).ThenBy(p => p).ToList();
                //provinceAreaList = provinceAreaList.OrderBy(p => areaList.FindIndex(p1 => p1 == p.Area))
                //                                   .ThenBy(p => p.ProvinceCode).ToList();

                provinceAreaList = provinceAreaList.Where(p => p.Area == SaleOffice).OrderBy(p => p.ProvinceCode).OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội").ToList();
                ViewBag.RequiredProvinceId = new SelectList(provinceAreaList, "ProvinceId", "ProvinceName", ProvinceId);
            }
            else
            {
                //provinceList = provinceList.OrderBy(p => p.ProvinceCode).ToList();
                //ViewBag.RequiredProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

                provinceAreaList = provinceAreaList.Where(p => p.Area == null).OrderBy(p => p.ProvinceCode).OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội").ToList();
                ViewBag.RequiredProvinceId = new SelectList(provinceAreaList, "ProvinceId", "ProvinceName", ProvinceId);
            }
            #endregion

            #region //Get list District (Quận/Huyện)
            var _districtRepository = new DistrictRepository(_context);
            var districtList = _districtRepository.GetBy(ProvinceId);
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);
            ViewBag.DistrictIdSearchList = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);
            #endregion

            #region //Get list Ward (Phường/Xã)
            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            ViewBag.WardId = new SelectList(wardList, "WardId", "WardName", WardId);
            #endregion

            #region //DayOfBirth (Ngày sinh)
            List<int> DayOfBirthList = new List<int>();
            for (int i = 1; i < 32; i++)
            {
                DayOfBirthList.Add(i);
            }
            ViewBag.DayOfBirth = new SelectList(DayOfBirthList, DayOfBirth);
            #endregion

            #region //MonthOfBith (Tháng sinh)
            List<int> MonthOfBirthList = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                MonthOfBirthList.Add(i);
            }
            ViewBag.MonthOfBirth = new SelectList(MonthOfBirthList, MonthOfBirth);
            #endregion

            #region //Get list CustomerGroup (Nhóm khách hàng doanh nghiệp)
            var customerGroupList = _catalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.CustomerGroupCode = customerGroupList;
            #endregion

            #region //Get list CustomerCareer (Ngành nghề khách hàng doanh nghiệp)
            //var customerCareerList = _context.CatalogModel.Where(
            //       p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer
            //       && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            var customerCareerList = _catalogRepository.GetCustomerCareer(CurrentUser.CompanyCode);
            ViewBag.CustomerCareerCode = new SelectList(customerCareerList, "CatalogCode", "CatalogText_vi", CustomerCareerCode);
            #endregion

            #region //PersonInCharge
            ViewBag.EmployeeList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.PersonRoleCodeList = _unitOfWork.PersonInChargeRepository.GetListPersonRole();
            if (ProfileType == ConstProfileType.Opportunity)
            {
                ViewBag.EmployeeList1 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVKD_Code);
                ViewBag.EmployeeList2 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSalesAdmin_Code);
                ViewBag.EmployeeList3 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSpec_Code);
                ViewBag.PersonInChargeList = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD);
                ViewBag.PersonInCharge2List = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVSalesAdmin);
                ViewBag.PersonInCharge3List = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVSpec);
       
            }
            else
            {
                ViewBag.PersonInChargeList = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD_A);

               

            }
            if (ProfileType == ConstProfileType.Opportunity || ProfileType == ConstProfileType.Lead)
            {
                ViewBag.EmployeeList4 = _unitOfWork.PersonInChargeRepository.GetListEmployee();
                ViewBag.EmployeeList5 = _unitOfWork.PersonInChargeRepository.GetListEmployee();
                ViewBag.PersonInCharge4List = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVMaster);
                ViewBag.PersonInCharge5List = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVTruyCap);
            }
            if (ProfileType == ConstProfileType.Account)
            {
                //NV Tư vấn vật liệu
                ViewBag.EmployeeList6 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVTVVL_Code);
                ViewBag.PersonInCharge6List = _unitOfWork.PersonInChargeRepository.List(profileId, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVTVVL);
            }


            #endregion

            #region //RoleInCharge
            //var listRoles = _repoRoleInCharge.GetRoleList();
            //ViewBag.RoleList = new SelectList(listRoles, "RolesId", "RolesName");
            //ViewBag.RoleList = _unitOfWork.RoleInChargeRepository.GetRoleList();
            //ViewBag.RoleInChargeList = _unitOfWork.RoleInChargeRepository.GetListtRoleByProfileId(profileId);
            #endregion

            #region //Get list CustomerSource (Nguồn khách hàng)
            //Get AddressType
            var srcLst = _catalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(srcLst, "CatalogCode", "CatalogText_vi", CustomerSourceCode);
            #endregion

            #region //SaleOrg
            StoreRepository _storeRepo = new StoreRepository(_context);
            var storeLst = new List<StoreViewModel>();
            ////Nếu là Sửa thì load theo phân quyền
            ////Nếu là Thêm thì load all chi nhánh
            //if (isEditMode == true)
            //{
            //    storeLst = _storeRepo.GetStoreByPermission(CurrentUser.AccountId);
            //}
            //else
            //{
            //    storeLst = _storeRepo.GetAllStore();
            //}

            //Cập nhật: Load chi nhánh theo Phân quyền và Khu vực
            //if (!string.IsNullOrEmpty(SaleOfficeCode))
            //{
            //    storeLst = _storeRepo.GetStoreBySaleOfice(SaleOfficeCode, CurrentUser.AccountId);
            //}
            ////Nếu có giữ liệu cũ có, mà phân quyền không có thì add thêm vào list
            //if (!string.IsNullOrEmpty(SaleOrg))
            //{
            //    if (storeLst == null || storeLst.Where(p => p.SaleOrgCode == SaleOrg).FirstOrDefault() == null)
            //    {
            //        var saleOrgModel = _storeRepo.GetBySaleOrgCode(SaleOrg);
            //        if (saleOrgModel != null)
            //        {
            //            storeLst.Add(new StoreViewModel()
            //            {
            //                SaleOrgCode = saleOrgModel.SaleOrgCode,
            //                StoreName = saleOrgModel.SaleOrgCode + " | " + saleOrgModel.StoreName,
            //            });
            //        }
            //    }
            //}

            //Load chi nhánh theo phân quyền và công ty đang đăng nhập
            //Sắp xếp 1000, 2000, 3000 dưới cùng
            storeLst = _storeRepo.GetStoreByCompanyPermission(CurrentUser.AccountId, CurrentUser.CompanyCode);
            var removeList = (from p in storeLst
                              where p.SaleOrgCode == ConstCompanyCode.AnCuong
                              || p.SaleOrgCode == ConstCompanyCode.Malloca
                              || p.SaleOrgCode == ConstCompanyCode.Aconcept
                              select p).ToList();
            storeLst = (from p in storeLst
                        where p.SaleOrgCode != ConstCompanyCode.AnCuong
                               && p.SaleOrgCode != ConstCompanyCode.Malloca
                               && p.SaleOrgCode != ConstCompanyCode.Aconcept
                        select p).ToList();
            storeLst.AddRange(removeList);

            //Nếu trong list chi nhánh phân quyền không có saleorg -> thêm saleorg vào list
            var saleOrgList = storeLst.Select(p => p.SaleOrgCode).ToList();
            if (!saleOrgList.Contains(SaleOrg))
            {
                var store = (from p in _context.StoreModel
                             where p.SaleOrgCode == SaleOrg
                             select new StoreViewModel()
                             {
                                 StoreId = p.StoreId,
                                 SaleOrgCode = p.SaleOrgCode,
                                 StoreName = p.SaleOrgCode + " | " + p.StoreName,
                                 Area = p.Area,
                                 DefaultCustomerSource = p.DefaultCustomerSource,
                             }).FirstOrDefault();
                if (store != null)
                {
                    storeLst.Add(store);
                    storeLst = storeLst.OrderBy(p => p.SaleOrgCode).ToList();
                }
            }
            ViewBag.CreateAtSaleOrg = new SelectList(storeLst, "SaleOrgCode", "StoreName", SaleOrg);
            #endregion

            #region //title
            var titleLst = _catalogRepository.GetBy(ConstCatalogType.Title)
                                             .Where(p => p.CatalogCode != ConstTitle.Company);
            ViewBag.CustomerTitle = new SelectList(titleLst, "CatalogCode", "CatalogText_vi", Title);
            #endregion

            #region Position (Chức vụ)
            var positionList = _catalogRepository.GetBy(ConstCatalogType.Position);
            ViewBag.ProfileContactPosition = new SelectList(positionList, "CatalogCode", "CatalogText_vi", Position);
            ViewBag.PositionB = new SelectList(positionList, "CatalogCode", "CatalogText_vi", Position);
            #endregion Position (Chức vụ)

            #region Department (Phòng ban)
            var departmentList = _catalogRepository.GetBy(ConstCatalogType.Department);
            ViewBag.DepartmentCode = new SelectList(departmentList, "CatalogCode", "CatalogText_vi", DepartmentCode);
            #endregion Position (Phòng ban)

            #region //Company and Store
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CompanyId);

            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);
            #endregion

            #region //AddressTypeCode (Loại địa chỉ)
            //Get AddressType
            var addressList = _catalogRepository.GetBy(ConstCatalogType.AddressType);
            if (string.IsNullOrEmpty(AddressTypeCode))
            {
                if (ProfileType == ConstProfileType.Opportunity || ProfileType == ConstProfileType.Competitor)
                {
                    AddressTypeCode = "VAT";
                }
            }
            ViewBag.RequiredAddressTypeCode = new SelectList(addressList, "CatalogCode", "CatalogText_vi", AddressTypeCode);
            #endregion

            #region //ProjectStatusCode (Trạng thái dự án)
            var projectStatusLst = _catalogRepository.GetBy(ConstCatalogType.ProjectStatus);
            ViewBag.ProjectStatusCode = new SelectList(projectStatusLst, "CatalogCode", "CatalogText_vi", ProjectStatusCode);
            #endregion

            #region //QualificationLevelCode (Mức độ xác định)
            var qualificationLst = _catalogRepository.GetBy(ConstCatalogType.QualificationLevel);
            ViewBag.QualificationLevelCode = new SelectList(qualificationLst, "CatalogCode", "CatalogText_vi", QualificationLevelCode);
            #endregion

            #region //ProjectSourceCode (Nguồn thông tin)
            var projectSourceLst = _catalogRepository.GetBy(ConstCatalogType.ProjectSource);
            ViewBag.ProjectSourceCode = new SelectList(projectSourceLst, "CatalogCode", "CatalogText_vi", ProjectSourceCode);
            #endregion

            #region //isCreateRequest (Yêu cầu tạo khách ở ECC)
            var isCreateRequestLst = new List<ISDSelectBoolItem>();
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "Không tạo",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = true,
                name = "Đang yêu cầu",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = false,
                name = "Đã tạo",
            });
            ViewBag.isCreateRequest = new SelectList(isCreateRequestLst, "id", "name", isCreateRequest);
            #endregion

            #region //PaymentMethod (Phương thức thanh toán)
            var paymentMethodLst = _catalogRepository.GetBy(ConstCatalogType.PaymentMethod);
            ViewBag.PaymentMethodCode = new SelectList(paymentMethodLst, "CatalogCode", "CatalogText_vi", PaymentMethodCode);
            #endregion

            #region //PartnerFunction (Vai trò trong giao dịch)
            var partnerFunctionLst = _catalogRepository.GetBy(ConstCatalogType.PartnerFunction);
            ViewBag.PartnerFunctionCode = new SelectList(partnerFunctionLst, "CatalogCode", "CatalogText_vi", PartnerFunctionCode);
            #endregion

            #region //Currency (Đơn vị tiền tệ)
            var currencyLst = _catalogRepository.GetBy(ConstCatalogType.Currency);
            ViewBag.CurrencyCode = new SelectList(currencyLst, "CatalogCode", "CatalogText_vi", CurrencyCode);
            #endregion

            #region //TaxClassification (Phân loại thuế VAT)
            var taxClassificationLst = _catalogRepository.GetBy(ConstCatalogType.TaxClassification);
            ViewBag.TaxClassificationCode = new SelectList(taxClassificationLst, "CatalogCode", "CatalogText_vi", TaxClassificationCode);
            #endregion

            #region //PaymentTermCode (Điều khoản thanh toán)
            var paymentTermLst = _catalogRepository.GetBy(ConstCatalogType.PaymentTerm);
            ViewBag.PaymentTermCode = new SelectList(paymentTermLst, "CatalogCode", "CatalogText_vi", PaymentTermCode);
            #endregion

            #region //ReconcileAccount (Tài khoản công nợ)
            var reconcileAccountLst = _catalogRepository.GetBy(ConstCatalogType.ReconcileAccount);
            ViewBag.ReconcileAccountCode = new SelectList(reconcileAccountLst, "CatalogCode", "CatalogText_vi", ReconcileAccountCode);
            #endregion

            #region //ReconcileAccount (Tài khoản doanh thu)
            var customerAccountAssignmentGroupLst = _catalogRepository.GetBy(ConstCatalogType.CustomerAccountAssignmentGroup);
            ViewBag.CustomerAccountAssignmentGroupCode = new SelectList(customerAccountAssignmentGroupLst, "CatalogCode", "CatalogText_vi", CustomerAccountAssignmentGroupCode);
            #endregion

            #region //Manager (Nhân viên quản lý)
            var managerLst = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.Manager = new SelectList(managerLst, "SalesEmployeeCode", "SalesEmployeeName", Manager);
            #endregion

            #region //DebsEmployee (Nhân viên công nợ)
            var debsEmployeeLst = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.DebsEmployee = new SelectList(debsEmployeeLst, "SalesEmployeeCode", "SalesEmployeeName", DebsEmployee);
            #endregion

            #region Năm hoàn thiện
            var completeYear = new List<ISDSelectIntItem>();
            var currentYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++)
            {
                completeYear.Add(new ISDSelectIntItem()
                {
                    id = currentYear,
                    name = currentYear.ToString(),
                });
                currentYear++;
            }
            ViewBag.CompleteYear = new SelectList(completeYear, "id", "name", CompleteYear);

            //Quý
            var completeQuarter = new List<ISDSelectIntItem>();
            for (int i = 1; i < 5; i++)
            {
                completeQuarter.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i.ToString(),
                });
                currentYear++;
            }
            ViewBag.CompleteQuarter = new SelectList(completeQuarter, "id", "name", CompleteQuarter);
            #endregion

            #region Thời gian
            //Tháng
            var monthList = new List<ISDSelectIntItem>();
            for (int i = 1; i <= 12; i++)
            {
                monthList.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i.ToString(),
                });
            }
            ViewBag.StartMonth = new SelectList(monthList, "id", "name", StartMonth);
            ViewBag.EndMonth = new SelectList(monthList, "id", "name", EndMonth);

            //Năm
            var yearList = new List<ISDSelectIntItem>();
            var currentYear2 = DateTime.Now.Year;
            if (EndYear != null)
            {
                yearList.Add(new ISDSelectIntItem()
                {
                    id = EndYear.Value,
                    name = EndYear.ToString()
                });
            }
            for (int i = 0; i < 10; i++)
            {
                if (currentYear2 != EndYear)
                {
                    yearList.Add(new ISDSelectIntItem()
                    {
                        id = currentYear2,
                        name = currentYear2.ToString(),
                    });
                }
                currentYear2++;
            }
            ViewBag.StartYear = new SelectList(yearList.OrderBy(x => x.id).ToList(), "id", "name", StartYear);
            ViewBag.EndYear = new SelectList(yearList.OrderBy(x=>x.id).ToList(), "id", "name", EndYear);
            #endregion

            #region Nội thất bàn giao 
            var handoverFurnitureLst = _catalogRepository.GetBy(ConstCatalogType.HandoverFurniture);
            ViewBag.HandoverFurnitureList = new MultiSelectList(handoverFurnitureLst, "CatalogCode", "CatalogText_vi", HandoverFurniture);
            #endregion

            #region Đánh giá tầm cỡ
            if (ProfileType == ConstProfileType.Competitor)
            {
                var caliberAssessmentList = _catalogRepository.GetBy(ConstCatalogType.CaliberAssessment);
                ViewBag.Dropdownlist7 = new SelectList(caliberAssessmentList, "CatalogCode", "CatalogText_vi", Dropdownlist7);
            }
            #endregion

            #region Thói quen dùng sản phẩm mới
            if (ProfileType == ConstProfileType.Account)
            {
                var habitUsingNewProducts = _catalogRepository.GetBy(ConstCatalogType.HabitUsingNewProducts);
                ViewBag.Dropdownlist7 = new SelectList(habitUsingNewProducts, "CatalogCode", "CatalogText_vi", Dropdownlist7);
            }
            #endregion

            #region Chỉ định dùng vật liệu An Cường
            if (ProfileType == ConstProfileType.Account)
            {
                var indicationsUseACMaterialList = _catalogRepository.GetBy(ConstCatalogType.IndicationsUseACMaterial);
                ViewBag.Dropdownlist8 = new SelectList(indicationsUseACMaterialList, "CatalogCode", "CatalogText_vi", Dropdownlist8);
            }
            #endregion

            #region Danh sách ngành hàng
            var industryLst = _catalogRepository.GetBy(ConstCatalogType.Opportunity_Industry);
            if (industryLst != null && industryLst.Count > 0)
            {
                //Danh sách spec theo cấu hình trong danh mục dùng chung
                var specLst = industryLst.Where(p => p.CatalogText_en.Contains(ConstCatalogType.Spec)).ToList();
                ViewBag.SpecList = specLst;
                //Danh sách spec đã lưu theo profile
                //1. An Cường
                var profileSpecInternalList = (from p in _context.Profile_Opportunity_InternalModel
                                               where p.ProfileId == profileId && p.MappingType == ConstCatalogType.Spec
                                               select new ConstructionViewModel()
                                               {
                                                   OpportunityConstructionId = p.OpportunityInternalId,
                                                   ProfileId = p.ProfileId,
                                                   CatalogCode = p.CatalogCode,
                                                   ProjectValue = p.ProjectValue,
                                                   IsWon = p.IsWon.Value,
                                                   IsChecked = true,
                                               }).ToList();
                if (profileSpecInternalList != null && profileSpecInternalList.Count > 0)
                {
                    //Danh sách mã vật liệu (chọn nhiều) đã lưu theo danh sách spec
                    var profileSpecInternalIdList = profileSpecInternalList.Select(p => p.OpportunityConstructionId).ToList();
                    var SpecInternalMaterialList = (from p in _context.Profile_Opportunity_MaterialModel
                                                    join a in _context.View_Product_ProductInfo on p.MaterialId equals a.MaterialId into aG
                                                    from prod in aG.DefaultIfEmpty()
                                                    where profileSpecInternalIdList.Contains(p.ReferenceId) && p.MaterialType == ConstMaterialType.SpecAnCuong
                                                    select new ISDSelectGuidItem()
                                                    {
                                                        id = p.MaterialId.Value,
                                                        name = prod.ProductCode,
                                                        additionalGuid = p.ReferenceId,
                                                    }).ToList();
                    ViewBag.ProfileSpecInternalMaterialList = SpecInternalMaterialList;
                }
                ViewBag.ProfileSpecInternalList = profileSpecInternalList;
                //2. Đối thủ
                var profileSpecCompetitorList = (from p in _context.Profile_Opportunity_CompetitorModel
                                                 where p.ProfileId == profileId && p.MappingType == ConstCatalogType.Spec
                                                 select new ConstructionViewModel()
                                                 {
                                                     OpportunityConstructionId = p.OpportunityCompetitorId,
                                                     ProfileId = p.ProfileId,
                                                     CatalogCode = p.CatalogCode,
                                                     MaterialCode = p.MaterialCode,
                                                     ProjectValue = p.ProjectValue,
                                                     IsWon = p.IsWon.Value,
                                                 }).ToList();
                if (profileSpecCompetitorList != null && profileSpecCompetitorList.Count > 0)
                {
                    //Danh sách đối thủ (chọn nhiều) đã lưu theo danh sách spec
                    var profileSpecCompetitorIdList = profileSpecCompetitorList.Select(p => p.OpportunityConstructionId).ToList();
                    var SpecCompetitorMappingList = (from p in _context.Profile_Opportunity_CompetitorDetailModel
                                                     join pro in _context.ProfileModel on p.CompetitorId equals pro.ProfileId
                                                     where profileSpecCompetitorIdList.Contains(p.OpportunityCompetitorId)
                                                     select new ISDSelectGuidItem()
                                                     {
                                                         id = p.CompetitorId.Value,
                                                         name = pro.ProfileName,
                                                         additionalGuid = p.OpportunityCompetitorId,
                                                     }).ToList();
                    ViewBag.ProfileSpecCompetitorMappingList = SpecCompetitorMappingList;
                }
                ViewBag.ProfileSpecCompetitorList = profileSpecCompetitorList;
                //Danh sách thi công theo cấu hình
                var constructionLst = industryLst.Where(p => p.CatalogText_en.Contains(ConstCatalogType.Construction)).ToList();
                ViewBag.ConstructionList = constructionLst;
                //Danh sách thi công đã lưu theo profile
                //1. An Cường
                var profileConstructionInternalList = (from p in _context.Profile_Opportunity_InternalModel
                                                       where p.ProfileId == profileId && p.MappingType == ConstCatalogType.Construction
                                                       select new ConstructionViewModel()
                                                       {
                                                           OpportunityConstructionId = p.OpportunityInternalId,
                                                           ProfileId = p.ProfileId,
                                                           CatalogCode = p.CatalogCode,
                                                           ProjectValue = p.ProjectValue,
                                                           IsWon = p.IsWon.Value,
                                                           IsChecked = true,
                                                       }).ToList();
                ViewBag.ProfileConstructionInternalList = profileConstructionInternalList;
                //2. Đối thủ
                var profileConstructionCompetitorList = (from p in _context.Profile_Opportunity_CompetitorModel
                                                         where p.ProfileId == profileId && p.MappingType == ConstCatalogType.Construction
                                                         select new ConstructionViewModel()
                                                         {
                                                             OpportunityConstructionId = p.OpportunityCompetitorId,
                                                             ProfileId = p.ProfileId,
                                                             CatalogCode = p.CatalogCode,
                                                             MaterialCode = p.MaterialCode,
                                                             ProjectValue = p.ProjectValue,
                                                             IsWon = p.IsWon.Value,
                                                         }).ToList();
                if (profileConstructionCompetitorList != null && profileConstructionCompetitorList.Count > 0)
                {
                    //Danh sách đối thủ (chọn nhiều) đã lưu theo danh sách thi công
                    var profileConstructionCompetitorIdList = profileConstructionCompetitorList.Select(p => p.OpportunityConstructionId).ToList();
                    var ConstructionCompetitorMappingList = (from p in _context.Profile_Opportunity_CompetitorDetailModel
                                                             join pro in _context.ProfileModel on p.CompetitorId equals pro.ProfileId
                                                             where profileConstructionCompetitorIdList.Contains(p.OpportunityCompetitorId)
                                                             select new ISDSelectGuidItem()
                                                             {
                                                                 id = p.CompetitorId.Value,
                                                                 name = pro.ProfileName,
                                                                 additionalGuid = p.OpportunityCompetitorId,
                                                             }).ToList();
                    ViewBag.ProfileConstructionCompetitorMappingList = ConstructionCompetitorMappingList;
                }
                ViewBag.ProfileConstructionCompetitorList = profileConstructionCompetitorList;
            }
            #endregion

            #region Danh sách khu vực đối thủ
            var competitorAreaLst = _catalogRepository.GetBy(ConstCatalogType.Competitor_Area);
            ViewBag.CompetitorAreaList = competitorAreaLst;

            //Danh sách khu vực hoạt động đã lưu theo profile
            var profileAreaList = (from p in _context.Profile_Catalog_Mapping
                                   where p.ProfileId == profileId && p.MappingType == ConstCatalogType.Competitor_Area
                                   select new Profile_Catalog_MappingViewModel()
                                   {
                                       CatalogCode = p.CatalogCode,
                                       IsChecked = true,
                                   }).ToList();
            ViewBag.ProfileAreaList = profileAreaList;
            #endregion

            #region Ngành hàng phân phối
            var distributionIndustry = _catalogRepository.GetBy(ConstCatalogType.DistributionIndustry);
            ViewBag.DistributionIndustryList = distributionIndustry;

            //Danh sách ngành hàng phân phối đã lưu theo profile
            var profileDistributionIndustryList = (from p in _context.Profile_Catalog_Mapping
                                                   where p.ProfileId == profileId && p.MappingType == ConstCatalogType.DistributionIndustry
                                                   select new Profile_Catalog_MappingViewModel()
                                                   {
                                                       CatalogCode = p.CatalogCode,
                                                       IsChecked = true,
                                                   }).ToList();
            ViewBag.ProfileDistributionIndustryList = profileDistributionIndustryList;
            #endregion

            #region Phân loại đối thủ
            if (ProfileType == ConstProfileType.Competitor)
            {
                var competitorIndustryLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Opportunity_Industry);
                ViewBag.CompetitorIndustryList = new MultiSelectList(competitorIndustryLst, "CatalogCode", "CatalogText_vi", CompetitorIndustryList);
            }
            #endregion

            #region Dự án
            //Loại hình
            if (ProfileType == ConstProfileType.Opportunity || ProfileType == ConstProfileType.Lead)
            {
                var opportunityTypeList = _catalogRepository.GetBy(ConstCatalogType.OpportunityType);
                opportunityTypeList = (from i in opportunityTypeList
                                       join pTemp in _context.CatalogModel on i.CatalogText_en equals pTemp.CatalogCode into pList
                                       from p in pList.DefaultIfEmpty()
                                       select new CatalogViewModel()
                                       {
                                           CatalogId = i.CatalogId,
                                           CatalogCode = i.CatalogCode,
                                           CatalogText_vi = i.CatalogText_vi,
                                           CatalogText_en = p.CatalogText_vi,
                                           OrderIndex = i.OrderIndex,
                                       }).ToList();

                ViewBag.OpportunityTypeList = new MultiSelectList(opportunityTypeList, "CatalogCode", "CatalogText_vi", "CatalogText_en", OpportunityType);
            }
            //Tình trạng dự án
            if (ProfileType == ConstProfileType.Opportunity)
            {
                var opportunityStatusTypeList = _catalogRepository.GetBy(ConstCatalogType.Opportunity_Status);
                ViewBag.OpportunityStatusTypeList = new MultiSelectList(opportunityStatusTypeList, "CatalogCode", "CatalogText_vi", OpportunityStatusTypeList);
            }
            #endregion
        }

        //ViewBag search for Index
        public void CreateViewBagSearch(string ProfileType = null)
        {
            var _catalogRepository = new CatalogRepository(_context);

            ViewBag.Type = ProfileType;

            #region //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.CatalogCode != ConstCustomerType.Contact
                && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Company and Store
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");
            #endregion

            #region //Get list Age (Độ tuổi)
            var ageList = _catalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list Province (Tỉnh/Thành phố)
            var _provinceRepository = new ProvinceRepository(_context);
            var provinceList = _provinceRepository.GetAll();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName");
            ViewBag.ProvinceIdSearchList = new SelectList(provinceList, "ProvinceId", "ProvinceName");
            #endregion

            #region //Get list CustomerCareer (Ngành nghề khách hàng doanh nghiệp)
            var customerCareerList = _context.CatalogModel.Where(
                   p => p.CatalogTypeCode == ConstCatalogType.CustomerCareer
                   && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerCareerCode = new SelectList(customerCareerList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list CustomerGroup (Nhóm khách hàng doanh nghiệp)
            var customerGroupList = _catalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list SalesEmployee (NV phụ trách)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesEmployeeCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");
            #endregion

            #region //Get list CustomerSource (Nguồn khách hàng)
            //Get AddressType
            var srcLst = _catalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(srcLst, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list CustomerAccountGroup (Phân nhóm khách hàng)
            var customerAccountGroupLst = _catalogRepository.GetCustomerAccountGroup();
            customerAccountGroupLst.Insert(0, new CatalogViewModel()
            {
                CatalogCode = null,
                CatalogText_vi = "Chưa xác định"
            });
            ViewBag.CustomerAccountGroupCode = new SelectList(customerAccountGroupLst, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //isCreateRequest (Yêu cầu tạo khách ở ECC)
            var isCreateRequestLst = new List<ISDSelectBoolItem>();
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "-- Tất cả --",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "Không tạo",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = true,
                name = "Đang yêu cầu",
            });
            isCreateRequestLst.Add(new ISDSelectBoolItem()
            {
                id = false,
                name = "Đã tạo",
            });
            ViewBag.isCreateRequest = new SelectList(isCreateRequestLst, "id", "name");
            #endregion

            #region //Get list CompetitorType (Loại hình đối thủ)
            var competitorTypeList = _catalogRepository.GetBy(ConstCatalogType.Competitor_Field);
            ViewBag.CompetitorType = new SelectList(competitorTypeList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list SaleOffice (Khu vực)
            var saleOfficeList = _catalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region Filters
            var filterLst = new List<DropdownlistFilter>();
            if (ProfileType == ConstProfileType.Competitor)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.ProfileCode, FilterName = LanguageResource.Profile_ProfileCode });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Phone, FilterName = LanguageResource.Profile_Phone });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Address, FilterName = LanguageResource.Profile_Address });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Email, FilterName = "Email" });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.DistrictId, FilterName = LanguageResource.Profile_DistrictId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.WardId, FilterName = LanguageResource.WardId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CommonCreateDate });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
            }
            else if (ProfileType == ConstProfileType.Lead)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.ProvinceId, FilterName = LanguageResource.Profile_ProvinceId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.DistrictId, FilterName = LanguageResource.Profile_DistrictId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.WardId, FilterName = LanguageResource.WardId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CommonCreateDate });
            }
            else
            {
                if (ProfileType == ConstProfileType.Account)
                {
                    //filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SearchProfileForeignCode, FilterName = LanguageResource.Profile_ProfileForeignCode });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerTypeCode, FilterName = LanguageResource.Profile_CustomerTypeCode });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CompanyId, FilterName = LanguageResource.Profile_CompanyId });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.StoreId, FilterName = LanguageResource.MasterData_Store });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaxNo, FilterName = LanguageResource.Profile_TaxNo });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerAccountGroupCode, FilterName = LanguageResource.Profile_CustomerAccountGroup });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.isCreateRequest, FilterName = LanguageResource.Profile_isCreateRequest });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.IsTopInvestor, FilterName = LanguageResource.Profile_IsTopInvestor });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.IsInvestor, FilterName = LanguageResource.Profile_IsInvestor });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.IsDesigner, FilterName = LanguageResource.Profile_IsDesigner });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.IsContractor, FilterName = LanguageResource.Profile_IsContractor });

                }
                else
                {
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Address, FilterName = LanguageResource.Profile_Address });
                }

                if (ProfileType != ConstProfileType.Opportunity)
                {
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerGroupCode, FilterName = LanguageResource.Profile_CustomerCategoryCode });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerCareerCode, FilterName = LanguageResource.Profile_CustomerCareerCode });
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerSourceCode, FilterName = LanguageResource.Profile_CustomerSourceCode }); 
                    filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Email, FilterName = "Email" });

                }
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
                //filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.ProvinceId, FilterName = LanguageResource.Profile_ProvinceId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.DistrictId, FilterName = LanguageResource.Profile_DistrictId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.WardId, FilterName = LanguageResource.WardId });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SalesEmployeeCode, FilterName = LanguageResource.PersonInCharge });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.RolesCode, FilterName = LanguageResource.Profile_Department });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CommonCreateDate });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Actived, FilterName = LanguageResource.Actived });
            }
            ViewBag.Filters = filterLst;
            #endregion
        }

        //ViewBag search for modal popup Profile
        public void CreateViewBagSearchPopup(string CustomerTypeCode = null, string ProfileType = null)
        {
            var _catalogRepository = new CatalogRepository(_context);

            ViewBag.Type = ProfileType;

            #region //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.CatalogCode != ConstCustomerType.Contact
                && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi", CustomerTypeCode);
            #endregion
        }
        #endregion

        #region Helper
        /// <summary>
        /// Lấy dữ liệu đối thủ autocomplete theo tên
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public JsonResult SearchCompetitorBy(string search, string type)
        {
            var slResult = (from p in _context.ProfileModel
                            join a in _context.Profile_ProfileType_Mapping on p.ProfileId equals a.ProfileId into mapG
                            from pt in mapG.DefaultIfEmpty()
                            join b in _context.Competitor_Industry_Mapping on p.ProfileId equals b.CompetitorId
                            where (p.CustomerTypeCode == ConstProfileType.Competitor || pt.ProfileType == ConstProfileType.Competitor)
                                    && (search == null || p.ProfileName.Contains(search) || p.ProfileShortName.Contains(search)
                                  )
                                  //search theo phân loại đối thủ
                                  && (b.IndustryCode == type)
                            orderby p.ProfileCode descending
                            select new
                            {
                                value = p.ProfileId,
                                text = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileName
                            }).Take(10).ToList();
            return Json(slResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách ngành hàng theo loại hình đối thủ
        /// </summary>
        /// <param name="CompetitorType"></param>
        /// <returns></returns>
        public ActionResult GetDistributionIndustryBy(string CompetitorType, Guid? ProfileId)
        {
            var _catalogRepository = new CatalogRepository(_context);
            var result = _catalogRepository.GetDistributionIndustryBy(CompetitorType);
            //Nếu có truyền ProfileId thì lấy danh sách ngành hàng đã lưu
            var profileDistributionList = new List<Profile_Catalog_MappingViewModel>();
            if (ProfileId.HasValue)
            {
                profileDistributionList = _context.Profile_Catalog_Mapping.Where(p => p.MappingType == ConstCatalogType.DistributionIndustry && p.ProfileId == ProfileId)
                                                  .Select(p => new Profile_Catalog_MappingViewModel()
                                                  {
                                                      CatalogCode = p.CatalogCode,
                                                  }).ToList();
            }
            return Json(new { DistributionList = result, ProfileDistributionList = profileDistributionList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy dữ liệu KH autocomplete theo ProfileCode
        /// </summary>
        /// <param name="ProfileCode"></param>
        /// <returns></returns>
        public JsonResult GetProfileByCode(string ProfileCode)
        {
            var slResult = (from p in _context.ProfileModel
                            where p.CustomerTypeCode == ConstCustomerType.Account 
                            && (p.ProfileCode.ToString().Contains(ProfileCode) || p.ProfileName.Contains(ProfileCode) || p.ProfileShortName.Contains(ProfileCode))
                            //&& p.Actived == true
                            orderby p.ProfileCode ascending
                            select new
                            {
                                ProfieLableName = p.ProfileCode + " | " + p.ProfileName,
                                ProfileCode = p.ProfileCode,
                                ProfileName = p.ProfileName,
                                ProfileId = p.ProfileId
                            }).Take(10).ToList();
            return Json(slResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy phòng ban theo nhân viên
        /// </summary>
        /// <param name="SalesEmployeeCode"></param>
        /// <returns></returns>
        public ActionResult GetRoleBySaleEmployee(string SalesEmployeeCode)
        {
            var role = (from p in _context.AccountModel
                        from m in p.RolesModel
                        where p.EmployeeCode == SalesEmployeeCode
                        && m.isEmployeeGroup == true
                        select m.RolesName).FirstOrDefault();

            role = role != null ? role : "";
            return Json(role, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách khu vực theo Đối tượng (trong nước/nước ngoài)
        /// </summary>
        /// <param name="isForeignCustomer"></param>
        /// <returns></returns>
        public ActionResult GetSaleOfficeBy(bool? isForeignCustomer)
        {
            var _catalogRepository = new CatalogRepository(_context);
            var saleOfficeList = _catalogRepository.GetSaleOffice(isForeignCustomer);
            var lst = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOpportunityRegionBy(bool? isForeignCustomer)
        {
            var _catalogRepository = new CatalogRepository(_context);
            var saleOfficeList = _catalogRepository.GetOpportunityRegion(isForeignCustomer);
            var lst = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách tỉnh thành theo khu vực
        /// </summary>
        /// <param name="SaleOfficeCode"></param>
        /// <returns></returns>
        public ActionResult GetProvinceBy(string SaleOfficeCode)
        {
            //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
            var provinceList = new List<ProvinceViewModel>();
            provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                       .Select(p => new ProvinceViewModel()
                                       {
                                           ProvinceId = p.ProvinceId,
                                           ProvinceCode = p.ProvinceCode,
                                           ProvinceName = p.ProvinceName,
                                           Area = p.Area,
                                           OrderIndex = p.OrderIndex
                                       }).ToList();

            if (!string.IsNullOrEmpty(SaleOfficeCode) && SaleOfficeCode != ConstSaleOffice.Khac)
            {
                //int SaleOffice = int.Parse(SaleOfficeCode);
                string SaleOffice = SaleOfficeCode;

                //List<int> areaList = new List<int>();
                //areaList.Add(SaleOffice);

                //if (provinceList != null && provinceList.Count > 0)
                //{
                //    foreach (var item in provinceList)
                //    {
                //        if (!areaList.Contains((int)item.Area))
                //        {
                //            areaList.Add((int)item.Area);
                //        }
                //    }
                //}
                //areaList = areaList.OrderBy(p => p != SaleOffice).ThenBy(p => p).ToList();
                //provinceList = provinceList.OrderBy(p => areaList.FindIndex(p1 => p1 == p.Area)).ThenBy(p => p.ProvinceCode).ToList();

                var existList = provinceList.Select(p => p.Area).ToList();
                if (!existList.Contains(SaleOffice))
                {
                    provinceList = provinceList.Where(p => p.Area == null).OrderBy(p => p.ProvinceCode).ToList();
                }
                else
                {
                    provinceList = provinceList.Where(p => p.Area == SaleOffice).OrderBy(p => p.ProvinceCode).ToList();
                }
            }
            else
            {
                //provinceList = provinceList.OrderBy(p => p.ProvinceCode).ToList();

                provinceList = provinceList.Where(p => p.Area == null).OrderBy(p => p.ProvinceCode).ToList();
            }

            //Load cửa hàng theo Phân quyền và Khu vực
            var saleOrgList = new List<StoreViewModel>();
            if (!string.IsNullOrEmpty(SaleOfficeCode))
            {
                saleOrgList = _unitOfWork.StoreRepository.GetStoreBySaleOfice(SaleOfficeCode, CurrentUser.AccountId);
            }

            return Json(new { provinceList = provinceList.OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội"), saleOrgList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy các field theo Profile config và tạo dynamic ViewBag
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="isCreateViewBag"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public List<ProfileConfigModel> GetProfileConfig(string Type, bool? isCreateViewBag = false, ProfileViewModel viewModel = null)
        {
            List<ProfileConfigModel> configList = new List<ProfileConfigModel>();
            configList = _context.ProfileConfigModel.Where(p => p.ProfileCategoryCode == Type).ToList();
            if (configList != null && configList.Count > 0)
            {
                foreach (var item in configList)
                {
                    if (string.IsNullOrEmpty(item.Note))
                    {
                        item.Note = PropertyHelper.GetDisplayNameByString<ProfileViewModel>(item.FieldCode);
                    }
                    //CreateViewBag
                    if (!string.IsNullOrEmpty(item.Parameters) && isCreateViewBag == true)
                    {
                        var lst = _unitOfWork.CatalogRepository.GetBy(item.Parameters);
                        object value = null;
                        if (viewModel != null)
                        {
                            var propertyName = _unitOfWork.ProfileRepository.GetPropertyNameByParameter(item.ProfileCategoryCode, item.Parameters);
                            value = viewModel.GetType().GetProperty(propertyName).GetValue(viewModel, null);
                        }
                        var objectData = new SelectList(lst, "CatalogCode", "CatalogText_vi", value);
                        ViewData.Add(item.Parameters, objectData);
                    }
                }
            }
            if (isCreateViewBag == true)
            {
                ViewBag.ProfileConfig = configList;
                ViewBag.ProfileConfigCode = configList.Select(p => p.FieldCode).ToList();
            }
            return configList;
        }

        /// <summary>
        /// Đồng bộ từ SAP mã khách hàng mới, chưa tồn tại trong hệ thống
        /// </summary>
        /// <param name="ProfileForeignCode"></param>
        /// <returns></returns>
        public ActionResult SyncProfile(string ProfileForeignCode)
        {
            bool Success = true;
            string Message = string.Empty;
            try
            {
                //var exist = _context.ProfileModel.FirstOrDefault(p => p.ProfileForeignCode == ProfileForeignCode);
                //if (exist == null)
                //{
                //    var result = _unitOfWork.ProfileRepository.SyncProfile(ProfileForeignCode, out Message);
                //}
                //else
                //{
                //    Message = string.Format("Khách hàng có mã \"{0}\" đã tồn tại trong hệ thống!", ProfileForeignCode);
                //}
                var result = _unitOfWork.ProfileRepository.SyncProfile(ProfileForeignCode, out Message);
                var profile = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                if (profile != null)
                {
                    #region sync_Profile_ExtendInfo_To_Table
                    _unitOfWork.ProfileRepository.sync_Profile_ExtendInfo_To_Table(profile.ProfileId);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Message = ex.InnerException.InnerException.Message;
                    }
                }
            }
            return Json(new { Success, Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách khách hàng theo nhóm: chủ đầu tư, thiết kế, tổng thầu, ...
        /// </summary>
        /// <param name="type">Nhóm khách hàng</param>
        /// <param name="type2">Nhóm khách hàng khác</param>
        /// <returns></returns>
        public ActionResult SearchProfileByType(string type, string search, string type2 = null)
        {
            var result = new List<ISDSelectItem>();
            result = (from a in _context.ProfileModel
                      //join b in _context.ProfileGroupModel on a.ProfileId equals b.ProfileId
                      where //a.CompanyCode == CurrentUser.CompanyCode 
                      //&& //b.ProfileGroupCode == type
                      (type !=ConstProfileType.IsInvestor || a.IsInvestor == true)
                      &&
                      (type != ConstProfileType.IsDesigner || a.IsDesigner == true)
                      &&
                      (type != ConstProfileType.IsContractor || a.IsContractor == true)
                      //loại khác
                      //|| (type2 == null || b.ProfileGroupCode == type2))
                      //tìm theo tên
                      && (search == null || (a.ProfileName.Contains(search) || a.ProfileShortName.Contains(search)))
                      //trạng thái đang hoạt động
                      && a.Actived == true
                      orderby a.ProfileName
                      select new ISDSelectItem()
                      {
                          value = a.ProfileId,
                          text = a.ProfileShortName + " | " + a.ProfileName
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchProfileByCode(string type, string search, string type2 = null)
        {
            var result = new List<ISDSelectItem2>();
            result = (from a in _context.ProfileModel
                          //join b in _context.ProfileGroupModel on a.ProfileId equals b.ProfileId
                      where 
                      //trạng thái đang hoạt động
                      a.Actived == true
                      && a.ProfileCode.ToString().Contains(search)
                      orderby a.ProfileCode
                      select new ISDSelectItem2()
                      {
                          value = a.ProfileCode.ToString(),
                          text = a.ProfileCode.ToString()
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách đối thủ (dùng cho autocomplete)
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCompetitor(string search)
        {
            var result = new List<ISDSelectItem>();
            result = (from a in _context.ProfileModel
                      join b in _context.Profile_ProfileType_Mapping on a.ProfileId equals b.ProfileId into bTemp
                      from map in bTemp.DefaultIfEmpty()
                      where (a.CustomerTypeCode == ConstProfileType.Competitor || map.ProfileType == ConstProfileType.Competitor)
                      //tìm theo tên
                      && (search == null || (a.ProfileName.Contains(search) || a.ProfileShortName.Contains(search)))
                      //trạng thái đang hoạt động
                      && a.Actived == true
                      orderby a.ProfileName
                      select new ISDSelectItem()
                      {
                          value = a.ProfileId,
                          text = !string.IsNullOrEmpty(a.ProfileShortName) ? a.ProfileShortName : a.ProfileName
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// Lấy danh sách dự án (dùng cho autocomplete)
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchOpportunity(string search)
        {
            var result = new List<ISDSelectItem>();
            result = (from a in _context.ProfileModel
                      join b in _context.Profile_ProfileType_Mapping on a.ProfileId equals b.ProfileId into bTemp
                      from map in bTemp.DefaultIfEmpty()
                      where (a.CustomerTypeCode == ConstProfileType.Opportunity || map.ProfileType == ConstProfileType.Opportunity)
                      //tìm theo tên
                      && (search == null || (a.ProfileName.Contains(search) || a.ProfileShortName.Contains(search)))
                      //trạng thái đang hoạt động
                      && a.Actived == true
                      orderby a.ProfileName
                      select new ISDSelectItem()
                      {
                          value = a.ProfileId,
                          text = !string.IsNullOrEmpty(a.ProfileShortName) ? a.ProfileShortName : a.ProfileName
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region Lấy thông tin khách hàng qua đối thủ
        /// <summary>
        /// Kéo thông tin khách hàng qua đối thủ
        /// </summary>
        /// <param name="ProfileCode"></param>
        /// <returns></returns>
        public ActionResult ExtendAccount(string ProfileCode)
        {
            bool Success = true;
            string Message = string.Empty;
            try
            {
                Success = _unitOfWork.ProfileRepository.ExtendAccount(ProfileCode, CurrentUser.AccountId, out Message);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Message = ex.InnerException.InnerException.Message;
                    }
                }
            }
            return Json(new { Success, Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Remote Validation
        #region Phone
        private bool IsExistsPhone(string PhoneNumber, string TypeCode)
        {
            var result = false;
            result = _context.ProfileModel.FirstOrDefault(p => p.Phone == PhoneNumber && p.CustomerTypeCode == TypeCode) != null;
            if (!result)
            {
                result = _context.ProfilePhoneModel.FirstOrDefault(p => p.PhoneNumber == PhoneNumber) != null;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingPhone(string Phone, string PhoneValid, string TypeCode)
        {
            try
            {
                if (PhoneValid != Phone && !string.IsNullOrEmpty(Phone))
                {
                    return Json(!IsExistsPhone(Phone, TypeCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingProfileContactPhone(string ProfileContactPhone, string ProfileContactPhoneValid, string ProfileContactTypeCode)
        {
            try
            {
                if (ProfileContactPhoneValid != ProfileContactPhone)
                {
                    return Json(!IsExistsPhone(ProfileContactPhone, ProfileContactTypeCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion

        #region email
        private bool IsExistsEmail(string Email, string TypeCode)
        {
            return (_context.ProfileModel.FirstOrDefault(p => p.Email == Email && p.CustomerTypeCode == TypeCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingEmail(string Email, string EmailValid, string TypeCode)
        {
            try
            {
                if (EmailValid != Email)
                {
                    return Json(!IsExistsEmail(Email, TypeCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingProfileContactEmail(string ProfileContactEmail, string ProfileContactEmailValid, string ProfileContactTypeCode)
        {
            try
            {
                if (ProfileContactEmailValid != ProfileContactEmail)
                {
                    return Json(!IsExistsEmail(ProfileContactEmail, ProfileContactTypeCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion

        #region TaxNo
        private bool IsExistsTaxNo(string TaxNo)
        {
            return (_context.ProfileBAttributeModel.FirstOrDefault(p => p.TaxNo == TaxNo) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingTaxNo(string TaxNo, string TaxNoValid)
        {
            try
            {
                if (TaxNoValid != TaxNo)
                {
                    return Json(!IsExistsTaxNo(TaxNo));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion

        #region Address
        private bool IsExistsAddress(string Address, string TypeCode)
        {
            var result = false;
            result = _context.ProfileModel.FirstOrDefault(p => p.Address == Address && p.CustomerTypeCode == TypeCode) != null;
            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingAddress(string Address, string AddressValid, string Type)
        {
            try
            {
                if (AddressValid != Address && !string.IsNullOrEmpty(Address.Trim()))
                {
                    return Json(!IsExistsAddress(Address, Type));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
        #endregion

        #region Profile Search in Model popup
        public ActionResult _ProfileBSearch()
        {
            ProfileBSearchViewModel model = new ProfileBSearchViewModel();
            CreateViewBag();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _ProfileBSearchResult(ProfileBSearchViewModel searchModel)
        {
            return ExecuteSearch(() =>
            {
                var profiles = (from p in _context.ProfileModel
                                join b in _context.ProfileBAttributeModel on p.ProfileId equals b.ProfileId
                                join c in _context.CatalogModel on p.CustomerTypeCode equals c.CatalogCode
                                orderby p.CreateTime
                                where
                                c.CatalogTypeCode == ConstCatalogType.CustomerType &&
                                //search by ProfileName
                                (searchModel.ProfileNameSearch == null || p.ProfileName.Contains(searchModel.ProfileNameSearch))
                                //Search by Phone
                                && (searchModel.ProfilePhoneSearch == null || p.Phone.Contains(searchModel.ProfilePhoneSearch))
                                //Search by TaxNo
                                && (searchModel.TaxNoSearch == null || b.TaxNo.Contains(searchModel.TaxNoSearch))
                                //Search by Email
                                && (searchModel.ProfileEmailSearch == null || p.Email.Contains(searchModel.ProfileEmailSearch))
                                //Search by Province
                                && (searchModel.ProvinceIdSearchList == null || p.ProvinceId == searchModel.ProvinceIdSearchList)
                                //Search by District
                                && (searchModel.DistrictIdSearchList == null || p.DistrictId == searchModel.DistrictIdSearchList)
                                //search by catalogcode = "B"
                                && (p.CustomerTypeCode == "B")
                                && p.Actived == true
                                select new ProfileViewModel()
                                {
                                    ProfileId = p.ProfileId,
                                    ProfileName = p.ProfileName,
                                    Address = p.Address,
                                    TaxNo = b.TaxNo,
                                }).ToList();

                return PartialView(profiles);
            });
        }

        //Customer and business
        public ActionResult _ProfileSearch(ProfileSearchViewModel model)
        {
            if (model == null)
            {
                model = new ProfileSearchViewModel();
            }
            CreateViewBagSearchPopup(ProfileType: ConstProfileType.Account);

            return PartialView(model);
        }

        [HttpPost]
        //Hàm tìm kiếm dành cho popup
        public ActionResult _ProfileSearchResult(ProfileSearchViewModel searchViewModel, DatatableViewModel model)
        {
            return ExecuteSearch(() =>
            {
                //Customer and Business => Type = Account
                if (string.IsNullOrEmpty(searchViewModel.ProfileType))
                {
                    searchViewModel.ProfileType = ConstProfileType.Account;
                }
                //Nếu là "Dự án" thì tìm theo:
                if (searchViewModel.ProfileType == ConstProfileType.Account)
                {
                    //Là thiết kế
                    if (searchViewModel.ProfileType2 == ConstProfileType.IsDesigner)
                    {
                        searchViewModel.IsDesigner = true;
                    }
                    //Là tổng thầu
                    if (searchViewModel.ProfileType2 == ConstProfileType.IsContractor)
                    {
                        searchViewModel.IsContractor = true;
                    }
                    //Là chủ đầu tư
                    if (searchViewModel.ProfileType2 == ConstProfileType.IsInvestor)
                    {
                        searchViewModel.IsInvestor = true;
                    }
                }
                
                //if (searchViewModel.ProfileType == ConstProfileType.Contact)
                //{
                //    searchViewModel.Type = ConstProfileType.Contact;
                //}
                //else
                //{
                //    searchViewModel.Type = ConstProfileType.Account;
                //}
                searchViewModel.Type = searchViewModel.ProfileType;

                int filteredResultsCount;
                int totalResultsCount = model.length;
                //Page Size 
                searchViewModel.PageSize = model.length;
                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;
                //SaleOrg
                if (searchViewModel.StoreId == null || searchViewModel.StoreId.Count == 0)
                {
                    //var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
                    //if (storeList != null && storeList.Count > 0)
                    //{
                    //    searchViewModel.StoreId = new List<Guid?>();
                    //    searchViewModel.StoreId = storeList.Select(p => (Guid?)p.StoreId).ToList();
                    //}
                }

                ProfileRepository repo = new ProfileRepository(_context);
                //var query = repo.SearchQuery(searchViewModel);

                //var res = CustomSearchRepository.CustomSearchFunc<ProfileSearchResultViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
                var res = repo.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);
                if (res != null && res.Count() > 0)
                {
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                        //item.Address = string.Format("{0}{1}{2}", item.Address, item.DistrictName, item.ProvinceName);
                        if (!string.IsNullOrEmpty(item.Address) && item.Address.StartsWith(","))
                        {
                            item.Address = item.Address.Remove(0, 1).Trim();
                        }
                    }
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = res,
                });
            });
        }
        #endregion Company Info List

        //Export
        #region Export to excel (old)
        //public ActionResult ExportExcel(ProfileSearchViewModel searchViewModel)
        //{

        //    //ProfileRepository repo = new ProfileRepository(_context);
        //    //var profiles = repo.Search(searchViewModel);

        //    //searchViewModel = (ProfileSearchViewModel) Session["frmSearchProfile"];

        //    //Get data filter
        //    //Get data from server

        //    var profiles = (from p in _context.ProfileModel
        //                    join c in _context.CatalogModel on p.CustomerTypeCode equals c.CatalogCode
        //                    join a in _context.AccountModel on p.CreateBy equals a.AccountId
        //                    orderby p.CreateTime
        //                    where
        //                    c.CatalogTypeCode == ConstCatalogType.CustomerType &&
        //                    //search by ProfileName
        //                    (searchViewModel.ProfileName == null || p.ProfileName == searchViewModel.ProfileName)
        //                    //Search by Phone
        //                    && (searchViewModel.Phone == null || p.Phone == searchViewModel.Phone)
        //                    //search by catalogcode
        //                    && (searchViewModel.CustomerTypeCode == null || p.CustomerTypeCode == searchViewModel.CustomerTypeCode)
        //                    //Search by Actived
        //                    && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
        //                    select new ProfileExcelViewModel()
        //                    {
        //                        //ProfileId = p.ProfileId,
        //                        CustomerTypeCode = c.CatalogText_vi,
        //                        ProfileName = p.ProfileName,
        //                        Phone = p.Phone,
        //                        Email = p.Email,
        //                        Actived = p.Actived,
        //                        Address = p.Address,
        //                        CreateBy = a.FullName,
        //                        CreateTime = p.CreateTime,
        //                    }).ToList();

        //    return Export(profiles);
        //}

        //[ISDAuthorizationAttribute]
        //public FileContentResult Export(List<ProfileExcelViewModel> viewModel)
        //{
        //    #region Dropdownlist
        //    //Tỉnh thành
        //    //List<DropdownModel> ProvinceId = (from c in _context.ProvinceModel
        //    //                                  where c.Actived == true
        //    //                                  orderby c.Area, c.ProvinceName
        //    //                                  select new DropdownModel()
        //    //                                  {
        //    //                                      Id = c.ProvinceId,
        //    //                                      Name = c.ProvinceName,
        //    //                                  }).ToList();

        //    ////Loại quận huyện
        //    //List<DropdownIdTypeStringModel> Appellation = new List<DropdownIdTypeStringModel>();
        //    //Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.ThanhPho, Name = ConstAppellation.ThanhPho });
        //    //Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.Quan, Name = ConstAppellation.Quan });
        //    //Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.Huyen, Name = ConstAppellation.Huyen });
        //    //Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.ThiXa, Name = ConstAppellation.ThiXa });
        //    #endregion Dropdownlist

        //    #region Master
        //    List<ExcelTemplate> columns = new List<ExcelTemplate>();

        //    //columns.Add(new ExcelTemplate() { ColumnName = "ProfileId", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate() { ColumnName = "CustomerTypeCode", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "ProfileName", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "Phone", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "Email", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "Address", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "CreateBy", isAllowedToEdit = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "CreateTime", isAllowedToEdit = true, isDateTime = true });
        //    columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });

        //    #endregion Master

        //    //Header
        //    string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Customer_Profiles);
        //    //List<ExcelHeadingTemplate> heading initialize in BaseController
        //    //Default:
        //    //          1. heading[0] is controller code
        //    //          2. heading[1] is file name
        //    //          3. headinf[2] is warning (edit)
        //    heading.Add(new ExcelHeadingTemplate()
        //    {
        //        Content = controllerCode,
        //        RowsToIgnore = 1,
        //        isWarning = false,
        //        isCode = true
        //    });
        //    heading.Add(new ExcelHeadingTemplate()
        //    {
        //        Content = fileheader.ToUpper(),
        //        RowsToIgnore = 1,
        //        isWarning = false,
        //        isCode = false
        //    });
        //    heading.Add(new ExcelHeadingTemplate()
        //    {
        //        Content = LanguageResource.Export_ExcelWarning1,
        //        RowsToIgnore = 0,
        //        isWarning = true,
        //        isCode = false
        //    });
        //    heading.Add(new ExcelHeadingTemplate()
        //    {
        //        Content = LanguageResource.Export_ExcelWarning2,
        //        RowsToIgnore = 0,
        //        isWarning = true,
        //        isCode = false
        //    });

        //    //Trạng thái
        //    heading.Add(new ExcelHeadingTemplate()
        //    {
        //        Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.MasterData_District),
        //        RowsToIgnore = 1,
        //        isWarning = true,
        //        isCode = false
        //    });

        //    //Body
        //    byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);
        //    //File name
        //    //Insert => THEM_MOI
        //    //Edit => CAP_NHAT
        //    //string exportType = LanguageResource.exportType_Insert;
        //    //if (isEdit == true)
        //    //{
        //    //    exportType = LanguageResource.exportType_Edit;
        //    //}
        //    string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

        //    return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        //}
        #endregion Export to excel

        #region Export to excel
        public ActionResult ExportExcel(ProfileSearchViewModel searchViewModel)
        {
            List<ProfileExcelViewModel> profile = new List<ProfileExcelViewModel>();
            return Export(profile);
        }

        [ISDAuthorization]
        public FileContentResult Export(List<ProfileExcelViewModel> profile)
        {
            #region Dropdownlist

            #region //Phân loại KH
            List<DropdownIdTypeStringModel> CustomerTypeCode = new List<DropdownIdTypeStringModel>();
            var customerTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            if (customerTypeCode != null && customerTypeCode.Count > 0)
            {
                CustomerTypeCode = customerTypeCode.Where(p => p.CatalogCode != ConstCustomerType.Contact).Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Phân loại KH

            #region //Nguồn KH
            List<DropdownIdTypeStringModel> CustomerSourceCode = new List<DropdownIdTypeStringModel>();
            var customerSourceCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            if (customerSourceCode != null && customerSourceCode.Count > 0)
            {
                CustomerSourceCode = customerSourceCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nguồn KH      

            #region //Danh xưng
            List<DropdownIdTypeStringModel> Title = new List<DropdownIdTypeStringModel>();
            var title = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Title);
            if (title != null && title.Count > 0)
            {
                Title = title.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Danh xưng 

            #region //Độ tuổi
            List<DropdownIdTypeStringModel> Age = new List<DropdownIdTypeStringModel>();
            var age = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            if (age != null && age.Count > 0)
            {
                Age = age.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Độ tuổi 

            #region //Khu vực
            List<DropdownIdTypeStringModel> SaleOfficeCode = new List<DropdownIdTypeStringModel>();
            var saleOfficeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            if (saleOfficeCode != null && saleOfficeCode.Count > 0)
            {
                SaleOfficeCode = saleOfficeCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Khu vực

            #region //Tỉnh/thành
            List<DropdownModel> ProvinceId = new List<DropdownModel>();
            ProvinceRepository _repoProvince = new ProvinceRepository(_context);
            var province = _repoProvince.GetAll();
            if (province != null && province.Count > 0)
            {
                ProvinceId = province.Select(p => new DropdownModel()
                {
                    Id = p.ProvinceId,
                    Name = p.ProvinceName,
                }).ToList();
            }
            #endregion //Tỉnh/thành

            #region //Quận/huyện
            List<DropdownModel> DistrictId = new List<DropdownModel>();
            DistrictRepository _repoDistrict = new DistrictRepository(_context);
            var district = _repoDistrict.GetAll();
            if (district != null && district.Count > 0)
            {
                DistrictId = district.Select(p => new DropdownModel()
                {
                    Id = p.DistrictId,
                    Name = p.DistrictName,
                    ParentLevel1Id = p.ProvinceId,
                    ParentLevel1Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Quận/huyện

            #region //Phường/xã
            List<DropdownModel> WardId = new List<DropdownModel>();
            WardRepository _repoWard = new WardRepository(_context);
            var ward = _repoWard.GetAll();
            if (ward != null && ward.Count > 0)
            {
                WardId = ward.Select(p => new DropdownModel()
                {
                    Id = p.WardId,
                    Name = p.WardName,
                    ParentLevel1Id = p.DistrictId,
                    ParentLevel1Name = p.DistrictName,
                    ParentLevel2Id = p.ProvinceId,
                    ParentLevel2Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Phường/xã

            #region //Chi nhánh
            //Lấy chi nhánh theo phân quyền
            List<DropdownIdTypeStringModel> CreateAtSaleOrg = new List<DropdownIdTypeStringModel>();
            var saleOrg = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
            if (saleOrg != null && saleOrg.Count > 0)
            {
                CreateAtSaleOrg = saleOrg.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SaleOrgCode,
                    Name = p.StoreName,
                }).ToList();
            }
            #endregion //Chi nhánh

            #region //Phòng ban
            List<DropdownIdTypeStringModel> DepartmentCode = new List<DropdownIdTypeStringModel>();
            var department = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Department);
            if (department != null && department.Count > 0)
            {
                DepartmentCode = department.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Phòng ban

            #region //Chức vụ
            List<DropdownIdTypeStringModel> Position = new List<DropdownIdTypeStringModel>();
            var position = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Position);
            if (position != null && position.Count > 0)
            {
                Position = position.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Chức vụ

            #region //Ngành nghề
            List<DropdownIdTypeStringModel> CustomerCareer = new List<DropdownIdTypeStringModel>();
            var career = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCareer);
            if (career != null && career.Count > 0)
            {
                CustomerCareer = career.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Ngành nghề

            #region //Nhóm KH
            List<DropdownIdTypeStringModel> CustomerGroup = new List<DropdownIdTypeStringModel>();
            var customerGroup = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            if (customerGroup != null && customerGroup.Count > 0)
            {
                CustomerGroup = customerGroup.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nhóm KH

            #region //Đối tượng
            List<DropdownIdTypeBoolModel> ForeignCustomer = new List<DropdownIdTypeBoolModel>();
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = false,
                Name = LanguageResource.Domestic,
            });
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = true,
                Name = LanguageResource.Foreign,
            });
            #endregion //Đối tượng

            #region //NV phụ trách
            List<DropdownIdTypeStringModel> PersonInCharge = new List<DropdownIdTypeStringModel>();
            var personInCharge = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            if (personInCharge != null && personInCharge.Count > 0)
            {
                PersonInCharge = personInCharge.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SalesEmployeeCode,
                    Name = p.SalesEmployeeName,
                }).ToList();
            }
            #endregion //NV phụ trách

            #region //AddressTypeCode (loại địa chỉ)
            List<DropdownIdTypeStringModel> AddressTypeCode = new List<DropdownIdTypeStringModel>();
            var addressList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.AddressType);
            if (addressList != null && addressList.Count > 0)
            {
                AddressTypeCode = addressList.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion

            #endregion Dropdownlist

            #region //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Customer_Profiles);

            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
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
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "Đánh dấu \"X\" vào cột \"Thị trường\" nếu KH ở nước ngoài",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "Điền đầy đủ thông tin chung vào cột màu \"xanh lá cây\"",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false,
                colorCode = "#00a65a",
                isWhiteText = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "Điền đầy đủ thông tin vào cột màu \"xanh dương\" nếu là KH Tiêu Dùng",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false,
                colorCode = "#4169E1",
                isWhiteText = true,
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "Điền đầy đủ thông tin vào cột màu \"cam\" nếu là KH Doanh Nghiệp",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false,
                colorCode = "#FF6347",
                isWhiteText = true,
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning1,
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false,
                colorCode = "#FFFFE0"
            });
            #endregion //Header

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //columns.Add(new ExcelTemplate() { ColumnName = "ProfileId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "isForeignCustomer",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.Bool,
                DropdownIdTypeBoolData = ForeignCustomer,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerTypeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerTypeCode
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerSourceCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerSourceCode
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CreateAtCompany",
                isAllowedToEdit = true,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CreateAtSaleOrg",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CreateAtSaleOrg,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Title",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Title
            });
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProfileShortName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Age",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Age,
                isDifferentColorHeader = true,
                ColorHeader = "#4169E1"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "TaxNo",
                isAllowedToEdit = true,
            });
            columns.Add(new ExcelTemplate() { ColumnName = "Phone", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Email", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Website", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOfficeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleOfficeCode
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "AddressTypeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = AddressTypeCode
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProvinceId",
                isAllowedToEdit = true,
                TypeId = ConstExcelController.GuidId,
                DropdownData = ProvinceId,
                isDependentDropdown = true,
                DependentDropdownSheetName = "PROVINCE",
                DependentDropdownFormula = "OFFSET(PROVINCE!$B$1, 1, 0, MATCH(\"ZZZZZZZZ\", PROVINCE!$B:$B) - 1)",
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "DistrictId",
                isAllowedToEdit = true,
                TypeId = ConstExcelController.GuidId,
                DropdownData = DistrictId,
                isDependentDropdown = true,
                DependentDropdownSheetName = "DISTRICT",
                DependentDropdownFormula = "OFFSET(DISTRICT!$C$1,MATCH([MainSheet]!$Q1,DISTRICT!$D$2:$D$714,0),,COUNTIF(DISTRICT!$D$2:$D$714,[MainSheet]!$Q1))", //TODO: Nếu thay đổi vị trí cột thì phải cập nhật lại hàm
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "WardId",
                isAllowedToEdit = true,
                TypeId = ConstExcelController.GuidId,
                DropdownData = WardId,
                isDependentDropdown = true,
                DependentDropdownSheetName = "WARD",
                DependentDropdownFormula = "OFFSET(WARD!$D$1,MATCH([MainSheet]!$R1,WARD!$E$2:$E$11165,0),,COUNTIFS(WARD!$F$2:$F$11165,[MainSheet]!$Q1,WARD!$E$2:$E$11165,[MainSheet]!$R1))",  //TODO: Nếu thay đổi vị trí cột thì phải cập nhật lại hàm
            });

            columns.Add(new ExcelTemplate() { ColumnName = "Address", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerCareerCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerCareer,
            });
            columns.Add(new ExcelTemplate() { ColumnName = "Note", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PersonInCharge",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PersonInCharge,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactName",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactNumber",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactEmail",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "DepartmentCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = DepartmentCode,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Position",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Position,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerGroupCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerGroup,
                isDifferentColorHeader = true,
                ColorHeader = "#FF6347"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CreateTime",
                isAllowedToEdit = true,
                isDateTime = true,
            });
            //columns.Add(new ExcelTemplate() { ColumnName = "CreateBy", isAllowedToEdit = true });
            //columns.Add(new ExcelTemplate() { ColumnName = "CreateTime", isAllowedToEdit = true, isDateTime = true });
            //columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            #endregion //Columns to take

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel

        #region Export customer
        public ActionResult ExportCustomer(ProfileSearchViewModel searchViewModel)
        {
            // List<ProfileExportExcelModel> profile = new List<ProfileExportExcelModel>();
            int filteredResultsCount;
            searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
            ProfileRepository repo = new ProfileRepository(_context);

            //Page Size 
            searchViewModel.PageSize = 999999;

            //Page Number
            searchViewModel.PageNumber = 1;
            var profile = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount)
                .Select(s => new ProfileExportExcelModel
                {
                    ProfileCode = s.ProfileCode.ToString(),
                    ProfileName = s.ProfileName,
                    ProfileShortName = s.ProfileShortName,
                    ReconAccount = s.ReconcileAccountName,
                    PaymentTerms = s.PaymentTermName,
                    AccountAssignmentGrp = s.CustomerAccountAssignmentGroupName,
                    isForeignCustomer = s.isForeignCustomer == true ? "Nước ngoài" : "Trong nước",
                    CreateAtCompany = s.CreateAtCompanyName,
                    SaleOrg = s.SaleOrgName,
                    Address = s.Address,
                    Country = s.isForeignCustomer == true ? s.CountryName : "Vietnam",
                    Phone = s.Phone,
                    TaxNo = s.TaxNo,
                    Email = s.Email,
                    Email2 = s.EmailHDDT,
                    Title = s.TitleType == "B" ? "Công ty" : "Anh/Chị",
                    CustomerCareerCode = s.CustomerCareerName,
                    CustomerTypeCode = s.CustomerTypeName,
                    SaleOfficeCode = s.SaleOfficeName,
                    CustomerGroupCode = s.CustomerGroupName,
                    Manager = s.ManagerName,
                    ManagementDepartment = s.RoleInCharge,
                    SaleEmployee = s.PersonInCharge,
                    ProfileId = s.ProfileId,
                    SalesDistrict = s.DistrictName,
                    SaleGroupCode = s.ProvinceName,
                    PaymentMethods = s.PaymentMethodName,
                    TaxClassification = s.TaxClassificationName,
                    Currency = s.CurrencyName,
                    DebsEmployee = s.DebsEmployeeName,
                    PartnerFunctions = s.PartnerFunctionName
                }).ToList();
            List<ProfileExportExcelModel> contact = new List<ProfileExportExcelModel>();
            foreach (var pro in profile)
            {
                var profileContact = _unitOfWork.ProfileRepository.GetContactListOfProfile(pro.ProfileId)
                    .Select(s => new ProfileExportExcelModel
                    {
                        ContactCompany = s.ProfileContactCompanyName,
                        ContactShortName = s.ProfileContactShortName,
                        ContactName = s.ProfileContactFullName,
                        ContactDepartment = s.ProfileContactDepartmentName,
                        ContactFunction = s.ProfileContactPositionName,
                        ContactTelephone = s.ProfileContactPhone,
                        ContactEmail = s.ProfileContactEmail,
                        ContactAddress = s.ProfileContactAddress
                    }).ToList();
                if (profileContact != null && profileContact.Count > 0)
                {
                    contact.AddRange(profileContact);
                }
            }
            return ExportCustomerExcel(profile, contact);
        }


        [ISDAuthorization]
        public FileContentResult ExportCustomerExcel(List<ProfileExportExcelModel> profile, List<ProfileExportExcelModel> contact)
        {
            #region Dropdownlist

            #region //Phân loại KH
            List<DropdownIdTypeStringModel> CustomerTypeCode = new List<DropdownIdTypeStringModel>();
            var customerTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            if (customerTypeCode != null && customerTypeCode.Count > 0)
            {
                CustomerTypeCode = customerTypeCode.Where(p => p.CatalogCode != ConstCustomerType.Contact).Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Phân loại KH

            #region //Nguồn KH
            List<DropdownIdTypeStringModel> CustomerSourceCode = new List<DropdownIdTypeStringModel>();
            var customerSourceCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            if (customerSourceCode != null && customerSourceCode.Count > 0)
            {
                CustomerSourceCode = customerSourceCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nguồn KH      

            #region //Danh xưng
            List<DropdownIdTypeStringModel> Title = new List<DropdownIdTypeStringModel>();
            var title = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Title);
            if (title != null && title.Count > 0)
            {
                Title = title.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Danh xưng 

            #region //Độ tuổi
            List<DropdownIdTypeStringModel> Age = new List<DropdownIdTypeStringModel>();
            var age = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            if (age != null && age.Count > 0)
            {
                Age = age.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Độ tuổi 

            #region //Khu vực
            List<DropdownIdTypeStringModel> SaleOfficeCode = new List<DropdownIdTypeStringModel>();
            var saleOfficeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            if (saleOfficeCode != null && saleOfficeCode.Count > 0)
            {
                SaleOfficeCode = saleOfficeCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Khu vực

            #region Quốc gia
            List<DropdownIdTypeStringModel> Country = new List<DropdownIdTypeStringModel>();
            var countryCode = _unitOfWork.ProvinceRepository.GetForeign();
            if (countryCode != null && countryCode.Count > 0)
            {
                Country = countryCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.ProvinceCode,
                    Name = p.ProvinceCode + " | " + p.ProvinceName,
                }).ToList();
            }
            #endregion Quốc gia

            #region //Tỉnh/thành
            List<DropdownModel> ProvinceId = new List<DropdownModel>();
            ProvinceRepository _repoProvince = new ProvinceRepository(_context);
            var province = _repoProvince.GetAll();
            if (province != null && province.Count > 0)
            {
                ProvinceId = province.Select(p => new DropdownModel()
                {
                    Id = p.ProvinceId,
                    Name = p.ProvinceName,
                }).ToList();
            }
            #endregion //Tỉnh/thành

            #region //Quận/huyện
            List<DropdownModel> DistrictId = new List<DropdownModel>();
            DistrictRepository _repoDistrict = new DistrictRepository(_context);
            var district = _repoDistrict.GetAll();
            if (district != null && district.Count > 0)
            {
                DistrictId = district.Select(p => new DropdownModel()
                {
                    Id = p.DistrictId,
                    Name = p.DistrictName,
                    ParentLevel1Id = p.ProvinceId,
                    ParentLevel1Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Quận/huyện

            #region //Phường/xã
            List<DropdownModel> WardId = new List<DropdownModel>();
            WardRepository _repoWard = new WardRepository(_context);
            var ward = _repoWard.GetAll();
            if (ward != null && ward.Count > 0)
            {
                WardId = ward.Select(p => new DropdownModel()
                {
                    Id = p.WardId,
                    Name = p.WardName,
                    ParentLevel1Id = p.DistrictId,
                    ParentLevel1Name = p.DistrictName,
                    ParentLevel2Id = p.ProvinceId,
                    ParentLevel2Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Phường/xã

            #region //Chi nhánh
            //Lấy chi nhánh theo phân quyền
            List<DropdownIdTypeStringModel> CreateAtSaleOrg = new List<DropdownIdTypeStringModel>();
            var saleOrg = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
            if (saleOrg != null && saleOrg.Count > 0)
            {
                CreateAtSaleOrg = saleOrg.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SaleOrgCode,
                    Name = p.StoreName,
                }).ToList();
            }
            #endregion //Chi nhánh
            #region Công ty
            List<DropdownIdTypeStringModel> CreateAtCompany = new List<DropdownIdTypeStringModel>();
            var company = _unitOfWork.CompanyRepository.GetAll();
            if (company != null && company.Count > 0)
            {
                CreateAtCompany = company.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CompanyCode,
                    Name = p.CompanyCode + " | " + p.CompanyName,
                }).ToList();
            }
            #endregion Công ty
            #region Vai trò trong giao dịch
            List<DropdownIdTypeStringModel> PartnerFunctions = new List<DropdownIdTypeStringModel>();
            PartnerFunctions.Add(new DropdownIdTypeStringModel
            {
                Id = "Sold",
                Name = "Sold to party"
            });
            PartnerFunctions.Add(new DropdownIdTypeStringModel
            {
                Id = "Bill",
                Name = "Bill to party"
            });
            #endregion Vai trò trong giao dịch

            #region tài khoản doanh thu
            List<DropdownIdTypeStringModel> AccountAssignmentGrps = new List<DropdownIdTypeStringModel>();
            var aag = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerAccountAssignmentGroup);
            if (aag != null && aag.Count > 0)
            {
                AccountAssignmentGrps = aag.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion tài khoản doanh thu

            #region Điều khoản thanh toán
            List<DropdownIdTypeStringModel> PaymentTerms = new List<DropdownIdTypeStringModel>();
            var pt = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.PaymentTerm);
            if (pt != null && pt.Count > 0)
            {
                PaymentTerms = pt.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion Điều khoản thanh toán

            #region //Phòng ban
            List<DropdownIdTypeStringModel> DepartmentCode = new List<DropdownIdTypeStringModel>();
            var department = _context.DepartmentModel.ToList();
            if (department != null && department.Count > 0)
            {
                DepartmentCode = department.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.DepartmentCode,
                    Name = p.DepartmentName,
                }).ToList();
            }
            #endregion //Phòng ban

            #region //Chức vụ
            List<DropdownIdTypeStringModel> Position = new List<DropdownIdTypeStringModel>();
            var position = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Position);
            if (position != null && position.Count > 0)
            {
                Position = position.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Chức vụ

            #region //Ngành nghề
            List<DropdownIdTypeStringModel> CustomerCareer = new List<DropdownIdTypeStringModel>();
            var career = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCareer);
            if (career != null && career.Count > 0)
            {
                CustomerCareer = career.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Ngành nghề

            #region //Nhóm KH
            List<DropdownIdTypeStringModel> CustomerGroup = new List<DropdownIdTypeStringModel>();
            var customerGroup = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            if (customerGroup != null && customerGroup.Count > 0)
            {
                CustomerGroup = customerGroup.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nhóm KH

            #region //Đối tượng
            List<DropdownIdTypeBoolModel> ForeignCustomer = new List<DropdownIdTypeBoolModel>();
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = false,
                Name = LanguageResource.Domestic,
            });
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = true,
                Name = LanguageResource.Foreign,
            });
            #endregion //Đối tượng

            #region //NV phụ trách
            List<DropdownIdTypeStringModel> PersonInCharge = new List<DropdownIdTypeStringModel>();
            var personInCharge = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            if (personInCharge != null && personInCharge.Count > 0)
            {
                PersonInCharge = personInCharge.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SalesEmployeeCode,
                    Name = p.SalesEmployeeName,
                }).ToList();
            }
            #endregion //NV phụ trách

            #region //NV kinh doanh
            List<DropdownIdTypeStringModel> SaleEmployee = new List<DropdownIdTypeStringModel>();
            var salesEmployees = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            if (salesEmployees != null && salesEmployees.Count > 0)
            {
                SaleEmployee = salesEmployees.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SalesEmployeeCode,
                    Name = p.SalesEmployeeName,
                }).ToList();
            }
            #endregion //NV phụ trách

            #region //AddressTypeCode (loại địa chỉ)
            List<DropdownIdTypeStringModel> AddressTypeCode = new List<DropdownIdTypeStringModel>();
            var addressList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.AddressType);
            if (addressList != null && addressList.Count > 0)
            {
                AddressTypeCode = addressList.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion

            #endregion Dropdownlist

            #region //Header
            string fileheader = string.Format(LanguageResource.ExportProfileHeader);

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
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "1. Nếu là KH doanh nghiệp, điền đủ thông tin ở 2 sheets: Customer và Contact",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "2. Nếu là KH tiêu dùng, chỉ cần điền đầy đủ thông tin KH ở sheet Customer",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "3. KH là Bill to party (Xuất hóa đơn): Chọn Nhóm KH (cột P) là KH doanh nghiệp - Ngành nghề KH (cột Q): ko cần chọn - Customer Group (cột X) là Nhóm khác. Không cần điền sheet Contact",
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });
            #endregion //Header
            #region Contact
            string sheetContactHeader = "Contact Person";
            List<ExcelHeadingTemplate> contactHeading = new List<ExcelHeadingTemplate>();
            contactHeading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 0,
                isWarning = false,
                isCode = true
            });
            contactHeading.Add(new ExcelHeadingTemplate()
            {
                Content = sheetContactHeader.ToUpper(),
                RowsToIgnore = 0,
                isWarning = false,
                isCode = false
            });

            #endregion

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileCode",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "CRM account",
                hasNote = true,
                Note = "Bắt buộc kiểm tra thông tin khách trên CRM trước, nếu đã có mã trên CRM thì điền vào",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "isForeignCustomer",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.Bool,
                DropdownIdTypeBoolData = ForeignCustomer,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Account group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CreateAtCompany",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CreateAtCompany,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Company code"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PartnerFunctions",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PartnerFunctions,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Partner functions"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOrg",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CreateAtSaleOrg,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales Organzaition"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Title",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Title,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Title"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileShortName",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Name",
                hasNote = true,
                Note = "Viết hoa chữ cái đầu",
                isWraptext = true,
                CustomWidth = 50
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileName",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fullname",
                hasNote = true,
                Note = "Viết hoa chữ cái đầu",
                isWraptext = true,
                CustomWidth = 70
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Address",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fulladdress",
                hasNote = true,
                isWraptext = true,
                CustomWidth = 80,
                Note = "Cách nhau bằng dấu phẩy (,) ko dùng dấu gạch ngang (-)"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Country",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Contry"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "TaxNo",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "VAT Reg. No."
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Phone",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Telephone No.",
                hasNote = true,
                Note = "Số ĐT công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Fax",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fax No.",
                hasNote = true,
                Note = "Số Fax công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Email",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Email Address",
                hasNote = true,
                Note = "Email công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Email2",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                isWraptext = true,
                hasNote = true,
                Note = "Để gửi hóa đơn điện tử về cho khách",
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerTypeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerTypeCode,
                isDifferentColorHeader = true,
                ColorHeader = "#00B0F0",
                hasAnotherName = true,
                AnotherName = "Customer class"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerCareerCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerCareer,
                isDifferentColorHeader = true,
                ColorHeader = "#00B0F0",
                hasAnotherName = true,
                AnotherName = "Industry code 1",
                isWraptext = true,
                CustomWidth = 30,
                hasNote = true,
                Note = "Nếu là Người tiêu dùng, ko cần chọn trường Ngành nghề KH"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ReconAccount",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Recon.Account"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PaymentTerms",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PaymentTerms,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Payment terms"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PaymentMethods",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Payment methods"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SalesDistrict",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales District"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOfficeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleOfficeCode,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales office"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleGroupCode",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerGroupCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerGroup,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Customer Group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Currency",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Currency"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "AccountAssignmentGrp",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = AccountAssignmentGrps,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Account Assignment Grp"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "TaxClassification",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Tax classification"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Manager",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PersonInCharge,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Manager"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleEmployee",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleEmployee,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales Employee"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "DebsEmployee",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleEmployee,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Debs Employee"

            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ManagementDepartment",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = DepartmentCode,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Management department"
            });
            #endregion //Columns to take

            #region Contact Col
            List<ExcelTemplate> contactColumns = new List<ExcelTemplate>();
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactCompany",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Company",
                isWraptext = true,
                CustomWidth = 30
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactShortName",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Name"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactName",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Full Name"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactDepartment",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Department"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactFunction",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Functions"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactTelephone",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Telephone"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactExtTelephone",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Ext Telephone"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactMobilephone",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Mobile phone"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactEmail",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Email"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactAddress",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Address"
            });

            #endregion
            //Body
            //byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, false);
            List<ExportExcelInputModel> input = new List<ExportExcelInputModel>();
            input.Add(new ExportExcelInputModel
            {
                Data = profile,
                ColumnsToTake = columns,
                Heading = heading,
                showSlno = false
            });
            input.Add(new ExportExcelInputModel
            {
                Data = contact,
                ColumnsToTake = contactColumns,
                Heading = contactHeading,
                showSlno = false
            });
            byte[] filecontent = ClassExportExcel.ExportExcel<ProfileExportExcelModel>(input);
            //File name
            string fileNameWithFormat = string.Format("{0}_YEUCAUTAOMAECC_{1}.xlsx", CurrentUser.UserName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            //_unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export customer


        #region request create ecc
        private List<ProfileExportExcelModel> GetProfile(ProfileSearchViewModel searchViewModel)
        {

            int filteredResultsCount;
            searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
            ProfileRepository repo = new ProfileRepository(_context);

            //Page Size 
            searchViewModel.PageSize = 999999;

            //Page Number
            searchViewModel.PageNumber = 1;
            searchViewModel.isCreateECCRequest = true;
            var profile = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount)
                .Select(s => new ProfileExportExcelModel
                {
                    ProfileCode = s.ProfileCode.ToString(),
                    ProfileName = s.ProfileName,
                    ProfileShortName = s.ProfileShortName,
                    ReconAccount = s.ReconcileAccountName,
                    PaymentTerms = s.PaymentTermName,
                    AccountAssignmentGrp = s.CustomerAccountAssignmentGroupName,
                    isForeignCustomer = s.isForeignCustomer == true ? "Nước ngoài" : "Trong nước",
                    CreateAtCompany = s.CreateAtCompanyName,
                    SaleOrg = s.SaleOrgName,
                    Address = s.Address,
                    Country = s.isForeignCustomer == true ? s.CountryName : "Vietnam",
                    Phone = s.Phone,
                    TaxNo = s.TaxNo,
                    Email = s.Email,
                    Email2 = s.EmailHDDT,
                    Title = s.TitleType == "B" ? "Công ty" : "Anh/Chị",
                    CustomerCareerCode = s.CustomerCareerName,
                    CustomerTypeCode = s.CustomerTypeName,
                    SaleOfficeCode = s.SaleOfficeName,
                    CustomerGroupCode = s.CustomerGroupName,
                    ManagerCode = s.ManagerCode,
                    Manager = s.ManagerName,
                    ManagementDepartment = s.RoleInCharge,
                    SaleEmployee = s.SaleEmployee,
                    ProfileId = s.ProfileId,
                    SalesDistrict = s.DistrictName,
                    SaleGroupCode = s.ProvinceName,
                    PaymentMethods = s.PaymentMethodName,
                    TaxClassification = s.TaxClassificationName,
                    Currency = s.CurrencyName,
                    DebsEmployeeCode = s.DebsEmployeeCode,
                    DebsEmployee = s.DebsEmployeeName,
                    PartnerFunctions = s.PartnerFunctionName
                }).ToList();
            return profile;
        }
        private List<ProfileExportExcelModel> GetContact(List<ProfileExportExcelModel> profile)
        {
            List<ProfileExportExcelModel> contact = new List<ProfileExportExcelModel>();
            foreach (var pro in profile)
            {
                var profileContact = _unitOfWork.ProfileRepository.GetContactListOfProfile(pro.ProfileId)
                    .Select(s => new ProfileExportExcelModel
                    {
                        ContactCompany = s.ProfileContactCompanyName,
                        ContactShortName = s.ProfileContactShortName,
                        ContactName = s.ProfileContactFullName,
                        ContactDepartment = s.ProfileContactDepartmentName,
                        ContactFunction = s.ProfileContactPositionName,
                        ContactTelephone = s.ProfileContactPhone,
                        ContactEmail = s.ProfileContactEmail,
                        ContactAddress = s.ProfileContactAddress
                    }).ToList();
                if (profileContact != null && profileContact.Count > 0)
                {
                    contact.AddRange(profileContact);
                }
            }
            return contact;
        }
        private byte[] GetFileAttackment(List<ProfileExportExcelModel> profile, List<ProfileExportExcelModel> contact)
        {
            #region Dropdownlist

            #region //Phân loại KH
            List<DropdownIdTypeStringModel> CustomerTypeCode = new List<DropdownIdTypeStringModel>();
            var customerTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            if (customerTypeCode != null && customerTypeCode.Count > 0)
            {
                CustomerTypeCode = customerTypeCode.Where(p => p.CatalogCode != ConstCustomerType.Contact).Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Phân loại KH

            #region //Nguồn KH
            List<DropdownIdTypeStringModel> CustomerSourceCode = new List<DropdownIdTypeStringModel>();
            var customerSourceCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            if (customerSourceCode != null && customerSourceCode.Count > 0)
            {
                CustomerSourceCode = customerSourceCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nguồn KH      

            #region //Danh xưng
            List<DropdownIdTypeStringModel> Title = new List<DropdownIdTypeStringModel>();
            var title = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Title);
            if (title != null && title.Count > 0)
            {
                Title = title.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Danh xưng 

            #region //Độ tuổi
            List<DropdownIdTypeStringModel> Age = new List<DropdownIdTypeStringModel>();
            var age = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            if (age != null && age.Count > 0)
            {
                Age = age.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Độ tuổi 

            #region //Khu vực
            List<DropdownIdTypeStringModel> SaleOfficeCode = new List<DropdownIdTypeStringModel>();
            var saleOfficeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            if (saleOfficeCode != null && saleOfficeCode.Count > 0)
            {
                SaleOfficeCode = saleOfficeCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Khu vực

            #region Quốc gia
            List<DropdownIdTypeStringModel> Country = new List<DropdownIdTypeStringModel>();
            var countryCode = _unitOfWork.ProvinceRepository.GetForeign();
            if (countryCode != null && countryCode.Count > 0)
            {
                Country = countryCode.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.ProvinceCode,
                    Name = p.ProvinceCode + " | " + p.ProvinceName,
                }).ToList();
            }
            #endregion Quốc gia

            #region //Tỉnh/thành
            List<DropdownModel> ProvinceId = new List<DropdownModel>();
            ProvinceRepository _repoProvince = new ProvinceRepository(_context);
            var province = _repoProvince.GetAll();
            if (province != null && province.Count > 0)
            {
                ProvinceId = province.Select(p => new DropdownModel()
                {
                    Id = p.ProvinceId,
                    Name = p.ProvinceName,
                }).ToList();
            }
            #endregion //Tỉnh/thành

            #region //Quận/huyện
            List<DropdownModel> DistrictId = new List<DropdownModel>();
            DistrictRepository _repoDistrict = new DistrictRepository(_context);
            var district = _repoDistrict.GetAll();
            if (district != null && district.Count > 0)
            {
                DistrictId = district.Select(p => new DropdownModel()
                {
                    Id = p.DistrictId,
                    Name = p.DistrictName,
                    ParentLevel1Id = p.ProvinceId,
                    ParentLevel1Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Quận/huyện

            #region //Phường/xã
            List<DropdownModel> WardId = new List<DropdownModel>();
            WardRepository _repoWard = new WardRepository(_context);
            var ward = _repoWard.GetAll();
            if (ward != null && ward.Count > 0)
            {
                WardId = ward.Select(p => new DropdownModel()
                {
                    Id = p.WardId,
                    Name = p.WardName,
                    ParentLevel1Id = p.DistrictId,
                    ParentLevel1Name = p.DistrictName,
                    ParentLevel2Id = p.ProvinceId,
                    ParentLevel2Name = p.ProvinceName,
                }).OrderBy(p => p.Name).ToList();
            }
            #endregion //Phường/xã

            #region //Chi nhánh
            //Lấy chi nhánh theo phân quyền
            List<DropdownIdTypeStringModel> CreateAtSaleOrg = new List<DropdownIdTypeStringModel>();
            var saleOrg = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
            if (saleOrg != null && saleOrg.Count > 0)
            {
                CreateAtSaleOrg = saleOrg.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SaleOrgCode,
                    Name = p.StoreName,
                }).ToList();
            }
            #endregion //Chi nhánh
            #region Công ty
            List<DropdownIdTypeStringModel> CreateAtCompany = new List<DropdownIdTypeStringModel>();
            var company = _unitOfWork.CompanyRepository.GetAll();
            if (company != null && company.Count > 0)
            {
                CreateAtCompany = company.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CompanyCode,
                    Name = p.CompanyCode + " | " + p.CompanyName,
                }).ToList();
            }
            #endregion Công ty
            #region Vai trò trong giao dịch
            List<DropdownIdTypeStringModel> PartnerFunctions = new List<DropdownIdTypeStringModel>();
            PartnerFunctions.Add(new DropdownIdTypeStringModel
            {
                Id = "Sold",
                Name = "Sold to party"
            });
            PartnerFunctions.Add(new DropdownIdTypeStringModel
            {
                Id = "Bill",
                Name = "Bill to party"
            });
            #endregion Vai trò trong giao dịch

            #region tài khoản doanh thu
            List<DropdownIdTypeStringModel> AccountAssignmentGrps = new List<DropdownIdTypeStringModel>();
            var aag = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerAccountAssignmentGroup);
            if (aag != null && aag.Count > 0)
            {
                AccountAssignmentGrps = aag.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion tài khoản doanh thu

            #region Điều khoản thanh toán
            List<DropdownIdTypeStringModel> PaymentTerms = new List<DropdownIdTypeStringModel>();
            var pt = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.PaymentTerm);
            if (pt != null && pt.Count > 0)
            {
                PaymentTerms = pt.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion Điều khoản thanh toán

            #region //Phòng ban
            List<DropdownIdTypeStringModel> DepartmentCode = new List<DropdownIdTypeStringModel>();
            var department = _context.DepartmentModel.ToList();
            if (department != null && department.Count > 0)
            {
                DepartmentCode = department.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.DepartmentCode,
                    Name = p.DepartmentName,
                }).ToList();
            }
            #endregion //Phòng ban

            #region //Chức vụ
            List<DropdownIdTypeStringModel> Position = new List<DropdownIdTypeStringModel>();
            var position = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Position);
            if (position != null && position.Count > 0)
            {
                Position = position.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Chức vụ

            #region //Ngành nghề
            List<DropdownIdTypeStringModel> CustomerCareer = new List<DropdownIdTypeStringModel>();
            var career = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCareer);
            if (career != null && career.Count > 0)
            {
                CustomerCareer = career.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Ngành nghề

            #region //Nhóm KH
            List<DropdownIdTypeStringModel> CustomerGroup = new List<DropdownIdTypeStringModel>();
            var customerGroup = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            if (customerGroup != null && customerGroup.Count > 0)
            {
                CustomerGroup = customerGroup.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion //Nhóm KH

            #region //Đối tượng
            List<DropdownIdTypeBoolModel> ForeignCustomer = new List<DropdownIdTypeBoolModel>();
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = false,
                Name = LanguageResource.Domestic,
            });
            ForeignCustomer.Add(new DropdownIdTypeBoolModel()
            {
                Id = true,
                Name = LanguageResource.Foreign,
            });
            #endregion //Đối tượng

            #region //NV phụ trách
            List<DropdownIdTypeStringModel> PersonInCharge = new List<DropdownIdTypeStringModel>();
            var personInCharge = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            if (personInCharge != null && personInCharge.Count > 0)
            {
                PersonInCharge = personInCharge.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SalesEmployeeCode,
                    Name = p.SalesEmployeeName,
                }).ToList();
            }
            #endregion //NV phụ trách

            #region //NV kinh doanh
            List<DropdownIdTypeStringModel> SaleEmployee = new List<DropdownIdTypeStringModel>();
            var salesEmployees = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            if (salesEmployees != null && salesEmployees.Count > 0)
            {
                SaleEmployee = salesEmployees.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.SalesEmployeeCode,
                    Name = p.SalesEmployeeName,
                }).ToList();
            }
            #endregion //NV phụ trách

            #region //AddressTypeCode (loại địa chỉ)
            List<DropdownIdTypeStringModel> AddressTypeCode = new List<DropdownIdTypeStringModel>();
            var addressList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.AddressType);
            if (addressList != null && addressList.Count > 0)
            {
                AddressTypeCode = addressList.Select(p => new DropdownIdTypeStringModel()
                {
                    Id = p.CatalogCode,
                    Name = p.CatalogText_vi,
                }).ToList();
            }
            #endregion

            #endregion Dropdownlist

            #region //Header
            string fileheader = string.Format(LanguageResource.ExportProfileHeader);

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
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "1. Nếu là KH doanh nghiệp, điền đủ thông tin ở 2 sheets: Customer và Contact",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "2. Nếu là KH tiêu dùng, chỉ cần điền đầy đủ thông tin KH ở sheet Customer",
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "3. KH là Bill to party (Xuất hóa đơn): Chọn Nhóm KH (cột P) là KH doanh nghiệp - Ngành nghề KH (cột Q): ko cần chọn - Customer Group (cột X) là Nhóm khác. Không cần điền sheet Contact",
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });
            #endregion //Header
            #region Contact
            string sheetContactHeader = "Contact Person";
            List<ExcelHeadingTemplate> contactHeading = new List<ExcelHeadingTemplate>();
            contactHeading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 0,
                isWarning = false,
                isCode = true
            });
            contactHeading.Add(new ExcelHeadingTemplate()
            {
                Content = sheetContactHeader.ToUpper(),
                RowsToIgnore = 0,
                isWarning = false,
                isCode = false
            });

            #endregion

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileCode",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "CRM account",
                hasNote = true,
                Note = "Bắt buộc kiểm tra thông tin khách trên CRM trước, nếu đã có mã trên CRM thì điền vào",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "isForeignCustomer",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.Bool,
                DropdownIdTypeBoolData = ForeignCustomer,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Account group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CreateAtCompany",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CreateAtCompany,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Company code"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PartnerFunctions",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PartnerFunctions,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Partner functions"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOrg",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CreateAtSaleOrg,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales Organzaition"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Title",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Title,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Title"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileShortName",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Name",
                hasNote = true,
                Note = "Viết hoa chữ cái đầu",
                isWraptext = true,
                CustomWidth = 50
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileName",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fullname",
                hasNote = true,
                Note = "Viết hoa chữ cái đầu",
                isWraptext = true,
                CustomWidth = 70
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Address",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fulladdress",
                hasNote = true,
                isWraptext = true,
                CustomWidth = 80,
                Note = "Cách nhau bằng dấu phẩy (,) ko dùng dấu gạch ngang (-)"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Country",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Contry"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "TaxNo",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "VAT Reg. No."
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Phone",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Telephone No.",
                hasNote = true,
                Note = "Số ĐT công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Fax",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Fax No.",
                hasNote = true,
                Note = "Số Fax công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Email",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Email Address",
                hasNote = true,
                Note = "Email công ty"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Email2",
                isAllowedToEdit = true,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                isWraptext = true,
                hasNote = true,
                Note = "Để gửi hóa đơn điện tử về cho khách",
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerTypeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerTypeCode,
                isDifferentColorHeader = true,
                ColorHeader = "#00B0F0",
                hasAnotherName = true,
                AnotherName = "Customer class"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerCareerCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerCareer,
                isDifferentColorHeader = true,
                ColorHeader = "#00B0F0",
                hasAnotherName = true,
                AnotherName = "Industry code 1",
                isWraptext = true,
                CustomWidth = 30,
                hasNote = true,
                Note = "Nếu là Người tiêu dùng, ko cần chọn trường Ngành nghề KH"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ReconAccount",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Recon.Account"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PaymentTerms",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PaymentTerms,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Payment terms"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "PaymentMethods",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Payment methods"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SalesDistrict",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales District"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOfficeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleOfficeCode,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales office"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleGroupCode",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CustomerGroupCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = CustomerGroup,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Customer Group"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Currency",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Currency"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "AccountAssignmentGrp",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = AccountAssignmentGrps,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Account Assignment Grp"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "TaxClassification",
                isAllowedToEdit = true,
                //isDropdownlist = true,
                //TypeId = ConstExcelController.StringId,
                //DropdownIdTypeStringData = Country,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Tax classification"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ManagerCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PersonInCharge,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "ManagerCode"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Manager",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = PersonInCharge,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Manager"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleEmployee",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleEmployee,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Sales Employee"
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "DebsEmployeeCode",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleEmployee,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Debs EmployeeCode"

            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "DebsEmployee",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = SaleEmployee,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Debs Employee"

            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ManagementDepartment",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = DepartmentCode,
                isDifferentColorHeader = true,
                ColorHeader = "#0070C0",
                hasAnotherName = true,
                AnotherName = "Management department"
            });
            #endregion //Columns to take

            #region Contact Col
            List<ExcelTemplate> contactColumns = new List<ExcelTemplate>();
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactCompany",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Company",
                isWraptext = true,
                CustomWidth = 30
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactShortName",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Name"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactName",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Full Name"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactDepartment",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Department"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactFunction",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Functions"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactTelephone",
                isAllowedToEdit = true,


                hasAnotherName = true,
                AnotherName = "Telephone"
            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactExtTelephone",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Ext Telephone"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactMobilephone",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Mobile phone"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactEmail",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Email"

            });
            contactColumns.Add(new ExcelTemplate()
            {
                ColumnName = "ContactAddress",
                isAllowedToEdit = true,

                hasAnotherName = true,
                AnotherName = "Address"
            });

            #endregion
            //Body
            //byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, false);
            List<ExportExcelInputModel> input = new List<ExportExcelInputModel>();
            input.Add(new ExportExcelInputModel
            {
                Data = profile,
                ColumnsToTake = columns,
                Heading = heading,
                showSlno = false
            });
            input.Add(new ExportExcelInputModel
            {
                Data = contact,
                ColumnsToTake = contactColumns,
                Heading = contactHeading,
                showSlno = false
            });
            byte[] filecontent = ClassExportExcel.ExportExcel<ProfileExportExcelModel>(input);
            return filecontent;
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult RequestCreateECC(EmailViewModel emailViewModel)
        {
            //get profile
            var profile = GetProfile(emailViewModel.SearchProfileData);
            //get contact
            List<ProfileExportExcelModel> contact = GetContact(profile);
            // get email config
            var config = _unitOfWork.RequestEccEmailConfigRepository.GetEmailConfig();
            MailMessage email = new MailMessage();
            email.From = new MailAddress(config.FromEmail);
            email.Sender = new MailAddress(config.FromEmail);
            List<string> toEmailList = config.ToEmail.Split(';').ToList();
            foreach (var toEmail in toEmailList)
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    email.To.Add(new MailAddress(toEmail.Trim()));
                }
            }
            if (!string.IsNullOrEmpty(emailViewModel.Cc))
            {
                List<string> ccEmailList = emailViewModel.Cc.Split(';').ToList();
                foreach (var ccEmail in ccEmailList)
                {
                    if (!string.IsNullOrEmpty(ccEmail))
                    {
                        email.CC.Add(ccEmail.Trim());
                    }
                }
            }
            email.Body = emailViewModel.EmailContent;
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = emailViewModel.Subject;
            var filecontent = GetFileAttackment(profile, contact);
            string fileNameWithFormat = string.Format("{0}_YEUCAUTAOMAECC_{1}.xlsx", CurrentUser.UserName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            using (var ms = new MemoryStream())
            {
                Attachment att = new Attachment(new MemoryStream(filecontent), fileNameWithFormat);
                email.Attachments.Add(att);
            }
            string message = "";
            using (var smtp = new SmtpClient())
            {
                smtp.Host = config.Host;
                smtp.Port = config.Port.Value;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(config.FromEmail, config.FromEmailPassword);
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = message
                    });
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Message = message
                    });
                }
            }
            return Json(new
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true
            });
        }
        #endregion
        //Import
        #region Import from excel
        //[ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                //DataSet ds = GetDataSetFromExcel();
                DataSet ds = GetDataSetFromExcelNew(true);
                List<string> errorList = new List<string>();
                List<Guid?> ProfileId = new List<Guid?>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //Get controller code from Excel file
                        string contCode = dt.Columns[0].ColumnName.ToString();
                        //Import data with accordant controller and action
                        if (contCode == controllerCode)
                        {
                            var index = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!AreAllColumnsEmpty(dr))
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        index++;
                                        //Check correct template
                                        ProfileExcelViewModel profileIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(profileIsValid.Error))
                                        {
                                            string error = profileIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            //Nếu là KH tiêu dùng thì bắt buộc nhập field số điện thoại
                                            if (profileIsValid.CustomerTypeCode == ConstCustomerType.Customer)
                                            {
                                                if (string.IsNullOrEmpty(profileIsValid.Phone))
                                                {
                                                    string error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, "SDT"), profileIsValid.RowIndex);
                                                    errorList.Add(error);
                                                }
                                            }
                                            else if (profileIsValid.CustomerTypeCode == ConstCustomerType.Bussiness)
                                            {
                                                if (string.IsNullOrEmpty(profileIsValid.ContactNumber))
                                                {
                                                    string error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, "SDT liên hệ"), profileIsValid.RowIndex);
                                                    errorList.Add(error);
                                                }
                                            }
                                            if (errorList == null || errorList.Count == 0)
                                            {
                                                string result = ExecuteImportExcelProfile(profileIsValid);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Customer_Profiles);
                            errorList.Add(error);
                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = errorList.Take(10),
                            });
                        }
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = LanguageResource.Validation_ImportExcelFile
                    });
                }
            });

        }

        #region Insert/Update data from excel file
        public string ExecuteImportExcelProfile(ProfileExcelViewModel profileIsValid)
        {
            //Check nếu có sđt và mst thì không insert
            var existsProfile = _context.ProfileModel.Where(p => profileIsValid.CustomerTypeCode == ConstCustomerType.Account
                                                                && ((profileIsValid.TaxNo != null && p.TaxNo == profileIsValid.TaxNo)
                                                                || (profileIsValid.Phone != null && p.Phone == profileIsValid.Phone))).FirstOrDefault();
            Guid? pId = null;
            if (existsProfile == null)
            {
                #region Insert
                try
                {
                    Guid ProfileId = Guid.NewGuid();
                    //--Thông tin chung
                    ProfileModel profile = new ProfileModel();
                    profile.ProfileId = ProfileId;
                    //Lấy ID để update bảng ExtendInfor
                    pId = ProfileId;
                    //1. Đối tượng(Trong nước: false | Nước ngoài: true)
                    if (profileIsValid.isForeignCustomer == null)
                    {
                        ProvinceRepository _provinceRepository = new ProvinceRepository(_context);
                        //Nếu không có Đối tượng thì lấy theo Area của ProvinceId
                        var Area = _provinceRepository.GetAreaByProvince(profileIsValid.ProvinceId);
                        if (!string.IsNullOrEmpty(Area))
                        {
                            profileIsValid.isForeignCustomer = false;
                        }
                    }
                    profile.isForeignCustomer = profileIsValid.isForeignCustomer;
                    //2. Danh xưng
                    profile.Title = profileIsValid.Title;
                    //3. Loại
                    profile.CustomerTypeCode = ConstCustomerType.Account;
                    //Phân loại KH
                    ProfileTypeModel model = new ProfileTypeModel();
                    model.ProfileTypeId = Guid.NewGuid();
                    model.ProfileId = ProfileId;
                    model.CustomerTypeCode = profileIsValid.CustomerTypeCode;
                    model.CompanyCode = profileIsValid.CreateAtCompany;
                    model.CreateBy = CurrentUser.AccountId;
                    model.CreateTime = DateTime.Now;
                    _context.Entry(model).State = EntityState.Added;
                    //4. Tên
                    //Nếu là doanh nghiệp viết hoa các chữ cái đầu tiên
                    var toLowerOtherChar = true;
                    if (profileIsValid.CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        toLowerOtherChar = false;
                    }
                    profile.ProfileName = profileIsValid.ProfileName?.FirstCharToUpper(toLowerOtherChar);
                    //5. Tên ngắn
                    profile.ProfileShortName = profileIsValid.ProfileShortName;
                    //6. Tên viết tắt
                    profile.AbbreviatedName = profileIsValid.ProfileName?.ToAbbreviation();
                    //7. Độ tuổi
                    //Nếu là KH tiêu dùng thì lưu độ tuổi
                    if (profileIsValid.CustomerTypeCode == ConstCustomerType.Customer)
                    {
                        profile.Age = profileIsValid.Age;
                    }
                    //8. Số điện thoại liên hệ
                    profile.Phone = profileIsValid.Phone;
                    //9. Email
                    profile.Email = profileIsValid.Email;
                    //10. Khu vực
                    profile.SaleOfficeCode = profileIsValid.SaleOfficeCode;
                    //11. Địa chỉ
                    profile.Address = profileIsValid.Address.FirstCharToUpper();
                    //12. Tỉnh/thành phố
                    profile.ProvinceId = profileIsValid.ProvinceId;
                    //13. Quận/huyện
                    profile.DistrictId = profileIsValid.DistrictId;
                    //14. Phường/xã
                    profile.WardId = profileIsValid.WardId;
                    //15. Ghi chú
                    profile.Note = profileIsValid.Note;
                    //16. Trạng thái
                    profile.Actived = true;
                    //17. Nhân viên tạo
                    profile.CreateByEmployee = CurrentUser.EmployeeCode;
                    //18. Tạo tại công ty
                    profile.CreateAtCompany = profileIsValid.CreateAtCompany;
                    //19. Tạo tại chi nhánh
                    profile.CreateAtSaleOrg = profileIsValid.CreateAtSaleOrg;
                    //20. CreateBy
                    profile.CreateBy = CurrentUser.AccountId;
                    //21. Thời gian tạo
                    profile.CreateTime = profileIsValid.CreateTime.HasValue ? profileIsValid.CreateTime : DateTime.Now;
                    //22. Nguồn khách hàng
                    profile.CustomerSourceCode = profileIsValid.CustomerSourceCode;
                    //23. Ngành nghề
                    if (profileIsValid.CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        ProfileCareerModel profileCareerAdd = new ProfileCareerModel();
                        profileCareerAdd.ProfileCareerId = Guid.NewGuid();
                        profileCareerAdd.ProfileId = ProfileId;
                        profileCareerAdd.CompanyCode = profileIsValid.CreateAtCompany;
                        profileCareerAdd.ProfileCareerCode = profileIsValid.CustomerCareerCode;
                        profileCareerAdd.CreateBy = CurrentUser.AccountId;
                        profileCareerAdd.CreateTime = DateTime.Now;

                        _context.Entry(profileCareerAdd).State = EntityState.Added;
                    }
                    //24. MST
                    profile.TaxNo = profileIsValid.TaxNo;
                    //25. Website
                    profile.Website = profileIsValid.Website;
                    //26. Loại địa chỉ
                    profile.AddressTypeCode = profileIsValid.AddressTypeCode;

                    //--Nhân viên phụ trách
                    PersonInChargeModel personInCharge = new PersonInChargeModel();
                    personInCharge.PersonInChargeId = Guid.NewGuid();
                    personInCharge.ProfileId = ProfileId;
                    personInCharge.SalesEmployeeCode = profileIsValid.PersonInCharge;
                    personInCharge.CreateBy = CurrentUser.AccountId;
                    personInCharge.CreateTime = DateTime.Now;
                    personInCharge.CompanyCode = profileIsValid.CreateAtCompany;
                    _context.Entry(personInCharge).State = EntityState.Added;

                    _context.Entry(profile).State = EntityState.Added;


                    //Nếu là KH doanh nghiệp thì lưu thêm 1 lần bảng Profile và thêm 3 bảng:
                    //-- 1. ProfileContactAttributeModel
                    //-- 2. ProfileGroupModel
                    //-- 3. PersonInChargeModel
                    if (profileIsValid.CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        //Check contact đã tồn tại hay chưa
                        var existsContact = _context.ProfileModel.Where(p => profileIsValid.CustomerTypeCode == ConstCustomerType.Contact
                                                                && (profileIsValid.ContactNumber != null && p.Phone == profileIsValid.ContactNumber)).FirstOrDefault();
                        //Chưa có => thêm mới
                        if (existsContact == null)
                        {
                            Guid ProfileContactId = Guid.NewGuid();
                            ProfileModel profileContact = new ProfileModel();
                            profileContact.ProfileName = profileIsValid.ContactName?.FirstCharToUpper();
                            profileContact.AbbreviatedName = profileIsValid.ContactName?.ToAbbreviation();
                            profileContact.ProfileId = ProfileContactId;
                            profileContact.Phone = profileIsValid.ContactNumber;
                            profileContact.Email = profileIsValid.ContactEmail;
                            profileContact.ProvinceId = profileIsValid.ProvinceId;
                            profileContact.DistrictId = profileIsValid.DistrictId;
                            profileContact.WardId = profileIsValid.WardId;
                            profileContact.Address = profileIsValid.Address;
                            profileContact.CustomerTypeCode = ConstCustomerType.Contact;
                            profileContact.isForeignCustomer = profileIsValid.isForeignCustomer;
                            profileContact.Actived = true;
                            profileContact.CreateByEmployee = CurrentUser.EmployeeCode;
                            profileContact.CreateAtCompany = profileIsValid.CreateAtCompany;
                            profileContact.CreateAtSaleOrg = profileIsValid.CreateAtSaleOrg;
                            profileContact.CreateBy = CurrentUser.AccountId;
                            profileContact.CreateTime = DateTime.Now;
                            _context.Entry(profileContact).State = EntityState.Added;

                            ProfileContactAttributeModel profileContactAttribute = new ProfileContactAttributeModel();
                            profileContactAttribute.IsMain = true;
                            profileContactAttribute.ProfileId = ProfileContactId;
                            profileContactAttribute.CompanyId = ProfileId;
                            profileContactAttribute.Position = profileIsValid.Position;
                            profileContactAttribute.DepartmentCode = profileIsValid.DepartmentCode;
                            _context.Entry(profileContactAttribute).State = EntityState.Added;

                            //Nhân viên phụ trách
                            PersonInChargeModel personInChargeContact = new PersonInChargeModel();
                            personInChargeContact.PersonInChargeId = Guid.NewGuid();
                            personInChargeContact.ProfileId = ProfileContactId;
                            personInChargeContact.SalesEmployeeCode = profileIsValid.PersonInCharge;
                            personInChargeContact.CreateBy = CurrentUser.AccountId;
                            personInChargeContact.CreateTime = DateTime.Now;
                            personInChargeContact.CompanyCode = profileIsValid.CreateAtCompany;
                            _context.Entry(personInChargeContact).State = EntityState.Added;
                        }
                        //Đã có => cập nhật thông tin contact
                        else
                        {
                            existsContact.Email = profileIsValid.ContactEmail;
                            //existsContact.ProvinceId = profileIsValid.ProvinceId;
                            //existsContact.DistrictId = profileIsValid.DistrictId;
                            //existsContact.WardId = profileIsValid.WardId;
                            //existsContact.Address = profileIsValid.Address;
                            existsContact.LastEditBy = CurrentUser.AccountId;
                            existsContact.LastEditTime = DateTime.Now;

                            _context.Entry(existsContact).State = EntityState.Modified;

                            ProfileContactAttributeModel profileContactAttribute = new ProfileContactAttributeModel();
                            profileContactAttribute.IsMain = true;
                            profileContactAttribute.ProfileId = existsContact.ProfileId;
                            profileContactAttribute.CompanyId = ProfileId;
                            profileContactAttribute.Position = profileIsValid.Position;
                            profileContactAttribute.DepartmentCode = profileIsValid.DepartmentCode;
                            _context.Entry(profileContactAttribute).State = EntityState.Added;
                        }
                    }

                    //--Nhóm khách hàng
                    if (!string.IsNullOrEmpty(profileIsValid.CustomerGroupCode))
                    {
                        ProfileGroupModel profileGroup = new ProfileGroupModel();
                        profileGroup.ProfileGroupId = Guid.NewGuid();
                        profileGroup.ProfileId = ProfileId;
                        profileGroup.CompanyCode = profileIsValid.CreateAtCompany;
                        profileGroup.ProfileGroupCode = profileIsValid.CustomerGroupCode;
                        profileGroup.CreateBy = CurrentUser.AccountId;
                        profileGroup.CreateTime = DateTime.Now;

                        _context.Entry(profileGroup).State = EntityState.Added;
                    }
                    // Phân nhóm khách hàng
                    //Trong nước: Z002
                    //Nước ngoài: Z003
                    if (profile.isForeignCustomer == true)
                    {
                        profile.CustomerAccountGroupCode = "Z003";
                    }
                    else
                    {
                        profile.CustomerAccountGroupCode = "Z002";
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
                #endregion Insert
                _context.SaveChanges();

                if (pId != null && pId != Guid.Empty)
                {
                    #region sync_Profile_ExtendInfo_To_Table
                    _unitOfWork.ProfileRepository.sync_Profile_ExtendInfo_To_Table(pId);
                    #endregion
                }

            }
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file

        #region Check data type 
        public ProfileExcelViewModel CheckTemplate(object[] row, int index)
        {
            ProfileExcelViewModel profileVM = new ProfileExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case ConstantExcelRow.A:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            profileVM.RowIndex = rowIndex;
                            break;
                        //Đối tượng
                        case ConstantExcelRow.AQ:
                            fieldName = "Thị trường";
                            string isForeign = row[i].ToString();
                            if (string.IsNullOrEmpty(isForeign))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.isForeignCustomer = GetTypeFunction<bool>(row[i].ToString(), i);
                            }
                            break;
                        //Loại KH
                        case ConstantExcelRow.AE:
                            fieldName = "Loại khách hàng";
                            string customerType = row[i].ToString();
                            if (string.IsNullOrEmpty(customerType))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.CustomerTypeCode = customerType;
                            }
                            break;
                        //Nguồn KH
                        case ConstantExcelRow.AF:
                            fieldName = "Nguồn khách hàng";
                            string customerSource = row[i].ToString();
                            if (!string.IsNullOrEmpty(customerSource))
                            {
                                profileVM.CustomerSourceCode = customerSource;
                            }
                            break;
                        //Mã công ty
                        case ConstantExcelRow.E:
                            fieldName = "Mã công ty";
                            string createAtCompany = row[i].ToString();
                            if (string.IsNullOrEmpty(createAtCompany))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.CreateAtCompany = createAtCompany;
                            }
                            break;
                        //Chi nhánh
                        case ConstantExcelRow.AG:
                            fieldName = "Chi nhánh";
                            string createAtSaleOrg = row[i].ToString();
                            if (string.IsNullOrEmpty(createAtSaleOrg))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.CreateAtSaleOrg = createAtSaleOrg;
                            }
                            break;
                        //Danh xưng
                        case ConstantExcelRow.AH:
                            fieldName = "Danh xưng";
                            string title = row[i].ToString();
                            if (string.IsNullOrEmpty(title))
                            {
                                //profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.Title = title;
                            }
                            break;
                        //Tên KH
                        case ConstantExcelRow.H:
                            fieldName = "Tên KH";
                            string profileName = row[i].ToString();
                            if (string.IsNullOrEmpty(profileName))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.ProfileName = profileName?.FirstCharToUpper();
                            }
                            break;
                        //Tên ngắn
                        case ConstantExcelRow.I:
                            fieldName = "Tên ngắn";
                            string profileShortName = row[i].ToString();
                            if (!string.IsNullOrEmpty(profileShortName))
                            {
                                profileVM.ProfileShortName = profileShortName;
                            }
                            break;
                        //Độ tuổi
                        case ConstantExcelRow.AI:
                            fieldName = "Độ tuổi";
                            string age = row[i].ToString();
                            if (!string.IsNullOrEmpty(age))
                            {
                                profileVM.Age = age;
                            }
                            break;
                        //Mã số thuế
                        case ConstantExcelRow.K:
                            fieldName = "Mã số thuế";
                            string taxNo = row[i].ToString();
                            if (!string.IsNullOrEmpty(taxNo))
                            {
                                profileVM.TaxNo = taxNo;
                            }
                            break;
                        //SDT
                        case ConstantExcelRow.L:
                            fieldName = "SDT";
                            string phone = row[i].ToString();
                            if (!string.IsNullOrEmpty(phone))
                            {
                                profileVM.Phone = phone;
                            }
                            break;
                        //Email
                        case ConstantExcelRow.M:
                            fieldName = "Email";
                            string email = row[i].ToString();
                            if (!string.IsNullOrEmpty(email))
                            {
                                if (IsValidEmail(email) == false)
                                {
                                    profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format("Vui lòng nhập đúng định dạng email khách hàng"), profileVM.RowIndex);
                                }
                                else
                                {
                                    profileVM.Email = email;
                                }
                            }
                            break;
                        //Website
                        case ConstantExcelRow.N:
                            fieldName = "Website";
                            string website = row[i].ToString();
                            if (!string.IsNullOrEmpty(website))
                            {
                                profileVM.Website = website;
                            }
                            break;
                        //Khu vực
                        case ConstantExcelRow.AJ:
                            fieldName = "Khu vực";
                            string saleOfficeCode = row[i].ToString();
                            if (!string.IsNullOrEmpty(saleOfficeCode))
                            {
                                profileVM.SaleOfficeCode = saleOfficeCode;
                            }
                            break;
                        //Loại địa chỉ
                        case ConstantExcelRow.AK:
                            fieldName = "Loại địa chỉ";
                            string addressTypeCode = row[i].ToString();
                            if (!string.IsNullOrEmpty(addressTypeCode))
                            {
                                profileVM.AddressTypeCode = addressTypeCode;
                            }
                            break;
                        //Tỉnh/thành
                        case ConstantExcelRow.AR:
                            fieldName = "Tỉnh/thành";
                            profileVM.ProvinceId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //Quận/huyện
                        case ConstantExcelRow.AS:
                            fieldName = "Quận/huyện";
                            profileVM.DistrictId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //Phường/xã
                        case ConstantExcelRow.AT:
                            fieldName = "Phường/xã";
                            profileVM.WardId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //Địa chỉ
                        case ConstantExcelRow.T:
                            fieldName = "Địa chỉ";
                            string address = row[i].ToString();
                            if (!string.IsNullOrEmpty(address))
                            {
                                profileVM.Address = address;
                            }
                            break;
                        //Ngành nghề
                        case ConstantExcelRow.AL:
                            fieldName = "Ngành nghề";
                            string customerCareer = row[i].ToString();
                            if (!string.IsNullOrEmpty(customerCareer))
                            {
                                profileVM.CustomerCareerCode = customerCareer;
                            }
                            break;
                        //Ghi chú
                        case ConstantExcelRow.V:
                            fieldName = "Ghi chú";
                            string note = row[i].ToString();
                            if (!string.IsNullOrEmpty(note))
                            {
                                profileVM.Note = note;
                            }
                            break;
                        //NV phụ trách
                        case ConstantExcelRow.AM:
                            fieldName = "NV phụ trách";
                            string employeeCode = row[i].ToString();
                            if (string.IsNullOrEmpty(employeeCode))
                            {
                                profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, fieldName), profileVM.RowIndex);
                            }
                            else
                            {
                                profileVM.PersonInCharge = employeeCode;
                            }
                            break;
                        //Tên người liên hệ
                        case ConstantExcelRow.X:
                            fieldName = "Tên người liên hệ";
                            string contactName = row[i].ToString();
                            if (!string.IsNullOrEmpty(contactName))
                            {
                                profileVM.ContactName = contactName;
                            }
                            break;
                        //SDT liên hệ
                        case ConstantExcelRow.Y:
                            fieldName = "SDT liên hệ";
                            string contactNumber = row[i].ToString();
                            if (!string.IsNullOrEmpty(contactNumber))
                            {
                                profileVM.ContactNumber = contactNumber;
                            }
                            break;
                        //Email liên hệ
                        case ConstantExcelRow.Z:
                            fieldName = "Email liên hệ";
                            string contactEmail = row[i].ToString();
                            if (!string.IsNullOrEmpty(contactEmail))
                            {
                                if (IsValidEmail(contactEmail) == false)
                                {
                                    profileVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format("Vui lòng nhập đúng định dạng email"), profileVM.RowIndex);
                                }
                                else
                                {
                                    profileVM.ContactEmail = contactEmail;
                                }
                            }
                            break;
                        //Phòng ban
                        case ConstantExcelRow.AN:
                            fieldName = "Phòng ban";
                            string departmentCode = row[i].ToString();
                            if (!string.IsNullOrEmpty(departmentCode))
                            {
                                profileVM.DepartmentCode = departmentCode;
                            }
                            break;
                        //Chức vụ
                        case ConstantExcelRow.AO:
                            fieldName = "Chức vụ";
                            string position = row[i].ToString();
                            if (!string.IsNullOrEmpty(position))
                            {
                                profileVM.Position = position;
                            }
                            break;
                        //Nhóm khách hàng
                        case ConstantExcelRow.AP:
                            fieldName = "Nhóm khách hàng";
                            string customerGroup = row[i].ToString();
                            if (!string.IsNullOrEmpty(customerGroup))
                            {
                                profileVM.CustomerGroupCode = customerGroup;
                            }
                            break;
                        //Ngày tạo
                        case ConstantExcelRow.AD:
                            fieldName = "Ngày tạo";
                            profileVM.CreateTime = GetTypeFunction<DateTime>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                profileVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (InvalidCastException ex)
            {
                profileVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (Exception ex)
            {
                profileVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index) + ex.Message;
            }
            return profileVM;
        }

        private bool IsValidEmail(string email)
        {
            string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                                          @"@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            return Regex.IsMatch(email, RegexPattern, RegexOptions.IgnoreCase);
        }
        #endregion Check data type

        #endregion Import from excel

        #region Show profile 3CX call center
        /// <summary>
        /// ACtion dùng cho call center 3CX. hiển thị thông tin khi có khách hàng gọi đến.
        /// </summary>
        /// <param name="PhoneNumber">string</param>
        /// <returns>Return to Action view detail profile</returns>
        public ActionResult SearchContact(string PhoneNumber)
        {
            //Tìm theo sdt (KH đang sử dụng)
            var profile = _context.ProfileModel.Where(p => p.Phone.Contains(PhoneNumber) && p.Actived != false).ToList();

            if (profile != null && profile.Count > 0)
            {
                //Tìm thấy 1 khách hàng thì hiển thị thông tin khách hàng đó
                if (profile.Count == 1)
                {
                    //Hiển thị thông tin của khách hàng.
                    var profileId = profile[0].ProfileId;
                    return RedirectToAction("Edit", new { id = profileId });
                }
                else
                {
                    //Tìm thấy nhiều hơn 1 khách hàng thì Truyền số dt vào Index
                    return RedirectToAction("Index", new { Type = "Account", phoneNumber = PhoneNumber });
                }
            }

            //Tìm theo số điện thoại trong bảng ProfilePhoneModel
            var profilePhones = _context.ProfilePhoneModel.Where(p => p.PhoneNumber.Contains(PhoneNumber)).ToList();
            if (profilePhones != null && profilePhones.Count > 0)
            {
                //Tìm thấy 1 khách hàng thì hiển thị thông tin khách hàng đó
                if (profilePhones.Count == 1)
                {
                    //Hiển thị thông tin của khách hàng.
                    var profileId = profilePhones[0].ProfileId;
                    return RedirectToAction("Edit", new { id = profileId });
                }
                else
                {
                    //Tìm thấy nhiều hơn 1 khách hàng thì Truyền số dt vào Index
                    return RedirectToAction("Index", new { Type = "Account", phoneNumber = PhoneNumber });
                }
            }

            //Không có khách hàng nào thì mở trang thêm mới khách hàng.
            return RedirectToAction("Create", new { type = "Account", phoneNumber = PhoneNumber });

        }
        #endregion

        #region Merge Profile
        /// <summary>
        /// Check thông tin các mã CRM cần gộp 
        /// </summary>
        /// <param name="ProfileCodeNeedToMerge">Mã CRM cần gộp</param>
        /// <param name="MergeList">Danh sách mã CRM bị trùng thông tin cần gộp về 1 mã</param>
        /// <returns></returns>
        public ActionResult CheckProfileInformation(string ProfileCodeNeedToMerge, List<string> MergeList)
        {
            string Message = string.Empty;
            try
            {
                //Thông tin khách hàng của mã CRM cần gộp
                var ProfileCode = Convert.ToInt32(ProfileCodeNeedToMerge);
                var profileNeedToMerge = new ProfileOverviewModel();
                var profile = _context.ProfileModel.Where(p => p.ProfileCode == ProfileCode).FirstOrDefault();
                if (profile != null)
                {
                    profileNeedToMerge = _unitOfWork.ProfileRepository.GetViewBy(profile.ProfileId, CurrentUser.CompanyCode);
                }

                //Thông tin khách hàng bị trùng mã CRM
                var profileDuplicateLst = new List<ProfileOverviewModel>();
                if (MergeList != null && MergeList.Count > 0)
                {

                    foreach (var item in MergeList)
                    {
                        var ProfileCodeDuplicate = Convert.ToInt32(item);
                        var profileDuplicate = _context.ProfileModel.Where(p => p.ProfileCode == ProfileCodeDuplicate).FirstOrDefault();

                        if (profileDuplicate != null)
                        {
                            profileDuplicateLst.Add(_unitOfWork.ProfileRepository.GetViewBy(profileDuplicate.ProfileId, CurrentUser.CompanyCode));
                        }
                    }
                }

                return Json(new
                {
                    Success = true,
                    ProfileNeedToMerge = profileNeedToMerge,
                    ProfileDuplicateList = profileDuplicateLst
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Message = ex.InnerException.InnerException.Message;
                    }
                }


                return Json(new { Success = false, Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gộp mã CRM
        /// </summary>
        /// <param name="ProfileCodeNeedToMerge">Mã CRM cần gộp</param>
        /// <param name="MergeList">Danh sách mã CRM bị trùng thông tin cần gộp về 1 mã</param>
        /// <returns></returns>
        public ActionResult MergeProfile(string ProfileCodeNeedToMerge, List<string> MergeList)
        {
            string Message = string.Empty;
            try
            {
                Guid? ProfileId = null;
                List<Guid?> ProfileDuplicateId = new List<Guid?>();

                #region validate
                //Ưu tiên mã có thông tin mã SAP đầu tiên
                //Thông tin khách hàng của mã CRM cần gộp
                var ProfileCode = Convert.ToInt32(ProfileCodeNeedToMerge);
                var profileNeedToMerge = new ProfileOverviewModel();
                var profile = _context.ProfileModel.Where(p => p.ProfileCode == ProfileCode).FirstOrDefault();
                if (profile != null)
                {
                    profileNeedToMerge = _unitOfWork.ProfileRepository.GetViewBy(profile.ProfileId, CurrentUser.CompanyCode);
                    ProfileId = profile.ProfileId;
                }

                //Thông tin khách hàng bị trùng mã CRM
                if (MergeList != null && MergeList.Count > 0)
                {
                    foreach (var item in MergeList)
                    {
                        var ProfileCodeDuplicate = Convert.ToInt32(item);
                        var profileDuplicate = _context.ProfileModel.Where(p => p.ProfileCode == ProfileCodeDuplicate).FirstOrDefault();

                        if (profileDuplicate != null)
                        {
                            var profileDuplicateDB = _unitOfWork.ProfileRepository.GetViewBy(profileDuplicate.ProfileId, CurrentUser.CompanyCode);
                            //Mã cần gộp không có mã SAP, mã trùng lại có mã SAP => báo lỗi
                            if (string.IsNullOrEmpty(profileNeedToMerge.ProfileForeignCode) && !string.IsNullOrEmpty(profileDuplicateDB.ProfileForeignCode))
                            {
                                return Json(new
                                {
                                    Success = false,
                                    Message = "Chỉ gộp khách hàng có thông tin mã SAP đầu tiên vì các mã CRM bị lặp dữ liệu có chứa mã SAP!"
                                }, JsonRequestBehavior.AllowGet);
                            }

                            ProfileDuplicateId.Add(profileDuplicate.ProfileId);
                        }
                    }
                }

                #endregion validate

                #region Gộp mã
                string errorMessage = string.Empty;
                if (ProfileId.HasValue && ProfileDuplicateId != null && ProfileDuplicateId.Count > 0)
                {
                    _unitOfWork.ProfileRepository.MergeProfile(ProfileId, ProfileDuplicateId, out errorMessage);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Không tìm thấy thông tin khách hàng cần cập nhật!"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = errorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
                #endregion Gộp mã

                return Json(new
                {
                    Success = true,
                    Message = "Gộp mã CRM thành công!"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    Message = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        Message = ex.InnerException.InnerException.Message;
                    }
                }


                return Json(new { Success = false, Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Merge Profile

        #region Restore contact
        [HttpPost]
        [ISDAuthorization]
        public ActionResult Restore(Guid id)
        {
            return ExecuteContainer(() =>
            {

                var profileDel = _context.ProfileDeletedModel.FirstOrDefault(p => p.ProfileId == id);
                var profileRestore = new ProfileModel();
                profileRestore.MapFromProfileDeleted(profileDel);
                _context.Entry(profileRestore).State = EntityState.Added;


                var profileAtrDel = _context.ProfileContactAttributeDeletedModel.FirstOrDefault(p => p.ProfileId == id);
                var resProfileAtr = new ProfileContactAttributeModel();
                resProfileAtr.MapFromContactAttributeDeleted(profileAtrDel);
                _context.Entry(resProfileAtr).State = EntityState.Added;


                var proPhoneDel = _context.ProfilePhoneDeletedModel.Where(p => p.ProfileId == id).ToList();
                if (proPhoneDel != null && proPhoneDel.Count > 0)
                {
                    foreach (var item in proPhoneDel)
                    {
                        var resProPhone = new ProfilePhoneModel();
                        resProPhone.MapFromProfilePhoneDeleted(item);
                        _context.Entry(resProPhone).State = EntityState.Added;
                        _context.Entry(item).State = EntityState.Deleted;
                    }
                }

                var proEmailDel = _context.ProfileEmailDeletedModel.Where(p => p.ProfileId == id).ToList();
                if (proEmailDel != null && proEmailDel.Count > 0)
                {
                    foreach (var item in proEmailDel)
                    {
                        var resProEmail = new ProfileEmailModel();
                        resProEmail.MapFromProfileEmailDeleted(item);
                        _context.Entry(resProEmail).State = EntityState.Added;
                        _context.Entry(item).State = EntityState.Deleted;
                    }
                }

                var personInChargeDel = _context.PersonInChargeDeletedModel.Where(p => p.ProfileId == id).ToList();
                if (personInChargeDel != null && personInChargeDel.Count > 0)
                {
                    foreach (var item in personInChargeDel)
                    {
                        var resPersonInCharge = new PersonInChargeModel();
                        resPersonInCharge.MapFromPersonInChargeDeleted(item);
                        _context.Entry(resPersonInCharge).State = EntityState.Added;
                        _context.Entry(item).State = EntityState.Deleted;
                    }
                }
                var roleInChargeDel = _context.RoleInChargeDeletedModel.Where(p => p.RoleInChargeId == id).ToList();
                if (roleInChargeDel != null && roleInChargeDel.Count > 0)
                {
                    foreach (var item in roleInChargeDel)
                    {
                        var resRoleInCharge = new RoleInChargeModel();
                        resRoleInCharge.MapFromRoleInChargeDeleted(item);
                        _context.Entry(resRoleInCharge).State = EntityState.Added;
                        _context.Entry(item).State = EntityState.Deleted;
                    }
                }
                _context.Entry(profileDel).State = EntityState.Deleted;
                _context.Entry(profileAtrDel).State = EntityState.Deleted;

                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Restore_Success, LanguageResource.Profile_Contact.ToLower())
                });

            });
        }

        #endregion Restore contact

        #region Export competitor
        public ActionResult ExportCompetitor(ProfileSearchViewModel searchViewModel)
        {
            // List<ProfileExportExcelModel> profile = new List<ProfileExportExcelModel>();
            int filteredResultsCount;
            //searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
            ProfileRepository repo = new ProfileRepository(_context);

            //Page Size 
            searchViewModel.PageSize = 999999;

            //Page Number
            searchViewModel.PageNumber = 1;
            searchViewModel.Type = ConstProfileType.Competitor;

            searchViewModel.CompetitorType = searchViewModel.SearchCompetitorType;

            //string MienBac = "MIENBAC";
            //string MienTrung = "MIENTRUNG";
            //string MienNam = "MIENNAM";
            var profile = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount)
               .Select(s => new CompetitorExportExcelViewModel
               {
                   ProfileName = !string.IsNullOrEmpty(s.ProfileShortName) ? s.ProfileShortName : s.ProfileName,
                   //Tỉnh/Thành phố
                   ProvinceName = s.ProvinceName,
                   //Vốn pháp định
                   Number1 = s.Number1,
                   //Độ phủ thị trường
                   Number2 = s.Number2,
                   //Website
                   Website = s.Website,
                   //Area1 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == MienBac && p.MappingType == ConstCatalogType.Competitor_Area).FirstOrDefault() != null ? true : false,
                   //Area2 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == MienTrung && p.MappingType == ConstCatalogType.Competitor_Area).FirstOrDefault() != null ? true : false,
                   //Area3 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == MienNam && p.MappingType == ConstCatalogType.Competitor_Area).FirstOrDefault() != null ? true : false,
                   ////Address = s.ExcelAddress,
                   //IsHasFactory = _context.AddressBookModel.Where(p => p.AddressTypeCode == ConstAddressType.NhaMay && p.ProfileId == s.ProfileId).FirstOrDefault() != null ? "Có" : "Không",
                   //FactoryAddress = s.FactoryAddress,
                   CaliberAssessment = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CaliberAssessment).Where(p => p.CatalogCode == s.Dropdownlist7).Select(p => p.CatalogText_vi).FirstOrDefault(),
                   Note = s.Note,
                   DistributionIndustryContent1 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent1" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent2 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent2" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent3 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent3" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent4 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent4" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent5 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent5" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent6 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent6" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent7 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent7" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent8 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent8" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent9 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent9" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent10 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent10" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent11 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent11" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent12 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent12" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent13 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent13" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent14 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent14" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent15 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent15" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent16 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent16" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent17 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent17" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent18 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent18" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent19 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent19" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
                   DistributionIndustryContent20 = _context.Profile_Catalog_Mapping.Where(p => p.ProfileId == s.ProfileId && p.CatalogCode == "DistributionIndustryContent20" && p.MappingType.Contains(ConstCatalogType.DistributionIndustry)).FirstOrDefault() != null ? true : false,
               }).ToList();

            return ExportCompetitorExcel(profile, searchViewModel.CompetitorType, searchViewModel.FileName);
        }


        [ISDAuthorization]
        public FileContentResult ExportCompetitorExcel(List<CompetitorExportExcelViewModel> profile, string CompetitorType, string FileHeader)
        {
            #region //Header
            string fileheader = FileHeader.ToUpper();

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
            #endregion //Header

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileName",
                isAllowedToEdit = false,
                isWraptext = true,
                CustomWidth = 25
            });

            //Danh sách ngành hàng phân phối 
            var competitorDI = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.DistributionIndustry).Where(p => p.CatalogText_en.Contains(CompetitorType)).ToList();
            if (competitorDI != null && competitorDI.Count > 0)
            {
                foreach (var element in competitorDI)
                {
                    columns.Add(new ExcelTemplate()
                    {
                        ColumnName = element.CatalogCode,
                        isAllowedToEdit = false,
                        isWraptext = true,
                        CustomWidth = 13,
                        MergeHeaderTitle = "NGÀNH HÀNG PHÂN PHỐI",
                        isBoolean = true,
                        hasAnotherName = true,
                        AnotherName = element.CatalogText_vi,
                    });
                }
            }

            //Danh sách khu vực hoạt động theo cấu hình danh mục
            //var competitorArea = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Competitor_Area);
            //if (competitorArea != null && competitorArea.Count > 0)
            //{
            //    int competitorAreaIndex = 0;
            //    foreach (var area in competitorArea)
            //    {
            //        competitorAreaIndex++;
            //        columns.Add(new ExcelTemplate()
            //        {
            //            ColumnName = "Area" + competitorAreaIndex,
            //            isAllowedToEdit = false,
            //            isWraptext = true,
            //            CustomWidth = 15,
            //            MergeHeaderTitle = "KHU VỰC",
            //            isBoolean = true,
            //            hasAnotherName = true,
            //            AnotherName = area.CatalogText_vi,
            //        });
            //    }
            //}

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "Area1",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 15,
            //    MergeHeaderTitle = "Khu vực",
            //    isBoolean = true,
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "Area2",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 15,
            //    MergeHeaderTitle = "Khu vực",
            //    isBoolean = true,
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "Area3",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 15,
            //    MergeHeaderTitle = "Khu vực",
            //    isBoolean = true,
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "Address",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 40
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "IsHasFactory",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 18
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "FactoryAddress",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 40
            //});

            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "CaliberAssessment",
            //    isAllowedToEdit = false,
            //    isWraptext = true,
            //    CustomWidth = 15
            //});
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProvinceName",
                isAllowedToEdit = false,
                isWraptext = true,
                CustomWidth = 15
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Number2",
                isAllowedToEdit = false,
                CustomWidth = 15,
                isNumber = true,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Number1",
                isAllowedToEdit = false,
                CustomWidth = 15,
                isNumber = true,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Website",
                isAllowedToEdit = false,
                isWraptext = false,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Note",
                isAllowedToEdit = false,
                isWraptext = true,
                CustomWidth = 30
            });
            #endregion
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, true, HasExtraSheet: false, headerRowMergeCount: 1, PageSize: ePaperSize.A4, Scale: 70, Orientation: eOrientation.Landscape);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion

        #region Export lead
        public ActionResult ExportLead(ProfileSearchViewModel searchViewModel)
        {
            int filteredResultsCount;
            ProfileRepository repo = new ProfileRepository(_context);

            //Page Size 
            searchViewModel.PageSize = 999999;

            //Page Number
            searchViewModel.PageNumber = 1;
            searchViewModel.Type = ConstProfileType.Lead;

            var profile = _unitOfWork.ProfileRepository.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount)
               .Select(s => new LeadExportExcelViewModel
               {
                   ProfileName = !string.IsNullOrEmpty(s.ProfileShortName) ? s.ProfileShortName : s.ProfileName,
                   Investor = s.Investor,
                   Design = s.Design,
                   SaleOfficeName = s.SaleOfficeName,
                   ProvinceName = s.ProvinceName,
                   OpportunityType = s.OpportunityType,
                   ProjectGabarit = s.ProjectGabarit,
                   OpportunityUnit = s.OpportunityUnit,
                   StartDate = s.StartDate,
                   LastEditByName = s.LastEditByName, 

               }).ToList();

            return ExportLeadExcel(profile, "Thông tin ban đầu");
        }


        [ISDAuthorization]
        public FileContentResult ExportLeadExcel(List<LeadExportExcelViewModel> profile, string FileHeader)
        {
            #region //Header
            string fileheader = FileHeader.ToUpper();

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
            #endregion //Header

            #region //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProfileName",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Investor",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Design",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "SaleOfficeName",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProvinceName",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "OpportunityType",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ProjectGabarit",
                CustomWidth = 20,
                isNumber = true,
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "OpportunityUnit",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "StartDate",
                isDateTime = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "LastEditByName",
                isWraptext = true,
                CustomWidth = 30
            });
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Note",
                CustomWidth = 30
            });
            #endregion
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(profile, columns, heading, true, HasExtraSheet: false, headerRowMergeCount: 1, PageSize: ePaperSize.A4, Scale: 70, Orientation: eOrientation.Landscape);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion
    }
}