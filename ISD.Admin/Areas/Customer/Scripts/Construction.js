$('#ProjectValue').inputFilter(function (value) {
    return /^-?\d*$/.test(value);
});

$(document).on("input", '#ProjectValue', function () {
    var projectValue = $(this).val();
    //alert(projectValue);
    if (projectValue) {
        var value = parseInt(projectValue);
        $("#display-project-value").html(formatCurrency(value) + '  Tỷ');
    }
});

//Thêm mới thi công
//Căn mẫu
$(document).on("click", "#btn-create-internal", function () {
    var $btn = $(this);
    $btn.button('loading');
    var profileType = $("input[name='ProfileTypeCode']").val();
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/_ProfileFormSearch',
        data: {
            hasNoContact: true,
            ProfileType: profileType
        },
        success: function (html) {
            $("#divConstructionPopup").html("");

            $("#divConstructionPopup").html(html);
            $("#divConstructionPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divConstructionPopup #divProfileFormSearch").attr("id", 'divProfileFormSearch-internal');
            $("#divConstructionPopup #frmProfileFormSearchPopup").attr("id", 'frmProfileFormSearchPopup-internal');
            $("#divConstructionPopup #divProfileFormSearch-internal").modal("show");

            $('#divProfileFormSearch #frmProfileFormSearchPopup-internal select[name="ConstructionCategory"]').val("").trigger("change");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-internal input[name="ProjectValue"]').val("");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-internal select[name="IsWon"]').val("").trigger("change");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileFormSearch-internal .btn-save-profile-choose", function () {
    if ($('#divProfileFormSearch-internal #frmProfileFormSearchPopup-internal input[name="ProfileIsChoosen"]:checked').length) {
        var data = $('#divProfileFormSearch-internal #frmProfileFormSearchPopup-internal').serialize();
        var profileId = $('#ProfileId').val();
        $.ajax({
            type: "POST",
            url: '/Customer/Construction/SaveInternal',
            data: data +
                "&ProfileId=" + profileId,
            success: function (jsonData) {
                if (jsonData.Success === true) {
                    $("#divConstructionPopup #divProfileFormSearch-internal").modal("hide");
                    //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                    ReloadConstructionList();
                }
                else {
                    setModalMessage("#divProfileFormSearch-internal .modalAlert", jsonData.Data);
                    $("#divProfileFormSearch-internal .modalAlert").show();
                }
            }
        });
    }
    else {
        // no radio button was checked
        setModalMessage("#divProfileFormSearch-internal .modalAlert", "Vui lòng chọn đơn vị thi công trong danh sách");
        $("#divProfileFormSearch-internal .modalAlert").show();
    }
});
//$(document).on("click", "#divProfileFormSearch-internal .btn-profile-choose", function () {
//    var profileId = $('#ProfileId').val();
//    var partnerId = $(this).data('id');
//    $.ajax({
//        type: "POST",
//        url: '/Customer/Construction/SaveInternal',
//        data: {
//            ProfileId: profileId,
//            PartnerId: partnerId
//        },
//        success: function (jsonData) {
//            if (jsonData.Success === true) {
//                $("#divConstructionPopup #divProfileFormSearch-internal").modal("hide");
//                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
//                ReloadConstructionList();
//            }
//            else {
//                setModalMessage("#divProfileFormSearch-internal .modalAlert", jsonData.Data);
//            }
//        }
//    });
//});
//Đại trà
$(document).on("click", "#btn-create-competitor", function () {
    var $btn = $(this);
    $btn.button('loading');
    var profileType = $("input[name='ProfileTypeCode']").val();
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/_ProfileFormSearch',
        data: {
            hasNoContact: true,
            ProfileType: profileType
        },
        success: function (html) {
            $("#divConstructionPopup").html("");

            $("#divConstructionPopup").html(html);
            $("#divConstructionPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divConstructionPopup #divProfileFormSearch").attr("id", 'divProfileFormSearch-competitor');
            $("#divConstructionPopup #frmProfileFormSearchPopup").attr("id", 'frmProfileFormSearchPopup-competitor');
            $("#divConstructionPopup #divProfileFormSearch-competitor").modal("show");

            $('#divProfileFormSearch #frmProfileFormSearchPopup-competitor select[name="ConstructionCategory"]').val("").trigger("change");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-competitor input[name="ProjectValue"]').val("");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-competitor select[name="IsWon"]').val("").trigger("change");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileFormSearch-competitor .btn-save-profile-choose", function () {
    if ($('#divProfileFormSearch-competitor #frmProfileFormSearchPopup-competitor input[name="ProfileIsChoosen"]:checked').length) {
        var data = $('#divProfileFormSearch-competitor #frmProfileFormSearchPopup-competitor').serialize();
        var profileId = $('#ProfileId').val();
        $.ajax({
            type: "POST",
            url: '/Customer/Construction/SaveCompetitor',
            data: data +
                "&ProfileId=" + profileId,
            success: function (jsonData) {
                if (jsonData.Success === true) {
                    $("#divConstructionPopup #divProfileFormSearch-competitor").modal("hide");
                    //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                    ReloadConstructionList();
                }
                else {
                    setModalMessage("#divProfileFormSearch-competitor .modalAlert", jsonData.Data);
                    $("#divProfileFormSearch-competitor .modalAlert").show();
                }
            }
        });
    }
    else {
        // no radio button was checked
        setModalMessage("#divProfileFormSearch-competitor .modalAlert", "Vui lòng chọn đơn vị thi công trong danh sách");
        $("#divProfileFormSearch-competitor .modalAlert").show();
    }
});

