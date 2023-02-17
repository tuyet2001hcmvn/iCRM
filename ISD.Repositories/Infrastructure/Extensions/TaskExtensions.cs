using ISD.EntityModels;
using ISD.ViewModels;
using System;

namespace ISD.Repositories.Infrastructure.Extensions
{
    public static class TaskExtensions
    {
        public static void MapAppointment(this AppointmentModel appointmentModel, AppointmentViewModel viewModel)
        {
            appointmentModel.AppointmentId = viewModel.AppointmentId;
            //2. Liên hệ chính
            appointmentModel.PrimaryContactId = viewModel.PrimaryContactId;
            //3. Phân loại Khách hàng
            appointmentModel.CustomerClassCode = viewModel.CustomerClassCode;
            //4. Danh mục
            appointmentModel.CategoryCode = viewModel.CategoryCode;
            //5. Địa điểm khách ghé
            appointmentModel.ShowroomCode = viewModel.ShowroomCode;
            //6. Người tiếp khách
            appointmentModel.SaleEmployeeCode = viewModel.SaleEmployeeCode;
            //7. Khách biết đến ACN qua
            appointmentModel.ChannelCode = viewModel.ChannelCode;
            //8. Ngày ghé thăm
            appointmentModel.VisitDate = viewModel.VisitDate;
            //Yêu cầu
            appointmentModel.Requirement = viewModel.Requirement;
            //Đề xuất
            appointmentModel.SaleEmployeeOffer = viewModel.SaleEmployeeOffer;
            //Có ghé thăm Cabinet
            appointmentModel.isVisitCabinetPro = viewModel.isVisitCabinetPro;
            if (appointmentModel.isVisitCabinetPro == true)
            {
                appointmentModel.VisitCabinetProRequest = viewModel.VisitCabinetProRequest;
            }
            else
            {
                appointmentModel.VisitCabinetProRequest = null;
            }
        }

        public static void MapTaskFromAppointmentVM(this TaskModel taskModel, AppointmentViewModel viewModel)
        {
            taskModel.TaskId = viewModel.TaskId;
            //2. Chủ đề
            taskModel.Summary = viewModel.Summary;
            //3. Khách hàng
            taskModel.ProfileId = viewModel.ProfileId;
            //4. Công ty
            taskModel.CompanyId = viewModel.CompanyId;
            //5. Chi nhánh
            taskModel.StoreId = viewModel.StoreId;
            //6. Thời gian bắt đầu
            taskModel.StartDate = viewModel.StartDate;
            //7. Thời gian kết thúc
            taskModel.EndDate = viewModel.EndDate;
            //8. Mức độ
            taskModel.PriorityCode = viewModel.PriorityCode;
            //9. Nhiệm vụ
            taskModel.WorkFlowId = viewModel.WorkFlowId;
            //10. Ghi chú
            taskModel.Description = viewModel.Description;
            //11. Trạng thái
            taskModel.TaskStatusId = viewModel.TaskStatusId;
            //ReceiveDate
            taskModel.ReceiveDate = viewModel.ReceiveDate;
            
        }

        public static void MapWorkFlow(this WorkFlowModel workFlow, WorkFlowViewModel viewModel)
        {
            //1 GUID
            workFlow.WorkFlowId = viewModel.WorkFlowId;
            //2 Tên nghiệp vụ
            workFlow.WorkFlowName = viewModel.WorkFlowName;
            //3 Hình ảnh
            workFlow.ImageUrl = viewModel.ImageUrl;
            //4 Thứ tự
            workFlow.OrderIndex = viewModel.OrderIndex;
            //4 Mã nghiệp vụ
            workFlow.WorkFlowCode = viewModel.WorkFlowCode;
            //5 Loại
            workFlow.WorkflowCategoryCode = viewModel.WorkflowCategoryCode;
            //5 Người tạo
            // workFlow.CreateBy = viewModel.CreateBy;
            //6  Thời gian tạo
            // workFlow.CreateTime = viewModel.CreateTime;
            //7  Người sửa cuối
            //  workFlow.LastEditBy = viewModel.LastEditBy;
            //8 Thời gian cập nhật cuối cùng
            // workFlow.LastEditTime = viewModel.LastEditTime;
            //9 Trạng thái
            workFlow.Actived = viewModel.Actived;
            //Công ty
            workFlow.CompanyCode = viewModel.CompanyCode;
            //Ẩn Summary
            workFlow.IsDisabledSummary = viewModel.IsDisabledSummary;
        }

