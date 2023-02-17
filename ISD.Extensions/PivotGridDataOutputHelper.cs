using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using ISD.ViewModels;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ISD.Resources;
using OfficeOpenXml.Style;

namespace ISD.Extensions
{
    public class PivotGridDataOutputHelper
    {
        public PivotGridSettings ExportPivotGridSettings { get; set; }

        public PivotGridSettings DataAwarePivotGridSettings { get; set; }

        public ActionResult GetExportActionResult(PivotGridExportOptions options, object data, List<SearchDisplayModel> searchInfoList = null, string HeaderName = null, ePaperSize? PagerSize = null, int? Scale = null, eOrientation? Orientation = null)
        {
            if (exportTypes == null)
                exportTypes = CreateExportTypes();
            PivotGridSettings settings = GetPivotGridExportSettings(options.WYSIWYG);
            //XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx();
            //exportOptions.ExportType = ExportType.WYSIWYG;
            PivotGridExportFormats format = options.ExportType;

            MemoryStream streamContentResult = new MemoryStream();

            #region Get devexpress export filestream
            //if (format == PivotGridExportFormats.Excel || format == PivotGridExportFormats.ExcelDataAware)
            //{
            //    PivotGridSettings settings = GetPivotGridExportSettings(options.WYSIWYG);
            //    XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx();
            //    exportOptions.ExportType = ExportType.WYSIWYG;

            //    FileStreamResult devexStreamResult = (FileStreamResult)exportTypes[format].ExcelMethod(settings, data, exportOptions);
            //    var stream = devexStreamResult.FileStream;

            //    #region use Epplus insert search value
            //    using (var package = new ExcelPackage(stream))
            //    {
            //        var workSheet = package.Workbook.Worksheets[1];
            //        //insert empty cell
            //        workSheet.InsertRow(1, searchInfoList.Count + 1);
            //        // insert value
            //        foreach (var i in searchInfoList)
            //        {
            //            string searchInfoString = i.DisplayName + ": " + i.DisplayValue;
            //            var cell = workSheet.Cells[searchInfoList.IndexOf(i) + 1, 1];
            //            cell.Value = searchInfoString;
            //        }
            //        streamContentResult = new MemoryStream(package.GetAsByteArray());
            //    }
            //    #endregion

            //    return new FileContentResult(streamContentResult.ToArray(), devexStreamResult.ContentType)
            //    {
            //        FileDownloadName = devexStreamResult.FileDownloadName
            //    };
            //}
            switch (format)
            {
                case PivotGridExportFormats.Excel:
                    XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx();
                    exportOptions.ExportType = ExportType.WYSIWYG;
                    FileStreamResult devexStreamResult = (FileStreamResult)exportTypes[format].ExcelMethod(settings, data, exportOptions);
                    var stream = devexStreamResult.FileStream;

                    #region use Epplus insert search value
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets[1];                     
                        #region PrinterSettings
                        if (PagerSize != null)
                        {
                            workSheet.PrinterSettings.PaperSize = PagerSize.Value;

                        }
                        if (Scale != null)
                        {
                            workSheet.PrinterSettings.Scale = Scale.Value;

                        }
                        if (Orientation != null)
                        {
                            workSheet.PrinterSettings.Orientation = Orientation.Value;
                        }
                        workSheet.PrinterSettings.FitToPage = true;
                        workSheet.PrinterSettings.FitToWidth = 1;
                        workSheet.PrinterSettings.FitToHeight = 0;
                        workSheet.PrinterSettings.LeftMargin = (decimal)0.2;
                        workSheet.PrinterSettings.RightMargin = (decimal)0.2;
                        workSheet.PrinterSettings.BottomMargin = (decimal)0.2;
                        workSheet.PrinterSettings.TopMargin = (decimal)0.2;
                        workSheet.PrinterSettings.HeaderMargin = (decimal)0.2;
                        workSheet.PrinterSettings.FooterMargin = (decimal)0.2;
                        workSheet.PrinterSettings.HorizontalCentered = true;
                        #endregion

                        //insert empty cell
                        #region Header
                        workSheet.InsertRow(1, 1);
                        if (!string.IsNullOrEmpty(HeaderName))
                        {
                            workSheet.InsertRow(2, 1);
                            var headercell = workSheet.Cells[2, 1];
                            headercell.Value = HeaderName.ToUpper();
                            headercell.Style.Font.Size = 18;
                            headercell.Style.Font.Bold = true;
                        }
                        #endregion

                        #region  insert value
                        int index = 0;
                        foreach (var i in searchInfoList)
                        {
                            int rowIndex = 3 + index;
                            workSheet.InsertRow(rowIndex, 1);
                            string searchInfoString = i.DisplayName + ": " + i.DisplayValue;
                            var cell = workSheet.Cells[rowIndex, 1];
                            cell.Value = searchInfoString;
                            index++;
                        }

                        #endregion

                        streamContentResult = new MemoryStream(package.GetAsByteArray());
                    }
                    #endregion

                    return new FileContentResult(streamContentResult.ToArray(), devexStreamResult.ContentType)
                    {
                        FileDownloadName = devexStreamResult.FileDownloadName
                    };

                case PivotGridExportFormats.ExcelDataAware:
                    XlsxExportOptionsEx exportOptions2 = new XlsxExportOptionsEx() { ExportType = ExportType.DataAware };
                    exportOptions2.AllowFixedColumnHeaderPanel = exportOptions2.AllowFixedColumns = options.DataAware.AllowFixedColumnAndRowArea ? DefaultBoolean.True : DefaultBoolean.False;
                    exportOptions2.AllowGrouping = options.DataAware.AllowGrouping ? DefaultBoolean.True : DefaultBoolean.False;
                    exportOptions2.RawDataMode = options.DataAware.ExportRawData;
                    exportOptions2.TextExportMode = options.DataAware.ExportDisplayText ? TextExportMode.Text : TextExportMode.Value;
                    return exportTypes[format].ExcelMethod(DataAwarePivotGridSettings, data, exportOptions2);
                case PivotGridExportFormats.Pdf:
                    settings.SettingsExport.OptionsPrint.PageSettings.Landscape = true;
                    settings.SettingsExport.OptionsPrint.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    settings.SettingsExport.OptionsPrint.PrintFilterHeaders = ConvertToDefaultBoolean(false);
                    settings.SettingsExport.OptionsPrint.PrintDataHeaders = ConvertToDefaultBoolean(false);


                    PrintingSystem ps = new PrintingSystem();
                    PrintableComponentLink link = new PrintableComponentLink(ps);
                    link.Landscape = true;
                    link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    link.Margins.Left = 20;
                    link.Margins.Right = 20;
                    link.Margins.Bottom = 20;
                    link.Margins.Top = 60;
                    if (!string.IsNullOrEmpty(HeaderName))
                    {
                        PageHeaderArea headerArea;
                        PageHeaderFooter header;
                        headerArea = new PageHeaderArea();

                        headerArea.Content.Add(HeaderName.ToUpper());
                        headerArea.Font = new Font("Tahoma", 18, FontStyle.Bold);
                        //headerArea.LineAlignment = BrickAlignment.Center;
                        header = new PageHeaderFooter(headerArea, null);

                        link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                        link.PageHeaderFooter = header;
                    }

                    var exporter = PivotGridExtension.CreatePrintableObject(settings, data);

                    link.Component = exporter;
                    link.CreateDocument();
                    ps.Document.AutoFitToPagesWidth = 1;
                    FileStreamResult result = CreateExcelPdfResult(link);
                    ps.Dispose();
                    return result;
                //return exportTypes[format].Method(settings, data);
                default:
                    return exportTypes[format].Method(settings, data);
            }

