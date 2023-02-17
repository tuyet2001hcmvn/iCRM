using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class WorkFlowFieldController : BaseController
    {
        // GET: WorkFlowField
        #region Index
        [ISDAuthorization]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string FieldCode, string FieldName)
        {
            var listWFConfig = (from p in _context.WorkFlowFieldModel
                                where (FieldCode == "" || p.FieldCode == FieldCode)
                                && (FieldName == "" || p.FieldName.Contains(FieldName))
                                orderby p.OrderIndex
                                select p).ToList();
            return PartialView(listWFConfig);
        }
        #endregion

        #region Create
        [ISDAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ISDAuthorization]
        public JsonResult Create(WorkFlowFieldModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.WorkFlowField.ToLower())
                });
            });
        }

        #endregion

        #region Edit
        [ISDAuthorization]
        public ActionResult Edit(string id)
        {
            var field = _context.WorkFlowFieldModel.FirstOrDefault(p => p.FieldCode == id);
            return View(field);
        }

        [HttpPost]
        [ISDAuthorization]
        public JsonResult Edit(WorkFlowFieldModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Upload_Success, LanguageResource.WorkFlowField.ToLower())
                });
            });
        }
        #endregion
    }
}