        public static void MapTaskStatus(this TaskStatusModel taskStatusModel, TaskStatusViewModel viewModel)
        {
            //1 GUID
            //taskStatusModel.TaskStatusId = (Guid)viewModel.TaskStatusId;
            //
            taskStatusModel.TaskStatusCode = viewModel.TaskStatusCode;
            //2 Trạng thái
            taskStatusModel.TaskStatusName = viewModel.TaskStatusName;
            //3 Quy trình
            taskStatusModel.WorkFlowId = viewModel.WorkFlowId;
            // Giai doan
            taskStatusModel.ProcessCode = viewModel.ProcessCode;
            //4 Thứ tự
            taskStatusModel.OrderIndex = viewModel.OrderIndex;
            //5 Người tạo
            // taskStatusModel.CreateBy = viewModel.CreateBy;
            //6 Thời gian tạo
            // taskStatusModel.CreateTime = viewModel.CreateTime;
            //7 Người sửa cuối
            // taskStatusModel.LastEditBy = viewModel.LastEditBy;
            //8 Thời gian cập nhật cuối cùng
            // taskStatusModel.LastEditTime = viewModel.LastEditTime;
            //9 Trạng thái
            taskStatusModel.Actived = viewModel.Actived;
            //Danh mục
            taskStatusModel.Category = viewModel.Category;
        }

