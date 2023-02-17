//Thêm mới tổng thầu
$(document).on("click", "#btn-create-contractor", function () {
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account',
            //ProfileGroup: '12',//Tổng thầu
            ProfileType2: 'IsContractor'

        },
        success: function (html) {
            $("#divContractorPopup").html("");

            $("#divContractorPopup").html(html);
            $("#divContractorPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divContractorPopup #divProfileSearch").attr("id", 'divProfileSearch-contractor');
            $("#divContractorPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-contractor');
            $("#divContractorPopup #divProfileSearch-contractor").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileSearch-contractor .btn-profile-choose", function () {
    var profileId = $('#ProfileId').val();
    var partnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/GeneralContractor/Save',
        data: {
            ProfileId: profileId,
            PartnerId: partnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divContractorPopup #divProfileSearch-contractor").modal("hide");
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadContractorList();
            }
            else {
                setModalMessage("#divProfileSearch-contractor .modalAlert", jsonData.Data);
            }
        }
    });
});

function setModalMessage(div, message) {
    if (Array.isArray(message)) {
        var arr = [];
        $.each(message, function (i, item) {
            arr[i] = { err: item };
            $(div + " .modal-alert-message").append("<li>" + arr[i].err + "</li>");
        });
    }
    else {
        $(div + " .modal-alert-message").html(message);
    }
}

$(document).on("click", "#contentContractor .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/GeneralContractor/SetMain',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadContractorList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

ReloadContractorList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var requestUrl = "/Customer/GeneralContractor/_List/" + profileId + "?isLoadContent=true";
    $("#contentContractor table tbody").load(requestUrl);
};