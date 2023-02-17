using ISD.EntityModels;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class EmailTemplateConfigViewModel : EmailTemplateConfigModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Type")]
        public string EmailTypeName { get; set; }
    }
}
