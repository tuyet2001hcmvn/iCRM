var employeeList = [];
$(document).on('click', '#btn-save', function () {
    var $btn = $(this);
    $btn.button('loading');
    var data = GetData();
    if (data != undefined && data != null) {
        var id = $('#Id').val();
        SaveData(id, data, $btn);
    }
});
$(document).on('click', '#btn-add-member', function () {
    $.ajax({
        type: "GET",
        url: "/Work/AssignedGroup/GetAccountInGroup",
        processData: false,
        success: function (htmlData) {
            // $("#divTaskPersonGroupPopup #group-name").html();
            $("#divTaskPersonGroupPopup #divSearchResult").html(htmlData);
            //ISD.Pagging(false, 100);

            $("#divTaskPersonGroupPopup #tbl-employee").DataTable({
                pageLength: 25,
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
                    sSearch: "Tìm nội dung : ",
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

            $('#divTaskPersonGroupPopup #tbl-employee_filter').show();
            $('#divTaskPersonGroupPopup #tbl-employee_filter').css('text-align', 'right');
            if (employeeList.length > 0) {
                var table = $('#divTaskPersonGroupPopup #tbl-employee').DataTable();
                for (var i = 0; i < employeeList.length; i++) {
                    table.rows({ order: 'index', search: 'applied' }).nodes().to$().find('input[type="checkbox"][value="' + employeeList[i].Code + '"]').prop("checked", true);
                }
            }
            //  $("#divTaskPersonGroupPopup #CheckAll").prop("checked", true).trigger('change');
            $("#divTaskPersonGroupPopup").modal("show");
        }
    });
});

$(document).on('change', '#divTaskPersonGroupPopup #CheckAll', function () {
    var table = $('#divTaskPersonGroupPopup #tbl-employee').DataTable();
    if ($('#divTaskPersonGroupPopup #CheckAll').is(":checked")) {
        //$('.salesEmployeeList').prop("checked", true);
        table.rows({ order: 'index', search: 'applied' }).nodes().to$().find('input[type="checkbox"]').prop("checked", true);
    }
    else {
        //$('.salesEmployeeList').prop("checked", false);
        table.rows({ order: 'index', search: 'applied' }).nodes().to$().find('input[type="checkbox"]').prop("checked", false);
    }
});
//$(document).on('click', '#btn-save-edit', function () {
//    var $btn = $(this);
//    $btn.button('loading');
//    var data = GetData();
//    if (data != undefined && data != null) {
//        SaveData(data, true);
//    }
//    $btn.button('reset');
//});
function GetData() {
    var arr = {};
    var frm = $("#frmCreate");
    if (frm.valid()) {
        var data = frm.serializeArray();
        var obj = {};
        $.each(data, function (index, val) {
            obj[val.name] = val.value;
            $.extend(true, arr, obj)
        });
        if (employeeList.length == 0) {
            alertPopup(false, "The Thành viên trong nhóm field is required.");
        }
        else {
            var employeeCodeList = [];
            for (var i = 0; i < employeeList.length; i++) {
                employeeCodeList.push(employeeList[i].Code);
            }
            obj["AccountIdList"] = employeeCodeList;
            $.extend(true, arr, obj);
        }
        return obj;
    }
    else {
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
    }
}
function SaveData(id, data, e) {
    loading2();
    $.ajax({
        type: "POST",
        url: "/Work/AssignedGroup/Edit/" + id,
        data: data,
        dataType: "json",
        success: function (res) {
            if (res.IsSuccess === true) {
                alertPopup(true, res.Message);
                e.button("reset");
            }
            else {
                alertPopup(false, res.Message);
                e.button("reset");
            }
        },
        error: function (res) {
            alertPopup(false, res.Message);
            e.button("reset");
        }
    });
}
function RenderTable() {
    var len = employeeList.length;
    if (len > 0) {
        tbody = '';
        if (len > 0) {
            for (var i = 0; i < len; i++) {
                var tr = '<tr>' +
                    '<td class="text-center">' + parseInt(i + 1) + '</td >' +
                    '<td>' + employeeList[i].Code + '</td >' +
                    '<td>' + employeeList[i].Name + '</td >' +
                    '<td>' + employeeList[i].Department + '</td >' +

                    '<td class="text-center">' +
                    '<a class="btn btn-danger btn-del-employee" data-row="1" data-code="' + employeeList[i].Code + '">Xóa</a>' +
                    '</td>' +
                    '</tr>';
                tbody = tbody + tr;
            }
            $('#empTbl').html(tbody);
        }
    }
    else {
        $('#empTbl').html("");
    }
}
$(document).on("click", ".btn-save-task-person-group", function () {
    employeeList = [];
    var table = $('#tbl-employee').DataTable();
    table.rows({ order: 'index', search: 'applied' }).nodes().to$().find('input[type="checkbox"]').each(function (index, element) {
        if ($(element).is(":checked")) {
            var employee = {
                Code: $(element).val(),
                Name: $(element).data("name"),
                Department: $(element).data("department")
            }
            //var isExist = false;
            //for (var i = 0; i < employeeList.length; i++) {
            //    if (employee.Code == employeeList[i].Code) {
            //        isExist = true;
            //        break;
            //    }
            //}
            //if (!isExist) {
            employeeList.push(employee);
            //  }
        }

    });

    $('#divTaskPersonGroupPopup').modal('hide');
    RenderTable();
});
$(document).on("click", ".btn-del-employee", function () {
    var code = $(this).data("code");
    for (var i = 0; i < employeeList.length; i++) {
        if (code == employeeList[i].Code) {
            employeeList.splice(i, 1);
            break;
        }
    }
    RenderTable();
});
function GetDataForEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Work/AssignedGroup/GetById",
        data: { id: id },
        dataType: "json",
        success: function (res) {
            if (res.IsSuccess === true) {
                $("#GroupName").val(res.Data.GroupName);
                $("#Type").val(res.Data.GroupType);
                for (var i = 0; i < res.Data.AccountIdList.length; i++) {
                    var employee = {
                        Code: res.Data.AccountIdList[i].SalesEmployeeCode,
                        Name: res.Data.AccountIdList[i].SalesEmployeeName,
                        Department: res.Data.AccountIdList[i].DepartmentName
                    }
                    employeeList.push(employee);
                }
                RenderTable();
            }
            else {
                alertPopup(false, res.Message);
            }
        },
        error: function (res) {
            alertPopup(false, res.Message);
        }
    });
}