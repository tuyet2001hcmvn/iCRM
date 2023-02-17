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
    public class SponsController : BaseController
    {
        private SponsRepository _sponsRepository;

        public SponsController()
        {
            _sponsRepository = new SponsRepository(_context);
        }
        public ActionResult _List(Guid? id, string Type, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var sponsList = _sponsRepository.GetAll(id, Type);
                CreateViewBag(Type);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", sponsList);
                }
                return PartialView(sponsList);
            });
        }
        public ActionResult _Create(Guid ProfileId, string Type)
        {
            SponsViewModel adbVM = new SponsViewModel()
            {
                ProfileId = ProfileId,
                Type = Type
                
            };
            CreateViewBag(adbVM.Type);
            return PartialView("_FormData", adbVM);
        }
        public ActionResult _Edit(Guid SponsId)
        {
            var sponsVM = _sponsRepository.GetById(SponsId);
            CreateViewBag(sponsVM.Type);
            return PartialView("_FormData", sponsVM);
        }
        [HttpPost]
        public ActionResult Save(SponsModel spons)
        {
            return ExecuteContainer(() =>
            {
                //Thêm mới
                if (spons.SponsId == Guid.Empty)
                {
                    #region Create
                    _sponsRepository.Create(spons);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_Spons.ToLower())
                    });
                    #endregion
                }
                else
                {
                    #region Edit
                    var ret = _sponsRepository.Update(spons);
                    if (ret)
                    {
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_Spons.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_NotExist_Update, LanguageResource.Customer_Spons.ToLower())
                        });
                    }
                    #endregion
                }
            });
        }
        //GET: /Spons/Delete
        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                _sponsRepository.Delete(id);
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Customer_Certificate.ToLower())
                });
            });
        }
        #endregion
        #region CreateViewBag, Helper
        private void CreateViewBag(string Type)
        {
            var title = _unitOfWork.CatalogRepository.GetBy(Type, ConstCatalogType.Customer_Spons);
            ViewBag.Title = title?.CatalogText_vi;
            ViewBag.TypeSpons = Type;
        }
        #endregion
    }
}