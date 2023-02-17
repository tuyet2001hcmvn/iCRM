using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class RolesViewModel
    {
        public Guid RolesId { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public Nullable<bool> Actived { get; set; }

        //Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
