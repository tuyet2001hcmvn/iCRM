using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using ISD.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ISD.Extensions
{
    public class PivotGridExportExcel 
    {
        //public static PivotGridSettings DataAwarePivotGridSettings(T searchModel)
        //{
        //    if (dataAwarePivotGridSettings == null)
        //        dataAwarePivotGridSettings = CreateDataAwarePivotGridSettings(searchModel);
        //    return dataAwarePivotGridSettings;

        //}
        /// <summary>
        /// Action Export
        /// </summary>
        /// <param name="options">PivotGrid ExportOptions</param>
        /// <param name="data">Data model</param>
        /// <returns>File</returns>
        public static ActionResult GetExportActionResult(string fileName,PivotGridExportOptions options ,List<FieldSettingModel> listSetting, object data, string HeaderName = null, string layoutConfigs = null, ePaperSize? PagerSize = null, int? Scale = null, eOrientation? Orientation = null)
        {
            var _pivotGridHelper = new PivotGridDataOutputHelper();
            _pivotGridHelper.ExportPivotGridSettings = CreatePivotGridSettings(fileName,listSetting, layoutConfigs);

            return _pivotGridHelper.GetExportActionResult(options, data, HeaderName: HeaderName, PagerSize: PagerSize,Scale: Scale,Orientation: Orientation);
        }
       
        public static ActionResult GetExportActionResult(string fileName, PivotGridExportOptions options, List<FieldSettingModel> listSetting, object data, List<SearchDisplayModel>searchInfoList, string HeaderName = null, string layoutConfigs = null, ePaperSize? PagerSize = null, int? Scale = null, eOrientation? Orientation = null)
        {
            var _pivotGridHelper = new PivotGridDataOutputHelper();

            _pivotGridHelper.ExportPivotGridSettings = CreatePivotGridSettings(fileName, listSetting, layoutConfigs);   
            return _pivotGridHelper.GetExportActionResult(options, data, searchInfoList, HeaderName, PagerSize: PagerSize, Scale: Scale, Orientation: Orientation);
        }
        private static int MeasureTextHeight(string text, Font font, double width)
        {
            if (string.IsNullOrEmpty(text)) return 1;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 7);  //7 pixels per excel column width
            var fontSize = font.Size * 1.01f;
            var drawingFont = new Font(font.Name, fontSize);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth, new StringFormat { FormatFlags = StringFormatFlags.MeasureTrailingSpaces });
            //72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            var heightpx = Math.Min(size.Height * 98 / 96, 409);
            var height = Math.Min(Convert.ToInt32(Math.Round((heightpx / 15),0)), 5);
            return height > 0 ? height : 1;
        }
        /// <summary>
        /// Setting config Field for pivot
        /// </summary>
        /// <returns></returns>
        private static PivotGridSettings CreatePivotGridSettings(string fileName, List<FieldSettingModel> listSetting, string layoutConfigs = null)
        {
            PivotGridSettings settings = CreatePivotGridSettingsBase(fileName, layoutConfigs);
            //Lấy setting theo LayoutConfigs
            
            foreach (var fieldSetting in listSetting)
            {
                settings.Fields.Add(field =>
                {
                    if (fieldSetting.PivotArea == 0)
                    {
                        field.Area = PivotArea.RowArea;
                    }
                    if (fieldSetting.PivotArea == 1)
                    {
                        field.Area = PivotArea.ColumnArea;
                    }
                    if (fieldSetting.PivotArea == 2)
                    {
                        field.Area = PivotArea.FilterArea;
                    }
                    if (fieldSetting.PivotArea == 3)
                    {
                        field.Area = PivotArea.DataArea;
                    }
                    field.FieldName = fieldSetting.FieldName;
                    field.Caption = fieldSetting.Caption;
                    field.AreaIndex = fieldSetting.AreaIndex.Value;
                    field.Visible = fieldSetting.Visible.Value;
                    if (fieldSetting.Width != null && fieldSetting.Width.Value > 0)
                    {
                        field.ExportBestFit = false;

                        field.Width = fieldSetting.Width.Value;
                    }
                    else
                    {
                        field.ExportBestFit = true;
                    }
                    if (fieldSetting.CellFormat_FormatType == "DateTime")
                    {
                        field.ValueFormat.FormatType = FormatType.DateTime;
                        field.ValueFormat.FormatString = fieldSetting.CellFormat_FormatString;
                        field.CellFormat.FormatType = FormatType.DateTime;
                        field.CellFormat.FormatString = fieldSetting.CellFormat_FormatString;
                        field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (fieldSetting.CellFormat_FormatType == "Numeric")
                    {
                        field.ValueFormat.FormatType = FormatType.Numeric;
                        field.ValueFormat.FormatString = fieldSetting.CellFormat_FormatString;
                        field.CellFormat.FormatType = FormatType.Numeric;
                        field.CellFormat.FormatString = fieldSetting.CellFormat_FormatString;
                        field.ValueStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    }
                    //field.ExportBestFit = true;
                });
            }

            return settings;
        }

        /// <summary>
        /// Setting base for pivot
        /// </summary>
        /// <returns>PivotGridSettings</returns>
        private static PivotGridSettings CreatePivotGridSettingsBase(string fileName, string layoutConfigs = null)
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = fileName;//Pivot name, File export name
            settings.Width = Unit.Percentage(100);
            settings.OptionsPager.RowsPerPage = 20;
            settings.OptionsPager.PageSizeItemSettings.Visible = true;
            settings.OptionsCustomization.AllowDrag = true;
            settings.OptionsCustomization.AllowDragInCustomizationForm = true;
            settings.OptionsCustomization.AllowSort = true;
            settings.OptionsView.ShowRowTotals = true;// Show total theo row
            if (layoutConfigs != null)
            {
                settings.SettingsExport.BeforeExport = (sender, e) =>
                {
                    ((MVCxPivotGrid)sender).LoadLayoutFromString(layoutConfigs);
                };
            }

            //custom  row header
            settings.SettingsExport.CustomExportHeader = (sender, e) =>
            {
                e.Appearance.BackColor = ColorTranslator.FromHtml("#00a65a");
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                if (e.Field != null)
                {
                    e.Field.ExportBestFit = false;
                    e.Appearance.WordWrap = true;
                }
                else
                {
                    e.Field.ExportBestFit = true;
                }

            };
            //custom colunm header
            settings.SettingsExport.CustomExportFieldValue = (sender, e) =>
            {
                if(e.IsColumn)
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#00a65a");
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    if (e.Field != null)
                    {
                        e.Field.ExportBestFit = false;
                        e.Appearance.WordWrap = true;
                    }
                }
                else
                {
                    if (e.Field != null)
                    {
                        //if (e.Value != null && e.Value.ToString().Length > 15)
                        //{
                            e.Field.ExportBestFit = false;
                            //e.Field.RowValueLineCount = MeasureTextHeight(e.Value.ToString(), e.Appearance.Font, e.Field.Width / 7);
                            e.Appearance.WordWrap = true;
                        //}
                    }
                }
            };
            //custom grand total row value
            settings.SettingsExport.CustomExportCell = (sender, e) =>
            {
                if (e.RowValue.ValueType == PivotGridValueType.GrandTotal)
                {
                    e.Appearance.BackColor = Color.Azure;
                    if (e.DataField != null)
                    {
                        e.DataField.ExportBestFit = false;
                        e.Appearance.WordWrap = true;
                    }
                }
                if (e.RowValue.ValueType == PivotGridValueType.Total)
                {
                    e.Appearance.BackColor = Color.Beige;
                    if (e.DataField != null)
                    {
                        e.DataField.ExportBestFit = false;
                        e.Appearance.WordWrap = true;
                    }
                }
            };

            return settings;
        }
        public static List<FieldSettingModel> GetCurrentSetting(ASPxPivotGrid PivotGrid)
        {
            List<FieldSettingModel> fieldSettingList = new List<FieldSettingModel>();
            var listField = PivotGrid.Fields;
            foreach (PivotGridField field in listField)
            {
                FieldSettingModel fieldSetting = new FieldSettingModel();
                fieldSetting.Caption = field.Caption;
                fieldSetting.AreaIndex = field.AreaIndex;
                fieldSetting.FieldName = field.FieldName;
                if (field.Area == PivotArea.RowArea)
                {
                    fieldSetting.PivotArea = 0;

                }
                else
                {
                    if (field.Area == PivotArea.ColumnArea)
                    {
                        fieldSetting.PivotArea = 1;
                    }
                    else
                    {
                        if (field.Area == PivotArea.FilterArea)
                        {
                            fieldSetting.PivotArea = 2;
                        }
                        else
                        {
                            fieldSetting.PivotArea = 3;
                        }
                    }
                }
                if (field.ValueFormat.FormatType == FormatType.Numeric || field.ValueFormat.FormatType == FormatType.DateTime)
                {
                    fieldSetting.CellFormat_FormatString = field.ValueFormat.FormatString;
                    fieldSetting.CellFormat_FormatType = field.ValueFormat.FormatType.ToString();
                }
                fieldSetting.Visible = field.Visible;
                fieldSettingList.Add(fieldSetting);
            }
            return fieldSettingList;
        }
    }
}
