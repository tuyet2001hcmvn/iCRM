using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskExcelViewModel
    {
        public Guid? TaskId { get; set; }

        [Display(Name = "Ngày tiếp nhận")]
        public DateTime? ReceiveDate { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateByName { get; set; }
        [Display(Name = "Mã CRM")]
        public int? ProfileCode { get; set; }
        [Display(Name = "Mã SAP")]
        public string ProfileForeignCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string ProfileName { get; set; }
        [Display(Name = "Tên ngắn KH")]
        public string ProfileShortName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "SĐT liên hệ")]
        public string Phone { get; set; }
        [Display(Name = "Loại")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Trung tâm bảo hành")]
        public string ServiceTechnicalTeam { get; set; }
        [Display(Name = "Mã NV được phân công")]
        public string AssigneeCode { get; set; }
        [Display(Name = "NV được phân công")]
        public string AssigneeName { get; set; }
        [Display(Name = "NV theo dõi/giám sát")]
        public string ReporterName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Kết quả")]
        public string CustomerReviews { get; set; }
        [Display(Name = "Đánh giá chất lượng dịch vụ")]
        public string ServiceRating { get; set; }
        [Display(Name = "Đánh giá chất lượng sản phẩm")]
        public string ProductRating { get; set; }
        [Display(Name = "Ý kiến khách hàng")]
        public string Review { get; set; }
        [Display(Name = "Mã SAP sản phẩm")]
        public string ERPProductCode { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Số lượng")]
        public int? Qty { get; set; }

        [Display(Name = "Phân loại sản phẩm")]
        public string ProductCategoryName { get; set; }
        [Display(Name = "Phương thức xử lý")]
        public string ErrorName { get; set; }
        [Display(Name = "Hình thức bảo hành")]
        public string ErrorTypeName { get; set; }
        [Display(Name = "Mã SAP phụ kiện")]
        public string ERPAccessoryCode { get; set; }
        [Display(Name = "Tên phụ kiện")]
        public string AccessoryName { get; set; }
        [Display(Name = "Số lượng PK")]
        public int AccessoryQty { get; set; }
        [Display(Name = "Hình thức bảo hành phụ kiện")]
        public string AccErrorTypeName { get; set; }
        [Display(Name = "Loại phụ kiện")]
        public string AccessoryCategoryName { get; set; }
        public Guid? ProfileId { get; set; }

        //THKH: Ngày thực hiện => EndDate
        [Display(Name = "Ngày thực hiện")]
        public DateTime? THKH_EndDate
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return EndDate;
                }
                return null;
            }
        }
       
        [Display(Name = "Trạng thái")]
        public string TaskStatusName { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Summary { get; set; }

        public string TaskStatusCode { get; set; }


        [Display(Name = "NV kinh doanh")]
        public string PersonInCharge { get; set; }

        [Display(Name = "Phòng ban")]
        public string RoleInCharge { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Địa điểm ghé thăm")]
        public string VisitAddress { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Ngày thi công")]
        public DateTime? ConstructionDate { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "Ngày dự kiến")]
        public DateTime? EstimateEndDate { get; set; }

        //THKH: Ngày dự kiến => StartDate
        [Display(Name = "Ngày dự kiến")]
        public DateTime? THKH_StartDate
        {
            get
            {
                if (StartDate.HasValue)
                {
                    return StartDate;
                }
                return null;
            }
        }

        [Display(Name = "Thời gian checkin")]
        public DateTime? CheckInTime { get; set; }
        [Display(Name = "Thời gian checkout")]
        public DateTime? CheckOutTime { get; set; }

        [Display(Name = "Số đơn hàng")]
        public string OrderCode { get; set; }

        [Display(Name = "Giá trị bảo hành")]
        public string WarrantyValue { get; set; }

        [Display(Name = "Mã màu")]
        public string ProductColorCode { get; set; }

        [Display(Name = "Danh sách lỗi")]
        public string UsualErrorName { get; set; }

        public string Ratings { get; set; }
        public string Reviews { get; set; }


        #region Bảo hành ACC
        [Display(Name = "Ngày tiếp nhận")]
        public DateTime? TICKET_ReceiveDate { get; set; }
        [Display(Name = "ID")]
        public string TICKET_TaskCode { get; set; }

        [Display(Name = "Mã CRM")]
        public int? TICKET_ProfileCode { get; set; }
        [Display(Name = "Mã SAP")]
        public string TICKET_ProfileForeignCode { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TICKET_ProfileName { get; set; }

        [Display(Name = "Tên ngắn KH")]
        public string TICKET_ProfileShortName { get; set; }

        [Display(Name = "Địa chỉ công trình")]
        public string TICKET_Address { get; set; }

        [Display(Name = "Tên liên hệ")]
        public string TICKET_ContactName { get; set; }

        [Display(Name = "SĐT liên hệ")]
        public string TICKET_Phone { get; set; }

        [Display(Name = "Mô tả")]
        public string TICKET_Description { get; set; }

        [Display(Name = "Ngày thực hiện")]
        public DateTime? TICKET_StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? TICKET_EndDate { get; set; }

        [Display(Name = "Kết quả/Ý kiến KH")]
        public string TICKET_CustomerReviews { get; set; }

        [Display(Name = "Danh sách lỗi")]
        public string TICKET_UsualErrorName { get; set; }
        [Display(Name = "Hình thức bảo hành")]
        public string TICKET_ErrorTypeName { get; set; }
        [Display(Name = "Phương thức xử lý")]
        public string TICKET_ErrorName { get; set; }
        [Display(Name = "Phụ kiện")]
        public string TICKET_AccessoryName { get; set; }
        [Display(Name = "Hình thức bảo hành phụ kiện")]
        public string TICKET_AccErrorTypeName { get; set; }
        [Display(Name = "Loại phụ kiện")]
        public string TICKET_AccessoryCategoryName { get; set; }

        [Display(Name = "Nguồn tiếp nhận")]
        public string TICKET_TaskSourceName { get; set; }

        [Display(Name = "Khu vực")]
        public string TICKET_SaleOfficeName { get; set; }

        [Display(Name = "NV tiếp nhận")]
        public string TICKET_CreateByName { get; set; }

        [Display(Name = "Mã ĐVTC")]
        public string TICKET_ConstructionUnit { get; set; }

        [Display(Name = "Tên ĐVTC")]
        public string TICKET_ConstructionUnitName { get; set; }

        [Display(Name = "NV Kinh doanh")]
        public string TICKET_PersonInCharge { get; set; }

        [Display(Name = "Ngày thi công")]
        public DateTime? TICKET_ConstructionDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        public string TICKET_OrderCode { get; set; }

        [Display(Name = "Mã NV được phân công")]
        public string TICKET_AssigneeCode { get; set; }

        [Display(Name = "NV được phân công")]
        public string TICKET_AssigneeName { get; set; }

        [Display(Name = "Nhóm vật tư")]
        public string TICKET_ProductCategoryName { get; set; }

        [Display(Name = "Tên sản phẩm/ Mã màu")]
        public string TICKET_ProductColorCode { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string TICKET_Unit { get; set; }

        [Display(Name = "Phân cấp SP")]
        public string TICKET_ProductLevelName { get; set; }

        [Display(Name = "Số lượng bảo hành")]
        public int? TICKET_Qty { get; set; }

        [Display(Name = "Số chứng từ")]
        public string TICKET_SAPSOProduct { get; set; }

        [Display(Name = "Giá trị đơn hàng")]
        public decimal? TICKET_ProductValue { get; set; }

        [Display(Name = "Giá trị bảo hành")]
        public decimal? TICKET_WarrantyValue { get; set; }

        [Display(Name = "Trạng thái")]
        public string TICKET_TaskStatusName { get; set; }

        [Display(Name = "Loại")]
        public string TICKET_WorkFlowName { get; set; }

        [Display(Name = "Hài lòng khách hàng")]
        public string TICKET_Ratings { get { return Ratings; } }

        [Display(Name = "Ý kiến khách hàng")]
        public string TICKET_Reviews { get { return Reviews; } }

        // Thông tin khảo sát
        public string TICKET_SurveyCreateByName { get; set; }
        public DateTime? TICKET_SurveyCreateByTime { get; set; }
        public string TICKET_SurveyProductQuality { get; set; }
        public string TICKET_SurveyEmployeeProfessional { get; set; }
        public string TICKET_SurveyServiceBehaviorEmployees { get; set; }
        public string TICKET_SurveyCustomerComments { get; set; }

        #endregion //Bảo hành ACC

        #region Điểm trưng bày

        public string GTB_TaskCode { get; set; }
        public string GTB_TaskSummary { get; set; }
        [Display(Name = "Mã")]
        public int? GTB_ProfileCode { get; set; }

        [Display(Name = "Mã SAP")]
        public string GTB_ProfileForeignCode { get; set; }

        [Display(Name = "Tên ngắn KH")]
        public string GTB_ProfileShortName { get; set; }

        [Display(Name = "Nhóm KH")]
        public string GTB_CustomerGroupName { get; set; }

        [Display(Name = "NV kinh doanh")]
        public string GTB_PersonInCharge { get; set; }

        [Display(Name = "Phòng ban")]
        public string GTB_RoleInCharge { get; set; }

        [Display(Name = "Trạng thái")]
        public string GTB_TaskStatusName { get; set; }

        [Display(Name = "Ngày lắp")]
        public DateTime? GTB_StartDate { get; set; }

        [Display(Name = "Địa chỉ GVL")]
        public string GTB_VisitAddress { get; set; }
        [Display(Name = "Địa chỉ ĐTB")]
        public string DTB_VisitAddress { get; set; }

        [Display(Name = "Phường/Xã")]
        public string GTB_WardName { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string GTB_DistrictName { get; set; }

        [Display(Name = "Tỉnh/Thành")]
        public string GTB_ProvinceName { get; set; }

        [Display(Name = "Khu vực")]
        public string GTB_SaleOfficeName { get; set; }

        [Display(Name = "Liên hệ")]
        public string GTB_ContactName { get; set; }

        [Display(Name = "SĐT liên hệ")]
        public string GTB_ContactPhone { get; set; }

        [Display(Name = "Giá trị GVL")]
        public decimal? GTB_ValueOfShowroom { get; set; }
        [Display(Name = "Giá trị ĐTB")]
        public decimal? DTB_ValueOfShowroom { get; set; }

        [Display(Name = "Ngày chăm sóc gần nhất")]
        public DateTime? GTB_NearestDate_THKH { get; set; }
        [Display(Name = "Nhân viên chăm sóc")]
        public string GTB_AssigneeName_THKH { get; set; }
        [Display(Name = "Nội dung chăm sóc")]
        public string GTB_Description_THKH { get; set; }

        [Display(Name = "Ngày chăm sóc dự kiến")]
        public DateTime? GTB_RemindDate_THKH { get; set; }

        [Display(Name = "Doanh thu năm trước đó")]
        public decimal? GTB_LastYearrevenue { get; set; }

        [Display(Name = "Doanh thu năm hiện tại")]
        public decimal? GTB_CurrentRevenue { get; set; }

        [Display(Name = "Số lượng")]
        public decimal? GTB_QuantityCatalogueOfShowroom { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? GTB_CreateTime { get; set; }

        [Display(Name = "Người tạo")]
        public string GTB_CreateByName { get; set; }

        #endregion //Điểm trưng bày

        #region Nhiệm vụ
        [Display(Name = "Ngày tiếp nhận")]
        public DateTime? ACTI_ReceiveDate { get; set; }
      
        [Display(Name = "Mã CRM")]
        public int? ACTI_ProfileCode { get; set; }
       
        [Display(Name = "Mã SAP")]
        public string ACTI_ProfileForeignCode { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string ACTI_ProfileName { get; set; }

        [Display(Name = "Tên ngắn KH")]
        public string ACTI_ProfileShortName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string ACTI_Address { get; set; }

        [Display(Name = "Khu vực")]
        public string ACTI_SaleOfficeName { get; set; }

        [Display(Name = "Tiêu đề")]
        public string ACTI_Summary { get; set; }

        [Display(Name = "Mô tả")]
        public string ACTI_Description { get; set; }

        [Display(Name = "Tên liên hệ")]
        public string ACTI_ContactName { get; set; }

        [Display(Name = "SDT liên hệ")]
        public string ACTI_Phone { get; set; }

        [Display(Name = "Email")]
        public string ACTI_Email { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? ACTI_EndDate { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? ACTI_StartDate { get; set; }

        [Display(Name = "Mã NV được phân công")]
        public string ACTI_AssigneeCode { get; set; }

        [Display(Name = "NV được phân công")]
        public string ACTI_AssigneeName { get; set; }

        [Display(Name = "Phòng Ban NV được phân công")]
        public string ACTI_RoleInCharge { get; set; }

        [Display(Name = "NV theo dõi/giám sát")]
        public string ACTI_ReporterName { get; set; }

        [Display(Name = "Trạng thái")]
        public string ACTI_TaskStatusName { get; set; }

        [Display(Name = "NV Kinh doanh")]
        public string ACTI_PersonInCharge { get; set; }

        [Display(Name = "Loại nhiệm vụ")]
        public string ACTI_WorkFlowName { get; set; }

        #endregion Nhiệm vụ

        #region Giao Việc
        [Display(Name = "Mã yêu cầu")]
        public string MIS_TaskCode { get; set; }
        [Display(Name = "Tiêu Đề")]
        public string MIS_Summary { get; set; }
        [Display(Name = "Trạng Thái")]
        public string MIS_TaskStatusName { get; set; }
        [Display(Name = "Mức Độ")]
        public string MIS_PriorityName { get; set; }
        [Display(Name = "Mô tả")]
        public string MIS_Description { get; set; }
        [Display(Name = "Người giao việc")]
        public string MIS_CreateByName { get; set; }
        [Display(Name = "Theo dõi/giám sát")]
        public string MIS_ReporterName { get; set; }
        [Display(Name = "Phân công cho")]
        public string MIS_AssigneeName { get; set; }
        [Display(Name = "SĐT NV được phân công")]
        public string MIS_AssigneePhone { get; set; }
        [Display(Name = "Ngày giao việc")]
        public DateTime? MIS_StartDate { get; set; }
        [Display(Name = "Ngày đến hạn")]
        public DateTime? MIS_EstimateEndDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? MIS_EndDate { get; set; }
        #endregion

        #region THKH
        [Display(Name = "Phân loại chuyến thăm")]
        public string THKH_VisitTypeName { get; set; }
        #endregion

        #region Bảo hành MLC
        public string TICKET_MLC_STT { get; set; }
        public string TICKET_MLC_WorkFlowName { get; set; }
        public string TICKET_MLC_TaskStatusName { get; set; }
        public string TICKET_MLC_Summary { get; set; }
        public string TICKET_MLC_VisitAddress { get; set; }
        public string TICKET_MLC_ServiceTechnicalTeam { get; set; }
        public string TICKET_MLC_ReporterName { get; set; }
        public string TICKET_MLC_AssigneeName { get; set; }
        public string TICKET_MLC_RoleInCharge { get; set; }
        public int? TICKET_MLC_ProfileCode { get {
                return ProfileCode;
            } }
        public string TICKET_MLC_ProfileForeignCode { get; set; }
        public string TICKET_MLC_ProfileName { get; set; }
        public string TICKET_MLC_ProfileAddress { get; set; }
        public string TICKET_MLC_Phone { get; set; }
        public string TICKET_MLC_ProvinceName { get; set; }
        public string TICKET_MLC_DistrictName { get; set; }
        public string TICKET_MLC_WardName { get; set; }
        public string TICKET_MLC_Email { get; set; }
        public string TICKET_MLC_PersonInCharge { get; set; }
        public string TICKET_MLC_ERPProductCode { get; set; }
        public string TICKET_MLC_ProductName { get; set; }
        public string TICKET_MLC_Description { get; set; }
        public string TICKET_MLC_CustomerReviews { get; set; }
        public string TICKET_MLC_Review { get; set; }
        public DateTime? TICKET_MLC_ReceiveDate { get; set; }
        public DateTime? TICKET_MLC_StartDate { get; set; }
        public DateTime? TICKET_MLC_EndDate { get; set; }
        public string TICKET_MLC_CreateByName { get; set; }
        public DateTime? TICKET_MLC_CreateTime { get; set; }
        public string TICKET_MLC_LastEditByName { get; set; }
        public DateTime? TICKET_MLC_LastEditTime { get; set; }
        public int? TICKET_MLC_Qty { get; set; }
        public string TICKET_MLC_SerialNumber { get; set; }
        public string TICKET_MLC_Unit { get; set; }
        public string TICKET_MLC_ProductCategoryName { get; set; }
        public string TICKET_MLC_ErrorName { get; set; }
        public string TICKET_MLC_ERPAccessoryCode { get; set; }
        public string TICKET_MLC_AccessoryName { get; set; }
        public string TICKET_MLC_ErrorTypeName { get; set; }
        public int? TICKET_MLC_AccessoryQty { get; set; }
        public string TICKET_MLC_AccessoryCategoryName { get; set; }
        public string TICKET_MLC_SurveyCreateByName { get; set; }
        public DateTime? TICKET_MLC_SurveyCreateByTime { get; set; }
        public string TICKET_MLC_SurveyProductQuality { get; set; }
        public string TICKET_MLC_SurveyEmployeeProfessional { get; set; }
        public string TICKET_MLC_SurveyServiceBehaviorEmployees { get; set; }
        public string TICKET_MLC_SurveyCustomerComments { get; set; }
        #endregion
    }
}