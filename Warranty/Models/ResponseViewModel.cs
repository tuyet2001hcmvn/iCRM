using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services.Description;

namespace Warranty.Models
{
    public class ResponseViewModel
    {
        public HttpStatusCode Code { get; set; }
        public bool Success { get; set; }
        public string Data { get; set; }
    }
}