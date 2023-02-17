/*Begin-CertificateAC*/
/*Popup Create From*/
$(document).on("click", "#btn-create-certificateac", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/CertificateAC/_Create',
        data: { ProfileId: profileId}
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmCertificateAC");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadCertificateACList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/CertificateAC/_List/" + profileId + "?isLoadContent=true";
    $("#contentCertificateAC table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentCertificateAC .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var addressBookId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/CertificateAC/_Edit',
        data: { CertificateACId: addressBookId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmCertificateAC");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-certificateac", function () {
    ISD.SaveDataWithPopup("/Customer/CertificateAC/Save", "#frmCertificateAC", this, "#popupProfile", false, "ReloadCertificateACList");
});

function reloadPage() {
    location.reload();
}
/*End-CertificateAC*/