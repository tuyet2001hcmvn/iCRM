$(document).on("change", "select[name='SearchCompanyId']", function () {
    var CompanyId = $(this).val();
    if (CompanyId == "") {
        $.ajax({
            type: "POST",
            url: "/MasterData/Store/GetAllStoreForDropDown",
            success: function (jsonData) {
                $("#SearchStoreId").html("");
                $("#SearchStoreId").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    $("#SearchStoreId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });
                $("#SearchStoreId").trigger("change");
            }
        });

    } else {
        $.ajax({
            type: "POST",
            url: "/MasterData/Store/GetStoreByCompany",
            data: {
                CompanyId: CompanyId
            },
            success: function (jsonData) {
                $("#SearchStoreId").html("");
                $("#SearchStoreId").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    $("#SearchStoreId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });
                $("#SearchStoreId").trigger("change");
            }
        });
    }
});

$(document).on("change", "select[name='SearchStoreId']", function () {
    var StoreId = $(this).val();
    if (StoreId == "") {
        $.ajax({
            type: "POST",
            url: "/Warehouse/Stock/GetAllForDropdown",
            success: function (jsonData) {
                $("#SearchStockId").html("");
                $("#SearchStockId").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    $("#SearchStockId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });
            }
        });

    } else {
        $.ajax({
            type: "POST",
            url: "/Warehouse/Stock/GetStockByStore",
            data: {
                StoreId: StoreId
            },
            success: function (jsonData) {
                $("#SearchStockId").html("");
                $("#SearchStockId").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    $("#SearchStockId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });
            }
        });
    }
});

$("#ProductCode").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Warehouse/StockReceiving/GetProductCode",
            data: JSON.stringify({ "ProductCode": $("#ProductCode").val() }),
            dataType: "json",
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.ProductCodeText, id: item.ProductId, value: item.ProductCode, name: item.ProductName };
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
            $("#SearchProductCode").val("");
            $("#ProductName").val("");
            $("#SearchProductName").val("");
            $("#SearchProductId").val("");
            $("#ProductCode").focus();
        }
    },
    select: function (event, ui) {
        $("#ProductCode").val(ui.item.value);
        $("#ProductCode").prop("disabled", true);
        $("#SearchProductCode").val(ui.item.value);
        $("#ProductName").val(ui.item.name);
        $("#SearchProductName").val(ui.item.name);
        $("#SearchProductId").val(ui.item.id);
    }
});
$(document).on("click", "#btn-clearProduct", function () {
    $("#ProductCode").val("");
    $("#ProductName").val("");
    $("#SearchProductId").val("");
    $("#ProductCode").prop("disabled", false);
})

$("#ProfileCode").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Customer/Profile/GetProfileByCode",
            data: JSON.stringify({ "ProfileCode": $("#ProfileCode").val() }),
            dataType: "json",
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.ProfieLableName, id: item.ProfileId, value: item.ProfileCode, name: item.ProfileName };
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
            $("#ProfileCode").val("");
            $("#SearchProfileCode").val("");
            $("#ProfileName").val("");
            $("#SearchProfileName").val("");
            $("#SearchProfileId").val("");
            $("#ProfileCode").focus();
        }
    },
    select: function (event, ui) {
        $("#ProfileCode").val(ui.item.value);
        $("#ProfileCode").prop("disabled", true);
        $("#SearchProfileCode").val(ui.item.value);
        $("#ProfileName").val(ui.item.name);
        $("#SearchProfileName").val(ui.item.name);
        $("#SearchProfileId").val(ui.item.id);
    }
});

$(document).on("click", "#btn-clearProfile", function () {
    $("#ProfileCode").val("");
    $("#ProfileName").val("");
    $("#SearchProfileId").val("");
    $("#ProfileCode").prop("disabled", false);
});

$(document).on("click", ".btn-delete-receive", function () {
    var StockReceivingId = $(this).data("id");
    var StockReceivingCode = $(this).data("name");
    $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='StockReceivingId']").val(StockReceivingId);
    $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving .alert-message").html("Bạn có chắc chắn muốn xóa phiếu nhập kho " + StockReceivingCode + "?");
    $("#divDeleteStockReceivingPopup").modal("show");
});

$(document).on("click", "#btn-confirm-del-receive", function () {
    var StockReceivingId = $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='StockReceivingId']").val();
    var DeletedReason = $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='DeletedReason']").val();

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockReceiving/Delete",
        data: {
            id: StockReceivingId,
            DeletedReason: DeletedReason
        },
        success: function (jsonData) {
            if (jsonData.Success == true) {
                $("#divDeleteStockReceivingPopup").modal("hide");
                if ($("#btn-search").length > 0) {
                    $("#btn-search").trigger("click");
                }
            }
            else {
                if (jsonData.Data != null && jsonData.Data != "") {
                    alertWaringPopup(false, jsonData.Data);
                }
            }
        },
        error: function (xhr, status, error) {
            alertWaringPopup(false, xhr.responseText);
        }
    });
});

