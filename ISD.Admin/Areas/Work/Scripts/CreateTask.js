
$(document).on('click', '.btn-addPerson', function (e) {
    e.preventDefault();
    $(".person").each(function (index, value) {
        indexRowPerson = index;
    });
    indexRowPerson++;

    var controlForm = $('.personControls:first'),
        currentEntry = $(this).parents('.person:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.employeeCode').attr('name', 'taskAssignList[' + indexRowPerson + '].SalesEmployeeCode').val('');
    newEntry.find('.employeeCode').data('row', indexRowPerson);
    newEntry.find('.typeCode').attr('name', 'taskAssignList[' + indexRowPerson + '].TaskAssignTypeCode').val('');
    newEntry.find('.roleName').removeClass('roleName_0');
    newEntry.find('.roleName').addClass('roleName_' + indexRowPerson);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName').html('');
    $("select").select2();

    controlForm.find('.person:not(:first) .btn-addPerson')
        .removeClass('btn-addPerson').addClass('btn-removePerson')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePerson', function (e) {
    $(this).parents('.person:last').remove();

    $(".person").each(function (index, value) {
        $(this).find('.employeeCode').attr("name", "taskAssignList[" + index + "].SalesEmployeeCode");
        $(this).find('.typeCode').attr("name", "taskAssignList[" + index + "].TaskAssignTypeCode");
    });
    e.preventDefault();
    return false;
});

$(document).on('click', '.btn-addGroup', function (e) {
    var indexRowGroup = 0;
    e.preventDefault();

    $(".group").each(function (index, value) {
        indexRowGroup = index;
    });
    indexRowGroup++;

    var controlForm = $('.groupControls:first'),
        currentEntry = $(this).parents('.group:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.groupCode').attr('name', 'taskAssignList[' + indexRowGroup + '].RolesCode').val('');
    newEntry.find('.groupCode').attr('id', 'taskAssignGroupList_' + indexRowGroup + '__RolesCode');
    newEntry.find('.groupCode').data('row', indexRowGroup);

    newEntry.find('.select2').remove();
    newEntry.find('.account-in-group').empty();
    newEntry.find('.roleName').html('');
    $("select").select2();

    controlForm.find('.group:not(:first) .btn-addGroup')
        .removeClass('btn-addGroup').addClass('btn-removeGroup')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeGroup', function (e) {
    $(this).parents('.group:last').remove();

    $(".group").each(function (index, value) {
        $(this).find('.groupCode').attr("name", "taskAssignList[" + index + "].RolesCode");
    });
    e.preventDefault();
    return false;
});

$(document).on('click', '.btn-addReporter', function (e) {
    var indexRowReporter = -1;
    e.preventDefault();

    $(".reporter").each(function (index, value) {
        indexRowReporter = index - 1;
    });
    indexRowReporter++;

    var controlForm = $('.reporterControls:first'),
        currentEntry = $(this).parents('.reporter:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.employeeCode_Reporter').attr('name', 'taskReporterList[' + indexRowReporter + '].SalesEmployeeCode');
    newEntry.find('.employeeCode_Reporter').attr('id', 'taskReporterList_' + indexRowReporter + '__SalesEmployeeCode');

    newEntry.find('.repoterTypeCode').attr('name', 'taskReporterList[' + indexRowReporter + '].TaskAssignTypeCode');
    newEntry.find('.repoterTypeCode').attr('id', 'taskReporterList_' + indexRowReporter + '__TaskAssignTypeCode');
    //newEntry.find('.employeeCode_Reporter').attr('name', 'taskReporterList[' + indexRowReporter + '].SalesEmployeeCode').val('');
    newEntry.find('.employeeCode_Reporter').data('row', indexRowReporter);

    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.reporter:not(:first) .btn-addReporter')
        .removeClass('btn-addReporter').addClass('btn-removeReporter')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeReporter', function (e) {
    $(this).parents('.reporter:last').remove();

    $(".reporter").each(function (index, value) {
        if (index > 0) {
            $(this).find('.employeeCode_Reporter').attr("name", "taskReporterList[" + (index - 1) + "].SalesEmployeeCode");
            $(this).find('.employeeCode_Reporter').attr("id", "taskReporterList_" + (index - 1) + "__SalesEmployeeCode");
        }
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".employeeCode", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName_" + row).html("");
            $(".roleName_" + row).html(jsonData);
        }
    });
});

$(document).on("click", ".btn-get-profile", function () {
    loading2();
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account'
        },
        success: function (html) {
            $(".divProfilePopup").html("");
            $(".divContactPopup").html("");

            $(".divProfilePopup").html(html);
            $(".with-search").select2();
            $(".divProfilePopup #divProfileSearch").modal("show");

            $("body").removeClass("loading2");
            $(".with-search").select2();
        }
    });
});

$(document).on("click", ".btn-get-contact", function () {
    loading2();
    var ProfileId = $("input[name='ProfileId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            ProfileId: ProfileId,
            hasNoContact: false,
            ProfileType: 'Contact'
        },
        success: function (html) {
            $(".divContactPopup").html("");
            $(".divProfilePopup").html("");

            $(".divContactPopup").html(html);
            $(".with-search").select2();
            $(".divContactPopup #divProfileSearch").modal("show");

            $("body").removeClass("loading2");
            $(".with-search").select2();
        }
    });
});

$(document).on("click", ".btn-get-construction", function () {
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account'
        },
        success: function (html) {
            $(".divProfilePopup").html("");
            $(".divContactPopup").html("");
            $(".divConstructionPopup").html("");

            $(".divConstructionPopup").html(html);
            $(".with-search").select2();
            $(".divConstructionPopup #divProfileSearch input[name='IsProfile']").val(false);
            $(".divConstructionPopup #divProfileSearch").modal("show");
        }
    });
});

$(document).on("click", ".btn-go-profile", function () {
    var id = $(this).data("id");
    if (id != null && id != "") {
        window.open("/Customer/Profile/Edit/" + id, "_blank");
    }
});

