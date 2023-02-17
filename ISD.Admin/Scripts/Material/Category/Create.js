$(document).ready(function () {
    var companyFromDropdownList = $("#WERKS").val();
    $.ajax({
        type: "POST",
        url: "/Material/Category/GetParentBy",
        data: {
            WERKS: companyFromDropdownList
        },
        dataType: "json",
        success: function (jsonData) {
            var listItems = "<option> -- Chọn nhóm sản phẩm cha (nếu có) -- </option>";

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
            var listItems = "<option> -- Chọn nhóm sản phẩm cha (nếu có) -- </option>";

            $.each(jsonData, function (i, item) {
                listItems += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            });

            $("#ParentCategory").html(listItems);
        }
    });
    return false;
});