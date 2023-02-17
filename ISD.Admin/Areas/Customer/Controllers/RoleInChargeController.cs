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
    public class RoleInChargeController : BaseController
    {
        // GET: RoleInCharge
        public ActionResult _List(Guid id, bool? isLoadContent = false)
        {
            var roleInChargeList = (from p in _context.RoleInChargeModel
                                    join rol in _context.RolesModel on p.RolesId equals rol.RolesId
                                    join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                                    where p.ProfileId == id
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
            if (isLoadContent == true)
            {
                return PartialView("_ListContent", roleInChargeList);
            }
            return PartialView(roleInChargeList);
        }

        public ActionResult _Create(Guid? profileId)
        {
            RoleInChargeViewModel roleInChargeVM = new RoleInChargeViewModel()
            {
                ProfileId = profileId
            };
            CreateViewBag();
            return PartialView("_FromRoleInCharge", roleInChargeVM);
        }

        public ActionResult _Edit(Guid? RoleInChargeId)
        {
            var roleInchargeDb = _context.RoleInChargeModel.FirstOrDefault(p => p.RoleInChargeId == RoleInChargeId);

            var roleInChargeVM = new RoleInChargeViewModel
            {
                RoleInChargeId = roleInchargeDb.RoleInChargeId,
                RolesId = roleInchargeDb.RolesId,
                ProfileId = roleInchargeDb.ProfileId
            };

            CreateViewBag(roleInchargeDb.RolesId);
            return PartialView("_FromRoleInCharge", roleInChargeVM);
        }

        [HttpPost]
        public ActionResult Save(RoleInChargeViewModel roleInChargeVM)
        {
            return ExecuteContainer(() =>
            {
                //Lấy ra danh sách role đã tồn tại bởi profileId
                var roleInChargeList = _context.RoleInChargeModel.Where(p => p.ProfileId == roleInChargeVM.ProfileId && p.RolesId == roleInChargeVM.RolesId).ToList();
                //check trùng employee
                if (roleInChargeList.Count == 0)
                {
                    if (roleInChargeVM.RoleInChargeId == Guid.Empty)
                    {
                        #region Create

                        var roleInChargeNew = new RoleInChargeModel
                        {
                            RoleInChargeId = Guid.NewGuid(),
                            ProfileId = roleInChargeVM.ProfileId,
                            RolesId = roleInChargeVM.RolesId,
                            CreateBy = CurrentUser.AccountId,
                            CreateTime = DateTime.Now,
                        };

                        _context.Entry(roleInChargeNew).State = EntityState.Added;
                        _context.SaveChanges();

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.RoleInCharge.ToLower())
                        });

                        #endregion Create
                    }
                    else
                    {
                        #region Edit
                        var roleInchargeBd = _context.RoleInChargeModel.FirstOrDefault(p => p.RoleInChargeId == roleInChargeVM.RoleInChargeId);

                        if (roleInchargeBd != null)
                        {
                            roleInchargeBd.RolesId = roleInChargeVM.RolesId;

                            _context.Entry(roleInchargeBd).State = EntityState.Modified;
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
                            Data = ""
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = string.Format(LanguageResource.Validation_Already_Exists, LanguageResource.RoleInCharge.ToLower())
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
                var roleInchargeDb = _context.RoleInChargeModel.FirstOrDefault(p => p.RoleInChargeId == id);
                if (roleInchargeDb != null)
                {
                    _context.Entry(roleInchargeDb).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.RoleInCharge.ToLower())
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

        public void CreateViewBag(Guid? RolesId = null)
        {
            var rolesList = _context.RolesModel.Where(p => p.isEmployeeGroup == true && p.Actived == true)
                                                        .Select(p => new RolesViewModel()
                                                        {
                                                            RolesId = p.RolesId,
                                                            RolesName = p.RolesName,
                                                            OrderIndex = p.OrderIndex
                                                        }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.RolesId = new SelectList(rolesList, "RolesId", "RolesName", RolesId);
        }
    }
}