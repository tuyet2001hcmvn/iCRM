
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

    newEntry.find('.taskAssignId').attr('name', 'taskAssignList[' + indexRowPerson + '].TaskAssignId').val('');
    newEntry.find('.employeeCode').attr('name', 'taskAssignList[' + indexRowPerson + '].SalesEmployeeCode').val('');
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
    ChangeDropdownColor();
});

$(document).on('click', '.btn-removePerson', function (e) {
    $(this).parents('.person:last').remove();

    $(".person").each(function (index, value) {
        $(this).find('.employeeCode').attr("name", "taskAssignList[" + index + "].SalesEmployeeCode");
        $(this).find('.typeCode').attr("name", "taskAssignList[" + index + "].TaskAssignTypeCode");
    });
    e.preventDefault();

    //ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
    //ChangeDropdownColor();
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

//not use
function GetDataTask() {
    loading2();
    var data = $("#frmSearch").serializeArray();

    $.ajax({
        type: "POST",
        url: "/Work/Task/_PaggingServerSide",
        data: data,
        success: function (jsonData) {
            //console.log(jsonData);
            GetKanban(jsonData);
        }
    });
}

function GetKanban(jsonData) {
    var fields = [
        { name: "id", type: "string" },
        { name: "status", map: "state", type: "string" },
        { name: "text", map: "label", type: "string" },
        { name: "tags", type: "string" },
        { name: "color", map: "hex", type: "string" },
        { name: "resourceId", type: "string" },
        { name: "className", type: "string" }
    ];

    if (jsonData.kanbanDetailList.length == 0) {
        var item = { id: "", state: "1", label: "", tags: "", hex: null, className: "hidden" };
        jsonData.kanbanDetailList.push(item);
    }

    var source =
    {
        localData: jsonData.kanbanDetailList,
        dataType: "array",
        dataFields: fields
    }
    var dataAdapter = new $.jqx.dataAdapter(source);

    $('#kanban').jqxKanban({
        source: dataAdapter,
        width: '100%',
        theme: "material-green",
        height: '100%',
        columns: jsonData.columns,
        columnRenderer: function (element, collapsedElement, column) {
            var number = $("#kanban").jqxKanban('getColumnItems', column.dataField).length;
            element.find(".jqx-kanban-column-header-status").html("(" + number + ")");
        }
    });
}

//Show modal popup edit task
$(document).on("click", "div.jqx-kanban-item-keyword:first-child", function () {
    loading2();
    var id = $(this).parent().parent().attr("id");
    var TaskId = id.substr(id.indexOf("_") + 1, id.length);
    var KanbanId = $("input[name='KanbanId']").val();

    SaveTaskDataPopup(TaskId, KanbanId);
});

$(document).on("click", "div.jqx-kanban-item", function (e) {
    var type = e.toElement.nodeName.toLowerCase();
    if (type == "i" || type == "a" || type == "img") {

    }
    else {
        loading2();
        var id = $(this).attr("id");
        var TaskId = id.substr(id.indexOf("_") + 1, id.length);
        var KanbanId = $("input[name='KanbanId']").val();

        SaveTaskDataPopup(TaskId, KanbanId);
    }
});

$(document).on("click", ".btn-showTaskPopup", function () {
    loading2();
    var TaskId = $(this).data("id");
    var KanbanId = $("input[name='KanbanId']").val();

    SaveTaskDataPopup(TaskId, KanbanId);
});

function SaveTaskDataPopup(TaskId, KanbanId) {
    $.ajax({
        type: "POST",
        url: '/Work/Task/_Edit',
        data: {
            TaskId: TaskId,
            KanbanId: KanbanId
        },
        success: function (html) {
            if (html.Success == false) {
                alertPopup(false, html.Data);
            }
            else {
                $(".with-search").select2();
                $("#popupTaskKanban").find(".modal-content").html(html).end().modal("show");
                ISD.ValidationOnModalPopup("#frmUpdateTask");
                GetAddressByContact("construction");
            }
        }
    });
}

function GetAddressByContact(type) {
    var ContactId = $("#popupTaskKanban input[name='ContactId']").val();
    var ProfileId = $("#popupTaskKanban input[name='ProfileId']").val();
    var ParentTaskId = $("#popupTaskKanban input[name='ParentTaskId']").val();
    if (ContactId == "" || ContactId == null) {
        ContactId = ProfileId;
    }
    if (type == "construction") {
        ContactId = $("#popupTaskKanban input[name='ConstructionUnit']").val();
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
                $("#popupTaskKanban input[name='Phone']").val(jsonData.ContactPhone);
                $("#popupTaskKanban input[name='Email']").val(jsonData.ContactEmail);
                $("#popupTaskKanban input[name='ContactShortName']").val(jsonData.ContactShortName);
            }
            else if (type == "profile") {
                $("#popupTaskKanban input[name='Phone']").val(jsonData.ContactPhone);
                $("#popupTaskKanban input[name='Email']").val(jsonData.ContactEmail);
                $("#popupTaskKanban input[name='ContactShortName']").val(jsonData.ContactShortName);

                if (jsonData.ProfileId != ProfileId) {
                    $("#popupTaskKanban input[name='ContactId']").val("");
                    $("#popupTaskKanban input[name='ContactName']").val("");
                    $("#popupTaskKanban input[name='ContactAddress']").val("");
                }
                $("#popupTaskKanban #ProfileAddress").html("");
                $("#popupTaskKanban #ProfileAddress").append("<option value=''>-- Vui lòng chọn --</option>");
                if (jsonData.AddressList != null && jsonData.AddressList.length > 0) {
                    $.each(jsonData.AddressList, function (index, value) {
                        if (jsonData.ExistProfileAddress != null && jsonData.ExistProfileAddress != "" && jsonData.ExistProfileAddress == value) {
                            $("#popupTaskKanban #ProfileAddress").append("<option value='" + value + "' selected>" + value + "</option>");
                        }
                        else {
                            $("#popupTaskKanban #ProfileAddress").append("<option value='" + value + "'>" + value + "</option>");
                        }
                    });
                }
                $("#popupTaskKanban input[name='SalesSupervisorName']").val(jsonData.SalesSupervisorName);
                $("#popupTaskKanban input[name='SalesSupervisorCode']").val(jsonData.SalesSupervisorCode).trigger("change");

                $("#popupTaskKanban .DepartmentName").html("");
                $("#popupTaskKanban .DepartmentName").html(jsonData.DepartmentName);
            }
            else if (type == "construction") {
                $("#popupTaskKanban input[name='Construction_Phone']").val(jsonData.MainContactPhone);
                $("#popupTaskKanban input[name='Construction_Email']").val(jsonData.MainContactEmail);
                $("#popupTaskKanban input[name='Construction_ContactName']").val(jsonData.MainContactName);

                $("#popupTaskKanban input[name='Construction_SalesSupervisorName']").val(jsonData.SalesSupervisorName);
                $("#popupTaskKanban .Construction_DepartmentName").html("");
                $("#popupTaskKanban .Construction_DepartmentName").html(jsonData.DepartmentName);
            }
        }
    });
}

