using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class LogChangeViewModel
    {
        public Guid? CreateBy { get; set; }
        public string CreateName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public string LastEditName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? LastEditTime { get; set; }

        public Guid? ExtendCreateBy { get; set; }
        public string ExtendCreateName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? ExtendCreateTime { get; set; }
    }
}