//$(document).on("click", "#divProfileFormSearch-competitor .btn-profile-choose", function () {
//    var profileId = $('#ProfileId').val();
//    var partnerId = $(this).data('id');
//    $.ajax({
//        type: "POST",
//        url: '/Customer/Construction/SaveCompetitor',
//        data: {
//            ProfileId: profileId,
//            PartnerId: partnerId
//        },
//        success: function (jsonData) {
//            if (jsonData.Success === true) {
//                $("#divConstructionPopup #divProfileFormSearch-competitor").modal("hide");
//                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
//                ReloadConstructionList();
//            }
//            else {
//                setModalMessage("#divProfileFormSearch-competitor .modalAlert", jsonData.Data);
//            }
//        }
//    });
//});
//Thầu phụ
$(document).on("click", "#btn-create-subcontractors", function () {
    var $btn = $(this);
    $btn.button('loading');
    var profileType = $("input[name='ProfileTypeCode']").val();
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/_ProfileFormSearch',
        data: {
            hasNoContact: true,
            ProfileType: profileType
        },
        success: function (html) {
            $("#divConstructionPopup").html("");

            $("#divConstructionPopup").html(html);
            $("#divConstructionPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divConstructionPopup #divProfileFormSearch").attr("id", 'divProfileFormSearch-subcontractors');
            $("#divConstructionPopup #frmProfileFormSearchPopup").attr("id", 'frmProfileFormSearchPopup-subcontractors');
            $("#divConstructionPopup #divProfileFormSearch-subcontractors").modal("show");

            $('#divProfileFormSearch #frmProfileFormSearchPopup-subcontractors select[name="ConstructionCategory"]').val("").trigger("change");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-subcontractors input[name="ProjectValue"]').val("");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-subcontractors select[name="IsWon"]').val("").trigger("change");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileFormSearch-subcontractors .btn-save-profile-choose", function () {
    if ($('#divProfileFormSearch-subcontractors #frmProfileFormSearchPopup-subcontractors input[name="ProfileIsChoosen"]:checked').length) {
        var data = $('#divProfileFormSearch-subcontractors #frmProfileFormSearchPopup-subcontractors').serialize();
        var profileId = $('#ProfileId').val();
        $.ajax({
            type: "POST",
            url: '/Customer/Construction/SaveSubcontractors',
            data: data +
                "&ProfileId=" + profileId,
            success: function (jsonData) {
                if (jsonData.Success === true) {
                    $("#divConstructionPopup #divProfileFormSearch-subcontractors").modal("hide");
                    //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                    ReloadConstructionList();
                }
                else {
                    setModalMessage("#divProfileFormSearch-subcontractors .modalAlert", jsonData.Data);
                    $("#divProfileFormSearch-subcontractors .modalAlert").show();
                }
            }
        });
    }
    else {
        // no radio button was checked
        setModalMessage("#divProfileFormSearch-subcontractors .modalAlert", "Vui lòng chọn đơn vị thi công trong danh sách");
        $("#divProfileFormSearch-subcontractors .modalAlert").show();
    }
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

$(document).on("click", "#contentInternal .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/SetMainInternal',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadConstructionList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});


$(document).on("click", "#contentCompetitor .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/SetMainCompetitor',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadConstructionList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

$(document).on("click", "#contentSubcontractors .btn-main", function () {
    var OpportunityPartnerId = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/SetMainSubcontractors',
        data: {
            OpportunityPartnerId: OpportunityPartnerId
        },
        success: function (jsonData) {
            if (jsonData.Success === true) {
                //window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                ReloadConstructionList();
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

ReloadConstructionList = function () {
    var profileId = $("input[name='ProfileId']").val();
    var profileType = $("input[name='ProfileTypeCode']").val();
    var requestUrl = "/Customer/Construction/_ListInternal/" + profileId + "?Type=" + profileType + "&isLoadContent=true";
    $("#contentInternal table tbody").load(requestUrl);
    var requestUrl2 = "/Customer/Construction/_ListCompetitor/" + profileId + "?Type=" + profileType + "&isLoadContent=true";
    $("#contentCompetitor table tbody").load(requestUrl2);
    var requestUrl3 = "/Customer/Construction/_ListSubcontractors/" + profileId + "?Type=" + profileType + "&isLoadContent=true";
    $("#contentSubcontractors table tbody").load(requestUrl3);
};

//Cập nhật thi công
$(document).on("click", ".btn-edit-partner", function () {
    var $btn = $(this);
    var id = $btn.data("id");
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/_ProfileFormSearch',
        data: {
            OpportunityPartnerId: id,
        },
        success: function (html) {
            $("#divConstructionPopup").html("");

            $("#divConstructionPopup").html(html);
            $("#divConstructionPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divConstructionPopup #divProfileFormSearch").attr("id", 'divProfileFormSearch-partner');
            $("#divConstructionPopup #frmProfileFormSearchPopup").attr("id", 'frmProfileFormSearchPopup-partner');
            $("#divConstructionPopup #divProfileFormSearch-partner").modal("show");

            $('#divProfileFormSearch #frmProfileFormSearchPopup-partner select[name="ConstructionCategory"]').val("").trigger("change");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-partner input[name="ProjectValue"]').val("");
            $('#divProfileFormSearch #frmProfileFormSearchPopup-partner select[name="IsWon"]').val("").trigger("change");

            $btn.button('reset');
        }
    });
});

$(document).on("click", "#divProfileFormSearch-partner .btn-save-profile-choose", function () {
    var data = $('#divProfileFormSearch-partner #frmProfileFormSearchPopup-partner').serialize();
    var id = $(this).data("id");
    $.ajax({
        type: "POST",
        url: '/Customer/Construction/SaveEditPartner',
        data: data +
            "&OpportunityPartnerId=" + id,
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divConstructionPopup #divProfileFormSearch-partner").modal("hide");
                ReloadConstructionList();
            }
            else {
                setModalMessage("#divProfileFormSearch-partner .modalAlert", jsonData.Data);
                $("#divProfileFormSearch-partner .modalAlert").show();
            }
        }
    });
});