$('#kanban').on('itemMoved', function (event) {
    loading2();
    var args = event.args;
    var TaskId = args.itemData.id;
    var NextColumn = args.newColumn.dataField;
    var NextColumnName = args.newColumn.text;
    var KanbanId = $("input[name='KanbanId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/_TaskPopup',
        data: {
            TaskId: TaskId,
            KanbanId: KanbanId,
            NextColumnName: NextColumnName.replace("<i class='fa fa-tasks'></i>", "")
        },
        success: function (html) {
            $(".with-search").select2();
            $("#popupSaveTaskStatus").find(".modal-content").html(html).end().modal("show");
            ISD.ValidationOnModalPopup("#frmSaveTaskStatus");
        }
    });
});

$(document).on("click", "#popupSaveTaskStatus #btn-save-status", function () {
    ISD.SaveDataWithPopup("/Work/Task/SaveTaskStatus", "#frmSaveTaskStatus", this, "#popupSaveTaskStatus", false, PaggingServerSide_Task("Work/Task"));
});

$('#popupSaveTaskStatus').on('hidden.bs.modal', function () {
    //GetDataTask();
    loading2();
    PaggingServerSide_Task("Work/Task");
});

/*Save Data*/
$(document).on("click", "#popupTaskKanban .btn-save-task", function () {
    ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true);
});

$('#popupTaskKanban').on('hidden.bs.modal', function () {
    var calendar_MyWork = $("#calendar_MyWork").length;
    loading2();

    if (calendar_MyWork > 0) {
        var Type = $("input[name='Type']").val();
        var KanbanId = $("input[name='KanbanId']").val();
        calendarFunction_Calendar(Type, KanbanId);
    }
    else {
        PaggingServerSide_Task("Work/Task");
    }
});

