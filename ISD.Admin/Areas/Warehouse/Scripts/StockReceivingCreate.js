//Autocomplete tìm kiếm sản phẩm
function SearchText_ProductCode() {
    $("#ProductCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Sale/Product/SearchProductByCodeOrName",
                data: JSON.stringify({ "SearchText": $("#ProductCode").val() }),
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        //return { label: item.ProductCodeText, id: item.ProductName, value: item.ProductCode, address: item.ProductId, price: item.ProductPrice };
                        return { label: item.ProductSearchName, id: item.ProductId, value: item.ProductCode, code: item.ProductName, price: item.Price };
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
                $("#ProductCode").val("");
                $("#ProductName").val("");
                $("#ProductPrice").val("");
                $("#ProductCode").focus();
            }
        },
        autoFocus: true,
        select: function (event, ui) {
            $("#ProductCode").val(ui.item.value);
            $("#ProductId").val(ui.item.id);
            $("#ProductCode").prop("disabled", true);
            $("#ProductName").val(ui.item.code);
            $("#ProductPrice").val(ui.item.price);

            $("#ProductPrice").focus();

            //tab or enter auto select first (autofocus = true)
            if (event.keyCode === 9 || event.keyCode === 13) {
                $("#ProductCode").val(ui.item.value);
                event.preventDefault();
            }
        }
    });
}

//Thêm vào lưới thông tin sản phẩm
$(document).on("click", "#btn-add-stockReceivingDetail", function () {
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

    if (ProductCode === "") {
        $btn.button('reset');
        alertPopup(false, "Vui lòng chọn sản phẩm trước khi thêm!");
    }
    else {
        if (ProductQuantity < 0 || ProductQuantity == "") {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng lớn hơn 0!");
        }
        else {
            if (ProductPrice !== "") {
                var frmId = "frmCreate";
                //var isThemMoi = true;

                //var $frmEdit = $("body #frmEdit");
                var $frmCreate = $("body #frmCreate");
                //if ($frmEdit.length === 0) {
                //    frmId = "frmCreate";
                //}
                //else {
                //    frmId = "frmEdit";
                //    isThemMoi = false;
                //}
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

                $.ajax({
                    type: "POST",
                    url: "/Warehouse/StockReceiving/InsertProductStock",
                    data: data,
                    success: function (xhr, status, error) {
                        $btn.button('reset');
                        if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                            alertPopup(false, xhr.Message);
                        }
                        else {
                            $("#stockReceivingDetailTable tbody").html(xhr);
                            TotalPrice();

                            $("#ProductId").val("");
                            $("#ProductCode").val("");
                            $("#ProductCode").prop("disabled", false);
                            $("#ProductName").val("");
                            $("#ProductQuantity").val("");
                            $("#ProductPrice").val("");
                            $("#ProductStockNote").val("");

                            $("#ProductCode").focus();
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

    $("#ProductId").val("");
    $("#ProductName").val("");
    $("#ProductQuantity").val("");
    $("#ProductPrice").val("");
    $("#ProductStockNote").val("");
    $("#ProductCode").focus();
});
//Xóa phụ tùng/phụ kiện khỏi lưới
$(document).on("click", ".btn-del-proDetail", function () {
    // var ProductId = $(this).data("id");

    var frmId = "";
    //var isThemMoi = true;

    //var $frmEdit = $("body #frmEdit");
    //var $frmCreate = $("body #frmCreate");
    //if ($frmEdit.length === 0) {
    frmId = "frmCreate";
    //}
    //else {
    //    frmId = "frmEdit";
    //    isThemMoi = false;
    //}
    var data = $("#" + frmId).serialize();

    var STT = $(this).data("row");
    data = data + "&STT=" + STT;
    // data = data + "&ProductId=" + ProductId;
    // data = data + "&isThemMoi=" + isThemMoi;
    $.ajax({
        type: "POST",
        url: "/Warehouse/StockReceiving/RemoveProductStock",
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                alertPopup(false, xhr.Message);
            }
            else {
                $("#stockReceivingDetailTable tbody#stockReceivingDetailList").html("");
                $("#stockReceivingDetailTable tbody#stockReceivingDetailList").html(xhr);
                TotalPrice();
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//Tính tổng tiền
function TotalPrice() {
    var SubTotal = 0;
    $(".stockReceivingDetailList-UnitPrice").each(function () {
        var dataRow = $(this).data("row");
        var TotalPrice = $("input[name='stockReceivingDetailList[" + dataRow + "].UnitPrice']").val();
        if (TotalPrice == "") {
            TotalPrice = 0;
        }
        SubTotal += parseInt(TotalPrice);
    });
    $("#total").html(formatCurrency(SubTotal));
    // $("#TotalPrice").val(SubTotal);
}

//enter add product to grid
$("#ProductPrice").keyup(function (event) {
    if (event.keyCode === 13) {
        $("#btn-add-stockReceivingDetail").click();
    }
});
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