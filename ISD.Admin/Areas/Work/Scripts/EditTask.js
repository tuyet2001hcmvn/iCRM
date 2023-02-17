
$(document).on('click', '.btn-addPerson', function (e) {
    var indexRowPerson = 0;
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
    newEntry.find('.groupCode').data('row', indexRowGroup);

    newEntry.find('.select2').remove();
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
            $(".divContactPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-contact');
            $(".divContactPopup #divProfileSearch").attr("id", 'divProfileSearch-contact');
            $(".divContactPopup #divProfileSearch-contact").modal("show");

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

//$(document).on("click", ".btn-profile-choose", function () {
//    var id = $(this).data("id");
//    $("input[name='ProfileId']").val(id).trigger('change');
//    $("#frmProfileSearchPopup input[name='ProfileId']").val("").trigger('change');
//});

$(document).on("click", ".divProfilePopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");
    $("input[name='ProfileId']").val(id).trigger('change');
    $("input[name='ProfileName']").val(code + " | " + name);

    $(".btn-go-profile").data("id", id);
});

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
    var TaskId = $("input[name='TaskId']").val();
    var ContactId = $("input[name='ContactId']").val();
    var ProfileId = $("input[name='ProfileId']").val();
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
            TaskId: TaskId
        },
        success: function (jsonData) {
            if (type == "contact") {
                $("input[name='Phone']").val(jsonData.ContactPhone);
                $("input[name='Email']").val(jsonData.ContactEmail);
                $("input[name='ContactName']").val(jsonData.ContactShortName);
            }
            else if (type == "profile") {
                $("input[name='Phone']").val(jsonData.ContactPhone);
                $("input[name='Email']").val(jsonData.ContactEmail);

                if (jsonData.ProfileId != ProfileId) {
                    $("input[name='ContactId']").val("");
                    $("input[name='ContactName']").val("");
                    $("input[name='ContactAddress']").val("");
                }
                $("#ProfileAddress").html("");
                $("#ProfileAddress").append("<option value=''>-- Vui lòng chọn --</option>");
                //if (jsonData.AddressList != null && jsonData.AddressList.length > 0) {
                //    $.each(jsonData.AddressList, function (index, value) {
                //        if (jsonData.ExistProfileAddress != null && jsonData.ExistProfileAddress != "" && jsonData.ExistProfileAddress == value) {
                //            $("#ProfileAddress").append("<option value='" + value + "' selected>" + value + "</option>");
                //        }
                //        else {
                //            if (index == 0) {
                //                $("#ProfileAddress").append("<option value='" + value + "' selected>" + value + "</option>");
                //            }
                //            else {
                //                $("#ProfileAddress").append("<option value='" + value + "'>" + value + "</option>");
                //            }
                //        }
                //    });
                //}
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

//$(document).on("change", "select[name='ContactId']", function () {
//    var ContactId = $(this).val();
//    var ProfileId = $("input[name='ProfileId']").val();
//    if (ContactId == "") {
//        ContactId = ProfileId;
//    }
//    $.ajax({
//        type: "POST",
//        url: "/Work/Task/GetAddressByContact",
//        data: {
//            ContactId: ContactId
//        },
//        success: function (jsonData) {
//            $(".editTask input[name='ProfileAddress']").val(jsonData.ContactAddress);
//            $(".editTask input[name='Phone']").val(jsonData.ContactPhone);
//            $(".editTask input[name='Email']").val(jsonData.ContactEmail);
//        }
//    });
//});

//Thêm mới product
$(document).on("click", ".btn-add-product", function () {
    var $btn = $(this);
    $btn.button("loading");

    var TaskId = $("input[name='TaskId']").val();
    var Type = $("input[name='Type']").val();
    var WorkFlowId = $("select[name='WorkFlowId']").val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/GetTaskProduct",
        data: {
            TaskId: TaskId,
            Type: Type,
            WorkFlowId: WorkFlowId
        },
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success == false) {
                alertPopup(false, jsonData.Data);
            }
            else {
                $("#divProductPopup").html("");
                $("#divProductPopup").html(jsonData);

                $('.serial-number').inputFilter(function (value) {
                    return /^-?\d*$/.test(value);
                });

                $("#divProductPopup").modal("show");

            }
        }
    });
});

