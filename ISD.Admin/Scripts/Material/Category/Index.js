$(document).ready(function () {
    getdropdownvalue();
    var companyFromDropdownList = $("#WERKS").val();
    $.ajax({
        type: "POST",
        url: "/Material/Category/GetParentBy",
        data: {
            WERKS: companyFromDropdownList
        },
        dataType: "json",
        success: function (jsonData) {
            var listItems = "<option value = ''> -- Tất cả -- </option>";

            $.each(jsonData, function (i, item) {
                listItems += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            });

            $("#ParentCategory").html(listItems);
        }
    });
    return false;
});

$(document).on("change", "#WERKS", function () {
    loading2();
    var companyFromDropdownList = $(this).val();
    $.ajax({
        type: "POST",
        url: "/Material/Category/GetParentBy",
        data: {
            WERKS: companyFromDropdownList
        },
        dataType: "json",
        success: function (jsonData) {
            var listItems = "<option value = ''> -- Tất cả -- </option>";

            $.each(jsonData, function (i, item) {
                listItems += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            });

            $("#ParentCategory").html(listItems);
            
        }
    });
    return false;
});

function getdropdownvalue() {
    var companyFromDropdownList = $("#WERKS").val();
    var ParentCategory = $("#ParentCategory").val();
    $.ajax({
        type: "POST",
        url: "/Material/Category/_Search",
        data: {
            WERKS: companyFromDropdownList,
            ParentCategory: ParentCategory
        },
        success: function (jsonData) {
            $('#divSearchResult').html(jsonData);
        }
    });
};