//Blur element in popup, hide button Cancel and button Save
//$(document).on("blur", "#frmUpdateTask input:not([type='file']), #frmUpdateTask select", function (e) {
//    if ($(e.relatedTarget).hasClass("btn-save-task")) {
//        return false;
//    }
//    else {
//        var visible = $("#frmUpdateTask .btn-save-hidden").is(":visible");
//        if (visible == true) {
//            $(".btn-save-hidden").hide();
//        }
//    }
//});

$('#popupTaskKanban').on('shown.bs.modal', function () {
    ChangeDropdownColor();
});

function ChangeDropdownColor() {
    var bgColor = $('input[name="TaskStatusBackgroundColor"]').val();
    var color = $('input[name="TaskStatusColor"]').val();

    $("#frmUpdateTask .select2-selection").has("#select2-ToStatus-container").css("background-color", bgColor);
    $("#frmUpdateTask .select2-selection #select2-ToStatus-container").css("color", color);
}

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

//Summary
//$(document).on("focus", "textarea[name='Summary']", function () {
//    $(".btn-save-hidden").hide();
//    $(".div-summary").show();
//});

////Requirement
//$(document).on("focus", "textarea[name='Requirement']", function () {
//    $(".btn-save-hidden").hide();
//    $(".div-requirement").show();
//});

////Description
//$(document).on("focus", "textarea[name='Description']", function () {
//    $(".btn-save-hidden").hide();
//    $(".div-description").show();
//});

////DateTime
//$(document).on("focus", "input[data-id='ReceiveDate'], input[data-id='StartDate'], input[data-id='EstimateEndDate'], input[data-id='EndDate']", function () {
//    $(".btn-save-hidden").hide();
//    $(".div-datetime").show();
//});

//File Attachment
$(document).on("change", ".fileTab input[name='MainCommentFileUrl']", function () {
    $(".btn-save-hidden").hide();
    $(".div-file").show();
});

////CommonMistakeCode
//$(document).on("change", "select[name='CommonMistakeCode']", function () {
//    ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
//});

//ToStatus
$(document).on("change", "select[name='ToStatus']", function () {
    var value = $(this).val();
    $("#frmUpdateTask input[name='TaskStatusId']").val(value);

    //ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
});

////PriorityCode
//$(document).on("change", "select[name='PriorityCode']", function () {
//    ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
//});

////Reporter
//$(document).on("change", "select[name='Reporter']", function () {
//    if ($("#frmUpdateTask").length > 0) {
//        ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
//    }
//});

////Assignee
//$(document).on("change", ".employeeCode, .typeCode", function () {
//    var row = $(this).attr("row");
//    var TaskAssignId = $("input[name='taskAssignList[" + row + "].TaskAssignId']").val();
//    var SalesEmployeeCode = $("select[name='taskAssignList[" + row + "].SalesEmployeeCode']").val();
//    if (TaskAssignId != "" && SalesEmployeeCode != "") {
//        ISD.SaveDataWithPopup("/Work/Task/SaveComment", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
//    }
//});

//Remind
$(document).on("change", "input[name='isRemind']", function () {
    //$(".btn-save-hidden").hide();
    //$(".div-remind").show();

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

//$(document).on("click", ".remindChange", function () {
//    $(".btn-save-hidden").hide();
//    $(".div-remind").show();
//});

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
    //$(".btn-save-hidden").hide();
    CancelUpdateTask();
});

//edit comment
$(document).on("click", ".btn-save-task-comment", function () {
    var id = $(this).data("id");
    var EditComment = $("textarea[data-id='" + id + "']").val();
    var Type = $("input[name='Type']").val();
    var popup = "#popupTaskKanban";

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
                $(popup + " .divPopupMessage .alert-message").html("");
                ISD.setMessage(popup + " .divPopupMessage", jsonData.Data);
                $(popup + " .divPopupMessage").show();
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
                CancelUpdateTask();
            }
        }
    });
});

//Add new files
$(document).on("click", ".btn-save-file", function () {
    ISD.SaveDataWithPopup("/Work/Task/SaveFileAttachments", "#frmUpdateTask", this, "#popupTaskKanban", true, "CancelUpdateTask");
});

