
//Calendar
function calendarFunction_Calendar(Type, KanbanId, workflowList, processCodeList) {

    $('#calendar_MyWork').fullCalendar({
        height: 650,
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
        eventRender: function (event, element, view) {
            //return $('<div>' + event.title + '</div>');
            let content = '';
            if (event.isRenderButton) {
                //content = "<a class='btn btn-default create-new-task' data-date='" + moment(event.start).format("YYYY-MM-DD") + "'><i class='fa fa-plus-square'> Thêm mới công việc</i></a>";
                content = "<a class='btn btn-default create-new-task' data-date='" + moment(event.start).format("YYYY-MM-DD") + "'><i class='fa fa-plus-square'> </i></a>";
            }
            else {
                content = '<span class="fc-day-grid-event fc-h-event fc-event fc-start fc-end" style="background-color:' + event.backgroundColor + ';">';
                content += '<a class="edit-task" data-id="' + event.id + '" style="color: ' + event.textColor + ';"><div class="fc-content"><span class="fc-title"><u>' + event.title + '</u></span></div></a>';
                content += '<a class="edit-note" data-id="' + event.id + '" style="color: ' + event.textColor + ';" data-title="' + event.title + '" data-description="' + event.description + '"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Ghi chú: ' + (event.description ? event.description : '') + '</a>';
                content += '</span>';
            }
            return $(content);
        },
        events: function (start, end, timezone, callback) {
            loading2();
            $.ajax({
                url: '/Work/Calendar/_PaggingServerSide',
                type: 'POST',
                data: {
                    Type: Type,
                    KanbanId: KanbanId,
                    workflowList: workflowList,
                    processCodeList: processCodeList,
                    //Load theo từ ngày đến ngày trên lịch
                    StartCommonDate: "Custom",
                    StartFromDate: moment(start._d).format('YYYY-MM-DD HH:mm'),
                    StartToDate: moment(end._d).format('YYYY-MM-DD HH:mm'),
                },
                success: function (jsonData) {
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
                                //Nếu event hiển thị trong 1 ngày (theo StartDate), chỉ hiển thị duration 1h
                                if (item.StartDate == item.EndDate) {
                                    endDate.setHours(endDate.getHours() + 1);
                                }
                                var formatEndDate = moment(endDate).format('YYYY-MM-DD HH:mm');
                                var endTime = GetTime(endDate);

                                //Nếu không có StartTime => sự kiện cả ngày
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
                                    textColor: item.TaskStatusColor,
                                    description: item.ShortNote,
                                });
                            }
                        });
                    }

                    var between = [];
                    for (var m = moment(start); m.diff(end, 'days') <= 0; m.add(1, 'days')) {
                        //console.log(m.format('YYYY-MM-DD'));
                        between.push(m.format('YYYY-MM-DD'));
                    }
                    $.each(between, function (index, item) {
                        events.push({
                            id: Math.random(), //set id into event
                            title: "Thêm mới task", //label on event
                            start: item,
                            end: item,
                            allDay: true,
                            isRenderButton: true
                        });
                    });

                    callback(events);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });


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

