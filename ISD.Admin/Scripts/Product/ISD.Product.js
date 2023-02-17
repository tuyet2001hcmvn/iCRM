
var Product = {};
//btn-add-row
Product.AddRow = function (action, list, id) {
    var form = $(".btn-add-row").closest("form").attr('id');

    var result = true;
    $('#' + form + " tfoot#" + id + " input").each(function () {
        var value = $(this).val();
        if (value == "") {
            result = false;
            return false;
        }
    });

    $('#' + form + " tfoot#" + id + " textarea").each(function () {
        var value = $(this).val();
        if (value == "") {
            result = false;
            return false;
        }
    });

    $('#' + form + " tfoot#" + id + " select:not(#StyleId)").each(function () {
        var value = $(this).val();
        if (value == "") {
            result = false;
            return false;
        }
    });

    if (id == "accessory") {
        var price = $("#Price").val();
        if (isNaN(parseInt(price))) {
            alertPopup(false, 'Vui lòng nhập "Giá" là số');
            return false;
        }
    }

    if (result == false) {
        alertPopup(false, 'Vui lòng nhập đầy đủ các thông tin trước khi thêm');
    }
    else {
        var frm = $("#" + form),
            formData = new FormData(),
            formParams = frm.serializeArray();
        if (frm.valid()) {
            $.each(frm.find('input[type="file"]'), function (i, tag) {
                $.each($(tag)[0].files, function (i, file) {
                    formData.append(tag.name, file);
                });
            });

            $.each(formParams, function (i, val) {
                formData.append(val.name, val.value);
            });

            $.ajax({
                url: "/Sale/Product/" + action,
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (htmlData) {
                    $("#uploadTable tfoot input, textarea").val("");
                    $('input[type=file]').val('');
                    //reset select2 dropdownlist
                    $('#' + form + ' tfoot select').val($('#' + form + ' tfoot select option:first-child').val()).trigger("change");

                    $("#" + list).html(htmlData);
                    Product.ColorPickerInitial();
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        }
        else {
            console.log("Jquery Validation Invalid");
        }
    }
}

//btn-del-product: delete row in product detail tables
Product.DeleteRow = function (action, list, STT) {
    var frm = $(".btn-del-product").closest("form").attr('id');

    $.ajax({
        type: "POST",
        url: "/Sale/Product/" + action + "?STT=" + STT,
        data: $("#" + frm).serialize(),
        success: function (data) {
            $("#" + list).html(data);
        }
    });
}

Product.PriceFormat = function () {
    var price = $(".price-currency").text();
    price = price.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    $(".price-currency").text(price);
}

//GetCategoryByBrand
Product.GetCategory = function () {
    $(document).on("change", "#BrandId", function () {
        var BrandId = $(this).val();

        $.ajax({
            type: "POST",
            url: "/Product/GetCategoryByBrand",
            data: {
                BrandId: BrandId
            },
            success: function (jsonData) {
                $("#CategoryId").html("");
                $("#CategoryId").append("<option value=''>-- Tất cả --</option>");
                $.each(jsonData, function (index, value) {
                    $("#CategoryId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });
            }
        });
    });
}

//GetAccessoryByCategory
Product.GetAccessory = function () {
    $(document).on("change", "#AccessoryCategoryId", function () {
        var AccessoryCategoryId = $(this).val();
        if (AccessoryCategoryId == "") {
            $("#AccessoryId").html("");
            $("#AccessoryId").append("<option>-- Tất cả --</option>");
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Product/GetAccessoryByCategory",
                data: {
                    AccessoryCategoryId: AccessoryCategoryId
                },
                success: function (jsonData) {
                    $("#AccessoryId").html("");
                    $.each(jsonData, function (index, value) {
                        $("#AccessoryId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                    });
                }
            });
        }
    });
}

//ChangeColor
Product.ChangeColor = function () {
    $(document).on("change", "#MainColorId", function () {
        var MainColorId = $(this).val();
        if (MainColorId == undefined) {
            $("[aria-labelledby='select2-MainColorId-container']").css("background-color", "#fff");
        }
        else {
            $("[aria-labelledby='select2-MainColorId-container']").css("background-color", MainColorId);
        }
    });
}

//ColorStyleImage required type .png
function checkUploadFileValid(fileName, fileType) {
    var file = $(fileName).val();
    if (file != "" && file.indexOf(fileType) == -1) {
        return false;
    }
    else {
        return true;
    }
}

$(document).on("change", "#ColorStyleImage", function () {
    if (checkUploadFileValid(this, ".png") == true || checkUploadFileValid(this, ".jpg") == true) {
        $("#btn-save").prop("disabled", false);
        $("#btn-save-continue").prop("disabled", false);
    }
    else {
        alertPopup(false, "Vui lòng chọn ảnh sản phẩm kiểu .png hoặc .jpg!");
        $("#btn-save").prop("disabled", true);
        $("#btn-save-continue").prop("disabled", true);
    }
});

function setModalMessage(div, message) {
    if (Array.isArray(message)) {
        var arr = [];
        $.each(message, function (i, item) {
            arr[i] = { err: item }
            $(div + " .modal-alert-message").append("<li>" + arr[i].err + "</li>");
        });
    }
    else {
        $(div + " .modal-alert-message").html(message);
    }
}
function alertModalPopup(type, message) {
    if (type == true) {
        setModalMessage("#divModalAlertSuccess", message);
        setTimeout(function () {
            $('#divModalAlertSuccess').show();
        }, 500)
        setTimeout(function () {
            $('#divModalAlertSuccess').hide();
        }, 3000)
    }
    else if (type == false) {
        setModalMessage("#divModalAlertWarning", message);
        setTimeout(function () {
            $('#divModalAlertWarning').show();
        }, 500)
    }
}

//show modal popup image detail
$(document).on("click", ".btn-edit-image", function () {
    var ColorProductId = $(this).data("id");

    $.ajax({
        url: "/Sale/Product/ImageDetails",
        data: {
            ColorProductId: ColorProductId
        },
        type: 'POST',
        success: function (jsonData) {
            if (jsonData.Success == true) {
                $("#divImageDetails .modal-title").html("");
                $("#divImageDetails .product-color").html("");

                $("#divImageDetails .modal-title").append(jsonData.Data.ProductName);
                $("#divImageDetails .product-color").append(jsonData.Data.ColorName);

                $("#divImageDetails").modal("show");
                $.ajax({
                    url: "/Sale/Product/_ImageDetailsInner",
                    data: {
                        ColorProductId: ColorProductId
                    },
                    type: "POST",
                    success: function (data) {
                        $("#detailColorList").html(data);
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//set image is default
$(document).on("click", ".btn-set-first", function () {
    var ColorProductId = $(this).data("color");
    var ImageId = $(this).data("id");

    $.ajax({
        url: "/Sale/Product/ChangeDefaultImage",
        data: {
            ColorProductId: ColorProductId,
            ImageId: ImageId
        },
        type: 'POST',
        success: function (jsonData) {
            $("#detailColorList").html("");
            $("#detailColorList").html(jsonData);
        },
        error: function (xhr, status, error) {
            alertModalPopup(false, xhr.responseText);
        }
    });
});

//add new image in modal popup
Product.PopupAddImage = function () {

    $(document).on("click", ".btn-add-image", function () {
        var result = true;
        $("#frmUploadImage tfoot#detailColor input").each(function () {
            var value = $(this).val();
            if (value == "") {
                result = false;
                return false;
            }
        });

        if (result == false) {
            alertPopup(false, 'Vui lòng chọn hình ảnh trước khi thêm');
        }
        else {
            var frm = $("#frmUploadImage"),
                formData = new FormData(),
                formParams = frm.serializeArray();
            if (frm.valid()) {
                $.each(frm.find('input[type="file"]'), function (i, tag) {
                    $.each($(tag)[0].files, function (i, file) {
                        formData.append(tag.name, file);
                    });
                });

                $.each(formParams, function (i, val) {
                    formData.append(val.name, val.value);
                });
                formData.append("ProductId", $("input[name=ProductId]").val());
                $.ajax({
                    url: "/Sale/Product/InsertImage",
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: 'POST',
                    success: function (htmlData) {
                        $('input[type=file]').val('');
                        $("#detailColorList").html(htmlData);
                    },
                    error: function (xhr, status, error) {
                        alertModalPopup(false, xhr.responseText);
                    }
                });
            }
            else {
                console.log("Jquery Validation Invalid");
            }
        }
    });
}

//delete image
$(document).on("click", ".btn-del-image", function () {
    var ColorProductId = $(this).data("color");
    var ImageId = $(this).data("id");

    $.ajax({
        url: "/Sale/Product/DeleteImage",
        data: {
            ColorProductId: ColorProductId,
            ImageId: ImageId
        },
        type: 'POST',
        success: function (jsonData) {
            $("#detailColorList").html("");
            $("#detailColorList").html(jsonData);
        },
        error: function (xhr, status, error) {
            alertModalPopup(false, xhr.responseText);
        }
    });
});

//load partial color list after hide modal popup image
$(document).on('hide.bs.modal', '#divImageDetails', function () {
    var ProductId = $("input[name=ProductId]").val();

    $.ajax({
        url: "/Sale/Product/_ProductColor",
        data: {
            ProductId: ProductId,
            Mode: "2"
        },
        type: 'POST',
        success: function (jsonData) {
            $("#ProductColorList").html(jsonData);
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

Product.ColorPickerInitial = function () {
    $('.myColorPicker1').colorpicker({
        format: 'rgb',
        customClass: 'colorpicker-1',
        sliders: {
            saturation: {
                maxLeft: 200,
                maxTop: 200
            },
            hue: {
                maxTop: 200
            },
            alpha: {
                maxTop: 200
            }
        }
    });

    $('.myColorPicker2').colorpicker({
        format: 'rgb',
        customClass: 'colorpicker-2',
        sliders: {
            saturation: {
                maxLeft: 200,
                maxTop: 200
            },
            hue: {
                maxTop: 200
            },
            alpha: {
                maxTop: 200
            }
        }
    });

    $('.myColorPicker3').colorpicker({
        format: 'rgb',
        customClass: 'colorpicker-3',
        sliders: {
            saturation: {
                maxLeft: 200,
                maxTop: 200
            },
            hue: {
                maxTop: 200
            },
            alpha: {
                maxTop: 200
            }
        }
    });
}

$(document).ready(function () {
    Product.ColorPickerInitial();
});