//Edit product
$(document).on("click", ".btn-edit-product", function () {
    var $btn = $(this);
    $btn.button("loading");
    var id = $(this).data("id");
    var TaskId = $("input[name='TaskId']").val();
    var Type = $("input[name='Type']").val();
    var WorkFlowId = $("select[name='WorkFlowId']").val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/GetTaskProduct",
        data: {
            TaskId: TaskId,
            TaskProductId: id,
            Type: Type,
            WorkFlowId: WorkFlowId
        },
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success == false) {
                alertPopup(false, jsonData.Data);
            }
            else {
                $("#divProductPopup").html("");
                $("#divProductPopup").html(jsonData);

                $('.serial-number').inputFilter(function (value) {
                    return /^-?\d*$/.test(value);
                });

                $("#divProductPopup").modal("show");
            }
        }
    });
});

$(document).on("click", "#divProductSearch .btn-product-choose", function () {
    $('#divProductSearch').modal("hide");
    //$("#divProductPopup").show();

    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");
    var serial = $(this).data("serial");

    //Lấy đơn vị tính
    $.ajax({
        type: "POST",
        url: "/Work/Task/GetProductUnitById",
        data: {
            ProductId: id,
            ProductWarrantyId: serial
        },
        success: function (jsonData) {
            $("#divProductPopup input[name='Unit']").val(jsonData.Unit).trigger("change");
            $("#divProductPopup input[name='SerialNumber']").val(jsonData.SerriNo).trigger("change");
        }
    });
    $("#divProductPopup input[name='ProductId']").val(id).trigger("change");
    $("#divProductPopup input[name='TaskProductName']").val(code + " | " + name);
    $("input[name='Qty']").focus();
});

$(document).on("change", "select[name='ProductCategoryCode']", function () {
    var ProductCategoryCode = $(this).val();
    var WorkFlowId = $("#WorkFlowId").val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/GetUsualErrorByProductCategory",
        data: {
            ProductCategoryCode: ProductCategoryCode,
            WorkFlowId: WorkFlowId
        },
        success: function (jsonData) {
            $("#UsualErrorCode").html("");
            //$("#UsualErrorCode").append("<option value=''>-- Vui lòng chọn --</option>");
            if (jsonData != null && jsonData.length > 0) {
                $.each(jsonData, function (index, value) {
                    $("#UsualErrorCode").append("<option value='" + value.CatalogCode + "'>" + value.CatalogText_vi + "</option>");
                });
            }
        }
    });
});

$('#divProductSearch').on('shown.bs.modal', function () {
    $("#divProductPopup").modal("hide");
    $('input[name="ProfileId"]').val($("#ProfileId").val());
    $(".modal-backdrop.in").remove();
});

$('#divProductSearch').on('hidden.bs.modal', function () {
    if ($(this).attr("id") == 'divProductSearch'){
        $("#divProductPopup").modal("show");
    }
});

//Save product
$(document).on("click", ".btn-save-product", function () {
    var $btn = $(this);
    $btn.button("loading");

    var TaskId = $("input[name='TaskId']").val();
    //var TaskProductId = $("form input[name='TaskProductId']").val();
    //var ProductId = $("form input[name='ProductId']").val();
    //var Qty = $("form input[name='Qty']").val();
    var frm = $("#frmTaskProduct"),
        frmData = new FormData(),
        frmParams = frm.serializeArray();

    $.each(frmParams, function (i, val) {
        frmData.append(val.name, val.value);
    });

    frmData.append("TaskId", TaskId);
    $.ajax({
        type: "POST",
        url: "/Work/Task/SaveTaskProduct",
        //data: {
        //    TaskId: TaskId,
        //    TaskProductId: TaskProductId,
        //    ProductId: ProductId,
        //    Qty: Qty
        //},
        data: frmData,
        processData: false,
        contentType: false,
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success === false) {
                alertModalPopup(jsonData.Data);
            }
            else {
                $("#divAccessorySearch").modal("hide");
                $("#divProductPopup").modal("hide");

                $("#contentTaskProduct table tbody").html("");
                $("#contentTaskProduct table tbody").html(jsonData);
            }
        }
    });
});

//Delete product
$(document).on("click", ".btn-delete-product", function () {
    var $btn = $(this);
    $btn.button("loading");
    var TaskProductId = $(this).data("id");
    $("#divDeleteProductPopup input[name='TaskProductId']").val(TaskProductId);
    $("#divDeleteProductPopup").modal("show");
    $btn.button("reset");
});

