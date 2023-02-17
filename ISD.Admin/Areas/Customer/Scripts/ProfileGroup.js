var $popup = $("#popupProfile");

//Hàm load lại list
ReloadProfileGroupList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/ProfileGroup/_List/" + profileId + "?isLoadContent=true";
    $("#profileGroupList table tbody").load(requestUrl);
}

//su kien click button create
$("#btn-create-profileGroup").on("click", function () {
    var id = $('#ProfileId').val();
    $.ajax({
        type: "GET",
        url: '/Customer/ProfileGroup/_Create',
        data: { id }
    }).done(function (html) {
        $popup.find(".modal-content").html(html).end().modal();
        ISD.ValidationOnModalPopup("#frmCreateProfileGroup");
    });
});

ISD.SaveDataProfileGroup = function (action, frmName, e) {
    var $btn = $(e);
    var frm = $(frmName),
        formData = new FormData(),
        formParams = frm.serializeArray();

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            isHasFile = true;
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "POST",
            url: action,
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                if (jsonData.Success == true) {
                    $btn.button('reset');

                    if (jsonData.Data != null) {
                        if (jsonData.Data == "FORCE_LOGOUT") {
                            window.location.href = "/Permission/Auth/Login";
                        } else {
                            //Load lại lưới
                            ReloadProfileGroupList();
                            //Đóng popup
                            $("#popupProfile").modal("hide");
                        }
                    }
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
        });
        $btn.button('reset');
    }
}

$popup.on("click", "#btn-save-profileGroup", function () {
    ISD.SaveDataProfileGroup("/Customer/ProfileGroup/Create", "#frmCreateProfileGroup", this);
});
/*End-ProfileGroup*/