//Delete file
$(document).on("click", ".btn-del-file", function () {
    var id = $(this).data("id");
    $("#divDeleteFilePopup input[name='FileAttachmentId']").val(id);
    $("#divDeleteFilePopup").modal("show");
});
$(document).on("click", "#btn-confirm-del-file", function () {
    var FileAttachmentId = $("#divDeleteFilePopup input[name='FileAttachmentId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/DeleteFileAttachment',
        data: {
            FileAttachmentId: FileAttachmentId
        },
        success: function (html) {
            $("#divDeleteFilePopup").modal("hide");
            if (html.Success == true) {
                CancelUpdateTask();
            }
        }
    });
});

function CancelUpdateTask() {
    $(".btn-save-hidden").hide();

    loading2Modal();
    var TaskId = $("input[name='TaskId']").val();
    var NextColumnName = $("input[name='NextColumnName']").val();
    var KanbanId = $("input[name='KanbanId']").val();

    $.ajax({
        type: "POST",
        url: '/Work/Task/_Edit',
        data: {
            TaskId: TaskId,
            KanbanId: KanbanId,
            NextColumnName: NextColumnName
        },
        success: function (html) {
            if (html.Success == false) {

            }
            else {
                $("#popupTaskKanban").find(".modal-content").html("");
                $("#popupTaskKanban").find(".modal-content").html(html);
                GetAddressByContact("construction");
            }
        }
    });
}

$(document).ajaxStop(function () {
    $(".with-search").select2();
    ChangeDropdownColor();
    $("#popupTaskKanban").removeClass("loading2");
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
            $("body").removeClass("loading2");
            $(".divProfilePopup").html("");
            $(".divContactPopup").html("");
            $(".divConstructionPopup").html("");

            $(".divProfilePopup").html(html);
            $(".with-search").select2();
            $(".divProfilePopup #divProfileSearch").modal("show");
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
            $("body").removeClass("loading2");
            $(".divContactPopup").html("");
            $(".divProfilePopup").html("");
            $(".divConstructionPopup").html("");

            $(".divContactPopup").html(html);
            $(".with-search").select2();
            $(".divContactPopup #divProfileSearch").modal("show");
        }
    });
});

$(document).on("click", ".btn-get-construction", function () {
    loading2();
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Account'
        },
        success: function (html) {
            $("body").removeClass("loading2");
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

$(document).on("click", ".divProfilePopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var name = $(this).data("name");

    $("input[name='CustomerTypeCode']").val("Account");
    $("input[name='ProfileId']").val(id).trigger('change');
    $("input[name='ProfileName']").val(name);
});

$(document).on("click", ".divContactPopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var name = $(this).data("name");

    $("input[name='CustomerTypeCode']").val("Contact");
    var profileId = $("input[name='ProfileId']").val();
    var profileName = $("input[name='ProfileName']").val();
    $("input[name='ProfileId']").val(profileId);
    $("input[name='ProfileName']").val(profileName);

    $("input[name='CompanyId']").val(id).trigger('change');
    $("input[name='CompanyName']").val(name);
});

$(document).on("click", ".divConstructionPopup .btn-profile-choose", function () {
    var id = $(this).data("id");
    var name = $(this).data("name");
    $("input[name='ConstructionUnit']").val(id).trigger('change');
    $("input[name='ConstructionUnitName']").val(name);
    GetAddressByContact("construction");
});

function loading2Modal() {
    $("#popupTaskKanban").addClass("loading2");
}

