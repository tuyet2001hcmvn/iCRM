using ISD.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.EntityModels
{
    [MetadataType(typeof(WorkFlowModel.MetaData))]
    public partial class WorkFlowModel
    {
        internal sealed class MetaData
        {
            public System.Guid WorkFlowId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkFlowCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string WorkFlowCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkFlowName")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string WorkFlowName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string WorkflowCategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Image")]
            public string ImageUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

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

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company", Description = "WorkflowCompanyCode_Hint")]
            public string CompanyCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsDisabledSummary", Description = "IsDisabledSummary_Hint")]
            public bool? IsDisabledSummary { get; set; }
        }
    }

    [MetadataType(typeof(TaskStatusModel.MetaData))]
    public partial class TaskStatusModel
    {
        internal sealed class MetaData
        {
            public System.Guid TaskStatusId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatusCode")]
            public string TaskStatusCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string TaskStatusName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus_ProcessCode")]
            public string ProcessCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_WorkFlowId")]
            public Nullable<System.Guid> WorkFlowId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public Nullable<int> OrderIndex { get; set; }

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
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AutoUpdateEndDate")]
            public bool? AutoUpdateEndDate { get; set; }
        }
    }

    [MetadataType(typeof(TaskModel.MetaData))]
    public partial class TaskModel
    {
        internal sealed class MetaData
        {
            public System.Guid TaskId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskCode")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public int TaskCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Summary")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string Summary { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
            public Nullable<System.Guid> ProfileId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_PriorityCode")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string PriorityCode { get; set; }

            //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "WorkFlowName")]
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Type")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid WorkFlowId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
            [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid TaskStatusId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ReceiveDate")]
            public Nullable<System.DateTime> ReceiveDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_StartDate")]
            public Nullable<System.DateTime> StartDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_EstimateEndDate")]
            public Nullable<System.DateTime> EstimateEndDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_EndDate")]
            public Nullable<System.DateTime> EndDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public Guid? CompanyId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public System.Guid StoreId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_FileUrl")]
            public string FileUrl { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_CommonMistakeCode")]
            public string CommonMistakeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
            public Nullable<System.Guid> CreateBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
            public Nullable<System.DateTime> CreateTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
            public Nullable<System.Guid> LastEditBy { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
            public Nullable<System.DateTime> LastEditTime { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
            public Nullable<bool> Actived { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Reporter")]
            //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
            public string Reporter { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConstructionUnit")]
            public Guid? ConstructionUnit { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ServiceTechnicalTeamCode")]
            public string ServiceTechnicalTeamCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerReviews")]
            public string CustomerReviews { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorTypeCode2")]
            public string ErrorTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_ErrorCode2")]
            public string ErrorCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_isRequiredCheckin")]
            public bool? isRequiredCheckin { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_VisitAddress")]
            public string VisitAddress { get; set; }

            public string lat { get; set; }
            public string lng { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_VisitTypeCode")]
            public string VisitTypeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_isPrivate")]
            public bool? isPrivate { get; set; }

            //Remind
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_isRemind")]
            public bool? isRemind { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_RemindTime")]
            public int? RemindTime { get; set; }

            public string RemindCycle { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_isRemindFor")]
            public bool? isRemindForReporter { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_isRemindFor")]
            public bool? isRemindForAssignee { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_RemindStartDate")]
            public DateTime? RemindStartDate { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_ProductType")]
            public string Property1 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_OrderNumber")]
            public string Property2 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warranty_Value")]
            public decimal? Property3 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskSource")]
            public string Property4 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerResult")]
            public string Property5 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaTriDTB")]
            public decimal? Property6 { get; set; }

            public Guid? ParentTaskId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PersonInCharge")]
            public string SalesSupervisorCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConstructionTime")]
            public DateTime? Date1 { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
            public string ProfileAddress { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsAssingee")]
            public bool? IsAssignGroup { get; set; }
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsTogether")]
            public bool? IsTogether { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Area")]
            public string VisitSaleOfficeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DeletedReason")]
            public string CancelReason { get; set; }

            //Nơi tham quan
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_VisitPlace")]
            public string VisitPlace { get; set; }
        }
    }

    [MetadataType(typeof(AppointmentModel.MetaData))]
    public partial class AppointmentModel
    {
        internal sealed class MetaData
        {
            public System.Guid AppointmentId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PrimaryContact")]
            public Nullable<System.Guid> PrimaryContactId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerClass")]
            public string CustomerClassCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category")]
            public string CategoryCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ShowroomCode")]
            public string ShowroomCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_SaleEmployeeCode")]
            public string SaleEmployeeCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_ChannelCode")]
            public string ChannelCode { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_VisitDate")]
            public Nullable<System.DateTime> VisitDate { get; set; }
        }
    }

    [MetadataType(typeof(StatusTransitionModel.MetaData))]
    public partial class StatusTransitionModel
    {
        internal sealed class MetaData
        {
            public System.Guid StatusTransitionId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransitionName")]
            public string TransitionName { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_WorkFlowId")]
            public Nullable<System.Guid> WorkFlowId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DestinationStatusId")]
            public Nullable<System.Guid> FromStatusId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TransitionStatusId")]
            public Nullable<System.Guid> ToStatusId { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isAssigneePermission")]
            public Nullable<bool> isAssigneePermission { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isReporterPermission")]
            public Nullable<bool> isReporterPermission { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ColorName")]
            public string Color { get; set; }
        }
    }

    [MetadataType(typeof(WorkFlowFieldModel.MetaData))]
    public partial class WorkFlowFieldModel
    {
        internal sealed class MetaData
        {
            [Display(ResourceType = typeof(LanguageResource), Name = "FieldCode")]
            [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
            public string FieldCode { get; set; }

            [Display(ResourceType = typeof(LanguageResource), Name = "WorkFlowField_FieldName")]
            [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
            public string FieldName { get; set; }

            [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(LanguageResource), Name = "OrderIndex")]
            [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
            public int OrderIndex { get; set; }
        }
    }

    
    [MetadataType(typeof(MetaData))]
    public partial class StatusTransition_Task_Mapping
    {
        internal sealed class MetaData
        {
            public Guid TaskTransitionLogId { get; set; }

            public Guid? TaskId { get; set; }

            [Display(Name = "Trạng thái hiện tại")]
            public Nullable<System.Guid> FromStatusId { get; set; }

            [Display(Name = "Trạng thái chuyển đến")]
            public Nullable<System.Guid> ToStatusId { get; set; }

            [Display(Name = "Nội dung")]
            public string Note { get; set; }

            [Display( Name = "Người thực hiện")]
            public Guid? ApproveBy { get; set; }

            [Display(Name = "Ngày thực hiện")]
            public DateTime? ApproveTime { get; set; }

            [Display(Name = "Người tạo")]
            public Guid? CreateBy { get; set; }

            [Display(Name = "Ngày tạo")]
            public DateTime? CreateTime { get; set; }
        }
    }
}