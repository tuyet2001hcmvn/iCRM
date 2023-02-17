using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace Marketing.Controllers
{
    public class TargetGroupController : BaseController
    {
        // GET: TargetGroup
        public ActionResult Index(string Type = null)
        {
            string pageUrl = "/Marketing/TargetGroup";
            var parameter = string.Empty;
            if (Type == ConstMarketingType.Marketing)
            {
                parameter = "?Type=Marketing";
            }
            else if (Type == ConstMarketingType.Event)
            {
                parameter = "?Type=Event";
            }

            ViewBag.PageId = GetPageId(pageUrl, parameter);
            CreateViewBag(Type: Type);
            return View();
        }


        public ActionResult Create(string Type)
        { 
            CreateViewBag(Type: Type);
            return View();
        }
       
        public ActionResult Edit(Guid Id,string Type)
        {
            ViewBag.Id = Id;
            CreateViewBag(Type: Type);
            return View();                  
        }
        public ActionResult LoadMember()
        {
            return PartialView();
        }
        #region export to excel
        public ActionResult ExportExcel(Guid? targetGroupId)
        {
            //Guid id = Guid.Parse(targetGroupId);
            var result = (from member in _context.MemberOfTargetGroupModel
                         join profile in _context.ProfileModel on member.ProfileId equals profile.ProfileId
                         where member.TargetGroupId == targetGroupId
                          select new MemberOfTargetGroupExcelModel
                         {
                             ProfileCode = profile.ProfileCode,
                             ProfileName = profile.ProfileName,
                             Phone = profile.Phone,
                             Email = profile.Email
                         }).ToList();
            return Export(result);
        }
        public ActionResult Export(List<MemberOfTargetGroupExcelModel> viewModel)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "Thông tin khách hàng trong nhóm mục tiêu";

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });

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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion

        /// <summary>
        /// Search popup Profile and Contact base on Type
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="hasNoContact"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult _ProfileSearch(string ProfileType)
        {
            ProfileSearchViewModel model = new ProfileSearchViewModel();
            model.ProfileType = ProfileType;

            var catalogList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                                                            && p.CatalogCode != ConstCustomerType.Contact
                                                            && p.Actived == true)
                                                   .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");
            CreateViewBagSearch(ProfileType);
            return PartialView("_ProfileSearch", model);
        }


        [HttpPost]
        //Hàm tìm kiếm dành cho popup
        public ActionResult _ProfileSearchResult(ProfileSearchViewModel searchViewModel, DatatableViewModel model, Guid TargetGroupId)
        {
            return ExecuteSearch(() =>
            {
                searchViewModel.Type = searchViewModel.ProfileType;

                int filteredResultsCount;
                int totalResultsCount = model.length;
                //Page Size 
                searchViewModel.PageSize = model.length;
                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;

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
                        item.InTarget =  _context.MemberOfTargetGroupModel.Where(x => x.TargetGroupId == TargetGroupId && x.ProfileId == item.ProfileId).Count() > 0;
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

        public ActionResult AddMember(Guid TargetGroupId, Guid ProfileId)
        {
            return ExecuteContainer(() =>
            {
                //Thêm khách hàng vào nhóm mục tiêu

                MemberOfTargetGroupModel member = _context.MemberOfTargetGroupModel.Where(x => x.TargetGroupId == TargetGroupId && x.ProfileId == ProfileId).FirstOrDefault();
                if (member == null)
                {
                    member = new MemberOfTargetGroupModel
                    {
                        TargetGroupId = TargetGroupId,
                        ProfileId = ProfileId,
                    };
                    _context.Entry(member).State = EntityState.Added;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MemberOfTargetGroup.ToLower()),
                });
            });
        }
        public ActionResult AddListMember(Guid TargetGroupId, ProfileSearchViewModel searchViewModel)
        {
            return ExecuteContainer(() =>
            {
                searchViewModel.Type = searchViewModel.ProfileType;
                int filteredResultsCount; 
                //Page Size 
                searchViewModel.PageSize = 500000;
                //Page Number
                searchViewModel.PageNumber = 1;
                ProfileRepository repo = new ProfileRepository(_context);
                //var query = repo.SearchQuery(searchViewModel);

                //var res = CustomSearchRepository.CustomSearchFunc<ProfileSearchResultViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
                var res = repo.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);

                foreach (var item in res)
                {
                    MemberOfTargetGroupModel member = _context.MemberOfTargetGroupModel.Where(x => x.TargetGroupId == TargetGroupId && x.ProfileId == item.ProfileId).FirstOrDefault();
                    if (member == null)
                    {
                        member = new MemberOfTargetGroupModel
                        {
                            TargetGroupId = TargetGroupId,
                            ProfileId = item.ProfileId,
                        };
                        _context.Entry(member).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MemberOfTargetGroup.ToLower()),
                });
            });
        }

        public ActionResult RemoveMember(Guid TargetGroupId, Guid ProfileId)
        {
            return ExecuteContainer(() =>
            {
                //Thêm khách hàng vào nhóm mục tiêu

                MemberOfTargetGroupModel member = _context.MemberOfTargetGroupModel.Where(x => x.TargetGroupId == TargetGroupId && x.ProfileId == ProfileId).FirstOrDefault();
                if (member != null)
                {
                    _context.Entry(member).State = EntityState.Deleted;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MemberOfTargetGroup.ToLower()),
                });
            });
        }
        public ActionResult RemoveListMember(Guid TargetGroupId, ProfileSearchViewModel searchViewModel)
        {
            return ExecuteContainer(() =>
            {
                searchViewModel.Type = searchViewModel.ProfileType;
                int filteredResultsCount;
                //Page Size 
                searchViewModel.PageSize = 500000;
                //Page Number
                searchViewModel.PageNumber = 1;
                ProfileRepository repo = new ProfileRepository(_context);
                var res = repo.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);

                foreach (var item in res)
                {
                    MemberOfTargetGroupModel member = _context.MemberOfTargetGroupModel.Where(x => x.TargetGroupId == TargetGroupId && x.ProfileId == item.ProfileId).FirstOrDefault();
                    if (member != null)
                    {
                        _context.Entry(member).State = EntityState.Deleted;
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MemberOfTargetGroup.ToLower()),
                });
            });
        }



        public void CreateViewBag(string Type = null)
        {
            ViewBag.Type = Type;
            var title = (from p in _context.PageModel
                         where p.PageUrl == "/Marketing/TargetGroup"
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;


        }

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
    }
}