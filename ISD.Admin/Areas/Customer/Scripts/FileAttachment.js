/*Begin-FileAttachment*/
/*Popup Create From*/
$(document).on("click", "#btn-add-fileAttachment", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/FileAttachment/_Create',
        data: {
            ObjectId: profileId
        }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmFileAttachment");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadFileAttachmentList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/FileAttachment/_List/" + profileId + "?isLoadContent=true";
    $("#contentFileAttachment table tbody").load(requestUrl);
}
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-fileAttachment", function () {
    ISD.SaveDataWithPopup("/Customer/FileAttachment/Save", "#frmFileAttachment", this, "#popupProfile");
});
/*End-FileAttachment*/