$(document).on("click", "#btn-confirm-del-product", function () {
    var $btn = $(this);
    $btn.button("loading");
    var TaskId = $("input[name='TaskId']").val();
    var TaskProductId = $("#divDeleteProductPopup input[name='TaskProductId']").val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/DeleteTaskProduct",
        data: {
            TaskId: TaskId,
            TaskProductId: TaskProductId
        },
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success == false) {

            }
            else {
                $("#divDeleteProductPopup").modal("hide");
                $("#contentTaskProduct table tbody").html("");
                $("#contentTaskProduct table tbody").html(jsonData);
            }
        }
    });
});

//Comment
$(document).on("click", ".btn-edit-cmt", function () {
    $(".btn-save-hidden").hide();

    var id = $(this).data("id");
    $(".renderComment-" + id).addClass("hidden");
    $(".editComment-" + id).removeClass("hidden");
});

$(document).on("click", ".btn-cancel-editComment", function () {
    $(".btn-save-hidden").hide();

    var id = $(this).data("id");
    $(".renderComment-" + id).removeClass("hidden");
    $(".editComment-" + id).addClass("hidden");
});

$(document).on("click", ".btn-cancel-task", function () {
    $(".btn-save-hidden").hide();
});

$(document).on("click", ".btn-save-task", function () {
    ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmEdit", this, null, null, "ReloadComment");
});

//edit comment
$(document).on("click", ".btn-save-task-comment", function () {
    var id = $(this).data("id");
    var EditComment = $("textarea[data-id='" + id + "']").val();
    var Type = $("input[name='Type']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/EditComment',
        data: {
            TaskCommentId: id,
            EditComment: EditComment,
            Type: Type
        },
        success: function (html) {
            if (html.Success == true) {
                $(".btn-save-hidden").hide();
                $(".renderComment-" + id).removeClass("hidden");
                $(".editComment-" + id).addClass("hidden");
                $("#RenderComment_" + id).html("");
                $("#RenderComment_" + id).html(EditComment);
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        }
    });
});

//delete comment
$(document).on("click", ".btn-del-cmt", function () {
    var id = $(this).data("id");
    $("#divDeleteCommentPopup input[name='TaskCommentId']").val(id);
    $("#divDeleteCommentPopup").modal("show");
});
$(document).on("click", "#btn-confirm-del-cmt", function () {
    var TaskCommentId = $("#divDeleteCommentPopup input[name='TaskCommentId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/DeleteComment',
        data: {
            TaskCommentId: TaskCommentId
        },
        success: function (html) {
            $("#divDeleteCommentPopup").modal("hide");
            if (html.Success == true) {
                ReloadComment();
            }
        }
    });
});

//File Attachment
$(document).on("change", ".fileTab input[name='MainCommentFileUrl']", function () {
    $(".btn-save-hidden").hide();
    $(".div-file").show();
});

//Add new files
$(document).on("click", ".btn-save-file", function () {
    ISD.SaveDataWithUrl("/Work/Task/SaveFileAttachments", "#frmEdit", this, "ReloadComment");
});

//Show modal popup add files
$(document).on("click", ".btn-add-file", function () {
    loading2();

    var TaskId = $("input[name='TaskId']").val();
    $("#divAddFilePopup input[name='TaskId']").val(TaskId);

    $("#divAddFilePopup").modal("show");
    $("body").removeClass("loading2");
});

//Save new files on modal popup
$(document).on("click", "#btn-confirm-add-file", function () {
    $("#divAddFilePopup").modal("hide");
    ISD.SaveDataWithUrl("/Work/Task/SaveFileAttachments", "#frmAddFile", this, "ReloadFileTable");
});

function ReloadFileTable() {
    ReloadComment(true);
}

$(document).on("click", "#img-tab", function () {
    ReloadFileTable();
});

//Delete file
$(document).on("click", ".btn-del-file", function () {
    var id = $(this).data("id");
    var isOnTab = $(this).data("tab");

    $("#divDeleteFilePopup input[name='FileAttachmentId']").val(id);
    $("#divDeleteFilePopup input[name='isOnTab']").val(isOnTab);
    $("#divDeleteFilePopup").modal("show");
});
$(document).on("click", "#btn-confirm-del-file", function () {
    var FileAttachmentId = $("#divDeleteFilePopup input[name='FileAttachmentId']").val();
    var isOnTab = $("#divDeleteFilePopup input[name='isOnTab']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/DeleteFileAttachment',
        data: {
            FileAttachmentId: FileAttachmentId
        },
        success: function (html) {
            $("#divDeleteFilePopup").modal("hide");
            if (html.Success == true) {
                ReloadComment(isOnTab);
            }
        }
    });
});

