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
            $("#ProductQuantityOffer").focus();
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
function RefreshTable() {
    var len = productList.length;
    tbody = '';
    if (len > 0) {
        for (var i = 0; i < len; i++) {
            var tr = '<tr>' +
                '<td class="text-center">' + parseInt(i + 1) + '</td >' +
                '<td>' + productList[i].fromStockName + '</td >' +
                '<td>' + productList[i].toStockName + '</td >' +
                '<td>' + productList[i].productCode + '</td >' +
                '<td>' + productList[i].productName + '</td >' +
                '<td class="text-center">' + productList[i].offerQuantity + '</td >' +
                '<td class="text-center">' + productList[i].requestQuantity + '</td >' +
                '<td class="text-center">' +
                '<a class="btn btn-danger btn-del-proDetail" data-row="1" data-id="' + productList[i].productId + '">Xóa</a>' +
                '</td>' +
                '</tr>';
            tbody = tbody + tr;
        }
        $('#transferDetailList').html(tbody);
        $("#ProductCode").val("");
        $("#ProductCode").prop("disabled", false);
        $("#ProductName").val("");
        $("#ProductId").val("");
        $("#ProductQuantity").val("");
        $("#ProductCode").focus();
    }
    else {
        tbody = '<tr><td class="text-center" colspan="7">Vui lòng thêm sản phẩm!</td></tr>';
        $('#transferDetailList').html(tbody);
    }
}
function RenderTable(data) {
    var len = productList.length;
    tbody = '';
    if (len > 0) {
        for (var i = 0; i < len; i++) {
            var tr = '<tr>' +
                '<td class="text-center">' + parseInt(i + 1) + '</td >' +
                '<td>' + data.fromStockName + '</td >' +
                '<td>' + data.toStockName + '</td >' +
                '<td>' + productList[i].productCode + '</td >' +
                '<td>' + productList[i].productName + '</td >' +
                '<td class="text-center">' + productList[i].offerQuantity + '</td >' +
                '<td class="text-center">' + productList[i].requestQuantity + '</td >' +
                '<td class="text-center">' + productList[i].transferredQuantity + '</td >' +
                '<td class="text-center">' + productList[i].remainingQuantity + '</td >' +
                '<td class="text-center">' +
                //'<a class="btn btn-danger btn-del-proDetail" data-row="1" data-id="' + productList[i].ProductId + '">Xóa</a>' +
                '</td>' +
                '</tr>';
            tbody = tbody + tr;
        }
        $('#transferDetailList').html(tbody);
    }
}
$(document).on("change keyup", ".transfer-quantity", function () {
    var newValue = $(this).val();
    var id = $(this).data("id");
    for (var i = 0; i < productList.length; i++) {
        if (productList[i].productId == id) {
            productList[i].requestQuantity = newValue;
        }
    }
});
function RenderTableCopy(data) {
    var len = productList.length;
    tbody = '';
    if (len > 0) {
        for (var i = 0; i < len; i++) {
            var tr = '<tr>' +
                '<td class="text-center">' + parseInt(i + 1) + '</td >' +
                '<td>' + data.fromStockName + '</td >' +
                '<td>' + data.toStockName + '</td >' +
                '<td>' + productList[i].productCode + '</td >' +
                '<td>' + productList[i].productName + '</td >' +
                '<td class="text-center"><input data-id="' + productList[i].productId + '" class="form-control transfer-quantity" value="' + productList[i].offerQuantity + '" type="number" min="0" step="1"></td >' +
                '<td class="text-center"><input data-id="' + productList[i].productId + '" class="form-control transfer-quantity" value="' + productList[i].requestQuantity + '" type="number" min="0" step="1"></td >' +
                '<td class="text-center">' +
                '<a class="btn btn-danger btn-del-proDetail" data-row="1" data-id="' + productList[i].productId + '">Xóa</a>' +
                '</td>' +
                '</tr>';
            tbody = tbody + tr;
        }

    }
    $('#transferDetailList').html(tbody);
}
var productList = [];

