using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class MemberOfTargetGroupInCampaignViewModel
    {
        public Guid? SendMailId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CampainId")]
        public Guid? CampaignId { get; set; }
        public string CampainName { get; set; }
        [Display(Name = "Loại")]
        public string Type { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public Guid? ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Member_ProfileCode")]
        public int? ProfileCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileForeignCode")]
        public string ProfileForeignCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfileName")]
        public string ProfileName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProfilePhone")]
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsSend")] 
        public bool? IsSend { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SendTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? SendTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsBounce")]
        public bool? IsBounce { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsOpened")]
        public bool? IsOpened { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastOpenedTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? LastOpenedTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isConfirm")]
        public bool? isConfirm { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ConfirmTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ConfirmTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "isCheckin")]
        public bool? isCheckin { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CheckInTime")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? CheckinTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NumberOfParticipant")]
        public int? NumberOfParticipant { get; set; }
    }
}
