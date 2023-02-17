using System.Collections.Generic;

namespace ISD.ViewModels.Extension
{
    public class ExcelSheetTemplate
    {
        //public List<T> data { get; set; }
        public List<ExcelTemplate> ColumnsToTake { get; set; }
        public List<ExcelHeadingTemplate> Heading { get; set; }
        public bool showSlno { get; set; }
    }
}
