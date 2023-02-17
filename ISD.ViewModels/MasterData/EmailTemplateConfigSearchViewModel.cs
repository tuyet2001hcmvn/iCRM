using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class EmailTemplateConfigSearchViewModel
    {
        public string EmailTemplateType { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string EmailFrom { get; set; }
        public string EmailSender { get; set; }
        public string EmailHost { get; set; }
        public Nullable<int> EmailPort { get; set; }
        public Nullable<bool> EmailEnableSsl { get; set; }
        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public Nullable<bool> Actived { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
    }
}
