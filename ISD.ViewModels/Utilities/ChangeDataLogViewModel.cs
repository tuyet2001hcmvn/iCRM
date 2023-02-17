using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ChangeDataLogViewModel
    {
        public System.Guid LogId { get; set; }
        public string TableName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PrimaryKey")]
        public Nullable<System.Guid> PrimaryKey { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FieldName")]
        public string FieldName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OldData")]
        public string OldData { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NewData")]
        public string NewData { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEdit_User")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEdit_Time")]
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEdit_User")]
        public string LastEditUser { get; set; }
    }
}
