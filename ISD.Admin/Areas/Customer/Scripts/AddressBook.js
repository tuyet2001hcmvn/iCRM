/*Begin-AddressBook*/
/*Popup Create From*/
$(document).on("click", "#btn-create-addressbook", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var profileType = $("input[name='ProfileCategory']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/AddressBook/_Create',
        data: { ProfileId: profileId, ProfileType: profileType }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmAddressBook");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadAddressBookList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/AddressBook/_List/" + profileId + "?isLoadContent=true";
    $("#contentAddressBook table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentAddressBook .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var addressBookId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/AddressBook/_Edit',
        data: { AddressBookId: addressBookId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmAddressBook");
        //Trigger Change => Load "Province", "District", "Ward" select
        $("#frmAddressBook input[name='CountryCode']:checked").trigger("click");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-addressbook", function () {
    ISD.SaveDataWithPopup("/Customer/AddressBook/Save", "#frmAddressBook", this, "#popupProfile", false, "reloadPage");
});

function reloadPage() {
    location.reload();
}
/*End-AddressBook*/