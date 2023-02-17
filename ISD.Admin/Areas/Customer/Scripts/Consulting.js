//Thêm mới đơn vị tư vấn giám sát
$(document).on("click", "#btn-create-consulting", function () {
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account'
        },
        success: function (html) {
            $("#divConsultingPopup").html("");

            $("#divConsultingPopup").html(html);
            $("#divConsultingPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divConsultingPopup #divProfileSearch").attr("id", 'divProfileSearch-consulting');
            $("#divConsultingPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-consulting');
            $("#divConsultingPopup #divProfileSearch-consulting").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileSearch-consulting .btn-profile-choose", function () {
    var profileId = $('#ProfileId').val();
    var partnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/ConsultingUnit/Save',
        data: {
            ProfileId: profileId,
            PartnerId: partnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divConsultingPopup #divProfileSearch-consulting").modal("hide");
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadConsultingList();
            }
            else {
                setModalMessage("#divProfileSearch-consulting .modalAlert", jsonData.Data);
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

$(document).on("click", "#contentConsulting .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/ConsultingUnit/SetMain',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadConsultingList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

ReloadConsultingList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var requestUrl = "/Customer/ConsultingUnit/_List/" + profileId + "?isLoadContent=true";
    $("#contentConsulting table tbody").load(requestUrl);
};