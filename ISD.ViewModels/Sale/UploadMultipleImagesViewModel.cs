using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class UploadMultipleImagesViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UploadType")]
        public bool? UploadType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl", Description = "Upload_ImageUrl_Hint")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public List<string> imageList { get; set; }
    }
}
