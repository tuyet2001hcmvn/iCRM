using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    // Format excel columns
    public class ExcelTemplate
    {
        public string ColumnName { get; set; }
        public bool isAllowedToEdit { get; set; }
        public bool isDateTime { get; set; }
        public bool isDateTimeTime { get; set; }
        public bool isCurrency { get; set; }
        public bool isPercent { get; set; }
        public bool isBoolean { get; set; }
        public bool isGender { get; set; }
        public bool isDetail { get; set; }
        public bool isDependentDropdown { get; set; }
        public string TypeId { get; set; }
        public string DependentDropdownSheetName { get; set; }
        public string DependentDropdownFormula { get; set; }
        public bool isDifferentColorHeader { get; set; }
        public string ColorHeader { get; set; }
        public bool isWraptext { get; set; }
        public int CustomWidth { get; set; }
        public bool hasAnotherName { get; set; }
        public string AnotherName { get; set; }
        public bool hasNote { get; set; }
        public string Note { get; set; }

        public bool isTotal { get; set; }
        public bool isNumber { get; set; }

        //Dropdownlist
        public bool isDropdownlist { get; set; }
        public List<DropdownModel> DropdownData { get; set; }
        public List<DropdownIdTypeIntModel> DropdownIdTypeIntData { get; set; }
        public List<DropdownIdTypeStringModel> DropdownIdTypeStringData { get; set; }
        public List<DropdownIdTypeBoolModel> DropdownIdTypeBoolData { get; set; }

        //Complex header
        public string MergeHeaderTitle { get; set; }

        

    }
    public class ExcelHeadingTemplate
    {
        public string Content { get; set; }
        public int RowsToIgnore { get; set; }
        public bool isWarning { get; set; }
        public bool isHasBorder { get; set; }
        public bool isCode { get; set; }
        public bool isHeadingDetail { get; set; }
        public bool? isTable { get; set; }
        public string colorCode { get; set; }
        public bool isWhiteText { get; set; }
    }

    public class DropdownModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? ParentLevel1Id { get; set; }
        public string ParentLevel1Name { get; set; }
        public Guid? ParentLevel2Id { get; set; }
        public string ParentLevel2Name { get; set; }
    }

    public class DropdownIdTypeIntModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OrderIndex { get; set; }
    }

    public class DropdownIdTypeStringModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DropdownIdTypeBoolModel
    {
        public bool Id { get; set; }
        public string Name { get; set; }
    }

    public class DropdownListModel
    {
        public string Type { get; set; }
        public int Index { get; set; }
    }
}
