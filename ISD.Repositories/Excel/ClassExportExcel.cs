using ISD.Constant;
using ISD.Resources;
using ISD.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.ModelBinding;

namespace ISD.Repositories.Excel
{
    public class ClassExportExcel
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        #region Convert List<T> to DataTable method
        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        #endregion Convert List<T> to DataTable method
       
        public static byte[] ExportExcel<T>(DataTable dataTable, List<ExcelTemplate> columnsToTake, List<ExcelHeadingTemplate> heading, bool showSrNo = false, bool? HasExtraSheet = true, bool? IsMergeCellHeader = true, int headerRowMergeCount = 0, ePaperSize? PageSize = null, int? Scale = null, eOrientation? Orientation = null, float? headerFontSize = null, float? bodyFontSize = null)
        {
            byte[] result = null;
            bool isEdit = false;
            if (dataTable.Rows.Count > 0)
            {
                isEdit = true;
            }
            using (ExcelPackage package = new ExcelPackage())
            {
                bool isHeadingHasValue = (heading != null && heading.Count > 0);
                string headingMainContent = "";
                int headingTotalRows = 0;
                if (isHeadingHasValue == true)
                {
                    //Set necessary attributes of heading
                    headingMainContent = heading[1].Content;
                    headingTotalRows = heading.Count();
                }

                // Creates a TextInfo based on the "en-US" culture.
                TextInfo headingWithFormat = new CultureInfo("en-US", false).TextInfo;
                //SheetName
                string sheetName = string.Empty;
                if (headingMainContent.Contains("CUSTOMER MASTER DATA"))
                {
                    sheetName = string.Format("Customer_SD({0})", dataTable.Rows.Count);
                }
                else
                {
                    sheetName = String.Format("{0}", headingWithFormat.ToTitleCase(RemoveSign4VietnameseString(headingMainContent.ToLower()))).Replace(" ", "");
                }
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(sheetName);
                if (bodyFontSize != null)
                {
                    workSheet.Cells.Style.Font.Size = bodyFontSize.Value;
                }
                workSheet.PrinterSettings.FitToPage = true;
                workSheet.PrinterSettings.FitToWidth = 1;
                workSheet.PrinterSettings.FitToHeight = 0;
                if (PageSize != null)
                {
                    workSheet.PrinterSettings.PaperSize = PageSize.Value;
                }
                if (Scale != null)
                {
                    workSheet.PrinterSettings.Scale = Scale.Value;
                }
                if (Scale != null)
                {
                    workSheet.PrinterSettings.Orientation = Orientation.Value;
                }
                int startRowFrom = (isHeadingHasValue == true) ? headingTotalRows + 1 : 1;
                int startBodyColumnFrom = 1;
                if (!showSrNo)
                {
                    startBodyColumnFrom = 0;
                }
                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add(string.Format(LanguageResource.Export_ExcelRequired, LanguageResource.NumberIndex), typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    if (isEdit == true)
                    {
                        foreach (DataRow item in dataTable.Rows)
                        {
                            item[0] = index;
                            index++;
                        }
                    }
                    else
                    {
                        dataTable.Rows.Add(index);
                    }
                }


                //removed ignored columns
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Select(p => p.ColumnName).Contains(dataTable.Columns[i].ColumnName))
                    {
                        dataTable.Columns.RemoveAt(i);
                    }
                }

                #region initialize list contain column with format
                List<int> columnIsAllowedToEdit = new List<int>();
                List<int> columnIsDateTime = new List<int>();
                List<int> columnIsDateTimeTime = new List<int>();
                List<int> columnIsCurrency = new List<int>();
                List<int> columnIsNumber = new List<int>();
                List<int> columnIsPercent = new List<int>();
                List<int> columnIsBoolean = new List<int>();
                List<int> columnIsGender = new List<int>();
                List<int> columnIsTotal = new List<int>();
                List<int> columnIsWraptext = new List<int>();
                List<DropdownListModel> columnIsDropdownlist = new List<DropdownListModel>();
                List<int> columnIsDifferentColorHeader = new List<int>();
                List<int> columnHasAnotherHeaderName = new List<int>();
                List<int> columnHasNote = new List<int>();
                List<List<DropdownModel>> dropdownModels = new List<List<DropdownModel>>();
                List<List<DropdownIdTypeIntModel>> dropdownIdTypeIntModels = new List<List<DropdownIdTypeIntModel>>();
                List<List<DropdownIdTypeStringModel>> dropdownIdTypeStringModels = new List<List<DropdownIdTypeStringModel>>();
                List<List<DropdownIdTypeBoolModel>> dropdownIdTypeBoolModels = new List<List<DropdownIdTypeBoolModel>>();
                List<int> columnIsDependentDropdownlist = new List<int>();
                List<ExcelTemplate> dependentDropdownModels = new List<ExcelTemplate>();
                #endregion
                #region Dynamic Column Headers 
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var type = typeof(T);
                    var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                        .OfType<MetadataTypeAttribute>().FirstOrDefault();
                    var metaData = (metadataType != null)
                        ? ModelMetadataProviders.Current.GetMetadataForType(null, metadataType.MetadataClassType)
                        : ModelMetadataProviders.Current.GetMetadataForType(null, type);

