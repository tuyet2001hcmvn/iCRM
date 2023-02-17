using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MaterialModel", Schema = "ghMasterData")]
    public partial class MaterialModel
    {
        public MaterialModel()
        {
            MaterialAccessoryMappingModels = new HashSet<MaterialAccessoryMappingModel>();
            MaterialInvoicePriceModels = new HashSet<MaterialInvoicePriceModel>();
            MaterialMinMaxPriceModels = new HashSet<MaterialMinMaxPriceModel>();
            MaterialPriceModels = new HashSet<MaterialPriceModel>();
            MaterialRegistrationFeePriceModels = new HashSet<MaterialRegistrationFeePriceModel>();
            MaterialServiceMappingModels = new HashSet<MaterialServiceMappingModel>();
            SaleOrderMasterModels = new HashSet<SaleOrderMasterModel>();
        }

        [Key]
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [StringLength(400)]
        public string MaterialName { get; set; }
        [StringLength(50)]
        public string MaterialUnit { get; set; }
        [StringLength(50)]
        public string MaterialUnitName { get; set; }
        [StringLength(50)]
        public string MaterialGroupCode { get; set; }
        [StringLength(50)]
        public string ProfitCenterCode { get; set; }
        [StringLength(50)]
        public string ProductHierarchyCode { get; set; }
        [StringLength(50)]
        public string LaborCode { get; set; }
        [StringLength(50)]
        public string MaterialFreightGroupCode { get; set; }
        [StringLength(50)]
        public string ExternalMaterialGroupCode { get; set; }
        [StringLength(50)]
        public string TemperatureConditionCode { get; set; }
        [StringLength(50)]
        public string ContainerRequirementCode { get; set; }
        [StringLength(155)]
        public string InternalComment { get; set; }
        [StringLength(155)]
        public string SalesText { get; set; }
        [StringLength(155)]
        public string BasicDataText { get; set; }
        [StringLength(50)]
        public string OldMaterial { get; set; }
        [StringLength(100)]
        public string Dimension { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Capacity { get; set; }
        [StringLength(10)]
        public string CapacityUnit { get; set; }
        [StringLength(400)]
        public string ImageUrl { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BalanceDueMax { get; set; }
        public bool? Actived { get; set; }

        [ForeignKey(nameof(ContainerRequirementCode))]
        [InverseProperty(nameof(ContainerRequirementModel.MaterialModels))]
        public virtual ContainerRequirementModel ContainerRequirementCodeNavigation { get; set; }
        [ForeignKey(nameof(ExternalMaterialGroupCode))]
        [InverseProperty(nameof(ExternalMaterialGroupModel.MaterialModels))]
        public virtual ExternalMaterialGroupModel ExternalMaterialGroupCodeNavigation { get; set; }
        [ForeignKey(nameof(LaborCode))]
        [InverseProperty(nameof(LaborModel.MaterialModels))]
        public virtual LaborModel LaborCodeNavigation { get; set; }
        [ForeignKey(nameof(MaterialFreightGroupCode))]
        [InverseProperty(nameof(MaterialFreightGroupModel.MaterialModels))]
        public virtual MaterialFreightGroupModel MaterialFreightGroupCodeNavigation { get; set; }
        [ForeignKey(nameof(MaterialGroupCode))]
        [InverseProperty(nameof(MaterialGroupModel.MaterialModels))]
        public virtual MaterialGroupModel MaterialGroupCodeNavigation { get; set; }
        [ForeignKey(nameof(ProductHierarchyCode))]
        [InverseProperty(nameof(ProductHierarchyModel.MaterialModels))]
        public virtual ProductHierarchyModel ProductHierarchyCodeNavigation { get; set; }
        [ForeignKey(nameof(ProfitCenterCode))]
        [InverseProperty(nameof(ProfitCenterModel.MaterialModels))]
        public virtual ProfitCenterModel ProfitCenterCodeNavigation { get; set; }
        [ForeignKey(nameof(TemperatureConditionCode))]
        [InverseProperty(nameof(TemperatureConditionModel.MaterialModels))]
        public virtual TemperatureConditionModel TemperatureConditionCodeNavigation { get; set; }
        [InverseProperty(nameof(MaterialAccessoryMappingModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialAccessoryMappingModel> MaterialAccessoryMappingModels { get; set; }
        [InverseProperty(nameof(MaterialInvoicePriceModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialInvoicePriceModel> MaterialInvoicePriceModels { get; set; }
        [InverseProperty(nameof(MaterialMinMaxPriceModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialMinMaxPriceModel> MaterialMinMaxPriceModels { get; set; }
        [InverseProperty(nameof(MaterialPriceModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialPriceModel> MaterialPriceModels { get; set; }
        [InverseProperty(nameof(MaterialRegistrationFeePriceModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialRegistrationFeePriceModel> MaterialRegistrationFeePriceModels { get; set; }
        [InverseProperty(nameof(MaterialServiceMappingModel.MaterialCodeNavigation))]
        public virtual ICollection<MaterialServiceMappingModel> MaterialServiceMappingModels { get; set; }
        [InverseProperty(nameof(SaleOrderMasterModel.MaterialCodeNavigation))]
        public virtual ICollection<SaleOrderMasterModel> SaleOrderMasterModels { get; set; }
    }
}
