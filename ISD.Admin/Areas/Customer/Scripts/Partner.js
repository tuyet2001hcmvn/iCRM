/*Begin-Partner*/
/*Popup Create From*/
$(document).on("click", "#btn-create-partner", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/Partner/_Create',
        data: { ProfileId: profileId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmPartner");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadPartnerList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/Partner/_List/" + profileId + "?isLoadContent=true";
    $("#contentPartner table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentPartner .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var partnerId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/Partner/_Edit',
        data: { PartnerId: partnerId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmPartner");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-partner", function () {
    ISD.SaveDataWithPopup("/Customer/Partner/Save", "#frmPartner", this, "#popupProfile");
});
/*End-Partner*/