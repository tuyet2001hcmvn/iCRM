using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskSearchViewModel
    {
        //1. Mã yêu cầu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskCode")]
        public string TaskCode { get; set; }

        //2. Yêu cầu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Summary")]
        public string Summary { get; set; }

        //Mô tả
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string Description { get; set; }

        //3. Type: MyWork, MyFollow, TICKET, ACTIVITIES
        public string Type { get; set; }

        //4. Loại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
        public Guid? WorkFlowId { get; set; }
        //chọn nhiều
        public List<Guid> WorkFlowIdList { get; set; }

        //5a. Trạng thái (Guid)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public string TaskStatusCode { get; set; }
        public List<string> TaskStatusCodeList { get; set; }

        //5b. Nhóm trạng thái (string)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskProcessCode")]
        public string TaskProcessCode { get; set; }

        //6. Người giao việc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Reporter")]
        public string Reporter { get; set; }

        //7. Nhân viên được phân công
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Assignee")]
        public string Assignee { get; set; }
        public List<string> AssigneeList { get; set; }

        //8. Khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        public Nullable<System.Guid> ProfileId { get; set; }

        public string ProfileName { get; set; }

        //9. Liên hệ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Contact")]
        public Nullable<System.Guid> ContactId { get; set; }

        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Contact")]
        public Nullable<System.Guid> CompanyId { get; set; }

        public string CompanyName { get; set; }

        //10. Người tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }

        //11. Mức độ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_PriorityCode")]
        public string PriorityCode { get; set; }

        //12. Ngày tiếp nhận
        public string ReceiveCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? ReceiveFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ReceiveToDate { get; set; }

        //13. Ngày bắt đầu
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }

        //14. Ngày kết thúc dự kiến
        public string EstimateEndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EstimateEndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EstimateEndToDate { get; set; }

        //15. Ngày kết thúc
        public string EndCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? EndFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? EndToDate { get; set; }

        //16. Ngày tạo
        //public string CreateCommonDate { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        //public DateTime? CreateFromTime { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        //public DateTime? CreateToTime { get; set; }

        //17. Đơn vị thi công
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConstructionUnit")]
        public Guid? ConstructionUnit { get; set; }

        public string ConstructionUnitName { get; set; }

        //18. Lỗi thường gặp
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_CommonMistakeCode")]
        public string CommonMistakeCode { get; set; }

        //19. Loại mã lỗi
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorTypeCode2")]
        public string ErrorTypeCode { get; set; }

        //20. Mã lỗi
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorCode2")]
        public string ErrorCode { get; set; }

        //21. Nhóm công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
        public List<string> ServiceTechnicalTeamCode { get; set; }
        public Guid? KanbanId { get; set; }

        //mobile
        public bool? IsMobile { get; set; }

        //Ngày tiếp nhận
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_CommonDate")]
        public string CommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileGroup")]
        public string ProfileGroupCode { get; set; }
        public List<string> ProfileGroupCodeList { get; set; }

        //Phân cấp sản phẩm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TikectUsual_ProductHierarchy")]
        public List<string> ProductLevelCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public string RolesCode { get; set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public bool? isUnassign { get; set; }

        //Mã màu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductColorCode")]
        public List<string> ProductColorCode { get; set; }

        //Lỗi thường gặp
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UsualErrorCode")]
        public List<string> UsualErrorCode { get; set; }

        //Nhóm vật tư
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ProductCategory")]
        public List<string> ProductCategoryCode { get; set; }
        public List<string> ProductCategoryCodeList { get; set; }

        //Nhóm KH
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_General_CustomerGroup")]
        public string CustomerGroupCode { get; set; }

        //NV kinh doanh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
        public string SalesSupervisorCode { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Department")]
        public List<string> DepartmentCode { get; set; }
        //public List<string> DepartmentCodeList { get; set; }

        //Ngày tạo

        public string CreatedCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? CreatedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? CreatedToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GTB_VisitAddress")]
        public string VisitAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        //Phân loại chuyến thăm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_VisitTypeCode")]
        public List<string> VisitTypeCode { get; set; }

        //Trạng thái hoạt động
        [Display(Name = "Trạng thái hoạt động")]
        public bool? Actived { get; set; }

        //Nhóm VT
        [Display(Name = "Nhóm vật tư")]
        public List<Guid> CategoryId { get; set; }

        //NV kết thúc (người nhập Đã hoàn thành)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CompletedEmployee")]
        public string CompletedEmployee { get; set; }
        // ý kiến kh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerResult")]
        public string Property5 { get; set; }
        // Mã SAP SP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ERPProductCode")]
        public string ERPProductCode { get; set; }
        // Mã SAP phụ kiện
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ERPAccessoryCode")]
        public string ERPAccessoryCode { get; set; }
        // Mã SAP phụ kiện
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_AccessoryTypeCode")]
        public string AccessoryTypeCode { get; set; }

        //Khu vực
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> SaleOfficeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public List<Guid> ProvinceId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_DistrictId")]
        public List<Guid> DistrictId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public List<Guid> WardId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> VisitSaleOfficeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GTB_AddressType")]
        public bool? AddressType { get; set; }

        //Tỉnh/thành
        //Tỉnh/thành(Địa chỉ điểm trưng bày)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProvinceId")]
        public Guid? ProvinceId2 { get; set; }

        public string ProfileCode { get; set; }
        public bool IsView { get; set; }
        public Guid? DefaultWorkFlowId { get; set; }

        public string ReportType { get; set; }
    }
}