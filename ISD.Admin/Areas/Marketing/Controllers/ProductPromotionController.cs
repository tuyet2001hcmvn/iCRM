using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marketing.Controllers
{
    public class ProductPromotionController : BaseController
    {
        // GET: ProductPromotion
        public ActionResult Index(string Type)
        {
            CreateViewBag(Type: Type);
            return View();
        }
        public ActionResult _PaggingServerSide(DatatableViewModel model, ProductPromotionSearchViewModel searchViewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var query = new ProductPromotionRepository(_context).GetBy(searchViewModel);

            var res = CustomSearchRepository.CustomSearchFunc<ProductPromotionViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
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
        public ActionResult Edit(Guid Id)
        {
            var model = new ProductPromotionRepository(_context).GetBy(Id);
            CreateViewBag(Type: model?.Type, viewModel: model);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Edit(ProductPromotionViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                var model = _context.ProductPromotionModel.Where(x => x.ProductPromotionId == viewModel.ProductPromotionId).FirstOrDefault();

                if (model != null)
                {
                    model.ProductPromotionTitle = viewModel.ProductPromotionTitle;
                    model.SendTypeCode = viewModel.SendTypeCode;
                    model.StartTime = viewModel.StartTime;
                    model.EndTime = viewModel.EndTime;
                    model.LastEditBy = CurrentUser.AccountId;
                    model.LastEditTime = DateTime.Now;
                    model.Actived = viewModel.Actived;
                }

                _context.Entry(model).State = EntityState.Modified;

                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ProductPromotion.ToLower()),
                    Id = model.ProductPromotionId
                });
            });
        }
        public ActionResult Create(string Type)
        {
            CreateViewBag(Type: Type);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(ProductPromotionViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                if(viewModel.SendTypeCode == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn Phương thức gửi",
                    });
                }                
                if(viewModel.TargetGroupId == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn Nhóm mục tiêu",
                    });
                }
                //Lưu master chiến dịch
                ProductPromotionModel model = new ProductPromotionModel()
                {
                    ProductPromotionId = Guid.NewGuid(),
                    TargetGroupId = viewModel.TargetGroupId,
                    IsSendCatalogue = viewModel.IsSendCatalogue,
                    ProductPromotionTitle = viewModel.ProductPromotionTitle,
                    SendTypeCode = viewModel.SendTypeCode,
                    Type = viewModel.Type,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    CreateBy = CurrentUser.AccountId,
                    CreateTime = DateTime.Now,
                    Actived = true
                };
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                CreateProductPromotionDetailForNewProductPromotion(model.ProductPromotionId);
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ProductPromotion.ToLower()),
                    Id = model.ProductPromotionId
                });
            });
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateStatusCustomer(ProductPromotionUpdateViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                //Cập nhật status
                var detail = _context.ProductPromotionDetailModel.Where(x => x.ProductPromotionDetailId == viewModel.ProductPromotionDetailId).FirstOrDefault();
                if (detail != null)
                {
                    detail.Status = viewModel.Status;
                    if (viewModel.Status == true)
                    {
                        detail.Checker = CurrentUser.AccountId;
                        detail.CheckDate = DateTime.Now;
                    }
                    else
                    {
                        detail.Checker = null;
                        detail.CheckDate = null;
                    }

                    _context.Entry(detail).State = EntityState.Modified;
                }
                //Nếu đã có contact thì update
                if (viewModel.ProductPromotionContactId != null && viewModel.ProductPromotionContactId != Guid.Empty)
                {
                    var model = _context.ProductPromotionContactModel.Where(x => x.ProductPromotionContactId == viewModel.ProductPromotionContactId).FirstOrDefault();
                    if (model != null)
                    {
                        model.ProfileAddress = viewModel.Address;
                        model.ContactName = viewModel.ContactName;
                        model.ContactPhone = viewModel.ContactPhone;
                        model.ContactId = viewModel.ProfileContactId;
                        model.CheckAddress = viewModel.CheckAddress;
                        model.CheckContact = viewModel.CheckContact;
                        _context.Entry(model).State = EntityState.Modified;
                    }

                }
                else
                {
                    //Chưa có contact => Thêm mới
                    var model = new ProductPromotionContactModel();
                    if (model != null)
                    {
                        model.ProductPromotionContactId = Guid.NewGuid();
                        model.ProductPromotionDetailId = viewModel.ProductPromotionDetailId;
                        model.ProfileAddress = viewModel.Address;
                        model.ContactName = viewModel.ContactName;
                        model.ContactPhone = viewModel.ContactPhone;
                        model.ContactId = viewModel.ProfileContactId;
                        model.CheckAddress = viewModel.CheckAddress;
                        model.CheckContact = viewModel.CheckContact;
                    }
                    viewModel.ProductPromotionContactId = model.ProductPromotionContactId;
                    _context.Entry(model).State = EntityState.Added;
                }

                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = viewModel.ProductPromotionContactId
                });
            });
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteContact(ProductPromotionUpdateViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                //Nếu đã có contact thì update
                if (viewModel.ProductPromotionContactId != null && viewModel.ProductPromotionContactId != Guid.Empty)
                {
                    var model = _context.ProductPromotionContactModel.Where(x => x.ProductPromotionContactId == viewModel.ProductPromotionContactId).FirstOrDefault();
                    if (model != null)
                    {
                        _context.Entry(model).State = EntityState.Deleted;
                    }
                }

                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = viewModel.ProductPromotionContactId
                });
            });
        }

        private void CreateProductPromotionDetailForNewProductPromotion(Guid id)
        {
            SqlParameter productionPromotionId = new SqlParameter("@ProductPromotionId", id);
            _context.Database.ExecuteSqlCommand("exec Marketing.CreateProductPromotionDetail @ProductPromotionId", productionPromotionId);
        }
        [HttpPost]
        public JsonResult UpdateProductPromotionDetail(Guid id)
        {
            return ExecuteContainer(() =>
            {
                SqlParameter productionPromotionId = new SqlParameter("@ProductPromotionId", id);
                _context.Database.ExecuteSqlCommand("exec Marketing.UpdateProductPromotionDetail @ProductPromotionId", productionPromotionId);
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Message = "Cập nhật khách hàng thành công!"
                });
            });
        }
        public ActionResult _GetMemberTargetGroup(DatatableViewModel model, ProductPromotionViewModel viewModel)
        {
            var lstProfileCode = new List<int>();
            // trypart list ProfileCode
            if (!string.IsNullOrEmpty(viewModel.ListProfileCode))
            {
                try
                {
                    lstProfileCode = viewModel.ListProfileCode.Split(';').Select(Int32.Parse).ToList();
                }
                catch (Exception ex)
                {
                    return Json(new { isSuccess = false, message = "Vui lòng nhập đúng định dạng mã CRM" }, JsonRequestBehavior.AllowGet);
                }
            }

            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var query = new ProductPromotionRepository(_context).GetDetailBy(viewModel);
            //Lấy danh sách đã gửi
            var countIsSend = query.Where(x => x.Status == true).Count();
            var res = CustomSearchRepository.CustomSearchFunc<ProductPromotionDetailViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
            if (res != null && res.Count() > 0)
            {
                int i = model.start;
                foreach (var item in res)
                {
                    i++;
                    item.STT = i;
                    #region Danh sách khách hàng
                    var lstAddress = new List<ProfileAddressProductPromotionViewModel>();
                    var profileAddress = new ProductPromotionRepository(_context).GetAddressByProfile(item.ProfileId);
                    lstAddress.AddRange(profileAddress);
                    item.ProfileAddress = lstAddress;

                    var lstContact = new List<ProfileContactProductPromotionViewModel>();
                    var profileContact = (from p in _context.ProfileModel
                                          join a in _context.ProfileContactAttributeModel on p.ProfileId equals a.ProfileId
                                          where a.CompanyId == item.ProfileId && p.Actived != false
                                          orderby a.IsMain descending, p.CreateTime descending
                                          select new ProfileContactProductPromotionViewModel()
                                          {
                                              ProfileId = p.ProfileId,
                                              ProfileContactId = a.ProfileId,
                                              ProfileCode = p.ProfileCode,
                                              ContactName = p.ProfileName,
                                              ContactPhone = p.Phone,
                                          }).ToList();
                    if (profileContact != null && profileContact.Count() > 0)
                    {
                        foreach (var ct in profileContact)
                        {
                            var contactIdList = profileContact.Select(p => p.ProfileId).ToList();
                            //Danh sách số điện thoại theo từng liên hệ
                            var phoneLst = (from p in _context.ProfilePhoneModel
                                            where p.ProfileId == item.ProfileId
                                            select p.PhoneNumber).ToList();
                            //Phone
                            if (phoneLst != null && phoneLst.Count() > 0)
                            {
                                ct.ContactPhone = ct.ContactPhone + "," + string.Join(", ", phoneLst);
                            }
                        }
                        lstContact.AddRange(profileContact);
                        item.ProfileContact = lstContact;
                    }
                    #endregion
                    #region Danh sách khách hàng đã chọn địa chỉ và liên hệ
                    item.ProfileContactActived = new List<ProfileContactProductPromotionViewModel>();
                    item.ProfileAddressActived = new List<ProfileAddressProductPromotionViewModel>();
                    var contact =  //Thông tin liên hệ và địa chỉ khách hàng
                                    (from c in _context.ProductPromotionContactModel
                                         //Thông tin liên hệ
                                     join conTemp in _context.ProfileModel on new { ProfileId = (c != null ? c.ContactId.Value : Guid.Empty), CustomerTypeCode = ConstCustomerType.Contact } equals new { conTemp.ProfileId, conTemp.CustomerTypeCode } into conList
                                     from con in conList.DefaultIfEmpty()
                                     where c.ProductPromotionDetailId == item.ProductPromotionDetailId
                                     select new ProfileContactProductPromotionViewModel
                                     {
                                         ProductPromotionContactId = c.ProductPromotionContactId,
                                         ProfileId = con.ProfileId,
                                         ProfileCode = con.ProfileCode,
                                         ContactName = c.ContactName,
                                         CheckContact = c.CheckContact,
                                         ContactPhone = c.ContactPhone,
                                         ProfileContactId = c.ContactId
                                     }).ToList();
                    var address = (from c in _context.ProductPromotionContactModel
                                   where c.ProductPromotionDetailId == item.ProductPromotionDetailId
                                   select new ProfileAddressProductPromotionViewModel
                                   {
                                       ProductPromotionContactId = c.ProductPromotionContactId,
                                       ProfileId = item.ProfileId,
                                       Address = c.ProfileAddress,
                                       CheckAddress = c.CheckAddress
                                   }).ToList();
                    item.ProfileContactActived.AddRange(contact);
                    item.ProfileAddressActived.AddRange(address);
                    #endregion
                }
            }

            ViewBag.datatableViewModel = model;
            ViewBag.draw = model.draw;
            ViewBag.recordsTotal = totalResultsCount;
            ViewBag.recordsFiltered = filteredResultsCount;
            ViewBag.IsSendCatalogue = viewModel.IsSendCatalogue;

            return PartialView(res);
        }



        public void CreateViewBag(string Type = null, ProductPromotionViewModel viewModel = null)
        {
            string pageUrl = "/Marketing/ProductPromotion";
            var parameter = "?Type=" + Type;
            ViewBag.PageId = GetPageId(pageUrl, parameter);
            ViewBag.Type = Type;
            var title = (from p in _context.PageModel
                         where p.PageUrl == pageUrl
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;

            var sendTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductPromotionSendType);
            ViewBag.SendTypeCode = new SelectList(sendTypeCode, "CatalogCode", "CatalogText_vi", viewModel?.SendTypeCode);

            var targetGroupList = _context.TargetGroupModel.Where(x => x.Type == ConstMarketingType.ProductPromotion && x.Actived != false);
            var targetGroup = targetGroupList.Select(x => new { x.Id, TargetGroupName = x.TargetGroupName + " | (" + x.MemberOfTargetGroupModel.Count() + " Khách hàng)" }).ToList();
            ViewBag.TargetGroupId = new SelectList(targetGroup, "Id", "TargetGroupName", viewModel?.TargetGroupId);

            #region //Get list SalesEmployee (NV phụ trách)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesEmployeeCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");
            #endregion

            //Phân loại khách hàng
            var CustomerGroup = _unitOfWork.CatalogRepository.GetCustomerAccountGroup();
            ViewBag.CustomerAccountGroup = new SelectList(CustomerGroup, "CatalogCode", "CatalogText_vi");
        }
    }
}