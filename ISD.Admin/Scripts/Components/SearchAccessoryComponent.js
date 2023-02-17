//$(document).ready(function () {
    if (isHasRadio === "False") {
        if (SearchAccessoryByCategoryType === 0) {
            $(".modal_accessory").css("display", "block");
            $(".modal_service").css("display", "none");
        }
        else if (SearchAccessoryByCategoryType === 1) {
            $(".modal_accessory").css("display", "none");
            $(".modal_service").css("display", "block");
        }
    }
//});

//button Tìm kiếm
//$(document).on("click", "#btn-accessory-search_" + ViewBagPreFix, function () {
$("#btn-accessory-search_" + ViewBagPreFix).click(function () {
    var $btn = $(this);
    $btn.button('loading');

    var SearchAccessoryCode = $("#divAccessorySearch_" + ViewBagPreFix + " #SearchAccessoryCode").val();
    var minCharacter = ViewBagMinCharacter;
    if (SearchAccessoryCode.length < minCharacter && SearchAccessoryByCategoryType == 0) {
        $btn.button('reset');
        $("#divSearchPopupResult_Accessory_" + ViewBagPreFix).html("");
        alertModalPopup(false, "Vui lòng nhập tối thiểu 5 ký tự Mã phụ tùng/phụ kiện!")
    }
    else {
        var isPopup = ViewBagisPopup === "True" ? true : false;
        var data = $("#frmSearchPopup_Accessory_" + ViewBagPreFix).serialize();
        data = data + "&isPopup=" + isPopup;
        data = data + "&prefix=" + ViewBagPreFix;
        data = data + "&isSale=" + ViewBagisSale;
        data = data + "&isPromo=" + ViewBagisPromo;

        $.ajax({
            type: "POST",
            url: "/Sale/Accessory/_AccessorySearchResult",
            data: data,
            success: function (xhr, status, error) {
                $btn.button('reset');
                if (xhr.Code === 500) {
                    //error
                    $("#divAccessorySearch_" + ViewBagPreFix).modal("hide");
                    alertPopup(false, xhr.Data);
                } else {
                    //success
                    $("#divSearchPopupResult_Accessory_" + ViewBagPreFix).html(xhr);
                    xemClick();
                    ISD.Pagging();
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                $("#divAccessorySearch_" + ViewBagPreFix).modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    }
});
function xemClick() {
    //button Xem
    //$(document).on("click", ".btn-accessory-choose_" + ViewBagPreFix, function () {
    $(".btn-accessory-choose_" + ViewBagPreFix).click(function () {
        var chooseBtnSelector = "table#resultTable_" + ViewBagPreFix + " .btn-accessory-choose_" + ViewBagPreFix;
        $(chooseBtnSelector).attr("disabled", true);

        //get and fill data to accessory info
        var AccessoryCode = $(this).data("code");
        var AccessoryName = $(this).data("name");
        var AccessoryPrice = $(this).data("price");
        var Unit = $(this).data("unit");
        var PreFix = $(this).data("prefix");

        var isTestBaoHanhKhan = false;
        if (PreFix == "accessory_") {
            isTestBaoHanhKhan = true;
        }

        $.ajax({
            type: "POST",
            url: "/Sale/Accessory/_OnhandAccessoryModal",
            data: {
                AccessoryCode: AccessoryCode,
                AccessoryName: AccessoryName,
                AccessoryPrice: AccessoryPrice,
                Unit: Unit,
                PreFix: PreFix,
                SaleOrg: ModelSaleOrg,
                isTestBaoHanhKhan: isTestBaoHanhKhan
            },
            success: function (xhr, status, error) {
                $(chooseBtnSelector).attr("disabled", false);
                if (xhr.Code === 500) {
                    //error
                    $("#divAccessorySearch_" + PreFix).modal("hide");
                    alertPopup(false, xhr.Data);
                } else {
                    //success
                    $("#divAccessoryConfirm_" + PreFix).html(xhr);

                    //Hiển thị popup tồn kho => close popup Tìm kiếm
                    $("#divAccessorySearch_" + PreFix).modal("hide");
                    $("#divAccessoryConfirm_" + PreFix).modal("show");
                }
            },
            error: function (xhr, status, error) {
                $(chooseBtnSelector).attr("disabled", false);
                $("#divAccessorySearch_" + PreFix).modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    });
}


//Close popup tồn kho => xóa mã phụ tùng/phụ kiện/công việc
$('#divAccessorySearch_' + ViewBagPreFix).on('hidden.bs.modal', function () {
    $("#divAccessorySearch_" + ViewBagPreFix + " #SearchAccessoryCode").val("");
});

//$('#divAccessorySearch_' + ViewBagPreFix).on('shown.bs.modal', function () {
//    $("#btn-accessory-search_" + ViewBagPreFix).trigger("click");
//});

//change radio button
//$(document).on("click", "input[name='SearchAccessoryByCategoryType']", function () {
$("input[name='SearchAccessoryByCategoryType']").click(function () {

    var value = $(this).val();
    //Phụ tùng/Phụ kiện: 0
    if (value === "0") {
        $(".modal_accessory").css("display", "block");
        $(".modal_service").css("display", "none");

        $(".label-phukien").show();
        $(".label-congviec").hide();
        SearchAccessoryByCategoryType = 0;
    }
    //Công việc: 1
    else {
        $(".modal_accessory").css("display", "none");
        $(".modal_service").css("display", "block");

        $(".label-phukien").hide();
        $(".label-congviec").show();
        SearchAccessoryByCategoryType = 1;
    }
});

//alert popup inside modal popup
function setModalMessage(div, message) {
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
function alertModalPopup(isSuccess, message) {
    if (isSuccess == true) {
        setModalMessage("#divModalAlertSuccess_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertSuccess_' + ViewBagPreFix).show();
        }, 500)
        setTimeout(function () {
            $('#divModalAlertSuccess_' + ViewBagPreFix).hide();
        }, 3000)
    }
    else if (isSuccess == false) {
        setModalMessage("#divModalAlertWarning_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalAlertWarning_' + ViewBagPreFix).show();
        }, 500)
    }
}


// LƯU Ý: VIÉT LẠI HÀM NÀY ĐỂ OVERIDE XỬ LÝ NÚT THÊM VÀ XÓA
// Đối số phải giống nhau
////Thêm phụ tùng/phụ kiện vào list
//function promotion_customAccessoryAction(AccessoryCode, AccessoryName, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix) {
//    var giftList = [];
//    $("#giftList input[type=hidden]").each(function () {
//        var value = $(this).val();
//        giftList.push(value);
//    });

//    $.ajax({
//        type: "POST",
//        url: "/MasterData/Promotion/InsertAccessory",
//        data: {
//            AccessoryCode: AccessoryCode,
//            AccessoryName: AccessoryName,
//            giftList: giftList
//        },
//        success: function (xhr, status, error) {
//            $("#giftAccessoryTable").html("");
//            $("#giftAccessoryTable").html(xhr);
//            //Sau khi thêm => Close 2 modal popup
//            $("#divAccessoryConfirm_" + PreFix).modal("hide");
//        },
//        error: function (xhr, status, error) {
//            alertPopup(false, xhr.responseText);
//        }
//    });
//    $("#divAccessorySearch_" + PreFix).modal("hide");
//}
////Xóa PT/PK
//function DeleteAccessory(AccessoryCode) {
//    var giftList = [];
//    $("#giftList input[type=hidden]").each(function () {
//        var value = $(this).val();
//        giftList.push(value);
//    });

//    $.ajax({
//        type: "POST",
//        url: "/MasterData/Promotion/DeleteAccessory",
//        data: {
//            AccessoryCode: AccessoryCode,
//            giftList: giftList
//        },
//        success: function (xhr, status, error) {
//            $("#giftAccessoryTable").html("");
//            $("#giftAccessoryTable").html(xhr);
//        },
//        error: function (xhr, status, error) {
//            alertPopup(false, xhr.responseText);
//        }
//    });
//}