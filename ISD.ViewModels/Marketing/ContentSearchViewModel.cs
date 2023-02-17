using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Marketing
{
    public class ContentSearchViewModel
    {
       
        public Guid Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentCode")]
        public int ContentCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentName")]
        public string ContentName { get; set; }
        //public Guid FromEmailAccountId { get; set; }
        //public string SenderName { get; set; }
        //public string SaleOrg { get; set; }
        //public string Subject { get; set; }
        //public string Content { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        //public Guid? CreateBy { get; set; }
        //public DateTime? CreateTime { get; set; }
        //public Guid? LastEditBy { get; set; }
        //public DateTime? LastEditTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }

        public string Type { get; set; }
    }
}