$(document).on("click", ".btn-go-contact", function () {
    var id = $(this).data("id");
    if (id != null && id != "") {
        window.open("/Customer/Profile/Edit/" + id, "_blank");
    }
});

$(document).on("click", ".btn-go-construction", function () {
    var id = $(this).data("id");
    if (id != null && id != "") {
        window.open("/Customer/Profile/Edit/" + id, "_blank");
    }
});

$(document).on("click", ".btn-del-profile", function () {
    $("input[name='ProfileId']").val("");
    $("input[name='ProfileName']").val("");
    $("input[name='ProfileAddress']").val("");
    $("input[name='Phone']").val("");
    $("input[name='Email']").val("");

    $("input[name='ContactId']").val("");
    $("input[name='ContactName']").val("");

    $("select[name='ProfileAddress']").html("");
    $("select[name='ProfileAddress']").append("<option>-- Vui lòng chọn --</option>");
    $("input[name='SalesSupervisorCode']").val("");
    $("input[name='SalesSupervisorName']").val("");
    $(".DepartmentName").html("");
    $("#task-summry-detail").empty();
    $("#task-summary").hide();
});

$(document).on("click", ".btn-del-contact", function () {
    $("input[name='ContactId']").val("");
    $("input[name='ContactName']").val("");
    $("input[name='ContactAddress']").val("");
    $("input[name='Phone']").val("");
    $("input[name='Email']").val("");

    GetAddressByContact("profile");
});

$(document).on("click", ".btn-del-construction", function () {
    $("input[name='ConstructionUnit']").val("");
    $("input[name='ConstructionUnitName']").val("");
    $("input[name='Construction_Phone']").val("");
    $("input[name='Construction_Email']").val("");
    $("input[name='Construction_ContactName']").val("");

    $("input[name='Construction_SalesSupervisorName']").val("");
    $(".Construction_DepartmentName").html("");
    $("select[name='ConstructionUnitContact']").find('option').not(':first').remove();
});

$(document).on("click", ".divProfilePopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");
    $("input[name='ProfileId']").val(id).trigger('change');
    $("input[name='ProfileName']").val(code + " | " + name);

    GetTHKHBy(id);

    $("#task-nearby").hide();

    $(".btn-go-profile").data("id", id);
});

function GetTHKHBy() {
    loading2();
    var ProfileId = $("input[name='ProfileId']").val();
    $.ajax({
        type: "POST",
        url: "/Work/Task/GetTHKHBy",
        data: {
            ProfileId: ProfileId
        },
        success: function (jsonData) {
            $("#task-summary").show();
            $("#task-summary-detail").empty();
            if (jsonData && jsonData.length > 0) {
                $.each(jsonData, function (index, item) {
                    // do something with `item` (or `this` is also `item` if you like)
                    $("#task-summary-detail").append('<div class="display-for"><a href="/Work/Task/Edit/' + item.TaskId + '" target="_blank">' + item.Summary + '</a></div>');
                });
            }
            else {
                $("#task-summary-detail").append('<div class="display-for">Chưa có lần thăm hỏi nào</div>');
            }
            $("body").removeClass("loading2");
        }
    });
}

function GetNearByAccountBy(Address) {
    //loading2();
    var ProfileId = $("input[name='ProfileId']").val();
    $("#tbl-nearby tbody").html('<tr><td valign="top" colspan="5" class="text-center">Đang tải dữ liệu, vui lòng đợi trong giây lát...</td></tr>');
    var table = $('#tbl-nearby').DataTable();
    table.clear();
    $.ajax({
        type: "POST",
        url: "/Work/Task/GetNearByAccountBy",
        data: {
            ProfileId: ProfileId,
            Address: Address
        },
        success: function (htmlData) {
            $("#task-nearby").show();
            $("#task-nearby-detail").html(htmlData);
            
            $("#tbl-nearby").DataTable({
                pageLength: 10,
                paging: true,
                destroy: true,
                scrollX: true,
                language: {
                    sProcessing: "Đang xử lý...",
                    sLengthMenu: "Xem _MENU_ mục",
                    sZeroRecords: "Không tìm thấy dòng nào phù hợp",
                    sInfo: "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                    sInfoEmpty: "Đang xem 0 đến 0 trong tổng số 0 mục",
                    sInfoFiltered: "(được lọc từ _MAX_ mục)",
                    sInfoPostFix: "",
                    sSearch: "Tìm nội dung:",
                    sUrl: "",
                    oPaginate: {
                        sFirst: "Đầu",
                        sPrevious: "&laquo;",
                        sNext: "&raquo;",
                        sLast: "Cuối"
                    },
                    columnDefs: [
                        { targets: [0, 1], visible: true },
                        { targets: 'no-sort', visible: false }
                    ],
                },
                "sDom": '<"top"flp>rt<"bottom"i><"clear">',
            });
            //$("body").removeClass("loading2");
        }
    });
}


$(document).on("click", ".divContactPopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");
    $("input[name='ContactId']").val(id).trigger('change');
    $("input[name='ContactName']").val(code + " | " + name);

    $(".btn-go-contact").data("id", id);
});

$(document).on("click", ".divConstructionPopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");
    $("input[name='ConstructionUnit']").val(id).trigger('change');
    $("input[name='ConstructionUnitName']").val(code + " | " + name);

    $(".btn-go-construction").data("id", id);
});

$(document).on("change", "input[name='ProfileId']", function () {
    GetAddressByContact("profile");
});

$(document).on("change", "input[name='ContactId']", function () {
    GetAddressByContact("contact");
});

$(document).on("change", "input[name='ConstructionUnit']", function () {
    //GetAddressByContact("construction");
    GetConstructionUnitInfo();
});
$(document).on("change", "select[name='ConstructionUnitContact']", function () {
    var ContactId = $("select[name='ConstructionUnitContact']").val();
    $.ajax({
        type: "POST",
        url: "/Work/Task/GetContactInfoBy",
        data: {
            ContactId: ContactId
        },
        success: function (jsonData) {
            $("input[name='Construction_Phone']").val(jsonData.ContactPhone);
            $("input[name='Construction_Email']").val(jsonData.ContactEmail);
        }
    });
});

