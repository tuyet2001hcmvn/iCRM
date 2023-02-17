function formatPrice() {
    //Chiết khấu
    var chietKhau = $("input[name=Discount]").val();
    var loaiChietKhau = parseInt($("#DiscountType").val());
    if (chietKhau) {
        var chietKhauInt = parseInt(chietKhau);
        $("input[name=Discount]").val(chietKhauInt);
    }
}

function TotalPrice() {
    var SubTotal = 0;
    $(".detailList-Total").each(function () {
        var dataRow = $(this).data("row");
        var TotalPrice = $("input[name='detailList[" + dataRow + "].Total']").val();
        if (TotalPrice == "") {
            TotalPrice = 0;
        }
        SubTotal += parseInt(TotalPrice);
    });

    //Nếu có tiền cọc thì cộng vào tổng tiền
    var deposit = parseInt($("#DepositPrice").val());
    if (deposit > 0) {
        SubTotal += deposit;
    }

    var discount = 0;
    //0: VND
    //1: %
    var LoaiChietKhau = parseInt($("#DiscountType").val());
    var ChietKhau = $("input[name='Discount']").val();
    if (ChietKhau) {
        discount = parseInt(ChietKhau);
        if (LoaiChietKhau === 0) {
            $("#discount-accessory-price").html(formatCurrency(discount));
        }
        else {
            var discountPercent = parseInt((discount / 100) * SubTotal);
            $("#discount-accessory-price").html(formatCurrency(discountPercent));
        }
    }

    if (LoaiChietKhau === 0) {
        SubTotal = SubTotal - discount;
    }
    else {
        SubTotal = SubTotal - parseInt((discount / 100) * SubTotal);
    }

    if (SubTotal < 0) {
        SubTotal = 0;
    }

    $("#total").html(formatCurrency(SubTotal));
    $("#TotalPrice").val(SubTotal);
}

