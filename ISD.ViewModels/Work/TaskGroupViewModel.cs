using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskGroupViewModel
    {
        [Required]
        [Display(Name = "Tên nhóm")]
        public string GroupName { get; set; }

        public List<TaskAssignViewModel> accountInGroup { get; set; }

        #region search form
        //Mã nhân viên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string SalesEmployeeCode { get; set; }

        //Tên nhân viên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_FullName")]
        public string SalesEmployeeName { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public Nullable<System.Guid> DepartmentId { get; set; }
        #endregion search form
    }
}