$(document).on("click", ".btn-delete-transfer", function () {
    var TransferId = $(this).data("id");
    var TransferCode = $(this).data("name");
    $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='TransferId']").val(TransferId);
    $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer .alert-message").html("Bạn có chắc chắn muốn xóa phiếu chuyển kho " + TransferCode + "?");
    $("#divDeleteStockTransferPopup").modal("show");
});

$(document).on("click", "#btn-confirm-del-transfer", function () {
    var TransferId = $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='TransferId']").val();
    var DeletedReason = $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='DeletedReason']").val();

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockTransfer/Delete",
        data: {
            id: TransferId,
            DeletedReason: DeletedReason
        },
        success: function (jsonData) {
            if (jsonData.Success == true) {
                $("#divDeleteStockTransferPopup").modal("hide");
                if ($("#btn-search").length > 0) {
                    $("#btn-search").trigger("click");
                }
            }
            else {
                if (jsonData.Data != null && jsonData.Data != "") {
                    alertWaringPopup(false, jsonData.Data);
                }
            }
        },
        error: function (xhr, status, error) {
            alertWaringPopup(false, xhr.responseText);
        }
    });
});

function alertWaringPopup(isSuccess, message) {
    $("#divAlertWarningPopup .alert-message").html("");
    setMessage("#divAlertWarningPopup", message);
    $('#divAlertWarningPopup').show();
}

$(document).on("hidden.bs.modal", "#divDeleteStockTransferPopup, #divDeleteStockReceivingPopup", function () {
    $("#divAlertWarningPopup").hide();
});



$(document).on("click", ".btn-cancel-receive", function () {
    var StockReceivingId = $(this).data("id");
    var StockReceivingCode = $(this).data("name");
    $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='StockReceivingId']").val(StockReceivingId);
    $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving .alert-message").html("Bạn có chắc chắn muốn huỷ phiếu nhập kho " + StockReceivingCode + "?");
    $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving  #btn-confirm-del-receive").attr("id","btn-confirm-cancel-receive");
    $("#divDeleteStockReceivingPopup").modal("show");
});

$(document).on("click", "#btn-confirm-cancel-receive", function () {
    var StockReceivingId = $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='StockReceivingId']").val();
    var DeletedReason = $("#divDeleteStockReceivingPopup #frmConfirmDeleteStockReceiving input[name='DeletedReason']").val();

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockReceiving/Cancel",
        data: {
            id: StockReceivingId,
            DeletedReason: DeletedReason
        },
        success: function (jsonData) {
            if (jsonData.Success == true) {
                alertPopup(true, jsonData.Data);
                $("#divDeleteStockReceivingPopup").modal("hide");
                if ($("#btn-search").length > 0) {
                    $("#btn-search").trigger("click");
                }
            }
            else {
                if (jsonData.Data != null && jsonData.Data != "") {
                    alertWaringPopup(false, jsonData.Data);
                }
            }
        },
        error: function (xhr, status, error) {
            alertWaringPopup(false, xhr.responseText);
        }
    });
});



$(document).on("click", ".btn-cancel-transfer", function () {
    var TransferId = $(this).data("id");
    var TransferCode = $(this).data("name");
    $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='TransferId']").val(TransferId);
    $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer .alert-message").html("Bạn có chắc chắn muốn huỷ phiếu chuyển kho " + TransferCode + "?");
    $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer #btn-confirm-del-transfer").attr("id", "btn-confirm-cancel-transfer");

    $("#divDeleteStockTransferPopup").modal("show");
});

$(document).on("click", "#btn-confirm-cancel-transfer", function () {
    var TransferId = $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='TransferId']").val();
    var DeletedReason = $("#divDeleteStockTransferPopup #frmConfirmDeleteStockTransfer input[name='DeletedReason']").val();

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockTransfer/Cancel",
        data: {
            id: TransferId,
            DeletedReason: DeletedReason
        },
        success: function (jsonData) {
            if (jsonData.Success == true) {
                $("#divDeleteStockTransferPopup").modal("hide");
                if ($("#btn-search").length > 0) {
                    $("#btn-search").trigger("click");
                }
            }
            else {
                if (jsonData.Data != null && jsonData.Data != "") {
                    alertWaringPopup(false, jsonData.Data);
                }
            }
        },
        error: function (xhr, status, error) {
            alertWaringPopup(false, xhr.responseText);
        }
    });
});