function GetConstructionUnitInfo() {
    //loading2();
    var ConstructionUnit = $("input[name='ConstructionUnit']").val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/GetConstructionUnitInfo",
        data: {
            ConstructionUnit: ConstructionUnit
        },
        success: function (jsonData) {
            //NV kinh doanh
            $("input[name='Construction_SalesSupervisorName']").val(jsonData.SalesSupervisorName);

            //Danh sách liên hệ => đổ vào drop down list cho chọn
            if (jsonData.ContactList && jsonData.ContactList.length > 0) {
                //remove old options
                $("select[name='ConstructionUnitContact']").find('option').not(':first').remove();
                //add new options
                $.each(jsonData.ContactList, (index, item) => {
                    $("select[name='ConstructionUnitContact']").append('<option value="' + item.ContactId + '">' + item.ContactName + '</option>');
                    if (index === 0) {
                        $("select[name='ConstructionUnitContact']").val(item.ContactId).trigger("change");
                    }
                });
            }
            $("body").removeClass("loading2");
        }
    });
}

function GetAddressByContact(type) {
    loading2();
    var ContactId = $("input[name='ContactId']").val();
    var ProfileId = $("input[name='ProfileId']").val();
    var ParentTaskId = $("input[name='ParentTaskId']").val();
    if (ContactId == "" || ContactId == null) {
        ContactId = ProfileId;
    }
    if (type == "construction") {
        ContactId = $("input[name='ConstructionUnit']").val();
    }
    $.ajax({
        type: "POST",
        url: "/Work/Task/GetAddressByContact",
        data: {
            ContactId: ContactId,
            TaskId: ParentTaskId
        },
        success: function (jsonData) {
            if (type == "contact") {
                $("input[name='Phone']").val(jsonData.ContactPhone);
                $("input[name='Email']").val(jsonData.ContactEmail);
                $("input[name='ContactShortName']").val(jsonData.ContactShortName);
                $("input[name='ContactName']").val(jsonData.ContactShortName);
            }
            else if (type == "profile") {
                $("input[name='Phone']").val(jsonData.ContactPhone);
                $("input[name='Email']").val(jsonData.ContactEmail);
                $("input[name='ContactShortName']").val(jsonData.ContactShortName);

                if (jsonData.ProfileId != ProfileId) {
                    $("input[name='ContactId']").val("");
                    $("input[name='ContactName']").val("");
                    $("input[name='ContactAddress']").val("");
                }
                $("#ProfileAddress").html("");
                $("#ProfileAddress").append("<option value=''>-- Vui lòng chọn --</option>");
                if (jsonData.AddressIdList != null && jsonData.AddressIdList.length > 0) {
                    $.each(jsonData.AddressIdList, function (index, value) {
                        if (jsonData.ExistProfileAddress != null && jsonData.ExistProfileAddress != "" && jsonData.ExistProfileAddress == value.Address) {
                            $("#ProfileAddress").append("<option value='" + value.AddressBookId + "' selected='selected'>" + value.Address + "</option>");
                        }
                        else {
                            if (index == 0) {
                                $("#ProfileAddress").append("<option value='" + value.AddressBookId + "' selected>" + value.Address + "</option>");
                            }
                            else {
                                $("#ProfileAddress").append("<option value='" + value.AddressBookId + "'>" + value.Address + "</option>");
                            }
                        }
                    });
                }
                $("#ProfileAddress").trigger("change");
                $("input[name='SalesSupervisorName']").val(jsonData.SalesSupervisorName);
                $("input[name='SalesSupervisorCode']").val(jsonData.SalesSupervisorCode).trigger("change");

                $(".DepartmentName").html("");
                $(".DepartmentName").html(jsonData.DepartmentName);
            }
            else if (type == "construction") {
                $("input[name='Construction_Phone']").val(jsonData.MainContactPhone);
                $("input[name='Construction_Email']").val(jsonData.MainContactEmail);
                $("input[name='Construction_ContactName']").val(jsonData.MainContactName);

                $("input[name='Construction_SalesSupervisorName']").val(jsonData.SalesSupervisorName);
                $(".Construction_DepartmentName").html("");
                $(".Construction_DepartmentName").html(jsonData.DepartmentName);
            }
            $("body").removeClass("loading2");
        }
    });
}

