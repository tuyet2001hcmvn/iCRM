var ISDPivotTemplate = {
    Init : function (pageUrl,controller,parameter) {
        $(document).on("click", "#btn-save-sysadmin", function () {
            $('.modal-title').html("LƯU MẪU BÁO CÁO");
            $('#btn-popup-save').attr('data-mode', 'CREATE');
            $('#btn-popup-save').attr('data-role', 'SYSADMIN');
            $('#TemplateName').val("");
            $('#IsDefault').prop('checked', false);
            $('#popupSaveTemplate').modal("show");
        });
        $(document).on("click", "#btn-save-user", function () {
            $('.modal-title').html("LƯU MẪU BÁO CÁO");
            $('#btn-popup-save').attr('data-mode', 'CREATE');
            $('#btn-popup-save').attr('data-role', 'USER');
            $('#TemplateName').val("");
            $('#IsDefault').prop('checked', false);
            $('#popupSaveTemplate').modal("show");
        });
        $(document).on("click", ".btn-update-template", function () {
            $('.modal-title').html("CẬP NHẬT MẪU BÁO CÁO");
            $('#btn-popup-save').attr('data-mode', 'EDIT');
            $('#TemplateName').val($(this).data('name'));
            $('#btn-popup-save').attr('data-id', $(this).data('id'));
            var isDefault = $(this).data('default');
            if (isDefault == true || isDefault == "True") {
                $('#IsDefault').prop('checked', true);
            }
            else {
                $('#IsDefault').prop('checked', false);
            }
            $('#popupSaveTemplate').modal("show");
        });
        $(document).on("click", ".btn-delete-template", function () {
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
        //$(document).off("click", ".pivot-template-item").on("click", ".pivot-template-item", function () {
        //    var arr = {};
        //    var obj = {};
        //    var templateId = $(this).data('id');
        //    obj["pivotTemplate"] = templateId;

        //    $.extend(true, arr, obj);
        //    var url = "/"+pageUrl + "/ChangeTemplate";
        //    ISD.Download(url, arr);
        //});
        $(document).on("click", "#btn-confirm-delete", function () {
            var templateId = $(this).data('id');
            $.ajax({
                type: "POST",
                url: "/PivotGridTemplate/Delete",
                data: {
                    templateId: templateId
                },
                success: function (jsonData) {
                    if (jsonData.Success) {
                        $('#divDeletePopup').modal("hide");
                        alertPopup(true, "Xóa mẫu báo cáo thành công");
                    }
                    else {
                        alertPopup(false, jsonData.Message);
                    }
                },
                error: function (jsonData) {
                    alertPopup(false, jsonData.Message);
                }
            });
            location.reload();
        });
        $(document).on("click", "#btn-popup-save", function () {
            var templateName = $('#TemplateName').val();
            var isDefault = $('#IsDefault').is(':checked');
            console.log(isDefault);
            var url = "/" + pageUrl;
            var pageParameter = null;
            if (parameter != "" && parameter != null && parameter != undefined) {
                pageParameter = parameter;
            }
            var saveMode = $(this).data('mode');
            if (templateName == "" || templateName == undefined || templateName == null) {
                alertPopup(false, "Vui lòng nhập tên mẫu báo cáo");
            }
            else {
                if (saveMode == "CREATE") {
                    var role = $(this).data('role');
                    if (role == "SYSADMIN") {
                        $.ajax({
                            type: "POST",
                            url: "/PivotGridTemplate/Create",
                            data: {
                                templateName: templateName,
                                pageUrl: url,
                                parameter: pageParameter,
                                isSystem: true,
                                isDefault: isDefault
                            },
                            success: function (jsonData) {
                                if (jsonData.Success) {
                                    $('#TemplateName').val("");
                                    $('#popupSaveTemplate').modal("hide");
                                    alertPopup(true, "Lưu mẫu báo cáo thành công");
                                    location.reload();
                                }
                                else {
                                    alertPopup(false, "Vui lòng chỉnh sửa báo báo trước khi lưu");
                                }
                            },
                            error: function (jsonData) {
                                alertPopup(false, jsonData.Message);
                            }
                        });
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "/PivotGridTemplate/Create",
                            data: {
                                templateName: templateName,
                                pageUrl: url,
                                parameter: pageParameter,
                                isSystem: false,
                                isDefault: isDefault
                            },
                            success: function (jsonData) {
                                if (jsonData.Success) {
                                    $('#TemplateName').val("");
                                    $('#popupSaveTemplate').modal("hide");
                                    alertPopup(true, "Lưu mẫu báo cáo thành công");
                                    location.reload();
                                }
                                else {
                                    alertPopup(false, jsonData.Message);
                                }

                            },
                            error: function (jsonData) {
                                alertPopup(false, jsonData.Message);
                            }
                        });
                    }
                }
                else {
                    var templateId = $(this).data('id');
                    $.ajax({
                        type: "POST",
                        url: "/PivotGridTemplate/Edit",
                        data: {
                            templateId: templateId,
                            templateName: templateName,
                            isDefault: isDefault
                        },
                        success: function (jsonData) {
                            if (jsonData.Success) {
                                $('#TemplateName').val("");
                                $('#popupSaveTemplate').modal("hide");
                                alertPopup(true, "Cập nhật mẫu báo cáo thành công");
                                location.reload();
                            }
                            else {
                                alertPopup(false, "Vui lòng chỉnh sửa báo báo trước khi lưu");
                            }
                        },
                        error: function (jsonData) {
                            alertPopup(false, jsonData.Message);
                        }
                    });
                }
                
            }

        });
    }
};
