using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ExportExcelInputModel
    {
        public List<ProfileExportExcelModel> Data { get; set; }
        public List<ExcelTemplate> ColumnsToTake { get; set; }
        public List<ExcelHeadingTemplate> Heading { get; set; }
        public bool showSlno { get; set; }
        public bool? HasExtraSheet { get; set; }
        public bool? IsMergeCellHeader { get; set; }
        public int headerRowMergeCount { get; set; }
    }
}