//Thêm phụ tùng/phụ kiện vào lưới
function accessory_customAccessoryAction(AccessoryCode, AccessoryName, Plant, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, Index, WarehouseName) {
    var $btn = $(".btn-accessory-confirm");
    $btn.attr("disabled", true);

    var AccessorySaleOrderId = $("#AccessorySaleOrderId").val();

    var inputQuantity = parseInt($("#input-quantity-" + Index).val());
    if (inputQuantity > 0) {
        var frmId = "";
        var isThemMoi = true;

        var $frmEdit = $("body #frmEdit");
        var $frmCreate = $("body #frmCreate");
        if ($frmEdit.length === 0) {
            frmId = "frmCreate";
        }
        else {
            frmId = "frmEdit";
            isThemMoi = false;
        }
        var data = $("#" + frmId).serialize();

        data = data + "&AccessoryCode=" + AccessoryCode;
        data = data + "&AccessoryName=" + AccessoryName;
        data = data + "&Plant=" + Plant;
        data = data + "&WarehouseCode=" + WarehouseCode;
        data = data + "&Quantity=" + inputQuantity;
        data = data + "&Unit=" + Unit;
        data = data + "&Location=" + Location;
        data = data + "&UnitPrice=" + AccessoryPrice;
        data = data + "&AccessorySaleOrderId=" + AccessorySaleOrderId;
        data = data + "&isThemMoi=" + isThemMoi;
        data = data + "&WarehouseName=" + WarehouseName;
        data = data + "&SumOfQuantity=" + Quantity;

        $.ajax({
            type: "POST",
            url: "/Sale/AccessorySaleOrder/InsertAccessory",
            data: data,
            success: function (xhr, status, error) {
                $btn.attr("disabled", false);
                if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                    alertModalPopupOnhand("#divModalOnhandWarning", xhr.Message);
                }
                else {
                    $("#accessoryDetailTable tbody").html(xhr);
                    TotalPrice();
                    //Sau khi thêm => Close 2 modal popup
                    $("#divAccessoryConfirm_" + PreFix).modal("hide");
                    //$('script[src*="SearchAccessoryComponent.js"]').remove();
                }
            },
            error: function (xhr, status, error) {
                $btn.attr("disabled", false);
                $("#divAccessoryConfirm_" + PreFix).modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
        //$("#divAccessoryConfirm_" + PreFix).html("");
        //$("#divAccessoryConfirm_" + PreFix).modal("hide");

        //$("#divAccessorySearch_" + PreFix).html("");
        //$("#divAccessorySearch_" + PreFix).modal("hide");

    }
    else {
        alertModalPopupOnhand("#divModalOnhandWarning", "Vui lòng nhập số lượng lớn hơn 0!");
    }
}

//Autocomplete tìm kiếm và check tồn kho phụ tùng/phụ kiện
function SearchText_AccessoryCode() {
    $("#AccessoryCode2").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Sale/AccessorySaleOrder/GetAccessoryCode",
                data: JSON.stringify({ "AccessoryCode": $("#AccessoryCode2").val() }),
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.AccessoryCodeText, value: item.AccessoryCode, id: item.AccessoryName };
                    }));
                    //response(data);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        },
        //prevent another value except dropdown value
        change: function (event, ui) {
            if (ui.item === null) {
                $("#AccessoryCode2").val("");
                $("#AccessoryName2").val("");
                $("#AccessoryCode2").focus();
            }
        },
        select: function (event, ui) {
            $("#AccessoryCode2").val(ui.item.value);
            $("#AccessoryCode2").prop("disabled", true);
            $("#AccessoryName2").val(ui.item.id);

            $("#Quantity2").focus();

            $.ajax({
                type: "POST",
                url: "/Sale/AccessorySaleOrder/GetOnhandAcessory",
                data: {
                    AccessoryCode: ui.item.value
                },
                success: function (xhr, status, error) {
                    $("#WarehouseCode2").val(xhr.WarehouseCode);
                    $("#SumOfQuantity").val(xhr.SumOfQuantity);
                    $("#Location2").val(xhr.Location);
                    $("#Unit2").val(xhr.Unit);
                    $("#WarehouseName2").val(xhr.WarehouseName);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        }
    });
}

//Xóa dữ liệu autocomplete phụ tùng/phụ kiện
$(document).on("click", "#btn-del-acc-2", function () {
    $("#AccessoryCode2").val("");
    $("#AccessoryCode2").prop("disabled", false);

    $("#AccessoryName2").val("");
    $("#WarehouseCode2").val("");
    $("#SumOfQuantity").val("");
    $("#Location2").val("");
    $("#Unit2").val("");
    $("#WarehouseName2").val("");
});

