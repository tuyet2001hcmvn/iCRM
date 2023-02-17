var indexRow = 0;
$(document).on('click', '.btn-addTaskStatus', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".taskstatus_fields").each(function (index, value) {
        indexRow = index;
    });
    indexRow++;

    var controlForm = $('.taskControls:first'),
        currentEntry = $(this).parents('.taskstatus_fields:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    
    newEntry.find('.taskStatusCode').attr('name', 'taskStatusList[' + indexRow + '].TaskStatusCode').val('');
    newEntry.find('.taskName').attr('name', 'taskStatusList[' + indexRow + '].TaskStatusName').val('');
    newEntry.find('.taskProcessCode').attr('name', 'taskStatusList[' + indexRow + '].ProcessCode').val('');
    newEntry.find('.taskOrderIndex').attr('name', 'taskStatusList[' + indexRow + '].OrderIndex').val('');
    newEntry.find('.taskStatusId').attr('name', 'taskStatusList[' + indexRow + '].TaskStatusId').val('');
    newEntry.find('.taskCategory').attr('name', 'taskStatusList[' + indexRow + '].Category').val('');
    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.taskstatus_fields:not(:first) .btn-addTaskStatus')
        .removeClass('btn-addTaskStatus').addClass('btn-removeTaskStatus')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeTaskStatus', function (e) {
    $(this).parents('.taskstatus_fields:last').remove();

    $(".taskstatus_fields").each(function (index, value) {
        $(this).find('.taskStatusId').attr("name", "taskStatusList[" + index + "].taskStatusId");
        $(this).find('.taskStatusCode').attr("name", "taskStatusList[" + index + "].TaskStatusCode");
        $(this).find('.taskName').attr("name", "taskStatusList[" + index + "].TaskStatusName");
        $(this).find('.taskProcessCode').attr("name", "taskStatusList[" + index + "].ProcessCode");
        $(this).find('.taskOrderIndex').attr("name", "taskStatusList[" + index + "].OrderIndex");
        $(this).find('.taskCategory').attr("name", "taskStatusList[" + index + "].Category");
    });
    e.preventDefault();
    return false;
});