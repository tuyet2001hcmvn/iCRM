using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class ProductModel
    {
        public ProductModel()
        {
            TemplateAndGiftMemberAddressModels = new HashSet<TemplateAndGiftMemberAddressModel>();
        }

        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ErpproductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? BrandId { get; set; }
        public decimal? CylinderCapacity { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid ConfigurationId { get; set; }
        public string GuaranteePeriod { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsHot { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public Guid? CompanyId { get; set; }
        public bool? IsInventory { get; set; }
        public decimal? Price { get; set; }
        public Guid? WarrantyId { get; set; }

        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModels { get; set; }
    }
}
