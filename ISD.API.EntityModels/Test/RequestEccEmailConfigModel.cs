using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class RequestEccEmailConfigModel
    {
        public Guid Id { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string EmailContent { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailPassword { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
    }
}