function calendarFunction(jsonData) {
    var calendar = $('#calendar').length;
    var id = '#calendar';
    if (calendar == 0) {
        id = '#calendarGTB';
    }

    $(id).fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        //Sunday = 0, Monday = 1, Tuesday = 2
        firstDay: 1,
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },
        //dayRender: function (info) {
        //    info.el.innerHTML += "<button class='dayButton' data-date='" + info.date + "'>Click me</button>";
        //    info.el.style.padding = "20px 0 0 10px";
        //},
        eventRender: function (event, element, view) {
            //return $('<div>' + event.title + '</div>');
            let content = '<span class="fc-day-grid-event fc-h-event fc-event fc-start fc-end" style="background-color:' + event.backgroundColor + ';">';
            content += '<a class="edit-task" data-id="' + event.id + '" style="color: ' + event.textColor + ';"><div class="fc-content"><span class="fc-title"><u>' + event.title + '</u></span></div></a>';
            content += '<a class="edit-note" data-id="' + event.id + '" style="color: ' + event.textColor + ';" data-title="' + event.title + '" data-description="' + event.description + '"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Ghi chú: ' + (event.description ? event.description : '') + '</a>';
            content += '</span>';

            return $(content);
        },
        events: function (start, end, timezone, callback) {
            var events = [];
            if (jsonData) {
                $.each(jsonData, function (index, item) {
                    if (item.StartDate != null) {
                        var startDate = new Date(parseInt(item.StartDate.substr(6)));
                        var formatStartDate = moment(startDate).format('YYYY-MM-DD HH:mm');
                        var startTime = GetTime(startDate);

                        //Nếu không có EndDate thì lấy theo EstimatedDate để hiển thị
                        //Nếu không có EstimatedDate và EndDate thì hiển thị trong 1 ngày (StartDate)
                        if (item.EndDate == null) {
                            item.EndDate = item.EstimateEndDate;
                            if (item.EndDate == null) {
                                item.EndDate = item.StartDate;
                            }
                        }
                        var endDate = new Date(parseInt(item.EndDate.substr(6)));
                        var formatEndDate = moment(endDate).format('YYYY-MM-DD HH:mm');
                        var endTime = GetTime(endDate);

                        var title = startTime + " - " + item.Summary;
                        var allDay = false;
                        if (startTime == "00:00") {
                            allDay = true;
                            title = item.Summary;
                        }
                        if (endTime == "00:00") {
                            formatEndDate = moment(endDate).format('YYYY-MM-DD 23:59');
                            allDay = false;
                        }

                        events.push({
                            id: item.TaskId, //set id into event
                            title: title, //label on event
                            start: formatStartDate,
                            end: formatEndDate,
                            allDay: allDay,
                            backgroundColor: item.TaskStatusBackgroundColor,
                            textColor: item.TaskStatusColor
                        });
                    }
                });
            }
            callback(events);
        },
        eventClick: function (event) {
            //loading2();

            //var TaskId = event._id;
            //var KanbanId = $("input[name='KanbanId']").val();
            //SaveTaskDataPopup(TaskId, KanbanId);
        }
    });

    //disable past
    $(".fc-past").prop('disabled', true);
    $(".fc-past").css('opacity', 0.5);
}

$(document).on("click", ".fc-next-button", function () {
    //loading2();
});

$(document).on("click", ".fc-prev-button", function () {
    //loading2();
});

$(document).on("click", "#toggle-tab-calendarGTB", function () {
    //loading2();
});

$(document).on("click", ".btn-del-profile", function () {
    $("input[name='ProfileId']").val("");
    $("input[name='ProfileName']").val("");
});

$(document).on("click", ".btn-del-contact", function () {
    $("input[name='ContactId']").val("");
    $("input[name='ContactName']").val("");

    $("input[name='CompanyId']").val("");
    $("input[name='CompanyName']").val("");
});

$(document).on("click", ".btn-del-construction", function () {
    $("input[name='ConstructionUnit']").val("");
    $("input[name='ConstructionUnitName']").val("");
    $("input[name='Construction_Phone']").val("");
    $("input[name='Construction_Email']").val("");
    $("input[name='Construction_ContactName']").val("");

    $("input[name='Construction_SalesSupervisorName']").val("");
    $(".Construction_DepartmentName").html("");
});

$(document).on("change", "select[name='ProductCategoryCode']", function () {
    var ProductCategoryCode = $(this).val();

    $.ajax({
        type: "POST",
        url: "/Work/Task/GetUsualErrorByProductCategory",
        data: {
            ProductCategoryCode: ProductCategoryCode,
            IsTakeAll: true
        },
        success: function (jsonData) {
            $("#UsualErrorCode").html("");
            $("#UsualErrorCode").append("<option value=''>-- Vui lòng chọn --</option>");
            if (jsonData != null && jsonData.length > 0) {
                $.each(jsonData, function (index, value) {
                    $("#UsualErrorCode").append("<option value='" + value.CatalogCode + "'>" + value.CatalogText_vi + "</option>");
                });
            }
        }
    });
});

$(document).on("change", "input[name='Property3']", function () {
    var value = $(this).val();
    $(".numberProperty3").html("");
    var formatCurrencyProperty3 = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".numberProperty3").html(formatCurrencyProperty3);
    }
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