function Select2_CustomForList(url, ListProductId) {
    $("select[name='" + ListProductId + "']").select2({

        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            }
            , minimumInputLength: 0 // Tối thiếu 1 kí tự thì mới search

        }
    });
    $("select[name='" + ListProductId + "']").each(function () {
        var dataRow = $(this).data("row");
        var ProducID = $("input[name='detail[" + dataRow + "].ProductId']").val();
        var ProductName = $("input[name='detail[" + dataRow + "].ProductName']").val();
        //console.log(ProducID + " " + ProductName);
        //alert(ProductName.substring(ProductName.indexOf("|") + 2 ) );
        $(this).html("<option value='" + ProducID + "'>" + ProductName + "</option>")
        $("#divProductId_" + dataRow + " .select2-selection__rendered").html("<option value='" + ProducID + "'>" + ProductName + "</option>")

    })
}

function Select2_Custom(url, ListProductId, selectedId, selectedName, divList) {
    $("select[name='" + ListProductId + "']").select2({
        //placeholder: "Hãy chọn 1 giá trị tuỳ ý",
        // allowClear: $("select[name='" + ListProductId + "']").first.select2(0,null),
        allowClear: true,
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            }
            , minimumInputLength: 0 // Tối thiếu 1 kí tự thì mới search

        }
    });
    // $("#divProductId .select2-selection__rendered").html("Hiển thị giá trị mặc định");
    if (selectedId != undefined && selectedName != undefined) {
        if (divList != undefined) {
            $("#" + divList + " select[name='" + ListProductId + "']").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
            $("#" + divList + " .select2-selection__rendered").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
        } else {
            $("select[name='" + ListProductId + "']").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
            $(".select2-selection__rendered").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
        }

    }
}

$(document).on("click", ".select2-selection__clear", function () {
    //$("select[name='ImportMasterId']").empty().trigger('change');
    var str = $(this).parent().attr("id");
    var indexFirst = str.indexOf('-');
    var strNext = str.substring(str.indexOf('-') + 1, 100)
    var indexLast = strNext.indexOf('-');
    var id = str.substring(indexFirst + 1, indexLast + indexFirst + 1);
    //alert(id);
    $("select[name='" + id + "']").empty().trigger('change');
});

function Select2_Custom(url, ListProductId, selectedId, selectedName, divList) {
    $("select[name='" + ListProductId + "']").select2({
        //placeholder: "Hãy chọn 1 giá trị tuỳ ý",
        // allowClear: $("select[name='" + ListProductId + "']").first.select2(0,null),
        allowClear: true,
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page,
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            }
            , minimumInputLength: 0 // Tối thiếu 1 kí tự thì mới search

        }
    });
    // $("#divProductId .select2-selection__rendered").html("Hiển thị giá trị mặc định");
    if (selectedId != undefined && selectedName != undefined) {
        if (divList != undefined) {
            $("#" + divList + " select[name='" + ListProductId + "']").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
            $("#" + divList + " .select2-selection__rendered").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
        } else {
            $("select[name='" + ListProductId + "']").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
            $(".select2-selection__rendered").html("<option value='" + selectedId + "'>" + selectedName + "</option>");
        }

    }
}

function Select2_CustomForList_GH_Accessory(url, ListProductId, MaterialCode) {

    console.log(MaterialCode);
    $("select[name='" + ListProductId + "']").select2({

        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page,
                    MaterialCode: MaterialCode
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.id, text: obj.name };
                    })
                };
            }
            , minimumInputLength: 0 // Tối thiếu 1 kí tự thì mới search

        }
    });
    $("select[name='" + ListProductId + "']").each(function () {
        var dataRow = $(this).data("row");
        var AccessoryCode = $("input[name='detail[" + dataRow + "].AccessoryCode']").val();
        var AccessoryName = $("input[name='detail[" + dataRow + "].AccessoryName']").val();
        if (AccessoryCode) {
            //console.log(ProducID + " " + ProductName);
            //alert(ProductName.substring(ProductName.indexOf("|") + 2 ) );
            $(this).html("<option value='" + AccessoryCode + "'>" + AccessoryCode + " | " + AccessoryName + "</option>")
            $("#divAccessoryCode_" + dataRow + " .select2-selection__rendered").html("<option value='" + AccessoryCode + "'>" + AccessoryCode + " | " + AccessoryName + "</option>")
        }
    })
}

function Select2_CustomForList_GH_AccessoryForService(url, className) {
    $(className).select2({
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                    //MaterialCode: MaterialCode
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.id, text: obj.id + " | " + obj.name }; // text: hiển thị trong select2
                    })
                };
            }
            , minimumInputLength: 1 // Tối thiếu 1 kí tự thì mới search
        }
    });
    $(className).each(function () {
        var dataRow = $(this).data("row");
        var AccessoryCode = $("input[name='detail[" + dataRow + "].AccessoryCode']").val();
        var AccessoryName = $("input[name='detail[" + dataRow + "].AccessoryName']").val();
            //console.log(AccessoryCode + " " + AccessoryName);
        if (AccessoryCode) {
            //alert(ProductName.substring(ProductName.indexOf("|") + 2 ) );
            $(this).html("<option value='" + AccessoryCode + "'>" + AccessoryCode + " | " + AccessoryName + "</option>");
            $("#divAccessoryCode_" + dataRow + " .select2-selection__rendered").html("<option value='" + AccessoryCode + "'>" + AccessoryCode + " | " + AccessoryName + "</option>")
        }
    });
}

function Select2_CustomForList_GH_Promotion(url, ListProductId) {
    $("select[name='" + ListProductId + "']").select2({

        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page,
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.id, text: obj.name };
                    })
                };
            }
            , minimumInputLength: 0 // Tối thiếu 1 kí tự thì mới search

        }
    });
    $("select[name='" + ListProductId + "']").each(function () {
        var dataRow = $(this).data("row");
        var PromotionCode = $("input[name='detailPromotion[" + dataRow + "].PromotionCode']").val();
        var PromotionName = $("input[name='detailPromotion[" + dataRow + "].PromotionName']").val();
        if (PromotionCode) {
            $(this).html("<option value='" + PromotionCode + "'>" + PromotionCode + " | " + PromotionName + "</option>")
            $("#divPromotionCode_" + dataRow + " .select2-selection__rendered").html("<option value='" + PromotionCode + "'>" + PromotionCode + " | " + PromotionName + "</option>")
        }
    })
}
