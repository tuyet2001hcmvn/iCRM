using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class EmailViewModel
    {
        public string Subject { get; set; }
        public string EmailContent { get; set; }
        //public string EmailContent { get; set; }
        public IFormFileCollection Attachments { get; set; }
    }
}
