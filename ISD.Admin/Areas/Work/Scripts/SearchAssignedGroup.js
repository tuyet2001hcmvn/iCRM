var config = {
    pageSize: parseInt($('#changeViewRow').val()),
    pageIndex: 1,
    type: $("#Type").val()
};
$(document).on('click','#btn-search', function () {
    LoadData(true);
});
$(document).on('change','#changeViewRow',function () {
    config.pageSize = $('#changeViewRow').val();
    LoadData(true);
});
function LoadData(changePageSize) {
    var GroupName = $('#GroupName').val();
    var url = '/Work/AssignedGroup/Search?GroupName=' + GroupName + '&pageIndex=' + config.pageIndex + '&pageSize=' + config.pageSize + '&Type=' + config.type;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.IsSuccess) {
                if (res.TotalRow !== 0) {
                    var data = res.Data;
                    var html = '';
                    var template = $('#data-template').html();
                    $('#tblData').html("");
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            i: (config.pageIndex - 1) * config.pageSize + i + 1,
                            GroupId: item.GroupId,
                            GroupName: item.GroupName,
                            GroupType: item.GroupType
                        });
                    });
                    $('#tblData').html(html);
                    Paging(res.TotalRow, function () {
                        LoadData(false);
                    }, config.pageSize, changePageSize);
                    var rowInPage = parseInt(Object.keys(data).length);
                    var count = "Đang xem " + (config.pageIndex * config.pageSize - config.pageSize + 1) + " đến " + (((config.pageIndex - 1) * config.pageSize) + rowInPage) + " trong tổng số " + res.TotalRow + " mục"
                    $('#tblCount').html(count);
                }
                else {
                    $('#tblData').html("");
                    $('#tblCount').html("Không tìm thấy dòng nào phù hợp");
                }
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
function Paging(totalRow, callback, pageSize, changePageSize) {
    if (totalRow === 0) {
        return;
    }
    var totalPage = Math.ceil(totalRow / pageSize);
    if ($('#pagination a').length === 0 || changePageSize === true) {
        $('#pagination').empty();
        $('#pagination').removeData("twbs-pagination");
        $('#pagination').unbind("page");
    }

    $('#pagination').twbsPagination({
        totalPages: totalPage,
        first: '<span aria-hidden="true">&laquo;</span>',
        next: '',
        prev: '',
        last: '<span aria-hidden="true">&raquo;</span>',
        visiblePages: 10,
        onPageClick: function (event, page) {
            config.pageIndex = page;
            setTimeout(callback, 200);
        }
    });
}
$(document).on("click", ".btn-delete", function () {
    $("#divDeletePopup .modal-title .item-name").html($(this).data('name'));
    //set text
    var text = $("#divDeletePopup .alert-message").html();
    //Replace new text
    text = text.replace(/"([^"]*)"/g, '"' + $(this).data('name') + '"');
    text = String.format(text, $(this).data('name'));
    //Show new text
    $("#divDeletePopup .alert-message").html(text);
    $('#btn-confirm-delete').attr('data-id', $(this).data('id'));
    $("#divDeletePopup").modal("show");
});
$(document).on("click", "#btn-confirm-delete", function () {
    var id = $(this).attr('data-id');
    $.ajax({
        type: "POST",
        url: "/Work/AssignedGroup/Delete",
        data: {
            id: id
        },
        success: function (res) {
            if (res.IsSuccess) {
                $('#divDeletePopup').modal("hide");
                alertPopup(true, "Xóa nhóm thành công");
                LoadData(true);
            }
            else {
                alertPopup(false, res.Message);
            }
        },
        error: function (res) {
            alertPopup(false, res.Message);
        }
    });
});