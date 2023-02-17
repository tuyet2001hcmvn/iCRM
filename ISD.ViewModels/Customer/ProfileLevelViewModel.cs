using ISD.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ProfileLevelViewModel : ProfileLevelModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyName { get; set; }
    }
}