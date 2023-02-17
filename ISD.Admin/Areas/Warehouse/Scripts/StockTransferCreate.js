//Autocomple kho chuyển xuất
function SearchText_FromStockCode() {
    $("#FromStockCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Warehouse/Stock/GetStockForAutocomple",
                data: JSON.stringify({ "SearchText": $("#FromStockCode").val() }),
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.StockCodeText, id: item.StockId, value: item.StockCode, code: item.StockName };
                    }));
                    //response(data);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        },
        autoFocus: true,
        //prevent another value except dropdown value
        change: function (event, ui) {
            if (ui.item === null) {
                $("#FromStockCode").val("");
                $("#FromStockName").val("");
                $("#FromStockId").val("");
                $("#FromStockCode").focus();
            }
        },
        select: function (event, ui) {

            $("#FromStockCode").val(ui.item.value);
            $("#FromStockCode").prop("disabled", true);
            $("#FromStockName").val(ui.item.code);
            $("#FromStockId").val(ui.item.id);
            $("#ToStockCode").focus();

            //Tab or enter auto select first (AutoFocus = true)
            if (event.keyCode === 9 || event.keyCode === 13) {
                $("#FromStockCode").val(ui.item.value);
                event.preventDefault();
            }
        }
    });
}

//Autocomlpe kho chuyển nhập
function SearchText_ToStockCode() {
    $("#ToStockCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Warehouse/Stock/GetToStockForAutocomple",
                data: JSON.stringify({ "SearchText": $("#ToStockCode").val(), "FromStockId": $("#FromStockId").val() }),
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.StockCodeText, id: item.StockId, value: item.StockCode, code: item.StockName };
                    }));
                    //response(data);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        },
        autoFocus: true,
        //prevent another value except dropdown value
        change: function (event, ui) {
            if (ui.item === null) {
                $("#ToStockCode").val("");
                $("#ToStockName").val("");
                $("#ToStockId").val("");
                $("#ToStockCode").focus();
            }
        },
        select: function (event, ui) {
            $("#ToStockCode").val(ui.item.value);
            $("#ToStockCode").prop("disabled", true);
            $("#ToStockName").val(ui.item.code);
            $("#ToStockId").val(ui.item.id);
            $("#ProductCode").focus();

            //Tab or Enter auto select first (autofocus = true)
            if (event.keyCode === 9 || event.keyCode === 13) {
                $("#ToStockCode").val(ui.item.value);
                event.preventDefault();
            }
        }
    });
}

//Autocomple sản phẩm
function SearchText_ProductCode() {
    $("#ProductCode").autocomplete({
        source: function (request, response) {
            var stockId = $("#FromStockId").val();
            if (stockId == "") {
                alertPopup(false, "Vui lòng chọn kho xuất trước!");
            } else {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Sale/Product/SearchProductByCodeOrName",
                    data: JSON.stringify({ "SearchText": $("#ProductCode").val() }),
                    dataType: "json",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.ProductSearchName, id: item.ProductId, value: item.ProductCode, code: item.ProductName };
                        }));
                        //response(data);
                    },
                    error: function (xhr, status, error) {
                        alertPopup(false, xhr.responseText);
                    }
                });
            }
        },
        autoFocus: true,
        //prevent another value except dropdown value
        change: function (event, ui) {
            if (ui.item === null) {
                $("#ProductCode").val("");
                $("#ProductName").val("");
                $("#ProductId").val("");
                $("#ProductQuantinyOld").val("");
                $("#ProductCode").focus();
            }
        },
        select: function (event, ui) {
            $("#ProductCode").val(ui.item.value);
            $("#ProductCode").prop("disabled", true);
            $("#ProductId").val(ui.item.id);
            $("#ProductName").val(ui.item.code);
            $("#ProductQuantity").focus();
            $.ajax({
                type: "POST",
                url: "/Warehouse/StockTransfer/GetProducOnHand",
                data: {
                    ProductId: ui.item.id,
                    StockId: $("#FromStockId").val(),
                },
                success: function (xhr, status, error) {
                    $("#ProductQuantinyOnHand").val(xhr.Qty);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });

            //tab or enter auto select first (autofocus = true)
            if (event.keyCode === 9 || event.keyCode === 13) {
                event.preventDefault();
                $("#ProductCode").val(ui.item.value);
            }
        }
    });
}

//Xóa dữ liệu autocomplete
$(document).on("click", "#btn-del-product", function () {
    $("#ProductCode").val("");
    $("#ProductCode").prop("disabled", false);
    $("#FromStockCode").val("");
    $("#FromStockCode").prop("disabled", false);
    $("#ToStockCode").val("");
    $("#ToStockCode").prop("disabled", false);

    $("#ProductId").val("");
    $("#FromStockId").val("");
    $("#ToStockId").val("");
    $("#ProductName").val("");
    $("#FromStockName").val("");
    $("#ToStockName").val("");
    $("#ProductQuantity").val("");
    $("#ProductQuantinyOnHand").val("");
    $("#ProductStockNote").val("");
    $("#FromStockCode").focus();
});

