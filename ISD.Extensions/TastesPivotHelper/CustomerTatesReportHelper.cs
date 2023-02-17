using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ISD.Extensions
{
    public class CustomerTatesReportHelper
    {
        private static PivotGridSettings dataAwarePivotGridSettings;

        public static PivotGridSettings DataAwarePivotGridSettings(CustomerTastesSummaryReportSearchViewModel searchModel)
        {
            if (dataAwarePivotGridSettings == null)
                dataAwarePivotGridSettings = CreateDataAwarePivotGridSettings(searchModel);
            return dataAwarePivotGridSettings;

        }
        /// <summary>
        /// Action Export
        /// </summary>
        /// <param name="options">PivotGrid ExportOptions</param>
        /// <param name="data">Data model</param>
        /// <returns>File</returns>
        public static ActionResult GetExportActionResult(PivotGridExportOptions options, CustomerTastesSummaryReportSearchViewModel searchModel, object data)
        {
            var _pivotGridHelper = new PivotGridDataOutputHelper();
            _pivotGridHelper.ExportPivotGridSettings = CreateDataAwarePivotGridSettings(searchModel);

            return _pivotGridHelper.GetExportActionResult(options, data);
        }
        /// <summary>
        /// Setting config Field for pivot
        /// </summary>
        /// <returns></returns>
        private static PivotGridSettings CreateDataAwarePivotGridSettings(CustomerTastesSummaryReportSearchViewModel searchModel)
        {
            PivotGridSettings settings = CreatePivotGridSettingsBase(searchModel);
            settings.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.LegacyOptimized;
            //settings.OptionsFilter.FilterPanelMode = FilterPanelMode.Filter;

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
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
                field.Area = PivotArea.RowArea;
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

        /// <summary>
        /// Setting base for pivot
        /// </summary>
        /// <returns>PivotGridSettings</returns>
        private static PivotGridSettings CreatePivotGridSettingsBase(CustomerTastesSummaryReportSearchViewModel searchModel)
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = "BAO_CAO_THI_HIEU_KHACH_HANG";//Pivot name, File export name
            settings.CallbackRouteValues = new { Controller = "CustomerTastesReport", Action = "PivotGridPartial", jsonReq = JsonConvert.SerializeObject(searchModel) };
            settings.Width = Unit.Percentage(100);
            settings.OptionsPager.RowsPerPage = 50;
            settings.OptionsPager.PageSizeItemSettings.Visible = true;
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
            return settings;
        }
    }
}
