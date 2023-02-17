using ISD.Constant;
using ISD.Core;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class PersonInChargeController : BaseController
    {
        // GET: PersonInCharge
        #region List
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            //Hiển thị danh sách nhân viên phụ trách theo cấu hình ở WebConfig
            var personInChargeList = _unitOfWork.PersonInChargeRepository.List(id, CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD_A);
            string viewMode = WebConfigurationManager.AppSettings["ViewExtens"].ToString();
            if (!string.IsNullOrEmpty(viewMode) && viewMode == "Mode_2")
            {
                personInChargeList = _unitOfWork.PersonInChargeRepository.List(id,SalesEmployeeType: ConstSalesEmployeeType.NVKD_A);
            }
            ViewBag.ViewExtens = viewMode;

            if (isLoadContent == true)
            {
                return PartialView("_ListContent", personInChargeList);
            }
            return PartialView(personInChargeList);
        }
        #endregion

        #region Create
        public ActionResult _Create(Guid? profileId)
        {
            var personInChargeVM = new PersonInChargeViewModel
            {
                ProfileId = profileId
            };
            CreateViewBag();
            return PartialView("_FromPersonInCharge", personInChargeVM);
        }
        #endregion

        #region Edit
        public ActionResult _Edit(Guid? PersonInChargeId)
        {
            var personInChargeDb = _context.PersonInChargeModel.FirstOrDefault(p => p.PersonInChargeId == PersonInChargeId);
            if (personInChargeDb == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.PersonInCharge.ToLower()) });
            }
            var personInChargeVM = new PersonInChargeViewModel()
            {
                PersonInChargeId = personInChargeDb.PersonInChargeId,
                ProfileId = personInChargeDb.ProfileId,
                SalesEmployeeCode = personInChargeDb.SalesEmployeeCode,
                SalesEmployeeType = personInChargeDb.SalesEmployeeType,
            };

            CreateViewBag(personInChargeDb.SalesEmployeeCode, personInChargeDb.RoleCode);
            return PartialView("_FromPersonInCharge", personInChargeVM);
        }
        #endregion

        #region Save
        [HttpPost]
        public ActionResult Save(PersonInChargeViewModel personInChargeViewModel)
        {
            return ExecuteContainer(() =>
            {
                if (personInChargeViewModel.PersonInChargeId == Guid.Empty)
                {
                    #region Create
                    var personInChargeValid = _context.PersonInChargeModel.FirstOrDefault(p => p.ProfileId == personInChargeViewModel.ProfileId && p.SalesEmployeeCode == personInChargeViewModel.SalesEmployeeCode);
                    if (personInChargeValid == null)
                    {
                        personInChargeViewModel.CreateBy = CurrentUser.AccountId;
                        if (string.IsNullOrEmpty(personInChargeViewModel.CompanyCode))
                        {
                            personInChargeViewModel.CompanyCode = CurrentUser.CompanyCode;
                        }
                        personInChargeViewModel.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                        bool result = _unitOfWork.PersonInChargeRepository.Create(personInChargeViewModel);
                        _context.SaveChanges();
                        if (result)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = true,
                                Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.PersonInCharge.ToLower())
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.BadRequest,
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
                            Data = string.Format(LanguageResource.Validation_Already_Exists, LanguageResource.PersonInCharge)
                        });
                    }

                    #endregion Create
                }
                else
                {
                    #region Edit

                    var personInchargeDb = _context.PersonInChargeModel.FirstOrDefault(p => p.PersonInChargeId == personInChargeViewModel.PersonInChargeId);
                    if (personInchargeDb != null)
                    {
                        //Mapping
                        personInchargeDb.SalesEmployeeCode = personInChargeViewModel.SalesEmployeeCode;
                        personInchargeDb.RoleCode = personInChargeViewModel.RoleCode;
                        if (string.IsNullOrEmpty(personInchargeDb.CompanyCode))
                        {
                            personInchargeDb.CompanyCode = CurrentUser.CompanyCode;
                        }
                        personInchargeDb.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                        _context.Entry(personInchargeDb).State = EntityState.Modified;
                        _context.SaveChanges();

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.PersonInCharge.ToLower())
                        });
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = ""
                    });
                    #endregion Edit
                }

            });
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var personInChargeDb = _context.PersonInChargeModel.FirstOrDefault(p => p.PersonInChargeId == id);
                if (personInChargeDb != null)
                {
                    _context.Entry(personInChargeDb).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.PersonInCharge.ToLower())
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

        #region ViewBag
        public void CreateViewBag(string SalesEmployeeCode = null, string RoleCode = "")
        {
            //Employee
            var employeeList = _context.SalesEmployeeModel.Where(p => p.Actived == true)
                                                        .Select(p => new SalesEmployeeViewModel()
                                                        {
                                                            SalesEmployeeCode = p.SalesEmployeeCode,
                                                            SalesEmployeeName = p.SalesEmployeeCode + " | " + p.SalesEmployeeName
                                                        }).ToList();
            //var employeeList = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVKD_Code);
            ViewBag.SalesEmployeeCode = new SelectList(employeeList, "SalesEmployeeCode", "SalesEmployeeName", SalesEmployeeCode);

            //Vai trò
            var personRoleList = _unitOfWork.PersonInChargeRepository.GetListPersonRole();
            ViewBag.RoleCode = new SelectList(personRoleList, "CatalogCode", "CatalogText_vi", RoleCode);
        }
        #endregion
    }
}