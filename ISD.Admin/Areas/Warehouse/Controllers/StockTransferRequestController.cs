using ISD.Core;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Warehouse.Controllers
{
    public class StockTransferRequestController : BaseController
    {
        // GET: StockTransferRequest
        public ActionResult Index()
        {
            CreateViewBagForSearch();
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            CreateViewBag();
            ViewBag.Id = id;
            ViewBag.ViewType = "Transfer";
            return View();
        }
        public ActionResult Transfer(Guid id,int stockTransferRequestCode)
        {
            CreateViewBag();
            ViewBag.Id = id;
            ViewBag.Code = stockTransferRequestCode;
            ViewBag.ViewType = "Transfer";
            return View();
        }
        public ActionResult Create(Guid? CopyFrom)
        {
            var companyId = CurrentUser.CompanyId;
            var storeId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
            var employeeCode = CurrentUser.EmployeeCode;
            CreateViewBag(companyId, storeId, employeeCode);
            ViewBag.StockList = _unitOfWork.StockRepository.GetAll();
            ViewBag.ViewType = "Transfer";
            ViewBag.CopyFrom = CopyFrom;
            return View();
        }
        private void CreateViewBag(Guid? CompanyId = null, Guid? StoreId = null, string SaleEmployeeCode = "")
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CompanyId);
            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetStoreByCompany(CompanyId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);
            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);
        }
        private void CreateViewBagForSearch()
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.FromStock = new SelectList(listStock, "StockId", "StockName");
            ViewBag.ToStock = new SelectList(listStock, "StockId", "StockName");

            ViewBag.Actived = new List<SelectListItem>()
            {
                new SelectListItem(){Text="Đang mở", Value="true" },
                new SelectListItem(){Text="Đã đóng", Value="false" }
            };
        }
    }
}