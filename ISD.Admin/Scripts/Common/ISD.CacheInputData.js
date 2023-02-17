var ISDCacheInput = {};
ISDCacheInput.Init = function (pageId) {
    var keyLocalStorage = "pageId-" + pageId+"-input-";
    var frm = $("#frmSearch input[type=text]");
    //$("#frmSearch").attr("autocomplete", "off");
        formData = new FormData(),
        formParams = frm.serializeArray();
    $.each(formParams, function (i, val) {
        //$("#" + val.name).attr("readonly", false);
        //localStorage.setItem(currentUrl + val.name, JSON.stringify([]));
        window[keyLocalStorage + val.name] = JSON.parse(localStorage.getItem(keyLocalStorage + val.name));
        if (window[keyLocalStorage + val.name] == null || window[keyLocalStorage + val.name] == undefined || window[keyLocalStorage + val.name] =="") {
            window[keyLocalStorage + val.name] = [];
        }
        //console.log(currentUrl + val.name + " :" + JSON.parse(localStorage.getItem(currentUrl + val.name)));
    });
    $(document).off("change", "#frmSearch input[type=text]").on("change", "#frmSearch input[type=text]", function () {
        var name = $(this).attr("name");
        var value = $(this).val();
        if (value != "") {
            var obj = {
                index: window[keyLocalStorage + name].length + 1,
                value: value
            }
            if (window[keyLocalStorage + name].length >= 10) {
                window[keyLocalStorage + name].splice(9, 1);
            }
            window[keyLocalStorage + name].unshift(obj);
            localStorage.setItem(keyLocalStorage + name, JSON.stringify(window[keyLocalStorage + name]));
            //console.log(currentUrl + name + " :" + localStorage.getItem(currentUrl + name));
        }

    });
    $(document).off("focus", "#frmSearch input[type=text]").on("focus", "#frmSearch input[type=text]", function () {
        var name = $(this).attr("name");
       // var reverseArr = window[currentUrl + name];
        // console.log(currentUrl + name + " :" + window[currentUrl + name]);
        $(this).autocomplete({
            source: window[keyLocalStorage + name],
            search: "",
            minLength: 0
        }).focus(function () {
            $(this).autocomplete("search", "");
        });
    });
}