                    var property = metaData.ModelType.GetProperty(dataTable.Columns[i].ColumnName);
                    if (property != null)
                    {
                        //DisplayName
                        var display = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        if (display != null)
                        {
                            //dd.GetName(): Get value by DisplayAttribute
                            dataTable.Columns[i].ColumnName = display.GetName();
                        }
                        //Required
                        var required = property.GetCustomAttribute(typeof(RequiredAttribute)) as RequiredAttribute;
                        if (required != null)
                        {
                            dataTable.Columns[i].ColumnName = string.Format(LanguageResource.Export_ExcelRequired, dataTable.Columns[i].ColumnName);
                        }
                        //Get column name if it is allowed to edit => isAllowedToEdit = true
                        var columnName = columnsToTake.Where(p => p.isAllowedToEdit == true).Select(p => p.ColumnName).ToList();
                        if (columnName.Contains(property.Name))
                        {
                            //get type of field
                            //dataType.Add(property.PropertyType.FullName);
                            columnIsAllowedToEdit.Add(i);
                        }
                        //date time 
                        var dateTime = columnsToTake.Where(p => p.isDateTime == true).Select(p => p.ColumnName).ToList();
                        if (dateTime != null && dateTime.Contains(property.Name))
                        {
                            columnIsDateTime.Add(i);
                        }
                        var wraptext = columnsToTake.Where(p => p.isWraptext == true).Select(p => p.ColumnName).ToList();
                        if (wraptext != null && wraptext.Contains(property.Name))
                        {
                            columnIsWraptext.Add(i);
                        }
                        // Date Time Time
                        var dateTimeTime = columnsToTake.Where(p => p.isDateTimeTime == true).Select(p => p.ColumnName).ToList();
                        if (dateTimeTime != null && dateTimeTime.Contains(property.Name))
                        {
                            columnIsDateTimeTime.Add(i);
                        }
                        //currency
                        var currency = columnsToTake.Where(p => p.isCurrency == true).Select(p => p.ColumnName).ToList();
                        if (currency != null && currency.Contains(property.Name))
                        {
                            columnIsCurrency.Add(i);
                        }
                        //number
                        var number = columnsToTake.Where(p => p.isNumber == true).Select(p => p.ColumnName).ToList();
                        if (number != null && number.Contains(property.Name))
                        {
                            columnIsNumber.Add(i);
                        }
                        //percent
                        var percent = columnsToTake.Where(p => p.isPercent == true).Select(p => p.ColumnName).ToList();
                        if (percent != null && percent.Contains(property.Name))
                        {
                            columnIsPercent.Add(i);
                        }
                        //boolean
                        var isBoolean = columnsToTake.Where(p => p.isBoolean == true).Select(p => p.ColumnName).ToList();
                        if (isBoolean != null && isBoolean.Contains(property.Name))
                        {
                            columnIsBoolean.Add(i);
                        }
                        //total
                        var isTotal = columnsToTake.Where(p => p.isTotal == true).Select(p => p.ColumnName).ToList();
                        if (isTotal != null && isTotal.Contains(property.Name))
                        {
                            columnIsTotal.Add(i);
                        }
                        //dropdownlist
                        var isDropdownlist = columnsToTake.Where(p => p.isDropdownlist == true).Select(p => p.ColumnName).ToList();
                        if (isDropdownlist != null && isDropdownlist.Contains(property.Name))
                        {
                            columnIsDropdownlist.Add(new DropdownListModel()
                            {
                                Type = columnsToTake.Where(p => p.ColumnName == property.Name).Select(p => p.TypeId).FirstOrDefault(),
                                Index = i,
                            });
                            //List to dropdown
                            var dropdown = columnsToTake.Where(p => p.isDropdownlist == true && p.isAllowedToEdit == true);

                            dropdownModels = dropdown.Where(p => p.TypeId == ConstExcelController.GuidId).Select(p => p.DropdownData).ToList();
                            dropdownIdTypeIntModels = dropdown.Where(p => p.TypeId == ConstExcelController.IntId).Select(p => p.DropdownIdTypeIntData).ToList();
                            dropdownIdTypeStringModels = dropdown.Where(p => p.TypeId == ConstExcelController.StringId).Select(p => p.DropdownIdTypeStringData).ToList();
                            dropdownIdTypeBoolModels = dropdown.Where(p => p.TypeId == ConstExcelController.Bool).Select(p => p.DropdownIdTypeBoolData).ToList();
                        }
                        //depentdent dropdown list
                        var isDependentDropdownlist = columnsToTake.Where(p => p.isDependentDropdown == true).Select(p => p.ColumnName).ToList();
                        if (isDependentDropdownlist != null && isDependentDropdownlist.Contains(property.Name))
                        {
                            columnIsDependentDropdownlist.Add(i);
                            //List to dropdown
                            var dropdown = columnsToTake.Where(p => p.isDependentDropdown == true && p.isAllowedToEdit == true);

                            dependentDropdownModels = dropdown.Where(p => p.TypeId == ConstExcelController.GuidId).ToList();
                        }
                        //gender
                        var isGender = columnsToTake.Where(p => p.isGender == true).Select(p => p.ColumnName).ToList();
                        if (isGender != null && isGender.Contains(property.Name))
                        {
                            columnIsGender.Add(i);
                        }
                        //different color header
                        var isDifferentColorHeader = columnsToTake.Where(p => p.isDifferentColorHeader == true).Select(p => p.ColumnName).ToList();
                        if (isDifferentColorHeader != null && isDifferentColorHeader.Contains(property.Name))
                        {
                            columnIsDifferentColorHeader.Add(i);
                        }
                        //another header name
                        var hasAnotherHeaderName = columnsToTake.Where(p => p.hasAnotherName == true).Select(p => p.ColumnName).ToList();
                        if (hasAnotherHeaderName != null && hasAnotherHeaderName.Contains(property.Name))
                        {
                            columnHasAnotherHeaderName.Add(i);
                        }
                        //note
                        var hasNote = columnsToTake.Where(p => p.hasNote == true).Select(p => p.ColumnName).ToList();
                        if (hasNote != null && hasNote.Contains(property.Name))
                        {
                            columnHasNote.Add(i);
                        }
                    }
                }
                #endregion
                // add the content into the Excel file
                if (headerRowMergeCount > 0)
                {
                    for (int i = 0; i < headerRowMergeCount; i++)
                    {
                        // generate the data you want to insert
                        DataRow toInsert = dataTable.NewRow();

                        // insert in the desired place
                        dataTable.Rows.InsertAt(toInsert, i);
                    }
                }
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content
                int columnIndex = 1;
                //format serial number column
                #region Format serial number column
                if (showSrNo)
                {
                    ExcelRange serialNumber = workSheet.Cells[startRowFrom + startBodyColumnFrom, columnIndex, startRowFrom + dataTable.Rows.Count, columnIndex];
                    serialNumber.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    //ExcelRange columnheaderCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.Start.Row, columnIndex];
                    if (isEdit == false)
                    {
                        columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.Start.Row, columnIndex];
                        int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        if (maxLength < 24)
                        {
                            workSheet.Column(columnIndex).AutoFit(24);
                        }
                        else
                        {
                            workSheet.Column(columnIndex).AutoFit();
                        }
                    }
                    else
                    {
                        int maxLength = 0;
                        maxLength = columnCells.Max(cell => cell.Value == null ? 0 : (cell.Value).ToString().Count());
                        if (maxLength < 150)
                        {
                            workSheet.Column(columnIndex).AutoFit();
                        }
                    }
                    columnIndex++;
                }

                #endregion


                #region Format Header
                int startRowFromAfterMergeHeaderTitle = startRowFrom;
                //merge complex header
                if (headerRowMergeCount > 0)
                {
                    var complexHeader = columnsToTake.Where(p => p.MergeHeaderTitle != null).Select(p => p.MergeHeaderTitle).GroupBy(p => p).ToList();
                    int columnsToTakeIndex = 1;
                    if (showSrNo)
                    {
                        ExcelRange range = workSheet.Cells[startRowFrom, 1, startRowFrom + headerRowMergeCount, 1];
                        range.Merge = true;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        columnsToTakeIndex += 1;
                    }
                    foreach (var columnsToTakeItem in columnsToTake)
                    {
                        if (string.IsNullOrEmpty(columnsToTakeItem.MergeHeaderTitle))
                        {
                            ExcelRange range = workSheet.Cells[startRowFrom, columnsToTakeIndex, startRowFrom + headerRowMergeCount, columnsToTakeIndex];
                            range.Merge = true;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        columnsToTakeIndex++;
                    }


                    foreach (var complexHeaderItem in complexHeader)
                    {
                        var currentComplexHeaderCount = columnsToTake.Where(p => p.MergeHeaderTitle == complexHeaderItem.Key).Count();
                        var currentComplexHeader = columnsToTake.Where(p => p.MergeHeaderTitle == complexHeaderItem.Key).FirstOrDefault();
                        int currentComplexHeaderIndex = columnsToTake.FindIndex(p => p.ColumnName == currentComplexHeader.ColumnName) + 1;
                        if (showSrNo)
                        {
                            currentComplexHeaderIndex += 1;
                        }
                        //header row
                        for (int i = 0; i < currentComplexHeaderCount; i++)
                        {
                            ExcelRange rangeHeaderRow = workSheet.Cells[startRowFrom + headerRowMergeCount, currentComplexHeaderIndex + i];
                            rangeHeaderRow.Value = dataTable.Columns[currentComplexHeaderIndex + i - 1].ColumnName;
                            rangeHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rangeHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        //header merge column
                        ExcelRange range = workSheet.Cells[startRowFrom, currentComplexHeaderIndex, startRowFrom, currentComplexHeaderIndex + (currentComplexHeaderCount - 1)];

                        range.Merge = true;
                        range.Value = complexHeaderItem.Key;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    }
                    startRowFromAfterMergeHeaderTitle += headerRowMergeCount;
                }

                // format header - bold, yellow on black
                if (showSrNo)
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom, startBodyColumnFrom, startRowFromAfterMergeHeaderTitle, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.WrapText = true;

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);
                    }
                }
                else
                {
                    using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFromAfterMergeHeaderTitle, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                        //r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

                    }
                }

                //Coloring header
                if (columnIsDifferentColorHeader != null && columnIsDifferentColorHeader.Count > 0)
                {
                    foreach (var columnHeaderIndex in columnIsDifferentColorHeader)
                    {
                        ExcelRange range = workSheet.Cells[startRowFrom, (columnHeaderIndex + 1)];
                        if (showSrNo)
                        {
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(columnsToTake[(columnHeaderIndex - 1)].ColorHeader));
                        }
                        else
                        {
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(columnsToTake[(columnHeaderIndex)].ColorHeader));
                        }

                    }
                }
                //another header name
                if (columnHasAnotherHeaderName != null && columnHasAnotherHeaderName.Count > 0)
                {
                    foreach (var columnHeaderIndex in columnHasAnotherHeaderName)
                    {
                        ExcelRange range = workSheet.Cells[(startRowFrom + 1), (columnHeaderIndex + 1)];
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Font.Bold = true;
                        range.Style.Locked = true;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                        range.Value = columnsToTake[(columnHeaderIndex - 1)].AnotherName;

                    }
                }

                //another header name
                if (columnHasNote != null && columnHasNote.Count > 0)
                {
                    foreach (var columnHeaderIndex in columnHasNote)
                    {
                        ExcelRange range = workSheet.Cells[(startRowFrom + 2), (columnHeaderIndex + 1)];
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Font.Bold = true;
                        range.Style.Locked = true;
                        range.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FF0000"));
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFF00"));
                        range.Value = columnsToTake[(columnHeaderIndex)].Note;

                    }
                }


                if (headerRowMergeCount > 0)
                {
                    startRowFrom += headerRowMergeCount;
                }


                // format cells - add borders
                //using (ExcelRange r = workSheet.Cells[startRowFrom + 1, startBodyColumnFrom, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                //{
                //    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                //}
                #endregion
                //Wraptext
                ExcelRange bodyCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                //bodyCell.Style.WrapText = true;
                bodyCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //Border
                bodyCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                bodyCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                bodyCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                bodyCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                bodyCell.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                bodyCell.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                bodyCell.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                bodyCell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                
                //Fill background color if item is allowed to edit => except number index column
                #region Format allow edit range
                if (isEdit == true)
                {
                    //fill color
                    if (columnHasAnotherHeaderName == null || columnHasAnotherHeaderName.Count == 0)
                    {
                        foreach (var index in columnIsAllowedToEdit)
                        {

                            ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                            cellToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cellToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                            cellToFill.Style.Locked = false;
                        }
                    }

                }
                else
                {
                    if (columnHasAnotherHeaderName == null || columnHasAnotherHeaderName.Count == 0)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            //Add new => number index column is allowed to edit => fill color
                            ExcelRange indexToFill = workSheet.Cells[startRowFrom + 1, 1];
                            indexToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            indexToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                            indexToFill.Style.Locked = false;

                            foreach (var index in columnIsAllowedToEdit)
                            {
                                int columnType = i + 1;
                                ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1)];
                                cellToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                cellToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                                cellToFill.Style.Locked = false;
                            }
                        }
                    }
                    //else
                    //{
                    //    for (int i = 0; i < dataTable.Columns.Count; i++)
                    //    {
                    //        //Add new => number index column is allowed to edit => fill color
                    //        ExcelRange indexToFill = workSheet.Cells[startRowFrom + 1, 1];
                    //        indexToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //        indexToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                    //        indexToFill.Style.Locked = false;

                    //        foreach (var index in columnIsAllowedToEdit)
                    //        {
                    //            int columnType = i + 1;
                    //            ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1)];
                    //            cellToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //            cellToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                    //            cellToFill.Style.Locked = false;
                    //        }
                    //    }
                    //}

                }
                #endregion
                #region Format Wraptext
                if (columnIsWraptext != null && columnIsWraptext.Count > 0)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        ExcelRange allCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                        allCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        allCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (var index in columnIsWraptext)
                        {
                            ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                            workSheet.Column(index + 1).Width = 80;
                            if (columnsToTake[index - 1].CustomWidth != 0)
                            {
                                workSheet.Column(index + 1).Width = columnsToTake[index - 1].CustomWidth;
                            }
                            cellToFill.Style.WrapText = true;
                            cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cellToFill.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        ExcelRange allCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                        allCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        allCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (var index in columnIsWraptext)
                        {
                            ExcelRange cellToFill = workSheet.Cells[startRowFrom + 2, (index + 1), startRowFrom + dataTable.Rows.Count + 2, (index + 1)];
                            workSheet.Column(index + 1).Width = 80;
                            if (columnsToTake[index].CustomWidth != 0)
                            {
                                workSheet.Column(index + 1).Width = columnsToTake[index].CustomWidth;
                            }
                            cellToFill.Style.WrapText = true;
                            cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cellToFill.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }

                }

                #endregion

                #region Format DateTime
                if (columnIsDateTime != null && columnIsDateTime.Count > 0)
                {
                    foreach (var index in columnIsDateTime)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        cellToFill.Style.Numberformat.Format = "DD/MM/YYYY";
                    }
                }
                if (columnIsDateTimeTime != null && columnIsDateTimeTime.Count > 0)
                {
                    foreach (var index in columnIsDateTimeTime)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        cellToFill.Style.Numberformat.Format = "DD/MM/YYYY hh:mm:ss";
                        cellToFill.AutoFitColumns();
                    }
                }
                #endregion

                #region Format currency
                if (columnIsCurrency != null && columnIsCurrency.Count > 0)
                {
                    foreach (var index in columnIsCurrency)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        cellToFill.Style.Numberformat.Format = "#,##0";
                    }
                }
                #endregion

                #region Format number
                if (columnIsNumber != null && columnIsNumber.Count > 0)
                {
                    foreach (var index in columnIsNumber)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        workSheet.Column(index + 1).Width = 80;
                        if (columnsToTake[index - 1].CustomWidth != 0)
                        {
                            workSheet.Column(index + 1).Width = columnsToTake[index - 1].CustomWidth;
                        }
                        foreach (var cell in cellToFill)
                        {
                            cell.Value = Convert.ToDecimal(cell.Value);
                        }
                        cellToFill.Style.Numberformat.Format = "##,##";
                        cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                }
                #endregion

                #region Format percent
                if (columnIsPercent != null && columnIsPercent.Count > 0)
                {
                    foreach (var index in columnIsPercent)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        foreach (var cell in cellToFill)
                        {
                            cell.Value = Convert.ToDecimal(cell.Value);
                        }
                        cellToFill.Style.Numberformat.Format = "#,##0.00%";
                    }
                }
                #endregion

                #region Boolen
                if (columnIsBoolean != null && columnIsBoolean.Count > 0)
                {
                    foreach (var index in columnIsBoolean)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        foreach (var item in cellToFill)
                        {
                            switch ((bool?)item.Value)
                            {
                                case true:
                                    item.Value = "X";
                                    break;
                                case false:
                                    item.Value = "";
                                    break;
                                default:
                                    item.Value = "";
                                    break;
                            }
                        }
                        cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
                #endregion

                #region Gender
                if (columnIsGender != null && columnIsGender.Count > 0)
                {
                    foreach (var index in columnIsGender)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        foreach (var item in cellToFill)
                        {
                            switch ((bool?)item.Value)
                            {
                                case true:
                                    item.Value = "Nam";
                                    break;
                                case false:
                                    item.Value = "Nữ";
                                    break;
                                default:
                                    item.Value = "";
                                    break;
                            }
                        }
                        cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
                #endregion

                #region Total
                if (columnIsTotal != null && columnIsTotal.Count > 0)
                {
                    foreach (var index in columnIsTotal)
                    {
                        for (int i = startRowFrom; i < startRowFrom + dataTable.Rows.Count; i++)
                        {
                            ExcelRange cellToFill = workSheet.Cells[i + 1, (index + 1)];
                            ExcelRange price = workSheet.Cells[i + 1, (index + 1) - 2];
                            ExcelRange quantity = workSheet.Cells[i + 1, (index + 1) - 1];
                            string formula = string.Format("IF(AND({0}<>\"\", {1}<>\"\"), {0}*{1}, \"\")", price, quantity);
                            cellToFill.Formula = formula;
                        }
                    }
                }
                #endregion

                #region Dropdownlist
                if (HasExtraSheet == true || HasExtraSheet == null)
                {
                    ExcelWorksheet workSheet2 = package.Workbook.Worksheets.Add("MasterData");
                    if (columnIsDropdownlist != null && columnIsDropdownlist.Count > 0)
                    {
                        columnIsDropdownlist = columnIsDropdownlist.OrderBy(p => p.Type == ConstExcelController.GuidId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.IntId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.StringId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.Bool).ToList();
                        List<ExcelRange> excelFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchIdFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchFormulaLst = new List<ExcelRange>();
                        #region Data Validation
                        int indexColumn = 2;
                        //Type Guid
                        if (dropdownModels != null && dropdownModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownModels)
                            {
                                //int indexRow = 0;
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type int
                        if (dropdownIdTypeIntModels != null && dropdownIdTypeIntModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeIntModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type string
                        if (dropdownIdTypeStringModels != null && dropdownIdTypeStringModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeStringModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;
                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type Bool
                        if (dropdownIdTypeBoolModels != null && dropdownIdTypeBoolModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeBoolModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        #endregion
                        //iterate through two collections of the same length
                        var smallestUpperBound = Math.Min(columnIsDropdownlist.Count, excelFormulaLst.Count);
                        for (var index = 0; index < smallestUpperBound; index++)
                        {
                            // Do something with collection1[index] and collection2[index]
                            //Dropdownlist
                            ExcelRange dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), startRowFrom + dataTable.Rows.Count, (columnIsDropdownlist[index].Index + 1)];
                            if (isEdit == false)
                            {
                                dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), 1000, (columnIsDropdownlist[index].Index + 1)];
                            }
                            var listResult = dropdownColumn.DataValidation.AddListDataValidation();

                            //Get data for dropdownlist
                            //MUST turn range to absolute $$ => apply for all row in table
                            //If not absolute, next row will leave the last data row before
                            var itemNameBefore = excelFormulaLst[index];
                            //start
                            var sItemNameRow = itemNameBefore.Start.Row.ToString();
                            var sItemNameCol = itemNameBefore.Start.Column.ToString();
                            var sItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sItemNameRow + "C" + sItemNameCol, 0, 0);
                            //end
                            var eItemNameRow = itemNameBefore.End.Row.ToString();
                            var eItemNameCol = itemNameBefore.End.Column.ToString();
                            var eItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eItemNameRow + "C" + eItemNameCol, 0, 0);
                            //matchName final
                            var itemNameEnd = string.Format("{0}:{1}", sItemNameFromRC, eItemNameFromRC);

                            listResult.Formula.ExcelFormula = string.Format("{0}!{1}", workSheet2.Name, itemNameEnd);
                            listResult.ShowErrorMessage = true;
                            listResult.Error = LanguageResource.DataValidationError;
                        }

                        var smallestUpperBoundMatch = Math.Min(matchIdFormulaLst.Count, matchFormulaLst.Count);
                        int matchIndex = 1;
                        for (var index = 0; index < smallestUpperBoundMatch; index++)
                        {
                            //INDEX, MATCH
                            var indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), (startRowFrom + dataTable.Rows.Count), (dataTable.Columns.Count + matchIndex)];
                            if (isEdit == false)
                            {
                                indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), 1000, (dataTable.Columns.Count + matchIndex)]; ;
                            }
                            #region MatchId with absolute $$
                            var matchIdBefore = matchIdFormulaLst[index];
                            //start
                            var sMatchIdRow = matchIdBefore.Start.Row.ToString();
                            var sMatchIdCol = matchIdBefore.Start.Column.ToString();
                            var sMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchIdRow + "C" + sMatchIdCol, 0, 0);
                            //end
                            var eMatchIdRow = matchIdBefore.End.Row.ToString();
                            var eMatchIdCol = matchIdBefore.End.Column.ToString();
                            var eMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchIdRow + "C" + eMatchIdCol, 0, 0);
                            //matchId final
                            var matchIdEnd = string.Format("{0}:{1}", sMatchIdFromRC, eMatchIdFromRC);
                            #endregion

                            #region MatchName with absolute $$
                            var matchNameBefore = matchFormulaLst[index];
                            //start
                            var sMatchNameRow = matchNameBefore.Start.Row.ToString();
                            var sMatchNameCol = matchNameBefore.Start.Column.ToString();
                            var sMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchNameRow + "C" + sMatchNameCol, 0, 0);
                            //end
                            var eMatchNameRow = matchNameBefore.End.Row.ToString();
                            var eMatchNameCol = matchNameBefore.End.Column.ToString();
                            var eMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchNameRow + "C" + eMatchNameCol, 0, 0);
                            //matchName final
                            var matchNameEnd = string.Format("{0}:{1}", sMatchNameFromRC, eMatchNameFromRC);
                            #endregion

                            var executeIndex = workSheet.Cells[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1)];
                            indexMatch.Formula = "IFERROR(INDEX('" + workSheet2.Name + "'!" + matchIdEnd + ",MATCH(" + executeIndex + ",'" + workSheet2.Name + "'!" + matchNameEnd + ",0)),\"\")";
                            matchIndex += 1;
                        }

                        //hide column dropdownlist data
                        //for (int i = 1; i <= (columnIsDropdownlist.Count * 3); i++)
                        //{
                        //    workSheet.Column(dataTable.Columns.Count + i).Hidden = true;
                        //}
                    }
                }
                #endregion

                #region Dependent Dropdownlist
                if (columnIsDependentDropdownlist != null && columnIsDependentDropdownlist.Count > 0)
                {
                    List<ExcelRange> excelFormulaLst = new List<ExcelRange>();
                    List<ExcelRange> matchIdFormulaLst = new List<ExcelRange>();
                    List<ExcelRange> matchFormulaLst = new List<ExcelRange>();
                    //Type Guid
                    if (dependentDropdownModels != null && dependentDropdownModels.Count > 0)
                    {
                        int index = 0;
                        foreach (var dropdown in dependentDropdownModels)
                        {
                            ExcelWorksheet workSheetDependentDropdown = package.Workbook.Worksheets.Add(dropdown.DependentDropdownSheetName);
                            int indexRow = 2;
                            int indexColumn = 1;
                            int indexColumnMatch = 0;

                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var item in dropdown.DropdownData)
                            {
                                //Have parent level
                                var isHaveParentLevel2 = dropdown.DropdownData.Where(p => p.ParentLevel2Id != null).FirstOrDefault() != null;
                                if (isHaveParentLevel2)
                                {
                                    //Id
                                    ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //ParentLevel1Id
                                    ExcelRange cellId2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                    cellId2.Value = item.ParentLevel1Id;

                                    //ParentLevel2Id
                                    ExcelRange cellId3 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 2)];
                                    cellId3.Value = item.ParentLevel2Id;

                                    //Name
                                    ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 3)];
                                    cellName.Value = item.Name;

                                    //ParentLevel1Name
                                    ExcelRange cellName2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 4)];
                                    cellName2.Value = item.ParentLevel1Name;

                                    //ParentLevel2Name
                                    ExcelRange cellName3 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 5)];
                                    cellName3.Value = item.ParentLevel2Name;

                                    indexRow++;

                                    indexColumnMatch = indexColumn + 3;
                                }
                                else
                                {
                                    var isHaveParentLevel1 = dropdown.DropdownData.Where(p => p.ParentLevel1Id != null).FirstOrDefault() != null;
                                    if (isHaveParentLevel1 == true)
                                    {
                                        //Id
                                        ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                        cellId.Value = item.Id;

                                        //ParentLevel1Id
                                        ExcelRange cellId2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                        cellId2.Value = item.ParentLevel1Id;

                                        //Name
                                        ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 2)];
                                        cellName.Value = item.Name;

                                        //ParentLevel1Name
                                        ExcelRange cellName2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 3)];
                                        cellName2.Value = item.ParentLevel1Name;

                                        indexRow++;

                                        indexColumnMatch = indexColumn + 2;
                                    }
                                    else
                                    {
                                        //Id
                                        ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                        cellId.Value = item.Id;

                                        //Name
                                        ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                        cellName.Value = item.Name;
                                        indexRow++;

                                        indexColumnMatch = indexColumn + 1;
                                    }
                                }


                            }

                            //Dropdownlist
                            ExcelRange dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDependentDropdownlist[index] + 1), startRowFrom + dataTable.Rows.Count, (columnIsDependentDropdownlist[index] + 1)];
                            if (isEdit == false)
                            {
                                dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDependentDropdownlist[index] + 1), 1000, (columnIsDependentDropdownlist[index] + 1)];
                            }
                            var listResult = dropdownColumn.DataValidation.AddListDataValidation();
                            listResult.Formula.ExcelFormula = dropdown.DependentDropdownSheetName;

                            var formula = dropdown.DependentDropdownFormula.Replace("[MainSheet]", headingWithFormat.ToTitleCase(RemoveSign4VietnameseString(headingMainContent.ToLower()))).Replace(" ", "");

                            package.Workbook.Names.AddFormula(dropdown.DependentDropdownSheetName,
                                                             formula);

                            //Index, Match
                            //index of id
                            matchIdFormula = workSheetDependentDropdown.Cells[2, (indexColumn), indexRow - 1, (indexColumn)];
                            matchIdFormulaLst = new List<ExcelRange>();
                            matchIdFormulaLst.Add(matchIdFormula);
                            //index of name
                            matchFormula = workSheetDependentDropdown.Cells[2, indexColumnMatch, indexRow - 1, indexColumnMatch];
                            matchFormulaLst = new List<ExcelRange>();
                            matchFormulaLst.Add(matchFormula);

                            var smallestUpperBoundMatch = Math.Min(matchIdFormulaLst.Count, matchFormulaLst.Count);
                            int matchIndex = columnIsDropdownlist.Count() + index + 1;
                            for (var indexUpperBoundMatch = 0; indexUpperBoundMatch < smallestUpperBoundMatch; indexUpperBoundMatch++)
                            {
                                //INDEX, MATCH
                                var indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), (startRowFrom + dataTable.Rows.Count), (dataTable.Columns.Count + matchIndex)];
                                if (isEdit == false)
                                {
                                    indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), 1000, (dataTable.Columns.Count + matchIndex)]; ;
                                }
                                #region MatchId with absolute $$
                                var matchIdBefore = matchIdFormulaLst[indexUpperBoundMatch];
                                //start
                                var sMatchIdRow = matchIdBefore.Start.Row.ToString();
                                var sMatchIdCol = matchIdBefore.Start.Column.ToString();
                                var sMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchIdRow + "C" + sMatchIdCol, 0, 0);
                                //end
                                var eMatchIdRow = matchIdBefore.End.Row.ToString();
                                var eMatchIdCol = matchIdBefore.End.Column.ToString();
                                var eMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchIdRow + "C" + eMatchIdCol, 0, 0);
                                //matchId final
                                var matchIdEnd = string.Format("{0}:{1}", sMatchIdFromRC, eMatchIdFromRC);
                                #endregion

                                #region MatchName with absolute $$
                                var matchNameBefore = matchFormulaLst[indexUpperBoundMatch];
                                //start
                                var sMatchNameRow = matchNameBefore.Start.Row.ToString();
                                var sMatchNameCol = matchNameBefore.Start.Column.ToString();
                                var sMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchNameRow + "C" + sMatchNameCol, 0, 0);
                                //end
                                var eMatchNameRow = matchNameBefore.End.Row.ToString();
                                var eMatchNameCol = matchNameBefore.End.Column.ToString();
                                var eMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchNameRow + "C" + eMatchNameCol, 0, 0);
                                //matchName final
                                var matchNameEnd = string.Format("{0}:{1}", sMatchNameFromRC, eMatchNameFromRC);
                                #endregion

                                var executeIndex = workSheet.Cells[startRowFrom + 1, (columnIsDependentDropdownlist[indexUpperBoundMatch + index] + 1)];
                                indexMatch.Formula = "IFERROR(INDEX(" + workSheetDependentDropdown.Name + "!" + matchIdEnd + ",MATCH(" + executeIndex + "," + workSheetDependentDropdown.Name + "!" + matchNameEnd + ",0)),\"\")";
                                matchIndex += 1;
                            }

                            index++;
                        }


                    }
                }
                #endregion

                #region Format Heading
                if (isHeadingHasValue == true)
                {
                    int headerIndex = 1;
                    int headerColumnIndex = 1;
                    int colunmWarning = 2;
                    foreach (var itemHeading in heading)
                    {
                        workSheet.Cells[headerIndex, headerColumnIndex].Value = itemHeading.Content;

                        if (itemHeading.isWarning == true)
                        {
                            ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, colunmWarning];
                            //headerCell.Merge = true;
                            //headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            headerCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);

                            if (itemHeading.isHasBorder == true)
                            {
                                ExcelRange headerBorderCell = workSheet.Cells[headerIndex, colunmWarning + 1];
                                headerBorderCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                headerBorderCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                headerBorderCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                headerBorderCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                headerBorderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerBorderCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                            }

                            if (!string.IsNullOrEmpty(itemHeading.colorCode))
                            {
                                ExcelRange headerCellWithColor = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, colunmWarning + 1];
                                headerCellWithColor.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCellWithColor.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(itemHeading.colorCode));

                                if (itemHeading.isWhiteText == true)
                                {
                                    headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                                }

                            }
                        }
                        else
                        {
                            if (itemHeading.isCode == false)
                            {
                                ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, dataTable.Columns.Count];
                                if (IsMergeCellHeader == true || IsMergeCellHeader == null)
                                {
                                    headerCell.Merge = true;
                                    if (itemHeading.isHeadingDetail == false)
                                    {
                                        headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                }
                                if (itemHeading.isHeadingDetail == false)
                                {
                                    if (headerFontSize != null)
                                    {
                                        headerCell.Style.Font.Size = headerFontSize.Value;
                                    }
                                    else
                                    {
                                        headerCell.Style.Font.Size = 20;
                                    }
                                   
                                }
                            }
                            if (itemHeading.isTable == true)
                            {
                                ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, headerColumnIndex];
                                headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);

                                ExcelRange inputCell = workSheet.Cells[headerIndex, headerColumnIndex + 1, headerIndex, headerColumnIndex + 1];
                                inputCell.Style.Locked = false;
                                inputCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                inputCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                                ExcelRange borderCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, headerColumnIndex + 1];
                                borderCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                borderCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                borderCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                borderCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                borderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            }
                        }
                        headerIndex++;
                        workSheet.InsertRow(headerIndex, itemHeading.RowsToIgnore);
                        headerIndex += itemHeading.RowsToIgnore;
                        if (showSrNo)
                        {
                            workSheet.Column(headerColumnIndex).Width = 15;
                        }
                    }


                    //workSheet.Cells[headerIndex, headerIndex].Value = heading;
                    //ExcelRange headerCell = workSheet.Cells[headerIndex, headerIndex, headerIndex, dataTable.Columns.Count];
                    //headerCell.Merge = true;
                    //headerCell.Style.Font.Size = 20;
                    //headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                #endregion
                //scroll
                if (!showSrNo)
                {
                    workSheet.View.FreezePanes(12, 2);
                }
                else
                {
                    workSheet.Column(1).Width = 10;
                }

                //Font Family
                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                allCells.Style.Font.Name = "Times New Roman";
              

                //Edit => lock sheet
                if (isEdit == true)
                {
                    //Protect sheet
                    //string PasswordProtectExcelFile = WebConfigurationManager.AppSettings["PasswordProtectExcelFile"].ToString();
                    //workSheet.Protection.IsProtected = true;
                    //workSheet.Protection.SetPassword(PasswordProtectExcelFile);

                    //Protect workbook
                    //package.Workbook.Protection.LockStructure = true;
                    //package.Workbook.Protection.SetPassword(PasswordProtectExcelFile);
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        //List<T> data: List of model need to select
        //List<ExcelTemplate> ColumnsToTake: Use this template to initialize necessary attributes
        //bool showSlno: Show serial number or not. Exp: No. 1, 2, 3,...
        public static byte[] ExportExcel<T>(List<T> data, List<ExcelTemplate> ColumnsToTake, List<ExcelHeadingTemplate> Heading, bool showSlno = false, bool? HasExtraSheet = true, bool? IsMergeCellHeader = true, int headerRowMergeCount = 0, ePaperSize? PageSize = null, int? Scale = null, eOrientation? Orientation = null, float? headerFontSize = null, float? bodyFontSize = null)
        {
            return ExportExcel<T>(ListToDataTable<T>(data), ColumnsToTake, Heading, showSlno, HasExtraSheet, IsMergeCellHeader, headerRowMergeCount,PageSize,Scale, Orientation, headerFontSize, bodyFontSize);
        }
        public static byte[] ExportExcel<T>(List<ExportExcelInputModel> listInput)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (var item in listInput)
                {
                    ExportExcelWorksheet<ProfileExportExcelModel>(package,ListToDataTable(item.Data), item.ColumnsToTake, item.Heading, item.showSlno, item.HasExtraSheet, item.IsMergeCellHeader, item.headerRowMergeCount);
                }
                return package.GetAsByteArray();
            }
            
        }
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        public static void ExportExcelWorksheet<T>(ExcelPackage package , DataTable dataTable, List<ExcelTemplate> columnsToTake, List<ExcelHeadingTemplate> heading, bool showSrNo = false, bool? HasExtraSheet = true, bool? IsMergeCellHeader = true, int headerRowMergeCount = 0)
        {
            bool isEdit = false;
            if (dataTable.Rows.Count > 0)
            {
                isEdit = true;
            }
            bool isHeadingHasValue = (heading != null && heading.Count > 0);
            string headingMainContent = "";
            int headingTotalRows = 0;
            if (isHeadingHasValue == true)
            {
                //Set necessary attributes of heading
                headingMainContent = heading[1].Content;
                headingTotalRows = heading.Count();
            }

            // Creates a TextInfo based on the "en-US" culture.
            TextInfo headingWithFormat = new CultureInfo("en-US", false).TextInfo;
            //SheetName
            string sheetName = string.Empty;
            if (headingMainContent.Contains("Khách hàng cần tạo trên ECC"))
            {
                sheetName = string.Format("Customer_SD({0})", dataTable.Rows.Count);
            }
            else
            {
                sheetName = String.Format("{0}", headingWithFormat.ToTitleCase(RemoveSign4VietnameseString(headingMainContent.ToLower()))).Replace(" ", "");
            }

            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(sheetName);

            int startRowFrom = (isHeadingHasValue == true) ? headingTotalRows + 1 : 1;
            int startBodyColumnFrom = 1;
            if (!showSrNo)
            {
                startBodyColumnFrom = 0;
            }
            if (showSrNo)
            {
                DataColumn dataColumn = dataTable.Columns.Add(string.Format(LanguageResource.Export_ExcelRequired, LanguageResource.NumberIndex), typeof(int));
                dataColumn.SetOrdinal(0);
                int index = 1;
                if (isEdit == true)
                {
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                else
                {
                    dataTable.Rows.Add(index);
                }
            }


            //removed ignored columns
            for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
            {
                if (i == 0 && showSrNo)
                {
                    continue;
                }
                if (!columnsToTake.Select(p => p.ColumnName).Contains(dataTable.Columns[i].ColumnName))
                {
                    dataTable.Columns.RemoveAt(i);
                }
            }

            #region initialize list contain column with format
            List<int> columnIsAllowedToEdit = new List<int>();
            List<int> columnIsDateTime = new List<int>();
            List<int> columnIsDateTimeTime = new List<int>();
            List<int> columnIsCurrency = new List<int>();
            List<int> columnIsNumber = new List<int>();
            List<int> columnIsBoolean = new List<int>();
            List<int> columnIsGender = new List<int>();
            List<int> columnIsTotal = new List<int>();
            List<int> columnIsWraptext = new List<int>();
            List<DropdownListModel> columnIsDropdownlist = new List<DropdownListModel>();
            List<int> columnIsDifferentColorHeader = new List<int>();
            List<int> columnHasAnotherHeaderName = new List<int>();
            List<int> columnHasNote = new List<int>();
            List<List<DropdownModel>> dropdownModels = new List<List<DropdownModel>>();
            List<List<DropdownIdTypeIntModel>> dropdownIdTypeIntModels = new List<List<DropdownIdTypeIntModel>>();
            List<List<DropdownIdTypeStringModel>> dropdownIdTypeStringModels = new List<List<DropdownIdTypeStringModel>>();
            List<List<DropdownIdTypeBoolModel>> dropdownIdTypeBoolModels = new List<List<DropdownIdTypeBoolModel>>();
            List<int> columnIsDependentDropdownlist = new List<int>();
            List<ExcelTemplate> dependentDropdownModels = new List<ExcelTemplate>();
            #endregion
            #region Dynamic Column Headers 
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var type = typeof(T);
                var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                    .OfType<MetadataTypeAttribute>().FirstOrDefault();
                var metaData = (metadataType != null)
                    ? ModelMetadataProviders.Current.GetMetadataForType(null, metadataType.MetadataClassType)
                    : ModelMetadataProviders.Current.GetMetadataForType(null, type);

                var property = metaData.ModelType.GetProperty(dataTable.Columns[i].ColumnName);
                if (property != null)
                {
                    //DisplayName
                    var display = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    if (display != null)
                    {
                        //dd.GetName(): Get value by DisplayAttribute
                        dataTable.Columns[i].ColumnName = display.GetName();
                    }
                    //Required
                    var required = property.GetCustomAttribute(typeof(RequiredAttribute)) as RequiredAttribute;
                    if (required != null)
                    {
                        dataTable.Columns[i].ColumnName = string.Format(LanguageResource.Export_ExcelRequired, dataTable.Columns[i].ColumnName);
                    }
                    //Get column name if it is allowed to edit => isAllowedToEdit = true
                    var columnName = columnsToTake.Where(p => p.isAllowedToEdit == true).Select(p => p.ColumnName).ToList();
                    if (columnName.Contains(property.Name))
                    {
                        //get type of field
                        //dataType.Add(property.PropertyType.FullName);
                        columnIsAllowedToEdit.Add(i);
                    }
                    //date time 
                    var dateTime = columnsToTake.Where(p => p.isDateTime == true).Select(p => p.ColumnName).ToList();
                    if (dateTime != null && dateTime.Contains(property.Name))
                    {
                        columnIsDateTime.Add(i);
                    }
                    var wraptext = columnsToTake.Where(p => p.isWraptext == true).Select(p => p.ColumnName).ToList();
                    if (wraptext != null && wraptext.Contains(property.Name))
                    {
                        columnIsWraptext.Add(i);
                    }
                    // Date Time Time
                    var dateTimeTime = columnsToTake.Where(p => p.isDateTimeTime == true).Select(p => p.ColumnName).ToList();
                    if (dateTimeTime != null && dateTimeTime.Contains(property.Name))
                    {
                        columnIsDateTimeTime.Add(i);
                    }
                    //currency
                    var currency = columnsToTake.Where(p => p.isCurrency == true).Select(p => p.ColumnName).ToList();
                    if (currency != null && currency.Contains(property.Name))
                    {
                        columnIsCurrency.Add(i);
                    }
                    //number
                    var number = columnsToTake.Where(p => p.isNumber == true).Select(p => p.ColumnName).ToList();
                    if (number != null && number.Contains(property.Name))
                    {
                        columnIsNumber.Add(i);
                    }
                    //boolean
                    var isBoolean = columnsToTake.Where(p => p.isBoolean == true).Select(p => p.ColumnName).ToList();
                    if (isBoolean != null && isBoolean.Contains(property.Name))
                    {
                        columnIsBoolean.Add(i);
                    }
                    //total
                    var isTotal = columnsToTake.Where(p => p.isTotal == true).Select(p => p.ColumnName).ToList();
                    if (isTotal != null && isTotal.Contains(property.Name))
                    {
                        columnIsTotal.Add(i);
                    }
                    //dropdownlist
                    var isDropdownlist = columnsToTake.Where(p => p.isDropdownlist == true).Select(p => p.ColumnName).ToList();
                    if (isDropdownlist != null && isDropdownlist.Contains(property.Name))
                    {
                        columnIsDropdownlist.Add(new DropdownListModel()
                        {
                            Type = columnsToTake.Where(p => p.ColumnName == property.Name).Select(p => p.TypeId).FirstOrDefault(),
                            Index = i,
                        });
                        //List to dropdown
                        var dropdown = columnsToTake.Where(p => p.isDropdownlist == true && p.isAllowedToEdit == true);

                        dropdownModels = dropdown.Where(p => p.TypeId == ConstExcelController.GuidId).Select(p => p.DropdownData).ToList();
                        dropdownIdTypeIntModels = dropdown.Where(p => p.TypeId == ConstExcelController.IntId).Select(p => p.DropdownIdTypeIntData).ToList();
                        dropdownIdTypeStringModels = dropdown.Where(p => p.TypeId == ConstExcelController.StringId).Select(p => p.DropdownIdTypeStringData).ToList();
                        dropdownIdTypeBoolModels = dropdown.Where(p => p.TypeId == ConstExcelController.Bool).Select(p => p.DropdownIdTypeBoolData).ToList();
                    }
                    //depentdent dropdown list
                    var isDependentDropdownlist = columnsToTake.Where(p => p.isDependentDropdown == true).Select(p => p.ColumnName).ToList();
                    if (isDependentDropdownlist != null && isDependentDropdownlist.Contains(property.Name))
                    {
                        columnIsDependentDropdownlist.Add(i);
                        //List to dropdown
                        var dropdown = columnsToTake.Where(p => p.isDependentDropdown == true && p.isAllowedToEdit == true);

                        dependentDropdownModels = dropdown.Where(p => p.TypeId == ConstExcelController.GuidId).ToList();
                    }
                    //gender
                    var isGender = columnsToTake.Where(p => p.isGender == true).Select(p => p.ColumnName).ToList();
                    if (isGender != null && isGender.Contains(property.Name))
                    {
                        columnIsGender.Add(i);
                    }
                    //different color header
                    var isDifferentColorHeader = columnsToTake.Where(p => p.isDifferentColorHeader == true).Select(p => p.ColumnName).ToList();
                    if (isDifferentColorHeader != null && isDifferentColorHeader.Contains(property.Name))
                    {
                        columnIsDifferentColorHeader.Add(i);
                    }
                    //another header name
                    var hasAnotherHeaderName = columnsToTake.Where(p => p.hasAnotherName == true).Select(p => p.ColumnName).ToList();
                    if (hasAnotherHeaderName != null && hasAnotherHeaderName.Contains(property.Name))
                    {
                        columnHasAnotherHeaderName.Add(i);
                    }
                    //note
                    var hasNote = columnsToTake.Where(p => p.hasNote == true).Select(p => p.ColumnName).ToList();
                    if (hasNote != null && hasNote.Contains(property.Name))
                    {
                        columnHasNote.Add(i);
                    }
                }
            }
            #endregion
            // add the content into the Excel file
            if (headerRowMergeCount > 0)
            {
                for (int i = 0; i < headerRowMergeCount; i++)
                {
                    // generate the data you want to insert
                    DataRow toInsert = dataTable.NewRow();

                    // insert in the desired place
                    dataTable.Rows.InsertAt(toInsert, i);
                }
            }

            if(columnHasAnotherHeaderName !=null && columnHasAnotherHeaderName.Count>0 && columnHasNote.Count>0)
            {
                DataTable listColumnName = new DataTable();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    listColumnName.Columns.Add(dataTable.Columns[i].ColumnName);
                }
                workSheet.Cells["A" + (startRowFrom)].LoadFromDataTable(listColumnName, true);
                workSheet.Cells["A" + (startRowFrom + 3)].LoadFromDataTable(dataTable, false);
            }
            else
            {
                if (columnHasAnotherHeaderName != null && columnHasAnotherHeaderName.Count > 0 && columnHasNote.Count ==0)
                {
                    DataTable listColumnName = new DataTable();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        listColumnName.Columns.Add(dataTable.Columns[i].ColumnName);
                    }
                    workSheet.Cells["A" + (startRowFrom)].LoadFromDataTable(listColumnName, true);
                    workSheet.Cells["A" + (startRowFrom + 2)].LoadFromDataTable(dataTable, false);
                }
                else
                {
                    workSheet.Cells["A" + (startRowFrom)].LoadFromDataTable(dataTable, true);
                }
                
            }
            

            // autofit width of cells with small content
            int columnIndex = 1;
            //format serial number column
            #region Format serial number column
            if (showSrNo)
            {
                ExcelRange serialNumber = workSheet.Cells[startRowFrom + startBodyColumnFrom, columnIndex, startRowFrom + dataTable.Rows.Count, columnIndex];
                serialNumber.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            foreach (DataColumn column in dataTable.Columns)
            {
                ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                //ExcelRange columnheaderCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.Start.Row, columnIndex];
                if (isEdit == false)
                {
                    columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.Start.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 24)
                    {
                        workSheet.Column(columnIndex).AutoFit(24);
                    }
                    else
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }
                }
                else
                {
                    int maxLength = 0;
                    maxLength = columnCells.Max(cell => cell.Value == null ? 0 : (cell.Value).ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }
                }
                columnIndex++;
            }

            #endregion


            #region Format Header
            int startRowFromAfterMergeHeaderTitle = startRowFrom;
            //merge complex header
            if (headerRowMergeCount > 0)
            {
                var complexHeader = columnsToTake.Where(p => p.MergeHeaderTitle != null).Select(p => p.MergeHeaderTitle).GroupBy(p => p).ToList();
                int columnsToTakeIndex = 1;
                if (showSrNo)
                {
                    ExcelRange range = workSheet.Cells[startRowFrom, 1, startRowFrom + headerRowMergeCount, 1];
                    range.Merge = true;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    columnsToTakeIndex += 1;
                }
                foreach (var columnsToTakeItem in columnsToTake)
                {
                    if (string.IsNullOrEmpty(columnsToTakeItem.MergeHeaderTitle))
                    {
                        ExcelRange range = workSheet.Cells[startRowFrom, columnsToTakeIndex, startRowFrom + headerRowMergeCount, columnsToTakeIndex];
                        range.Merge = true;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    columnsToTakeIndex++;
                }


                foreach (var complexHeaderItem in complexHeader)
                {
                    var currentComplexHeaderCount = columnsToTake.Where(p => p.MergeHeaderTitle == complexHeaderItem.Key).Count();
                    var currentComplexHeader = columnsToTake.Where(p => p.MergeHeaderTitle == complexHeaderItem.Key).FirstOrDefault();
                    int currentComplexHeaderIndex = columnsToTake.FindIndex(p => p.ColumnName == currentComplexHeader.ColumnName) + 1;
                    if (showSrNo)
                    {
                        currentComplexHeaderIndex += 1;
                    }
                    //header row
                    for (int i = 0; i < currentComplexHeaderCount; i++)
                    {
                        ExcelRange rangeHeaderRow = workSheet.Cells[startRowFrom + headerRowMergeCount, currentComplexHeaderIndex + i];
                        rangeHeaderRow.Value = dataTable.Columns[currentComplexHeaderIndex + i - 1].ColumnName;
                        rangeHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rangeHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    //header merge column
                    ExcelRange range = workSheet.Cells[startRowFrom, currentComplexHeaderIndex, startRowFrom, currentComplexHeaderIndex + (currentComplexHeaderCount - 1)];

                    range.Merge = true;
                    range.Value = complexHeaderItem.Key;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }
                startRowFromAfterMergeHeaderTitle += headerRowMergeCount;
            }

            // format header - bold, yellow on black
            if (showSrNo)
            {
                using (ExcelRange r = workSheet.Cells[startRowFrom, startBodyColumnFrom, startRowFromAfterMergeHeaderTitle, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                    //r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);
                }
            }
            else
            {
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFromAfterMergeHeaderTitle, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                    //r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

                }
            }

            //Coloring header
            if (columnIsDifferentColorHeader != null && columnIsDifferentColorHeader.Count > 0)
            {
                foreach (var columnHeaderIndex in columnIsDifferentColorHeader)
                {
                    ExcelRange range = workSheet.Cells[startRowFrom, (columnHeaderIndex + 1)];
                    if (showSrNo)
                    {
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(columnsToTake[(columnHeaderIndex - 1)].ColorHeader));
                    }
                    else
                    {
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(columnsToTake[(columnHeaderIndex)].ColorHeader));
                    }

                }
            }
            //another header name
            if (columnHasAnotherHeaderName != null && columnHasAnotherHeaderName.Count > 0)
            {
                foreach (var columnHeaderIndex in columnHasAnotherHeaderName)
                {
                    ExcelRange range = workSheet.Cells[(startRowFrom + 1), (columnHeaderIndex + 1)];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Locked = true;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FCD5B4"));
                    range.Value = columnsToTake[(columnHeaderIndex)].AnotherName;

                }
            }

            //another header name
            if (columnHasNote != null && columnHasNote.Count > 0)
            {
                foreach (var columnHeaderIndex in columnHasNote)
                {
                    ExcelRange range = workSheet.Cells[(startRowFrom + 2), (columnHeaderIndex + 1)];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Locked = true;
                    range.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FF0000"));
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFF00"));
                    range.Value = columnsToTake[(columnHeaderIndex)].Note;

                }
            }


            if (headerRowMergeCount > 0)
            {
                startRowFrom += headerRowMergeCount;
            }


            // format cells - add borders
            //using (ExcelRange r = workSheet.Cells[startRowFrom + 1, startBodyColumnFrom, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
            //{
            //    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            //    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            //    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            //    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            //    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            //}
            #endregion
            //Wraptext
            ExcelRange bodyCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
            //bodyCell.Style.WrapText = true;
            bodyCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            //Border
            bodyCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            bodyCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            bodyCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            bodyCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            bodyCell.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            bodyCell.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            bodyCell.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            bodyCell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

            //Fill background color if item is allowed to edit => except number index column
            #region Format allow edit range
            if (isEdit == true)
            {
                //fill color
                if (columnHasAnotherHeaderName == null || columnHasAnotherHeaderName.Count == 0)
                {
                    foreach (var index in columnIsAllowedToEdit)
                    {

                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                        cellToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                        cellToFill.Style.Locked = false;
                    }
                }

            }
            else
            {
                if (columnHasAnotherHeaderName == null || columnHasAnotherHeaderName.Count == 0)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        //Add new => number index column is allowed to edit => fill color
                        ExcelRange indexToFill = workSheet.Cells[startRowFrom + 1, 1];
                        indexToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        indexToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                        indexToFill.Style.Locked = false;

                        foreach (var index in columnIsAllowedToEdit)
                        {
                            int columnType = i + 1;
                            ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1)];
                            cellToFill.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cellToFill.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                            cellToFill.Style.Locked = false;
                        }
                    }
                }               

            }
            #endregion
            #region Format Wraptext
            if (columnIsWraptext != null && columnIsWraptext.Count > 0)
            {
                if (dataTable.Rows.Count > 0)
                {
                    ExcelRange allCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                    allCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    allCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    foreach (var index in columnIsWraptext)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count +3, (index + 1)];
                        workSheet.Column(index + 1).Width = 80;
                        if (columnsToTake[index].CustomWidth != 0)
                        {
                            workSheet.Column(index + 1).Width = columnsToTake[index].CustomWidth;
                        }
                        cellToFill.Style.WrapText = true;
                        cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cellToFill.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                }
                else
                {
                    ExcelRange allCell = workSheet.Cells[startRowFrom + startBodyColumnFrom, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
                    allCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    allCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    foreach (var index in columnIsWraptext)
                    {
                        ExcelRange cellToFill = workSheet.Cells[startRowFrom + 2, (index + 1), startRowFrom + dataTable.Rows.Count + 2, (index + 1)];
                        workSheet.Column(index + 1).Width = 80;
                        if (columnsToTake[index].CustomWidth != 0)
                        {
                            workSheet.Column(index + 1).Width = columnsToTake[index].CustomWidth;
                        }
                        cellToFill.Style.WrapText = true;
                        cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cellToFill.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                }

            }

            #endregion

            #region Format DateTime
            if (columnIsDateTime != null && columnIsDateTime.Count > 0)
            {
                foreach (var index in columnIsDateTime)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    cellToFill.Style.Numberformat.Format = "DD/MM/YYYY";
                }
            }
            if (columnIsDateTimeTime != null && columnIsDateTimeTime.Count > 0)
            {
                foreach (var index in columnIsDateTimeTime)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    cellToFill.Style.Numberformat.Format = "DD/MM/YYYY hh:mm:ss";
                    cellToFill.AutoFitColumns();
                }
            }
            #endregion

            #region Format currency
            if (columnIsCurrency != null && columnIsCurrency.Count > 0)
            {
                foreach (var index in columnIsCurrency)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    cellToFill.Style.Numberformat.Format = "#,##0";
                }
            }
            #endregion

            #region Format number
            if (columnIsNumber != null && columnIsNumber.Count > 0)
            {
                foreach (var index in columnIsNumber)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    foreach (var cell in cellToFill)
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    cellToFill.Style.Numberformat.Format = "#,##0.00"; 
                    cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
            }
            #endregion

            #region Boolen
            if (columnIsBoolean != null && columnIsBoolean.Count > 0)
            {
                foreach (var index in columnIsBoolean)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    foreach (var item in cellToFill)
                    {
                        switch ((bool?)item.Value)
                        {
                            case true:
                                item.Value = "X";
                                break;
                            case false:
                                item.Value = "";
                                break;
                            default:
                                item.Value = "";
                                break;
                        }
                    }
                    cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }
            #endregion

            #region Gender
            if (columnIsGender != null && columnIsGender.Count > 0)
            {
                foreach (var index in columnIsGender)
                {
                    ExcelRange cellToFill = workSheet.Cells[startRowFrom + 1, (index + 1), startRowFrom + dataTable.Rows.Count, (index + 1)];
                    foreach (var item in cellToFill)
                    {
                        switch ((bool?)item.Value)
                        {
                            case true:
                                item.Value = "Nam";
                                break;
                            case false:
                                item.Value = "Nữ";
                                break;
                            default:
                                item.Value = "";
                                break;
                        }
                    }
                    cellToFill.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }
            #endregion

            #region Total
            if (columnIsTotal != null && columnIsTotal.Count > 0)
            {
                foreach (var index in columnIsTotal)
                {
                    for (int i = startRowFrom; i < startRowFrom + dataTable.Rows.Count; i++)
                    {
                        ExcelRange cellToFill = workSheet.Cells[i + 1, (index + 1)];
                        ExcelRange price = workSheet.Cells[i + 1, (index + 1) - 2];
                        ExcelRange quantity = workSheet.Cells[i + 1, (index + 1) - 1];
                        string formula = string.Format("IF(AND({0}<>\"\", {1}<>\"\"), {0}*{1}, \"\")", price, quantity);
                        cellToFill.Formula = formula;
                    }
                }
            }
            #endregion

            #region Dropdownlist
            if (HasExtraSheet == true || HasExtraSheet == null)
            {
                var listSheet = package.Workbook.Worksheets;
                bool sheetExisting = false;
                foreach(var sheet in listSheet)
                {
                    if(sheet.Name == "MasterData")
                    {
                        sheetExisting = true;
                        break;
                    }    
                }    
               if(!sheetExisting)
                {
                    ExcelWorksheet workSheet2 = package.Workbook.Worksheets.Add("MasterData");
                    if (columnIsDropdownlist != null && columnIsDropdownlist.Count > 0)
                    {
                        columnIsDropdownlist = columnIsDropdownlist.OrderBy(p => p.Type == ConstExcelController.GuidId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.IntId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.StringId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.Bool).ToList();
                        List<ExcelRange> excelFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchIdFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchFormulaLst = new List<ExcelRange>();
                        #region Data Validation
                        int indexColumn = 2;
                        //Type Guid
                        if (dropdownModels != null && dropdownModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownModels)
                            {
                                //int indexRow = 0;
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type int
                        if (dropdownIdTypeIntModels != null && dropdownIdTypeIntModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeIntModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type string
                        if (dropdownIdTypeStringModels != null && dropdownIdTypeStringModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeStringModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;
                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type Bool
                        if (dropdownIdTypeBoolModels != null && dropdownIdTypeBoolModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeBoolModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = workSheet2.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = workSheet2.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = workSheet2.Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        #endregion
                        //iterate through two collections of the same length
                        var smallestUpperBound = Math.Min(columnIsDropdownlist.Count, excelFormulaLst.Count);
                        for (var index = 0; index < smallestUpperBound; index++)
                        {
                            // Do something with collection1[index] and collection2[index]
                            //Dropdownlist
                            ExcelRange dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), startRowFrom + dataTable.Rows.Count, (columnIsDropdownlist[index].Index + 1)];
                            if (isEdit == false)
                            {
                                dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), 1000, (columnIsDropdownlist[index].Index + 1)];
                            }
                            var listResult = dropdownColumn.DataValidation.AddListDataValidation();

                            //Get data for dropdownlist
                            //MUST turn range to absolute $$ => apply for all row in table
                            //If not absolute, next row will leave the last data row before
                            var itemNameBefore = excelFormulaLst[index];
                            //start
                            var sItemNameRow = itemNameBefore.Start.Row.ToString();
                            var sItemNameCol = itemNameBefore.Start.Column.ToString();
                            var sItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sItemNameRow + "C" + sItemNameCol, 0, 0);
                            //end
                            var eItemNameRow = itemNameBefore.End.Row.ToString();
                            var eItemNameCol = itemNameBefore.End.Column.ToString();
                            var eItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eItemNameRow + "C" + eItemNameCol, 0, 0);
                            //matchName final
                            var itemNameEnd = string.Format("{0}:{1}", sItemNameFromRC, eItemNameFromRC);

                            listResult.Formula.ExcelFormula = string.Format("{0}!{1}", workSheet2.Name, itemNameEnd);
                            listResult.ShowErrorMessage = true;
                            listResult.Error = LanguageResource.DataValidationError;
                        }

                        var smallestUpperBoundMatch = Math.Min(matchIdFormulaLst.Count, matchFormulaLst.Count);
                        int matchIndex = 1;
                        for (var index = 0; index < smallestUpperBoundMatch; index++)
                        {
                            //INDEX, MATCH
                            var indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), (startRowFrom + dataTable.Rows.Count), (dataTable.Columns.Count + matchIndex)];
                            if (isEdit == false)
                            {
                                indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), 1000, (dataTable.Columns.Count + matchIndex)]; ;
                            }
                            #region MatchId with absolute $$
                            var matchIdBefore = matchIdFormulaLst[index];
                            //start
                            var sMatchIdRow = matchIdBefore.Start.Row.ToString();
                            var sMatchIdCol = matchIdBefore.Start.Column.ToString();
                            var sMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchIdRow + "C" + sMatchIdCol, 0, 0);
                            //end
                            var eMatchIdRow = matchIdBefore.End.Row.ToString();
                            var eMatchIdCol = matchIdBefore.End.Column.ToString();
                            var eMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchIdRow + "C" + eMatchIdCol, 0, 0);
                            //matchId final
                            var matchIdEnd = string.Format("{0}:{1}", sMatchIdFromRC, eMatchIdFromRC);
                            #endregion

                            #region MatchName with absolute $$
                            var matchNameBefore = matchFormulaLst[index];
                            //start
                            var sMatchNameRow = matchNameBefore.Start.Row.ToString();
                            var sMatchNameCol = matchNameBefore.Start.Column.ToString();
                            var sMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchNameRow + "C" + sMatchNameCol, 0, 0);
                            //end
                            var eMatchNameRow = matchNameBefore.End.Row.ToString();
                            var eMatchNameCol = matchNameBefore.End.Column.ToString();
                            var eMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchNameRow + "C" + eMatchNameCol, 0, 0);
                            //matchName final
                            var matchNameEnd = string.Format("{0}:{1}", sMatchNameFromRC, eMatchNameFromRC);
                            #endregion

                            var executeIndex = workSheet.Cells[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1)];
                            //indexMatch.Formula = "IFERROR(INDEX(" + workSheet2.Name + "!" + matchIdEnd + ",MATCH(" + executeIndex + "," + workSheet2.Name + "!" + matchNameEnd + ",0)),\"\")";
                            matchIndex += 1;
                        }

                        //hide column dropdownlist data
                        //for (int i = 1; i <= (columnIsDropdownlist.Count * 3); i++)
                        //{
                        //    workSheet.Column(dataTable.Columns.Count + i).Hidden = true;
                        //}
                    }
                }
                else
                {
                    if (columnIsDropdownlist != null && columnIsDropdownlist.Count > 0)
                    {
                        columnIsDropdownlist = columnIsDropdownlist.OrderBy(p => p.Type == ConstExcelController.GuidId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.IntId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.StringId)
                                                                   .OrderBy(p => p.Type == ConstExcelController.Bool).ToList();
                        List<ExcelRange> excelFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchIdFormulaLst = new List<ExcelRange>();
                        List<ExcelRange> matchFormulaLst = new List<ExcelRange>();
                        #region Data Validation
                        int indexColumn = 2;
                        //Type Guid
                        if (dropdownModels != null && dropdownModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownModels)
                            {
                                //int indexRow = 0;
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type int
                        if (dropdownIdTypeIntModels != null && dropdownIdTypeIntModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeIntModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type string
                        if (dropdownIdTypeStringModels != null && dropdownIdTypeStringModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeStringModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;
                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        //Type Bool
                        if (dropdownIdTypeBoolModels != null && dropdownIdTypeBoolModels.Count > 0)
                        {
                            ExcelRange excelFormula;
                            ExcelRange matchIdFormula;
                            ExcelRange matchFormula;
                            foreach (var dropdownList in dropdownIdTypeBoolModels)
                            {
                                int indexRow = startRowFrom + 1;
                                foreach (var item in dropdownList)
                                {
                                    //Id
                                    //ExcelRange cellId = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn)];
                                    ExcelRange cellId = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    //ExcelRange cellName = workSheet2.Cells[indexRow, (dataTable.Columns.Count + indexColumn + 1)];
                                    ExcelRange cellName = package.Workbook.Worksheets["MasterData"].Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;
                                }
                                //index of dropdown data (display name)
                                //excelFormula = workSheet2.Cells[startRowFrom + 4, (dataTable.Columns.Count + indexColumn + 1), indexRow + 2, (dataTable.Columns.Count + indexColumn + 1)];
                                excelFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                excelFormulaLst.Add(excelFormula);
                                //index of id
                                //matchIdFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn), indexRow - 1, (dataTable.Columns.Count + indexColumn)];
                                matchIdFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn), indexRow - 1, (indexColumn)];
                                matchIdFormulaLst.Add(matchIdFormula);
                                //index of name
                                //matchFormula = workSheet2.Cells[startRowFrom + 1, (dataTable.Columns.Count + indexColumn + 1), indexRow - 1, (dataTable.Columns.Count + indexColumn + 1)];
                                matchFormula = package.Workbook.Worksheets["MasterData"].Cells[startRowFrom + 1, (indexColumn + 1), indexRow - 1, (indexColumn + 1)];
                                matchFormulaLst.Add(matchFormula);
                                indexColumn += 3;
                            }
                        }
                        #endregion
                        //iterate through two collections of the same length
                        var smallestUpperBound = Math.Min(columnIsDropdownlist.Count, excelFormulaLst.Count);
                        for (var index = 0; index < smallestUpperBound; index++)
                        {
                            // Do something with collection1[index] and collection2[index]
                            //Dropdownlist
                            ExcelRange dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), startRowFrom + dataTable.Rows.Count, (columnIsDropdownlist[index].Index + 1)];
                            if (isEdit == false)
                            {
                                dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1), 1000, (columnIsDropdownlist[index].Index + 1)];
                            }
                            var listResult = dropdownColumn.DataValidation.AddListDataValidation();

                            //Get data for dropdownlist
                            //MUST turn range to absolute $$ => apply for all row in table
                            //If not absolute, next row will leave the last data row before
                            var itemNameBefore = excelFormulaLst[index];
                            //start
                            var sItemNameRow = itemNameBefore.Start.Row.ToString();
                            var sItemNameCol = itemNameBefore.Start.Column.ToString();
                            var sItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sItemNameRow + "C" + sItemNameCol, 0, 0);
                            //end
                            var eItemNameRow = itemNameBefore.End.Row.ToString();
                            var eItemNameCol = itemNameBefore.End.Column.ToString();
                            var eItemNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eItemNameRow + "C" + eItemNameCol, 0, 0);
                            //matchName final
                            var itemNameEnd = string.Format("{0}:{1}", sItemNameFromRC, eItemNameFromRC);

                            listResult.Formula.ExcelFormula = string.Format("{0}!{1}", package.Workbook.Worksheets["MasterData"].Name, itemNameEnd);
                            listResult.ShowErrorMessage = true;
                            listResult.Error = LanguageResource.DataValidationError;
                        }

                        var smallestUpperBoundMatch = Math.Min(matchIdFormulaLst.Count, matchFormulaLst.Count);
                        int matchIndex = 1;
                        for (var index = 0; index < smallestUpperBoundMatch; index++)
                        {
                            //INDEX, MATCH
                            var indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), (startRowFrom + dataTable.Rows.Count), (dataTable.Columns.Count + matchIndex)];
                            if (isEdit == false)
                            {
                                indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), 1000, (dataTable.Columns.Count + matchIndex)]; ;
                            }
                            #region MatchId with absolute $$
                            var matchIdBefore = matchIdFormulaLst[index];
                            //start
                            var sMatchIdRow = matchIdBefore.Start.Row.ToString();
                            var sMatchIdCol = matchIdBefore.Start.Column.ToString();
                            var sMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchIdRow + "C" + sMatchIdCol, 0, 0);
                            //end
                            var eMatchIdRow = matchIdBefore.End.Row.ToString();
                            var eMatchIdCol = matchIdBefore.End.Column.ToString();
                            var eMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchIdRow + "C" + eMatchIdCol, 0, 0);
                            //matchId final
                            var matchIdEnd = string.Format("{0}:{1}", sMatchIdFromRC, eMatchIdFromRC);
                            #endregion

                            #region MatchName with absolute $$
                            var matchNameBefore = matchFormulaLst[index];
                            //start
                            var sMatchNameRow = matchNameBefore.Start.Row.ToString();
                            var sMatchNameCol = matchNameBefore.Start.Column.ToString();
                            var sMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchNameRow + "C" + sMatchNameCol, 0, 0);
                            //end
                            var eMatchNameRow = matchNameBefore.End.Row.ToString();
                            var eMatchNameCol = matchNameBefore.End.Column.ToString();
                            var eMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchNameRow + "C" + eMatchNameCol, 0, 0);
                            //matchName final
                            var matchNameEnd = string.Format("{0}:{1}", sMatchNameFromRC, eMatchNameFromRC);
                            #endregion

                            var executeIndex = workSheet.Cells[startRowFrom + 1, (columnIsDropdownlist[index].Index + 1)];
                            //indexMatch.Formula = "IFERROR(INDEX(" + workSheet2.Name + "!" + matchIdEnd + ",MATCH(" + executeIndex + "," + workSheet2.Name + "!" + matchNameEnd + ",0)),\"\")";
                            matchIndex += 1;
                        }

                        //hide column dropdownlist data
                        //for (int i = 1; i <= (columnIsDropdownlist.Count * 3); i++)
                        //{
                        //    workSheet.Column(dataTable.Columns.Count + i).Hidden = true;
                        //}
                    }
                }
            }
            #endregion

            #region Dependent Dropdownlist
            if (columnIsDependentDropdownlist != null && columnIsDependentDropdownlist.Count > 0)
            {
                List<ExcelRange> excelFormulaLst = new List<ExcelRange>();
                List<ExcelRange> matchIdFormulaLst = new List<ExcelRange>();
                List<ExcelRange> matchFormulaLst = new List<ExcelRange>();
                //Type Guid
                if (dependentDropdownModels != null && dependentDropdownModels.Count > 0)
                {
                    int index = 0;
                    foreach (var dropdown in dependentDropdownModels)
                    {
                        ExcelWorksheet workSheetDependentDropdown = package.Workbook.Worksheets.Add(dropdown.DependentDropdownSheetName);
                        int indexRow = 2;
                        int indexColumn = 1;
                        int indexColumnMatch = 0;

                        ExcelRange matchIdFormula;
                        ExcelRange matchFormula;
                        foreach (var item in dropdown.DropdownData)
                        {
                            //Have parent level
                            var isHaveParentLevel2 = dropdown.DropdownData.Where(p => p.ParentLevel2Id != null).FirstOrDefault() != null;
                            if (isHaveParentLevel2)
                            {
                                //Id
                                ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                cellId.Value = item.Id;

                                //ParentLevel1Id
                                ExcelRange cellId2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                cellId2.Value = item.ParentLevel1Id;

                                //ParentLevel2Id
                                ExcelRange cellId3 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 2)];
                                cellId3.Value = item.ParentLevel2Id;

                                //Name
                                ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 3)];
                                cellName.Value = item.Name;

                                //ParentLevel1Name
                                ExcelRange cellName2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 4)];
                                cellName2.Value = item.ParentLevel1Name;

                                //ParentLevel2Name
                                ExcelRange cellName3 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 5)];
                                cellName3.Value = item.ParentLevel2Name;

                                indexRow++;

                                indexColumnMatch = indexColumn + 3;
                            }
                            else
                            {
                                var isHaveParentLevel1 = dropdown.DropdownData.Where(p => p.ParentLevel1Id != null).FirstOrDefault() != null;
                                if (isHaveParentLevel1 == true)
                                {
                                    //Id
                                    ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //ParentLevel1Id
                                    ExcelRange cellId2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                    cellId2.Value = item.ParentLevel1Id;

                                    //Name
                                    ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 2)];
                                    cellName.Value = item.Name;

                                    //ParentLevel1Name
                                    ExcelRange cellName2 = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 3)];
                                    cellName2.Value = item.ParentLevel1Name;

                                    indexRow++;

                                    indexColumnMatch = indexColumn + 2;
                                }
                                else
                                {
                                    //Id
                                    ExcelRange cellId = workSheetDependentDropdown.Cells[indexRow, (indexColumn)];
                                    cellId.Value = item.Id;

                                    //Name
                                    ExcelRange cellName = workSheetDependentDropdown.Cells[indexRow, (indexColumn + 1)];
                                    cellName.Value = item.Name;
                                    indexRow++;

                                    indexColumnMatch = indexColumn + 1;
                                }
                            }


                        }

                        //Dropdownlist
                        ExcelRange dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDependentDropdownlist[index] + 1), startRowFrom + dataTable.Rows.Count, (columnIsDependentDropdownlist[index] + 1)];
                        if (isEdit == false)
                        {
                            dropdownColumn = workSheet.SelectedRange[startRowFrom + 1, (columnIsDependentDropdownlist[index] + 1), 1000, (columnIsDependentDropdownlist[index] + 1)];
                        }
                        var listResult = dropdownColumn.DataValidation.AddListDataValidation();
                        listResult.Formula.ExcelFormula = dropdown.DependentDropdownSheetName;

                        var formula = dropdown.DependentDropdownFormula.Replace("[MainSheet]", headingWithFormat.ToTitleCase(RemoveSign4VietnameseString(headingMainContent.ToLower()))).Replace(" ", "");

                        package.Workbook.Names.AddFormula(dropdown.DependentDropdownSheetName,
                                                         formula);

                        //Index, Match
                        //index of id
                        matchIdFormula = workSheetDependentDropdown.Cells[2, (indexColumn), indexRow - 1, (indexColumn)];
                        matchIdFormulaLst = new List<ExcelRange>();
                        matchIdFormulaLst.Add(matchIdFormula);
                        //index of name
                        matchFormula = workSheetDependentDropdown.Cells[2, indexColumnMatch, indexRow - 1, indexColumnMatch];
                        matchFormulaLst = new List<ExcelRange>();
                        matchFormulaLst.Add(matchFormula);

                        var smallestUpperBoundMatch = Math.Min(matchIdFormulaLst.Count, matchFormulaLst.Count);
                        int matchIndex = columnIsDropdownlist.Count() + index + 1;
                        for (var indexUpperBoundMatch = 0; indexUpperBoundMatch < smallestUpperBoundMatch; indexUpperBoundMatch++)
                        {
                            //INDEX, MATCH
                            var indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), (startRowFrom + dataTable.Rows.Count), (dataTable.Columns.Count + matchIndex)];
                            if (isEdit == false)
                            {
                                indexMatch = workSheet.SelectedRange[(startRowFrom + 1), (dataTable.Columns.Count + matchIndex), 1000, (dataTable.Columns.Count + matchIndex)]; ;
                            }
                            #region MatchId with absolute $$
                            var matchIdBefore = matchIdFormulaLst[indexUpperBoundMatch];
                            //start
                            var sMatchIdRow = matchIdBefore.Start.Row.ToString();
                            var sMatchIdCol = matchIdBefore.Start.Column.ToString();
                            var sMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchIdRow + "C" + sMatchIdCol, 0, 0);
                            //end
                            var eMatchIdRow = matchIdBefore.End.Row.ToString();
                            var eMatchIdCol = matchIdBefore.End.Column.ToString();
                            var eMatchIdFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchIdRow + "C" + eMatchIdCol, 0, 0);
                            //matchId final
                            var matchIdEnd = string.Format("{0}:{1}", sMatchIdFromRC, eMatchIdFromRC);
                            #endregion

                            #region MatchName with absolute $$
                            var matchNameBefore = matchFormulaLst[indexUpperBoundMatch];
                            //start
                            var sMatchNameRow = matchNameBefore.Start.Row.ToString();
                            var sMatchNameCol = matchNameBefore.Start.Column.ToString();
                            var sMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + sMatchNameRow + "C" + sMatchNameCol, 0, 0);
                            //end
                            var eMatchNameRow = matchNameBefore.End.Row.ToString();
                            var eMatchNameCol = matchNameBefore.End.Column.ToString();
                            var eMatchNameFromRC = ExcelCellBase.TranslateFromR1C1("R" + eMatchNameRow + "C" + eMatchNameCol, 0, 0);
                            //matchName final
                            var matchNameEnd = string.Format("{0}:{1}", sMatchNameFromRC, eMatchNameFromRC);
                            #endregion

                            var executeIndex = workSheet.Cells[startRowFrom + 1, (columnIsDependentDropdownlist[indexUpperBoundMatch + index] + 1)];
                            indexMatch.Formula = "IFERROR(INDEX(" + workSheetDependentDropdown.Name + "!" + matchIdEnd + ",MATCH(" + executeIndex + "," + workSheetDependentDropdown.Name + "!" + matchNameEnd + ",0)),\"\")";
                            matchIndex += 1;
                        }

                        index++;
                    }


                }
            }
            #endregion

            #region Format Heading
            if (isHeadingHasValue == true)
            {
                int headerIndex = 1;
                int headerColumnIndex = 1;
                int colunmWarning = 2;
                foreach (var itemHeading in heading)
                {
                    workSheet.Cells[headerIndex, headerColumnIndex].Value = itemHeading.Content;

                    if (itemHeading.isWarning == true)
                    {
                        ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, colunmWarning];
                        headerCell.Style.Font.Color.SetColor(System.Drawing.Color.Red);

                        if (itemHeading.isHasBorder == true)
                        {
                            ExcelRange headerBorderCell = workSheet.Cells[headerIndex, colunmWarning + 1];
                            headerBorderCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            headerBorderCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            headerBorderCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            headerBorderCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            headerBorderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerBorderCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                        }

                        if (!string.IsNullOrEmpty(itemHeading.colorCode))
                        {
                            ExcelRange headerCellWithColor = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, colunmWarning + 1];
                            headerCellWithColor.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCellWithColor.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(itemHeading.colorCode));

                            if (itemHeading.isWhiteText == true)
                            {
                                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            }

                        }
                    }
                    else
                    {
                        if (itemHeading.isCode == false)
                        {
                            ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, dataTable.Columns.Count];
                            if (IsMergeCellHeader == true || IsMergeCellHeader == null)
                            {
                                headerCell.Merge = true;
                                if (itemHeading.isHeadingDetail == false)
                                {
                                    headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }
                            if (itemHeading.isHeadingDetail == false)
                            {
                                headerCell.Style.Font.Size = 20;
                            }
                        }
                        if (itemHeading.isTable == true)
                        {
                            ExcelRange headerCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, headerColumnIndex];
                            headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00a65a"));
                            headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);

                            ExcelRange inputCell = workSheet.Cells[headerIndex, headerColumnIndex + 1, headerIndex, headerColumnIndex + 1];
                            inputCell.Style.Locked = false;
                            inputCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            inputCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                            ExcelRange borderCell = workSheet.Cells[headerIndex, headerColumnIndex, headerIndex, headerColumnIndex + 1];
                            borderCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            borderCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            borderCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            borderCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            borderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        }
                    }
                    headerIndex++;
                    workSheet.InsertRow(headerIndex, itemHeading.RowsToIgnore);
                    headerIndex += itemHeading.RowsToIgnore;
                    if (showSrNo)
                    {
                        workSheet.Column(headerColumnIndex).Width = 15;
                    }
                }

            }
            #endregion
            //scroll
            if (!showSrNo)
            {
                workSheet.View.FreezePanes(12, 2);
            }

            //Font Family
            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
            allCells.Style.Font.Name = "Times New Roman";

            //Edit => lock sheet
            if (isEdit == true)
            {
                
            }
        }
    }
}
