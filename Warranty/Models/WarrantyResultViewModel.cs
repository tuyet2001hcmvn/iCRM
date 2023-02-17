using ISD.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Warranty.Models
{
    public class WarrantyResultViewModel
    {
        public Guid ProductWarrantyId { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarrantyCode")]
        public Nullable<int> ProductWarrantyCode { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Profile_CustomerId")]
        //public System.Guid ProfileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Material_MaterialModel")]
        public System.Guid ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarranty_SerriNo")]
        public string SerriNo { get; set; }

        //Số phiếu BH
        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarrantyNo")]
        public string ProductWarrantyNo { get; set; }

        //Loại bảo hành
        //[Display(ResourceType = typeof(LanguageResource), Name = "WarrantyName")]
        //public Guid WarrantyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WarrantyName")]
        public string WarrantyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarranty_FormDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProWarranty_ToDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Profile_CustomerId")]
        public string ProfileName { get; set; }
        public string ProfilePhoneNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Material_MaterialModel")]
        public string ProductName { get; set; }
        public string Address { get; set; }
        public Guid ProfileId { get; set; }

    }
}