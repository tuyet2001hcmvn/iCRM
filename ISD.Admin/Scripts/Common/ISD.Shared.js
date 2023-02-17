// Active sidebar menu
$(document).ready(function () {
    var $body = $('body'),
        menu_state = $.cookie('menu_state');
    selected_module = $.cookie('selected_module');
    if (menu_state && menu_state === 'open') {
        $body.removeClass('sidebar-collapse');
    }
    // Expand sub menu
    var pathName = location.pathname + location.search;
    var $currentA = $(".sidebar-menu ul li a[href='" + pathName + "']");

    $currentA.parent().addClass('active');
    $currentA.closest('.treeview-menu').show();
    $currentA.closest('.treeview').addClass('menu-open');

    $(".dataTables_filter").prepend("<span class='search-icon'></span>");
    $(".dataTables_filter").append($(".has-btn-add-new").html());
    $(".has-btn-add-new").html("");

    ISD.SidebarSearch.init();

    //if (window.OneSignal || OneSignal) {
    //    //Lưu id OneSignal vào Account
    //    OneSignal.getUserId(function (id) {
    //        console.log(id);
    //    });
    //}

    $(window).not("textarea").keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    $('.required-icon').tooltip({
        placement: 'left',
        title: 'Vui lòng nhập thông tin này!'
    });

    //loading when load datatable first time
    $("body").has("#frmSearch").addClass("loading2");
});

function convertListToArray(id) {
    var dataList = $('#' + id).val();
    var data = {};
    if (dataList != null && dataList.length > 0) {
        data[id] = dataList;
    }
    return data;
}

function loading(isLoading) {
    if (isLoading) {
        $(".overlay").show();
    } else {
        $(".overlay").hide();
    }
}

function getParam(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

//just hide alert when close, not remove
$(document).on("click", ".alert-close", function () {
    var div = $(this).data("div");
    $("#" + div + " .alert-message").html("");
    $('.alert').hide();

    //Modal import
    $(".modal-alert-message").html("");

});

//loading.gif when processing
function loading2() {
    $("body").addClass("loading2");
}
$(document).ajaxStop(function () {
    $("body").removeClass("loading2");
    $(".with-search").select2();

    $("#btn-search").button("reset");
});

$("#btn-search").click(function () {
    $("#btn-search").button("loading");
});

//calculator
//$(document).on("click", "#myCalculator", function () {
//    $('#inlineCalc').calculator();
//});
//$(document).on("click", ".calculator-row button, .calculator-result", function (e) {
//    e.stopPropagation();
//});

//select2
$('select').select2({
    width: '100%'
});
$('select').not('.with-search').select2({
    minimumResultsForSearch: Infinity,
});
$(".with-search").select2();
$("#ChildrenClassId").select2("destroy");

$(document.body).on('hide.bs.modal,hidden.bs.modal', function () {
    $('body').css('padding-right', '0');
});

//Xử lý nút chọn chi nhánh
//save-current-store
$(document).on("click", "#save-current-store", function () {
    var $btn = $(this);
    $btn.button('loading');
    var saleOrg = $("#CurrentSaleOrg").val();
    $.ajax({
        type: "POST",
        url: "/Permission/Auth/ChangeCurrentStore",
        data: {
            saleOrgCode: saleOrg,
        },
        success: function (jsonData) {
            $btn.button('reset');
            if (jsonData == "Success") {
                $("#storeModal").modal("hide");
                location.reload();
            }
            else {
                $("#store-error-message").html("Đã xảy ra lỗi khi đổi chi nhánh! Vui lòng thử lại sau.")
            }
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            alertPopup(false, xhr.responseText);
        }
    });
});
//$(document).on('click', '.sidebar-toggle', function () {
//    //Set sidebar-collapse
//    var $body = $('body'),
//        $btn = $(this),
//        menu_state = $.cookie('menu_state');
//    if (!menu_state || (menu_state && menu_state === 'close')) {
//        //Đang không set hoặc set đóng => set lại mở
//        //Do mặc định là close
//        menu_state = 'open';
//    } else {
//        //Đang mở => set đóng
//        menu_state = 'close';
//    }
//    $.cookie('menu_state', menu_state);

//    $(".isd-table-th-stt").trigger("click");
//});

//Resize header datatable when toggle side bar
//$(document).on('click', '.sidebar-toggle', function () {
//    $(".isd-table-th-stt").trigger("click");
//});

$(document).ready(function () {
    //setTimeout(function () {
    //    $(".isd-table-th-stt").trigger("click");
    //}, 5);
});

$(document).on("click", ".nav-tabs li a", function () {
    $(".isd-table-th-stt").trigger("click");
});

ISD.LoadingDataTable = function (processing, element) {
    var height = $(element + " tbody").height();
    var width = $(element).width();

    var ele = $(element).parent(".dataTables_scrollBody");
    var processingElement = ele.find('.dataTables_processing');
    if (processingElement.length == 0) {
        $(element).parent(".dataTables_scrollBody").append('<div class="dataTables_processing"></div>');
    }
    $(processingElement).css('width', width + 10);
    $(processingElement).css('padding-top', height / 2);
    $(processingElement).css('display', processing ? 'block' : 'none');

    if ($($("ul.nav-tabs li.active a").attr('href')).find('.dataTables_processing').length > 0 || $('.nav-tabs-custom').length == 0) {
        //Loading content
        var contentWidth = $(element).parent(".dataTables_scrollBody").width();
        if (contentWidth > 0) {
            $('.loading-content').css($(element).parent(".dataTables_scrollBody").position());
            $('.loading-content').css('left', contentWidth / 2);
            $('.loading-content').css('padding-top', height / 2);
            $('.loading-content').css('display', $(element + ' tbody tr').length > 0 && processing ? 'block' : 'none');
        }
    }
}