var indexRowRole = 0;
$(document).on('click', '.btn-addContact', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".contact").each(function (index, value) {
        indexRowRole = index;
    });
    indexRowRole++;

    var controlForm = $('.contactControls:first'),
        currentEntry = $(this).parents('.contact:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find('.contactIdByProfile').attr('name', 'contactList[' + indexRowRole + '].ContactId').val('');
    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.contact:not(:first) .btn-addContact')
        .removeClass('btn-addContact').addClass('btn-removeContact')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeContact', function (e) {
    $(this).parents('.contact:last').remove();

    $(".contact").each(function (index, value) {
        $(this).find('.contactIdByProfile').attr("name", "contactList[" + index + "].ContactId");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", "select[name='WorkFlowId']", function () {
    var copy = $("#CopyFrom").val();
    if (!copy) {
        loading2();

        $.ajax({
            type: "POST",
            url: '/Work/Task/LoadFormByWorkFlow_Create',
            data: $("#frmCreate").serialize(),
            success: function (data) {

                $(".frmCreateTask").html(data);
                $(".field-validation-error[data-valmsg-for='Summary']").html("");
                $("#WorkFlowId").select2({
                    templateResult: formatState
                });
                $('.tooltip-icon').tooltip({
                    placement: 'left'
                });
                $("body").removeClass("loading2");
                $(".with-search").select2();
            }
        });
    }
});

$(document).on("change", "input[name='isRemind']", function () {
    var isRemind = $("input[name='isRemind']:checked").val();
    if (isRemind == undefined) {
        isRemind = null;
    }

    if (isRemind == "True") {
        $(".isRemind_true").removeClass("hidden");
    }
    else {
        $(".isRemind_true").addClass("hidden");
    }
});

$(document).on("click", ".btn-copy-profileaddress", function () {
    var ProfileId = $("select[name='ProfileAddress']").val();
    if (ProfileId != "") {
        $.ajax({
            type: "POST",
            url: '/Work/Task/GetAddressByProfile',
            data: {
                ProfileId: ProfileId
            },
            success: function (data) {
                if (data != null && data != "") {
                    var ProfileAddress = data.Address;
                    $("input[name='VisitAddress']").val(ProfileAddress);
                    GetLocationFromAddress(ProfileAddress);
                    $("select[name='VisitSaleOfficeCode']").val(data.SaleOfficeCode).trigger("change");
                    $("#DistrictIdValue").val(data.DistrictId);
                    $("#WardIdValue").val(data.WardId);
                    $("select[name='ProvinceId']").val(data.ProvinceId).trigger("change");
                }
            }
        });
    }
});

$(document).on("change", "select[name='ProvinceId']", function () {
    var ProvinceId = $(this).val();
    var DistrictId = $("#DistrictIdValue").val();
    $.ajax({
        type: "POST",
        url: "/MasterData/District/GetDistrictByProvince",
        data: {
            ProvinceId: ProvinceId
        },
        success: function (jsonData) {
            $("#DistrictId").html("");
            $("#DistrictId").append("<option value=''>-- Tất cả --</option>");
            $.each(jsonData, function (index, value) {
                $("#DistrictId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
            if (DistrictId) {
                $("select[name='DistrictId']").val(DistrictId).trigger("change");
            }
        }
    });
});


$(document).on("change", "select[name='DistrictId']", function () {
    var DistrictId = $(this).val();
    var WardId = $("#WardIdValue").val();
    $.ajax({
        type: "POST",
        url: "/MasterData/Ward/GetWardByDistrict",
        data: {
            DistrictId: DistrictId
        },
        success: function (jsonData) {
            $("#WardId").html("");
            $("#WardId").append("<option value=''>-- Tất cả --</option>");
            $.each(jsonData, function (index, value) {
                $("#WardId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
            if (WardId) {
                $("select[name='WardId']").val(WardId).trigger("change");
            }

        }
    });
});


function GetLocationFromAddress(address) {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            $("#lat").val(results[0].geometry.location.lat());
            $("#lng").val(results[0].geometry.location.lng());

            $("#lat_display").val(results[0].geometry.location.lat());
            $("#lng_display").val(results[0].geometry.location.lng());
        }
        else {
            console.log(status);
        }
    });
}

$(document).on("change", "input[name='Property3']", function () {
    var value = $(this).val();
    $(".numberProperty3").html("");
    var formatCurrencyProperty3 = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".numberProperty3").removeClass("hidden");
        $(".numberProperty3").html(formatCurrencyProperty3);
    }
    else {
        $(".numberProperty3").addClass("hidden");
    }
});

//Thăm hỏi khách hàng
function ChangeSummary_THKH() {
    //Summary
    function SetValueSummary_THKH() {
        var IsDisabledSummary = $("#IsDisabledSummary").val();
        if (IsDisabledSummary == true || IsDisabledSummary == "True" || IsDisabledSummary == "true") {
            var value = $("select[name='VisitTypeCode']").val();
            var formatStartDate = "";
            if (value != "") {
                var type = $("select[name='VisitTypeCode'] option:selected").text();
                var ContactShortName = $("input[name='ContactShortName']").val();
                var StartDate = $("input[name='StartDate']").val();
                if (StartDate != null && StartDate != "") {
                    formatStartDate = "-" + StartDate.split("-").reverse().join("/");
                }
                if (ContactShortName != null && ContactShortName != "") {
                    $("textarea[name='Summary']").val(type + formatStartDate + "-" + ContactShortName);
                    $("input[name='Summary']").val(type + formatStartDate + "-" + ContactShortName);
                }
                else {
                    $("textarea[name='Summary']").val(type + formatStartDate);
                    $("input[name='Summary']").val(type + formatStartDate);
                }
            }
            else {
                $("textarea[name='Summary']").val("");
                $("input[name='Summary']").val("");
            }
        }
    }
    //VisitTypeCode
    $(document).on("change", "select[name='VisitTypeCode']", function () {
        setTimeout(function () {
            SetValueSummary_THKH();
        }, 200);
    });
    //Profile
    $(document).on("change", "input[name='ProfileId']", function () {
        setTimeout(function () {
            SetValueSummary_THKH();
        }, 200);
    });
    //StartDate
    $(document).on("change", "input[id='dateFieldStartDate']", function () {
        setTimeout(function () {
            SetValueSummary_THKH();
        }, 200);
    });
}

//Bảo hành
function ChangeSummary_BH() {
    //Summary
    function SetValueSummary_BH() {
        var IsDisabledSummary = $("#IsDisabledSummary").val();
        if (IsDisabledSummary == true || IsDisabledSummary == "True" || IsDisabledSummary == "true") {
            var value = $("select[name='WorkFlowId']").val();
            var formatReceiveDate = "";
            if (value != "") {
                var type = $("select[name='WorkFlowId'] option:selected").text();
                var ContactShortName = $("input[name='ContactShortName']").val();
                var ReceiveDate = $("input[name='ReceiveDate']").val();
                if (ReceiveDate != null && ReceiveDate != "") {
                    formatReceiveDate = "-" + ReceiveDate.split("-").reverse().join("/");
                }
                if (ContactShortName != null && ContactShortName != "") {
                    $("textarea[name='Summary']").val(type + formatReceiveDate + "-" + ContactShortName);
                    $("input[name='Summary']").val(type + formatReceiveDate + "-" + ContactShortName);
                }
                else {
                    $("textarea[name='Summary']").val(type + formatReceiveDate);
                    $("input[name='Summary']").val(type + formatReceiveDate);
                }
            }
            else {
                $("textarea[name='Summary']").val("");
                $("input[name='Summary']").val("");
            }
        }
    }
    //Profile
    $(document).on("change", "input[name='ProfileId']", function () {
        setTimeout(function () {
            SetValueSummary_BH();
        }, 200);
    });
    //ReceiveDate
    $(document).on("change", "input[id='dateFieldReceiveDate']", function () {
        setTimeout(function () {
            SetValueSummary_BH();
        }, 200);
    });
}

//Điểm trưng bày
function ChangeSummary_GTB() {
    //Summary
    function SetValueSummary_GTB() {
        var IsDisabledSummary = $("#IsDisabledSummary").val();
        if (IsDisabledSummary == true || IsDisabledSummary == "True" || IsDisabledSummary == "true") {
            var value = $("select[name='WorkFlowId']").val();
            var formatStartDate = "";
            if (value != "") {
                var type = $("select[name='WorkFlowId'] option:selected").text();
                var ContactShortName = $("input[name='ContactShortName']").val();
                var StartDate = $("input[name='StartDate']").val();
                if (StartDate != null && ReceiveDate != "") {
                    formatStartDate = "-" + StartDate.split("-").reverse().join("/");
                }
                if (ContactShortName != null && ContactShortName != "") {
                    $("textarea[name='Summary']").val(type + formatStartDate + "-" + ContactShortName);
                    $("input[name='Summary']").val(type + formatStartDate + "-" + ContactShortName);
                }
                else {
                    $("textarea[name='Summary']").val(type + formatStartDate);
                    $("input[name='Summary']").val(type + formatStartDate);
                }
            }
            else {
                $("textarea[name='Summary']").val("");
                $("input[name='Summary']").val("");
            }
        }
    }
    //Profile
    $(document).on("change", "input[name='ProfileId']", function () {
        setTimeout(function () {
            SetValueSummary_GTB();
        }, 200);
    });
    //StartDate
    $(document).on("change", "input[id='dateFieldStartDate']", function () {
        setTimeout(function () {
            SetValueSummary_GTB();
        }, 200);
    });
}

$(document).on("change", "input[name='HasRequest']", function () {
    var WorkFlowId = $("select[name='WorkFlowId']").val();
    var Category = $("input[name='HasRequest']:checked").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/GetTaskStatusList',
        data: {
            WorkFlowId: WorkFlowId,
            Category: Category
        },
        success: function (jsonData) {
            $("select[name='TaskStatusId']").html("");
            if (jsonData != null && jsonData.length > 0) {
                $.each(jsonData, function (index, value) {
                    $("select[name='TaskStatusId']").append("<option value='" + value.TaskStatusId + "'>" + value.TaskStatusName + "</option>");
                });
            }
        }
    });
});

