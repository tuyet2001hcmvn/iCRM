using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class StatisticLikeViewProductViewModel
    {
        public int? NumberIndex { get; set; }
        [Display(Name = "Nhóm Sản phẩm")]
        public string NhomVT { get; set; }
        [Display(Name = "Phân loại vật tư")]
        public string PLoaiVT { get; set; }

        [Display(Name = "Mã SAP")]
        public string MaSAP { get; set; }

        [Display(Name = "Mã thương mại")]
        public string MaSP { get; set; }

        [Display(Name = "Tên sản phầm")]
        public string TenSP { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuotLikedCRM")]
        public int? SoLuotLikedCRM { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuotLikedCRMNV")]
        public int? SoLuotLikedCRMNV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuotViewedAC")]
        public int? SoLuotViewedAC { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuotLikedAC")]
        public int? SoLuotLikedAC { get; set; }


        public int? Total { get; set; }
    }

    public class StatisticLikeViewSearchViewModel
    {
        [Display(Name = "Nhân viên")]
        public string SaleEmployeeCode { get; set; }
        [Display(Name = "Sắp xêp theo")] 
        public string FieldTOP { get; set; }

        [Display(Name = "Top sản phẩm yêu thích")]
        public int? TOP { get; set; }
        [Display(Name = "Thời gian")]
        public string CommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        public List<Guid> StoreId { get; set; }
        public bool IsView { get; set; }
    }
}