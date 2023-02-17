using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ISD.Extensions
{
    public class TastesPivotGridHelper
    {
        static PivotGridSettings _settings;
        public static PivotGridSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = GetSettings();
                }
                return _settings;
            }
        }
        static PivotXlsxExportOptions _xlsOptions;
        public static PivotXlsxExportOptions XlsxOptions
        {
            get
            {
                if (_xlsOptions == null)
                {
                    _xlsOptions = GetXlsxOptions();
                }
                return _xlsOptions;
            }
        }

        static PivotGridSettings GetSettings()
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Visible;
            settings.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.LegacyOptimized;

            settings.Name = "BAO_CAO_THI_HIEU_KHACH_HANG";
            settings.CallbackRouteValues = new { Controller = "CustomerTastesReport", Action = "CustomerTastesPivotGridPartial" };
            settings.Width = Unit.Percentage(100);
            settings.OptionsPager.RowsPerPage = 50;
            settings.OptionsPager.PageSizeItemSettings.Visible = true;

            settings.PreRender = (sender, e) =>
            {
                var pivot = (sender as MVCxPivotGrid);
                pivot.OptionsPager.Position = PagerPosition.TopAndBottom;
                pivot.OptionsPager.ShowDisabledButtons = true;
                pivot.OptionsPager.ShowNumericButtons = true;
                pivot.OptionsPager.ShowSeparators = true;
                pivot.OptionsPager.PageSizeItemSettings.Visible = true;
                pivot.OptionsPager.PageSizeItemSettings.Position = PagerPageSizePosition.Right;
                pivot.OptionsPager.RowsPerPage = 50;
            };

            settings.BeforeGetCallbackResult = (sender, e) =>
            {
                var pivot = (sender as MVCxPivotGrid);
                pivot.OptionsPager.Position = PagerPosition.TopAndBottom;
                pivot.OptionsPager.ShowDisabledButtons = true;
                pivot.OptionsPager.ShowNumericButtons = true;
                pivot.OptionsPager.ShowSeparators = true;
                pivot.OptionsPager.PageSizeItemSettings.Visible = true;
                pivot.OptionsPager.PageSizeItemSettings.Position = PagerPageSizePosition.Right;
            };

            //settings.SettingsExport.OptionsPrint.PrintColumnAreaOnEveryPage = true;
            //settings.SettingsExport.OptionsPrint.PrintRowAreaOnEveryPage = true;
            settings.SettingsExport.OptionsPrint.PageSettings.Landscape = true;

            //settings.SettingsExport.CustomExportCell = (sender, e) =>
            //{

            //    // Determine whether the cell is Grand Total.
            //    if ((e.ColumnField == null) || (e.RowField == null))
            //    {
            //        // Specify the text to display in a cell.
            //        ((DevExpress.XtraPrinting.TextBrick)e.Brick).Text = string.Format(
            //             "=> {0}",
            //             ((DevExpress.XtraPrinting.TextBrick)e.Brick).Text);
            //        // Specify the colors used to paint the cell.
            //        e.Appearance.BackColor = Color.Gray;
            //        e.Appearance.ForeColor = Color.Orange;
            //        return;
            //    }

            //    var pivot = ((MVCxPivotGridExporter)sender).PivotGrid as MVCxPivotGrid;
            //    var lastRowFieldIndex = pivot.Fields.GetVisibleFieldCount(PivotArea.RowArea) - 1;
            //    var lastColumnFieldIndex = pivot.Fields.GetVisibleFieldCount(PivotArea.ColumnArea) - 1;

            //    // Determine whether the cell is an ordinary cell.
            //    if (e.RowField.AreaIndex == lastRowFieldIndex && e.ColumnField.AreaIndex == lastColumnFieldIndex)
            //    {
            //        e.Appearance.ForeColor = Color.Gray;
            //    }
            //    // The cell is a Total cell.
            //    else
            //    {
            //        e.Appearance.BackColor = Color.DarkOliveGreen;
            //        e.Appearance.ForeColor = Color.White;
            //    }
            //};

            //settings.SettingsExport.CustomExportFieldValue = (sender, e) =>
            //{
            //    if (e.Field != null)
            //    {
            //        if (e.Field.FieldName == "ProductName")
            //        {
            //            e.Brick.Url = String.Format("https://www.google.com/search?q={0}", e.Text);
            //            ((DevExpress.XtraPrinting.TextBrick)e.Brick).Target = "_blank";
            //        }
            //        if (e.Field.FieldName == "Country" && e.Text == "USA")
            //        {
            //            DevExpress.XtraPrinting.ImageBrick imBrick = new DevExpress.XtraPrinting.ImageBrick();
            //            imBrick.Image = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/us.png"));
            //            imBrick.SizeMode = DevExpress.XtraPrinting.ImageSizeMode.AutoSize;
            //            e.Brick = imBrick;
            //        }
            //    }
            //};

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.FieldName = "MaSAP";
                field.Caption = "Mã SAP";
                field.AreaIndex = 0;
                field.SortBySummaryInfo.FieldName = "SoLuotLiked";
                field.SortOrder = PivotSortOrder.Descending;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.FieldName = "MaSP";
                field.Caption = "Mã thương mại";
                field.AreaIndex = 1;
                field.SortBySummaryInfo.FieldName = "SoLuotLiked";
                field.SortOrder = PivotSortOrder.Descending;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.FieldName = "TenSP";
                field.Caption = "Tên sản phẩm";
                field.AreaIndex = 2;
                field.SortBySummaryInfo.FieldName = "SoLuotLiked";
                field.SortOrder = PivotSortOrder.Descending;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;
                field.FieldName = "PLoaiVT";
                field.Caption = "Phân loại vật tư";
                field.AreaIndex = 0;
                field.SortBySummaryInfo.FieldName = "SoLuotLiked";
                field.SortOrder = PivotSortOrder.Descending;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.FieldName = "NhomVT";
                field.Caption = "Nhóm sản phẩm";
                field.AreaIndex = 1;
                field.SortBySummaryInfo.FieldName = "SoLuotLiked";
                field.SortOrder = PivotSortOrder.Descending;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.FieldName = "SoLuotLiked";
                field.Caption = "Số lượt liked";
                field.AreaIndex = 0;
            });

            return settings;
        }



        static PivotXlsxExportOptions GetXlsxOptions()
        {
            PivotXlsxExportOptions options = new PivotXlsxExportOptions();
            options.CustomizeCell += Options_CustomizeCell;
            return options;
        }
        static void Options_CustomizeCell(CustomizePivotCellEventArgs e)
        {
            if (e.CellItemInfo != null)
            {
                switch (e.CellItemInfo.ColumnValueType)
                {
                    case PivotGridValueType.GrandTotal:
                        {
                            // Specify the text to display in a cell.
                            e.Value = string.Format("=> {0}", e.Value.ToString());
                            // Specify the colors used to paint the cell.
                            e.Formatting.BackColor = Color.Gray;
                            e.Formatting.Font.Color = Color.Orange;
                            break;
                        }
                    case PivotGridValueType.Total:
                        {
                            e.Formatting.BackColor = Color.DarkOliveGreen;
                            e.Formatting.Font.Color = Color.White;
                            break;
                        }
                    case PivotGridValueType.Value:
                        {
                            e.Formatting.Font.Color = Color.Gray;
                            break;
                        }

                    default:
                        throw new Exception("Unexpected Case");
                }
                e.Handled = true;
            }
        }
    }
}