//Thêm vào lưới thông tin sản phẩm
$(document).on("click", "#btn-add-stockReceivingDetail", function () {
    var $btn = $(this);
    $btn.button('loading');

    var ProductId = $("#ProductId").val();
    var ProductCode = $("#ProductCode").val();
    var ProductName = $("#ProductName").val();
    var ProductQuantity = $("#ProductQuantity").val();
    var ProductQuantityOffer = $("#ProductQuantityOffer").val();
    var FromStockName = $("#FromStockName").val();
    var ToStockName = $("#ToStockName").val();
    if (ProductCode === "") {
        $btn.button('reset');
        alertPopup(false, "Vui lòng chọn sản phẩm trước khi thêm!");
    }
    else {
        if (ProductQuantityOffer == "" || parseInt(ProductQuantityOffer) < 0) {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng đề xuất lớn hơn 0!");
        }
        else {

            var product = {
                productId: ProductId,
                productCode: ProductCode,
                productName: ProductName,
                requestQuantity: ProductQuantity,
                offerQuantity: ProductQuantityOffer,
                fromStockName: FromStockName,
                toStockName: ToStockName
            }
            productList.push(product);
            RefreshTable();
            $btn.button('reset');
        }
    }

    //var $btn = $(this);
    //$btn.button('loading');

    //var ProductId = $("#ProductId").val();
    //var ProductCode = $("#ProductCode").val();
    //var ProductName = $("#ProductName").val();
    //var ProductQuantity = $("#ProductQuantity").val();
    //var ProductQuantinyOnHand = $("#ProductQuantinyOnHand").val();
    //var ProductPrice = $("#ProductPrice").val();
    //var FromStockId = $("#FromStockId").val();
    //var FromStockCode = $("#FromStockCode").val();
    //var FromStockName = $("#FromStockName").val();
    //var ToStockId = $("#ToStockId").val();
    //var ToStockCode = $("#ToStockCode").val();
    //var ToStockName = $("#ToStockName").val();
    //var ProductStockNote = $("#ProductStockNote").val();

    //if (ProductCode === "") {
    //    $btn.button('reset');
    //    alertPopup(false, "Vui lòng chọn sản phẩm trước khi thêm!");
    //}
    //else {
    //    if (ProductQuantity == "" || parseInt(ProductQuantity) < 0) {
    //        $btn.button('reset');
    //        alertPopup(false, "Vui lòng nhập số lượng lớn hơn 0!");
    //    }
    //    else {
    //        var frmId = "frmCreate";
    //        var $frmCreate = $("body #frmCreate");
    //        var data = $("#" + frmId).serialize();

    //        data = data + "&ProductId=" + ProductId;
    //        data = data + "&ProductCode=" + ProductCode;
    //        data = data + "&ProductName=" + ProductName;
    //        data = data + "&Quantity=" + ProductQuantity;
    //        data = data + "&QuantinyOnHand=" + ProductQuantinyOnHand;
    //        data = data + "&Price=" + ProductPrice;
    //        data = data + "&FromStockId=" + FromStockId;
    //        data = data + "&FromStockCode=" + FromStockCode;
    //        data = data + "&FromStockName=" + FromStockCode + " | " + FromStockName;
    //        data = data + "&ToStockId=" + ToStockId;
    //        data = data + "&ToStockCode=" + ToStockCode;
    //        data = data + "&ToStockName=" + ToStockCode + " | " + ToStockName;
    //        data = data + "&DetailNote=" + ProductStockNote;

    //        $.ajax({
    //            type: "POST",
    //            url: "/Warehouse/StockTransfer/InsertProductStock",
    //            data: data,
    //            success: function (xhr, status, error) {
    //                $btn.button('reset');
    //                if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
    //                    alertPopup(false, xhr.Message);
    //                }
    //                else {
    //                    $("#transferDetailTable tbody").html(xhr);
    //                    //TotalPrice();

    //                    //$("#btn-del-product").trigger("click");
    //                    ClearProductData();
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                $btn.button('reset');
    //                alertPopup(false, xhr.responseText);
    //            }
    //        });
    //    }

    //}
});


