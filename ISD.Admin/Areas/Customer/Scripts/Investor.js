//Thêm mới chủ đầu tư
$(document).on("click", "#btn-create-investor", function () {
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account',
            //ProfileGroup: '10',//Chủ đầu tư
            ProfileType2: 'IsInvestor'
        },
        success: function (html) {
            $("#divInvestorPopup").html("");

            $("#divInvestorPopup").html(html);
            $("#divInvestorPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divInvestorPopup #divProfileSearch").attr("id", 'divProfileSearch-investor');
            $("#divInvestorPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-investor');
            $("#divInvestorPopup #divProfileSearch-investor").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileSearch-investor .btn-profile-choose", function () {
    var profileId = $('#ProfileId').val();
    var partnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/Investor/Save',
        data: {
            ProfileId: profileId,
            PartnerId: partnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divInvestorPopup #divProfileSearch-investor").modal("hide");
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadInvestorList();
            }
            else {
                setModalMessage("#divProfileSearch-investor .modalAlert", jsonData.Data);
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

$(document).on("click", "#contentInvestor .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Customer/Investor/SetMain',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadInvestorList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
            $btn.button('reset');
        }
    });
});

$(document).on("click", ".btn-view-contact", function () {
    var ProfileId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/ProfileContact/_List',
        data: {
            id: ProfileId,
            isLoadContent: true,
            ProfileType: "Opportunity"
        },
        success: function (html) {
            $('#divContactPopup #contact-information').html(html);
            $('#divContactPopup').modal("show");
        }
    });
});

ReloadInvestorList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var requestUrl = "/Customer/Investor/_List/" + profileId + "?isLoadContent=true";
    $("#contentInvestor table tbody").load(requestUrl);
};