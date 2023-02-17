using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class RateOfAppointmentWithShowRoomReportViewModel
    {
        [Display(Name = "STT")]
        public int OrderNumber { get; set; }

        [Display(Name = "Chi nhánh")]
        public string StoreName { get; set; }

        [Display(Name = "Nguồn KH")]
        public string ShowroomName { get; set; }

        [Display(Name = "Tổng khách ghé thăm")]
        public decimal? ProfileCount { get; set; }

        [Display(Name = "Tỷ lệ nhóm KH tiêu dùng")]
        public string PrecentConsumptionGroup {
            get
            {
                string ret = "0 %";
                if (ProfileCount > 0)
                {
                    ret = String.Format("{0:n1} %", (ConsumptionGroup * 100) / ProfileCount);
                }
                return ret;
            }
        }
        public decimal? ConsumptionGroup { get; set; }

        [Display(Name = "Tỷ lệ nhóm khách Thiết kế -Thi công")]
        public string PrecentConstructionGroup {
            get
            {
                string ret = "0 %";
                if (ProfileCount > 0)
                {
                    ret = String.Format("{0:n1} %", (DesignConstructionGroup * 100) / ProfileCount);
                }
                return ret;
            }
        }
        public decimal? DesignConstructionGroup { get; set; }

        [Display(Name = "Tỷ lệ nhóm khách thiết kế")]
        public string PrecentDesignGroup {
            get
            {
                string ret = "0 %";
                if (ProfileCount > 0)
                {
                    ret = String.Format("{0:n1} %", (DesignGroup * 100) / ProfileCount);
                }
                return ret;
            }
        }
        public decimal? DesignGroup { get; set; }

        [Display(Name = "Tỷ lệ các nhóm khác")]
        public string PrecentOtherGroup {
            get
            {
                string ret = "0 %";
                if (ProfileCount > 0)
                {
                    ret = String.Format("{0:n1} %", (OtherGroup * 100) / ProfileCount);
                }
                return ret;
            }
        }
        public decimal? OtherGroup { get; set; }

    }
    public class RateOfAppointmentWithShowRoomReportSearchViewModel
    {

        //Showroom
        [Display(Name = "Chi nhánh")]
        public List<string> CreateAtSaleOrg { get; set; }
        //Ngày ghé thăm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }
        [Display(Name = "Công ty")]
        public string CompanyCode { get; set; }

        [Display(Name = "Nhân Viên")]
        public List<string> SalesEmployeeCode { get; set; }

        [Display(Name = "Phòng ban")]
        public List<string> RolesCode { get; set; }
    }
}