function ReloadComment(isOnTab) {
    loading2();
    var TaskId = $("input[name='TaskId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/GetTaskCommentList',
        data: {
            TaskId: TaskId,
            isOnTab: isOnTab
        },
        success: function (html) {
            if (html.Success == false) {

            }
            else {
                if (isOnTab == "true" || isOnTab == "True" || isOnTab == true) {
                    $(".div-list-file").html("");
                    $(".div-list-file").html(html);
                }
                else {
                    $(".div-list-comment").html("");
                    $(".div-list-comment").html(html);
                }
            }
        }
    });
}

function ReloadEdit() {
    loading2();
    location.reload();
}

$(document).ajaxStop(function () {
    $(".with-search").select2();
    ChangeDropdownColor();
    $("#frmEdit").removeClass("loading2");
});

function ChangeDropdownColor() {
    var bgColor = $('input[name="TaskStatusBackgroundColor"]').val();
    var color = $('input[name="TaskStatusColor"]').val();

    $("#frmEdit .select2-selection").has("#select2-TaskStatusIdList-container").css("background-color", bgColor);
    $("#frmEdit .select2-selection #select2-TaskStatusIdList-container").css("color", color);
}

//$(document).on("change", "select[name='TaskStatusIdList']", function () {
//    var value = $(this).val();
//    $("input[name='TaskStatusId']").val(value);
//});

//Save all task
$(document).on("click", "#btn-save-task-all", function () {
    $('<input>').attr({
        type: 'hidden',
        id: 'isSaveAll',
        name: 'isSaveAll',
        value: true
    }).appendTo('form#frmEdit');
    ISD.SaveDataWithUrl("/Work/Task/SaveComment", "#frmEdit", this);
});

$(document).on("click", "#btn-save-task-all-continue", function () {
    $('<input>').attr({
        type: 'hidden',
        id: 'isSaveAll',
        name: 'isSaveAll',
        value: true
    }).appendTo('form#frmEdit');
    ISD.SaveDataWithUrl("/Work/Task/SaveComment", "#frmEdit", this, "ReloadEdit");
});

//$(document).on("change", "select[name='ShowroomCode']", function () {
//    var CustomerSourceCode = $(this).val();
//    $.ajax({
//        type: "POST",
//        url: "/Customer/Profile/GetSaleOrgByCustomerSource",
//        data: {
//            CustomerSourceCode: CustomerSourceCode
//        },
//        success: function (jsonData) {
//            $("#StoreId").html("");
//            $("#StoreId").append("<option value=''>-- Vui lòng chọn --</option>");
//            $.each(jsonData, function (index, value) {
//                $("#StoreId").append("<option value='" + value.StoreId + "'>" + value.StoreName + "</option>");
//            });
//        }
//    });
//});

//#region Sản phẩm/Phụ kiện
$(document).on('click', '.btn-add-accessory', function (e) {
    var indexRow = 0;
    e.preventDefault();

    $(".accessory").each(function (index, value) {
        indexRow = index;
    });
    indexRow++;

    var controlForm = $('.accessoryControls:first'),
        currentEntry = $(this).parents(' .accessory:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.accessoryId').attr('name', 'accessoryList[' + indexRow + '].ProductId').val('');
    newEntry.find('.accessoryName').attr('name', 'accessoryList[' + indexRow + '].ProductName').val('');
    newEntry.find('.accessoryQty').attr('name', 'accessoryList[' + indexRow + '].Qty').val('');
    newEntry.find('.accErrorTypeCode').attr('name', 'accessoryList[' + indexRow + '].AccErrorTypeCode').val('');
    newEntry.find('.productAccessoryTypeCode').attr('name', 'accessoryList[' + indexRow + '].ProductAccessoryTypeCode').val('');
    newEntry.find('.open-accessory-modal').attr('data-row', indexRow);
    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.accessory:not(:first) .btn-add-accessory')
        .removeClass('btn-add-accessory').addClass('btn-remove-accessory')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-remove-accessory', function (e) {
    $(this).parents('.accessory:last').remove();

    $(".accessory").each(function (index, value) {
        $(this).find('.accessoryId').attr("name", "accessoryList[" + index + "].ProductId");
        $(this).find('.accessoryName').attr("name", "accessoryList[" + index + "].ProductName");
        $(this).find('.accessoryQty').attr("name", "accessoryList[" + index + "].Qty");
        $(this).find('.accErrorTypeCode').attr("name", "accessoryList[" + index + "].AccErrorTypeCode");
        $(this).find('.productAccessoryTypeCode').attr("name", "accessoryList[" + index + "].ProductAccessoryTypeCode");
    });
    e.preventDefault();
    return false;
});