$(document).on("change", "input[name='IsAssignGroup']", function () {
    var IsAssingee = $("input[name='IsAssignGroup']:checked").val();
    if (IsAssingee == undefined) {
        IsAssingee = null;
    }

    if (IsAssingee == "True") {
        $(".assign_Group").removeClass("hidden");
        $(".assign_Personal").addClass("hidden");
    }
    else {
        $(".assign_Group").addClass("hidden");
        $(".assign_Personal").removeClass("hidden");
    }
});

$(document).on("click", ".btn-seemore", function () {
    var hasClass = $(".hidden-fields").hasClass("hidden");
    if (hasClass == true) {
        $(".hidden-fields").removeClass("hidden");
        $(".btn-seemore").html("Thu gọn");
    }
    else {
        $(".hidden-fields").addClass("hidden");
        $(".btn-seemore").html("Mở rộng");
    }
});

$(document).on('keydown', 'textarea', function (e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        this.value = this.value.substring(0, this.selectionStart) + "" + "\n" + this.value.substring(this.selectionEnd, this.value.length);
    }
});

/**
 * Thêm mới nhóm phân công
 * Khi user chọn Phân công theo nhóm thì cho phép thêm mới nhóm phân công ngoài nhóm từ hệ thống
 **/
var allids = [];
//$(document).on("click", ".btn-add-external-group", function () {
//    $.ajax({
//        type: "POST",
//        url: '/Work/Task/AddTaskGroup',
//        data: {

//        },
//        success: function (jsonData) {
//            $("#divTaskGroupPopup").html(jsonData);
//            $("#divTaskGroupPopup").modal("show");
//            allids = [];
//            $('#divTaskGroupPopup #btn-search').trigger('click');
//        }
//    });
//});

$(document).on('click', '.btn-addPersonGroup', function (e) {
    e.preventDefault();
    var indexRowPersonGroup = 0;
    $(".personGroup").each(function (index, value) {
        indexRowPersonGroup = index;
    });
    indexRowPersonGroup++;

    var controlForm = $('.personGroupControls:first'),
        currentEntry = $(this).parents('.personGroup:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.employeeGroupCode').attr('name', 'accountInGroup[' + indexRowPersonGroup + '].SalesEmployeeCode').val('');
    newEntry.find('.employeeGroupCode').attr('id', 'accountInGroup_' + indexRowPersonGroup + '__SalesEmployeeCode');
    newEntry.find('.employeeGroupCode').data('row', indexRowPersonGroup);
    newEntry.find('.roleName').removeClass('roleName_0');
    newEntry.find('.roleName').addClass('roleName_' + indexRowPersonGroup);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName').html('');
    $("select").select2();

    controlForm.find('.personGroup:not(:first) .btn-addPersonGroup')
        .removeClass('btn-addPersonGroup').addClass('btn-removePersonGroup')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonGroup', function (e) {
    $(this).parents('.personGroup:last').remove();

    $(".personGroup").each(function (index, value) {
        $(this).find('.employeeGroupCode').attr("name", "accountInGroup[" + index + "].SalesEmployeeCode");
        $(this).find('.employeeGroupCode').attr("id", "accountInGroup_" + index + "__SalesEmployeeCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".employeeGroupCode", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".personGroup .roleName_" + row).html("");
            $(".personGroup .roleName_" + row).html(jsonData);
        }
    });
});