//Xóa sản phẩm khỏi lưới
$(document).off("click", ".btn-del-proDetail").on("click", ".btn-del-proDetail", function () {
    var id = $(this).data('id');
    var len = productList.length;
    for (var i = 0; i < len; i++) {
        if (productList[i].ProductId == id || productList[i].productId == id) {
            productList.splice(i, 1);
            RefreshTable();
        }
    }

    //var frmId = "";
    //frmId = "frmCreate";
    //var data = $("#" + frmId).serialize();
    //var STT = $(this).data("row");
    //data = data + "&STT=" + STT;

    //$.ajax({
    //    type: "POST",
    //    url: "/Warehouse/StockTransfer/RemoveProductStock",
    //    data: data,
    //    success: function (xhr, status, error) {
    //        if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
    //            alertPopup(false, xhr.Message);
    //        }
    //        else {
    //            $("#transferDetailTable tbody#transferDetailList").html("");
    //            $("#transferDetailTable tbody#transferDetailList").html(xhr);
    //            //TotalPrice();
    //        }
    //    },
    //    error: function (xhr, status, error) {
    //        alertPopup(false, xhr.responseText);
    //    }
    //});
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
$(document).off("click", "#btn-save").on("click", "#btn-save", function () {
    var $btn = $(this);
    $btn.button('loading');
    var url;
    var data = GetData();
    if (viewType == 'Edit') {
        url = net5apidomain + 'api/Warehouse/StockTransferRequests/' + editId;
    }
    else {
        url = net5apidomain + 'api/Warehouse/StockTransferRequests';
    }
    SaveData(data, url, "Warehouse/StockTransferRequest", "POST", false, false);
    $btn.button('reset');
});
$(document).off("click", "#btn-save-edit").on("click", "#btn-save-edit", function () {
    var $btn = $(this);
    $btn.button('loading');
    var data = GetData();
    var url = net5apidomain + 'api/Warehouse/StockTransferRequests';
    SaveData(data, url, "Warehouse/StockTransferRequest", "POST", true, true);
    $btn.button('reset');
});
function SaveData(data, saveToApiUrl, controller, httpMethod, isContinue, isToEditMode) {
    loading2();
    $.ajax({
        type: "POST"/*httpMethod*/,
        url: saveToApiUrl,
        data: JSON.stringify(data),
        processData: false,
        contentType: "application/json",
        success: function (res) {

            if (res.isSuccess == true) {
                alertPopup(true, res.message);
                //if (actionType == "EDIT" && controller == "Marketing/TargetGroup") {               
                //    ISDNET5.ReloadDataChange($('#Id').val())
                //}
                if (isContinue == true) {
                    if (isToEditMode) {
                        window.location.href = "/" + controller + "/Edit/" + res.data.id;
                    }
                    else {
                        location.reload();
                        window.location.href = "/" + controller;
                    }
                }
                else {
                    location.reload();
                    window.location.href = "/" + controller;
                }
            }
            else {
                var resObj = JSON.parse(res);
                alertPopup(false, resObj.Message);
            }
        },
        error: function (res) {
            var resObj = JSON.parse(res);
            alertPopup(false, resObj.Message);
        }
    });
}
function GetData() {
    var CompanyId = $("#CompanyId").val();
    var DocumentDate = $("#DocumentDate").val();
    var FromPlanDate = $("#FromPlanDate").val();
    var ToPlanDate = $("#ToPlanDate").val();
    var SalesEmployeeCode = $("#SalesEmployeeCode").val();
    var Note = $("#Note").val();
    var SenderName = $("#SenderName").val();
    var SenderAddress = $("#SenderAddress").val();
    var SenderPhone = $("#SenderPhone").val();
    var RecipientCompany = $("#RecipientCompany").val();
    var RecipientName = $("#RecipientName").val();
    var RecipientAddress = $("#RecipientAddress").val();
    var RecipientPhone = $("#RecipientPhone").val();
    var StoreId = $("#StoreId").val();
    var FromStock = $("#FromStockId").val();
    var ToStock = $("#ToStockId").val();
    var TransferRequestDetails = productList;
    var CreateBy = currentUser;
    var data = {
        CompanyId: CompanyId,
        DocumentDate: DocumentDate,
        FromPlanDate: FromPlanDate,
        ToPlanDate: ToPlanDate,
        SalesEmployeeCode: SalesEmployeeCode,
        Note: Note,
        SenderName: SenderName,
        SenderAddress: SenderAddress,
        SenderPhone: SenderPhone,
        RecipientCompany: RecipientCompany,
        RecipientName: RecipientName,
        RecipientAddress: RecipientAddress,
        RecipientPhone: RecipientPhone,
        StoreId: StoreId,
        FromStock: FromStock,
        ToStock: ToStock,
        TransferRequestDetails: TransferRequestDetails,
        CreateBy: CreateBy
    }
    return data;
}
function EditInitial(id) {
    var url = net5apidomain + 'api/Warehouse/StockTransferRequests/' + id;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data;
                $("#CompanyId").val(data.companyId).change();
                $("#DocumentDate").val(data.documentDate);
                $("#FromPlanDate").val(data.fromPlanDate);
                $("#ToPlanDate").val(data.toPlanDate);
                $("#SalesEmployeeCode").val(data.salesEmployeeCode);
                $("#Note").val(data.note);
                $("#SenderName").val(data.senderName);
                $("#SenderAddress").val(data.senderAddress);
                $("#SenderPhone").val(data.senderPhone);
                $("#RecipientCompany").val(data.recipientCompany);
                $("#RecipientName").val(data.recipientName);
                $("#RecipientAddress").val(data.recipientAddress);
                $("#RecipientPhone").val(data.recipientPhone);
                $("#StoreId").val(data.storeId);
                $('#StoreId').append(new Option(data.storeName, data.storeId, true));
                $("#FromStockId").val(data.fromStock);
                $("#FromStockName").val(data.fromStockName);
                $("#FromStockCode").val(data.fromStockCode);
                $("#ToStockId").val(data.toStock);
                $("#ToStockName").val(data.toStockName);
                $("#ToStockCode").val(data.toStockCode);
                productList = data.transferRequestDetails;

                $("#StoreId").prop("disabled", true);
                $("#ToStockCode").prop("disabled", true);
                $("#CompanyId").prop("disabled", true);
                $("#divproductcode").hide();
                $("#divproductname").hide();
                $("#divquantity").hide();
                RenderTable(data);

                var createBy = ' Được tạo bởi ' + data.createByName + ' vào lúc ' + moment(data.createTime).format('DD/MM/YYYY hh:mm:ss A');
                var lastEditBy = 'Cập nhật lần cuối bởi ' + data.lastEditByName + ' vào lúc ' + moment(data.lastEditTime).format('DD/MM/YYYY hh:mm:ss A');
                $('#createBy').html(createBy);
                $('#lastEditBy').html(lastEditBy);
            }
            else {
                alertPopup(false, res.message);
            }
        },
        error: function (res) {
            alertPopup(false, res.responseJSON.message);
        }
    });
}
function CopyFrom(id) {
    var url = net5apidomain + 'api/Warehouse/StockTransferRequests/' + id;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data;
                console.log(data);
                $("#CompanyId").val(data.companyId).change();
                $("#DocumentDate").val(data.documentDate);
                $("#FromPlanDate").val(data.fromPlanDate);
                $("#ToPlanDate").val(data.toPlanDate);
                $("#SalesEmployeeCode").val(data.salesEmployeeCode);
                $("#Note").val(data.note);
                $("#SenderName").val(data.senderName);
                $("#SenderAddress").val(data.senderAddress);
                $("#SenderPhone").val(data.senderPhone);
                $("#RecipientCompany").val(data.recipientCompany);
                $("#RecipientName").val(data.recipientName);
                $("#RecipientAddress").val(data.recipientAddress);
                $("#RecipientPhone").val(data.recipientPhone);
                $("#StoreId").val(data.storeId).change();
                // $('#StoreId').append(new Option(data.storeName, data.storeId, true));
                $("#FromStockId").val(data.fromStock);
                $("#FromStockName").val(data.fromStockName);
                $("#FromStockCode").val(data.fromStockCode);
                $("#ToStockId").val(data.toStock);
                $("#ToStockName").val(data.toStockName);
                $("#ToStockCode").val(data.toStockCode);
                var detail = data.transferRequestDetails;
                for (var i = 0; i < detail.length; i++) {
                    var product = {
                        productId: detail[i].productId,
                        productCode: detail[i].productCode,
                        productName: detail[i].productName,
                        requestQuantity: detail[i].requestQuantity,
                        offerQuantity: detail[i].offerQuantity,
                        fromStockName: data.fromStockName,
                        toStockName: data.toStockName
                    }
                    productList.push(product);
                }
                // productList = data.transferRequestDetails;

                //$("#StoreId").prop("disabled", true);
                //$("#ToStockCode").prop("disabled", true);
                //$("#CompanyId").prop("disabled", true);
                //$("#divproductcode").hide();
                //$("#divproductname").hide();
                //$("#divquantity").hide();
                setTimeout(function () {
                    RenderTableCopy(data);
                }, 1000)



            }
            else {
                alertPopup(false, res.message);
            }
        },
        error: function (res) {
            alertPopup(false, res.responseJSON.message);
        }
    });
}
