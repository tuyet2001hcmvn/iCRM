$(document).ready(function () {

    var companyFromDropdownList = $("#WERKS").val();
    $.ajax({
        type: "POST",
        url: "/Material/Material/GetCategoryBy",
        data: {
            COMPNO: companyFromDropdownList
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
        url: "/Material/Material/GetCategoryBy",
        data: {
            COMPNO: companyFromDropdownList
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

$(document).on("change", "#ParentCategory", function () {
    loading2();
    var companyFromDropdownList = $("#WERKS").val();
    var parent = $(this).val();
    $.ajax({
        type: "POST",
        url: "/Material/Material/GetCategoryChildBy",
        data: {
            COMPNO: companyFromDropdownList,
            TPGRPNM: parent
        },
        dataType: "json",
        success: function (jsonData) {
            var listItems = "<option value = ''> -- Tất cả -- </option>";

            $.each(jsonData, function (i, item) {
                listItems += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            });

            $("#TPGRPNM").html(listItems);
        }
    });
    return false;
});