function calendarFunction_CalendarGTB() {

    $('#calendarGTB').fullCalendar({
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
        eventRender: function (event, element, view) {
            //return $('<div>' + event.title + '</div>');
            let content = '<span class="fc-day-grid-event fc-h-event fc-event fc-start fc-end" style="background-color:' + event.backgroundColor + ';">';
            content += '<a class="edit-task" data-id="' + event.id + '" style="color: ' + event.textColor + ';"><div class="fc-content"><span class="fc-title"><u>' + event.title + '</u></span></div></a>';
            content += '<a class="edit-note" data-id="' + event.id + '" style="color: ' + event.textColor + ';" data-title="' + event.title + '" data-description="' + event.description + '"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Ghi chú: ' + (event.description ? event.description : '') + '</a>';
            content += '</span>';

            return $(content);
        },
        events: function (start, end, timezone, callback) {
            loading2();
            $.ajax({
                url: '/Work/Calendar/_PaggingServerSide_GTB',
                type: 'POST',
                data: {
                    CompanyId: $("select[name='CompanyId']").val(),
                    RolesCode: $("select[name='RolesCode']").val(),
                    Reporter: $("select[name='Reporter']").val(),
                    Assignee: $("select[name='Assignee']").val(),

                    //Load theo từ ngày đến ngày trên lịch
                    StartCommonDate: "Custom",
                    StartFromDate: moment(start._d).format('YYYY-MM-DD HH:mm'),
                    StartToDate: moment(end._d).format('YYYY-MM-DD HH:mm'),
                },
                success: function (jsonData) {
                    var events = [];
                    if (jsonData) {
                        $.each(jsonData, function (index, item) {
                            if (item.RemindDate != null) {
                                var startDate = new Date(parseInt(item.RemindDate.substr(6)));
                                var formatStartDate = moment(startDate).format('YYYY-MM-DD HH:mm');
                                var startTime = GetTime(startDate);

                                var endDate = new Date(parseInt(item.RemindDate.substr(6)));
                                //Chỉ hiển thị duration 1h
                                endDate.setHours(endDate.getHours() + 1);
                                var formatEndDate = moment(endDate).format('YYYY-MM-DD HH:mm');
                                var endTime = GetTime(endDate);

                                //Nếu không có StartTime => sự kiện cả ngày
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
                                    textColor: item.TaskStatusColor,
                                    description: item.ShortNote,
                                });
                            }
                        });
                    }
                    callback(events);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        },
        eventClick: function (event) {
            //loading2();

            //var TaskId = event._id;
            //SaveTaskDataPopup(TaskId);
        }
    });

    //disable past
    $(".fc-past").prop('disabled', true);
    $(".fc-past").css('opacity', 0.5);
}

//handle click event custom
$(document).on("click", "#calendar_MyWork .edit-task", function () {
    loading2();

    var TaskId = $(this).data('id');
    var KanbanId = $("input[name='KanbanId']").val();
    SaveTaskDataPopup(TaskId, KanbanId);
});

$(document).on("click", "#calendarGTB .edit-task", function () {
    loading2();

    var TaskId = $(this).data('id');
    SaveTaskDataPopup(TaskId);
});

$(document).on("click", ".edit-note", function () {
    var id = $(this).data("id");
    var title = $(this).data("title");
    var description = $(this).data("description");
    $("#popupTaskShortNote #popup-title").html(title);
    $("#popupTaskShortNote #ShortNoteTaskId").val(id);
    if (description && description !== "undefined") {
        $("#popupTaskShortNote #ShortNote").val(description);
    }

    $("#popupTaskShortNote").modal("show");
});

$(document).on("click", "#btn-save-short-note", function () {
    $.ajax({
        type: "POST",
        url: '/Work/Calendar/SaveShortNote',
        data: $('#frmUpdateTaskShortNote').serialize(),
        success: function (html) {
            if (html.Success === false) {
                //alertPopup(false, html.Data);
                setModalMessage("#divAlertWarningTaskShortNote", html.Data);
                setTimeout(function () {
                    $('#divAlertWarningTaskShortNote').show();
                }, 500);
            }
            else {
                $("#popupTaskShortNote").modal("hide");
                if (GetListSearchCalendar) {
                    GetListSearchCalendar();
                }
                if (GetListSearchCalendar_GTB) {
                    GetListSearchCalendar_GTB();
                }
            }
        }
    });
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
            }
        }
    });
}

function GetTime(date) {
    if (date.value !== "") {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        hours = hours < 10 ? "0" + hours : hours;
        minutes = minutes < 10 ? "0" + minutes : minutes;

        var displayTime = hours + ":" + minutes;
        return displayTime;
    }
}

function ConvertTime(time) {
    if (time.value !== "") {
        var hours = time.Hours;
        var minutes = time.Minutes;
        hours = hours < 10 ? "0" + hours : hours;
        minutes = minutes < 10 ? "0" + minutes : minutes;

        var displayTime = hours + ":" + minutes;
        return displayTime;
    }
}

$(document).on("click", ".create-new-task", function () {
    //alert($(this).data("date"));
    var $btn = $(this);
    var date = $(this).data("date");
    $btn.button("loading");
    $.ajax({
        type: "POST",
        url: '/Work/Calendar/_CreateNewTask',
        data: {

        },
        success: function (jsonData) {
            //$("#popupCreateNewTask #popup-title").html(title);
            //if (description && description !== "undefined") {
            //    $("#popupTaskShortNote #ShortNote").val(description);
            //}
            $("#popupCreateNewTask #popup-title").html("THÊM MỚI CÔNG VIỆC");
            $("#popupCreateNewTask #StartDate").val(date);
            $('#popupCreateNewTask #TaskType').empty();
            //set option
            if (jsonData && jsonData.length > 0) {
                $.each(jsonData, function (index, value) {
                    $('<option>').val(value.id).text(value.name).appendTo('#popupCreateNewTask #TaskType');
                });
            }

            $("#popupCreateNewTask").modal("show");
            $btn.button("reset");
        }
    });
});

$(document).on("click", "#btn-create-new-task", function () {
    var Type = $('#popupCreateNewTask #TaskType').val();
    var StartDate = $('#popupCreateNewTask #StartDate').val();
    var url = "/Work/Task/Create?Type=" + Type + "&StartDate=" + StartDate;

    $("#popupCreateNewTask").modal("hide");
    window.open(url, '_blank');
});


