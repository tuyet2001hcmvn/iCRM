using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warranty.Models
{
    public class TaskTicketViewModel
    {
        public Guid? TaskId { get; set; }
        public int TaskCode { get; set; }
        public string Summary { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? CreateTime { get; set; }
    }
}