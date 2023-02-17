//Thêm mới đối thủ
$(document).on("click", "#btn-create-opp-competitor", function () {
    var $btn = $(this);
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Competitor'
        },
        success: function (html) {
            $("#divOppCompetitorPopup").html("");

            $("#divOppCompetitorPopup").html(html);
            $("#divOppCompetitorPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divOppCompetitorPopup #divProfileSearch").attr("id", 'divProfileSearch-opp-competitor');
            $("#divOppCompetitorPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-opp-competitor');
            $("#divOppCompetitorPopup #divProfileSearch-opp-competitor").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileSearch-opp-competitor .btn-profile-choose", function () {
    var profileId = $('#ProfileId').val();
    var partnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/Competitor/Save',
        data: {
            ProfileId: profileId,
            PartnerId: partnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divOppCompetitorPopup #divProfileSearch-opp-competitor").modal("hide");
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadCompetitorList();
            }
            else {
                setModalMessage("#divProfileSearch-opp-competitor .modalAlert", jsonData.Data);
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

ReloadCompetitorList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var requestUrl = "/Customer/Competitor/_List/" + profileId + "?isLoadContent=true";
    $("#contentOppCompetitor table tbody").load(requestUrl);
};