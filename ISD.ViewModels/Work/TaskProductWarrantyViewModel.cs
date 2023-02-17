using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskProductWarrantyViewModel
    {

        public int? STT { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public string ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public string Profile_ProfileName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_MaterialModel")]
        public string ProductName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyName")]
        public string WarrantyName { get; set; }

        public System.Guid? ProductWarrantyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarrantyCode")]
        public Nullable<int> ProductWarrantyCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public System.Guid? ProfileId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Material_MaterialModel")]
        public System.Guid? ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_FormDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WarrantyName")]
        public System.Guid? WarrantyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_SerriNo")]
        public string SerriNo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarrantyNo")]
        public string ProductWarrantyNo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProWarranty_ToDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}
