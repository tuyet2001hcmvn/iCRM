using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.Constant
{
    public static class ConstFunction
    {
        //Permission: Create, Edit, Delete, Import, Export,...

        //Permission on master 
        public const string Access = "INDEX";
        public const string Create = "CREATE";
        public const string Import = "IMPORT";
        public const string Export = "EXPORT";
        public const string CreatePivotReportSystem = "CREATEPIVOTREPORTSYSTEM";
        public const string CreatePivotReport = "CREATEPIVOTREPORT";

        //Permission on details
        public const string Edit = "EDIT";
        public const string Delete = "DELETE";
        public const string Upload = "UPLOAD";
        public const string UploadMultipleImages = "UPLOAD_IMG";
        public const string View = "VIEW";
        public const string Restore = "RESTORE";

        //Thêm Item 
        public const string CREATE_ACCESSORY = "CREATE_ACCESSORY";
        //Cập nhật Item
        public const string UPDATE_ACCESSORY = "UPDATE_ACCESSORY";
        //Xóa Item
        public const string DELETE_ACCESSORY = "DELETE_ACCESSORY";

        public const string EDIT_ACCESSORY_TYPE = "EDIT_ACCESSORY_TYPE";
        

        public const string CREATE_PROMOTION = "CREATE_PROMOTION";
        public const string CREATE_SERVICE = "CREATE_SERVICE";
        public const string CREATE_RECEIPT = "CREATE_RECEIPT";
        public const string CREATE_AFTER_COMPLETE = "CREATE_AFTER_COMPLETE";
        public const string SAVE_CUSTOMER = "SAVE_CUSTOMER";
        
        public const string CANCEL = "CANCEL";
        public const string CANCEL_AFTER_COMPLETE = "CANCEL_AFTER_COMPLETE";
        public const string SAVE = "SAVE";
        public const string ESTIMATED = "ESTIMATED";

        public const string VIEW_STORE = "VIEW_STORE";

        public const string FORCE_LOGOUT = "FORCE_LOGOUT";

        //Giảm giá
        public const string DISCOUNT = "DISCOUNT";


        // EXPORT
        
        public const string EXPORT_DONBANXE = "EXPORT_DONBANXE";
        public const string EXPORT_BANTHUTUNG = "EXPORT_BANTHUTUNG";
        public const string EXPORT_KHAN = "EXPORT_KHAN";
        public const string EXPORT_DATHANG = "EXPORT_DATHANG";
        public const string EXPORT_HUY = "EXPORT_HUY";
        public const string EXPORT_BAOCAOTHUCHI = "EXPORT_BAOCAOTHUCHI";
        public const string EXPORTBAOCAO = "EXPORTBAOCAO";

        //Đồng bộ
        public const string SYNC = "SYNC";

        //Gộp dữ liệu trùng
        public const string MERGE = "MERGE";

        //Print Profile
        public const string PRINTPROFILE = "PRINTPROFILE";


        //Chỉnh sửa PO
        public const string CHINHSUA_PO = "CHINHSUA_PO";
    }
}
