

function isArray(what) {
    return Object.prototype.toString.call(what) === '[object Array]';
}

var ISD = {};
var ISDAPI = {};
//create: Save Data
ISD.SaveData = function (controller, frmCreate, isContinue, e, isSaveImageToAPIUrl, isToEditMode, isToViewMode) {
    controller = controller.split('?')[0];

    var $btn = $(e);
    //var frm = $(frmCreate);
    var frm = $(frmCreate),
        formData = new FormData(),
        formParams = frm.serializeArray();
    var isHasFile = false;
    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            isHasFile = true;
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        //Nếu lưu hình bên domain API thì truyền tham số isSaveImageToAPIUrl = true
        if (isSaveImageToAPIUrl && isHasFile) {
            //TODO: Viết ajax gọi thẳng đến API để lưu hình
            RedirectSaveImageToAPI(formData);
        }
        loading2();
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Create",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success == true) {
                    if (isContinue == true) {
                        //redirect to edit form
                        if (isToEditMode) {
                            window.location.href = "/" + controller + "/Edit/" + jsonData.Id;
                        } else {
                            if (isToViewMode) {
                                window.location.href = "/" + controller + "/View/" + jsonData.Id;
                            }
                            //stay and add new item
                            else {
                                frm[0].reset();
                                //reset select2 dropdownlist
                                $(frmCreate + ' select').val($(frmCreate + ' select option:first-child').val()).trigger("change");

                                if (jsonData.Data != null) {

                                    //Force Logout
                                    //[Châu][08/08/2019]
                                    if (jsonData.Data == "FORCE_LOGOUT") {
                                        window.location.href = "/Permission/Auth/Login";
                                    }
                                    alertPopup(true, jsonData.Data);
                                }
                            }
                        }

                    }
                    else {
                        //Force Logout
                        //[Châu][08/08/2019]
                        if (jsonData.Data == "FORCE_LOGOUT") {
                            window.location.href = "/Permission/Auth/Login";
                        }
                        if (jsonData.RedirectUrl) {
                            if (jsonData.RedirectUrl.indexOf("?") == -1) {
                                window.location.href = jsonData.RedirectUrl + "?message=" + jsonData.Data;
                            }
                            else {
                                window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                            }
                        }
                        else {
                            window.location.href = "/" + controller + "?message=" + jsonData.Data;
                        }
                    }
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
        $btn.button('reset');
    }
}
ISD.SaveDataWithPopup = function (action, frmCreate, e, popup, notClosePopup, callbackFunc) {
    var $btn = $(e);
    $btn.button('loading');

    var frm = $(frmCreate),
        formData = new FormData(),
        formParams = frm.serializeArray();
    var callBackFunction = $btn.data("add-success-call-back");
    //console.log(callBackFunction);

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            isHasFile = true;
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "POST",
            url: action,
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success == true) {
                    //Force Logout
                    if (jsonData.Data == "FORCE_LOGOUT") {
                        window.location.href = "/Permission/Auth/Login";
                    }
                    //close popup
                    if (popup) {
                        if (notClosePopup != true) {
                            $(popup).modal("hide");
                        }
                    }
                    //call back
                    if (callbackFunc) {
                        window[callbackFunc]();
                        return false;
                    }
                    if (callBackFunction) {
                        window[callBackFunction]();
                        return false;
                    }
                    //window.location.href = "/" + controller + "?message=" + jsonData.Data;
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        //alertPopup(false, jsonData.Data);

                        $(popup + " .divPopupMessage .alert-message").html("");
                        ISD.setMessage(popup + " .divPopupMessage", jsonData.Data);
                        $(popup + " .divPopupMessage").show();
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                console.log(xhr.responseText);
                //alertPopup(false, xhr.responseText);

                $(popup + " .divPopupMessage .alert-message").html("");
                ISD.setMessage(popup + " .divPopupMessage", xhr.responseText);
                $(popup + " .divPopupMessage").show();
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            $(popup + " .divPopupMessage .alert-message").html("");
            ISD.setMessage(popup + " .divPopupMessage", arr);
            $(popup + " .divPopupMessage").show();
        }
        $btn.button('reset');
    }
}

ISD.CreateInitial = function (controller, isSaveImageToAPIUrl) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        ISD.SaveData(controller, "#frmCreate", isContinue, this, isSaveImageToAPIUrl);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        ISD.SaveData(controller, "#frmCreate", isContinue, this, isSaveImageToAPIUrl);
    });

    $(document).on("click", "#btn-save-edit", function (e) {
        var isContinue = true;
        var isToEditMode = true;
        ISD.SaveData(controller, "#frmCreate", isContinue, this, isSaveImageToAPIUrl, isToEditMode);
    });

    $(document).on("click", "#btn-save-view", function (e) {
        var isContinue = true;
        var isToEditMode = false;
        var isViewMode = true;
        ISD.SaveData(controller, "#frmCreate", isContinue, this, isSaveImageToAPIUrl, isToEditMode, isViewMode);
    });
}