$(document).on('click', '.open-accessory-modal', function (e) {
    var row = $(this).data('row');
    $('#CurrentIndex').val(row);
    $('input[name="ProfileId"]').val($("#ProfileId").val());
    $("#divProductPopup").modal("hide");
});

$('#divAccessorySearch').on('shown.bs.modal', function () {
    $("#divProductPopup").modal("hide");
});

$('#divAccessorySearch').on('hidden.bs.modal', function () {
    $("#divProductPopup").modal("show");
});

$(document).on("click", ".btn-accessory-choose", function () {
    $('#divAccessorySearch').modal("hide");
    $("#divProductPopup").modal("show");


    var row = $("#CurrentIndex").val();
    var id = $(this).data("id");
    var code = $(this).data("code");
    var name = $(this).data("name");

    $('.accessory input[name="accessoryList[' + row + '].ProductId"').val(id);
    $('.accessory input[name="accessoryList[' + row + '].ProductName"').val(name);

    $('.accessory input[name="accessoryList[' + row + '].Qty"').focus();
});

//#endregion 

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

//$(document).on("click", ".btn-copy-contactaddress", function () {
//    var ContactAddress = $("input[name='ContactAddress']").val();
//    if (ContactAddress != "") {
//        $("input[name='VisitAddress']").val(ContactAddress);
//        GetLocationFromAddress(ContactAddress);
//    }
//});

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

$(document).on("input", "input[name='WarrantyValue']", function () {
    var value = $(this).val();
    $(".warranty-value").html("");
    var formatCurrencyProperty3 = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".warranty-value").removeClass("hidden");
        $(".warranty-value").html(formatCurrencyProperty3);
    }
    else {
        $(".warranty-value").addClass("hidden");
    }
});

$(document).on("input", "input[name='ProductValue']", function () {
    var value = $(this).val();
    $(".product-value").html("");
    var formatCurrencyProperty3 = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".product-value").removeClass("hidden");
        $(".product-value").html(formatCurrencyProperty3);
    }
    else {
        $(".product-value").addClass("hidden");
    }
});

$(document).on("input", "input[name='DiscountValue']", function () {
    var value = $(this).val();
    $(".discount-value").html("");
    var formatCurrencyProperty3 = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".discount-value").removeClass("hidden");
        $(".discount-value").html(formatCurrencyProperty3);
    }
    else {
        $(".discount-value").addClass("hidden");
    }
});

//Get Warranty value
$(document).on("click", ".btn-get-warranty-value", function () {
    var $btn = $(this);
    $btn.button("loading");
    var SO = $("#SAPSOWarranty").val();
    $.ajax({
        type: "GET",
        url: "/Work/Task/GetValueWithSO",
        data: {
            SO: SO,
        },
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success === true) {
                console.log(jsonData.Data)
                $("#WarrantyValue").val(jsonData.Data).trigger("input");
            }
            else {
                $("#divUpdateType .alert-message").html(jsonData.Data);
            }
        }
    });
});

//Get Product value
$(document).on("click", ".btn-get-product-value", function () {
    var $btn = $(this);
    $btn.button("loading");
    var SO = $("#SAPSOProduct").val();
    $.ajax({
        type: "GET",
        url: "/Work/Task/GetValueWithSO",
        data: {
            SO: SO,
        },
        success: function (jsonData) {
            $btn.button("reset");
            if (jsonData.Success === true) {
                console.log(jsonData.Data)
                $("#ProductValue").val(jsonData.Data).trigger("input");
            }
            else {
                $("#divUpdateType .alert-message").html(jsonData.Data);
            }
        }
    });
});


