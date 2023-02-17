
$(document).on("click", "#btn-material-search_" + ViewBagPreFix, function () {
    var $btn = $("#btn-search");
    $btn.button('loading');

    //var SearchMaterialCode = $("#divMaterialSearch_" + ViewBagPreFix + " #SearchMaterialCode").val();
    //var minCharacter = ViewBagMinCharacter;
    //if (SearchMaterialCode.length < minCharacter) {
    //    $btn.button('reset');
    //    alertModalPopup_Material(false, "Vui lòng nhập tối thiểu 5 ký tự Mã xe!")
    //}
    //else {
        
    //}

    var isPopup = ViewBagisPopup == "True" ? true : false;
    var data = $("#frmSearchPopup_" + ViewBagPreFix).serialize();
    data = data + "&isPopup=" + isPopup;
    data = data + "&PreFix=" + "" + ViewBagPreFix;

    $.ajax({
        type: "POST",
        url: "/Sale/Material/_MaterialSearchResult",
        data: data,
        success: function (xhr, status, error) {
            $btn.button('reset');
            if (xhr.Code == 500) {
                //error
                $("#divMaterialSearch_" + ViewBagPreFix).modal("hide");
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divSearchPopupResult_" + ViewBagPreFix).html(xhr);
                ISD.Pagging();
            }
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            $("#divMaterialSearch_" + ViewBagPreFix).modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

$(document).on("click", ".btn-material-choose_" + ViewBagPreFix, function () {
    var chooseBtnSelector = "table#materialResultTable_" + ViewBagPreFix + " .btn-material-choose_" + ViewBagPreFix;
    $(chooseBtnSelector).attr("disabled", true);

    //get and fill data to material info
    var MaterialCode = $(this).data("code");
    var MaterialName = $(this).data("name");
    var PreFix = $(this).data("prefix");

    $.ajax({
        type: "POST",
        url: "/Sale/Material/_OnhandMaterialModal",
        data: {
            MaterialCode: MaterialCode,
            MaterialName: MaterialName,
            PreFix: PreFix,
            SaleOrg: ModelSaleOrg
        },
        success: function (xhr, status, error) {
            $(chooseBtnSelector).attr("disabled", false);
            if (xhr.Code == 500) {
                //error
                $("#divMaterialSearch_" + PreFix).modal("hide");
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divMaterialConfirm_" + PreFix).html(xhr);

                //Hiển thị popup tồn kho => close popup Tìm kiếm
                $("#divMaterialSearch_" + PreFix).modal("hide");
                $("#divMaterialConfirm_" + PreFix).modal("show");
            }
        },
        error: function (xhr, status, error) {
            $(chooseBtnSelector).attr("disabled", false);
            $("#divMaterialSearch_" + PreFix).modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//Close popup tồn kho => hiển thị lại popup Tìm kiếm Xe
$('#divMaterialSearch_' + ViewBagPreFix).on('hidden.bs.modal', function () {
    $("#divMaterialSearch_" + ViewBagPreFix + " #SearchMaterialCode").val("");
});

//$('#divMaterialSearch_' + ViewBagPreFix).on('shown.bs.modal', function () {
//    $("#btn-material-search_" + ViewBagPreFix).trigger("click");
//});

function alertModalPopup_Material(isSuccess, message) {
    if (isSuccess == true) {
        setModalMessage("#divModalAlertSuccess_Material_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertSuccess_Material_' + ViewBagPreFix).show();
        }, 500)
        setTimeout(function () {
            $('#divModalAlertSuccess_Material_' + ViewBagPreFix).hide();
        }, 3000)
    }
    else if (isSuccess == false) {
        setModalMessage("#divModalAlertWarning_Material_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertWarning_Material_' + ViewBagPreFix).show();
        }, 500)
    }
}