//$(document).on("change", ".groupCode", function () {
//    var RolesCode = $(this).val();
//    var row = $(this).data('row');

//    loading2();

//    $.ajax({
//        type: "POST",
//        url: "/Work/Task/GetAccountInGroup",
//        data: {
//            RolesCode: RolesCode
//        },
//        success: function (jsonData) {
//            if (jsonData.Success === true) {
//                if (jsonData.Data && jsonData.Data.length > 0) {
//                    var labelContent = '<div class="row"><div><label class="control-label"> Nhân viên trong nhóm</label></div></div>';
//                    $('#taskAssignGroupList_' + row + '__RolesCode').closest('.group').find('.account-in-group').html(labelContent);
//                    var currentIndex = 0;
//                    jQuery.each(jsonData.Data, function (index, item) {
//                        // do something with `item` (or `this` is also `item` if you like)
//                        currentIndex = $('.taskAssignPersonGroup').length;

//                        //select content
//                        var selectContent = '';
//                        //role content
//                        var roleContent = '';
//                        if (jsonData.EmployeeList && jsonData.EmployeeList.length > 0) {
//                            var id = "taskAssignPersonGroupList_" + currentIndex + "__SalesEmployeeCode";
//                            var name = "taskAssignPersonGroupList[" + currentIndex + "].SalesEmployeeCode";
//                            selectContent = "<select id=\"" + id + "\" name=\"" + name + "\" class=\"taskAssignPersonGroup\">";
//                            for (var i in jsonData.EmployeeList) {
//                                var selected = "";
//                                if (jsonData.EmployeeList[i].SalesEmployeeCode === item.SalesEmployeeCode) {
//                                    selected = "selected";
//                                }
//                                selectContent += '<option value="' + jsonData.EmployeeList[i].SalesEmployeeCode + '"' + selected + '>' + jsonData.EmployeeList[i].SalesEmployeeName + '</option>';
//                            }
//                            selectContent += '</select>';


//                            if (item.RolesName) {
//                                roleContent = '<div class="col-md-5 input-group" style="margin-top:10px;">';
//                                roleContent += '<div class="display-for roleName roleName_' + currentIndex + '" > ' + item.RolesName + '</div>';
//                                roleContent += '<span class="input-group-btn">';
//                                if (index > 0) {
//                                    roleContent += '<button class="btn btn-danger btn-removePersonTaskGroup" type="button">';
//                                    roleContent += '<span class="glyphicon glyphicon-minus"></span>';
//                                }
//                                else {
//                                    roleContent += '<button class="btn btn-default btn-addPersonTaskGroup" type="button">';
//                                    roleContent += '<span class="glyphicon glyphicon-plus"></span>';
//                                }

//                                roleContent += '</button>';
//                                roleContent += '</span>';
//                                roleContent += '</div>';
//                            }
//                        }


//                        var bodyContent = '<div class="person-in-group row">';
//                        bodyContent += '<div class="col-md-5 no-padding" style="margin-top:10px;">';
//                        bodyContent += selectContent;
//                        bodyContent += '</div>';
//                        bodyContent += roleContent;
//                        bodyContent += '</div>';
//                        $('#taskAssignGroupList_' + row + '__RolesCode').closest('.group').find('.account-in-group').append(bodyContent);
//                        $('.group select').select2();
//                    });
//                }
//            }
//        }
//    });
//});

$(document).on("change", ".groupCode", function () {
    var RolesCode = $(this).val();
    var RolesName = $(".groupCode option:selected").text();
    $(".btn-remove-external-group").remove();
    $(".btn-edit-external-group").remove();
    if (RolesCode) {
        loading2();
        var accountInGroup = [];

        $(".person-in-group").each(function (index, value) {
            var code = $(this).find('input[name="taskAssignPersonGroupList[' + index + '].SalesEmployeeCode"]').val();
            accountInGroup.push(code);
        });
        $.ajax({
            type: "POST",
            url: "/Work/Task/GetAccountInGroup",
            data: {
                RolesCode: RolesCode,
                accountInGroup: accountInGroup
            },
            success: function (htmlData) {
                $("#divTaskPersonGroupPopup #group-name").html(RolesName);
                $("#divTaskPersonGroupPopup #divSearchResult").html(htmlData);
                //ISD.Pagging(false, 100);

                $("#divTaskPersonGroupPopup #tbl-employee").DataTable({
                    pageLength: 100,
                    paging: true,
                    destroy: true,
                    language: {
                        sProcessing: "Đang xử lý...",
                        sLengthMenu: "Xem _MENU_ mục",
                        sZeroRecords: "Không tìm thấy dòng nào phù hợp",
                        sInfo: "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                        sInfoEmpty: "Đang xem 0 đến 0 trong tổng số 0 mục",
                        sInfoFiltered: "(được lọc từ _MAX_ mục)",
                        sInfoPostFix: "",
                        sSearch: "Tìm nội dung:",
                        sUrl: "",
                        oPaginate: {
                            sFirst: "Đầu",
                            sPrevious: "&laquo;",
                            sNext: "&raquo;",
                            sLast: "Cuối"
                        },
                        columnDefs: [
                            { targets: [0, 1], visible: true },
                            { targets: 'no-sort', visible: false }
                        ],
                    },
                    "sDom": '<"top"flp>rt<"bottom"i><"clear">',
                });
                $("body").removeClass("loading2");
                $('#divTaskPersonGroupPopup #tbl-employee_filter').show();
                $('#divTaskPersonGroupPopup #tbl-employee_filter').css('text-align', 'right');

                $("#divTaskPersonGroupPopup #CheckAll").prop("checked", true).trigger('change');
                $("#divTaskPersonGroupPopup").modal("show");
            }
        });

        //display remove group button
        if (isGuid(RolesCode)) {
            var removeGroupBtn = '<span title="Xóa nhóm phân công"><button class="btn btn-danger btn-remove-external-group" type="button">';
            
            removeGroupBtn += '<span class="glyphicon glyphicon-remove"></span>';
            removeGroupBtn += '</button></span>';
            var editGroupBtn ='<span title="Chỉnh sửa nhóm phân công">' + 
                '<a href="/Work/AssignedGroup/Edit/' + RolesCode+'" target="_blank" class="btn btn-default btn-edit-external-group">'+
                                    '<i class="fa fa-arrow-circle-o-right"></i>'+
                                '</a>'
            '</span>'
            $(".task-group").find(".input-group-btn").append(removeGroupBtn).append(editGroupBtn);
        }
    }
  
});
$(document).on('click', '.btn-refresh-group', function () {
    var $btn = $(this);
    $btn.button("loading");
    $.ajax({
        type: "GET",
        url: "/Work/Task/GetAssignedGroupForDropdown",
        dataType: "json",
        success: function (res) {
            $('#TaskRolesList').html("");
            $('#TaskRolesList').append(new Option("-- Chọn nhóm thực hiện --", "",true))
            $.each(res.Data, function (i, item) {
                $('#TaskRolesList').append(new Option(item.RolesName, item.RolesCode));
            });
            $btn.button("reset");
            
        },
        error: function (res) {
            alertPopup(false, res);
            $btn.button("reset");
        }
    });
});