//Xem chi tiết catalog
$(document).on("click", ".btn-showStockDelivery", function () {
    var DeliveryId = $(this).data("id");
    $("#contentCatalogue .dropdown-menu").addClass("hidden");

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockDelivery/GetProductDetails",
        data: {
            DeliveryId: DeliveryId
        },
        success: function (jsonData) {
            $("#contentCatalogue .dropdown-menu").html("");
            $.each(jsonData, function (index, value) {
                $("#contentCatalogue .dropdown-menu").append("<li>" + value.ProductName + "</li>");
            });
            $("#contentCatalogue .dropdown-menu").removeClass("hidden");
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

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
            $("select[name='TaskStatusIdList']").html("");
            if (jsonData != null && jsonData.length > 0) {
                $.each(jsonData, function (index, value) {
                    $("select[name='TaskStatusIdList']").append("<option value='" + value.TaskStatusId + "'>" + value.TaskStatusName + "</option>");
                });
            }
            //Update hidden TaskStatusId
            $("select[name='TaskStatusIdList']").trigger("change");
        }
    });
});

//$(document).on("change", "input[name='IsAssignGroup']", function () {
//    var IsAssingee = $("input[name='IsAssignGroup']:checked").val();
//    if (IsAssingee == undefined) {
//        IsAssingee = null;
//    }

//    if (IsAssingee == "True") {
//        $(".assign_Group").removeClass("hidden");
//        $(".assign_Personal").addClass("hidden");
//    }
//    else {
//        $(".assign_Group").addClass("hidden");
//        $(".assign_Personal").removeClass("hidden");
//    }
//});

