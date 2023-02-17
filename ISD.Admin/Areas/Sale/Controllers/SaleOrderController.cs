using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sale.Controllers
{
    public class SaleOrderController : BaseController
    {
        // GET: SaleOrder
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            SaleOrderView_ViewModel model = new SaleOrderView_ViewModel();
            model.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.ToDate = model.FromDate.Value.AddMonths(1).AddDays(-1);
            return View(model);
        }
        public ActionResult _Search(string SaleOrderCode = "", string CustomerCode = "", string CustomerName = "", DateTime? FromDate = null, DateTime? ToDate = null)
        {
            return ExecuteSearch(() =>
            {
                var saleOrderList = (from p in _context.SaleOrderModel
                                     where
                                     //search by SaleOrderCode
                                     (SaleOrderCode == "" || p.SaleOrderCode.Contains(SaleOrderCode))
                                     //search by CustomerCode
                                     && (CustomerCode == "" || p.CustomerCode.Contains(CustomerCode))
                                     //search by CustomerName
                                     && (CustomerName == "" || p.CustomerName.Contains(CustomerName))
                                     //search by CreatedDate
                                     && (FromDate <= p.CreatedDate && p.CreatedDate <= ToDate)
                                     select new SaleOrderView_ViewModel()
                                     {
                                         SaleOrderId = p.SaleOrderId,
                                         SaleOrderCode = p.SaleOrderCode,
                                         CustomerCode = p.CustomerCode,
                                         CustomerName = p.CustomerName,
                                         StoreName = p.StoreName,
                                         CreatedDate = p.CreatedDate,
                                         Total = p.Total
                                     }).ToList();

                return PartialView(saleOrderList);
            });
        }
        #endregion

        //GET: /SaleOrder/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var saleOrder = (from p in _context.SaleOrderModel.AsEnumerable()
                             where p.SaleOrderId == id
                             select new SaleOrderView_ViewModel()
                             {
                                 SaleOrderId = p.SaleOrderId,
                                 SaleOrderCode = p.SaleOrderCode,
                                 CustomerCode = p.CustomerCode,
                                 CustomerName = p.CustomerName,
                                 StoreName = p.StoreName,
                                 CreatedDate = p.CreatedDate,
                                 PaidDate = p.PaidDate,
                                 SubTotal = p.SubTotal,
                                 Total = p.Total,
                                 Note = p.Note,
                                 SystemNote = p.SystemNote
                             }).FirstOrDefault();

            if (saleOrder == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_SaleOrder.ToLower()) });
            }
            else
            {
                var detailList = (from p in _context.SaleOrderDetailModel
                                  join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                  where p.SaleOrderId == id
                                  select new SaleOrderView_ViewModel.SaleOrderDetailViewModel()
                                  {
                                      ProductName = pr.ProductName,
                                      Description = p.Description,
                                      SerialNumber = p.SerialNumber,
                                      EngineNumber = p.EngineNumber,
                                      Quantity = p.Quantity,
                                      Price = p.Price,
                                      DiscountType = p.DiscountType,
                                      Discount = p.Discount,
                                      UnitPrice = p.UnitPrice
                                  }).ToList();
                saleOrder.detailList = detailList;
            }
            return View(saleOrder);
        }
        #endregion View
    }
}