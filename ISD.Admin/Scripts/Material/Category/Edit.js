$(document).on("change", "#WERKS", function () {
    var companyFromDropdownList = $(this).val();
    $.ajax({
        type: "POST",
        url: "/Material/Category/GetParentBy",
        data: {
            WERKS: companyFromDropdownList
        },
        dataType: "json",
        success: function (jsonData) {
            var listItems = "";

            $.each(jsonData, function (i, item) {
                listItems += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            });

            $("#ParentCategory").html(listItems);
        }
    });
    return false;
});