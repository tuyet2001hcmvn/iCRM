using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public static class ConstExcelController
    {
        #region Type Id Dropdownlist Excel
        public const string GuidId = "Guid";
        public const string IntId = "Int";
        public const string StringId = "String";
        public const string Bool = "Bool";
        #endregion

        //Url: Permission/Menu/ImportExcel
        public const string Menu = "I-00001";

        //Url: Permission/Roles/ImportExcel
        public const string Roles = "I-00002";

        //Url: Permission/Menu/ImportExcel
        public const string Module = "I-00003";


        // For Sale Excel Export 
        public const string Product = "I-00010"; // Sản phẩm (Phiên bản)
        public const string Material = "I-00010"; // Sản phẩm (Xe)
        public const string MaterialMinMaxPriceExport = "I-00010M";
        public const string Brand = "I-00020"; // Hãng xe
        public const string Category = "I-00030"; // Loại xe
        public const string Configuration = "I-00040"; // Đời xe
        public const string Style = "I-00050"; // Kiểu dáng
        public const string Color = "I-00060"; // Màu sắc

        // For MasterData Excel Export 
        public const string Province = "I-00100"; // Tỉnh thành
        public const string District = "I-00200"; // Quận huyện
        public const string SalesEmployee = "I-00300"; //Nhân viên
        public const string AcountI = "I00400"; //Insert Account - Tài khoản
        public const string AcountU = "U00400"; //Update Account - Tài khoản
        public const string Department = "I-01200"; //Phòng ban
        public const string QuestionBank = "I-00500"; //Phòng ban

        // For Customer Excel Export 
        public const string Profile = "I-00800"; // Thông tin khách hàng
        public const string Appointment = "I-00900"; // Khách ghé thăm

        public const string BeginningInventory = "I-00070"; // Tồn kho đầu kỳ
    }
}
