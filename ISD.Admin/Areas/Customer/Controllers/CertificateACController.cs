using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class CertificateACController : BaseController
    {
        private CertificateACRepository _certificateACRepository;

        public CertificateACController()
        {
            _certificateACRepository = new CertificateACRepository(_context);
        }
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var certificateACList = _certificateACRepository.GetAll(id);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", certificateACList);
                }
                return PartialView(certificateACList);
            });
        }
        public ActionResult _Create(Guid ProfileId)
        {
            CertificateACViewModel adbVM = new CertificateACViewModel()
            {
                ProfileId = ProfileId
            };
            return PartialView("_FormCertificate", adbVM);
        }
        public ActionResult _Edit(Guid CertificateACId)
        {
            var certificateACVM = _certificateACRepository.GetById(CertificateACId);
            return PartialView("_FormCertificate", certificateACVM);
        }
        [HttpPost]
        public ActionResult Save(CertificateACModel certificateAC)
        {
            return ExecuteContainer(() =>
            {
                //Thêm mới
                if (certificateAC.CertificateId == Guid.Empty)
                {
                    #region Create
                    certificateAC.CreateBy = CurrentUser.AccountId;
                    certificateAC.CreateTime = DateTime.Now;

                    _certificateACRepository.Create(certificateAC);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_Certificate.ToLower())
                    });
                    #endregion
                }
                else
                {
                    #region Edit
                    certificateAC.LastEditBy = CurrentUser.AccountId;
                    certificateAC.LastEditTime = DateTime.Now;

                    var ret = _certificateACRepository.Update(certificateAC);
                    if (ret)
                    {
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_Certificate.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_NotExist_Update, LanguageResource.Customer_Certificate.ToLower())
                        });
                    }
                    #endregion
                }
            });
        }
        //GET: /CertificateAC/Delete
        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var certificateAC = _context.CertificateACModel.FirstOrDefault(p => p.CertificateId == id);
                if (certificateAC != null)
                {
                    _context.Entry(certificateAC).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Customer_Certificate.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Customer_Certificate.ToLower())
                    });
                }
            });
        }
        #endregion
        #region CreateViewBag, Helper
        
        #endregion
    }
}