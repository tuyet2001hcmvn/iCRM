using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Extensions
{
    public enum PivotGridExportFormats { ExcelDataAware, Pdf, Excel, Mht, Rtf, Text, Html }
    public class PivotGridExportOptions
    {
        public PivotGridExportOptions()
        {
            WYSIWYG = new PivotGridExportWYSIWYGOptions();
            DataAware = new PivotGridDataAwareExportOptions();
        }
        public PivotGridExportFormats ExportType { get; set; }
        public PivotGridExportWYSIWYGOptions WYSIWYG { get; set; }
        public PivotGridDataAwareExportOptions DataAware { get; set; }
    }

    public class PivotGridExportWYSIWYGOptions
    {
        public PivotGridExportWYSIWYGOptions()
        {
            PrintFilterHeaders = true;
            PrintColumnHeaders = true;
            PrintRowHeaders = true;
            PrintDataHeaders = true;
        }

        public bool PrintColumnAreaOnEveryPage { get; set; }
        public bool PrintRowAreaOnEveryPage { get; set; }
        public bool PrintFilterHeaders { get; set; }
        public bool PrintColumnHeaders { get; set; }
        public bool PrintRowHeaders { get; set; }
        public bool PrintDataHeaders { get; set; }
    }

    public class PivotGridDataAwareExportOptions
    {
        public PivotGridDataAwareExportOptions()
        {
            AllowGrouping = true;
            AllowFixedColumnAndRowArea = true;
        }

        public bool AllowGrouping { get; set; }
        public bool AllowFixedColumnAndRowArea { get; set; }
        public bool ExportDisplayText { get; set; }
        public bool ExportRawData { get; set; }
    }
}
