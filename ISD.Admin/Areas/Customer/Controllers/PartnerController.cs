using ISD.Constant;
using ISD.EntityModels;
using ISD.Core;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class PartnerController : BaseController
    {
        // GET: Partner
        public ActionResult _List(Guid id, bool? isLoadContent = false)
        {
            var partnerList = (from p in _context.PartnerModel
                               //Profile
                               join c in _context.ProfileModel on p.PartnerProfileId equals c.ProfileId
                               //PartnerType
                               //join caTmp in _context.CatalogModel on p.PartnerType equals caTmp.CatalogTypeCode into CataLog
                               //from ca in CataLog.DefaultIfEmpty()
                               //Câu sửa sai => do đã bắt buộc nhập nên join
                               join ca in _context.CatalogModel on p.PartnerType equals ca.CatalogCode
                                //Create User
                               join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                               where p.ProfileId == id
                               //Bổ xung where điều kiện type đúng config
                               && ca.CatalogTypeCode == ConstCatalogType.PartnerType
                               orderby p.CreateTime descending
                               select new PartnerViewModel
                               {
                                   //Id Partner
                                   PartnerId = p.PartnerId,                                   
                                   //Partner Type
                                   PartnerTypeName = ca.CatalogText_vi,
                                   //Partner
                                   PartnerName = c.ProfileName,
                                   //Note
                                   Note = p.Note,
                                   //Create User
                                   CreateUser = acc.UserName,
                                   CreateTime = p.CreateTime
                               }).ToList();
            if (isLoadContent == true)
            {
                return PartialView("_ListContent", partnerList);
            }
            return PartialView(partnerList);
        }

        public ActionResult _Create(Guid profileId)
        {
            PartnerViewModel partnerViewModel = new PartnerViewModel
            {
                ProfileId = profileId
            };

            CreateViewBag(profileId);
            return PartialView("_FromPartner", partnerViewModel);
        }
        public ActionResult _Edit(Guid? PartnerId)
        {
            var partnerInDb = _context.PartnerModel.FirstOrDefault(p => p.PartnerId == PartnerId);
            if (partnerInDb == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_Partner.ToLower()) });
            }
            var partnerView = new PartnerViewModel
            {
                //1. GUID
                PartnerId = partnerInDb.PartnerId,
                //2. Mã Profile
                ProfileId = partnerInDb.ProfileId,
                //3. Loại đối tác
                PartnerType = partnerInDb.PartnerType,
                //4. Mã đối tác
                PartnerProfileId = partnerInDb.PartnerProfileId,
                //5. Note
                Note = partnerInDb.Note

            };
            CreateViewBag(partnerView.ProfileId, partnerView.PartnerType, partnerView.PartnerProfileId);
            return PartialView("_FromPartner", partnerView);
        }

        [HttpPost]
        public ActionResult Save(PartnerViewModel partnerVM)
        {
            return ExecuteContainer(() =>
            {
                //Lấy ra Partner đã tồn tại bởi profileId
                var partnerList = _context.PartnerModel.Where(p => p.ProfileId == partnerVM.ProfileId && p.PartnerProfileId == partnerVM.PartnerProfileId).ToList();

                //check trùng partner
                if (partnerList.Count == 0)
                {
                    //Neu partnerId = empty => Create
                    // else => Edit
                    if (partnerVM.PartnerId == Guid.Empty)
                    {
                        #region Create

                        var partnerNew = new PartnerModel
                        {
                            PartnerId = Guid.NewGuid(),
                            //1. Mã Profile
                            ProfileId = partnerVM.ProfileId,
                            //2. Loại đối tác
                            PartnerType = partnerVM.PartnerType,
                            //3. Mã đối tác
                            PartnerProfileId = partnerVM.PartnerProfileId,
                            //Note
                            Note = partnerVM.Note,
                            //4. Người tạo
                            CreateBy = CurrentUser.AccountId,
                            //5. hời gian tạo
                            CreateTime = DateTime.Now
                        };

                        _context.Entry(partnerNew).State = EntityState.Added;
                        _context.SaveChanges();

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_Partner.ToLower())
                        });

                        #endregion Create
                    }
                    else
                    {
                        #region Edit
                        var partnerInDb = _context.PartnerModel.FirstOrDefault(p => p.PartnerId == partnerVM.PartnerId);

                        if (partnerInDb != null)
                        {
                            //1.Loại DT
                            partnerInDb.PartnerType = partnerVM.PartnerType;
                            //2. Mã đối tác
                            partnerInDb.PartnerProfileId = partnerVM.PartnerProfileId;
                            // Note
                            partnerInDb.Note = partnerVM.Note;
                            //3. Người sửa
                            partnerInDb.LastEditBy = CurrentUser.AccountId;
                            //4. Thời gian sửa
                            partnerInDb.LastEditTime = DateTime.Now;

                            _context.Entry(partnerInDb).State = EntityState.Modified;
                            _context.SaveChanges();

                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = true,
                                Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.RoleInCharge.ToLower())
                            });
                        }

                        #endregion Edit

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = LanguageResource.Mobile_NotFound
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = string.Format(LanguageResource.Validation_Already_Exists, LanguageResource.Customer_Partner.ToLower())
                    });
                }
            });
        }

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var partnerInDb = _context.PartnerModel.FirstOrDefault(p => p.PartnerId == id);
                if (partnerInDb != null)
                {
                    _context.Entry(partnerInDb).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Customer_Partner.ToLower())
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

        public void CreateViewBag(Guid? profileId, string PartnerType = null, Guid? PartnerProfileId = null)
        {
            var partnerTypeList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.PartnerType).ToList();
            ViewBag.PartnerType = new SelectList(partnerTypeList, "CatalogCode", "CatalogText_vi", PartnerType);

            var parnerList = _context.ProfileModel.Where(p => p.ProfileId != profileId).ToList();
            ViewBag.PartnerProfileId = new SelectList(parnerList, "ProfileId", "ProfileName", PartnerProfileId);
        }
    }
}