function isGuid(stringToTest) {
    if (stringToTest[0] === "{") {
        stringToTest = stringToTest.substring(1, stringToTest.length - 1);
    }
    var regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
    return regexGuid.test(stringToTest);
}

$(document).on('click', '.btn-addPersonTaskGroup', function (e) {
    e.preventDefault();
    var indexRowPersonTaskGroup = 0;
    $(".person-in-group").each(function (index, value) {
        indexRowPersonTaskGroup = index;
    });
    indexRowPersonTaskGroup++;

    var controlForm = $('.account-in-group:first'),
        currentEntry = $(this).parents('.person-in-group:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.taskAssignPersonGroup').attr('name', 'taskAssignPersonGroupList[' + indexRowPersonTaskGroup + '].SalesEmployeeCode').val('');
    newEntry.find('.taskAssignPersonGroup').attr('id', 'taskAssignPersonGroupList_' + indexRowPersonTaskGroup + '__SalesEmployeeCode');
    newEntry.find('.taskAssignPersonGroup').data('row', indexRowPersonTaskGroup);
    newEntry.find('.roleName').removeClass('roleName_0');
    newEntry.find('.roleName').addClass('roleName_' + indexRowPersonTaskGroup);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName').html('');
    $("select").select2();

    controlForm.find('.person-in-group:not(:first) .btn-addPersonTaskGroup')
        .removeClass('btn-addPersonTaskGroup').addClass('btn-removePersonTaskGroup')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonTaskGroup', function (e) {
    $(this).parents('.person-in-group:last').remove();

    $(".person-in-group").each(function (index, value) {
        $(this).find('.taskAssignPersonGroup').attr("name", "taskAssignPersonGroupList[" + index + "].SalesEmployeeCode");
        $(this).find('.taskAssignPersonGroup').attr("id", "taskAssignPersonGroupList_" + index + "__SalesEmployeeCode");
        $(this).find('.taskAssignPersonGroup').attr("name", "taskAssignPersonGroupList[" + index + "].SalesEmployeeName");
        $(this).find('.taskAssignPersonGroup').attr("id", "taskAssignPersonGroupList_" + index + "__SalesEmployeeName");
        $(this).find('.taskAssignPersonGroup').attr("name", "taskAssignPersonGroupList[" + index + "].TaskAssignTypeCode");
        $(this).find('.taskAssignPersonGroup').attr("id", "taskAssignPersonGroupList_" + index + "__TaskAssignTypeCode");
    });
    e.preventDefault();
    return false;
});

$(document).on('change', '.taskAssignPersonGroup', function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".person-in-group .roleName_" + row).html("");
            $(".person-in-group .roleName_" + row).html(jsonData);
        }
    });
});

$(document).on("click", ".btn-save-task-group", function () {
    //var data = $("#frmTaskGroup").serialize();
    var GroupName = $("#frmTaskGroup #GroupName").val();
    var accountInGroup = allids;

    //var table = $('#divTaskGroupPopup #tbl-employee').DataTable();
    //table.rows().nodes().to$().find('input[type="checkbox"]').each(function () {
    //    if ($(this).is(':checked')) {
    //        accountInGroup.push($(this).val());
    //    }
    //});
    $.ajax({
        type: "POST",
        url: '/Work/Task/SaveTaskGroup',
        data: {
            GroupName: GroupName,
            accountInGroup: accountInGroup
        },
        success: function (jsonData) {
            if (jsonData.Success) {
                $("#divTaskGroupPopup").modal("hide");
                $(".groupCode option:first").after('<option value="' + jsonData.Data.id + '">' + jsonData.Data.name + '</option>');
            }
            else {
                if (jsonData.Data) {
                    alertTaskGroupModalPopup(jsonData.Data);
                }
            }
        }
    });
});

//Tìm thông tin nhân viên
$(document).on('click', '#divTaskGroupPopup #btn-search', function myfunction() {
    var controller = "/Work/Task/_SearchEmployee";
    var $btn = $(this);
    $btn.button('loading');

    $.ajax({
        type: "POST",
        url: controller,
        data: $("#frmTaskGroup").serializeArray(),
        success: function (xhr, status, error) {
            $btn.button('reset');
            if (xhr.Code === 500) {
                //error
                alertTaskGroupModalPopup(false, xhr.Data);
            } else {
                //success
                $("#divSearchResult").html(xhr);
                ISD.Pagging(false);
                $.each(allids, function (index, value) {
                    $('#divTaskGroupPopup').find('input[type=checkbox][value=' + value + ']').prop("checked", true);
                });
            }
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            alertTaskGroupModalPopup(false, xhr.responseText);
        }
    });
});

