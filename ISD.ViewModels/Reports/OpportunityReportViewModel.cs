using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class OpportunityReportViewModel
    {
        public Int64 NumberIndex { get; set; }
        public string InvestorName { get; set; }
        public string DesignName { get; set; }
        public string Contractor { get; set; }
        public string Competitor { get; set; }
        public string CompetitorName { get; set; }
        public string SaleOfficeName { get; set; }
        public decimal? OpportunityPercentage { get; set; }
        public string SalesEmployeeName1 { get; set; }
        public string SalesEmployeeName2 { get; set; }
        public string SalesEmployeeName3 { get; set; }
        public string OpportunityName { get; set; }
        public string AccessoryList { get; set; }
        public string CompleteYear { get; set; }

        //Địa chỉ
        public string Address { get; set; }
        //Số điện thoại
        public string Phone { get; set; }
        //Website
        public string Website { get; set; }
        //Độ phủ thị trường
        public decimal? Number1 { get; set; }
        //Vốn pháp định
        public decimal? Number2 { get; set; }
        //loại hình
        public string OpportunityType { get; set; }
        // Qui mô
        public decimal? ProjectGabarit { get; set; }
        // đvt
        public string OpportunityUnit { get; set; }
        // tỉnh thành
        public string ProvinceName { get; set; }
        // tiêu chuẩn bàn giao
        public string HandoverFurniture { get; set; }
        // Tình trạng dự án
        public string OpportunityStatusType { get; set; }
        // tình hình dự án
        public string OpportunityStatus { get; set; }
        // Spec
        public string SpecDescription { get; set; }
        // sale/Spec
        //public string SaleSpec { get; set; }
        public string SaleSpec
        {
            get
            {
                if (string.IsNullOrEmpty(SalesEmployeeName1) && string.IsNullOrEmpty(SalesEmployeeName2) && string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return string.Empty;
                }
                else if (!string.IsNullOrEmpty(SalesEmployeeName1) && string.IsNullOrEmpty(SalesEmployeeName2) && string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName1;
                }
                else if (!string.IsNullOrEmpty(SalesEmployeeName1) && !string.IsNullOrEmpty(SalesEmployeeName2) && string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName1 + ", " + SalesEmployeeName2;
                }
                else if (string.IsNullOrEmpty(SalesEmployeeName1) && !string.IsNullOrEmpty(SalesEmployeeName2) && string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName2;
                }
                else if (string.IsNullOrEmpty(SalesEmployeeName1) && !string.IsNullOrEmpty(SalesEmployeeName2) && !string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName2 + ", " + SalesEmployeeName3;
                }
                else if (string.IsNullOrEmpty(SalesEmployeeName1) && string.IsNullOrEmpty(SalesEmployeeName2) && !string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName3;
                }
                else if (!string.IsNullOrEmpty(SalesEmployeeName1) && string.IsNullOrEmpty(SalesEmployeeName2) && !string.IsNullOrEmpty(SalesEmployeeName3))
                {
                    return SalesEmployeeName1 + ", " + SalesEmployeeName3;
                }
                return SalesEmployeeName1 + ", " + SalesEmployeeName2 + ", " + SalesEmployeeName3;
            }
        }
        //GT spec
        public decimal? Spec { get; set; }
        // Thi công
        public decimal? Construction { get; set; }
        //Giá trị
        public decimal? ProjectValue { get; set; }
        //Tổng GT trúng thầu
        public decimal? ProjectWonValue { get; set; }
        //Tổng GT rớt thầu
        public decimal? ProjectLoseValue { get; set; }
        public decimal? SumProjectValue { get; set; }
        public decimal? PercentProjectValue {
            get {
                decimal? res = 0;
                if (SumProjectValue != 0 && SumProjectValue != null)
                {
                    res = (ProjectValue / SumProjectValue) * 100;
                }
                return res;
            }
        }
        //Nhóm hàng
        public string Category { get; set; }

        #region CĐT
        [Display(Name = "Tên CĐT")]
        public string CDT_InvestorName { get { return InvestorName; } }
        [Display(Name = "Tên dự án")]
        public string CDT_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Xác suất")]
        public decimal? CDT_OpportunityPercentage { get { return OpportunityPercentage; } }

        [Display(Name = "Qui mô")]
        public decimal? CDT_ProjectGabarit { get { return ProjectGabarit; } }
        [Display(Name = "ĐVT")]
        public string CDT_OpportunityUnit { get { return OpportunityUnit; } }
        [Display(Name = "Tỉnh/TP")]
        public string CDT_ProvinceName { get { return ProvinceName; } }
        [Display(Name = "Tiêu chuẩn bàn giao")]
        public string CDT_HandoverFurniture { get { return HandoverFurniture; } }
        [Display(Name = "Tình hình dự án")]
        public string CDT_OpportunityStatus { get { return OpportunityStatus; } }
        [Display(Name = "Giá trị")]
        public decimal? CDT_ProjectValue { get { return ProjectValue; } }
        [Display(Name = "Năm hoàn thiện")]
        public string CDT_CompleteYear { get { return CompleteYear; } }
        #endregion

        #region Thiết kế
        [Display(Name = "Tên thiết kế")]
        public string DS_DesignName { get { return DesignName; } }
        [Display(Name = "Tên dự án")]
        public string DS_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Xác suất")]
        public decimal? DS_OpportunityPercentage { get { return OpportunityPercentage; } }
        [Display(Name = "Spec")]
        public string DS_Spec { get { return SpecDescription; } }
        [Display(Name = "Tên NVKD")]
        public string DS_SalesEmployeeName1 { get { return SalesEmployeeName1; } }
        [Display(Name = "NV Spec")]
        public string DS_SaleSpec { get { return SalesEmployeeName3; } }
        [Display(Name = "Qui mô")]
        public decimal? DS_ProjectGabarit { get { return ProjectGabarit; } }
        [Display(Name = "ĐVT")]
        public string DS_OpportunityUnit { get { return OpportunityUnit; } }
        [Display(Name = "Tỉnh/TP")]
        public string DS_ProvinceName { get { return ProvinceName; } }
        [Display(Name = "GT Spec")]
        public decimal? DS_GTSpec { get { return Spec; } }
        #endregion

        #region Tổng thầu
        [Display(Name = "Tên tổng thầu")]
        public string CR_ContractorName { get { return Contractor; } }
        [Display(Name = "Tên dự án")]
        public string CR_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Xác suất")]
        public decimal? CR_OpportunityPercentage { get { return OpportunityPercentage; } }
        [Display(Name = "Tên NVKD")]
        public string CR_SalesEmployeeName1 { get { return SalesEmployeeName1; } }
        [Display(Name = "Qui mô")]
        public decimal? CR_ProjectGabarit { get { return ProjectGabarit; } }
        [Display(Name = "ĐVT")]
        public string CR_OpportunityUnit { get { return OpportunityUnit; } }
        [Display(Name = "GT thi công")]
        public decimal? CR_Construction { get { return Construction; } }
        [Display(Name = "Năm hoàn thiện")]
        public string CR_CompleteYear { get { return CompleteYear; } }
        #endregion

        #region Đối thủ
        [Display(Name = "Tên đối thủ")]
        public string CP_CompetitorName { get { return CompetitorName; } }
        [Display(Name = "Địa chỉ")]
        public string CP_Address { get { return Address; } }
        [Display(Name = "Điện thoại")]
        public string CP_Phone { get { return Phone; } }
        [Display(Name = "Web")]
        public string CP_Webstie { get { return Website; } }
        [Display(Name = "Nhóm hàng")]
        public string CP_Category { get { return Category; } }
        [Display(Name = "Độ phủ thị trường(%)")]
        public decimal? CP_Number1 { get { return Number1; } }
        [Display(Name = "Vốn pháp định(Tỷ)")]
        public decimal? CP_Number2 { get { return Number2; } }
        #endregion

        #region Khu vực
        [Display(Name = "Tỉnh/TP")]
        public string P_ProvinceName { get { return ProvinceName; } }
        [Display(Name = "Tên dự án")]
        public string P_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên CĐT")]
        public string P_InvestorName { get { return InvestorName; } }
        [Display(Name = "Tên thiết kế")]
        public string P_DesignName { get { return DesignName; } }
        [Display(Name = "Giá trị")]
        public decimal? P_ProjectValue { get { return ProjectValue; } }
        #endregion

        #region Sale/spec
        [Display(Name = "Tên sale/spec")]
        public string S_SaleSpec { get { return SaleSpec; } }
        [Display(Name = "Tên dự án")]
        public string S_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên chủ đầu tư")]
        public string S_InvestorName { get { return InvestorName; } }
        [Display(Name = "Tên thiết kế")]
        public string S_DesignName { get { return DesignName; } }
        [Display(Name = "Spec")]
        public string S_Spec { get { return SpecDescription; } }
        [Display(Name = "Tình hình dự án")]
        public string S_OpportunityStatus { get { return OpportunityStatus; } }
        [Display(Name = "GT Spec")]
        public decimal? S_GTSpec { get { return Spec; } }
        [Display(Name = "GT Thi công")]
        public decimal? S_Construction { get { return Construction; } }
        #endregion

        #region Xác suất
        [Display(Name = "Xác suất")]
        public decimal? OP_OpportunityPercentage { get { return OpportunityPercentage; } }
        [Display(Name = "Tên dự án")]
        public string OP_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên chủ đầu tư")]
        public string OP_InvestorName { get { return InvestorName; } }
        [Display(Name = "Giá trị")]
        public decimal? OP_ProjectValue { get { return ProjectValue; } }
        #endregion

        #region Phụ kiện
        [Display(Name = "Phụ kiện")]
        public string A_AccessoryList { get { return AccessoryList; } }
        [Display(Name = "Tên dự án")]
        public string A_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên chủ đầu tư")]
        public string A_InvestorName { get { return InvestorName; } }
        [Display(Name = "Tên thiết kế")]
        public string A_DesignName { get { return DesignName; } }
        [Display(Name = "Giá trị")]
        public decimal? A_AccessoryValue { get { return ProjectValue; } }
        #endregion

        #region Giá trị hợp đồng
        [Display(Name = "Tên dự án")]
        public string GT_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên CĐT")]
        public string GT_InvestorName { get { return InvestorName; } }
        [Display(Name = "Giá trị")]
        public decimal? GT_ProjectValue { get { return ProjectValue; } }
        #endregion

        #region Qui mô
        [Display(Name = "Qui mô")]
        public decimal? QM_ProjectGabarit { get { return ProjectGabarit; } }
        [Display(Name = "Tên dự án")]
        public string QM_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên CĐT")]
        public string QM_InvestorName { get { return InvestorName; } }
        #endregion

        #region Nhóm hàng
        [Display(Name = "Nhóm hàng")]
        public string NH_Category { get { return Category; } }
        [Display(Name = "Tên dự án")]
        public string NH_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên CĐT")]
        public string NH_InvestorName { get { return InvestorName; } }
        [Display(Name = "% Giá trị")]
        public decimal? NH_PercentProjectValue { get { return Math.Round((decimal)PercentProjectValue,3); } }
        [Display(Name = "Giá trị")]
        public decimal? NH_AccessoryValue { get { return ProjectValue; } }      
    
        #endregion

        #region Năm hoàn thiện
        [Display(Name = "Năm hoàn thiện")]
        public string CY_CompleteYear { get { return CompleteYear; } }
        [Display(Name = "Tên dự án")]
        public string CY_OpportunityName { get { return OpportunityName; } }
        [Display(Name = "Tên CĐT")]
        public string CY_InvestorName { get { return InvestorName; } }
        [Display(Name = "Giá trị")]
        public decimal? CY_ProjectValue { get { return ProjectValue; } }
        #endregion
    }
}
