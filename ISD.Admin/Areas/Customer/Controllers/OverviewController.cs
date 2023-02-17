using ISD.Constant;
using ISD.Core;
using ISD.Repositories;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    [AllowAnonymous]
    public class OverviewController : BaseController
    {
        // GET: Overview
        public ActionResult _Index(Guid id)
        {
            var ProfileView = _unitOfWork.ProfileRepository.GetViewBy(id, CurrentUser.CompanyCode);
            //Nhóm KH
            var profileGroupList = _unitOfWork.ProfileGroupRepository.GetListProfileGroupBy(id, CurrentUser.CompanyCode);
            ProfileView.profileGroupList = profileGroupList;

            //Bảo hành
            ViewBag.Ticket = GetTask(id, ConstWorkFlowCategory.TICKET);
            //Điểm trưng bày
            ViewBag.GTB = GetTask(id, ConstWorkFlowCategory.GTB);
            //Catalogue
            ViewBag.Catalogue = _unitOfWork.CatalogueRepository.GetAll(id)
                                           .Where(p => p.isDeleted == null || p.isDeleted == false).OrderByDescending(p => p.CreatedDate).ToList();
            //Công ty
            ViewBag.OverviewCompany = _unitOfWork.ProfileRepository.GetCompanyInOverviewProfile(id).ToList();
            //Thị hiếu
            ViewBag.CustomerTaste = _unitOfWork.CustomerTasteRepository.GetTastes(id);
            //Nhân viên phụ trách
            if (ProfileView.CustomerTypeCode == ConstProfileType.Opportunity)
            {
                ViewBag.PersonInCharge = _unitOfWork.PersonInChargeRepository.List(id, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD);
                ViewBag.PersonInCharge2 = _unitOfWork.PersonInChargeRepository.List(id, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVSalesAdmin);
                ViewBag.PersonInCharge3 = _unitOfWork.PersonInChargeRepository.List(id, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVSpec);

                // Chủ đầu tư
                ViewBag.Investor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ChuDauTu).Where(p => p.IsMain == true).ToList();
                ProfileView.Investor = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Investor).Select(x => x.ProfileName));

                //Thiết kế
                ViewBag.Design = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ThietKe).Where(p => p.IsMain == true).ToList();
                ProfileView.Designer = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Design).Select(x => x.ProfileName));

                //Tổng thầu
                ViewBag.Contractor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.TongThau).Where(p => p.IsMain == true).ToList();
                ProfileView.Contractor = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Contractor).Select(x => x.ProfileName));

                //Đơn vị tư vấn giám sát
                ViewBag.Consulting = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DonViTuVan).Where(p => p.IsMain == true).ToList();
                ProfileView.ConsultingUnit = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Consulting).Select(x => x.ProfileName));


                //Thi công
                ViewBag.Internal = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.CanMau).Where(p => p.IsMain == true).ToList();
                ViewBag.Competitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DaiTra).Where(p => p.IsMain == true).ToList();
                ViewBag.Competitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ThauPhu).Where(p => p.IsMain == true).ToList();

                //Đối thủ
                ViewBag.OppCompetitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DoiThu).ToList();

                //Hoạt động
                var _activitiesRepository = new ActivitiesRepository(_context);
                ViewBag.OpportunityActivities = _activitiesRepository.GetAll(id);
                ViewBag.IsView = true;
            }
            else
            {
                ViewBag.PersonInCharge = _unitOfWork.PersonInChargeRepository.List(id, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD_A);
            }
            //Phong ban phụ trách
            ViewBag.RoleInCharge = GetRoleInCharge(id);
            //Liên hệ
            ViewBag.ProfileContact = GetProfileContact(id);
            //Ghé thăm
            ViewBag.Appointment = _unitOfWork.AppointmentRepository.Search(id);
            //Thăm hỏi
            ViewBag.Activities = GetTask(id, ConstWorkFlowCategory.THKH).OrderByDescending(p => p.StartDate).ToList();
            //Nhiệm vụ
            ViewBag.Mission = GetTask(id, ConstWorkFlowCategory.ACTIVITIES);
            //Đăng ký bảo hành
            ViewBag.ProductWarranty = GetProductWarranty(id, CurrentUser.CompanyCode);

            return PartialView(ProfileView);
        }    
        [AllowAnonymous]
        public PartialViewResult _Detail(Guid id, string CompanyCode)
        {
            var ProfileView = _unitOfWork.ProfileRepository.GetViewBy(id, CompanyCode);
            //Nhóm KH
            var profileGroupList = _unitOfWork.ProfileGroupRepository.GetListProfileGroupBy(id, CompanyCode);
            ProfileView.profileGroupList = profileGroupList;

            //Bảo hành
            ViewBag.Ticket = GetTask(id, ConstWorkFlowCategory.TICKET);
            //Điểm trưng bày
            ViewBag.GTB = GetTask(id, ConstWorkFlowCategory.GTB);
            //Catalogue
            ViewBag.Catalogue = _unitOfWork.CatalogueRepository.GetAll(id)
                                           .Where(p => p.isDeleted == null || p.isDeleted == false).OrderByDescending(p => p.CreatedDate).ToList();
            //Công ty
            ViewBag.OverviewCompany = _unitOfWork.ProfileRepository.GetCompanyInOverviewProfile(id).ToList();
            //Thị hiếu
            ViewBag.CustomerTaste = _unitOfWork.CustomerTasteRepository.GetTastes(id);
            //Nhân viên phụ trách
            if (ProfileView.CustomerTypeCode == ConstProfileType.Opportunity)
            {
                ViewBag.PersonInCharge = _unitOfWork.PersonInChargeRepository.List(id, CompanyCode, ConstSalesEmployeeType.NVKD);
                ViewBag.PersonInCharge2 = _unitOfWork.PersonInChargeRepository.List(id, CompanyCode, ConstSalesEmployeeType.NVSalesAdmin);
                ViewBag.PersonInCharge3 = _unitOfWork.PersonInChargeRepository.List(id, CompanyCode, ConstSalesEmployeeType.NVSpec);

                // Chủ đầu tư
                ViewBag.Investor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ChuDauTu).Where(p => p.IsMain == true).ToList();
                ProfileView.Investor = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Investor).Select(x => x.ProfileName));

                //Thiết kế
                ViewBag.Design = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ThietKe).Where(p => p.IsMain == true).ToList();
                ProfileView.Designer = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Design).Select(x => x.ProfileName));

                //Tổng thầu
                ViewBag.Contractor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.TongThau).Where(p => p.IsMain == true).ToList();
                ProfileView.Contractor = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Contractor).Select(x => x.ProfileName));

                //Đơn vị tư vấn giám sát
                ViewBag.Consulting = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DonViTuVan).Where(p => p.IsMain == true).ToList();
                ProfileView.ConsultingUnit = string.Join(", ", ((List<OpportunityPartnerViewModel>)ViewBag.Consulting).Select(x => x.ProfileName));


                //Thi công
                ViewBag.Internal = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.CanMau).Where(p => p.IsMain == true).ToList();
                ViewBag.Competitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DaiTra).Where(p => p.IsMain == true).ToList();
                ViewBag.Competitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.ThauPhu).Where(p => p.IsMain == true).ToList();

                //Đối thủ
                ViewBag.OppCompetitor = _unitOfWork.ProfileRepository.GetOpportunityPartner(id, ConstPartnerType.DoiThu).ToList();

                //Hoạt động
                var _activitiesRepository = new ActivitiesRepository(_context);
                ViewBag.OpportunityActivities = _activitiesRepository.GetAll(id);
                ViewBag.IsView = true;
            }
            else
            {
                ViewBag.PersonInCharge = _unitOfWork.PersonInChargeRepository.List(id, CompanyCode, ConstSalesEmployeeType.NVKD_A);
            }
            //Phong ban phụ trách
            ViewBag.RoleInCharge = GetRoleInCharge(id);
            //Liên hệ
            ViewBag.ProfileContact = GetProfileContact(id);
            //Ghé thăm
            ViewBag.Appointment = _unitOfWork.AppointmentRepository.Search(id);
            //Thăm hỏi
            ViewBag.Activities = GetTask(id, ConstWorkFlowCategory.THKH).OrderByDescending(p => p.StartDate).ToList();
            //Nhiệm vụ
            ViewBag.Mission = GetTask(id, ConstWorkFlowCategory.ACTIVITIES);
            //Đăng ký bảo hành
            ViewBag.ProductWarranty = GetProductWarranty(id, CompanyCode);

            return PartialView(ProfileView);
        }
        private List<ProductWarrantyViewModel> GetProductWarranty(Guid profileId, string currentCompanyCode)
        {
            var list = (from pw in _context.ProductWarrantyModel
                        join profile in _context.ProfileModel on pw.ProfileId equals profile.ProfileId
                        join w in _context.WarrantyModel on pw.WarrantyId equals w.WarrantyId
                        join p in _context.ProductModel on pw.ProductId equals p.ProductId
                        join c in _context.CompanyModel on pw.CompanyId equals c.CompanyId
                        where pw.ProfileId == profileId && c.CompanyCode == currentCompanyCode
                        select new ProductWarrantyViewModel()
                        {
                            ProductWarrantyId = pw.ProductWarrantyId,
                            ProductWarrantyCode = pw.ProductWarrantyCode,
                            ProductName = p.ProductName,
                            SerriNo = pw.SerriNo,
                            WarrantyName = w.WarrantyName,
                            FromDate = pw.FromDate,
                            ToDate = pw.ToDate
                        }).ToList();
            return list;
        }
        private List<TaskViewModel> GetTask(Guid ProfileId, string Type)
        {
            var lst = (from p in _context.TaskModel
                       join ts in _context.TaskStatusModel on p.TaskStatusId equals ts.TaskStatusId
                       join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                       join pr in _context.CatalogModel on p.PriorityCode equals pr.CatalogCode
                       //Reporter
                       join re in _context.SalesEmployeeModel on p.Reporter equals re.SalesEmployeeCode into reg
                       from report in reg.DefaultIfEmpty()
                       where p.ProfileId == ProfileId
                       && w.WorkflowCategoryCode == Type
                       //Khác Ghé thăm
                       && w.WorkFlowCode != ConstWorkFlow.GT
                       && p.isDeleted != true
                       select new TaskViewModel
                       {
                           TaskId = p.TaskId,
                           TaskCode = p.TaskCode,
                           Summary = p.Summary,
                           WorkFlowName = w.WorkFlowName,
                           TaskStatusName = ts.TaskStatusName,
                           PriorityText_vi = pr.CatalogText_vi,
                           ReporterName = report.SalesEmployeeName,
                           StartDate = p.StartDate,
                           EndDate = p.EndDate
                       }).ToList();
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    //Assignee
                    var assigneeLst = (from p in _context.TaskAssignModel
                                       join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                       where p.TaskId == item.TaskId
                                       select new SalesEmployeeViewModel
                                       {
                                           SalesEmployeeCode = s.SalesEmployeeCode,
                                           SalesEmployeeName = s.SalesEmployeeName
                                       }).ToList();
                    item.AssigneeName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                }
            }
            return lst;
        }

        private List<RoleInChargeViewModel> GetRoleInCharge(Guid ProfileId)
        {
            var roleInChargeList = (from p in _context.RoleInChargeModel
                                    join rol in _context.RolesModel on p.RolesId equals rol.RolesId
                                    join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                                    where p.ProfileId == ProfileId
                                    orderby p.CreateTime descending
                                    select new RoleInChargeViewModel
                                    {
                                        RoleInChargeId = p.RoleInChargeId,
                                        ProfileId = p.ProfileId,
                                        RolesId = p.RolesId,
                                        RoleCode = p.RolesModel.RolesCode,
                                        RoleName = p.RolesModel.RolesName,
                                        CreateTime = p.CreateTime,
                                        CreateUser = acc.UserName
                                    }).ToList();
            return roleInChargeList;
        }

        private List<ProfileViewModel> GetProfileContact(Guid profileId)
        {
            var contactProfileList = (from p in _context.ProfileModel
                                      join a in _context.ProfileContactAttributeModel on p.ProfileId equals a.ProfileId
                                      join acc in _context.AccountModel on p.CreateBy equals acc.AccountId into ag
                                      from ac in ag.DefaultIfEmpty()
                                      //Phòng ban
                                      join dept in _context.CatalogModel on a.DepartmentCode equals dept.CatalogCode into deptG
                                      from d in deptG.DefaultIfEmpty()
                          
                                      //Province
                                      join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                                      from province in prG.DefaultIfEmpty()
                                          //District
                                      join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                                      from district in dG.DefaultIfEmpty()
                                          //Ward
                                      join w in _context.WardModel on p.WardId equals w.WardId into wG
                                      from ward in wG.DefaultIfEmpty()
                                      where a.CompanyId == profileId
                                      orderby a.IsMain descending, p.CreateTime descending
                                      select new ProfileViewModel()
                                      {
                                          ProfileId = a.ProfileId,
                                          ProfileCode = p.ProfileCode,
                                          ProfileName = p.ProfileName,
                                          ProfileContactPosition = a.Position,
                                          Phone = p.Phone,
                                          Email = p.Email,
                                          IsMain = a.IsMain,
                                          CreateUser = ac.UserName,
                                          CreateTime = p.CreateTime,
                                          DepartmentName = d.CatalogText_vi,
                                          Position = a.Position,
                                          Address = p.Address,
                                          ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                          DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                          WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                          DayOfBirth = p.DayOfBirth,
                                          MonthOfBirth = p.MonthOfBirth,
                                      }).ToList();

            if (contactProfileList != null && contactProfileList.Count() > 0)
            {
                
                var contactIdList = contactProfileList.Select(p => p.ProfileId).ToList();
                var allAssigneeLst = (from p in _context.PersonInChargeModel
                                      join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                      where contactIdList.Contains(p.ProfileId.Value)
                                      && p.SalesEmployeeType == ConstSalesEmployeeType.NVKD_A
                                      select new SalesEmployeeViewModel
                                      {
                                          CompanyId = p.ProfileId,
                                          SalesEmployeeCode = s.SalesEmployeeCode,
                                          SalesEmployeeName = s.SalesEmployeeName
                                      }).ToList();

                foreach (var item in contactProfileList)
                {
                    //Chức vụ
                    var position = (from p in _context.CatalogModel
                                    where item.Position == p.CatalogCode && p.CatalogTypeCode == "Position"
                                    select p.CatalogText_vi).ToList();
                    if (position.Count > 0)
                    {
                        item.PositionName = position.FirstOrDefault();
                    }
                    item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);
                    //Assignee
                    var assigneeLst = (from p in allAssigneeLst
                                       where p.CompanyId == item.ProfileId
                                       select new SalesEmployeeViewModel
                                       {
                                           SalesEmployeeCode = p.SalesEmployeeCode,
                                           SalesEmployeeName = p.SalesEmployeeName
                                       }).ToList();
                    if (assigneeLst != null && assigneeLst.Count() > 0)
                    {
                        item.PersonInChargeListName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                    }
                }
            }
            return contactProfileList;

        }

    }
}