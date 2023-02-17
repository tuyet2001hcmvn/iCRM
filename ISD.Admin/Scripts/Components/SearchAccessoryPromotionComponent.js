
//button Tìm kiếm
$(document).on("click", "#btn-accessoryPromotion-search_" + ViewBagPreFix, function () {
    var $btn = $(this);
    $btn.button('loading');

    var isPopup = ViewBagisPopup === "True" ? true : false;
    var data = $("#frmSearchPopup_AccessoryPromotion_" + ViewBagPreFix).serialize();
    data = data + "&isPopup=" + isPopup;
    data = data + "&prefix=" + ViewBagPreFix;
    data = data + "&SaleOrderMasterId=" + ViewBagSaleOrderMasterId;
    data = data + "&SearchPromotionId=" + ViewBagSearchPromotionId;

    $.ajax({
        type: "POST",
        url: "/Sale/Accessory/_AccessoryPromotionSearchResult",
        data: data,
        success: function (xhr, status, error) {
            $btn.button('reset');
            if (xhr.Code === 500) {
                //error
                $("#divAccessoryPromotionSearch_" + ViewBagPreFix).modal("hide");
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divSearchPopupResult_AccessoryPromotion_" + ViewBagPreFix).html(xhr);
                ISD.Pagging();
            }
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            $("#divAccessoryPromotionSearch_" + ViewBagPreFix).modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//button Xem
$(document).on("click", ".btn-accessoryPromotion-choose_" + ViewBagPreFix, function () {
    var chooseBtnSelector = "table#resultPromotionTable_" + ViewBagPreFix + " .btn-accessoryPromotion-choose_" + ViewBagPreFix;
    $(chooseBtnSelector).attr("disabled", true);

    //get and fill data to accessory info
    var AccessoryCode = $(this).data("code");
    var AccessoryName = $(this).data("name");
    var Unit = $(this).data("unit");
    var PreFix = $(this).data("prefix");

    $.ajax({
        type: "POST",
        url: "/Sale/Accessory/_OnhandAccessoryPromotionModal",
        data: {
            AccessoryCode: AccessoryCode,
            AccessoryName: AccessoryName,
            Unit: Unit,
            PreFix: PreFix,
            SaleOrg: ModelSaleOrg
        },
        success: function (xhr, status, error) {
            $(chooseBtnSelector).attr("disabled", false);
            if (xhr.Code === 500) {
                //error
                $("#divAccessoryPromotionSearch_" + PreFix).modal("hide");
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divAccessoryPromotionConfirm_" + PreFix).html(xhr);

                //Hiển thị popup tồn kho => close popup Tìm kiếm
                $("#divAccessoryPromotionSearch_" + PreFix).modal("hide");
                $("#divAccessoryPromotionConfirm_" + PreFix).modal("show");
            }
        },
        error: function (xhr, status, error) {
            $(chooseBtnSelector).attr("disabled", false);
            $("#divAccessoryPromotionSearch_" + PreFix).modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//Close popup tồn kho => xóa mã phụ tùng/phụ kiện/công việc
$('#divAccessoryPromotionSearch_' + ViewBagPreFix).on('hidden.bs.modal', function () {
    $("#divAccessoryPromotionSearch_" + ViewBagPreFix + " #SearchAccessoryCode").val("");
});

//$('#divAccessorySearch_' + ViewBagPreFix).on('shown.bs.modal', function () {
//    $("#btn-accessory-search_" + ViewBagPreFix).trigger("click");
//});

//alert popup inside modal popup
function setModalMessage_Promotion(div, message) {
    if (Array.isArray(message)) {
        var arr = [];
        $.each(message, function (i, item) {
            arr[i] = { err: item }
            $(div + " .modal-alert-message").append("<li>" + arr[i].err + "</li>");
        });
    }
    else {
        $(div + " .modal-alert-message").html(message);
    }
}
function alertModalPopupPromotion(isSuccess, message) {
    if (isSuccess == true) {
        setModalMessage_Promotion("#divModalAlertSuccessPromotion_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertSuccessPromotion_' + ViewBagPreFix).show();
        }, 500)
        setTimeout(function () {
            $('#divModalAlertSuccessPromotion_' + ViewBagPreFix).hide();
        }, 3000)
    }
    else if (isSuccess == false) {
        setModalMessage_Promotion("#divModalAlertWarningPromotion_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertWarningPromotion_' + ViewBagPreFix).show();
        }, 500)
    }
}