//Thêm vào lưới phụ tùng/phụ kiện
$(document).on("click", "#btn-add-acc-2", function () {
    var $btn = $(this);
    $btn.button('loading');

    var AccessorySaleOrderId = $("#AccessorySaleOrderId").val();
    var AccessoryCode = $("#AccessoryCode2").val();
    var AccessoryName = $("#AccessoryName2").val();
    var WarehouseCode = $("#WarehouseCode2").val();
    var Location = $("#Location2").val();
    var Unit = $("#Unit2").val();
    var Quantity = parseInt($("#Quantity2").val());
    var WarehouseName = $("#WarehouseName2").val();
    var SumOfQuantity = $("#SumOfQuantity").val();

    if (AccessoryCode === "") {
        $btn.button('reset');
        alertPopup(false, "Vui lòng chọn phụ tùng/phụ kiện trước khi thêm!");
    }
    else {
        if (Quantity > 0) {
            var frmId = "";
            var isThemMoi = true;

            var $frmEdit = $("body #frmEdit");
            var $frmCreate = $("body #frmCreate");
            if ($frmEdit.length === 0) {
                frmId = "frmCreate";
            }
            else {
                frmId = "frmEdit";
                isThemMoi = false;
            }
            var data = $("#" + frmId).serialize();

            data = data + "&AccessoryCode=" + AccessoryCode;
            data = data + "&AccessoryName=" + AccessoryName;
            data = data + "&WarehouseCode=" + WarehouseCode;
            data = data + "&Quantity=" + Quantity;
            data = data + "&Unit=" + Unit;
            data = data + "&Location=" + Location;
            data = data + "&AccessorySaleOrderId=" + AccessorySaleOrderId;
            data = data + "&isThemMoi=" + isThemMoi;
            data = data + "&WarehouseName=" + WarehouseName;
            data = data + "&SumOfQuantity=" + SumOfQuantity;

            $.ajax({
                type: "POST",
                url: "/Sale/AccessorySaleOrder/InsertAccessory2",
                data: data,
                success: function (xhr, status, error) {
                    $btn.button('reset');
                    if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                        alertPopup(false, xhr.Message);
                    }
                    else {
                        $("#accessoryDetailTable tbody").html(xhr);
                        TotalPrice();

                        $("#AccessoryCode2").val("");
                        $("#AccessoryCode2").prop("disabled", false);

                        $("#AccessoryName2").val("");
                        $("#WarehouseCode2").val("");
                        $("#SumOfQuantity").val("");
                        $("#Location2").val("");
                        $("#Unit2").val("");
                        $("#Quantity2").val("");
                        $("#WarehouseName2").val("");

                        $("#AccessoryCode2").focus();
                    }
                },
                error: function (xhr, status, error) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                }
            });
        }
        else {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng lớn hơn 0!");
        }
    }
});

//Nếu là đơn tiền cọc
function DisabledDepositWhenChangeType() {
    var value = $("#AccessorySellTypeId").val();

    //Nếu là đơn hàng bán lẻ thì KHÔNG cho "Ghi chú tiền cọc"
    if (value == "3ffa9750-a34f-4bfe-a686-f9335728e302") {
        $("#DepositPrice").val("");
        $("#DepositPrice").prop("disabled", true);
    }
    //Nếu là đơn hàng đặt cọc thì cho "Ghi chú tiền cọc"
    else {
        TotalPrice();
        $("#DepositPrice").prop("disabled", false);
    }
}

$(document).on("change", "#AccessorySellTypeId", function () {
    DisabledDepositWhenChangeType();
});

function SetValueByDepositPrice() {
    var value = parseInt($("#DepositPrice").val());
    if (value > 0) {
        $("#DepositTotalPrice").html(formatCurrency(value));
        DisabledChietKhau();
    }
    else {
        $("#DepositTotalPrice").html("");
        EnabledChietKhau();
    }

    TotalPrice();
}

//Tính thành tiền đặt cọc dựa vào đơn giá nhập
$(document).on("keyup", "#DepositPrice", function (e) {
    if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57) && (e.which < 96 || e.which > 105) && e.which !== 231) {
        $("#DepositPrice").val("");
        $("#DepositTotalPrice").html("");
    }
    else {
        SetValueByDepositPrice();
    }
});
$(document).on("change", "#DepositPrice", function () {
    SetValueByDepositPrice();
});

//$('.modal').on('hidden.bs.modal', function () {
//    $(".modal-backdrop").remove();
//});
$(document).on("shown.bs.modal", ".modal", function () {
    if ($(".modal-backdrop").length > 1) {
        $(".modal-backdrop").not(':first').remove();
    }
})
$(document).on("click", ".modal-backdrop", function () { $(this).remove() });

