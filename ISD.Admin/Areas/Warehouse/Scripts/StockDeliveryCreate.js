//Autocomple sản phẩm
function SearchText_ProductCode() {
    $("#ProductCode").autocomplete({
        source: function (request, response) {
            var stockId = $("#StockId").val();
            if (stockId == "") {
                alertPopup(false, "Vui lòng chọn kho nguồn trước!");
            } else {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Sale/Product/SearchProductByCodeOrName",
                    data: JSON.stringify({ "SearchText": $("#ProductCode").val() }),
                    dataType: "json",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.ProductSearchName, id: item.ProductId, value: item.ProductCode, code: item.ProductName, price: item.Price };
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
            $("#ProductPrice").val(ui.item.price);

            $("#ProductPrice").focus();
            $.ajax({
                type: "POST",
                url: "/Warehouse/StockTransfer/GetProducOnHand",
                data: {
                    ProductId: ui.item.id,
                    StockId: $("#StockId").val(),
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
                $("#ProductCode").val(ui.item.value);
                event.preventDefault();
            }
        }
    });
}

//Thêm vào lưới thông tin sản phẩm
$(document).on("click", "#btn-add-deliveryDetail", function () {
    var $btn = $(this);
    $btn.button('loading');

    var ProductId = $("#ProductId").val();
    var ProductCode = $("#ProductCode").val();
    var ProductName = $("#ProductName").val();
    var ProductQuantity = $("#ProductQuantity").val();
    var ProductPrice = $("#ProductPrice").val();
    var StockId = $("#StockId").val();
    var StockCode = $("#StockCode").val();
    var StockName = $("#StockName").val();
    var ProductStockNote = $("#ProductStockNote").val();
    //SL tồn
    var ProductQuantinyOnHand = $("input[name='ProductQuantinyOnHand']").val();

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
            alertPopup(false, "Vui lòng nhập số lưọng xuất không vượt quá số lượng tồn!");
        }
        else {
            if (ProductPrice !== "") {
                var frmId = "frmCreate";
                var data = $("#" + frmId).serialize();

                data = data + "&ProductId=" + ProductId;
                data = data + "&ProductCode=" + ProductCode;
                data = data + "&ProductName=" + ProductName;
                data = data + "&Quantity=" + ProductQuantity;
                data = data + "&Price=" + ProductPrice;
                data = data + "&StockId=" + StockId;
                data = data + "&StockCode=" + StockCode;
                data = data + "&StockName=" + StockName;
                data = data + "&DetailNote=" + ProductStockNote;
                data = data + "&ProductQuantinyOnHand=" + ProductQuantinyOnHand;

                $.ajax({
                    type: "POST",
                    url: "/Warehouse/StockDelivery/InsertProductStock",
                    data: data,
                    success: function (xhr, status, error) {
                        $btn.button('reset');
                        if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                            alertPopup(false, xhr.Message);
                        }
                        else {
                            $("#deliceryDetailTable tbody").html(xhr);
                            TotalPrice();
                            $("#btn-del-product").trigger("click");
                        }
                    },
                    error: function (xhr, status, error) {
                        $btn.button('reset');
                        alertPopup(false, xhr.responseText);
                    }
                });
            } else {
                $btn.button('reset');
                alertPopup(false, "Catalogue chưa thiết lập giá, vui lòng cấu hình giá trong danh mục!");
            }
        }
    }
});

//Xóa dữ liệu autocomplete phụ tùng/phụ kiện
$(document).on("click", "#btn-del-product", function () {
    $("#ProductCode").val("");
    $("#ProductCode").prop("disabled", false);
    //$("#StockCode").val("");
    //$("#StockCode").prop("disabled", false);

    $("#ProductId").val("");
    //$("#StockId").val("");
    $("#ProductName").val("");
    //$("#StockName").val("");
    $("#ProductQuantity").val("");
    $("#ProductQuantinyOnHand").val("");
    $("#ProductPrice").val("");
    $("#ProductStockNote").val("");
    //$("#StockCode").focus();
    $("#ProductCode").focus();
});

//Xóa phụ tùng/phụ kiện khỏi lưới
$(document).on("click", ".btn-del-proDetail", function () {
    // var ProductId = $(this).data("id");

    var frmId = "";
    frmId = "frmCreate";

    var data = $("#" + frmId).serialize();

    var STT = $(this).data("row");
    data = data + "&STT=" + STT;
    $.ajax({
        type: "POST",
        url: "/Warehouse/StockDelivery/RemoveProductStock",
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                alertPopup(true, xhr.Message);
            }
            else {
                $("#deliceryDetailTable tbody#deliveryDetailList").html("");
                $("#deliceryDetailTable tbody#deliveryDetailList").html(xhr);
                TotalPrice();
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});
//Autocomple kho xuất
function SearchText_FromStockCode() {
    $("#StockCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Warehouse/Stock/GetStockForAutocomple",
                data: JSON.stringify({ "SearchText": $("#StockCode").val() }),
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
                $("#StockCode").val("");
                $("#StockName").val("");
                $("#StockId").val("");
                $("#StockCode").focus();
            }
        },
        select: function (event, ui) {

            $("#StockCode").val(ui.item.value);
            $("#StockCode").prop("disabled", true);
            $("#StockName").val(ui.item.code);
            $("#StockId").val(ui.item.id);
            $("#ProductCode").focus();

            //Tab or enter auto select first (AutoFocus = true)
            if (event.keyCode === 9 || event.keyCode === 13) {
                $("#StockCode").val(ui.item.value);
                event.preventDefault();
            }
        }
    });
}
//Tính tổng tiền
function TotalPrice() {
    var SubTotal = 0;
    $(".deliveryDetailList-UnitPrice").each(function () {
        var dataRow = $(this).data("row");
        var TotalPrice = $("input[name='deliveryDetailList[" + dataRow + "].UnitPrice']").val();
        if (TotalPrice == "") {
            TotalPrice = 0;
        }
        SubTotal += parseInt(TotalPrice);
    });
    $("#total").html(formatCurrency(SubTotal));
    // $("#TotalPrice").val(SubTotal);
}

//enter add product to grid
$("#ProductQuantity").keyup(function (event) {
    if (event.keyCode === 13) {
        $("#btn-add-deliveryDetail").click();
    }
});
$("#btn-add-deliveryDetail").keyup(function (event) {
    if (event.keyCode === 13 || event.keyCode === 32) {
        $("#btn-add-deliveryDetail").click();
    }
});

$(document).on("change", "select[name='SalesEmployeeCode']", function () {
    var SalesEmployeeCode = $(this).val();

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockDelivery/GetSalesEmployeeInfo",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        dataType: "json",
        success: function (jsonData) {
            $("input[name='SenderName']").val("");
            $("input[name='SenderPhone']").val("");
            if (jsonData != null) {
                $("input[name='SenderName']").val(jsonData.SalesEmployeeName);
                $("input[name='SenderPhone']").val(jsonData.Phone);
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});