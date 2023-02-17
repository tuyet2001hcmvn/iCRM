//Thêm mới tư vấn-thiết kế
$(document).on("click", "#btn-create-design", function () {
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account',
            //ProfileGroup: '11',//Thiết kế
            //ProfileGroup2: '16',//Thiết kế_Thi công
            ProfileType2: 'IsDesigner'

        },
        success: function (html) {
            $("#divDesignPopup").html("");

            $("#divDesignPopup").html(html);
            $("#divDesignPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divDesignPopup #divProfileSearch").attr("id", 'divProfileSearch-design');
            $("#divDesignPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-design');
            $("#divDesignPopup #divProfileSearch-design").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileSearch-design .btn-profile-choose", function () {
    var profileId = $('#ProfileId').val();
    var partnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/ConsultingDesign/Save',
        data: {
            ProfileId: profileId,
            PartnerId: partnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divDesignPopup #divProfileSearch-design").modal("hide");
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadDesignList();
            }
            else {
                setModalMessage("#divProfileSearch-design .modalAlert", jsonData.Data);
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

$(document).on("click", "#contentDesign .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/ConsultingDesign/SetMain',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadDesignList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

ReloadDesignList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var requestUrl = "/Customer/ConsultingDesign/_List/" + profileId + "?isLoadContent=true";
    $("#contentDesign table tbody").load(requestUrl);
};