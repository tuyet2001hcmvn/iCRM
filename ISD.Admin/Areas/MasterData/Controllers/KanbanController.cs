using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ISD.Core;

namespace MasterData.Controllers
{
    public class KanbanController : BaseController
    {
        // GET: Kanban
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.Actived = true;
            return View();
        }
        public ActionResult _Search(KanbanSearchViewModel searchModel)
        {
            var lst = _unitOfWork.KanbanRepository.Search(searchModel);
            return PartialView(lst);
        }
        #endregion

        // GET: Kanban/Create/
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(KanbanModel model, List<KanbanDetailViewModel> detailList)
        {
            return ExecuteContainer(() =>
            {
                model.CreateBy = CurrentUser.AccountId;
                _unitOfWork.KanbanRepository.Create(model, detailList);
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Kanban.ToLower()),
                });
            });
        }
        #endregion

        // GET: Kanban/Edit/
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var kanban = _context.KanbanModel.FirstOrDefault(p => p.KanbanId == id);
            if (kanban == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Kanban.ToLower()) });
            }
            var detailList = (from p in _context.KanbanDetailModel
                              where p.KanbanId == id
                              select new KanbanDetailViewModel()
                              {
                                  KanbanDetailId = p.KanbanDetailId,
                                  KanbanId = p.KanbanId,
                                  ColumnName = p.ColumnName,
                                  OrderIndex = p.OrderIndex,
                                  Note = p.Note
                              }).ToList();
            ViewBag.detailList = detailList;

            return View(kanban);
        }
        [HttpPost]
        public ActionResult Edit(KanbanViewModel viewModel, List<KanbanDetailViewModel> detailList)
        {
            return ExecuteContainer(() =>
            {
                viewModel.LastEditBy = CurrentUser.AccountId;
                _unitOfWork.KanbanRepository.Update(viewModel, detailList);
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Kanban.ToLower()),
                });
            });
        }
        #endregion

        //Check trùng
        #region Remote Validation
        private bool IsExists(string KanbanCode)
        {
            return (_context.KanbanModel.FirstOrDefault(p => p.KanbanCode == KanbanCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingKanbanCode(string KanbanCode, string KanbanCodeValid)
        {
            try
            {
                if (KanbanCodeValid != KanbanCode)
                {
                    return Json(!IsExists(KanbanCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}