//edit: Edit Data
ISD.EditData = function (controller, frmEdit, isContinue, e, isSaveImageToAPIUrl) {
    controller = controller.split('?')[0];

    var $btn = $(e);
    //var frm = $(frmEdit);
    var frm = $(frmEdit),
        formData = new FormData(),
        formParams = frm.serializeArray();

    var isHasFile = false;
    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            if ($(tag)[0].files.length > 0) {
                isHasFile = true;
                $.each($(tag)[0].files, function (i, file) {
                    formData.append(tag.name, file);
                });
            }
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        //Nếu lưu hình bên domain API thì truyền tham số isSaveImageToAPIUrl = true
        if (isSaveImageToAPIUrl && isHasFile) {
            //TODO: Viết ajax gọi thẳng đến API để lưu hình
            RedirectSaveImageToAPI(formData);
        }

        $.ajax({
            type: "POST",
            url: "/" + controller + "/Edit",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success == true) {
                    if (isContinue == true) {
                        if (jsonData.Data != null) {
                            if (jsonData.RedirectEditUrl) {
                                window.location.href = jsonData.RedirectEditUrl + "&message=" + jsonData.Data;
                            }
                            else {
                                alertPopup(true, jsonData.Data);
                            }
                        }
                    }
                    else {
                        if (jsonData.RedirectUrl) {
                            if (jsonData.RedirectUrl.indexOf("?") == -1) {
                                window.location.href = jsonData.RedirectUrl + "?message=" + jsonData.Data;
                            }
                            else {
                                window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                            }
                        }
                        else {
                            window.location.href = "/" + controller + "?message=" + jsonData.Data;
                        }
                    }
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
        $btn.button('reset');
    }
}

ISD.EditInitial = function (controller, isSaveImageToAPIUrl) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        ISD.EditData(controller, "#frmEdit", isContinue, this, isSaveImageToAPIUrl);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        ISD.EditData(controller, "#frmEdit", isContinue, this, isSaveImageToAPIUrl);
    });
}

ISD.SaveDataWithUrl = function (action, frmEdit, e, callbackFunc) {
    var $btn = $(e);
    $btn.button('loading');

    var frm = $(frmEdit),
        formData = new FormData(),
        formParams = frm.serializeArray();

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            isHasFile = true;
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "POST",
            url: action,
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success == true) {
                    //Force Logout
                    if (jsonData.Data == "FORCE_LOGOUT") {
                        window.location.href = "/Permission/Auth/Login";
                    }
                    //call back
                    if (callbackFunc) {
                        window[callbackFunc]();
                        return false;
                    }
                    if (jsonData.RedirectUrl) {
                        if (jsonData.RedirectUrl.indexOf("?") == -1) {
                            window.location.href = jsonData.RedirectUrl + "?message=" + jsonData.Data;
                        }
                        else {
                            window.location.href = jsonData.RedirectUrl + "&message=" + jsonData.Data;
                        }
                    }
                    else {
                        window.location.href = "/" + controller + "?message=" + jsonData.Data;
                    }
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                console.log(xhr.responseText);
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
        $btn.button('reset');
    }
}

//change password
ISD.ChangePasswordInitial = function (controller) {
    $(document).on("click", "#btn-save", function () {
        ISD.ChangePassword(controller, "#frmChangePassword", this);
    });
}

