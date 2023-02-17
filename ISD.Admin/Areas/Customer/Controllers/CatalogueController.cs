using ISD.Repositories;
using System;
using System.Web.Mvc;
using ISD.Core;

namespace Customer.Controllers
{
    public class CatalogueController : BaseController
    {
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.ProfileId = id;
                var catalogueList = _unitOfWork.CatalogueRepository.GetAll(id);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", catalogueList);
                }
                return PartialView(catalogueList);
            });
        }

        public ActionResult _ListCatalog(Guid? ProfileId, Guid? TaskId = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.ProfileId = ProfileId;
                ViewBag.TaskId = TaskId;
                var catalogueList = _unitOfWork.CatalogueRepository.GetAllCatalog(ProfileId, TaskId, "CTL");
                if (isLoadContent == true)
                {
                    return PartialView("_ListContentCatalog", catalogueList);
                }
                return PartialView(catalogueList);
            });
        }

        public ActionResult _ListCatalogKe(Guid? ProfileId, Guid? TaskId = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.ProfileId = ProfileId;
                ViewBag.TaskId = TaskId;
                var catalogueList = _unitOfWork.CatalogueRepository.GetAllCatalog(ProfileId, TaskId, "KE");
                if (isLoadContent == true)
                {
                    return PartialView("_ListContentCatalog", catalogueList);
                }
                return PartialView(catalogueList);
            });
        }

        public ActionResult _ListCatalogMau(Guid? ProfileId, Guid? TaskId = null, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                ViewBag.ProfileId = ProfileId;
                ViewBag.TaskId = TaskId;
                var catalogueList = _unitOfWork.CatalogueRepository.GetAllCatalog(ProfileId, TaskId, "MAU");
                if (isLoadContent == true)
                {
                    return PartialView("_ListContentCatalog", catalogueList);
                }
                return PartialView(catalogueList);
            });
        }
    }
}