/*Begin-ProfileContact*/
/*Popup Create From*/
$(document).on("click", "#btn-create-profile-contact", function () {
    var $btn = $(this);
    $btn.button("loading");

    var companyId = $("#frmEdit input[name='ProfileId']").val();
    var companyName = $("#frmEdit input[name='ProfileName']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/ProfileContact/_Create',
        data: {
            CompanyId: companyId,
            CompanyName: companyName
        }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmProfileContact");
    });
    $btn.button("reset");
});
/*Reload data*/
ReloadProfileContactList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/ProfileContact/_List/" + profileId + "?isLoadContent=true";
    $("#contentProfileContact table tbody").load(requestUrl);
}
/*Edit button*/
$(document).on("click", "#contentProfileContact .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/ProfileContact/_Edit',
        data: { ProfileId: profileId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmProfileContact");
    });
    $btn.button("reset");
});
/*Save Data*/
$(document).on("click", "#popupProfile #btn-save-profile-contact", function () {
    ISD.SaveDataWithPopup("/Customer/ProfileContact/Save", "#frmProfileContact", this, "#popupProfile");
});
/*Get district by province*/
$(document).on("change", "#frmProfileContact #ProvinceId", function () {
    var provinceId = $(this).val();
    $.ajax({
        type: "POST",
        url: "/MasterData/District/GetDistrictByProvince",
        data: {
            ProvinceId: provinceId
        },
        success: function (jsonData) {
            $("#frmProfileContact #DistrictId").html("");
            $("#frmProfileContact #DistrictId").append("<option value=''>-- Vui lòng chọn --</option>");
            $.each(jsonData, function (index, value) {
                $("#frmProfileContact #DistrictId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
            $("#frmProfileContact #DistrictId").trigger("change");
        }
    });
});
/*Get ward by district*/
$(document).on("change", "#frmProfileContact #DistrictId", function () {
    var DistrictId = $(this).val();

    $.ajax({
        type: "POST",
        url: "/MasterData/Ward/GetWardByDistrict",
        data: {
            DistrictId: DistrictId
        },
        success: function (jsonData) {
            var $ward = $("#frmProfileContact #WardId");
            $ward.html("");
            $ward.append("<option value=''>-- Vui lòng chọn --</option>");
            $.each(jsonData, function (index, value) {
                $ward.append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
        }
    });
});


var indexRowPerson = 0;
$(document).on('click', '.btn-addPersonInCharge', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".personCharge_fields").each(function (index, value) {
        indexRowPerson = index;
    });
    indexRowPerson++;

    var controlForm = $('.personInChargeControl:first'),
        currentEntry = $(this).parents('.personCharge_fields:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find('.personEmployeeCode').attr('name', 'personInChargeList[' + indexRowPerson + '].SalesEmployeeCode').val('');
    newEntry.find('.personRoleCode').attr('name', 'personInChargeList[' + indexRowPerson + '].RoleCode').val('');
    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.personCharge_fields:not(:first) .btn-addPersonInCharge')
        .removeClass('btn-addPersonInCharge').addClass('btn-removePersonInCharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonInCharge', function (e) {
    $(this).parents('.personCharge_fields:last').remove();

    $(".personCharge_fields").each(function (index, value) {
        $(this).find('.personEmployeeCode').attr("name", "personInChargeList[" + index + "].SalesEmployeeCode");
        $(this).find('.personRoleCode').attr("name", "personInChargeList[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});


var indexRowRole = 0;
$(document).on('click', '.btn-addRoleInCharge', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".roleCharge_fields").each(function (index, value) {
        indexRowRole = index;
    });
    indexRowRole++;

    var controlForm = $('.roleInChargeControl:first'),
        currentEntry = $(this).parents('.roleCharge_fields:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find('.roleChargeRoleId').attr('name', 'roleInChargeList[' + indexRowRole + '].RolesId').val('');
    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.roleCharge_fields:not(:first) .btn-addRoleInCharge')
        .removeClass('btn-addRoleInCharge').addClass('btn-removeRoleInCharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeRoleInCharge', function (e) {
    $(this).parents('.roleCharge_fields:last').remove();

    $(".roleCharge_fields").each(function (index, value) {
        $(this).find('.roleChargeRoleId').attr("name", "roleInChargeList[" + index + "].RolesId");
    });
    e.preventDefault();
    return false;
});

//$(document).on('change', 'input[name="PhoneBusiness"]', function () {
//    $('input[name="Phone"]').val($(this).val());
//});

///add More Phone
$(document).on('click', '#popupProfile .btn-addPhone', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    var controlForm = $('#popupProfile .phoneControls:first'),
        currentEntry = $(this).parents('#popupProfile .phonenumber:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('input').val('');
    controlForm.find('.phonenumber:not(:first) .btn-addPhone')
        .removeClass('btn-addPhone').addClass('btn-removePhone')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '#popupProfile .btn-removePhone', function (e) {
    $(this).parents('#popupProfile .phonenumber:last').remove();

    e.preventDefault();
    return false;
});

/*End-ProfileContact*/