            #endregion
        }

        private void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            //PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.DateTime, "", Color.DarkBlue,
            //   new RectangleF(0, 0, 100, 20), BorderSide.None);
            PageInfoBrick brick = new PageInfoBrick();
            brick.LineAlignment = BrickAlignment.Center;
            brick.Alignment = BrickAlignment.Center;
            brick.AutoWidth = true;
        }

        FileStreamResult CreateExcelPdfResult(DevExpress.XtraPrinting.Links.IWinLink link)
        {
            MemoryStream stream = new MemoryStream();
            link.PrintingSystem.ExportToPdf(stream);
            stream.Position = 0;
            //return FileContentResult(stream, "application/pdf", "pivot.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }

        public ActionResult GetExportActionResult(PivotGridExportOptions options, object data)
        {
            if (exportTypes == null)
                exportTypes = CreateExportTypes();

            PivotGridExportFormats format = options.ExportType;
            PivotGridSettings settings = GetPivotGridExportSettings(options.WYSIWYG);
            switch (format)
            {
                case PivotGridExportFormats.Excel:
                    XlsxExportOptionsEx exportOptions1 = new XlsxExportOptionsEx();
                    exportOptions1.ExportType = ExportType.WYSIWYG;
                    return exportTypes[format].ExcelMethod(settings, data, exportOptions1);

                case PivotGridExportFormats.ExcelDataAware:
                    XlsxExportOptionsEx exportOptions2 = new XlsxExportOptionsEx() { ExportType = ExportType.DataAware };
                    exportOptions2.AllowFixedColumnHeaderPanel = exportOptions2.AllowFixedColumns = options.DataAware.AllowFixedColumnAndRowArea ? DefaultBoolean.True : DefaultBoolean.False;
                    exportOptions2.AllowGrouping = options.DataAware.AllowGrouping ? DefaultBoolean.True : DefaultBoolean.False;
                    exportOptions2.RawDataMode = options.DataAware.ExportRawData;
                    exportOptions2.TextExportMode = options.DataAware.ExportDisplayText ? TextExportMode.Text : TextExportMode.Value;
                    return exportTypes[format].ExcelMethod(DataAwarePivotGridSettings, data, exportOptions2);
                case PivotGridExportFormats.Pdf:
                    settings.SettingsExport.OptionsPrint.PageSettings.Landscape = true;
                    settings.SettingsExport.OptionsPrint.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    settings.SettingsExport.OptionsPrint.PrintFilterHeaders = ConvertToDefaultBoolean(false);
                    settings.SettingsExport.OptionsPrint.PrintDataHeaders = ConvertToDefaultBoolean(false);


                    PrintingSystem ps = new PrintingSystem();
                    PrintableComponentLink link = new PrintableComponentLink(ps);
                    link.Landscape = true;
                    link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    link.Margins.Left = 20;
                    link.Margins.Right = 20;
                    link.Margins.Bottom = 20;
                    link.Margins.Top = 40;

                    var exporter = PivotGridExtension.CreatePrintableObject(settings, data);

                    link.Component = exporter;
                    link.CreateDocument();
                    ps.Document.AutoFitToPagesWidth = 1;
                    FileStreamResult result = CreateExcelPdfResult(link);
                    ps.Dispose();
                    return result;
                //return exportTypes[format].Method(settings, data);
                default:
                    return exportTypes[format].Method(settings, data);
            }
        }

        public PivotGridSettings GetPivotGridExportSettings(PivotGridExportWYSIWYGOptions options)
        {
            PivotGridSettings exportSettings = ExportPivotGridSettings;
            exportSettings.SettingsExport.OptionsPrint.PrintFilterHeaders = ConvertToDefaultBoolean(options.PrintFilterHeaders);
            exportSettings.SettingsExport.OptionsPrint.PrintColumnHeaders = ConvertToDefaultBoolean(options.PrintColumnHeaders);
            exportSettings.SettingsExport.OptionsPrint.PrintRowHeaders = ConvertToDefaultBoolean(options.PrintRowHeaders);
            exportSettings.SettingsExport.OptionsPrint.PrintDataHeaders = ConvertToDefaultBoolean(options.PrintDataHeaders);
            return exportSettings;
        }

        public PivotGridSettings GetPivotGridExportSettings(PivotGridDataAwareExportOptions options)
        {
            PivotGridSettings exportSettings = ExportPivotGridSettings;
            return exportSettings;
        }

        private Dictionary<PivotGridExportFormats, PivotGridExportType> exportTypes;

        private Dictionary<PivotGridExportFormats, PivotGridExportType> CreateExportTypes()
        {
            Dictionary<PivotGridExportFormats, PivotGridExportType> types = new Dictionary<PivotGridExportFormats, PivotGridExportType>();
            types.Add(PivotGridExportFormats.Pdf, new PivotGridExportType { Title = "Export to PDF", Method = PivotGridExtension.ExportToPdf });
            types.Add(PivotGridExportFormats.Excel, new PivotGridExportType { Title = "Export to XLSX", ExcelMethod = PivotGridExtension.ExportToXlsx });
            types.Add(PivotGridExportFormats.ExcelDataAware, new PivotGridExportType { Title = "Export to XLSX", ExcelMethod = PivotGridExtension.ExportToXlsx });
            types.Add(PivotGridExportFormats.Mht, new PivotGridExportType { Title = "Export to MHT", Method = PivotGridExtension.ExportToMht });
            types.Add(PivotGridExportFormats.Rtf, new PivotGridExportType { Title = "Export to RTF", Method = PivotGridExtension.ExportToRtf });
            types.Add(PivotGridExportFormats.Text, new PivotGridExportType { Title = "Export to TEXT", Method = PivotGridExtension.ExportToText });
            types.Add(PivotGridExportFormats.Html, new PivotGridExportType { Title = "Export to HTML", Method = PivotGridExtension.ExportToHtml });
            return types;
        }

        private static DefaultBoolean ConvertToDefaultBoolean(bool value)
        {
            return value ? DefaultBoolean.True : DefaultBoolean.False;
        }

        public delegate ActionResult PivotGridExportMethod(PivotGridSettings settings, object dataObject);

        public delegate ActionResult PivotGridDataAwareExportMethod(PivotGridSettings settings, object dataObject, XlsxExportOptions exportOptions);

        public class PivotGridExportType
        {
            public string Title { get; set; }
            public PivotGridExportMethod Method { get; set; }
            public PivotGridDataAwareExportMethod ExcelMethod { get; set; }
        }
    }
}