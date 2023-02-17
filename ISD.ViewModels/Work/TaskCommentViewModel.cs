using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskCommentViewModel
    {
        public Guid? TaskCommentId { get; set; }
        public System.Guid? TaskId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        public string ProcessUser { get; set; }

        public Guid? FromStatusId { get; set; }
        public Guid? ToStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public string LastEditByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        public Nullable<System.DateTime> LastEditTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_FileUrl")]
        public FileAttachmentViewModel CommentFileUrl { get; set; }

        public string LogoName { get; set; }
        public string CreateTimeMobile
        {
            get
            {
                if (CreateTime.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", CreateTime.Value);
                }
                return string.Empty;
            }
        }
    }
}