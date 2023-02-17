// isHasRadio:
//   true: Hiển thị radio button Phụ tùng/Phụ kiện và Công việc
//   false: Không hiển thị
// SearchAccessoryByCategoryType:
//   0: Phụ tùng/Phụ kiện
//   1: Công việc
// SaleOrg: Chi nhánh
// isPopup:
//   true: Hiển thị popup Tồn kho
//   false: Không hiển thị
// prefix: để phân biệt các popup nếu có nhiều popup
// componentContainerSelector
//   <div id="resultAcessoryComponent"></div> => componentContainerSelector = #resultAcessoryComponent
// callback
// current button
// isSale: lấy theo giá bán hàng
// isPromo: lấy theo mã khuyến mãi (M005)

function OpenModalAccessorySearch(isHasRadio, SearchAccessoryByCategoryType, SaleOrg, isPopup, prefix, componentContainerSelector, callback, btnCaller, isSale, isPromo) {
    if (btnCaller) {
        var $this = $(btnCaller);
        $this.button('loading');
    }

    var data = {
        isHasRadio: isHasRadio,
        SearchAccessoryByCategoryType: SearchAccessoryByCategoryType,
        SaleOrg: SaleOrg,
        isPopup: isPopup,
        prefix: prefix,
        isSale: isSale,
        isPromo: isPromo
    }

    $.ajax({
        type: "POST",
        url: "/Sale/Accessory/_AccessorySearch",
        data: data,
        success: function (xhr, status, error) {
            if (btnCaller) {
                var $this = $(btnCaller);
                $this.button('reset');
            }
            $(componentContainerSelector).html(xhr);
            $("#divAccessorySearch_" + prefix).modal("show");
            isHasRadioFunction(isHasRadio, SearchAccessoryByCategoryType);

            if (callback) {
                callback();
            }
        },
        error: function (xhr, status, error) {
            if (btnCaller) {
                var $this = $(btnCaller);
                $this.button('reset');
            }
            alertPopup(false, xhr.responseText);
        }
    });

    //var isLoadedPrefix = window["isLoaded_" + prefix];

    //if (!isLoadedPrefix) {

    //    var data = {
    //        isHasRadio: isHasRadio,
    //        SearchAccessoryByCategoryType: SearchAccessoryByCategoryType,
    //        SaleOrg: SaleOrg,
    //        isPopup: isPopup,
    //        prefix: prefix,
    //        isSale: isSale,
    //        isPromo: isPromo
    //    }

    //    $.ajax({
    //        type: "POST",
    //        url: "/Sale/Accessory/_AccessorySearch",
    //        data: data,
    //        success: function (xhr, status, error) {
    //            if (btnCaller) {
    //                var $this = $(btnCaller);
    //                $this.button('reset');
    //            }
    //            $(componentContainerSelector).html(xhr);
    //            $("#divAccessorySearch_" + prefix).modal("show");
    //            isHasRadioFunction(isHasRadio, SearchAccessoryByCategoryType);

    //            if (callback) {
    //                callback();
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            if (btnCaller) {
    //                var $this = $(btnCaller);
    //                $this.button('reset');
    //            }
    //            alertPopup(false, xhr.responseText);
    //        }
    //    });
    //} else {
    //    if (btnCaller) {
    //        $this.button('reset');
    //    }
    //    $("#divAccessorySearch_" + prefix).modal("show");
    //    ViewBagPreFix = prefix;
    //    isHasRadioFunction(isHasRadio, SearchAccessoryByCategoryType);

    //    if (callback) {
    //        callback();
    //    }
    //}

}

// SaleOrg: Chi nhánh
// isPopup:
//   true: Hiển thị popup Tồn kho
//   false: Không hiển thị
// prefix: để phân biệt các popup nếu có nhiều popup
// componentContainerSelector
//   <div id="resultMaterialComponent"></div> => componentContainerSelector = #resultMaterialComponent
// callback
// current button

function OpenModalMaterialSearch(SaleOrg, isPopup, prefix, componentContainerSelector, callback, btnCaller) {
    if (btnCaller) {
        var $this = $(btnCaller);
        $this.button('loading');
    }

    $.ajax({
        type: "POST",
        url: "/Sale/Material/_MaterialSearch",
        data: {
            SaleOrg: SaleOrg,
            isPopup: isPopup,
            prefix: prefix
        },
        success: function (xhr, status, error) {
            if (btnCaller) {
                var $this = $(btnCaller);
                $this.button('reset');
            }
            $(componentContainerSelector).html(xhr);
            $("#divMaterialSearch_" + prefix).modal("show");

            if (callback) {
                callback();
            }
        },
        error: function (xhr, status, error) {
            if (btnCaller) {
                var $this = $(btnCaller);
                $this.button('reset');
            }
            alertPopup(false, xhr.responseText);
        }
    });

    //var isLoadedPrefix = window["isLoaded_Material_" + prefix];
    //if (!isLoadedPrefix) {
    //    $.ajax({
    //        type: "POST",
    //        url: "/Sale/Material/_MaterialSearch",
    //        data: {
    //            SaleOrg: SaleOrg,
    //            isPopup: isPopup,
    //            prefix: prefix
    //        },
    //        success: function (xhr, status, error) {
    //            if (btnCaller) {
    //                var $this = $(btnCaller);
    //                $this.button('reset');
    //            }
    //            $(componentContainerSelector).html(xhr);
    //            $("#divMaterialSearch_" + prefix).modal("show");

    //            if (callback) {
    //                callback();
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            if (btnCaller) {
    //                var $this = $(btnCaller);
    //                $this.button('reset');
    //            }
    //            alertPopup(false, xhr.responseText);
    //        }
    //    });
    //}
    //else {
    //    if (btnCaller) {
    //        $this.button('reset');
    //    }
    //    $("#divMaterialSearch_" + prefix).modal("show");
    //    ViewBagPreFix = prefix;

    //    if (callback) {
    //        callback();
    //    }
    //}
}

function isHasRadioFunction(isHasRadio, SearchAccessoryByCategoryType) {
    if (isHasRadio === false) {
        if (SearchAccessoryByCategoryType === 0) {
            $(".modal_accessory").css("display", "block");
            $(".modal_service").css("display", "none");
        }
        else if (SearchAccessoryByCategoryType === 1) {
            $(".modal_accessory").css("display", "none");
            $(".modal_service").css("display", "block");
        }
    }
}

// SaleOrg: Chi nhánh
// isPopup:
//   true: Hiển thị popup Tồn kho
//   false: Không hiển thị
// prefix: để phân biệt các popup nếu có nhiều popup
// componentContainerSelector
//   <div id="resultAcessoryComponent"></div> => componentContainerSelector = #resultAcessoryComponent
// callback
// current button
// SaleOrderMasterId: mã đơn hàng bán hàng

function OpenModalAccessoryPromotionSearch(SaleOrg, isPopup, prefix, componentContainerSelector, callback, btnCaller, SaleOrderMasterId) {
    if (btnCaller) {
        var $this = $(btnCaller);
        $this.button('loading');
    }

    var isLoadedPrefix = window["isLoaded_" + prefix];

    if (!isLoadedPrefix) {

        var data = {
            SaleOrg: SaleOrg,
            isPopup: isPopup,
            prefix: prefix,
            SaleOrderMasterId: SaleOrderMasterId
        }

        $.ajax({
            type: "POST",
            url: "/Sale/Accessory/_AccessoryPromotionSearch",
            data: data,
            success: function (xhr, status, error) {
                if (btnCaller) {
                    var $this = $(btnCaller);
                    $this.button('reset');
                }
                $(componentContainerSelector).html(xhr);
                $("#divAccessoryPromotionSearch_" + prefix).modal("show");

                if (callback) {
                    callback();
                }
            },
            error: function (xhr, status, error) {
                if (btnCaller) {
                    var $this = $(btnCaller);
                    $this.button('reset');
                }
                alertPopup(false, xhr.responseText);
            }
        });
    } else {
        if (btnCaller) {
            $this.button('reset');
        }
        $("#divAccessoryPromotionSearch_" + prefix).modal("show");
        ViewBagPreFix = prefix;

        if (callback) {
            callback();
        }
    }
}

// prefix: để phân biệt các popup nếu có nhiều popup
// componentContainerSelector
//   <div id="resultAcessoryComponent"></div> => componentContainerSelector = #resultAcessoryComponent
// callback
// current button

function OpenModalCustomerSearch(prefix, componentContainerSelector, callback, btnCaller) {
    if (btnCaller) {
        var $this = $(btnCaller);
        $this.button('loading');
    }

    var isLoadedPrefix = window["isLoaded_" + prefix];

    if (!isLoadedPrefix) {

        var data = {
            prefix: prefix
        }

        $.ajax({
            type: "POST",
            url: "/Customer/Customer/_CustomerSearch",
            data: data,
            success: function (xhr, status, error) {
                if (btnCaller) {
                    var $this = $(btnCaller);
                    $this.button('reset');
                }
                $(componentContainerSelector).html(xhr);
                $("#divCustomerSearch_" + prefix).modal("show");

                if (callback) {
                    callback();
                }
            },
            error: function (xhr, status, error) {
                if (btnCaller) {
                    var $this = $(btnCaller);
                    $this.button('reset');
                }
                alertPopup(false, xhr.responseText);
            }
        });
    } else {
        if (btnCaller) {
            $this.button('reset');
        }
        $("#divCustomerSearch_" + prefix).modal("show");
        ViewBagPreFix = prefix;

        if (callback) {
            callback();
        }
    }
}