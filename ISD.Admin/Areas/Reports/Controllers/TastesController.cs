using DevExpress.Web.Mvc;
using ISD.Core;
using ISD.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class TastesController : BaseController
    {
        // GET: Tastes
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //CreateViewBag();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult PivotGridPartial()
        {
             var searchView = new CustomerTastesSearchViewModel();
            var model = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchView);
            return PartialView("_PivotGridPartial", model);
        }
        //public ActionResult ExportToXLSX_DataAware()
        //{
        //    var searchView = new CustomerTastesSearchViewModel();
        //    var model = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchView);
        //    //var model = GetTastes(searchViewModel, ChooseStoreId);
        //    return PivotGridExtension.ExportToXlsx(TastesPivotGridHelper.Settings, model, TastesPivotGridHelper.XlsxOptions);
        //}
    }
}