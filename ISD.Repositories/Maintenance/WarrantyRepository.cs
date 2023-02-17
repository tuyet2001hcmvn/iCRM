using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace ISD.Repositories
{
    public class WarrantyRepository
    {
        private EntityDataContext _context;

        public WarrantyRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Tìm kiếm bảo hành theo Mẵ bảo hành || Tên bảo hành || Trạng thái
        /// </summary>
        /// <param name="warrantyCode">Mã bảo hành</param>
        /// <param name="warrantyName">Tên bảo hành</param>
        /// <param name="actived">Trạng thái</param>
        /// <returns>Danh sách bảo hành</returns>
        public List<WarrantyModel> Search(string warrantyCode = "", string warrantyName = "", bool? actived = null)
        {
            var warrantyList = (from w in _context.WarrantyModel
                                where
                                //Search by WarrantyCode
                                (warrantyCode == "" || w.WarrantyCode == warrantyCode)
                                //Search by WarrantyName
                                && (warrantyName == "" || w.WarrantyName == warrantyName)
                                //Search by Actived
                                && (actived == null || w.Actived == actived)
                                orderby w.WarrantyCode
                                select w).ToList();
            return warrantyList;
        }

        /// <summary>
        /// Get một warranty trong DB theo id
        /// </summary>
        /// <param name="warrantyId">Warranty Id</param>
        /// <returns>Warranty Model</returns>
        public WarrantyModel GetWarranty(Guid warrantyId)
        {
            var warranty = _context.WarrantyModel.FirstOrDefault(p => p.WarrantyId == warrantyId);
            return warranty;
        }

        public IEnumerable<WarrantyModel> GetAll()
        {
            var warrantyList = _context.WarrantyModel.Where(p => p.Actived == true).OrderBy(p => p.Duration);
            return warrantyList;
        }

        /// <summary>
        /// Thêm mới một loại bảo hành
        /// </summary>
        /// <param name="warrantyModel">Warranty Model</param>
        /// <returns>Warranty model</returns>
        public WarrantyModel Create(WarrantyModel warrantyModel)
        {
            return _context.WarrantyModel.Add(warrantyModel);
        }

        /// <summary>
        /// Cập nhật loại bảo hành
        /// </summary>
        /// <param name="warrantyModel">Warranty Model</param>
        public void Update(WarrantyModel warrantyModel)
        {
            var warrantyInDb = _context.WarrantyModel.FirstOrDefault(p => p.WarrantyId == warrantyModel.WarrantyId);
            //Name
            warrantyInDb.WarrantyName = warrantyModel.WarrantyName;
            //Bao gom
            warrantyInDb.Coverage = warrantyModel.Coverage;
            //Thoi gian BH
            warrantyInDb.Duration = warrantyModel.Duration;

            warrantyInDb.LastEditBy = warrantyModel.LastEditBy;
            warrantyInDb.LastEditTime = warrantyModel.LastEditTime;

            _context.Entry(warrantyInDb).State = EntityState.Modified;
        }
        //Web Tra Cứu
        #region Tra cứu theo số serial
        /// <summary>
        /// Get productId from Serial
        /// </summary>
        /// <param name="Serial">Serial of Product</param>
        /// <returns>ProductId</returns>
        public Guid GetProductIdBySerial(string Serial)
        {
            var serialInfor = GetSerialInfor(Serial);
            var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == serialInfor.CompanyCode);
            var productId = (from p in _context.ProductModel
                             where (p.CompanyId == company.CompanyId)
                             && (p.ERPProductCode == serialInfor.ProductCode)
                             select p.ProductId).FirstOrDefault();
            if (productId == null)
            {
                var materialType = "Z" + serialInfor.ProductCode.Substring(0, 2);
                productId = new ProductRepository(_context).SyncMaterial(company.CompanyCode, 1, materialType, serialInfor.ProductCode);
            }
            return productId;
        }

        /// <summary>
        /// Get ProductId theo ERPCode
        /// </summary>
        /// <param name="ERPCode"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public Guid GetProductIdByERPCode(string ERPCode, string companyCode)
        {
            var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == companyCode);
            var productId = (from p in _context.ProductModel
                             where (p.CompanyId == company.CompanyId)
                             && (p.ERPProductCode == ERPCode)
                             select p.ProductId).FirstOrDefault();
            return productId;
        }
        public SerialViewModel GetSerialInfor(string Serial)
        {
            var serialInfor = new SerialViewModel();
            var dataTable = GetSerialStatus(Serial);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var item = dataTable.Rows[0];
                serialInfor.SerialNo = item["SERIALNO"].ToString();
                //Công ty
                serialInfor.CompanyCode = item["WERKS"].ToString();
                //ID trạng thái: I0099 || I0000
                serialInfor.StatusId = item["IDSTATUS"].ToString();
                //Mã trạng thái: YES || NONE
                serialInfor.StatusCode = item["NOSTATUS"].ToString();
                //Mo tả trạng thái: Yes available || No available
                serialInfor.StatusName = item["NAMESTATUS"].ToString();
                serialInfor.ProductName = item["MAKTX"].ToString();
                serialInfor.ProductCode = item["MATNR"].ToString();
                serialInfor.Unit = item["MATNR"].ToString();
                //Số lô
                serialInfor.Batch = item["BATCH"].ToString();
            }
            return serialInfor;
        }

        public bool CheckSerialReady(string Serial)
        {
            var result = false;
            var serialInfor = GetSerialInfor(Serial);
            //ID trạng thái = I0099 và Mã trạng thái = YES thì serial hợp lệ và sẵn sàng kích hoạt bảo hành
            if (serialInfor.StatusId == "I0099" && serialInfor.StatusCode == "YES")
            {
                result = true;
            }
            return result;
        }
        private DataTable GetSerialStatus(string Serial)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_CHECK_SERIAL_BH);

            //Thông số truyền vào
            function.SetValue("IM_SERIAL", Serial);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("SERI_T").ToDataTable("SERI_T");

            return datatable;
        }
        #endregion

        #region Tra cứu theo mã OD
        public string ConvertQRCodeToOrderDelivery(string QRCode)
        {
            if (QRCode.Contains("ACC"))
            {
                QRCode = QRCode.Substring(3);
            }
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_GET_GIAIMA);

            //Thông số truyền vào
            function.SetValue("IM_STRING", QRCode);

            //Thực thi
            function.Invoke(destination);

            var result = function.GetString("EX_DECODE").ToString();
            var OrderDelivery = result.Substring(0, result.IndexOf("*"));

            return OrderDelivery;
        }
        public DataTable GetOrderInfo(string OrderDelivery)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();

            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_OD_BH);

            //Thông số truyền vào
            function.SetValue("IM_VBELN", OrderDelivery);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("LISTOD_T").ToDataTable("LISTOD_T");

            return datatable;
        }
        public List<OrderDetailViewModel> GetOrderDetails(string OrderDelivery)
        {
            List<OrderDetailViewModel> lst = new List<OrderDetailViewModel>();
            var dataTable = GetOrderInfo(OrderDelivery);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var SerialNo = row["SERIALNO"].ToString();
                    var ProductCode = row["MATNR"].ToString();
                    var ProductId = _context.ProductModel.Where(p => p.ERPProductCode == ProductCode)
                                            .Select(p => p.ProductId).FirstOrDefault();

                    OrderDetailViewModel detail = new OrderDetailViewModel();
                    detail.OrderDelivery = row["VBELN"].ToString();
                    detail.CompanyCode = row["WERKS"].ToString();
                    var DocumentDate_Str = row["DOCUDAT"].ToString();
                    if (!string.IsNullOrEmpty(DocumentDate_Str))
                    {
                        UtilitiesRepository _utilitiesRepository = new UtilitiesRepository();
                        var DocumentDate = _utilitiesRepository.ConvertDateTimeFromddMMyyyy(DocumentDate_Str);
                        detail.DocumentDate = DocumentDate;
                    }
                    var PostDate_Str = row["POSTDAT"].ToString();
                    if (!string.IsNullOrEmpty(PostDate_Str))
                    {
                        UtilitiesRepository _utilitiesRepository = new UtilitiesRepository();
                        detail.PostDate = _utilitiesRepository.ConvertDateTimeFromddMMyyyy(PostDate_Str);
                    }
                    detail.OrderDeliveryCategory = row["POSNR"].ToString();
                    detail.ProductCode = ProductCode;
                    detail.ProductName = row["ARKTX"].ToString();
                    detail.Unit = row["VRKME"].ToString();
                    var Quantity_Str = row["LFIMG"].ToString();
                    if (!string.IsNullOrEmpty(Quantity_Str))
                    {
                        detail.Quantity = Convert.ToDecimal(Quantity_Str);//Số lượng mua/Bán
                    }
                    detail.Batch = row["BATCH"].ToString();
                    detail.SerialNo = SerialNo;
                    detail.ProductCategoryCode = row["PRDHA"].ToString();
                    detail.ProductCategoryName = row["PRDHANM"].ToString();
                    detail.WarrantyCode = row["WARRANTY"].ToString();
                    detail.WarrantyUnit = row["WARRDVT"].ToString();
                    detail.SaleOrderCode = row["VGBEL"].ToString();
                    detail.SaleOrderCodeCategory = row["VGPOS"].ToString();
                    detail.ProfileForeignCode = row["KUNNR"].ToString();
                    detail.ProfileName = row["CUSTNM"].ToString();
                    detail.ProfileAddress = row["CUSTADDR"].ToString();
                    detail.Phone = row["TEL1"].ToString();

                    //Nếu có số serial => check exist theo SerialNo
                    if (!string.IsNullOrEmpty(SerialNo))
                    {
                        var res = (from p in _context.ProductWarrantyModel
                                   where p.SerriNo == SerialNo
                                   select p).FirstOrDefault();
                        if (res == null)
                        {
                            lst.Add(detail);
                        }
                    }
                    //Nếu không có Serial, check theo số OD và sản phẩm
                    else
                    {
                        //Nếu SP đã đồng bộ, check theo ProductId

                        //Do trong 1 OD có sản phẩm có nhiều số lượng và mỗi lần kích hoạt có thễ khác nhau
                        //=> check theo OD + ProductId và nếu số lượng đã kích hoạt nhỏ hơn SL trong OD thì được tiếp tục kích hoạt BH

                        if (ProductId != null && ProductId != Guid.Empty)
                        {
                            //Get SL đã kích hoạt
                            var ActivatedQuantity = (from p in _context.ProductWarrantyModel
                                                     where p.ProductId == ProductId && p.OrderDelivery == OrderDelivery
                                                     group p by new { p.ProductId, p.OrderDelivery } into g
                                                     select new
                                                     {
                                                         Value = g.Sum(p => p.ActivatedQuantity)
                                                     }).FirstOrDefault();
                           
                            if (ActivatedQuantity != null)
                            {
                                //Số lượng kích hoạt nhỏ hơn số lượng trong OD => tiếp tục được kích hoạt
                                if (ActivatedQuantity.Value < detail.Quantity)
                                {
                                    detail.Quantity = detail.Quantity - ActivatedQuantity.Value; //Set lại số lượng sau khi có SP đã đăng ký.
                                    lst.Add(detail);
                                }
                            }
                            else
                            {
                                //Chưa có SP nào trong OD đó được kích hoạt
                                lst.Add(detail);
                            }
                           
                            //var res = (from p in _context.ProductWarrantyModel
                            //           where p.OrderDelivery == OrderDelivery
                            //           && p.ProductId == ProductId
                            //           select p).FirstOrDefault();
                            //if (res == null)
                            //{
                            //    lst.Add(detail);
                            //}
                        }
                        //Nếu SP chưa được đồng bộ, check theo mã SAP
                        else
                        {
                            var res = (from p in _context.ProductWarrantyModel
                                       where p.OrderDelivery == OrderDelivery
                                       && p.ERPProductCode == ProductCode
                                       select p).FirstOrDefault();
                            if (res == null)
                            {
                                lst.Add(detail);
                            }
                        }
                    }
                }
            }
            return lst;
        }
        #endregion
    }
}