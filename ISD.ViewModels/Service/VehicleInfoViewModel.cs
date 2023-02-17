using AutoMapper;
using ISD.Core;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class VehicleInfoViewModel : IHaveCustomMappings
    {
        public System.Guid VehicleId { get; set; }
        public Nullable<System.Guid> CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Step2HangMuc")]
        public Nullable<decimal> CurrentKilometers { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Số sổ bảo hành")]
        public string WarrantyNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BuyDate")]
        public Nullable<System.DateTime> BuyDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProfitCenterCode")]
        public string ProfitCenterCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProfitCenterCode")]
        public string ProfitCenterName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProductHierarchyCode")]
        public string ProductHierarchyCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VehicleInfo_ProductHierarchyCode")]
        public string ProductHierarchyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        //public string ProfitCenter { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        //public string ProductHierarchy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string LaborCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureConditionCode { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        //public string SerialNumber { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        //public string EngineNumber { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<VehicleInfoModel, VehicleInfoViewModel>();
            configuration.CreateMap<ProfitCenterModel, VehicleInfoViewModel>();
            configuration.CreateMap<ProductHierarchyModel, VehicleInfoViewModel>();


            // Test only
            //configuration.CreateMap<VehicleInfoJoined, VehicleInfoViewModel>()
            //.ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.EngineNumber, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.CurrentKilometers, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.WarrantyNumber, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.BuyDate, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.SaleOrg, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProfitCenterCode, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProfitCenterName, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProductHierarchyCode, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProductHierarchyName, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProductHierarchyName, opt => opt.MapFrom(src => src.ProductHierarchy.ProductHierarchyName))
            //.ForMember(dest => dest.ProfitCenterName, opt => opt.MapFrom(src => src.ProfitCenter.ProfitCenterName));


        }
    }
}
