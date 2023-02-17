
//Mở popup cập nhật trạng thái của dự án
//1. Chọn trạng thái chuyển đến tùy ý
$(document).on("click", "#btn-create-status", function () {
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    $.ajax({
        type: "GET",
        url: '/Customer/Activities/_Create',
        data: { ProfileId: profileId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmTaskTransition");
    });
    $btn.button("reset");
});

//2. Cập nhật trạng thái theo cấu hình
$(document).on("click", ".btn-update-status", function () {
    var isRequiredComment = $(this).data("comment-required");
    if (isRequiredComment === "True" || isRequiredComment === "true") {
        var Note = $("#frmTaskTransition #Note").val();
        if (!Note) {
            var popup = "#popupProfile";
            $(popup + " .divPopupMessage .alert-message").html("");
            ISD.setMessage(popup + " .divPopupMessage", "Vui lòng nhập thông tin \"Nội dung\"");
            $(popup + " .divPopupMessage").show();

            return;
        }
    }
    var $btn = $(this);
    $btn.button("loading");

    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var ToStatusId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/Activities/_Create',
        data: { ProfileId: profileId, ToStatusId: ToStatusId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmTaskTransition");
    });
    $btn.button("reset");
});

ReloadActivitiesList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/Activities/_List/" + profileId + "?isLoadContent=true";
    $("#contentActivities table tbody").load(requestUrl);

    $.ajax({
        type: "GET",
        url: '/Customer/Activities/GetTransitionList',
        data: { ProfileId: profileId }
    }).done(function (jsonData) {
        if (jsonData.Success) {
            if (jsonData.Data && jsonData.Data.length > 0) {
                $("#transition-list").empty();
                $.each(jsonData.Data, function (index, value) {
                    var htmlContent = '<li>';
                    htmlContent += '<a class="btn btn-update-status"';
                    htmlContent += 'style="color:' + value.Color + '; margin-bottom: 5px; margin-top: 5px;"';
                    htmlContent += 'data-id="' + value.ToStatusId + '"';
                    htmlContent += 'data-comment-required="' + value.isRequiredComment + '">';
                    htmlContent += ' <i class="fa fa-pencil"></i> ' + value.TransitionName;
                    htmlContent += '</a>';
                    htmlContent += '</li>';
                    $("#transition-list").append(htmlContent);
                });
            }
        }
    }); 
};

//Lưu thông tin cập nhật trạng thái
$(document).on("click", "#popupProfile #btn-save-status", function () {
    ISD.SaveDataWithPopup("/Customer/Activities/Save", "#frmTaskTransition", this, "#popupProfile", false, "ReloadActivitiesList");
});

function reloadPage() {
    location.reload();
}

$(document).on("click", "#contentActivities .btn-edit", function () {
    var $btn = $(this);
    $btn.button("loading");

    var TaskTransitionLogId = $(this).data("id");
    $.ajax({
        type: "GET",
        url: '/Customer/Activities/_Edit',
        data: { TaskTransitionLogId: TaskTransitionLogId }
    }).done(function (html) {
        $btn.button("reset");
        $("#popupProfile").find(".modal-content").html(html).end().modal("show");
        ISD.ValidationOnModalPopup("#frmTaskTransition");
    });
    $btn.button("reset");
});