        public static void MapTask(this TaskModel taskModel, TaskViewModel taskViewModel)
        {
            //GUID
             taskModel.TaskId = taskViewModel.TaskId;
            //Mã yêu cầu
            taskModel.TaskCode = taskViewModel.TaskCode;
            //Yêu cầu
            taskModel.Summary = taskViewModel.Summary;
            //Khách hàng
            taskModel.ProfileId = taskViewModel.ProfileId;
            //Địa chỉ
            taskModel.ProfileAddress = taskViewModel.ProfileAddress;
            //Mô tả
            taskModel.Description = taskViewModel.Description;
            //Mức độ
            taskModel.PriorityCode = taskViewModel.PriorityCode;
            //Nhiệm vụ
            taskModel.WorkFlowId = taskViewModel.WorkFlowId;
            //Trạng thái
            taskModel.TaskStatusId = taskViewModel.TaskStatusId;
            //Ngày tiếp nhận
            taskModel.ReceiveDate = taskViewModel.ReceiveDate;
            //Ngày bắt đầu
            taskModel.StartDate = taskViewModel.StartDate;
            //Ngày kết thúc dự kiến
            taskModel.EstimateEndDate = taskViewModel.EstimateEndDate;
            //Ngày kết thúc thực tế
            taskModel.EndDate = taskViewModel.EndDate;
            //Công ty
            taskModel.CompanyId = taskViewModel.CompanyId;
            //Chi nhánh
            taskModel.StoreId = taskViewModel.StoreId;
            //Đính kèm
            taskModel.FileUrl = taskViewModel.FileUrl;
            //Lỗi hay gặp(Cong, Mốc, Trầy,…)
            taskModel.CommonMistakeCode = taskViewModel.CommonMistakeCode;
            //Người tạo
            //taskModel.CreateBy = taskViewModel.CreateBy;
            //Thời gian tạo
            //taskModel.CreateTime = taskViewModel.CreateTime;
            //Người sửa cuối
            // taskModel.LastEditBy = taskViewModel.LastEditBy;
            //Thời gian cập nhật cuối cùng
            // taskModel.LastEditTime = taskViewModel.LastEditTime;
            //Trạng thái
            taskModel.Actived = taskViewModel.Actived;
            //Phiếu đăng ký bảo hành
            taskModel.ProductWarrantyId = taskViewModel.ProductWarrantyId;
            //Người giao việc
            taskModel.Reporter = taskViewModel.Reporter;
            //Đơn vị thi công
            taskModel.ConstructionUnit = taskViewModel.ConstructionUnit;
            //Liên hệ của đơn vị thi công
            taskModel.ConstructionUnitContact = taskViewModel.ConstructionUnitContact;
            //Nhóm công ty
            taskModel.ServiceTechnicalTeamCode = taskViewModel.ServiceTechnicalTeamCode;
            //Ý kiến khách hàng
            taskModel.CustomerReviews = taskViewModel.CustomerReviews;
            //Mã loại lỗi
            taskModel.ErrorTypeCode = taskViewModel.ErrorTypeCode;
            //Mã lỗi
            taskModel.ErrorCode = taskViewModel.ErrorCode;
            //Địa điểm ghé thăm
            taskModel.VisitAddress = taskViewModel.VisitAddress;
            //lat
            taskModel.lat = taskViewModel.lat;
            //lng
            taskModel.lng = taskViewModel.lng;
            //Yêu cầu checkin
            taskModel.isRequiredCheckin = taskViewModel.isRequiredCheckin;
            //Phân loại chuyến thăm
            taskModel.VisitTypeCode = taskViewModel.VisitTypeCode;
            //Riêng tư
            taskModel.isPrivate = taskViewModel.isPrivate;
            //Nhắc nhở
            taskModel.isRemind = taskViewModel.isRemind;
            if (taskModel.isRemind == true)
            {
                taskModel.RemindTime = taskViewModel.RemindTime;
                taskModel.RemindCycle = taskViewModel.RemindCycle;
                taskModel.isRemindForReporter = taskViewModel.isRemindForReporter;
                taskModel.isRemindForAssignee = taskViewModel.isRemindForAssignee;
                taskModel.RemindStartDate = taskViewModel.RemindStartDate;
            }
            //Bảo hành
            taskModel.Property1 = taskViewModel.Property1;
            taskModel.Property2 = taskViewModel.Property2;
            taskModel.Property3 = taskViewModel.Property3;
            taskModel.Property4 = taskViewModel.Property4;
            taskModel.Property5 = taskViewModel.Property5;
            //Subtask
            taskModel.ParentTaskId = taskViewModel.ParentTaskId;
            //NV kinh doanh
            taskModel.SalesSupervisorCode = taskViewModel.SalesSupervisorCode;
            //Ngày
            taskModel.Date1 = taskViewModel.Date1;
            taskModel.Date2 = taskViewModel.Date2;
            taskModel.Date3 = taskViewModel.Date3;
            taskModel.Date4 = taskViewModel.Date4;
            taskModel.Date5 = taskViewModel.Date5;
            //Text
            taskModel.Text1 = taskViewModel.Text1;
            taskModel.Text2 = taskViewModel.Text2;
            taskModel.Text3 = taskViewModel.Text3;
            taskModel.Text4 = taskViewModel.Text4;
            taskModel.Text5 = taskViewModel.Text5;
            //Phân công cho nhóm
            taskModel.IsAssignGroup = taskViewModel.IsAssignGroup;
            //Khu vực
            taskModel.VisitSaleOfficeCode = taskViewModel.VisitSaleOfficeCode;
            //Tỉnh/thành phố
            taskModel.ProvinceId = taskViewModel.ProvinceId;
            // Quận/ huyện
            taskModel.DistrictId = taskViewModel.DistrictId;
            // Phường/ Xã
            taskModel.WardId = taskViewModel.WardId;
            //ĐTB
            taskModel.Property6 = taskViewModel.Property6;
            //Nơi tham quan
            taskModel.VisitPlace = taskViewModel.VisitPlace;
            //Loại công việc: True: Làm chung | False: Làm riêng
            taskModel.IsTogether = taskViewModel.IsTogether;
        }
    }
}