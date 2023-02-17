using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;
using System.Collections.Generic;

namespace MasterData.Controllers
{
    public class WardController : BaseController
    {
        // GET: Ward
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(Guid? ProvinceId, Guid? DistrictId, string WardName)
        {
            return ExecuteSearch(() =>
            {
                var wardList = (from p in _context.WardModel
                                join d in _context.DistrictModel on p.DistrictId equals d.DistrictId
                                join pr in _context.ProvinceModel on d.ProvinceId equals pr.ProvinceId
                                orderby pr.Area, pr.ProvinceName, d.Appellation, d.DistrictName, p.Appellation, p.WardName
                                where
                                //search by ProvinceId
                                (ProvinceId == null || pr.ProvinceId == ProvinceId)
                                //search by DistrictId
                                && (DistrictId == null || p.DistrictId == DistrictId)
                                //search by WardName
                                && (WardName == null || (p.WardName.Contains(WardName) || p.Appellation.Contains(WardName)))
                                //search by Actived
                                select new WardViewModel()
                                {
                                    WardId = p.WardId,
                                    ProvinceName = pr.ProvinceName,
                                    DistrictName = d.Appellation + " " + d.DistrictName,
                                    WardCode = p.WardCode,
                                    WardName = p.Appellation + " " + p.WardName,
                                    OrderIndex = p.OrderIndex
                                })
                                .Take(200)
                                .ToList();

                return PartialView(wardList);
            });
        }
        #endregion

        //GET: /Ward/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(WardModel model)
        {
            return ExecuteContainer(() =>
            {
                model.WardId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Ward.ToLower())
                });
            });
        }
        #endregion

        //GET: /District/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var ward = (from p in _context.WardModel
                        join d in _context.DistrictModel on p.DistrictId equals d.DistrictId
                        join pr in _context.ProvinceModel on d.ProvinceId equals pr.ProvinceId
                        where p.WardId == id
                        select new WardViewModel()
                        {
                            WardId = p.WardId,
                            ProvinceId = pr.ProvinceId,
                            ProvinceName = pr.ProvinceName,
                            DistrictId = p.DistrictId,
                            DistrictName = d.Appellation + " " + d.DistrictName,
                            WardCode = p.WardCode,
                            Appellation = p.Appellation,
                            WardName = p.WardName,
                            OrderIndex = p.OrderIndex
                        })
                         .FirstOrDefault();
            if (ward == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Ward.ToLower()) });
            }
            CreateViewBag(ward.ProvinceId, ward.DistrictId);
            return View(ward);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(WardModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Ward.ToLower())
                });
            });
        }
        #endregion

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? ProvinceId = null, Guid? DistrictId = null)
        {
            //Get list Province
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                                     .OrderBy(p => p.Area)
                                                     .ThenBy(p => p.ProvinceName).ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //Get list District
            var districtList = _context.DistrictModel.Where(p => p.Actived == true)
                                                     .Select(p => new ISD.ViewModels.DistrictViewModel()
                                                     {
                                                         DistrictId = p.DistrictId,
                                                         DistrictName = p.Appellation + " " + p.DistrictName,
                                                         OrderIndex = p.OrderIndex
                                                     })
                                                     .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);
        }
        //GetDistrictBy
        public ActionResult GetDistrictBy(Guid? ProvinceId)
        {
            var districtList = _context.DistrictModel.Where(p => p.Actived == true && p.ProvinceId == ProvinceId)
                                                     .Select(p => new ISD.ViewModels.DistrictViewModel()
                                                     {
                                                         DistrictId = p.DistrictId,
                                                         DistrictName = p.Appellation + " " + p.DistrictName,
                                                         OrderIndex = p.OrderIndex
                                                     })
                                                     .OrderBy(p => p.OrderIndex).ToList();
            var lst = new SelectList(districtList, "DistrictId", "DistrictName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get ward by district
        public ActionResult GetWardByDistrict(Guid? DistrictId)
        {
            WardRepository repo = new WardRepository(_context);
            var wardList = repo.GetBy(DistrictId);
            var lst = new SelectList(wardList, "WardId", "WardName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWardByMultiDistrict(List<Guid?> DistrictId)
        {
            var _wardRepository = new WardRepository(_context);
            var wardList = new List<WardViewModel>();
            if (DistrictId != null && DistrictId.Count > 0)
            {
                wardList = (from d in _context.WardModel
                            join c in _context.DistrictModel on d.DistrictId equals c.DistrictId
                            join p in DistrictId on d.DistrictId equals p
                            orderby d.OrderIndex, d.WardName
                            select new WardViewModel
                            {
                                WardId = d.WardId,
                                WardName = c.DistrictName + " | " + d.Appellation + " " + d.WardName
                            }).ToList();
            }

            var wardIdList = new MultiSelectList(wardList, "WardId", "WardName");
            return Json(wardIdList, JsonRequestBehavior.AllowGet);
        }

        #endregion Get ward by district
    }
}