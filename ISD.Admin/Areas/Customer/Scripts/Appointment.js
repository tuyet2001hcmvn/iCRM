/*Popup Create From*/
$(document).on("click", "#btn-create-appointment", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
   // console.log(profileId);
    $.ajax({
        type: "GET",
        url: '/Customer/Appointment/_Create',
        data: { ProfileId: profileId}
    }).done(function (html) {
        $btn.button("reset");
        $(".with-search").select2();
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        $('input[type=radio][name="isVisitCabinetPro"]').trigger("change");
        ISD.ValidationOnModalPopup("#frmAppointment");
    });
    $btn.button("reset");
});

/*Edit button*/
$(document).on("click", "#contentAppointment .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var appointmentId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/Appointment/_Edit',
        data: { AppointmentId: appointmentId }
    }).done(function (html) {
        $btn.button("reset");
        $(".with-search").select2();
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        $('input[type=radio][name="isVisitCabinetPro"]').trigger("change");
        ISD.ValidationOnModalPopup("#frmAppointment");
    });
    $btn.button("reset");
});

/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-appointment", function () {
    ISD.SaveDataWithPopup("/Customer/Appointment/Save", "#frmAppointment", this, "#popupProfile");
});

/*Reload data*/
ReloadAppointmentList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/Appointment/_List/" + profileId + "?isLoadContent=true";
    $("#contentAppointment table tbody").load(requestUrl);
}

//$(document).on("change", "select[name='ShowroomCode']", function () {
//    var CustomerSourceCode = $(this).val();
//    $.ajax({
//        type: "POST",
//        url: "/Customer/Profile/GetSaleOrgByCustomerSource",
//        data: {
//            CustomerSourceCode: CustomerSourceCode
//        },
//        success: function (jsonData) {
//            $("#StoreId").html("");
//            $("#StoreId").append("<option value=''>-- Vui lòng chọn --</option>");
//            $.each(jsonData, function (index, value) {
//                $("#StoreId").append("<option value='" + value.StoreId + "'>" + value.StoreName + "</option>");
//            });
//        }
//    });
//});

var indexRowProduct = 0;
$(document).on('click', '.btn-addProduct', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".productCode").each(function (index, value) {
        indexRowProduct = index;
    });
    indexRowProduct++;

    var controlForm = $('.productCodeControls:first'),
        currentEntry = $(this).parents('.productCode:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.productcode').attr('name', 'productList[' + indexRowProduct + ']').val('');
    newEntry.find('.productcode').data('row', indexRowProduct);

    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.productCode:not(:first) .btn-addProduct')
        .removeClass('btn-addProduct').addClass('btn-removeProduct')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeProduct', function (e) {
    $(this).parents('.productCode:last').remove();

    $(".productCode").each(function (index, value) {
        $(this).find('.productcode').attr("name", "productList[" + index + "]");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", "#frmAppointment select[name='StoreId']", function () {
    loading2();
    var StoreId = $(this).val();
    $.ajax({
        type: "POST",
        url: "/Customer/Appointment/GetProductBySaleOrg",
        data: {
            StoreId: StoreId
        },
        success: function (jsonData) {
            $(".productcode").html("");
            $(".productcode").append("<option value=''>-- Vui lòng chọn --</option>");
            $.each(jsonData, function (index, value) {
                $(".productcode").append("<option value='" + value.ERPProductCode + "'>" + value.ProductCode + " - "  + value.ProductName + "</option>");
            });
        }
    });
});

//Có ghé thăm cabinet pro
$(document).on("change", 'input[type=radio][name="isVisitCabinetPro"]', function () {
    if ($(this).is(':checked')) {
        var isVisitCabinetPro = $(this).val();
        if (isVisitCabinetPro.toLowerCase() === "true") {
            $(".visit-cabinet").show();
        }
        else {
            $(".visit-cabinet").hide();
        }
    }
});