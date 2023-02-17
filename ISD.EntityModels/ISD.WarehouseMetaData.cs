using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.EntityModels
{
    [MetadataType(typeof(StockModel.MetaData))]
    public partial class StockModel
    {
        internal sealed class MetaData
        {
            public System.Guid StockId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockCode")]
            public string StockCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockName")]
            public string StockName { get; set; }

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

    [MetadataType(typeof(StockReceivingMasterModel.MetaData))]
    public partial class StockReceivingMasterModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockReceivingCode")]
            public int StockReceivingCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<System.DateTime> DocumentDate { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
            public Nullable<System.Guid> CompanyId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public Nullable<System.Guid> StoreId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
            public string SalesEmployeeCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_ProfileId")]
            public Nullable<System.Guid> ProfileId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }
        }
    }

    [MetadataType(typeof(StockReceivingDetailModel.MetaData))]
    public partial class StockReceivingDetailModel
    {
        internal sealed class MetaData
        {
            public Nullable<System.Guid> StockReceivingId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
            public Nullable<System.Guid> ProductId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
            public Nullable<System.Guid> StockId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<int> DateKey { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
            public Nullable<decimal> Quantity { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
            public Nullable<decimal> Price { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
            public Nullable<decimal> UnitPrice { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
        }
    }

    [MetadataType(typeof(TransferModel.MetaData))]
    public partial class TransferModel
    {
        internal sealed class MetaData
        {
            public System.Guid TransferId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransferCode")]
            public int TransferCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<System.DateTime> DocumentDate { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
            public Nullable<System.Guid> CompanyId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public Nullable<System.Guid> StoreId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
            public string SalesEmployeeCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }
        }
    }

    [MetadataType(typeof(TransferDetailModel.MetaData))]
    public partial class TransferDetailModel
    {
        internal sealed class MetaData
        {
            public System.Guid TransferDetailId { get; set; }
            public Nullable<System.Guid> TransferId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
            public Nullable<System.Guid> ProductId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Transfer_FromStock")]
            public Nullable<System.Guid> FromStockId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Tranfer_ToStock")]
            public Nullable<System.Guid> ToStockId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<int> DateKey { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
            public Nullable<decimal> Quantity { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
            public Nullable<decimal> Price { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
            public Nullable<decimal> UnitPrice { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
        }
    }

    [MetadataType(typeof(DeliveryModel.MetaData))]
    public partial class DeliveryModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeliveryCode")]
            public int DeliveryCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<System.DateTime> DocumentDate { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
            public Nullable<System.Guid> CompanyId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            public Nullable<System.Guid> StoreId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
            public string SalesEmployeeCode { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
            public Nullable<System.Guid> ProfileId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            public Nullable<System.DateTime> CreateTime { get; set; }
        }
    }

    [MetadataType(typeof(DeliveryDetailModel.MetaData))]
    public partial class DeliveryDetailModel
    {
        internal sealed class MetaData
        {

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Promotion_ProductName")]
            public Nullable<System.Guid> ProductId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
            public Nullable<System.Guid> StockId { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
            public Nullable<int> DateKey { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
            public Nullable<decimal> Quantity { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
            public Nullable<decimal> Price { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
            public Nullable<decimal> UnitPrice { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
            public string Note { get; set; }
        }
    }
}