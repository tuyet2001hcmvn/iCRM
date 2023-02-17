//Change template load and fill to input searct
$(document).on("change", "#SearchTemplateId", function () {
    var currentSelectId = $(this).val();
    var frmSearchId = $(this).data('formid');

    if (currentSelectId && currentSelectId != "Recently") {
        $(".btn-delete-frmSearch").removeClass("display-none");
        var url = net5apidomain + "api/Utilities/SearchTemplates/" + currentSelectId;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.isSuccess) {
                    var dataJson = res.data.searchContent;
                    //Nếu có dữ liệu thì set vào local storage
                    if (dataJson) {
                        var recentSearch = JSON.parse(dataJson);
                        localStorage.setItem('pageId-' + currentPageId, dataJson);
                        if (frmSearchId && frmSearchId != '' && frmSearchId != undefined) {
                            FillToInput(recentSearch, frmSearchId);
                        } else {
                            FillToInput(recentSearch);
                        }
                    }
                }
                else {
                    alertPopup(false, res.message);
                    console.log(res);
                }
            },
            error: function (res) {
                alertPopup(false, res.responseJSON.message);
                console.log(res);
            }
        });
    } else {
        $(".btn-delete-frmSearch").addClass("display-none");
    }
})

//Delete template search
$(document).on("click", ".btn-delete-frmSearch", function () {
    var currentSelectId = $("#SearchTemplateId").val();
    var url = net5apidomain + "api/Utilities/SearchTemplates/Delete?searchTemplateId=" + currentSelectId;
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                alertPopup(true, res.message);
                LoadTemplateSearch(currentUserId, currentPageId);
            }
            else {
                alertPopup(false, res.message);
                console.log(res);
            }
        },
        error: function (res) {
            alertPopup(false, res.responseJSON.message);
            console.log(res);
        }
    });
})
$(document).on("click", ".btn-save-frmSearch", function () {
    var searchData = GetDataFormSearch();
    $(".divSaveSearchForm #AccountId").val(currentUserId);
    $(".divSaveSearchForm #PageId").val(currentPageId);
    $(".divSaveSearchForm #SearchContent").val(searchData);
    $(".divSaveSearchForm #TemplateName").val("");
    $(".divSaveSearchForm #divSaveSearchForm").modal("show");
})

//Save - Creare new search template
$(document).on("click", "#divSaveSearchForm #btn-save-template", function () {
    var saveToApiUrl = net5apidomain + 'api/Utilities/SearchTemplates';
    SaveDataFormSearch(saveToApiUrl, "#frmSaveSearchForm", this, "#divSaveSearchForm");
})




function LoadTemplateSearch(accountId, pageId, selectedId) {
    var url = net5apidomain + "api/Utilities/SearchTemplates?accountId=" + accountId + "&pageId=" + pageId;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var jsonData = res.data;
                var $searchTemp = $("#SearchTemplateId")
                $searchTemp.html("");
                $searchTemp.append("<option value=''>-- Mặc định --</option>");
                $.each(jsonData, function (index, value) {
                    $searchTemp.append("<option value='" + value.searchTemplateId + "'>" + value.templateName + "</option>");
                });
                $searchTemp.append("<option value='Recently'>-- Gần đây --</option>");
                if (selectedId) {
                    $("#SearchTemplateId").val(selectedId).change();
                }
            }
            else {
                console.log(res);
            }
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveDataFormSearch(saveToApiUrl, form, e, popup) {
    var $btn = $(e);
    $btn.button('loading');
    var frm = $(form),
        formParams = frm.serializeArray();
    var callBackFunction = $btn.data("save-success-call-back");
    if (frm.valid()) {
        var obj = {}
        $.each(formParams, function (i, val) {
            obj[val.name] = val.value;
        });
        $.ajax({
            type: "POST",
            url: saveToApiUrl,
            data: JSON.stringify(obj),
            processData: false,
            contentType: "application/json",
            success: function (res) {
                $btn.button('reset');
                if (res.isSuccess == true) {
                    alertPopup(true, res.message);
                    //Reload template
                    LoadTemplateSearch(res.data.accountId, res.data.pageId, res.data.searchTemplateId);
                    $(popup).modal("hide");
                }
                else {
                    var resObj = JSON.parse(res);
                    alertPopup(false, resObj.Message);
                    $(popup + " .divPopupMessage .alert-message").html("");
                    ISD.setMessage(popup + " .divPopupMessage", jsonData.Data);
                    $(popup + " .divPopupMessage").show();
                }
            },
            error: function (res) {
                $btn.button('reset');
                var resObj = JSON.parse(res);
                alertPopup(false, resObj.Message);
                $(popup + " .divPopupMessage .alert-message").html("");
                ISD.setMessage(popup + " .divPopupMessage", xhr.responseText);
                $(popup + " .divPopupMessage").show();
            }
        });
    } else {
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


function FillToInput(recentSearch, formId) {
    var frmInputList = [];
    if (formId == "" || formId == undefined) {
        formId = "#frmSearch";
    }
    //Get all Input, selector in Form
    $(formId).each(function () {
        frmInputList = $(this).find(":input");
    });
    var elmInputList = [];

    if (frmInputList.length > 0) {
        frmInputList.each(function (index, val) {
            if (val.name != '') {
                elmInputList.push({ tagName: val.localName, attName: val.name, idName: val.id });
            }
        });
    }
    //Set value for typeInput
    $.each(elmInputList, function (index, value) {
        var inputName = value.attName;
        if (inputName.substring(inputName.length - 6) == "IsNull") {
            //For search is null value
            if (recentSearch[inputName] != "") {
                $("input[name='" + inputName + "']").val(recentSearch[inputName]);

                var nameInputView = inputName.substring(0, inputName.length - 6)
                var tagInputView = $("#" + value.idName.substring(0, value.idName.length - 6)).prop("localName");

                if (tagInputView == "input") {
                    var $inputView = $("input[name='" + nameInputView + "']");
                    $inputView.prop("disabled", true)
                    if (recentSearch[inputName] == "true") {
                        $inputView.val("Không có dữ liệu");
                    } else {
                        $inputView.val("Có dữ liệu");
                    }
                } else if (tagInputView == "select") {

                    $("select[name='" + nameInputView + "']").prop("disabled", true);
                }
            }
        } else {

            if (value.tagName == "input") {
                //Set value input type
                $("input[name='" + inputName + "']").val(recentSearch[inputName]);
            } else if (value.tagName == "select" && recentSearch[inputName] && inputName != "SearchTemplateId") {
                //Set value select type => trigger change
                //Nếu là ngày tùy chỉnh mà có value thì không change drop down common date nếu change dữ liệu search ban đầu bị mất
                if (inputName.indexOf('Common') > -1 && recentSearch[inputName] === "Custom") {

                }
                else {
                    $("select[name='" + inputName + "']").val(recentSearch[inputName]).change();
                }

            }

        }
    });
    if (recentSearch.CheckAll) {
        $("#CheckAll").prop('checked', true);
    }
}