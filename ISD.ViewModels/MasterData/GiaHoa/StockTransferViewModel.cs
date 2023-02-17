using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class StockTransferViewModel
    {
        //Mã đơn hàng dạng Guid trên web
        public string SO_WEB_GUID { get; set; }
        //Mã đơn hàng
        public string SO_WEB { get; set; }
        //Mã chi nhánh
        public string SALEORG { get; set; }
        //Mã plant
        public string PLANT { get; set; }
        //Ngày chuyển kho: YYYYMMDD
        //dd/MM/yyyy
        public string DOCDATE { get; set; }

        //Tên khách hàng
        public string CUS_NAME { get; set; }
        //Mã sản phẩm
        public string MATERIAL { get; set; }
        //Batch
        public string BATCH { get; set; }
        //Mã kho
        public string KHO { get; set; }
        //Số lượng
        public int SOLUONG { get; set; }
        //Đơn vị tính: viết tắt 3 ký tự
        public string UNIT { get; set; }
        //Chuyển về kho lắp ráp
        public string IS_LAPRAP { get; set; }
        //Chuyển về kho bán hàng
        public string IS_BANHANG { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
    }

    public class AcessoryStockTransferViewModel
    {
        //Mã đơn hàng dạng Guid trên web
        public string SO_WEB_GUID { get; set; }
        //Mã đơn hàng
        public string SO_WEB { get; set; }
        //Mã phụ tùng/phụ kiện
        public string LINE_GUID { get; set; }
        //Mã plant
        public string PLANT { get; set; }
        //Mã chi nhánh
        public string SALEORG { get; set; }
        //Ngày chuyển kho: YYYYMMDD
        //dd/MM/yyyy
        public string DOCDATE { get; set; }
        //Tên khách hàng
        public string CUS_NAME { get; set; }
        //Mã nhân viên
        public string USERNAME { get; set; }
        //Mã sản phẩm
        public string MATERIAL { get; set; }
        //Mã kho
        public string KHO { get; set; }
        //Số lượng
        public int SOLUONG { get; set; }
        //Đơn vị tính: viết tắt 3 ký tự
        public string UNIT { get; set; }
        //Chuyển về kho lắp ráp
        public string IS_LAPRAP { get; set; }
        //Chuyển về kho bán hàng
        public string IS_BANHANG { get; set; }
        //Là bảo hành khẩn
        public bool? IsBaoHanhKhan { get; set; }
        public string IS_BAOHANHKHAN { get; set; }
    }
}
