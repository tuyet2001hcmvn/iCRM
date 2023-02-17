/*Begin-Spons*/
/*Popup Create From*/
$(document).on("click", "#btn-create-spons", function (e) {
    e.preventDefault();
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var type = $btn.data("type");
    $.ajax({
        type: "GET",
        url: '/Customer/Spons/_Create',
        data: { ProfileId: profileId, Type: type }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmSpons");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadSponsList = function (type) {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/Spons/_List/" + profileId + "?Type="+ type +"&isLoadContent=true";
    $("#contentSpons"+type+" table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#tableSpons .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var sponsId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/Spons/_Edit',
        data: { SponsId: sponsId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmSpons");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-spons", function () {
    ISD.SaveDataWithPopup("/Customer/Spons/Save", "#frmSpons", this, "#popupProfile");
    ReloadSponsList($(this).data("type"));
});

function reloadPage() {
    location.reload();
}

$(document).on("input", "#frmSpons #Value", function () {
    var projectValue = $(this).val();
    //alert(projectValue);
    if (projectValue) {
        var value = parseFloat(projectValue);
        $("#ValueDisplay").html(formatCurrency(value) + '  VNĐ');
    } else {
        $("#ValueDisplay").text('');
    }
});



$('#frmSpons #Value').inputFilter(function (value) {
    return /^-?\d*$/.test(value);
});
/*End-Spons*/