$(document).on("change", "select[name='WorkFlowId']", function () {
    loading2();

    $.ajax({
        type: "POST",
        url: '/Work/Task/LoadFormByWorkFlow_Edit',
        data: $("#frmEdit").serialize(),
        success: function (data) {
            $(".frmUpdateTask").html(data);
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
});

$(document).on('keydown', 'textarea', function (e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        this.value = this.value.substring(0, this.selectionStart) + "" + "\n" + this.value.substring(this.selectionEnd, this.value.length);
    }
});

//Có ghé thăm cabinet pro
$(document).on("change", 'input[type=radio][name="isVisitCabinetPro"]', function () {
    if ($(this).is(':checked')) {
        var isVisitCabinetPro = $(this).val();
        if (isVisitCabinetPro.toLowerCase() === "true") {
            $(".visit-cabinet").show();
        }
        else {
            $(".visit-cabinet").hide();
        }
    }
});

/*
    Bảo hành - Ý kiến khách hàng
    1. Không ý kiến => Không làm gì hết
    2. Đánh giá theo sao => Hiển thị 2 phần
        2.1. Về sản phẩm: Đánh giá từ 1 đến 5 sao
        2.2. Về dịch vụ: Đánh giá từ 1 đến 5 sao
    3. Khác: Nhập nội dung (textarea)
*/
$(document).on('change', 'select[name="CustomerRatings"]', function () {
    var customerRatings = $(this).val();
    if (customerRatings) {
        if (customerRatings === "none") {
            $(".rating-star").hide();
            $(".rating-other").hide();
        }
        else if (customerRatings === "rating") {
            $(".rating-star").show();
            $(".rating-other").show();
        }
        //else if (customerRatings === "other") {
        //    $(".rating-star").hide();
        //    $(".rating-other").show();
        //}
    }
});

/* 1. Visualizing things on Hover - See next part for action on click */
$(document).on('mouseover', '#stars-product li', function () {
    var onStar = parseInt($(this).data('text'), 10); // The star currently mouse on

    // Now highlight all the stars that's not after the current hovered star
    $(this).parent().children('li.star').each(function (e) {
        if (e < onStar) {
            $(this).addClass('hover');
        }
        else {
            $(this).removeClass('hover');
        }
    });

}).on('mouseout', '#stars-product li', function () {
    $(this).parent().children('li.star').each(function (e) {
        $(this).removeClass('hover');
    });
});

$(document).on('mouseover', '#stars-service li', function () {
    var onStar = parseInt($(this).data('text'), 10); // The star currently mouse on

    // Now highlight all the stars that's not after the current hovered star
    $(this).parent().children('li.star').each(function (e) {
        if (e < onStar) {
            $(this).addClass('hover');
        }
        else {
            $(this).removeClass('hover');
        }
    });

}).on('mouseout', '#stars-service li', function () {
    $(this).parent().children('li.star').each(function (e) {
        $(this).removeClass('hover');
    });
});


/* 2. Action to perform on click */
$(document).on('click', '#stars-product li', function () {
    var onStar = parseInt($(this).data('text'), 10); // The star currently selected
    var stars = $(this).parent().children('li.star');

    for (i = 0; i < stars.length; i++) {
        $(stars[i]).removeClass('selected');
    }

    for (i = 0; i < onStar; i++) {
        $(stars[i]).addClass('selected');
    }

    // JUST RESPONSE (Not needed)
    var ratingValue = parseInt($('#stars-product li.selected').last().data('text'), 10);
    //alert(ratingValue);
    var rating = $('#stars-product li.selected').last().data('value');
    $("#Ticket_CustomerReviews_Product").val(rating);
});

$(document).on('click', '#stars-service li', function () {
    var onStar = parseInt($(this).data('text'), 10); // The star currently selected
    var stars = $(this).parent().children('li.star');

    for (i = 0; i < stars.length; i++) {
        $(stars[i]).removeClass('selected');
    }

    for (i = 0; i < onStar; i++) {
        $(stars[i]).addClass('selected');
    }

    // JUST RESPONSE (Not needed)
    var ratingValue = parseInt($('#stars-service li.selected').last().data('text'), 10);
    //alert(ratingValue);
    var rating = $('#stars-service li.selected').last().data('value');
    $("#Ticket_CustomerReviews_Service").val(rating);
});

function loadRatingProduct() {
    var ratingProduct = $("#Ticket_CustomerReviews_Product").val();
    if (ratingProduct) {
        var $this = $("#stars-product li[data-value='" + ratingProduct + "']");
        var onStar = parseInt($($this).data('text'), 10);

        var stars = $($this).parent().children('li.star');

        for (i = 0; i < stars.length; i++) {
            $(stars[i]).removeClass('selected');
        }

        for (i = 0; i < onStar; i++) {
            $(stars[i]).addClass('selected');
        }
    }
}

function loadRatingService() {
    var ratingProduct = $("#Ticket_CustomerReviews_Service").val();
    if (ratingProduct) {
        var $this = $("#stars-service li[data-value='" + ratingProduct + "']");
        var onStar = parseInt($($this).data('text'), 10);

        var stars = $($this).parent().children('li.star');

        for (i = 0; i < stars.length; i++) {
            $(stars[i]).removeClass('selected');
        }

        for (i = 0; i < onStar; i++) {
            $(stars[i]).addClass('selected');
        }
    }
}


// Thị hiếu sản phẩm
$(".open-customertaste-model").on("click", function () {
    $("#divProductSearch").removeAttr("id").attr("id","customertaste-model")
    $("#customertaste-model").modal("show");
})

$(document).on('click', '#customertaste-model .btn-product-choose', function (e) {
    $('#customertaste-model').modal("hide");
    var id = $(this).data("id");
    var name = $(this).data("name");
       
    var indexRowPerson = 0;
    e.preventDefault();

    $(".customerTaste").each(function (index, value) {
        indexRowPerson++;
    });

    $(".customerTastesControls").append('<div class="customerTaste">' 
                                        +'<div class="col-md-8 no-padding width-fixed position-relative dropdown-absolute">'
        + '<input type="text" name="customerTasteList[' + indexRowPerson + '].ProductName" value="' + name +'" readonly class="form-control productName with-search" data-row='+ indexRowPerson +'>'
        + '<input name="customerTasteList[' + indexRowPerson + '].ProductId" value="' + id +'" type="hidden"  class="form-control productId with-search" data-row='+ indexRowPerson +'>'
                                        +'</div>'
                                        +'<div class="col-md-4  offset-md-1">'
                                        +'<span class="input-group-btn">'
                                        +'<button class="btn btn-default btn-removeCustomerTaste btn-danger" type="button">'
                                        +'<span class="glyphicon glyphicon-minus"></span>'
                                        +'</button>'
                                        +'</span>'
                                        +'</div>'
                                        + '</div>');
});

$(document).on('click', '.btn-removeCustomerTaste', function (e) {
    $(this).parents('.customerTaste:last').remove();

    $(".customerTaste").each(function (index, value) {
        $(this).find('.productName').attr("name", "customerTasteList[" + index + "].ProductName").attr("data-row",index);
        $(this).find('.productId').attr("name", "customerTasteList[" + index + "].ProductId").attr("data-row", index);
    });
    e.preventDefault();
    return false;
});