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
    public class GeneralContractorController : BaseController
    {
        // GET: GeneralContractor
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
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

                              where a.ProfileId == id && a.PartnerType == ConstPartnerType.TongThau //Tổng thầu
                              orderby a.IsMain descending, a.CreateTime descending
                              select new ProfileViewModel()
                              {
                                  OpportunityPartnerId = a.OpportunityPartnerId,
                                  ProfileId = p.ProfileId,
                                  ProfileCode = p.ProfileCode,
                                  ProfileName = !string.IsNullOrEmpty(p.ProfileShortName) ? p.ProfileShortName : p.ProfileCode + "(Chưa đặt tên ngắn)",
                                  Address = p.Address,
                                  ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                  DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                  WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                  CreateBy = a.CreateBy,
                                  CreateTime = a.CreateTime,
                                  IsMain = a.IsMain,
                                  CreateUser = emp.SalesEmployeeShortName,
                              }).ToList();

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);
                    }
                }
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }
                return PartialView(result);
            });
        }
        #region Handle data
        [HttpPost]
        public ActionResult Save(Guid? ProfileId, Guid? PartnerId)
        {
            return ExecuteContainer(() =>
            {
                var existsPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.TongThau).FirstOrDefault();
                #region Create
                Profile_Opportunity_PartnerModel partner = new Profile_Opportunity_PartnerModel();
                partner.OpportunityPartnerId = Guid.NewGuid();
                partner.ProfileId = ProfileId;
                partner.PartnerId = PartnerId;
                partner.PartnerType = ConstPartnerType.TongThau; //Tổng thầu
                partner.CreateBy = CurrentUser.AccountId;
                partner.CreateTime = DateTime.Now;
                partner.IsMain = existsPartner != null ? false : true;

                _context.Entry(partner).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TabGeneralContractor.ToLower()),
                    RedirectUrl = "/Customer/Profile/Edit/?id=" + ProfileId,
                });
                #endregion
            });
        }

        [HttpPost]
        public ActionResult SetMain(Guid? OpportunityPartnerId)
        {
            return ExecuteContainer(() =>
            {
                var partner = _context.Profile_Opportunity_PartnerModel.Where(p => p.OpportunityPartnerId == OpportunityPartnerId).FirstOrDefault();
                if (partner != null)
                {
                    partner.IsMain = true;

                    var remainPartner = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == partner.ProfileId && p.OpportunityPartnerId != OpportunityPartnerId && p.PartnerType == ConstPartnerType.TongThau).ToList();
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
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TabGeneralContractor.ToLower()),
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
        #endregion Handle data

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var design = _context.Profile_Opportunity_PartnerModel.FirstOrDefault(p => p.OpportunityPartnerId == id);
                if (design != null)
                {
                    var ProfileId = design.ProfileId;
                    _context.Entry(design).State = EntityState.Deleted;
                    _context.SaveChanges();

                    var remainDesign = _context.Profile_Opportunity_PartnerModel.Where(p => p.ProfileId == ProfileId && p.PartnerType == ConstPartnerType.TongThau).ToList();
                    if (remainDesign.Count == 1)
                    {
                        foreach (var item in remainDesign)
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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TabConsultingDesign.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion
    }
}