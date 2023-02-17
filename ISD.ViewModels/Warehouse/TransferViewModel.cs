using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TransferViewModel
    {
        public Guid? StockTransferRequestId { get; set; }

        public int StockTransferRequestCode { get; set; }

        public System.Guid TransferId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransferCode")]
        public int TransferCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DocumentDate")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DocumentDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Nullable<System.Guid> CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Nullable<System.Guid> StoreId { get; set; }
        public string StoreName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_SalesEmployee")]
        public string SalesEmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }
        public string CreateByName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }
        public bool? isDeleted { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientName")]
        public string RecipientName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientAddress")]
        public string RecipientAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientPhone")]
        public string RecipientPhone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RecipientCompany")]
        public string RecipientCompany { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderAddress")]
        public string SenderAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SenderPhone")]
        public string SenderPhone { get; set; }
        public bool? IsEdit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeletedReason")]
        public string DeletedReason { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_StockReceive")]
        public string ToStockCode { get; set; }

        public List<TransferDetailViewModel> transferDetail { get; set; }

        public DeliveryViewModel transferSender { get; set; }
    }
}
