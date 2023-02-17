using ISD.Constant;
using ISD.Core;
using ISD.Repositories;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class CustomerSaleOrderController : BaseController
    {
        public ActionResult _List(string ProfileForeignCode, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var customerSaleOrderList = new List<CustomerSaleOrderViewModel>();
                if (isLoadContent == true)
                {
                    // Khởi tạo thư viện và kết nối
                    var _sap = new SAPRepository();
                    var destination = _sap.GetRfcWithConfig();
                    //Định nghĩa hàm cần gọi
                    var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_SO_CUST);
                    //Truyền parameters
                    function.SetValue("IM_WERKS", CurrentUser.CompanyCode); //Mã công ty
                    function.SetValue("IM_KUNNR", ProfileForeignCode); //Mã SAP Khách hàng

                    function.Invoke(destination);

                    var datatable = function.GetTable("SALEORDER_T").ToDataTable("SALEORDER_T");
                    if (datatable != null && datatable.Rows.Count > 0)
                    {
                        foreach (DataRow item in datatable.Rows)
                        {
                            //Mã đơn hàng
                            var SONumber = item["VBELN"].ToString();

                            //Mã lệnh
                            var OrderNumber = item["VBELN_OD"].ToString();

                            //Mã sản phẩm
                            var ProductCode = item["MATNR"].ToString().TrimStart(new Char[] { '0' });

                            //Tên sản phẩm
                            var ProductName = item["MAKTX"].ToString();

                            //Số lượng
                            var ProductQuantity = (decimal?)item["KWMENG"];

                            customerSaleOrderList.Add(new CustomerSaleOrderViewModel()
                            {
                                SONumber = SONumber,
                                OrderNumber = OrderNumber,
                                ProductCode = ProductCode,
                                ProductName = ProductName,
                                ProductQuantity = ProductQuantity,
                            });
                        }
                    }
                }

                return PartialView(customerSaleOrderList);
            });
        }
    }
}