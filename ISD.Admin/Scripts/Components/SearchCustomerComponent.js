
//button Tìm kiếm
$(document).on("click", "#btn-customer-search_" + ViewBagPreFix, function () {
    var $btn = $(this);
    $btn.button('loading');

    var data = $("#frmSearchPopup_Customer_" + ViewBagPreFix).serialize();
    data = data + "&prefix=" + ViewBagPreFix;

    $.ajax({
        type: "POST",
        url: "/Customer/Customer/_CustomerSearchResult",
        data: data,
        success: function (xhr, status, error) {
            $btn.button('reset');
            if (xhr.Code === 500) {
                //error
                $("#divCustomerSearch_" + ViewBagPreFix).modal("hide");
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divSearchPopupResult_Customer_" + ViewBagPreFix).html(xhr);
                ISD.Pagging();
            }
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            $("#divCustomerSearch_" + ViewBagPreFix).modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

//Close popup tìm KH => xóa mã KH
$('#divCustomerSearch_' + ViewBagPreFix).on('hidden.bs.modal', function () {
    $("#divCustomerSearch_" + ViewBagPreFix + " #CustomerCode").val("");
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
function alertModalPopupCustomer(isSuccess, message) {
    if (isSuccess == true) {
        setModalMessage("#divModalCustomerAlertSuccess_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalCustomerAlertSuccess_' + ViewBagPreFix).show();
        }, 500)
        setTimeout(function () {
            $('#divModalCustomerAlertSuccess_' + ViewBagPreFix).hide();
        }, 3000)
    }
    else if (isSuccess == false) {
        setModalMessage("#divModalCustomerAlertWarning_" + ViewBagPreFix, message);
        setTimeout(function () {
            $('#divModalCustomerAlertWarning_' + ViewBagPreFix).show();
        }, 500)
    }
}