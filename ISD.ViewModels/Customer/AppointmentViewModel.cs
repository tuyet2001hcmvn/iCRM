using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class AppointmentViewModel
    {
        //columns.Add(new ExcelTemplate { ColumnName = "TaskCode", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "Summary", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "Address", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "SaleEmployeeName", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "StartDate", isAllowedToEdit = false, isDateTime= true });
        //    columns.Add(new ExcelTemplate { ColumnName = "EndDate", isAllowedToEdit = false, isDateTime = true });
        //    columns.Add(new ExcelTemplate { ColumnName = "Description", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "ShowroomCode", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false });
        //    columns.Add(new ExcelTemplate { ColumnName = "VisitDate", isAllowedToEdit = false, isDateTime = true });

        public int? STT { get; set; }
        public System.Guid AppointmentId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Code")]
        public int TaskCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Summary")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public int ProfileCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Phone")]
        public string Phone { get; set; }
        public string Email { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerTypeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerTypeCode")]
        public string CustomerTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_TaxNo")]
        public string TaxNo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_SaleEmployeeCode")]
        public string SaleEmployeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_ReceiveDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ReceiveDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_StartDate")]
        public Nullable<System.DateTime> StartDate { get; set; }
        public string StartDateWithFormat
        {
            get
            {
                if (StartDate.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy}", StartDate.Value);
                }
                return string.Empty;
            }
        }
      
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_EndDate")]
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EndDateWithFormat
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy}", EndDate.Value);
                }
                return string.Empty;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_EstimateEndDate")]
        public Nullable<System.DateTime> EstimateEndDate { get; set; }


        [AllowHtml]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ShowroomCode")]
        public string ShowroomCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ShowroomCode")]

        public string ShowroomName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PrimaryContact")]
        public Nullable<System.Guid> PrimaryContactId { get; set; }

        public string PrimaryContactName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CustomerClass")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CustomerClassCode { get; set; }

        public string CustomerClassName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Category")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public System.Guid StoreId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]

        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_SaleEmployeeCode")]
        public string SaleEmployeeCode { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_ChannelCode")]
        public string ChannelCode { get; set; }

        public string ChannelName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Appointment_VisitDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> VisitDate { get; set; }
        public string VisitDateWithFormat
        {
            get
            {
                if (VisitDate.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy}", VisitDate.Value);
                }
                return string.Empty;
            }
        }

        //Task Model
        public System.Guid TaskId { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<System.Guid> ProfileId { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_CompanyId")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_PriorityCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PriorityCode { get; set; }

        public string PriorityName { get; set; }

        public System.Guid WorkFlowId { get; set; }
        public string WorkFlowCode { get; set; }
        public string WorkFlowName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaskStatus")]
        public Guid TaskStatusId { get; set; }

        public string TaskStatusName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Address")]
        public string ProfileAddress { get; set; }

        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }

        public List<TaskContactViewModel> taskContactList { get; set; }
        public TaskContactViewModel taskContact { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public string LastEditByName { get; set; }

        public string ProcessCode { get; set; }

        public List<TaskCommentViewModel> taskCommentList { get; set; }
        public int? NumberOfComments { get; set; }
        public List<FileAttachmentViewModel> taskFileList { get; set; }
        public int? NumberOfFiles { get; set; }

        public string Reporter { get; set; }

        public List<TaskAssignViewModel> taskAssignList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCategoryCode")]
        public string CustomerGroupName { get; set; }
        public string CustomerGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerTaste")]
        public string customerTasteLst { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Catalogue")]
        public string customerCatalogueLst { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
        public string CustomerCareerCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerCareerCode")]
        public string CustomerCareerName { get; set; }

        public string ProductCode { get; set; }
        public List<string> productList { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Requirement")]
        public string Requirement { get; set; }

        //Ý kiến/ phản hồi của khách hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Reviews")]
        public string Reviews { get; set; }

        //Rating
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_Ratings")]
        public string Ratings { get; set; }

        //Đề xuất của nhân viên tiếp khách
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleEmployeeOffer")]
        public string SaleEmployeeOffer { get; set; }

        //Có ghé thăm cabinet pro
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isVisitCabinetPro")]
        public bool? isVisitCabinetPro { get; set; }

        //Nhu cầu KH khi ghé thăm cabinet pro
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VisitCabinetProRequest")]
        public string VisitCabinetProRequest { get; set; }

        //Mobile additional field
        public bool? isSentSMS { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string CreateByFullName { get; set; }
        public string LastEditByFullName { get; set; }

        //Survey
        public List<SurveyViewModel> surveyList { get; set; }
    }
}