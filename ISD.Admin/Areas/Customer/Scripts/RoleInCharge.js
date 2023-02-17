/*Begin-RoleInCharge*/
/*Popup Create From*/
$(document).on("click", "#btn-create-roleInCharge", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/RoleInCharge/_Create',
        data: { ProfileId: profileId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmRoleInCharge");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadRoleInChargeList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/RoleInCharge/_List/" + profileId + "?isLoadContent=true";
    $("#contentRoleInCharge table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentRoleInCharge .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var roleInChargeId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/RoleInCharge/_Edit',
        data: { RoleInChargeId: roleInChargeId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmRoleInCharge");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-roleInCharge", function () {
    ISD.SaveDataWithPopup("/Customer/RoleInCharge/Save", "#frmRoleInCharge", this, "#popupProfile");
});
/*End-RoleInCharge*/