//Hiển thị modal popup cập nhật số lượng phụ tùng/phụ kiện
$(document).on("click", ".btn-edit-acc", function () {
    var $btn = $(this)
    $btn.button("loading");

    var AccessorySaleOrderDetailId = $(this).data("id");

    var frmId = "";
    var isThemMoi = true;

    var $frmEdit = $("body #frmEdit");
    var $frmCreate = $("body #frmCreate");
    if ($frmEdit.length === 0) {
        frmId = "frmCreate";
    }
    else {
        frmId = "frmEdit";
        isThemMoi = false;
    }
    var data = $("#" + frmId).serialize();

    var STT = $(this).data("row");
    data = data + "&STT=" + STT;
    data = data + "&AccessorySaleOrderDetailId=" + AccessorySaleOrderDetailId;
    data = data + "&isThemMoi=" + isThemMoi;

    $.ajax({
        type: "POST",
        url: "/Sale/AccessorySaleOrder/_AccessoryModal",
        data: data,
        success: function (xhr, status, error) {
            $btn.button("reset");

            $("#modalEditAccessory").html("");
            $("#modalEditAccessory").html(xhr);
            //Sau khi thêm => Close 2 modal popup
            $("#modalEditAccessory").modal("show");
        },
        error: function (xhr, status, error) {
            $btn.button("reset");
            alertPopup(false, xhr.responseText);
        }
    });
});
//Xác nhận cập nhật
$(document).on("click", "#btn-update-accessory", function () {
    var $btn = $(this)
    $btn.button("loading");

    var AccessorySaleOrderDetailId = $("#modalEditAccessory #AccessorySaleOrderDetailId").val();
    var AccessorySellTypeId = $("#AccessorySellTypeId").val();

    var frmId = "";
    var isThemMoi = true;

    var $frmEdit = $("body #frmEdit");
    var $frmCreate = $("body #frmCreate");
    if ($frmEdit.length === 0) {
        frmId = "frmCreate";
    }
    else {
        frmId = "frmEdit";
        isThemMoi = false;
    }
    var data = $("#" + frmId).serialize();

    var STT = $("#modalEditAccessory input[name='STT']").val();
    var Quantity = parseInt($("#modalEditAccessory input[name='Quantity']").val());
    data = data + "&STT=" + STT;
    data = data + "&Quantity=" + Quantity;
    data = data + "&AccessorySaleOrderDetailId=" + AccessorySaleOrderDetailId;
    data = data + "&isThemMoi=" + isThemMoi;
    data = data + "&AccessorySellTypeId=" + AccessorySellTypeId;

    if (Quantity > 0) {
        $.ajax({
            type: "POST",
            url: "/Sale/AccessorySaleOrder/UpdateAccessory",
            data: data,
            success: function (xhr, status, error) {
                if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                    alertModalPopupOnhand("#divModalAccessoryWarning", xhr.Message);
                    $btn.button("reset");
                }
                else {
                    $btn.button("reset");
                    $("#modalEditAccessory").modal("hide");

                    $("#accessoryDetailTable tbody#accessoryDetailList").html("");
                    $("#accessoryDetailTable tbody#accessoryDetailList").html(xhr);
                    TotalPrice();
                }
            },
            error: function (xhr, status, error) {
                $btn.button("reset");
                $("#modalEditAccessory").modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        alertModalPopupOnhand("#divModalAccessoryWarning", "Vui lòng nhập số lượng lớn hơn 0!");
    }
});

//Xóa phụ tùng/phụ kiện khỏi lưới
$(document).on("click", ".btn-del-acc", function () {
    var AccessorySaleOrderDetailId = $(this).data("id");

    var frmId = "";
    var isThemMoi = true;

    var $frmEdit = $("body #frmEdit");
    var $frmCreate = $("body #frmCreate");
    if ($frmEdit.length === 0) {
        frmId = "frmCreate";
    }
    else {
        frmId = "frmEdit";
        isThemMoi = false;
    }
    var data = $("#" + frmId).serialize();

    var STT = $(this).data("row");
    data = data + "&STT=" + STT;
    data = data + "&AccessorySaleOrderDetailId=" + AccessorySaleOrderDetailId;
    data = data + "&isThemMoi=" + isThemMoi;

    $.ajax({
        type: "POST",
        url: "/Sale/AccessorySaleOrder/DeleteAccessory",
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                alertPopup(false, xhr.Message);
            }
            else {
                $("#accessoryDetailTable tbody#accessoryDetailList").html("");
                $("#accessoryDetailTable tbody#accessoryDetailList").html(xhr);
                TotalPrice();
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//Lấy hàng (đối với đơn hàng đặt cọc)
$(document).on("click", "#btn-get-accessory", function () {
    $("#divConfirmGetAccessoryPopup").modal("show");
});
//Confirm Có
$(document).on("click", "#btn-confirm-yes", function () {
    var $btn = $(this)
    $btn.button("loading");

    var data = $("#frmEdit").serialize();

    var AccessorySaleOrderId = $("#AccessorySaleOrderId").val();

    $.ajax({
        type: "POST",
        url: "/Sale/AccessorySaleOrder/GetDepositAccessory",
        data: data,
        success: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmGetAccessoryPopup").modal("hide");
            if (xhr.Success === true) {
                window.location.href = "/Sale/AccessorySaleOrder/Edit/" + AccessorySaleOrderId + "?message=" + xhr.Data;
                //alertPopup(true, xhr.Data);
                $("#btn-get-accessory").hide();
            }
            else {
                alertPopup(false, xhr.Data);
            }
        },
        error: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmGetAccessoryPopup").modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//Xác nhận đơn hàng
$(document).on("click", "#btn-confirm-order", function () {
    var AccessorySaleOrderCode = $("#AccessorySaleOrderCode").val();

    $("#divConfirmSaleOrderCompletePopup form#frmConfirm .alert-message").html("Bạn có chắc muốn xác nhận đơn hàng " + AccessorySaleOrderCode + " không?");
    $("#divConfirmSaleOrderCompletePopup").modal("show");
});
//Confirm Có
$(document).on("click", "#btn-confirm-complete", function () {
    var $btn = $(this)
    $btn.button("loading");

    var AccessorySaleOrderId = $("#AccessorySaleOrderId").val();
    var ConsultEmployeeCode = $("#ConsultEmployeeCode").val();
    var Note = $("#Note").val();

    $.ajax({
        type: "POST",
        url: "/Sale/AccessorySaleOrder/ConfirmSaleOrderComplete",
        data: {
            AccessorySaleOrderId: AccessorySaleOrderId,
            ConsultEmployeeCode: ConsultEmployeeCode,
            Note: Note
        },
        success: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmSaleOrderCompletePopup").modal("hide");
            if (xhr.Success === true) {
                window.location.href = "/Sale/AccessorySaleOrder/Edit/" + AccessorySaleOrderId + "?message=" + xhr.Data;
                //alertPopup(true, xhr.Data);
            }
            else {
                alertPopup(false, xhr.Data);
            }
        },
        error: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmSaleOrderCompletePopup").modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//Hủy đơn hàng
$(document).on("click", "#btn-delete-order", function () {
    var AccessorySaleOrderCode = $("#AccessorySaleOrderCode").val();

    $("#divConfirmDeleteOrderPopup form#frmConfirmDel .alert-message").html('<div class="form-group text-center"><div class="row" style="margin-bottom:10px;">Bạn có chắc chắn muốn hủy đơn hàng <b>"' + AccessorySaleOrderCode + '"</b>? </div> <div class="row"><div class="col-md-4 text-right"><div class="label-wrapper"><label class="control-label" for="CanceledNote">Lý do hủy</label></div></div><div class="col-md-6"><textarea name="CanceledNote" style="width:100%;height:70px;"></textarea></div></div></div>');
    $("#divConfirmDeleteOrderPopup").modal("show");
});
//Confirm Có
$(document).on("click", "#btn-confirm-del", function () {
    var $btn = $(this)
    $btn.button("loading");

    var AccessorySaleOrderId = $("#AccessorySaleOrderId").val();
    var CanceledNote = $("textarea[name='CanceledNote']").val();

    $.ajax({
        type: "POST",
        url: "/Sale/AccessorySaleOrder/CancelAccessorySaleOrder",
        data: {
            AccessorySaleOrderId: AccessorySaleOrderId,
            CanceledNote: CanceledNote,
        },
        success: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmDeleteOrderPopup").modal("hide");
            if (xhr.Success == true) {
                window.location.href = "/Sale/AccessorySaleOrder/Edit/" + AccessorySaleOrderId + "?message=" + xhr.Data;
                //alertPopup(true, xhr.Data);
            }
            else {
                alertPopup(false, xhr.Data);
            }
        },
        error: function (xhr, status, error) {
            $btn.button("reset");
            $("#divConfirmDeleteOrderPopup").modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
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
function alertModalPopupOnhand(divSelector, message) {
    setModalMessage(divSelector, message);
    setTimeout(function () {
        $(divSelector).show();
    }, 500)
}

function DecimalSeparator() {
    //var n = 1.1;
    //n = n.toLocaleString().substring(1, 2);
    //return n;

    var decSep = ".";

    try {
        // this works in FF, Chrome, IE, Safari and Opera
        var sep = parseFloat(3 / 2).toLocaleString().substring(1, 2);
        if (sep === '.' || sep === ',') {
            decSep = sep;
        }
    } catch (e) {
        console.log(e);
    }

    return decSep;
}

function DisabledChietKhau() {
    //Disabled phần chiết khấu
    $("#Discount").val("");
    $("#Discount").prop("disabled", true);
    $("#DiscountType").prop("disabled", true);
    $("#DiscountNote").val("");
    $("#DiscountNote").prop("disabled", true);
}

function EnabledChietKhau() {
    //Enabled phần chiết khấu
    $("#Discount").prop("disabled", false);
    $("#DiscountType").prop("disabled", false);
    $("#DiscountNote").prop("disabled", false);
}

function SetValueByDiscountPrice() {
    var value = parseInt($("#DepositPrice").val());
    if (value > 0) {
        $("#DepositTotalPrice").html(formatCurrency(value));
    }
    else {
        $("#DepositTotalPrice").html("");
    }

    TotalPrice();
}

//Chiết khấu
$(document).on("keyup", "#Discount", function (e) {
 
    var chietKhau = $(this).val();
    var chietKhauInt = parseInt(chietKhau);
    if (chietKhau) {
        //Disabled field đặt cọc
        DisabledDatCoc();
    }
    else {
        $("#discount-accessory-price").html("");
        var discountNote = $("#DiscountNote").val();
        if (!discountNote) {
            EnabledDatCoc();
        }
    }
    TotalPrice();
});

$(document).on("keydown", "input[name=Discount]", function (event) {
    if (event.shiftKey === true) {
        event.preventDefault();
    }

    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
        event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 37 ||
        event.keyCode === 39 || event.keyCode === 46 || event.keyCode === 190) {
        console.log();
    } else {
        event.preventDefault();
    }

    if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
        event.preventDefault();

});

//Ghi chú chiết khấu
$(document).on("keyup", "#DiscountNote", function (e) {
    var value = $(this).val();
    if (value) {
        DisabledDatCoc();
    }
    else {
        var discount = $("#Discount").val();
        if (!discount) {
            EnabledDatCoc();
        }
    }
});

$(document).on("change", "#DiscountType", function (e) {
    $("#Discount").val("");
    $("#discount-accessory-price").html("");
    TotalPrice();
});

function DisabledDatCoc() {
    //Disabled phần đặt cọc
    $("#DepositPrice").val("");
    $("#DepositPrice").prop("disabled", true);
}

function EnabledDatCoc() {
    //Enabled phần đặt cọc
    $("#DepositPrice").prop("disabled", false);
}

function DisabledWhenKeyUp() {
    var discount = $("#Discount").val();
    var discountNote = $("#DiscountNote").val();
    var depositPrice = $("#DepositPrice").val();
    if (discount || discountNote) {
        DisabledDatCoc();
    }
    else if (depositPrice) {
        DisabledChietKhau();
    }
}