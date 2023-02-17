using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ISD.Extensions
{
    public class TastesPivotGridDataOutputHelper
    {
        private static PivotGridSettings dataAwarePivotGridSettings;

        public static PivotGridSettings DataAwarePivotGridSettings
        {
            get
            {
                if (dataAwarePivotGridSettings == null)
                    dataAwarePivotGridSettings = CreateDataAwarePivotGridSettings();
                return dataAwarePivotGridSettings;
            }
        }

        /// <summary>
        /// Action Export
        /// </summary>
        /// <param name="options">PivotGrid ExportOptions</param>
        /// <param name="data">Data model</param>
        /// <returns>File</returns>
        public static ActionResult GetExportActionResult(PivotGridExportOptions options, object data)
        {
            var _pivotGridHelper = new PivotGridDataOutputHelper();
            _pivotGridHelper.ExportPivotGridSettings = CreateDataAwarePivotGridSettings();

            return _pivotGridHelper.GetExportActionResult(options, data);
        }

        /// <summary>
        /// Config field for pivot => export
        /// </summary>
        /// <returns></returns>
        /// 
       /*
        private static PivotGridSettings CreatePivotGridSettings()
        {
            PivotGridSettings settings = CreatePivotGridSettingsBase();
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
       */
        /// <summary>
        /// Setting config Field for pivot
        /// </summary>
        /// <returns></returns>
        private static PivotGridSettings CreateDataAwarePivotGridSettings()
        {
            PivotGridSettings settings = CreatePivotGridSettingsBase();
            settings.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.LegacyOptimized;
            //settings.OptionsFilter.FilterPanelMode = FilterPanelMode.Filter;

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
                field.Area = PivotArea.FilterArea;
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

            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.ColumnArea;
            //    field.AreaIndex = 0;
            //    field.Caption = "Year";
            //    field.FieldName = "OrderDate";
            //    field.GroupInterval = PivotGroupInterval.DateYear;
            //});
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.ColumnArea;
            //    field.AreaIndex = 1;
            //    field.Caption = "Quarter";
            //    field.FieldName = "OrderDate";
            //    field.GroupInterval = PivotGroupInterval.DateQuarter;
            //});
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.DataArea;
            //    field.AreaIndex = 0;
            //    field.Caption = "Product Amount";
            //    field.FieldName = "ProductAmount";
            //    field.CellFormat.FormatString = "c";
            //    field.CellFormat.FormatType = FormatType.Custom;
            //});

            return settings;
        }

        /// <summary>
        /// Setting base for pivot
        /// </summary>
        /// <returns>PivotGridSettings</returns>
        private static PivotGridSettings CreatePivotGridSettingsBase()
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = "BAO_CAO_THI_HIEU_KHACH_HANG";//Pivot name, File export name
            settings.CallbackRouteValues = new { Controller = "Tastes", Action = "PivotGridPartial" };
            settings.Width = Unit.Percentage(100);
            settings.OptionsPager.RowsPerPage = 50;
            settings.OptionsPager.PageSizeItemSettings.Visible = true;
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
            return settings;
        }
    }
}