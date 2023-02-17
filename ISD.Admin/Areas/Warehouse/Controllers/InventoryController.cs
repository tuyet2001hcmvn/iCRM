using ISD.Core;
using ISD.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Warehouse.Controllers
{
    public class InventoryController : BaseController
    {
        // GET: Inventory
        [ISDAuthorization]
        public ActionResult Index()
        {
            //var CompanyId = CurrentUser.CompanyId;
            //var StoreId = _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == CurrentUser.SaleOrg).StoreId;
            //CreateViewBagForSearch(CompanyId, StoreId);

            CreateViewBagForSearch();
            return View();
        }
        public ActionResult _Search(Guid? CompanyId, Guid? StoreId, Guid? SearchProductId)
        {
            var inventoryList = _unitOfWork.InventoryRepository.Search(CompanyId, StoreId, SearchProductId);
            return PartialView(inventoryList);
        }

        private void CreateViewBagForSearch(Guid? companyId = null, Guid? storeId = null)
        {
            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", storeId);

            //Dropdown Company
            var storeCodeList = storeList.Select(p => p.SaleOrgCode).ToList();
            var companyList = _unitOfWork.CompanyRepository.GetCompanyByStoreList(storeCodeList);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", companyId);

        }
    }
}