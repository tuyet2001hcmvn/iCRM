using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Customer;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ConstructionController : BaseController
    {
        // GET: Construction
        //Căn mẫu
        public ActionResult _ListInternal(Guid? id, string Type = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.Type = Type;
                var result = (from p in _context.ProfileModel
                              join a in _context.Profile_Opportunity_PartnerModel on p.ProfileId equals a.PartnerId
                              join acc in _context.AccountModel on a.CreateBy equals acc.AccountId into ag
                              from ac in ag.DefaultIfEmpty()
                              join s in _context.SalesEmployeeModel on ac.EmployeeCode equals s.SalesEmployeeCode into sg
                              from emp in sg.DefaultIfEmpty()
                                  //Province
                              join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                              from province in prG.DefaultIfEmpty()
                                  //District
                              join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                              from district in dG.DefaultIfEmpty()
                                  //Ward
                              join w in _context.WardModel on p.WardId equals w.WardId into wG
                              from ward in wG.DefaultIfEmpty()
                              //Hạng mục thi công
                              join c in _context.CatalogModel on new { CatalogCode = a.ConstructionCategory, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                              from cat in cG.DefaultIfEmpty()

                              where a.ProfileId == id && a.PartnerType == ConstPartnerType.CanMau //Căn mẫu
                              orderby a.IsMain descending, a.CreateTime descending
                              select new ProfileViewModel()
                              {
                                  OpportunityPartnerId = a.OpportunityPartnerId,
                                  ProfileId = p.ProfileId,
                                  ProfileCode = p.ProfileCode,
                                  ProfileName = p.ProfileName,
                                  ProfileShortName = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileCode + "(Chưa đặt tên ngắn)",
                                  Address = p.Address,
                                  ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                  DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                  WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                  CreateBy = a.CreateBy,
                                  CreateTime = a.CreateTime,
                                  IsMain = a.IsMain,
                                  CreateUser = emp.SalesEmployeeShortName,
                                  ConstructionCategory = cat.CatalogText_vi,
                                  ProjectValue = a.ProjectValue,
                                  IsWon = a.IsWon
                              }).ToList();

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);

                        //Hạng mục thi công
                        //var constructionCategoryList = (from p in _context.Profile_Opportunity_MaterialModel
                        //                                join c in _context.CatalogModel on new { CatalogCode = p.MaterialCode, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                        //                                from cat in cG.DefaultIfEmpty()
                        //                                where p.ProfileId == item.ProfileId && p.MaterialType == ConstMaterialType.HangMucThiCong
                        //                                orderby cat.OrderIndex
                        //                                select cat.CatalogText_vi).ToList();
                        //if (constructionCategoryList != null && constructionCategoryList.Count > 0)
                        //{
                        //    item.ConstructionCategory = string.Join(", ", constructionCategoryList);
                        //}
                    }
                }
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }
                return PartialView(result);
            });
        }
        //Đại trà
        public ActionResult _ListCompetitor(Guid? id, string Type = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.Type = Type;
                var result = (from p in _context.ProfileModel
                              join a in _context.Profile_Opportunity_PartnerModel on p.ProfileId equals a.PartnerId
                              join acc in _context.AccountModel on a.CreateBy equals acc.AccountId into ag
                              from ac in ag.DefaultIfEmpty()
                              join s in _context.SalesEmployeeModel on ac.EmployeeCode equals s.SalesEmployeeCode into sg
                              from emp in sg.DefaultIfEmpty()
                                  //Province
                              join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                              from province in prG.DefaultIfEmpty()
                                  //District
                              join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                              from district in dG.DefaultIfEmpty()
                                  //Ward
                              join w in _context.WardModel on p.WardId equals w.WardId into wG
                              from ward in wG.DefaultIfEmpty()
                                  //Hạng mục thi công
                              join c in _context.CatalogModel on new { CatalogCode = a.ConstructionCategory, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                              from cat in cG.DefaultIfEmpty()
                              where a.ProfileId == id && a.PartnerType == ConstPartnerType.DaiTra //Đại trà
                              orderby a.IsMain descending, a.CreateTime descending
                              select new ProfileViewModel()
                              {
                                  OpportunityPartnerId = a.OpportunityPartnerId,
                                  ProfileId = p.ProfileId,
                                  ProfileCode = p.ProfileCode,
                                  ProfileName = p.ProfileName,
                                  ProfileShortName = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileCode + "(Chưa đặt tên ngắn)",
                                  Address = p.Address,
                                  ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                  DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                  WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                  CreateBy = a.CreateBy,
                                  CreateTime = a.CreateTime,
                                  IsMain = a.IsMain,
                                  CreateUser = emp.SalesEmployeeShortName,
                                  ConstructionCategory = cat.CatalogText_vi,
                                  ProjectValue = a.ProjectValue,
                                  IsWon = a.IsWon
                              }).ToList();

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);

                        //Hạng mục thi công
                        //var constructionCategoryList = (from p in _context.Profile_Opportunity_MaterialModel
                        //                                join c in _context.CatalogModel on new { CatalogCode = p.MaterialCode, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                        //                                from cat in cG.DefaultIfEmpty()
                        //                                where p.ProfileId == item.ProfileId && p.MaterialType == ConstMaterialType.HangMucThiCong
                        //                                orderby cat.OrderIndex
                        //                                select cat.CatalogText_vi).ToList();
                        //if (constructionCategoryList != null && constructionCategoryList.Count > 0)
                        //{
                        //    item.ConstructionCategory = string.Join(", ", constructionCategoryList);
                        //}
                    }
                }
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }
                return PartialView(result);
            });
        }
        //Thầu phụ
        public ActionResult _ListSubcontractors(Guid? id, string Type = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.Type = Type;
                var result = (from p in _context.ProfileModel
                              join a in _context.Profile_Opportunity_PartnerModel on p.ProfileId equals a.PartnerId
                              join acc in _context.AccountModel on a.CreateBy equals acc.AccountId into ag
                              from ac in ag.DefaultIfEmpty()
                              join s in _context.SalesEmployeeModel on ac.EmployeeCode equals s.SalesEmployeeCode into sg
                              from emp in sg.DefaultIfEmpty()
                                  //Province
                              join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                              from province in prG.DefaultIfEmpty()
                                  //District
                              join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                              from district in dG.DefaultIfEmpty()
                                  //Ward
                              join w in _context.WardModel on p.WardId equals w.WardId into wG
                              from ward in wG.DefaultIfEmpty()
                                  //Hạng mục thi công
                              join c in _context.CatalogModel on new { CatalogCode = a.ConstructionCategory, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                              from cat in cG.DefaultIfEmpty()
                              where a.ProfileId == id && a.PartnerType == ConstPartnerType.ThauPhu //Thầu phụ
                              orderby a.IsMain descending, a.CreateTime descending
                              select new ProfileViewModel()
                              {
                                  OpportunityPartnerId = a.OpportunityPartnerId,
                                  ProfileId = p.ProfileId,
                                  ProfileCode = p.ProfileCode,
                                  ProfileName = p.ProfileName,
                                  ProfileShortName = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileCode + "(Chưa đặt tên ngắn)",
                                  Address = p.Address,
                                  ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                  DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                  WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                  CreateBy = a.CreateBy,
                                  CreateTime = a.CreateTime,
                                  IsMain = a.IsMain,
                                  CreateUser = emp.SalesEmployeeShortName,
                                  ConstructionCategory = cat.CatalogText_vi,
                                  ProjectValue = a.ProjectValue,
                                  IsWon = a.IsWon
                              }).ToList();

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);

                        //Hạng mục thi công
                        //var constructionCategoryList = (from p in _context.Profile_Opportunity_MaterialModel
                        //                                join c in _context.CatalogModel on new { CatalogCode = p.MaterialCode, CatalogTypeCode = ConstCatalogType.HandoverFurniture } equals new { c.CatalogCode, c.CatalogTypeCode } into cG
                        //                                from cat in cG.DefaultIfEmpty()
                        //                                where p.ProfileId == item.ProfileId && p.MaterialType == ConstMaterialType.HangMucThiCong
                        //                                orderby cat.OrderIndex
                        //                                select cat.CatalogText_vi).ToList();
                        //if (constructionCategoryList != null && constructionCategoryList.Count > 0)
                        //{
                        //    item.ConstructionCategory = string.Join(", ", constructionCategoryList);
                        //}
                    }
                }
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }
                return PartialView(result);
            });
        }

        #region Search data
        /// <summary>
        /// Search popup Profile and form input base on Type
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="hasNoContact"></param>
        /// <param name="Type"></param>
        /// <param name="hasNoSearch"></param>
        /// <returns></returns>
        public ActionResult _ProfileFormSearch(Guid? ProfileId, bool? hasNoContact, string ProfileType, Guid? OpportunityPartnerId)
        {
            ProfileSearchViewModel model = new ProfileSearchViewModel();
            model.ProfileId = ProfileId;
            model.hasNoContact = hasNoContact;
            model.ProfileType = ProfileType;

            ViewBag.OpportunityPartnerId = OpportunityPartnerId;
            string ConstructionCategory = string.Empty;
            bool? IsWon = null;
            if (OpportunityPartnerId.HasValue)
            {
                var info = (from p in _context.ProfileModel
                            join a in _context.Profile_Opportunity_PartnerModel on p.ProfileId equals a.PartnerId
                            join acc in _context.AccountModel on a.CreateBy equals acc.AccountId into ag
                            from ac in ag.DefaultIfEmpty()
                            join s in _context.SalesEmployeeModel on ac.EmployeeCode equals s.SalesEmployeeCode into sg
                            from emp in sg.DefaultIfEmpty()
                                //Province
                            join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                            from province in prG.DefaultIfEmpty()
                                //District
                            join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                            from district in dG.DefaultIfEmpty()
                                //Ward
                            join w in _context.WardModel on p.WardId equals w.WardId into wG
                            from ward in wG.DefaultIfEmpty()

                            where a.OpportunityPartnerId == OpportunityPartnerId
                            orderby a.IsMain descending, a.CreateTime descending
                            select new ProfileViewModel()
                            {
                                OpportunityPartnerId = a.OpportunityPartnerId,
                                ProfileId = p.ProfileId,
                                ProfileCode = p.ProfileCode,
                                ProfileName = p.ProfileName,
                                ProfileShortName = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileCode + "(Chưa đặt tên ngắn)",
                                ConstructionCategory = a.ConstructionCategory,
                                ProjectValue = a.ProjectValue,
                                IsWon = a.IsWon
                            }).FirstOrDefault();
                if (info != null)
                {
                    model.PartnerInfo = info;
                    ConstructionCategory = info.ConstructionCategory;
                    IsWon = info.IsWon;
                }
            }

            var catalogList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                                                            && p.CatalogCode != ConstCustomerType.Contact
                                                            && p.Actived == true)
                                                   .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");

            //if (ProfileType == ConstProfileType.Competitor)
            //{
            //    ViewBag.PopupTitle = "TÌM KIẾM THÔNG TIN ĐỐI THỦ";
            //}

            //hạng mục thi công => lấy dữ liệu từ tiêu chuẩn bàn giao
            var constructionCategoryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.HandoverFurniture);
            ViewBag.ConstructionCategory = new SelectList(constructionCategoryList, "CatalogCode", "CatalogText_vi", ConstructionCategory);

            //trúng thầu
            var IsWonList = new List<ISDSelectBoolItem>();
            IsWonList.Add(new ISDSelectBoolItem()
            {
                id = true,
                name = "Trúng thầu"
            });
            IsWonList.Add(new ISDSelectBoolItem()
            {
                id = false,
                name = "Thất bại"
            });
            ViewBag.IsWon = new SelectList(IsWonList, "id", "name", IsWon);

            return PartialView("_ProfileFormSearch", model);
        }

        [HttpPost]
        //Hàm tìm kiếm dành cho popup
        public ActionResult _ProfileFormSearchResult(ProfileSearchViewModel searchViewModel, DatatableViewModel model)
        {
            return ExecuteSearch(() =>
            {
                //Customer and Business => Type = Account
                if (string.IsNullOrEmpty(searchViewModel.ProfileType))
                {
                    searchViewModel.ProfileType = ConstProfileType.Account;
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
        #endregion

        #region Handle data
        [HttpPost]
        public ActionResult SaveInternal(Guid? ProfileId, Guid? ProfileIsChoosen, string ConstructionCategory, decimal? ProjectValue, bool? IsWon)
        {
            return ExecuteContainer(() =>
            {
                var existsPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.CanMau).FirstOrDefault();
                #region Create
                Profile_Opportunity_PartnerModel partner = new Profile_Opportunity_PartnerModel();
                partner.OpportunityPartnerId = Guid.NewGuid();
                partner.ProfileId = ProfileId;
                partner.PartnerId = ProfileIsChoosen;
                partner.PartnerType = ConstPartnerType.CanMau; //Căn mẫu
                partner.CreateBy = CurrentUser.AccountId;
                partner.CreateTime = DateTime.Now;
                partner.IsMain = existsPartner != null ? false : true;
                partner.ConstructionCategory = ConstructionCategory;
                partner.ProjectValue = ProjectValue;
                partner.IsWon = IsWon;

                _context.Entry(partner).State = EntityState.Added;

                //Hạng mục thi công
                //if (ConstructionCategory != null && ConstructionCategory.Count > 0)
                //{
                //    foreach (var category in ConstructionCategory)
                //    {
                //        Profile_Opportunity_MaterialModel newCategory = new Profile_Opportunity_MaterialModel();
                //        newCategory.OpportunityMaterialId = Guid.NewGuid();
                //        newCategory.ProfileId = ProfileIsChoosen;
                //        newCategory.MaterialCode = category;
                //        newCategory.MaterialType = ConstMaterialType.HangMucThiCong;
                //        newCategory.CreateBy = CurrentUser.AccountId;
                //        newCategory.CreateTime = DateTime.Now;

                //        _context.Entry(newCategory).State = EntityState.Added;
                //    }
                //}

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TabConstruction.ToLower()),
                    RedirectUrl = "/Customer/Profile/Edit/?id=" + ProfileId,
                });
                #endregion
            });
        }

        [HttpPost]
        public ActionResult SetMainInternal(Guid? OpportunityPartnerId)
        {
            return ExecuteContainer(() =>
            {
                var partner = _context.Profile_Opportunity_PartnerModel.Where(p => p.OpportunityPartnerId == OpportunityPartnerId).FirstOrDefault();
                if (partner != null)
                {
                    partner.IsMain = true;

                    var remainPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == partner.ProfileId && p.OpportunityPartnerId != OpportunityPartnerId && p.PartnerType == ConstPartnerType.CanMau).ToList();
                    if (remainPartner != null && remainPartner.Count > 0)
                    {
                        foreach (var remain in remainPartner)
                        {
                            remain.IsMain = false;
                            _context.Entry(remain).State = EntityState.Modified;
                        }
                    }

                    _context.Entry(partner).State = EntityState.Modified;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TabConstruction.ToLower()),
                        RedirectUrl = "/Customer/Profile/Edit/?id=" + partner.ProfileId,
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Không tìm thấy dữ liệu phù hợp",
                    });
                }
            });
        }

        [HttpPost]
        public ActionResult SetMainCompetitor(Guid? OpportunityPartnerId)
        {
            return ExecuteContainer(() =>
            {
                var partner = _context.Profile_Opportunity_PartnerModel.Where(p => p.OpportunityPartnerId == OpportunityPartnerId).FirstOrDefault();
                if (partner != null)
                {
                    partner.IsMain = true;

                    var remainPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == partner.ProfileId && p.OpportunityPartnerId != OpportunityPartnerId && p.PartnerType == ConstPartnerType.DaiTra).ToList();
                    if (remainPartner != null && remainPartner.Count > 0)
                    {
                        foreach (var remain in remainPartner)
                        {
                            remain.IsMain = false;
                            _context.Entry(remain).State = EntityState.Modified;
                        }
                    }

                    _context.Entry(partner).State = EntityState.Modified;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TabConstruction.ToLower()),
                        RedirectUrl = "/Customer/Profile/Edit/?id=" + partner.ProfileId,
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Không tìm thấy dữ liệu phù hợp",
                    });
                }
            });
        }

        [HttpPost]
        public ActionResult SetMainSubcontractors(Guid? OpportunityPartnerId)
        {
            return ExecuteContainer(() =>
            {
                var partner = _context.Profile_Opportunity_PartnerModel.Where(p => p.OpportunityPartnerId == OpportunityPartnerId).FirstOrDefault();
                if (partner != null)
                {
                    partner.IsMain = true;

                    var remainPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == partner.ProfileId && p.OpportunityPartnerId != OpportunityPartnerId && p.PartnerType == ConstPartnerType.ThauPhu).ToList();
                    if (remainPartner != null && remainPartner.Count > 0)
                    {
                        foreach (var remain in remainPartner)
                        {
                            remain.IsMain = false;
                            _context.Entry(remain).State = EntityState.Modified;
                        }
                    }

                    _context.Entry(partner).State = EntityState.Modified;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TabConstruction.ToLower()),
                        RedirectUrl = "/Customer/Profile/Edit/?id=" + partner.ProfileId,
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Không tìm thấy dữ liệu phù hợp",
                    });
                }
            });
        }

        [HttpPost]
        public ActionResult SaveCompetitor(Guid? ProfileId, Guid? ProfileIsChoosen, string ConstructionCategory, decimal? ProjectValue, bool? IsWon)
        {
            return ExecuteContainer(() =>
            {
                var existsPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.DaiTra).FirstOrDefault();
                #region Create
                Profile_Opportunity_PartnerModel partner = new Profile_Opportunity_PartnerModel();
                partner.OpportunityPartnerId = Guid.NewGuid();
                partner.ProfileId = ProfileId;
                partner.PartnerId = ProfileIsChoosen;
                partner.PartnerType = ConstPartnerType.DaiTra; //Đại trà
                partner.CreateBy = CurrentUser.AccountId;
                partner.CreateTime = DateTime.Now;
                partner.IsMain = existsPartner != null ? false : true;
                partner.ConstructionCategory = ConstructionCategory;
                partner.ProjectValue = ProjectValue;
                partner.IsWon = IsWon;

                _context.Entry(partner).State = EntityState.Added;

                //Hạng mục thi công
                //if (ConstructionCategory != null && ConstructionCategory.Count > 0)
                //{
                //    foreach (var category in ConstructionCategory)
                //    {
                //        Profile_Opportunity_MaterialModel newCategory = new Profile_Opportunity_MaterialModel();
                //        newCategory.OpportunityMaterialId = Guid.NewGuid();
                //        newCategory.ProfileId = ProfileIsChoosen;
                //        newCategory.MaterialCode = category;
                //        newCategory.MaterialType = ConstMaterialType.HangMucThiCong;
                //        newCategory.CreateBy = CurrentUser.AccountId;
                //        newCategory.CreateTime = DateTime.Now;

                //        _context.Entry(newCategory).State = EntityState.Added;
                //    }
                //}

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TabConstruction.ToLower()),
                    RedirectUrl = "/Customer/Profile/Edit/?id=" + ProfileId,
                });
                #endregion
            });
        }

        [HttpPost]
        public ActionResult SaveSubcontractors(Guid? ProfileId, Guid? ProfileIsChoosen, string ConstructionCategory, decimal? ProjectValue, bool? IsWon)
        {
            return ExecuteContainer(() =>
            {
                var existsPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.ThauPhu).FirstOrDefault();
                #region Create
                Profile_Opportunity_PartnerModel partner = new Profile_Opportunity_PartnerModel();
                partner.OpportunityPartnerId = Guid.NewGuid();
                partner.ProfileId = ProfileId;
                partner.PartnerId = ProfileIsChoosen;
                partner.PartnerType = ConstPartnerType.ThauPhu; //Thầu phụ
                partner.CreateBy = CurrentUser.AccountId;
                partner.CreateTime = DateTime.Now;
                partner.IsMain = existsPartner != null ? false : true;
                partner.ConstructionCategory = ConstructionCategory;
                partner.ProjectValue = ProjectValue;
                partner.IsWon = IsWon;

                _context.Entry(partner).State = EntityState.Added;

                //Hạng mục thi công
                //if (ConstructionCategory != null && ConstructionCategory.Count > 0)
                //{
                //    foreach (var category in ConstructionCategory)
                //    {
                //        Profile_Opportunity_MaterialModel newCategory = new Profile_Opportunity_MaterialModel();
                //        newCategory.OpportunityMaterialId = Guid.NewGuid();
                //        newCategory.ProfileId = ProfileIsChoosen;
                //        newCategory.MaterialCode = category;
                //        newCategory.MaterialType = ConstMaterialType.HangMucThiCong;
                //        newCategory.CreateBy = CurrentUser.AccountId;
                //        newCategory.CreateTime = DateTime.Now;

                //        _context.Entry(newCategory).State = EntityState.Added;
                //    }
                //}

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Subcontractors.ToLower()),
                    RedirectUrl = "/Customer/Profile/Edit/?id=" + ProfileId,
                });
                #endregion
            });
        }

        [HttpPost]
        public ActionResult SaveEditPartner(Guid? OpportunityPartnerId, string ConstructionCategory, decimal? ProjectValue, bool? IsWon)
        {
            return ExecuteContainer(() =>
            {
                var existsPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.OpportunityPartnerId == OpportunityPartnerId).FirstOrDefault();
                if (existsPartner != null)
                {
                    existsPartner.ConstructionCategory = ConstructionCategory;
                    existsPartner.ProjectValue = ProjectValue;
                    existsPartner.IsWon = IsWon;

                    _context.Entry(existsPartner).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                });
            });
        }
        #endregion Handle data

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                //Căn mẫu
                var internalOp = _context.Profile_Opportunity_PartnerModel.FirstOrDefault(p => p.OpportunityPartnerId == id);
                if (internalOp != null)
                {
                    var ProfileId = internalOp.ProfileId;
                    _context.Entry(internalOp).State = EntityState.Deleted;

                    var constructionCategory = _context.Profile_Opportunity_MaterialModel.Where(p => p.ProfileId == id).ToList();
                    if (constructionCategory != null)
                    {
                        _context.Profile_Opportunity_MaterialModel.RemoveRange(constructionCategory);
                    }
                    _context.SaveChanges();

                    var remainInternal = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.CanMau).ToList();
                    if (remainInternal.Count == 1)
                    {
                        foreach (var item in remainInternal)
                        {
                            item.IsMain = true;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        _context.SaveChanges();
                    }

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TabConstruction.ToLower())
                    });
                }

                //Đại trà
                var competitorOp = _context.Profile_Opportunity_PartnerModel.FirstOrDefault(p => p.OpportunityPartnerId == id);
                if (competitorOp != null)
                {
                    var ProfileId = competitorOp.ProfileId;
                    _context.Entry(competitorOp).State = EntityState.Deleted;

                    var constructionCategory = _context.Profile_Opportunity_MaterialModel.Where(p => p.ProfileId == id).ToList();
                    if (constructionCategory != null)
                    {
                        _context.Profile_Opportunity_MaterialModel.RemoveRange(constructionCategory);
                    }
                    _context.SaveChanges();

                    var remainCompetitor = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.DaiTra).ToList();
                    if (remainCompetitor.Count == 1)
                    {
                        foreach (var item in remainCompetitor)
                        {
                            item.IsMain = true;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        _context.SaveChanges();
                    }

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TabConstruction.ToLower())
                    });
                }

                //Thầu phụ
                var subcontractors = _context.Profile_Opportunity_PartnerModel.FirstOrDefault(p => p.OpportunityPartnerId == id);
                if (subcontractors != null)
                {
                    var ProfileId = subcontractors.ProfileId;
                    _context.Entry(subcontractors).State = EntityState.Deleted;

                    var constructionCategory = _context.Profile_Opportunity_MaterialModel.Where(p => p.ProfileId == id).ToList();
                    if (constructionCategory != null)
                    {
                        _context.Profile_Opportunity_MaterialModel.RemoveRange(constructionCategory);
                    }
                    _context.SaveChanges();

                    var remainCompetitor = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.ThauPhu).ToList();
                    if (remainCompetitor.Count == 1)
                    {
                        foreach (var item in remainCompetitor)
                        {
                            item.IsMain = true;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        _context.SaveChanges();
                    }

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Subcontractors.ToLower())
                    });
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = "Không tìm thấy dữ liệu phù hợp"
                });
            });
        }
        #endregion
    }
}