function ClearProductData() {
    $("#ProductCode").val("");
    $("#ProductCode").prop("disabled", false);
    //$("#FromStockCode").val("");
    //$("#FromStockCode").prop("disabled", false);
    //$("#ToStockCode").val("");
    //$("#ToStockCode").prop("disabled", false);

    $("#ProductId").val("");
    //$("#FromStockId").val("");
    //$("#ToStockId").val("");
    $("#ProductName").val("");
    //$("#FromStockName").val("");
    //$("#ToStockName").val("");
    $("#ProductQuantity").val("");
    $("#ProductQuantinyOnHand").val("");
    $("#ProductStockNote").val("");

    $("#ProductCode").focus();
}

//Thêm vào lưới thông tin sản phẩm
$(document).on("click", "#btn-add-stockReceivingDetail", function () {
    var $btn = $(this);
    $btn.button('loading');

    var ProductId = $("#ProductId").val();
    var ProductCode = $("#ProductCode").val();
    var ProductName = $("#ProductName").val();
    var ProductQuantity = $("#ProductQuantity").val();
    var ProductQuantinyOnHand = $("#ProductQuantinyOnHand").val();
    var ProductPrice = $("#ProductPrice").val();
    var FromStockId = $("#FromStockId").val();
    var FromStockCode = $("#FromStockCode").val();
    var FromStockName = $("#FromStockName").val();
    var ToStockId = $("#ToStockId").val();
    var ToStockCode = $("#ToStockCode").val();
    var ToStockName = $("#ToStockName").val();
    var ProductStockNote = $("#ProductStockNote").val();

    if (ProductCode === "") {
        $btn.button('reset');
        alertPopup(false, "Vui lòng chọn sản phẩm trước khi thêm!");
    }
    else {
        if (ProductQuantity == "" || parseInt(ProductQuantity) < 0) {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng lớn hơn 0!");
        }
        else if (ProductQuantinyOnHand == "" || parseInt(ProductQuantinyOnHand) < parseInt(ProductQuantity)) {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng chuyển kho không vượt quá số lượng tồn!");
        }
        else {
            var frmId = "frmCreate";
            var $frmCreate = $("body #frmCreate");
            var data = $("#" + frmId).serialize();

            data = data + "&ProductId=" + ProductId;
            data = data + "&ProductCode=" + ProductCode;
            data = data + "&ProductName=" + ProductName;
            data = data + "&Quantity=" + ProductQuantity;
            data = data + "&QuantinyOnHand=" + ProductQuantinyOnHand;
            data = data + "&Price=" + ProductPrice;
            data = data + "&FromStockId=" + FromStockId;
            data = data + "&FromStockCode=" + FromStockCode;
            data = data + "&FromStockName=" + FromStockCode + " | " + FromStockName;
            data = data + "&ToStockId=" + ToStockId;
            data = data + "&ToStockCode=" + ToStockCode;
            data = data + "&ToStockName=" + ToStockCode + " | " + ToStockName;
            data = data + "&DetailNote=" + ProductStockNote;

            $.ajax({
                type: "POST",
                url: "/Warehouse/StockTransfer/InsertProductStock",
                data: data,
                success: function (xhr, status, error) {
                    $btn.button('reset');
                    if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                        alertPopup(false, xhr.Message);
                    }
                    else {
                        $("#transferDetailTable tbody").html(xhr);
                        //TotalPrice();

                        //$("#btn-del-product").trigger("click");
                        ClearProductData();
                    }
                },
                error: function (xhr, status, error) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                }
            });

        }
    }
});


//Xóa sản phẩm khỏi lưới
$(document).on("click", ".btn-del-proDetail", function () {

    var frmId = "";
    frmId = "frmCreate";
    var data = $("#" + frmId).serialize();
    var STT = $(this).data("row");
    data = data + "&STT=" + STT;

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockTransfer/RemoveProductStock",
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                alertPopup(false, xhr.Message);
            }
            else {
                $("#transferDetailTable tbody#transferDetailList").html("");
                $("#transferDetailTable tbody#transferDetailList").html(xhr);
                //TotalPrice();
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//enter add product to grid
//$("#ProductQuantity").keyup(function (event) {
//    if (event.keyCode === 13) {
//        //$("#btn-add-stockReceivingDetail").click();
//        $("#ProductStockNote").focus();
//    }
//});
$("#ProductStockNote").keyup(function (event) {
    if (event.keyCode === 13) {
        $("#btn-add-stockReceivingDetail").click();
    }
});
$("#btn-add-stockReceivingDetail").keyup(function (event) {
    if (event.keyCode === 13 || event.keyCode === 32) {
        $("#btn-add-stockReceivingDetail").click();
    }
}); 