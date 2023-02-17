
(function ($) {
    "use strict";

    
    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('submit',function(){
        var check = true;

        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i]);
                check=false;
            }
        }

        return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
        });
    });

    function validate (input) {
        if($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
            if($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else {
            if($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }

    $(document).ready(function () {
        ShowCompanyDropdown();
    });

    $(document).on("change", "select[name='CompanyId']", function () {
        var CompanyId = $(this).val();
        var username = $("input[name='UserName']").val();

        $("#SaleOrg").html("");
        //$("#SaleOrg").append("<option value=''>-- Chi nh\u00E1nh --</option>");

        if (CompanyId != "") {
            $(".SaleOrg").removeClass("hidden");
            $.ajax({
                type: "POST",
                url: "/Permission/Auth/GetSaleOrgBy",
                data: {
                    CompanyId: CompanyId,
                    UserName: username
                },
                success: function (jsonData) {
                    if (jsonData != null && jsonData.length > 0) {
                        $.each(jsonData, function (index, value) {
                            $("#SaleOrg").append("<option value='" + value.SaleOrgCode + "'>" + value.StoreName + "</option>");
                        });
                    }
                    else {
                        $("#SaleOrg").append("<option value=''>-- Chi nh\u00E1nh --</option>");
                    }
                },
                error: function (xhr, status, error) {
                    console.log(xhr.responseText);
                }
            });
        }
        else {
            $(".SaleOrg").addClass("hidden");
        }
    });

    $(document).on("change", "input[name='UserName']", function () {
        var username = $("input[name='UserName']").val();
        $.ajax({
            type: "POST",
            url: "/Permission/Auth/GetCompanyBy",
            data: {
                UserName: username
            },
            success: function (jsonData) {
                $("#CompanyId").html("");
                $("#CompanyId").append("<option value=''>-- C\u00F4ng ty --</option>");

                if (jsonData != false && jsonData != null && jsonData.length > 0) {
                    $.each(jsonData, function (index, value) {
                        $("#CompanyId").append("<option value='" + value.CompanyId + "'>" + value.CompanyName + "</option>");
                    });
                }
                ShowCompanyDropdown();
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    });

    $(document).on("change", "input[name='Password']", function () {
        ShowCompanyDropdown();
    });

    function ShowCompanyDropdown() {
        var username = $("input[name='UserName']").val();
        var password = $("input[name='Password']").val();
        var loginValidation = $("#login-validation").html();
        if (username != "") {
            $(".CompanyId").removeClass("hidden");
            var CompanyId = $("#CompanyId").val();
            if (CompanyId != "") {
                $(".SaleOrg").removeClass("hidden");
            }
            else {
                $(".SaleOrg").addClass("hidden");
            }
        }
        else {
            if (!$(".CompanyId").hasClass("hidden")) {
                $(".CompanyId").addClass("hidden");
            }
            if (!$(".SaleOrg").hasClass("hidden")) {
                $(".SaleOrg").addClass("hidden");
            }
        }
    }

})(jQuery);