ISD.ChangePassword = function (controller, frmChangePassword, e) {
    var $btn = $(e);

    var frm = $(frmChangePassword);
    if (frm.valid()) {
        $.ajax({
            type: "POST",
            url: "/" + controller + "/ChangePassword",
            data: frm.serialize() + "&UserName=" + $("#UserName").val(),
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success == true) {
                    $('input,select,textarea').not('[readonly],[disabled],:button').val('');
                    alertPopup(true, jsonData.Data);
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        //show error invalid
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
        $btn.button('reset');
    }
}

//pagging: Datatable
ISD.Pagging = function (isScrollX, pageLength) {
    if (!pageLength) {
        pageLength = 10;
    }
    $('.dataTable:not(.dataTableServerSide)').DataTable({
        pageLength: pageLength,
        paging: true,
        scrollX: isScrollX === true ? true : false,
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
            //"decimal": ".",
            //"thousands": ","
        },
        //"decimal": ".",
        //"thousands": ",",
        //"bLengthChange": false,
        //"bInfo": true,
        //"bPaginate" : false,
        "sDom": '<"top"flp>rt<"bottom"i><"clear">',
    });
}

ISD.bold = function (html) {
    return "<b>" + html + "</b>";
};

//search button event
ISD.SearchDefault = function (controller, isScrollX) {
    var $btn = $("#btn-search");
    $btn.button('loading');

    $.ajax({
        type: "POST",
        url: "/" + controller + "/_Search",
        data: $("#frmSearch").serializeArray(),
        success: function (xhr, status, error) {
            $btn.button('reset');
            if (xhr.Code == 500) {
                //error
                alertPopup(false, xhr.Data);
            } else {
                //success
                $("#divSearchResult").html(xhr);
                ISD.Pagging(isScrollX);
            }

            $(document).on("click", ".btn-edit", function (e) {
                if ($(this).is('[disabled=disabled]')) {
                    e.preventDefault();
                    return false;
                }
            });
        },
        error: function (xhr, status, error) {
            $btn.button('reset');
            alertPopup(false, xhr.responseText);
        }
    });
}

/**
 * Tự động ấn nút search, trigger submit là nút search
 */
ISD.SearchInitial = function (controller, isScrollX, isNotTrigger) {
    //set btn-search event
    $("#btn-search").click(function () {
        ISD.SearchDefault(controller, isScrollX);
    });

    //click btn-search button at first time
    if (!isNotTrigger) {
        $("#btn-search").trigger("click");
    }

    //set default form submit => click btn-search button
    $("#frmSearch").submit(function (e) {
        e.preventDefault();
        $("#btn-search").trigger("click");
    });

    //delete button
    ISD.Delete();

    //cancel button
    ISD.Cancel();

    //import button
    ISD.UploadFile(controller);
    ISD.ImportModalHideHandler();
}
ISD.SearchInitialWithNoLoadFirstTime = function (controller) {
    //set btn-search event
    $("#btn-search").click(function () {
        ISD.SearchDefault(controller);
    });
    //set default form submit => click btn-search button
    $("#frmSearch").submit(function (e) {
        e.preventDefault();
        $("#btn-search").trigger("click");
    });

    //delete button
    ISD.Delete();

    //import button
    ISD.UploadFile(controller);
    ISD.ImportModalHideHandler();
}


/**
 * Setup nút Delete là hiển thị modal xác nhận
 * tagetControl: "#Id" id html element chứa nút delete
 * callBackFunction: "callback()" function Name Call Back khi delete thành công
 */
ISD.Delete = function () {
    //Set event click for button
    $(document).on("click", ".btn-delete", function () {
        var $btn = $(this);

        var controller = $btn.data("current-url");
        var itemName = $btn.data("item-name");
        var id = $btn.data("id");
        var callBackFunction = $btn.data("delete-success-call-back");

        $("#divDeletePopup").modal("show");
        //set title
        $("#divDeletePopup .modal-title .item-name").html(itemName);
        //set text
        var text = $("#divDeletePopup .alert-message").html();
        //Replace new text
        text = text.replace(/"([^"]*)"/g, '"' + itemName + '"');
        text = String.format(text, itemName);
        //Show new text
        $("#divDeletePopup .alert-message").html(text);

        //get id, controller
        $("#divDeletePopup #id").val(id);
        $("#divDeletePopup #controller").val(controller);

        if (callBackFunction) {
            $("#divDeletePopup #btn-confirm-delete").data("delete-success-call-back", callBackFunction);
        }
    });

    //click button confirm
    $(document).on("click", "#btn-confirm-delete", function () {
        var $btn = $(".btn-delete");

        var controller = $('form[id="frmConfirm"] #controller').val();
        //Attribute delete-success-call-back:
        var successfunction = $(this).data("delete-success-call-back");
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Delete",
            data: $('form[id="frmConfirm"]').serialize(),
            success: function (jsonData) {
                $btn.button('reset');
                $("#divDeletePopup").modal("hide");
                if (jsonData.Success == true) {
                    console.log(successfunction);
                    //Gọi function này khi xóa thành công
                    if (successfunction) {
                        window[successfunction]();
                    } else {
                        //mặc định trigger button search, thông báo thành công
                        if ($("#btn-search").length > 0) {
                            $("#btn-search").trigger("click");
                        }
                        else {
                            $(".btn-search").trigger("click");
                        }
                        alertPopup(true, jsonData.Data);
                    }
                }
                else {
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                $("#divDeletePopup").modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    });

    //click button cancel
    $(document).on("click", "#btn-cancel-delete", function () {
        var $btn = $(".btn-delete");

        $btn.button('reset');
        $("#divDeletePopup").modal("hide");
    });


    //click outside popup
    $('#divDeletePopup').on('hidden.bs.modal', function () {
        var $btn = $(".btn-delete");

        $btn.button('reset');
        $("#divDeletePopup").modal("hide");
    });

    //click button cancel
    $(document).on("click", "#btn-cancel-convert", function () {
        var $btn = $(".btn-convert");

        $btn.button('reset');
        $("#divConvertPopup").modal("hide");
    });


};

ISD.Cancel = function () {
    //Set event click for button
    $(document).on("click", ".btn-cancel", function () {
        var $btn = $(this);

        var controller = $btn.data("current-url");
        var itemName = $btn.data("item-name");
        var id = $btn.data("id");
        var callBackFunction = $btn.data("cancel-success-call-back");
        $("#divCancelPopup #CancelReason").val("");
        $("#divCancelPopup #CancelType").trigger("change");
        $("#divCancelPopup").modal("show");
        //set title
        $("#divCancelPopup .modal-title .item-name").html(itemName);
        //set text
        var text = $("#divCancelPopup .alert-message").html();
        //Replace new text
        text = text.replace(/"([^"]*)"/g, '"' + itemName + '"');
        text = String.format(text, itemName);
        //Show new text
        $("#divCancelPopup .alert-message").html(text);

        //get id, controller
        $("#divCancelPopup #id").val(id);
        $("#divCancelPopup #controller").val(controller);

        if (callBackFunction) {
            $("#divCancelPopup #btn-confirm-cancel").data("cancel-success-call-back", callBackFunction);
        }
    });

    //click button confirm
    $(document).on("click", "#btn-confirm-cancel", function () {
        var $btn = $(".btn-cancel");

        var controller = $('form[id="frmCancelConfirm"] #controller').val();
        //Attribute delete-success-call-back:
        var successfunction = $(this).data("cancel-success-call-back");

        $.ajax({
            type: "POST",
            url: "/" + controller + "/Cancel",
            data: $('form[id="frmCancelConfirm"]').serialize(),
            success: function (jsonData) {
                $btn.button('reset');

                if (jsonData.Success === true) {
                    $("#divCancelPopup").modal("hide");
                    console.log(successfunction);
                    //Gọi function này khi hủy thành công
                    if (successfunction) {
                        window[successfunction]();
                    } else {
                        //mặc định trigger button search, thông báo thành công
                        if ($("#btn-search").length > 0) {
                            $("#btn-search").trigger("click");
                        }
                        else {
                            $(".btn-search").trigger("click");
                        }
                        alertPopup(true, jsonData.Data);
                    }
                }
                else {
                    if (jsonData.Data !== null && jsonData.Data !== "") {
                        setModalMessage("#divCancelPopup #divModalAlertWarning", jsonData.Data);
                        setTimeout(function () {
                            $('#divCancelPopup #divModalAlertWarning').show();
                        }, 500);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                $("#divCancelPopup").modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    });

    //click outside popup
    $('#divCancelPopup').on('hidden.bs.modal', function () {
        var $btn = $(".btn-cancel");

        $btn.button('reset');
        $("#divCancelPopup").modal("hide");
    });
};

//Convert
ISD.Convert = function () {
    $(document).on('click', '#btn-confirm-convert', function () {
        var $btn = $(".btn-convert");

        var controller = $('form[id="frmConvertConfirm"] #controller').val();

        $btn.button('loading');
        var ProfileId = $('input[name="ProfileId"]').val();
        loading2();
        $.ajax({
            type: "POST",
            url: "/" + controller + "/ConvertToOpportunity",
            data: {
                ProfileId: ProfileId
            },
            success: function (jsonData) {
                if (jsonData.Success) {
                    if (jsonData.Data) {
                        if (jsonData.RedirectEditUrl) {
                            window.location.href = jsonData.RedirectEditUrl + "&message=" + jsonData.Data;
                        }
                        else {
                            alertPopup(true, jsonData.Data);
                        }
                    }
                }
                else {
                    $btn.button('reset');
                    if (jsonData.Data != null && jsonData.Data != "") {
                        alertPopup(false, jsonData.Data);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertPopup(false, xhr.responseText);
            }
        });
    });
    $(document).on('click', '#btn-convert', function () {
        var $btn = $(this);
        var controller = $btn.data("current-url");
        var itemName = $btn.data("item-name");
        var id = $btn.data("id");
        var callBackFunction = $btn.data("cancel-success-call-back");
        $("#divConvertPopup").modal("show");
        //set title
        $("#divConvertPopup .modal-title .item-name").html(itemName);
        //set text
        var text = $("#divConvertPopup .alert-message").html();
        //Replace new text
        text = text.replace(/"([^"]*)"/g, '"' + itemName + '"');
        text = String.format(text, itemName);
        //Show new text
        $("#divConvertPopup .alert-message").html(text);

        //get id, controller
        $("#divConvertPopup #id").val(id);
        $("#divConvertPopup #controller").val(controller);


        if (callBackFunction) {
            $("#divConvertPopup #btn-confirm-convert").data("cancel-success-call-back", callBackFunction);
        }
    });
    $('#divConvertPopup').on('hidden.bs.modal', function () {
        var $btn = $(".btn-convert");

        $btn.button('reset');
        $("#divConvertPopup").modal("hide");
    });
};


//format string
String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
}

ISD.DateTimeInitial = function () {
    $(document).on("change", ".select-date-time", function () {
        var input = $("input[name='" + $(this).data("id") + "']");
        var mydate = $("#dateField" + $(this).data("id")).val();
        var mytime = $("#timeField" + $(this).data("id")).val();
        var mystring = mydate;
        if (mytime) {
            mystring = mydate + 'T' + mytime;
        }
        input.val(mystring);
    });
}
//get message from current url
ISD.GetQueryString = function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

//show message on Index
ISD.ShowMessage = function (url) {
    var text = ISD.GetQueryString("message", url);
    if (text != null) {
        alertPopup(true, text);

        //split the message parameter on url
        var result = window.location.href.split("?message")[0];
        if (result.indexOf("message") != -1) {
            result = window.location.href.split("&message")[0];
        }
        history.pushState(null, '', result);
    }
}

//typeahead sidebar search
ISD.SidebarSearch = (function () {
    var itemTemplate = function (data) {
        var path = function () {
            if (data.grandParent) {
                return $("<p/>").text([data.grandParent, data.parent].join(" > ")).html();
            }
            else {
                return data.parent;
            }
        };
        var result = '<div id="user-selection">' +
            "<h5>" + data.title + "</h5>";
        if (path()) {
            result = result + path();
        }
        result = result + "</div>";
        return result;
    };

    var substringMatcher = function (enumerate) {
        var byRateAndTitle = function (a, b) {
            if (a.rate < b.rate)
                return 1;
            if (a.rate > b.rate)
                return -1;
            if (a.title < b.title)
                return -1;
            if (a.title > b.title)
                return 1;
            return 0;
        };

        return function findMatches(q, cb) {

            var matches = [];

            var substrRegex = new RegExp(q, "i");

            enumerate(function (item) {

                var rate = 0;
                var missKeyword = false;

                if (substrRegex.test(item.title)) {
                    rate += 10;
                }
                else if (item.node && substrRegex.test(item.node)) {
                    rate += 5;
                }
                else if (substrRegex.test(item.root)) {
                    rate += 1;
                } else {
                    missKeyword = true;
                }

                item.rate = rate;

                if (!missKeyword) {
                    matches.push(item);
                }
            });


            matches.sort(byRateAndTitle);
            cb(matches);
        };
    };

    return {
        init: function () {
            ISD.Navigation.initOnce();

            var $input = $(".isd-search-box");
            $input.blur(function (e) { e.preventDefault(); e.stopPropagation(); });
            $input.typeahead({ minLength: 2, highlight: true, hint: false },
                {
                    name: "pages",
                    displayKey: "name",
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            "NO RESULTS",
                            "</div>"
                        ].join("\n"),
                        suggestion: itemTemplate
                    },
                    source: substringMatcher(ISD.Navigation.enumerate),
                    limit: 10
                });

            var navigateTo = function (item) {
                $input.typeahead("val", "");
                ISD.Navigation.open(item.link);
            };

            $input.on("typeahead:selected", function (e, item) {
                navigateTo(item);
            });
        }
    };
})();

ISD.Navigation = (function () {
    var buildMap = function () {
        var map = {};

        var linkElements = $("a.menu-item-link");

        linkElements.each(function () {
            var parents = $(this).parentsUntil(".sidebar-menu");
            var href;
            var title;
            var parent;
            var grandParent;
            switch (parents.length) {
                // items in level one
                case 1:
                    {
                        href = $(parents).find("a").attr("href");
                        title = $(parents).find("a").find("span").html();
                        map[href] = { title: title, link: href, parent: null, grandParent: null };

                        break;
                    }
                // items in level two, these items have parent but have not grand parent
                case 3:
                    {
                        href = $(parents).eq(0).find("a").attr("href");
                        title = $(parents).eq(0).find("a").find("span").html();
                        parent = $(parents).eq(2).find("a").find("span").html();
                        map[href] = { title: title, link: href, parent: parent, grandParent: null };

                        break;
                    }
                // items in level three, these items have both parent and grand parent
                case 5:
                    {
                        href = $(parents).eq(0).find("a").attr("href");
                        title = $(parents).eq(0).find("a").find("span").html();
                        parent = $(parents).eq(2).find("a").find("span").html();
                        grandParent = $(parents).eq(4).find("a").find("span").html();
                        map[href] = { title: title, link: href, parent: parent, grandParent: grandParent };
                        break;
                    }
                default: break;
            }
        });

        return map;
    };

    var map;

    var init = function () {
        map = buildMap();
    };
    var events = {};
    return {
        enumerate: function (callback) {
            for (var url in map) {
                var node = map[url];
                callback.call(node, node);
            }
        },
        open: function (url) {
            if (events["open"]) {
                var event = $.Event("open", { url: url });
                events["open"].fire(event);
                if (event.isDefaultPrevented())
                    return;
            }
            window.location.href = url;
        },

        initOnce: function () {
            if (!map)
                init();
        },
        init: init
    };
})();

//progressbar upload file
ISD.ProgressBar = ISD.ProgressBar || (function () {
    var pleaseWaitDiv = $(".progress");
    return {
        showPleaseWait: function () {
            pleaseWaitDiv.show();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.hide();
        },

    };
})();

/**
 *  Setup nút import data
 */
ISD.UploadFile = function (controller) {
    $(document).on("click", "#btn-importExcel", function () {
        var $btn = $(this);
        $btn.button('loading');

        //alert
        $(".modal-alert-message").html("");
        $(".modalAlert").hide();

        ISD.ProgressBar.showPleaseWait(); //show dialog
        var file = document.getElementById('importexcelfile').files[0];
        var formData = new FormData();
        formData.append("importexcelfile", file);

        //#region using ajax XMLHttpRequest
        //ajax = new XMLHttpRequest();
        //ajax.upload.addEventListener("progress", progressHandler, false);
        //ajax.addEventListener("load", completeHandler, false);
        //ajax.open("POST", "/ReportTest/ImportExcelMTDynamic");
        //ajax.send(formData);
        //#endregion using ajax XMLHttpRequest

        $.ajax({
            type: "POST",
            url: "/" + controller + "/Import",
            data: formData,
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', progressHandler, false);
                    myXhr.addEventListener("load", completeHandler, false);
                }
                return myXhr;
            },
            cache: false,
            contentType: false,
            processData: false,
            success: function (jsonData) {
                $btn.button('reset');
                if (jsonData.Success === true) {
                    //formData[0].reset();
                    if (jsonData.Data) {
                        //alertModalPopup(true, jsonData.Data);
                        setModalMessage("#importexcel-window #divModalAlertSuccess", jsonData.Data);
                        setTimeout(function () {
                            $('#importexcel-window #divModalAlertSuccess').show();
                        }, 500);
                        setTimeout(function () {
                            $('#importexcel-window #divModalAlertSuccess').hide();
                        }, 3000);
                    }
                    setTimeout(function () {
                        $("#importexcelfile").val("");
                    }, 3000);
                }
                else {
                    if (jsonData.Data) {
                        //alertModalPopup(false, jsonData.Data);
                        setModalMessage("#importexcel-window #divModalAlertWarning", jsonData.Data);
                        setTimeout(function () {
                            $('#importexcel-window #divModalAlertWarning').show();
                        }, 500);
                    }
                }
            },
            error: function (xhr, status, error) {
                $btn.button('reset');
                alertModalPopup(false, xhr.responseText);
                setModalMessage("#importexcel-window #divModalAlertWarning", xhr.responseText);
                setTimeout(function () {
                    $('#importexcel-window #divModalAlertWarning').show();
                }, 500);
            }
        });
    });

    function progressHandler(event) {
        if (event.lengthComputable) {
            var percent = Math.round((event.timeStamp / event.total) * 100);
            var loadingPercent = percent + "%";
            $('.progress-bar').width(loadingPercent); //from bootstrap bar class
        }
    }

    function completeHandler() {
        $('.progress-bar').width("100%");
        setTimeout(function () {
            ISD.ProgressBar.hidePleaseWait(); //hide dialog
        }, 3000);
    }
}

//reset input file when modal hide
ISD.ImportModalHideHandler = function () {
    $('#importexcel-window').on('hidden.bs.modal', function (e) {
        document.getElementById("importexcelfile").value = "";

        //alert
        $(".modal-alert-message").html("");
        $(".modalAlert").hide();

        $.ajax({
            type: "GET",
            url: "/Permission/Menu/_Search",
            success: function (jsonData) {
                $("#menuTable").html(jsonData);

            },
            error: function (xhr, status, error) {
                alertModalPopup(false, xhr.responseText);
            }
        });
    });
};

ISD.BtnLoading = function ($target, isLoading) {

    //var allButton = $(".btn:not(:disabled)");

    if (isLoading) {
        $target.button('loading'); // Bootstrap 3: Disables the button and changes the button text to "loading..."
        //$target.prop("disabled", loading);

        //if (isDisabledAll) {
        //    allButton.prop("disabled", true);
        //}

    } else {
        $target.button('reset');
        //$target.removeProp("disabled");
        //if (isDisabledAll) {
        //    allButton.removeProp("disabled");
        //}
    }
};

ISD.post = function (postUrl, dataObj, success, error) {
    $.ajax({
        type: "POST",
        url: postUrl,
        data: dataObj,
        success: function (ret) {
            if (success) {
                success(ret);
            }

        },
        error: function (xhr, status, error) {
            if (error) {
                error(xhr, status, error);
            }
        }
    });
}

ISD.setMessage = function (div, message) {
    if (Array.isArray(message)) {
        var arr = [];
        $.each(message, function (i, item) {
            //Code cũ
            //arr[i] = { err: item.ErrorMessage }
            if (item.ErrorMessage != undefined && item.ErrorMessage != "") {
                arr[i] = { err: item.ErrorMessage }
            }
            else {
                arr[i] = { err: item }
            }
            $(div + " .alert-message").append("<li>" + arr[i].err + "</li>");
        });
    }
    else {
        $(div + " .alert-message").html(message);
    }
}

ISD.alertPopup = function (isSuccess, message) {
    if (isSuccess == true) {
        $("#divAlertSuccess .alert-message").html("");
        ISD.setMessage("#divAlertSuccess", message);
        $('.alert-success').show();
        setTimeout(function () {
            $('.alert-success').hide();
        }, 5000)
    }
    else if (isSuccess == false) {
        $("#divAlertWarning .alert-message").html("");
        ISD.setMessage("#divAlertWarning", message);
        $('.alert-warning').show();
    }

    $("html, body").animate({ scrollTop: 0 }, "fast");
}

/**
 * Tạo một modal động
 * @param {any} Tiêu đề
 * @param {any} Nội dung
 * @param {any} Tên của function chạy
 * @param {any} Tên nút confirm
 */
ISD.CreateModal = function (heading, formContent, strSubmitFunc, btnText, radioBtn) {
    var placementId = "dynamicModalContainer";
    var html = '<div id="dynamicModal" class="modal fade in" data-backdrop="static" style="display:none;"><div class="modal-dialog"><div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<a class="close" data-dismiss="modal">×</a>';
    html += '<h4 class="modal-title">' + heading + '</h4>'
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p class="text-center">';
    html += formContent;
    //radio button
    if (radioBtn && radioBtn.length > 0) {
        html += '<div class="radio" style="text-align: center;">';
        radioBtn.forEach(function (item, index) {
            var checked = "";
            if (index == 0) {
                checked = "checked";
            }
            html += '<label style="margin-right: 20px;">';
            html += '<input type="radio" name="' + item.Name + '"' + checked + ' value="' + item.Value + '">' + item.Label + '';
            html += '</label>';
        });
        html += '</div>';
    }
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-default" data-dismiss="modal">';
    html += 'Không';
    html += '</span>'; // close button
    if (btnText != '') {
        html += '<span class="btn btn-primary"';
        html += ' onClick="' + strSubmitFunc + '">' + btnText;
        html += '</span>';
    }
    html += '</div></div></div>';  // footer
    html += '</div>';  // modalWindow
    $("#" + placementId).html(html);
    $("#dynamicModal").modal("show");
}

ISD.CreateModal2 = function (heading, formContent, strSubmitFunc, btnText, radioBtn) {
    var placementId = "dynamicModalContainer";
    var html = '<div id="dynamicModal" class="modal fade in" data-backdrop="static" style="display:none;"><div class="modal-dialog"><div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<a class="close" data-dismiss="modal">×</a>';
    html += '<h4 class="modal-title">' + heading + '</h4>'
    html += '</div>';
    html += '<div class="modal-body">';
    html += formContent;
    //radio button
    if (radioBtn && radioBtn.length > 0) {
        html += '<div class="radio" style="text-align: center;">';
        radioBtn.forEach(function (item, index) {
            var checked = "";
            if (index == 0) {
                checked = "checked";
            }
            html += '<label style="margin-right: 20px;">';
            html += '<input type="radio" name="' + item.Name + '"' + checked + ' value="' + item.Value + '">' + item.Label + '';
            html += '</label>';
        });
        html += '</div>';
    }
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-default" data-dismiss="modal">';
    html += 'Không';
    html += '</span>'; // close button
    if (btnText != '') {
        html += '<span class="btn btn-primary"';
        html += ' onClick="' + strSubmitFunc + '">' + btnText;
        html += '</span>';
    }
    html += '</div></div></div>';  // footer
    html += '</div>';  // modalWindow
    $("#" + placementId).html(html);
    $("#dynamicModal").modal("show");
}

ISD.ClearModal = function () {
    $("#dynamicModal").modal("hide");
};

ISD.CloseModal = function () {
    $("#dynamicModal").modal("hide");
};

$.fn.serializeAnything = function () {

    var toReturn = {};
    var els = $(this).find(':input').get();

    $.each(els, function () {
        if (this.name) {
            if (/select|textarea/i.test(this.nodeName) || /text|date|hidden|password/i.test(this.type)) {
                var val = $(this).val();
                toReturn[this.name] = val;
            } else if (/checkbox/i.test(this.type)) {
                var val = $(this).is(':checked');
                toReturn[this.name] = val;
            }
        }

    });

    return toReturn;
}

function formatCurrency(data) {
    if (data == '' || data == null || data == undefined || data == 0) return '0';
    return data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function getNumberOnly(text, isToFloat) {
    let newText = '';
    const numbers = '-0123456789';
    if (text.length < 1) {
        newText = 0;
    }
    for (let i = 0; i < text.length; i++) {
        // Nếu là số
        if (numbers.indexOf(text[i]) > -1) {
            newText += text[i];
        }
    }
    if (isToFloat) {
        const newTextFloat = parseFloat(newText);
        return newTextFloat;
    }
    return newText;
}

function addValueInObject(object, key, value) {
    var res = {};
    var textObject = JSON.stringify(object);
    if (textObject === '{}') {
        res = JSON.parse('{"' + key + '":"' + value + '"}');
    } else {
        res = JSON.parse('{' + textObject.substring(1, textObject.length - 1) + ',"' + key + '":"' + value + '"}');
    }
    return res;
}

function RedirectSaveImageToAPI(formData) {
    var DomainAPI = $("input[name='DomainAPI']").val();
    $.ajax({
        type: "POST",
        url: DomainAPI + "Sale/UploadImageRedirectToAPI",
        data: formData,
        processData: false,
        contentType: false,
        success: function (jsonData) {
            if (jsonData.Success == false) {
                alertPopup(false, jsonData.Data);
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
}

ISD.ValidationOnModalPopup = function (frmSelector) {
    $(frmSelector).removeData('validator');
    $(frmSelector).removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(frmSelector);
}

ISD.Download = function (url, data, isBlank) {
    // Build a form
    var form = $('<form></form>').attr('action', url).attr('method', 'post');
    if (isBlank) {
        form = $('<form></form>').attr('action', url).attr('method', 'post').attr('target', '_blank');
    }
    // Add the one key/value
    $.each(data, function (k, v) {
        if (isArray(v)) {
            // array
            for (var i in v) {
                //console.log(k + '[' + i + ']' + ' = ' + v[i]);
                form.append($("<input></input>").attr('type', 'hidden').attr('name', k + '[' + i + ']').attr('value', v[i]));
            }
        } else {
            // Do another thing
            //console.log(k + ' = ' + v);
            form.append($("<input></input>").attr('type', 'hidden').attr('name', k).attr('value', v));
        }
    });
    //send request
    form.appendTo('body').submit().remove();
};

// Restricts input for the set of matched elements to the given inputFilter function.
(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));

//select2 search input placeholder
(function ($) {

    var Defaults = $.fn.select2.amd.require('select2/defaults');

    $.extend(Defaults.defaults, {
        searchInputPlaceholder: ''
    });

    var SearchDropdown = $.fn.select2.amd.require('select2/dropdown/search');

    var _renderSearchDropdown = SearchDropdown.prototype.render;

    SearchDropdown.prototype.render = function (decorated) {

        // invoke parent method
        var $rendered = _renderSearchDropdown.apply(this, Array.prototype.slice.apply(arguments));

        this.$search.attr('placeholder', this.options.get('searchInputPlaceholder'));

        return $rendered;
    };

})(window.jQuery);

//auto complete
function Select2_CustomFor_CRM(url, selector, type, type2, isHasValue) {
    $(selector).select2({
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    search: params.term, // search term
                    page: params.page,
                    type: type,
                    type2: type2,
                };
            },
            processResults: function (data) {
                if (!isHasValue) {
                    data.splice(0, 0, { value: "", text: "-- Tất cả --" });
                    return {
                        results: $.map(data, function (obj) {
                            return { id: obj.value, text: obj.text };
                        })
                    };
                }
                
            }
            , minimumInputLength: 0

        }
    });
}
//auto complete
function Select2_CustomFor_CRM_NotAll(url, selector, type, type2, isHasValue) {
    $(selector).select2({
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    search: params.term, // search term
                    page: params.page,
                    type: type,
                    type2: type2,
                };
            },
            processResults: function (data) {
                if (!isHasValue) {
                    //data.splice(0, 0, { value: "", text: "-- Tất cả --" });
                    return {
                        results: $.map(data, function (obj) {
                            return { id: obj.value, text: obj.text };
                        })
                    };
                }
                
            }
            , minimumInputLength: 0

        }
    });
}