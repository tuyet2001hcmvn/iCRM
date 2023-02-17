/*Begin-PersonInChage*/
/*Popup Create From*/
$(document).on("click", "#btn-create-personInCharge", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/PersonInCharge/_Create',
        data: { ProfileId: profileId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmPersonInCharge");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadPersonInChargeList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/PersonInCharge/_List/" + profileId + "?isLoadContent=true";
    $("#contentPersonInCharge table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentPersonInCharge .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var personInChargeId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/PersonInCharge/_Edit',
        data: { PersonInChargeId: personInChargeId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmPersonInCharge");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-personInCharge", function () {
    ISD.SaveDataWithPopup("/Customer/PersonInCharge/Save", "#frmPersonInCharge", this, "#popupProfile");
});
/*End-PersonInChage*/