//Check all chọn nhân viên
$(document).on('change', '#divTaskGroupPopup #CheckAll', function () {
    var table = $('#divTaskGroupPopup #tbl-employee').DataTable();
    if ($('#divTaskGroupPopup #CheckAll').is(":checked")) {
        //$('.salesEmployeeList').prop("checked", true);
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", true);
    }
    else {
        //$('.salesEmployeeList').prop("checked", false);
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", false);
    }
});

$(document).on('change', '#divTaskPersonGroupPopup #CheckAll', function () {
    var table = $('#divTaskPersonGroupPopup #tbl-employee').DataTable();
    if ($('#divTaskPersonGroupPopup #CheckAll').is(":checked")) {
        //$('.salesEmployeeList').prop("checked", true);
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", true);
    }
    else {
        //$('.salesEmployeeList').prop("checked", false);
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", false);
    }
});

$(document).on("click", ".btn-save-task-person-group", function () {
    var bodyLength = $('.person-in-group').length;
    if (bodyLength === 0) {
        var labelContent = '<div class="row"><div><label class="control-label"> Nhân viên trong nhóm</label></div></div>';
        $('.assign_Group').find('.account-in-group').html(labelContent);
    }

    var table = $('#divTaskPersonGroupPopup #tbl-employee').DataTable();
    table.rows().nodes().to$().find('input[type="checkbox"]').each(function (index, element) {
        if ($(this).is(':checked:not(":disabled")')) {
            //employee content
            var id = "taskAssignPersonGroupList_" + bodyLength + "__SalesEmployeeCode";
            var name = "taskAssignPersonGroupList[" + bodyLength + "].SalesEmployeeCode";

            var idTaskAssignTypeDisplay = "taskAssignPersonGroupList_" + bodyLength + "__TaskAssignTypeCode";
            var nameTaskAssignTypeDisplay = "taskAssignPersonGroupList[" + bodyLength + "].TaskAssignTypeCode";

            var employeeContent = '<input type="text" class="form-control taskAssignPersonGroup" readonly name="' + nameDisplay + '" id="' + idDisplay + '" value="' + $(this).data('name') + '" />';
            employeeContent += '<input type="hidden" taskAssignPersonGroup name="' + name + '" id="' + id + '" value="' + $(this).val() + '" />';
            employeeContent += '<input type="hidden" taskAssignPersonGroup name="' + nameTaskAssignTypeDisplay + '" id="' + idTaskAssignTypeDisplay + '" value="' + $("#TaskRolesList").val() + '" />';

            //role content
            roleContent = '<div class="col-md-5 input-group" style="margin-top:10px;">';
            var idDisplay = "taskAssignPersonGroupList_" + bodyLength + "__SalesEmployeeName";
            var nameDisplay = "taskAssignPersonGroupList[" + bodyLength + "].SalesEmployeeName";

            roleContent += '<div class="display-for roleName roleName_' + bodyLength + '" > ' + $(this).data('department') + '</div>';
            roleContent += '<span class="input-group-btn">';
            //remove button
            roleContent += '<button class="btn btn-danger btn-removePersonTaskGroup" type="button">';
            roleContent += '<span class="glyphicon glyphicon-minus"></span>';

            //body content
            var bodyContent = '<div class="person-in-group row">';
            bodyContent += '<div class="col-md-5 no-padding" style="margin-top:10px;">';
            bodyContent += employeeContent;
            bodyContent += '</div>';
            bodyContent += roleContent;
            bodyContent += '</div>';
            $('.assign_Group').find('.account-in-group').append(bodyContent);

            bodyLength++;
        }
    });

    $('#divTaskPersonGroupPopup').modal('hide');
  
});

//xóa nhóm tự tạo
$(document).on('click', '.btn-remove-external-group', function () {
    var id = $('#TaskRolesList').val();
    var controller = '/Work/Task/DeleteExternalTaskGroup';
    $("#divDeleteTaskGroupPopup").modal("show");
    var itemName = $('#TaskRolesList option:selected').text();
    //set title
    $("#divDeleteTaskGroupPopup .modal-title .item-name").html(itemName);
    //set text
    var text = $("#divDeleteTaskGroupPopup .alert-message").html();
    //Replace new text
    text = text.replace(/"([^"]*)"/g, '"' + itemName + '"');
    text = String.format(text, itemName);
    //Show new text
    $("#divDeleteTaskGroupPopup .alert-message").html(text);

    //get id, controller
    $("#divDeleteTaskGroupPopup #id").val(id);
    $("#divDeleteTaskGroupPopup #controller").val(controller);
});

$(document).on('click', '#btn-confirm-delete-task-group', function () {
    var $btn = $(this);
    $btn.button('loading');
    var controller = $('form[id="frmDeleteTaskGroupConfirm"] #controller').val();
    var id = $('form[id="frmDeleteTaskGroupConfirm"] #id').val();
    $.ajax({
        type: "POST",
        url: controller,
        data: $('form[id="frmDeleteTaskGroupConfirm"]').serialize(),
        success: function (jsonData) {
            $btn.button('reset');
            $("#divDeleteTaskGroupPopup").modal("hide");
            if (jsonData.Success === true) {
                alertPopup(true, jsonData.Data);
            }
            else {
                if (jsonData.Data !== null && jsonData.Data !== "") {
                    alertPopup(false, jsonData.Data);
                }
            }
            $('.groupCode option[value="' + id + '"]').remove();
            $('.groupCode').val('').trigger('change');
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            $("#divDeleteTaskGroupPopup").modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });
});

$(document).on('change', '.salesEmployeeList', function () {
    if ($(this).is(":checked")) {
        allids.push($(this).val());
    }
    else {
        var removeIndex = allids.findIndex(e => e === $(this).val());
        if (removeIndex > -1) {
            allids.splice(removeIndex, 1);
        }
    }
});

//Chọn địa chỉ => load thông tin danh sách các khách hàng cùng khu vực
$(document).on("change", 'select[name="ProfileAddress"]', function () {
    var Address = $(this).val();
    if (Address) {
        GetNearByAccountBy(Address);
    }
});