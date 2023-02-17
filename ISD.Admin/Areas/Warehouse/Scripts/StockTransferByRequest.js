function RenderTable(data) {
    var len = productList.length;
    tbody = '';
    if (len > 0) {
        for (var i = 0; i < len; i++) {
            var tr = '<tr>' +
                '<td class="text-center">' + parseInt(i + 1) + '</td >' +
                '<td>' + productList[i].productCode + '</td >' +
                '<td>' + productList[i].productName + '</td >' +
                '<td class="text-center">' + productList[i].offerQuantity + '</td >' +
                '<td class="text-center">' + productList[i].requestQuantity + '</td >' +
                '<td class="text-center">' + productList[i].transferredQuantity + '</td >' +
                '<td class="text-center">' + productList[i].remainingQuantity + '</td >' +
                '<td class="text-center "> <input data-id="' + productList[i].id + '" class="form-control transfer-quantity" type="number" min="0" step="1"></td >' +
               
                '</tr>';
            tbody = tbody + tr;
        }
        $('#transferDetailList').html(tbody);
    }
}
var productList = [];
$(document).off("click", "#btn-save").on("click", "#btn-save", function () {
    var $btn = $(this);
    $btn.button('loading');
    var listIsVaid = false;
    for (var i = 0; i < productList.length; i++) {
        if (productList[i].transferQuantity != null && productList[i].transferQuantity != undefined && productList[i].transferQuantity != 0) {
            listIsVaid = true;
            break;
        }
    }
    if (!listIsVaid) {
        alertPopup(false, "Vui lòng chọn ít nhất 1 sản phẩm để chuyển kho");
        $btn.button('reset');
    }
    else {
        var url = net5apidomain + 'api/Warehouse/StockTransfers' ;
        var data = GetData();
        SaveData(data, url, "Warehouse/StockTransfer", "POST", false, false);
        $btn.button('reset');
    }

});
//$(document).off("click", "#btn-save-edit").on("click", "#btn-save-edit", function () {
//    var $btn = $(this);
//    $btn.button('loading');
//    var data = GetData();
//    var url = net5apidomain + 'api/Warehouse/StockTransferRequests';
//    SaveData(data, url, "Warehouse/StockTransferRequest", "POST", true, true);
//    $btn.button('reset');
//});
$(document).on("change", ".transfer-quantity", function () {
    var id = $(this).data("id");
    var transferQuantity = parseInt($(this).val());

    for (var i = 0; i < productList.length; i++) {
        if (productList[i].id == id) {
            if (transferQuantity < 0 || transferQuantity > productList[i].remainingQuantity) {
                alertPopup(false, "Số lượng chuyển không được nhỏ hơn 0 và không được lớn hơn Số lượng còn lại");
                $(this).val('');
                $(this).focus();
            }
            else {
                productList[i].transferQuantity = transferQuantity;
            }
        }
    }

});
function SaveData(data, saveToApiUrl, controller, httpMethod, isContinue, isToEditMode) {
    var $btn = $("#btn-save");
    $btn.button('loading');
    $.ajax({
        type: "POST"/*httpMethod*/,
        url: saveToApiUrl,
        data: JSON.stringify(data),
        processData: false,
        contentType: "application/json",
        success: function (res) {

            if (res.isSuccess == true) {
                alertPopup(true, res.message);
                productList = [];
                $btn.button('reset');
                if (isContinue == true) {
                    if (isToEditMode) {
                        window.location.href = "/" + controller + "/Edit/" + res.data.id;
                    }
                    else {
                        window.location.href = "/" + controller;
                    }
                }
                else {
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
    var SalesEmployeeCode = $("#SalesEmployeeCode").val();
    var Note = $("#Note").val();
    var SenderName = $("#SenderName").val();
    var SenderAddress = $("#SenderAddress").val();
    var SenderPhone = $("#SenderPhone").val();
    var RecipientCompany = $("#RecipientCompany").val();
    var RecipientName = $("#RecipientName").val();
    var RecipientAddress = $("#RecipientAddress").val();
    var RecipientPhone = $("#RecipientPhone").val();
    var TransferDetails = productList;
    var CreateBy = currentUser;
    var data = {      
        StockTransferRequestId: StockTransferRequestId,
        SalesEmployeeCode: SalesEmployeeCode,
        Note: Note,
        SenderName: SenderName,
        SenderAddress: SenderAddress,
        SenderPhone: SenderPhone,
        RecipientCompany: RecipientCompany,
        RecipientName: RecipientName,
        RecipientAddress: RecipientAddress,
        RecipientPhone: RecipientPhone,    
        TransferDetails: TransferDetails,
        CreateBy: CreateBy
    }
    return data;
}
function CreateInitial(id) {
    var url = net5apidomain + 'api/Warehouse/StockTransferRequests/' + id;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data;
                $("#CompanyId").val(data.companyId);
                //$("#DocumentDate").val(data.documentDate);
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