using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public static class ConstController
    {
        //Quyền: Thêm, Sửa, Xóa, Import, Export,..
        public const string Function = "Function";
        public const string Menu = "Menu";
        public const string Module = "Module";
        public const string Page = "Page";
        public const string MobileScreen = "MobileScreen";
        public const string Access = "Access";

        //Nhóm người dùng, Người dùng
        public const string Roles = "Roles";
        public const string Account = "Account";
        public const string Auth = "Auth";
        public const string Home = "Home";

        #region Dữ liệu nền
        //Danh muc dung chung
        public const string Catalog = "Catalog";
        //Phòng ban
        public const string Department = "Department";
        //Công ty
        public const string Company = "Company";
        //Chi nhánh
        public const string Store = "Store";
        //Kho
        public const string Stock = "Stock";
        //Nhóm mục tiêu 
        public const string TargetGroup = "TargetGroup";
        //Nhóm mục tiêu quản bá sản phẩm
        public const string TargetGroupProductPromotion = "TargetGroupProductPromotion";
        //Nhóm mục tiêu  gửi mẫu quà tết
        public const string TemplateAndGiftTargetGroup = "TemplateAndGiftTargetGroup";
        //Quản lý nội dung 
        public const string Content = "Content";
        //Quản lý Chiến dịch 
        public const string Campaign = "Campaign";
        //Quản lý chiến dịch sản phẩm
        public const string ProductPromotion = "ProductPromotion";
        //Quản lý Chiến dịch gửi mẫu 
        public const string TemplateAndGiftCampaign = "TemplateAndGiftCampaign";
        public const string Unfollow = "Unfollow";
        public const string Question = "Question";
        public const string QuestionBank = "QuestionBank";
        public const string FavoriteReport = "FavoriteReport";
        //1. Nhãn hiệu
        public const string ProfitCenter = "ProfitCenter";
        //2. Loại xe
        public const string ProductHierarchy = "ProductHierarchy";
        //3. Dòng xe
        public const string MaterialGroup = "MaterialGroup";
        //4. Phiên bản
        public const string Labor = "Labor";
        //5. Màu sắc
        public const string MaterialFreightGroup = "MaterialFreightGroup";
        //6. Đời xe
        public const string ExternalMaterialGroup = "ExternalMaterialGroup";
        //7. Kiểu xe
        public const string TemperatureCondition = "TemperatureCondition";
        //8. Option
        public const string ContainerRequirement = "ContainerRequirement";
        //9. Sản phẩm
        public const string Material = "Material";
        // Nhóm được phân công
        public const string AssignedGroup = "AssignedGroup";

        //Tỉnh thành
        public const string Province = "Province";
        //Quận huyện
        public const string District = "District";
        //Phường xã
        public const string Ward = "Ward";
        //Đánh giá nhân viên
        public const string EmployeeRatings = "EmployeeRatings";

        //Nhân viên
        public const string SalesEmployee = "SalesEmployee";

        //Nhóm phụ tùng/PK
        public const string AccessoryCategory = "AccessoryCategory";
        //Nhóm công việc
        public const string ServiceType = "ServiceType";
        //Phụ tùng/Phụ kiện/Công việc
        public const string Accessory = "Accessory";
        //Nghề nghiệp
        public const string Career = "Career";
        //Loại danh mục
        public const string CatalogType = "CatalogType";
        //Kanban
        public const string Kanban = "Kanban";
        //Bảng tin
        public const string News = "News";
        //Bảng tin
        public const string NewsCategory = "NewsCategory";
        //Cấu hình nhiệm vụ
        public const string WorkFlowConfig = "WorkFlowConfig";
        //Cấu hình thuộc tính
        public const string WorkFlowField = "WorkFlowField";

        //SMSLog
        public const string SMSLog = "SMSLog";
        public const string RegisterReceiveNews = "RegisterReceiveNews";
        public const string EmailTemplateConfig = "EmailTemplateConfig";

        // Face check in 
        public const string FaceCheckInOut = "FaceCheckInOut";
        #endregion

        #region Khách hàng
        public const string Customer = "Customer";
        //KH tiềm năng
        public const string Prospect = "Prospect";

        public const string Profile = "Profile";

        public const string AddressBook = "AddressBook";

        public const string CertificateAC = "CertificateAC";

        public const string Spons = "Spons";

        public const string PersonInCharge = "PersonInCharge";

        public const string RoleInCharge = "RoleInCharge";

        public const string Partner = "Partner";

        public const string FileAttachment = "FileAttachment";

        public const string ProfileContact = "ProfileContact";

        public const string ProfileGroup = "ProfileGroup";

        public const string ProfileLevel = "ProfileLevel";

        public const string Revenue = "Revenue";

        public const string Catalogue = "Catalogue"; //Catalogue đã xuất

        public const string CustomerTaste = "CustomerTaste"; //Tab Thị hiếu khách hàng

        public const string Tastes = "Tastes"; //Báo cáo Thị hiếu khách hàng

        public const string ProfileConfig = "ProfileConfig"; //Loại khách hàng: Account/Contact/Lead/Opportunity

        public const string CustomerSaleOrder = "CustomerSaleOrder"; //Danh sách đơn hàng theo khách hàng

        public const string Activities = "Activities"; //Hoạt động dự án

        public const string Competitor = "Competitor"; //Đối thủ
        #endregion

        #region Bán hàng
        //Bán xe
        public const string SaleOrder = "SaleOrder";
        //bán lẻ phụ tùng
        public const string AccessorySaleOrder = "AccessorySaleOrder";
        #endregion

        #region Quản lý hồ sơ trước bạ
        //Tiếp nhận hồ sơ
        public const string ConfirmRecord = "ConfirmRecord";
        //Chuyển hồ sơ cho chi nhánh
        public const string SendRecordToStore = "SendRecordToStore";
        //Chi nhánh xác nhận
        public const string StoreConfirmReceived = "StoreConfirmReceived";
        //Chi nhánh chuyển hồ sơ cho KH
        public const string SendRecordToCustomer = "SendRecordToCustomer";
        #endregion

        #region Dịch vụ
        //Loại đơn hàng DV
        public const string ServiceOrderType = "ServiceOrderType";
        //Lịch hẹn sửa chữa
        public const string Working = "Working";
        //Đơn hàng dịch vụ
        public const string ServiceOrder = "ServiceOrder";
        //Thông tin xe
        public const string VehicleInfo = "VehicleInfo";
        //Loại sửa chữa
        public const string FixingType = "FixingType";
        //Loại bán phụ tùng/phụ kiện
        public const string AccessorySellType = "AccessorySellType";
        //Flag dịch vụ
        public const string ServiceFlag = "ServiceFlag";
        //Danh sách phụ tùng cần gửi yêu cầu lên hãng
        public const string ClaimAccessory = "ClaimAccessory";
        #endregion

        #region Kế toán
        //Thu tiền
        public const string ReceiptVoucher = "ReceiptVoucher";
        //Báo cáo thu chi theo ngày (kỳ)
        public const string DailyAccountancyReport = "DailyAccountancyReport";
        #endregion Kế toán

        #region Cài đặt
        public const string Setting = "Setting";
        //reset data
        public const string ResetTestData = "ResetTestData";
        //Đồng bộ
        public const string MasterData = "MasterData";
        #endregion

        #region Tiến Thu: không dùng
        public const string StoreType = "StoreType";
        public const string CustomerPromotion = "CustomerPromotion";
        public const string CustomerLevel = "CustomerLevel";
        public const string CustomerGift = "CustomerGift";
        public const string Brand = "Brand";
        public const string Category = "Category";
        public const string Configuration = "Configuration";
        public const string Style = "Style";
        public const string Color = "Color";
        public const string Specifications = "Specifications";
        public const string Warehouse = "Warehouse";
        public const string Product = "Product";
        public const string PlateFee = "PlateFee";
        public const string PeriodicallyChecking = "PeriodicallyChecking";
        #endregion

        #region Bảo hành

        public const string Warranty = "Warranty";
        public const string ProductWarranty = "ProductWarranty";

        #endregion

        #region Công việc
        public const string WorkFlow = "WorkFlow";
        public const string Task = "Task";
        public const string Appointment = "Appointment";
        #endregion

        #region Kho
        public const string StockReceiving = "StockReceiving";
        public const string StockTransfer = "StockTransfer";
        public const string StockDelivery = "StockDelivery";
        public const string BeginningInventory = "BeginningInventory";
        public const string Inventory = "Inventory";
        public const string StockTransferRequest = "StockTransferRequest";
        #endregion

        #region Báo cáo
        //Báo cáo chi tiết phân cấp khách hàng
        public const string CustomerHierarchyDetailReport = "CustomerHierarchyDetailReport";
        //KH theo NV kinh doanh
        public const string ProfileWithPersonInChargeReport = "ProfileWithPersonInChargeReport";
        //Số lượng hoạt động theo NV kinh doanh
        public const string CustomerCareActivities = "CustomerCareActivities";
        //Tổng hợp KH + LH
        public const string ProfileReport = "ProfileReport";
        //KH theo nhóm KH
        public const string ProfileGroupReport = "ProfileGroupReport";
        //SL khách ghé thăm showroom
        public const string ProfileQuantityAppointmentWithShowRoomReport = "ProfileQuantityAppointmentWithShowRoomReport";
        //BÁO CÁO TỶ LỆ NHÓM KHÁCH HÀNG ĐẾN SHOWROOM
        public const string RateOfAppointmentWithShowRoomReport = "RateOfAppointmentWithShowRoomReport";
        //BÁO CÁO TỔNG HỢP XUẤT NHẬP TỒN CATALOGUE
        public const string TotalRDCatalogueReport = "TotalRDCatalogueReport";      
        //BÁO CÁO NHÓM GIAO VIỆC
        public const string TaskGroupReport = "TaskGroupReport";
        //BÁO CÁO SỐ LƯỢNG KHÁCH GHÉ THĂM THEO NHÂN VIÊN
        public const string AppointmentWithPersonInChargeReport = "AppointmentWithPersonInChargeReport";
        //BÁO CÁO SỐ LƯỢNG KHÁCH HÀNG THEO CRM/ECC
        public const string CustomerAmountByCRM = "CustomerAmountByCRM";
        //Hoạt động CSKH theo NV kinh doanh
        public const string ActivitiesBySalesEmployeeReport = "ActivitiesBySalesEmployeeReport";
        //Báo cáo tổng hợp thị hiếu khách hàng
        public const string CustomerTastesReport = "CustomerTastesReport";
        //Thống kê số lượt LIKED và lượt VIEW SP
        public const string StatisticLikeViewProduct = "StatisticLikeViewProduct";
        //Báo cáo tổng hợp khách ghé thăm
        public const string CustomerAppointment = "CustomerAppointment";
        //Báo cáo nhu cầu khách hàng đến showroom
        public const string CustomerDemandReport = "CustomerDemandReport";
        //BÁO CÁO DANH SÁCH YÊU CẦU CỦA KHÁCH HÀNG
        public const string CustomerRequirement = "CustomerRequirement";
        //Các báo cáo liên quan công việc
        public const string TaskReport = "TaskReport";
        //Báo cáo tồn kho catalogue
        public const string StockOnHandReport = "StockOnHandReport";
        //Báo cáo tổng hợp lỗi thường gặp trong xử lý khiếu nại
        public const string TicketUsualErrorReport = "TicketUsualErrorReport";
        //Báo cáo số lượng catalogue đã xuất
        public const string StockDeliveryReport = "StockDeliveryReport";
        //Báo cáo đánh giá của khách hàng
        public const string CustomerReviewsReport = "BaoCaoDanhGiaCuaKhachHang";
        //BÁO CÁO GIAO DỊCH CHUYỂN KHO CATALOGUE
        public const string StockTransferReport = "StockTransferReport";
        //BÁO CÁO GIAO DỊCH NHẬP KHO CATALOGUE
        public const string StockReceivingReport = "StockReceivingReport";
        //Báo cáo tổng hợp điểm trưng bày
        public const string ShowroomReport = "ShowroomReport";
        //BÁO CÁO CHI TIẾT VẬT TƯ ĐÃ XUẤT CHO GVL
        public const string MaterialExportedReport = "MaterialExportedReport";
        //BÁO CÁO CHI TIẾT XUẤT VẬT TƯ CHO ĐIỂM TRƯNG BÀY
        public const string MaterialExportedDetailReport = "MaterialExportedDetailReport";
        //Catalogue tồn kho
        public const string StockOnHand = "StockOnHand";
        //Catalogue số lượng phân bổ thực tế so với kế hoạch
        public const string StockAllocation = "StockAllocationReport"; 
        //tỷ lệ lịch bảo hành của NVKT
        public const string TaskWarrantyNVKTReport = "TaskWarrantyNVKTReport";
        //Phân tích bảo hành DVKT
        public const string TaskAnalysisDVKTReport = "TaskAnalysisDVKTReport";
        //Dữ liệu khách hàng
        public const string CustomerDataReport = "CustomerDataReport";
        //Báo có số lượng GVL theo thời gian
        public const string TaskGTBQuantityReport = "TaskGTBQuantityReport";

        //Báo cáo MLC
        //Tổng hợp sản phẩm - phụ kiện bảo hành
        public const string ProductAccessoryReport = "ProductAccessoryReport";
        //Sản phẩm bảo hành
        public const string TaskProductWarrantyReport = "TaskProductWarrantyReport";
        //Báo cáo bảo hành sản phẩm
        public const string ProductWarrantyReport = "ProductWarrantyReport";
        //Phụ  kiện bảo hành
        public const string TaskAccessoryWarrantyReport = "TaskAccessoryWarrantyReport";
        //Kết lịch hằng ngày DVKT
        public const string TaskTicketMLCReport = "TaskTicketMLCReport";
        //Tổng hợp ý kiến khách hàng
        public const string TaskCustomerReviewReport = "TaskCustomerReviewReport";

        //Danh sách dự án
        public const string ProfileOpportunityReport = "ProfileOpportunityReport";
        //Tổng hợp thông tin dự án
        public const string OpportunityReport = "TongHopThongTinDuAn";
        //Tổng Lịch DVKT
        public const string TechnicalServicesTicketMLCReport = "TechnicalServicesTicketMLCReport";
        //Tổng hợp thông tin khách hàng theo chiến dịch
        public const string SendMailReport = "SendMailReport";
        // Báo cáo quản trị
        public const string ManagementReport = "ManagementReport";
        // Báo cáo số lượng KH theo Phân cấp doanh số
        public const string NumberOfCustomersBySalesReport = "NumberOfCustomersBySalesReport";
        // Báo cáo SL Khách hàng theo Tăng trưởng doanh số
        public const string NumberOfCustomerBySaleGrowthReport = "NumberOfCustomerBySaleGrowthReport";
        //  Báo cáo Khách hàng có giấy chứng nhận đối tác An Cường
        public const string CustomerCertificateACReport = "CustomerCertificateACReport";
        //  Báo cáo chi phí sử dụng catalogue so với phê duyệt
        public const string CostOfUsingCatalogueComparedToApprovalReport = "CostOfUsingCatalogueComparedToApprovalReport";
        // Báo cáo tổng hợp chiến dịch quản bá sản phẩm
        public const string ProductPromotionReport = "ProductPromotionReport";
        // Báo cáo chi tiết chiến dịch quản bá sản phẩm
        public const string ProductPromotionDetailReport = "ProductPromotionDetailReport";
        // Báo cáo chi tiết xuất nhập tồn catalogue
        public const string RDCatalogueDetailReport = "RDCatalogueDetailReport";
        #endregion Báo cáo

        #region Dự án
        //Chủ đầu tư
        public const string Investor = "Investor";
        //Tư vấn - thiết kế
        public const string ConsultingDesign = "ConsultingDesign";
        //Tổng thầu
        public const string GeneralContractor = "GeneralContractor";
        //Thi công
        public const string Construction = "Construction";
        //Đơn vị tư vấn giám sát
        public const string ConsultingUnit = "ConsultingUnit";
        #endregion
    }

    public static class ConstArea
    {
        public const string Permission = "Permission";
        public const string MasterData = "MasterData";
        public const string Reports = "Reports";
        public const string Sale = "Sale";
        public const string Service = "Service";
        public const string Utilities = "Utilities";
        public const string TransferDataFromSAP = "TransferDataFromSAP";
        public const string Customer = "Customer";
        public const string Accountancy = "Accountancy";
        public const string Maintenance = "Maintenance";
        public const string Work = "Work";
        public const string Warehouse = "Warehouse";
        public const string Marketing = "Marketing";
    }

    public static class ConstAction
    {
        public const string Login = "Login";
    }
}


