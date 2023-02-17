using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionViewModel
    {     
        public int? STT { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion")]
        public System.Guid ProductPromotionId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_TargetGroup")]
        public Nullable<System.Guid> TargetGroupId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_Type")]
        public string Type { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_Title")]
        public string ProductPromotionTitle { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_isSendCatalogue")]
        public bool? IsSendCatalogue { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_SendTypeCode")]
        public string SendTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_SendTypeName")]
        public string SendTypeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_StartTime")]
        public Nullable<System.DateTime> StartTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductPromotion_EndTime")]
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public string LastEditByName { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
        public int? ProfileCode { get; set; }
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesEmployeeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public string RolesCode { get; set; }
        public bool? Status { get; set; }
        public string CustomerAccountGroup { get; set; }
        public bool? isHasSAPCode { get; set; }
        public string ListProfileCode { get; set; } 
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        public List<ProductPromotionDetailViewModel> ppDetail { get; set; }
        public List<ProductPromotionDetailViewModel> TargetGroup { get; set; }
    }
}
