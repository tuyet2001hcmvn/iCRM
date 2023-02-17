
function CustomerTypeCodeChangeInitial() {
    var customerType = $("input[name='CustomerTypeCode']:checked").val();
    var profileType = $("input[name='ProfileTypeCode']").val();
    $("#TypeCode").val(customerType);


    if (profileType == "Account") {
        //Doanh nghiep
        if (customerType == "B") {
            $(".profileB").show();
            $(".profileC").hide();
            $("#divProfileName label").html(Profile_NameBussiness);
            //Add placeholder: cảnh báo nhập sđt + email
            $("#Email").attr("placeholder", EmailBusiness_Hint);
            $("input[name=CompanyNumber]").attr("placeholder", PhoneBusiness_Hint);

            //set width profileName
            setProfileNameWidth("col-md-8");
        } else {
            //Ca nhan
            $(".profileB").hide();
            $(".profileC").show();
            $("#divProfileName label").html(Profile_NameCustomer);
            //Remove placeholder: cảnh báo nhập sđt + email
            $("#Email").attr("placeholder", "");
            $("input[name=Phone]").attr("placeholder", "");

            //set width profileName
            setProfileNameWidth("col-md-7");
        }
    }
}

function setProfileNameWidth(cssClass) {
    //Nếu 7 remove 8
    //if ($("#Profile_General_ProfileName_BC").hasClass("col-md-8") && cssClass == "col-md-7") {
    //    $("#Profile_General_ProfileName_BC").removeClass("col-md-8");
    //    $("#Profile_General_ProfileName_BC").addClass("col-md-7");
    //} else if ($("#Profile_General_ProfileName_BC").hasClass("col-md-7") && cssClass == "col-md-8") {
    //    $("#Profile_General_ProfileName_BC").removeClass("col-md-7");
    //    $("#Profile_General_ProfileName_BC").addClass("col-md-8");
    //}
}

$(document).on("change", "input[name='isForeignCustomer']", function () {
    var isForeignCustomer = $("input[name='isForeignCustomer']:checked").val();
    var type = $("input[name='Type']").val();
    if (isForeignCustomer == undefined) {
        isForeignCustomer = null;
    }

    var SaleOfficeCode = $("#SaleOfficeCode").val();
    if (type == "Opportunity" && isForeignCustomer != "True") {
        $.ajax({
            type: "POST",
            url: "/Customer/Profile/GetOpportunityRegionBy",
            data: {
                isForeignCustomer: isForeignCustomer
            },
            success: function (jsonData) {
                var arr = [];

                $("#RequiredSaleOfficeCode").html("");
                $("#RequiredSaleOfficeCode").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    arr.push(value.Value);
                    $("#RequiredSaleOfficeCode").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });

                if (arr.indexOf(SaleOfficeCode) > -1) {
                    $("#RequiredSaleOfficeCode").val(SaleOfficeCode);
                }
                $("#RequiredSaleOfficeCode").trigger("change");
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Customer/Profile/GetSaleOfficeBy",
            data: {
                isForeignCustomer: isForeignCustomer
            },
            success: function (jsonData) {
                var arr = [];

                $("#RequiredSaleOfficeCode").html("");
                $("#RequiredSaleOfficeCode").append("<option value=''>-- Vui lòng chọn --</option>");
                $.each(jsonData, function (index, value) {
                    arr.push(value.Value);
                    $("#RequiredSaleOfficeCode").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
                });

                if (arr.indexOf(SaleOfficeCode) > -1) {
                    $("#RequiredSaleOfficeCode").val(SaleOfficeCode);
                }
                $("#RequiredSaleOfficeCode").trigger("change");
            }
        });
    }
   
    if (isForeignCustomer == "True") {
        $(".address-foreign").hide();
        $("#provinceName label").html("Quốc gia");
    } else {
        $(".address-foreign").show();
        $("#provinceName label").html("Tỉnh/Thành phố");
    }
});

//ProvinceId change
$(document).on("change", "select[name='RequiredProvinceId']", function () {
    var ProvinceId = $(this).val();
    var DistrictId = $("#DistrictId").val();
    $.ajax({
        type: "POST",
        url: "/MasterData/District/GetDistrictByProvince",
        data: {
            ProvinceId: ProvinceId
        },
        success: function (jsonData) {
            $("#DistrictId").html("");
            $("#DistrictId").append("<option value=''>-- Vui lòng chọn --</option>");
            $.each(jsonData, function (index, value) {
                $("#DistrictId").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
            if (DistrictId) {
                $("#DistrictId").val(DistrictId).trigger("change");
            }
            else {
                $("#DistrictId").trigger("change");
            }
        }
    });
});

//change sale office
$(document).on("change", "select[name='RequiredSaleOfficeCode']", function () {
    var SaleOfficeCode = $(this).val();
    $('#SaleOfficeCode').val(SaleOfficeCode);
    var ProvinceId = $("#RequiredProvinceId").val();
    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetProvinceBy",
        data: {
            SaleOfficeCode: SaleOfficeCode
        },
        success: function (jsonData) {
            var provinceLst = jsonData.provinceList;

            $("#RequiredProvinceId").html("");
            $("#RequiredProvinceId").append("<option value=''>-- Vui lòng chọn --</option>");
            $.each(provinceLst, function (index, value) {
                $("#RequiredProvinceId").append("<option value='" + value.ProvinceId + "'>" + value.ProvinceName + "</option>");
            });
            if (ProvinceId) {
                $("#RequiredProvinceId").val(ProvinceId).trigger("change");
            }
            else {
                $("#RequiredProvinceId").trigger("change");
            }
        }
    });
});

//District change
$(document).on("change", "#frmEdit select[name='DistrictId']", function () {
    var districtId = $(this).val();
    var WardId = $('#WardId').val();
    $.ajax({
        type: "POST",
        url: "/MasterData/Ward/GetWardByDistrict",
        data: {
            DistrictId: districtId
        },
        success: function (jsonData) {
            var $ward = $("#frmEdit #WardId");
            $ward.html("");
            $ward.append("<option value=''>- Vui lòng chọn -</option>");
            $.each(jsonData, function (index, value) {
                $ward.append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });
            if (WardId) {
                $ward.val(WardId).trigger('change');
            }
        }
    });
});

///add More Phone
$(document).on('click', '#frmEdit .btn-addPhone', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    var controlForm = $('#frmEdit .phoneControls:first'),
        currentEntry = $(this).parents('#frmEdit .phonenumber:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('input').val('');
    controlForm.find('.phonenumber:not(:first) .btn-addPhone')
        .removeClass('btn-addPhone').addClass('btn-removePhone')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '#frmEdit .btn-removePhone', function (e) {
    $(this).parents('#frmEdit .phonenumber:last').remove();

    e.preventDefault();
    return false;
});
//end add more phone

var indexperson = 0;
///add More PersonInCharge
$(document).on('click', '.btn-addpersonincharge', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    indexperson++;
    var controlForm = $('.personInchargeControls:first'),
        currentEntry = $(this).parents('.personincharge:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('select[name="personInCharge[0].SalesEmployeeCode"]').attr('name', 'personInCharge[' + indexperson + '].SalesEmployeeCode');
    newEntry.find('select[name="personInCharge[0].RoleCode"]').attr('name', 'personInCharge[' + indexperson + '].RoleCode');

    newEntry.find('.select2').remove();
    $("select").select2();
    controlForm.find('.personincharge:not(:first) .btn-addpersonincharge')
        .removeClass('btn-addpersonincharge').addClass('btn-removepersonincharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '.btn-removepersonincharge', function (e) {
    $(this).parents('.personincharge:last').remove();

    e.preventDefault();
    return false;
});
//end add more PersonInCharge

var indexrole = 0;
///add More RoleInCharge
$(document).on('click', '.btn-addroleincharge', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    indexrole++;
    var controlForm = $('.roleInChargeControls:first'),
        currentEntry = $(this).parents('.roleincharge:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);
    newEntry.find('select[name="roleInCharge[0].RolesId"]').attr('name', 'roleInCharge[' + indexrole + '].RolesId');
    newEntry.find('.select2').remove();
    $("select").select2();
    controlForm.find('.roleincharge:not(:first) .btn-addroleincharge')
        .removeClass('btn-addroleincharge').addClass('btn-removeroleincharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '.btn-removeroleincharge', function (e) {
    $(this).parents('.roleincharge:last').remove();

    e.preventDefault();
    return false;
});
//end add more RoleInCharge

var indexRowProfileGroup = 0;
$(document).on('click', '.btn-addProfileGroup', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".profileGroup").each(function (index, value) {
        indexRowProfileGroup = index;
    });
    indexRowProfileGroup++;

    var controlForm = $('.profileGroupControls:first'),
        currentEntry = $(this).parents('.profileGroup:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.profilegroupcode').attr('name', 'profileGroupList[' + indexRowProfileGroup + '].ProfileGroupCode').val('');
    newEntry.find('.profilegroupcode').data('row', indexRowProfileGroup);

    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.profileGroup:not(:first) .btn-addProfileGroup')
        .removeClass('btn-addProfileGroup').addClass('btn-removeProfileGroup')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeProfileGroup', function (e) {
    $(this).parents('.profileGroup:last').remove();

    $(".profileGroup").each(function (index, value) {
        $(this).find('.profilegroupcode').attr("name", "profileGroupList[" + index + "].ProfileGroupCode");
    });
    e.preventDefault();
    return false;
});

//Xem chi tiết catalog
$(document).on("click", ".btn-showStockDelivery", function () {
    var DeliveryId = $(this).data("id");
    $("#contentCatalogue .dropdown-menu").addClass("hidden");

    $.ajax({
        type: "POST",
        url: "/Warehouse/StockDelivery/GetProductDetails",
        data: {
            DeliveryId: DeliveryId
        },
        success: function (jsonData) {
            $("#contentCatalogue .dropdown-menu").html("");
            $.each(jsonData, function (index, value) {
                $("#contentCatalogue .dropdown-menu").append("<li>" + value.ProductName + "</li>");
            });
            $("#contentCatalogue .dropdown-menu").removeClass("hidden");
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

var indexRowPerson = 0;
//NV kinh doanh cho frmEdit
$(document).on('click', '#frmEdit .btn-addPersonCharge', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var frm = '#frmEdit';
    $(frm + ' .personCharge').each(function (index, value) {
        indexRowPerson = index;
    });
    indexRowPerson++;

    var controlForm = $(frm + ' .personInChargeControls:first'),
        currentEntry = $(this).parents(frm +' .personCharge:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find(' .personemployeecode').attr('name', 'personInChargeList[' + indexRowPerson + '].SalesEmployeeCode').val('');
    newEntry.find(' .personemployeecode').data('row', indexRowPerson);
    newEntry.find(' .personrolecode').attr('name', 'personInChargeList[' + indexRowPerson + '].RoleCode').val('');
    newEntry.find(' .roleName').removeClass('roleName_0');
    newEntry.find(' .roleName').addClass('roleName_' + indexRowPerson);

    newEntry.find(' .select2').remove();
    newEntry.find(' .roleName').html('');
    $(' select').select2();

    controlForm.find(' .personCharge:not(:first) .btn-addPersonCharge')
        .removeClass('btn-addPersonCharge').addClass('btn-removePersonCharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');

    displayTitle();

});

$(document).on("change", "#frmEdit .personemployeecode", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');
    var frm = '#frmEdit';
    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(frm + ' .roleName_' + row).html("");
            $(frm + ' .roleName_' + row).html(jsonData);
        }
    });
});


//Người phụ trách cho frmProfileContact
$(document).on('click', '#frmProfileContact .btn-addPersonCharge', function (e) {
    e.preventDefault();
    var frm = '#frmProfileContact';
    $(frm + ' .personCharge').each(function (index, value) {
        indexRowPerson = index;
    });
    indexRowPerson++;

    var controlForm = $(frm + ' .personInChargeControls:first'),
        currentEntry = $(this).parents(frm + ' .personCharge:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find(' .personemployeecode').attr('name', 'personInChargeList[' + indexRowPerson + '].SalesEmployeeCode').val('');
    newEntry.find(' .personemployeecode').data('row', indexRowPerson);
    newEntry.find(' .personrolecode').attr('name', 'personInChargeList[' + indexRowPerson + '].RoleCode').val('');
    //newEntry.find(' .roleName').removeClass('roleName_0');
    //newEntry.find(' .roleName').addClass('roleName_' + indexRowPerson);

    newEntry.find(' .select2').remove();
/*    newEntry.find(' .roleName').html('');*/
    $(' select').select2();

    controlForm.find(' .personCharge:not(:first) .btn-addPersonCharge')
        .removeClass('btn-addPersonCharge').addClass('btn-removePersonCharge')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');

    displayTitle();

});

$(document).on("change", "#frmProfileContact .personemployeecode", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');
    var frm = '#frmProfileContact';
    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(frm + ' .roleName_' + row).html("");
            $(frm + ' .roleName_' + row).html(jsonData);
        }
    });
});
//==================================================================
//BEGIN NV KINH DOANH
$(document).on('click', '.btn-addPersonCharge1', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson1 = 0;
    $(".personCharge").each(function (index, value) {
        indexRowPerson1 = index;
    });
    indexRowPerson1++;

    var controlForm = $('.personInChargeControls:first'),
        currentEntry = $(this).parents('.personCharge:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode1').attr('name', 'personInCharge1List[' + indexRowPerson1 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode1').data('row', indexRowPerson1);
    newEntry.find('.personrolecode1').attr('name', 'personInCharge1List[' + indexRowPerson1 + '].RoleCode').val('');
    newEntry.find('.roleName').removeClass('roleName1_0');
    newEntry.find('.roleName').addClass('roleName1_' + indexRowPerson1);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName').html('');
    $("select").select2();

    controlForm.find('.personCharge:not(:first) .btn-addPersonCharge1')
        .removeClass('btn-addPersonCharge1').addClass('btn-removePersonCharge1')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge1', function (e) {
    $(this).parents('.personCharge:last').remove();

    $(".personCharge").each(function (index, value) {
        $(this).find('.personemployeecode1').attr("name", "personInCharge1List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode1').attr("name", "personInCharge1List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode1", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName1_" + row).html("");
            $(".roleName1_" + row).html(jsonData);
        }
    });
});
//END NV KINH DOANH
//==================================================================

//==================================================================
//BEGIN NV SALES ADMIN
$(document).on('click', '.btn-addPersonCharge2', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson2 = 0;
    $(".personCharge2").each(function (index, value) {
        indexRowPerson2 = index;
    });
    indexRowPerson2++;

    var controlForm = $('.personInChargeControls2:first'),
        currentEntry = $(this).parents('.personCharge2:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode2').attr('name', 'personInCharge2List[' + indexRowPerson2 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode2').data('row', indexRowPerson2);
    newEntry.find('.personrolecode2').attr('name', 'personInCharge2List[' + indexRowPerson2 + '].RoleCode').val('');
    newEntry.find('.roleName2').removeClass('roleName2_0');
    newEntry.find('.roleName2').addClass('roleName2_' + indexRowPerson2);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName2').html('');
    $("select").select2();

    controlForm.find('.personCharge2:not(:first) .btn-addPersonCharge2')
        .removeClass('btn-addPersonCharge2').addClass('btn-removePersonCharge2')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge2', function (e) {
    $(this).parents('.personCharge2:last').remove();

    $(".personCharge2").each(function (index, value) {
        $(this).find('.personemployeecode2').attr("name", "personInCharge2List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode2').attr("name", "personInCharge2List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode2", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName2_" + row).html("");
            $(".roleName2_" + row).html(jsonData);
        }
    });
});
//END NV SALES ADMIN
//==================================================================

//==================================================================
//BEGIN NV SPEC
$(document).on('click', '.btn-addPersonCharge3', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson3 = 0;
    $(".personCharge3").each(function (index, value) {
        indexRowPerson3 = index;
    });
    indexRowPerson3++;

    var controlForm = $('.personInChargeControls3:first'),
        currentEntry = $(this).parents('.personCharge3:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode3').attr('name', 'personInCharge3List[' + indexRowPerson3 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode3').data('row', indexRowPerson3);
    newEntry.find('.personrolecode3').attr('name', 'personInCharge3List[' + indexRowPerson3 + '].RoleCode').val('');
    newEntry.find('.roleName3').removeClass('roleName3_0');
    newEntry.find('.roleName3').addClass('roleName3_' + indexRowPerson3);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName3').html('');
    $("select").select2();

    controlForm.find('.personCharge3:not(:first) .btn-addPersonCharge3')
        .removeClass('btn-addPersonCharge3').addClass('btn-removePersonCharge3')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge3', function (e) {
    $(this).parents('.personCharge3:last').remove();

    $(".personCharge3").each(function (index, value) {
        $(this).find('.personemployeecode3').attr("name", "personInCharge3List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode3').attr("name", "personInCharge3List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode3", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName3_" + row).html("");
            $(".roleName3_" + row).html(jsonData);
        }
    });
});
//END NV SPEC
//==================================================================
//==================================================================
//BEGIN NV Master
$(document).on('click', '.btn-addPersonCharge4', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson4 = 0;
    $(".personCharge4").each(function (index, value) {
        indexRowPerson4 = index;
    });
    indexRowPerson4++;

    var controlForm = $('.personInChargeControls4:first'),
        currentEntry = $(this).parents('.personCharge4:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode4').attr('name', 'personInCharge4List[' + indexRowPerson4 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode4').data('row', indexRowPerson4);
    newEntry.find('.personrolecode4').attr('name', 'personInCharge4List[' + indexRowPerson4 + '].RoleCode').val('');
    newEntry.find('.roleName4').removeClass('roleName4_0');
    newEntry.find('.roleName4').addClass('roleName4_' + indexRowPerson4);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName4').html('');
    $("select").select2();

    controlForm.find('.personCharge4:not(:first) .btn-addPersonCharge4')
        .removeClass('btn-addPersonCharge4').addClass('btn-removePersonCharge4')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge4', function (e) {
    $(this).parents('.personCharge4:last').remove();

    $(".personCharge4").each(function (index, value) {
        $(this).find('.personemployeecode4').attr("name", "personInCharge4List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode4').attr("name", "personInCharge4List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode4", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName4_" + row).html("");
            $(".roleName4_" + row).html(jsonData);
        }
    });
});
//END NV MAster
//==================================================================
//==================================================================
//BEGIN NV Truy cập
$(document).on('click', '.btn-addPersonCharge5', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson5 = 0;
    $(".personCharge5").each(function (index, value) {
        indexRowPerson5 = index;
    });
    indexRowPerson5++;

    var controlForm = $('.personInChargeControls5:first'),
        currentEntry = $(this).parents('.personCharge5:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode5').attr('name', 'personInCharge5List[' + indexRowPerson5 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode5').data('row', indexRowPerson5);
    newEntry.find('.personrolecode5').attr('name', 'personInCharge5List[' + indexRowPerson5 + '].RoleCode').val('');
    newEntry.find('.roleName5').removeClass('roleName5_0');
    newEntry.find('.roleName5').addClass('roleName5_' + indexRowPerson5);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName5').html('');
    $("select").select2();

    controlForm.find('.personCharge5:not(:first) .btn-addPersonCharge5')
        .removeClass('btn-addPersonCharge5').addClass('btn-removePersonCharge5')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge5', function (e) {
    $(this).parents('.personCharge5:last').remove();

    $(".personCharge5").each(function (index, value) {
        $(this).find('.personemployeecode5').attr("name", "personInCharge5List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode5').attr("name", "personInCharge5List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode5", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName5_" + row).html("");
            $(".roleName5_" + row).html(jsonData);
        }
    });
});
//END NV Truy cập
//==================================================================
//==================================================================
//BEGIN NV TVVL
$(document).on('click', '.btn-addPersonCharge6', function (e) {
    //console.log("Dzoo");
    e.preventDefault();
    var indexRowPerson6 = 0;
    $(".personCharge6").each(function (index, value) {
        indexRowPerson6 = index;
    });
    indexRowPerson6++;

    var controlForm = $('.personInChargeControls6:first'),
        currentEntry = $(this).parents('.personCharge6:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('.personemployeecode6').attr('name', 'personInCharge6List[' + indexRowPerson6 + '].SalesEmployeeCode').val('');
    newEntry.find('.personemployeecode6').data('row', indexRowPerson6);
    newEntry.find('.personrolecode6').attr('name', 'personInCharge6List[' + indexRowPerson6 + '].RoleCode').val('');
    newEntry.find('.roleName6').removeClass('roleName6_0');
    newEntry.find('.roleName6').addClass('roleName6_' + indexRowPerson6);

    newEntry.find('.select2').remove();
    newEntry.find('.roleName6').html('');
    $("select").select2();

    controlForm.find('.personCharge6:not(:first) .btn-addPersonCharge6')
        .removeClass('btn-addPersonCharge6').addClass('btn-removePersonCharge6')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removePersonCharge6', function (e) {
    $(this).parents('.personCharge6:last').remove();

    $(".personCharge6").each(function (index, value) {
        $(this).find('.personemployeecode6').attr("name", "personInCharge6List[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode6').attr("name", "personInCharge6List[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change", ".personemployeecode6", function () {
    var SalesEmployeeCode = $(this).val();
    var row = $(this).data('row');

    $.ajax({
        type: "POST",
        url: "/Customer/Profile/GetRoleBySaleEmployee",
        data: {
            SalesEmployeeCode: SalesEmployeeCode
        },
        success: function (jsonData) {
            $(".roleName6_" + row).html("");
            $(".roleName6_" + row).html(jsonData);
        }
    });
});
//END NV TVVL
//==================================================================
function displayTitle() {
    var customerType = $("input[name='CustomerTypeCode']:checked").val();
    if (customerType == "B") {
        $("#divProfileB").show();
        $("#divProfileC").hide();
    }
    else {
        $("#divProfileB").hide();
        $("#divProfileC").show();
    }
}

$(document).on('click', '.btn-removePersonCharge', function (e) {
    $(this).parents('.personCharge:last').remove();

    $(".personCharge").each(function (index, value) {
        $(this).find('.personemployeecode').attr("name", "personInChargeList[" + index + "].SalesEmployeeCode");
        $(this).find('.personrolecode').attr("name", "personInChargeList[" + index + "].RoleCode");
    });
    e.preventDefault();
    return false;
});

$(document).on("change input", "input[name='ContractValue']", function () {
    var value = $(this).val();
    $(".numberContractValue").html("");
    var formatCurrencyContractValue = formatCurrency(value);
    if (value != "" && value > 0) {
        $(".numberContractValue").removeClass("hidden");
        $(".numberContractValue").html(formatCurrencyContractValue + ' VNĐ');
    }
    else {
        $(".numberContractValue").addClass("hidden");
    }
});

$(document).on("change", "input[name='IsAnCuongAccessory']", function () {
    var IsAnCuongAccessory = $("input[name='IsAnCuongAccessory']:checked").val();
    if (IsAnCuongAccessory == true || IsAnCuongAccessory == "True" || IsAnCuongAccessory == "true") {
        $(".hidden_IsAnCuongAccessory").removeClass("hidden");
    }
    else {
        $(".hidden_IsAnCuongAccessory").addClass("hidden");
    }
});

$(document).on("change", "input[name='IsThiCong']", function () {
    var IsThiCong = $("input[name='IsThiCong']:checked").val();
    if (IsThiCong === "True" || IsThiCong === "true") {
        $(".hidden_IsThiCong").removeClass("hidden");
    }
    else {
        $(".hidden_IsThiCong").addClass("hidden");
    }
});

//Lấy danh sách đơn hàng trên SAP
function GetCustomerSaleOrder() {
    var ProfileForeignCode = $("#ProfileForeignCode").val();
    if (ProfileForeignCode) {
        $.ajax({
            type: "POST",
            url: "/Customer/CustomerSaleOrder/_List",
            data: {
                ProfileForeignCode: ProfileForeignCode,
                isLoadContent: true
            },
            success: function (jsonData) {
                if (!jsonData.Code) {
                    $("#tab-don-hang").html(jsonData);
                }
            }
        });
    }
}

//Xem doanh số theo năm
$(document).on("click", "#btn-view-revenue", function () {
    var $btn = $(this);
    $btn.button('loading');
    var ProfileId = $("#ProfileId").val();
    var CompanyCode = $("#CompanyCode").val();
    var Year = $("#Year").val();

    $.ajax({
        type: "POST",
        url: "/Customer/Revenue/_ProfileRevenue",
        data: {
            id: ProfileId,
            CompanyCode: CompanyCode,
            Year: Year
        },
        success: function (jsonData) {
            if (!jsonData.Code) {
                $("#tab-revenue").html(jsonData);
            }
            $btn.button('reset');
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
            $btn.button('reset');
        }
    });
});

//dự án - thi công 
var indexRowInternal = 0;
$(document).on('click', '.btn-addInternal', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".internal").each(function (index, value) {
        indexRowInternal = index;
    });
    indexRowInternal++;

    var controlForm = $('.internalControls:first'),
        currentEntry = $(this).parents('.internal:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find('.constructionname').attr('name', 'internalList[' + indexRowInternal + '].ConstructionName').val('');
    newEntry.find('.constructionname').attr('id', 'internalList_' + indexRowInternal + '__ConstructionName').val('');
    newEntry.find('.constructionid').attr('name', 'internalList[' + indexRowInternal + '].ConstructionId').val('');
    newEntry.find('.constructionid').attr('id', 'internalList_' + indexRowInternal + '__ConstructionId').val('');
    newEntry.find('.btn-get-profile').data('row', indexRowInternal);
    newEntry.find('.btn-del-profile').data('row', indexRowInternal);

    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.internal:not(:first) .btn-addInternal')
        .removeClass('btn-addInternal').addClass('btn-removeInternal')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeInternal', function (e) {
    $(this).parents('.internal:last').remove();

    $(".internal").each(function (index, value) {
        $(this).find('.constructionname').attr("name", "internalList[" + index + "].ConstructionName");
        $(this).find('.constructionname').attr('id', 'internalList_' + index + '__ConstructionName');
        $(this).find('.constructionid').attr("name", "internalList[" + index + "].ConstructionId");
        $(this).find('.constructionid').attr('id', 'internalList_' + index + '__ConstructionId');
        $(this).find('.btn-get-profile').data('row', index);
        $(this).find('.btn-del-profile').data('row', index);
    });
    e.preventDefault();
    return false;
});

var indexRowCompetitor = 0;
$(document).on('click', '.btn-addCompetitor', function (e) {
    //console.log("Dzoo");
    e.preventDefault();

    $(".competitor").each(function (index, value) {
        indexRowCompetitor = index;
    });
    indexRowCompetitor++;

    var controlForm = $('.competitorControls:first'),
        currentEntry = $(this).parents('.competitor:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);


    newEntry.find('.constructionname').attr('name', 'competitorList[' + indexRowCompetitor + '].ConstructionName').val('');
    newEntry.find('.constructionname').attr('id', 'competitorList_' + indexRowCompetitor + '__ConstructionName').val('');
    newEntry.find('.constructionid').attr('name', 'competitorList[' + indexRowCompetitor + '].ConstructionId').val('');
    newEntry.find('.constructionid').attr('id', 'competitorList_' + indexRowCompetitor + '__ConstructionId').val('');
    newEntry.find('.btn-get-profile').data('row', indexRowCompetitor);
    newEntry.find('.btn-del-profile').data('row', indexRowCompetitor);

    newEntry.find('.select2').remove();
    $("select").select2();

    controlForm.find('.competitor:not(:first) .btn-addCompetitor')
        .removeClass('btn-addCompetitor').addClass('btn-removeCompetitor')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
});

$(document).on('click', '.btn-removeCompetitor', function (e) {
    $(this).parents('.competitor:last').remove();

    $(".competitor").each(function (index, value) {
        $(this).find('.constructionname').attr("name", "competitorList[" + index + "].ConstructionName");
        $(this).find('.constructionname').attr('id', 'competitorList' + index + '__ConstructionName');
        $(this).find('.constructionid').attr("name", "competitorList[" + index + "].ConstructionId");
        $(this).find('.constructionid').attr('id', 'competitorList_' + index + '__ConstructionId');
        $(this).find('.btn-get-profile').data('row', index);
        $(this).find('.btn-del-profile').data('row', index);
    });
    e.preventDefault();
    return false;
});

//add More Email
$(document).on('click', '#frmEdit .btn-addEmail', function (e) {
    e.preventDefault();

    var controlForm = $('#frmEdit .emailControls:first'),
        currentEntry = $(this).parents('#frmEdit .email:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('input').val('');
    controlForm.find('.email:not(:first) .btn-addEmail')
        .removeClass('btn-addEmail').addClass('btn-removeEmail')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '#frmEdit .btn-removeEmail', function (e) {
    $(this).parents('#frmEdit .email:last').remove();

    e.preventDefault();
    return false;
});

$(document).on('click', '#frmProfileContact .btn-addEmail', function (e) {
    e.preventDefault();

    var controlForm = $('#frmProfileContact .emailControls:first'),
        currentEntry = $(this).parents('#frmProfileContact .email:last'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('input').val('');
    controlForm.find('.email:not(:first) .btn-addEmail')
        .removeClass('btn-addEmail').addClass('btn-removeEmail')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="glyphicon glyphicon-minus"></span>');
}).on('click', '#frmProfileContact .btn-removeEmail', function (e) {
    $(this).parents('#frmProfileContact .email:last').remove();

    e.preventDefault();
    return false;
});
//end add more email

$(document).on("click", ".btn-get-spec-profile", function () {
    var $btn = $(this);
    var field = $btn.data('field');
    var row = $btn.data('row');
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Competitor'
        },
        success: function (html) {
            $("#divOppCompetitorPopup").html("");

            $("#divOppCompetitorPopup").html(html);
            $("#divOppCompetitorPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divOppCompetitorPopup #divProfileSearch").attr("id", 'divProfileSearch-spec-competitor');
            $("#divOppCompetitorPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-spec-competitor');

            $("#divOppCompetitorPopup #frmProfileSearchPopup-spec-competitor #ProfileField").val(field);
            $("#divOppCompetitorPopup #frmProfileSearchPopup-spec-competitor #ProfileFieldRow").val(row);

            $("#divOppCompetitorPopup #divProfileSearch-spec-competitor").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", ".btn-del-spec-profile", function () {
    var row = $(this).data("row");
    $('input[name="SpecList[' + row + '].CompetitorId"]').val('');
    $('input[name="SpecList[' + row + '].CompetitorName"]').val('');
});

$(document).on("click", ".btn-get-construction-profile", function () {
    var $btn = $(this);
    var field = $btn.data('field');
    var row = $btn.data('row');
    $btn.button('loading');
    $.ajax({
        type: "POST",
        url: '/Work/Task/_ProfileSearch',
        data: {
            hasNoContact: true,
            ProfileType: 'Competitor'
        },
        success: function (html) {
            $("#divOppCompetitorPopup").html("");

            $("#divOppCompetitorPopup").html(html);
            $("#divOppCompetitorPopup input[id='SearchProfileId']").val("");
            $(".with-search").select2();
            $("#divOppCompetitorPopup #divProfileSearch").attr("id", 'divProfileSearch-construction-competitor');
            $("#divOppCompetitorPopup #frmProfileSearchPopup").attr("id", 'frmProfileSearchPopup-construction-competitor');

            $("#divOppCompetitorPopup #frmProfileSearchPopup-construction-competitor #ProfileField").val(field);
            $("#divOppCompetitorPopup #frmProfileSearchPopup-construction-competitor #ProfileFieldRow").val(row);

            $("#divOppCompetitorPopup #divProfileSearch-construction-competitor").modal("show");

            $btn.button('reset');
        }
    });
});

$(document).on("click", ".btn-del-construction-profile", function () {
    var row = $(this).data("row");
    $('input[name="ConstructionList[' + row + '].CompetitorId"]').val('');
    $('input[name="ConstructionList[' + row + '].CompetitorName"]').val('');
});

//Nếu user chọn là đối thủ => hiển thị loại hình để lưu thông tin đối thủ
//$(document).on("change", "#isCompetitor", function () {
//    if ($(this).is(":checked")) {
//        $(".hidden_Competitor").show();
//    }
//    else {
//        $(".hidden_Competitor").hide();
//    }
//});

$(document).on("change", "#Dropdownlist8", function () {
    //var profileType = $("#ProfileTypeCode").val();
    var competitorType = $(this).val();
    var competitorTypeText = $("#Dropdownlist8 option:selected").text();
    var profileId = $("#ProfileId").val();
    $("#contentDistributionIndustry").html('<tr><td colspan="3" class="text-center"> - Vui lòng chọn loại hình - </td></tr>');
    if (competitorType) {
        loading2();
        $.ajax({
            type: "POST",
            url: '/Customer/Profile/GetDistributionIndustryBy',
            data: {
                CompetitorType: competitorType,
                ProfileId: profileId
            },
            success: function (jsonData) {
                var distributionData = jsonData.DistributionList;
                if (distributionData && distributionData.length > 0) {
                    var htmlContent = "";
                    for (var i = 0; i < distributionData.length; i++) {
                        var check = "";
                        var profileDistributionList = jsonData.ProfileDistributionList;
                        if (profileDistributionList && profileDistributionList.length > 0) {
                            var checkIndex = profileDistributionList.findIndex(e => e.CatalogCode === distributionData[i].CatalogCode);
                            if (checkIndex > -1) {
                                check = "checked";
                            }
                        }
                        htmlContent += '<tr>';
                        htmlContent += '<td class="text-center">' + (i + 1) + '</td>';
                        htmlContent += '<td>';
                        htmlContent += '<input type="hidden" id="DistributionIndustryCatalogCode-' + i + '" name="DistributionIndustry[' + i + '].CatalogCode" value="' + distributionData[i].CatalogCode + '" />';
                        htmlContent += '<span>' + distributionData[i].CatalogText_vi + '</span>';
                        htmlContent += '</td>';
                        htmlContent += '<td class="text-center">';
                        htmlContent += '<input type="checkbox" id="DistributionIndustryIsChecked-' + i + '" name="DistributionIndustry[' + i + '].IsChecked" value="True" ' + check + '/>';
                        htmlContent += '</td>';
                        htmlContent += '</tr>';
                    }
                    $("#contentDistributionIndustry").html(htmlContent);
                }
                else {
                    $("#contentDistributionIndustry").html('<tr><td colspan="3" class="text-center"> Loại hình "' + competitorTypeText + '" không chọn ngành hàng </td></tr>');
                }
            }
        });
    }
});

$(document).on('change', '#CompleteYear', function () {
    $("#Text4").val($(this).val());
});

$(document).on('change', '#CompleteQuarter', function () {
    $("#Text5").val($(this).val());
});

//Autocomplete
function Select2_CustomForList(url, selector) {
    $(selector).select2({
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    search: params.term, // search term
                    page: params.page,
                    type: $(this).data('code'),
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            }
            , minimumInputLength: 1 // Tối thiếu 2 kí tự thì mới search

        }
    });

    $(".specInternalMaterial").each(function () { 
        var row = $(this).data("row");
        $('select[name="SpecInternalList[' + row + '].MaterialId"] option').each(function () {
            //$('select[name="SpecInternalList[' + row + '].MaterialId"]').val($(this).val());
            $(this).prop('selected', true);
        });
        $('select[name="SpecInternalList[' + row + '].MaterialId"]').trigger("change");
    });

    $(".specCompetitor").each(function () {
        var row = $(this).data("row");
        $('select[name="SpecCompetitorList[' + row + '].CompetitorId"] option').each(function () {
            //$('select[name="SpecInternalList[' + row + '].MaterialId"]').val($(this).val());
            $(this).prop('selected', true);
        });

        $('select[name="SpecCompetitorList[' + row + '].CompetitorId"]').trigger("change");
    });

    $(".constructionCompetitor").each(function () {
        var row = $(this).data("row");
        $('select[name="ConstructionCompetitorList[' + row + '].CompetitorId"] option').each(function () {
            //$('select[name="SpecInternalList[' + row + '].MaterialId"]').val($(this).val());
            $(this).prop('selected', true);
        });

        $('select[name="ConstructionCompetitorList[' + row + '].CompetitorId"]').trigger("change");
    });
}

//$('.projectValue, .projectValue2, .projectValue3, .projectValue4').inputFilter(function (value) {
//    return /^-?\d*$/.test(value);
//});

$(document).on("input", ".projectValue", function () {
    var projectValue = $(this).val();
    var row = $(this).data("row");
    //alert(projectValue);
    if (projectValue) {
        var value = parseFloat(projectValue);
        $("#SpecInternalProjectValueDisplay-" + row).html(formatCurrency(value) + '  Tỷ');
    } else {
        $("#SpecInternalProjectValueDisplay-" + row).text('');
    }
});
$(document).on("input", ".projectValue2", function () {
    var projectValue = $(this).val();
    var row = $(this).data("row");
    //alert(projectValue);
    if (projectValue) {
        var value = parseFloat(projectValue);
        $("#SpecCompetitorProjectValueDisplay-" + row).html(formatCurrency(value) + '  Tỷ');
    } else {
        $("#SpecCompetitorProjectValueDisplay-" + row).text('');
    }
});

$(document).on("input", ".projectValue3", function () {
    var projectValue = $(this).val();
    var row = $(this).data("row");
    //alert(projectValue);
    if (projectValue) {
        var value = parseFloat(projectValue);
        $("#ConstructionInternalProjectValueDisplay-" + row).html(formatCurrency(value) + '  Tỷ');
    } else {
        $("#ConstructionInternalProjectValueDisplay-" + row).text('');
    }
});

$(document).on("input", ".projectValue4", function () {
    var projectValue = $(this).val();
    var row = $(this).data("row");
    //alert(projectValue);
    if (projectValue) {
        var value = parseFloat(projectValue);
        $("#ConstructionCompetitorProjectValueDisplay-" + row).html(formatCurrency(value) + '  Tỷ');
    } else {
        $("#ConstructionCompetitorProjectValueDisplay-" + row).text('');
    }
});



//Vốn pháp định
$(document).on("input", ".Number1", function () {
    if ($('#Type').val() === "Competitor") {
        var projectValue = $(this).val();
        var row = $(this).data("row");
        //alert(projectValue);
        if (projectValue) {
            var value = parseFloat(projectValue);
            $("#Number1ValueDisplay").html(formatCurrency(value) + '  Tỷ');
        } else {
            $("#Number1ValueDisplay").text('');
        }
    }
});
//Độ phủ thị trường
$(document).on("input", ".Number2", function () {
    if ($('#Type').val() === "Competitor") {
        var projectValue = $(this).val();
        var row = $(this).data("row");
        //alert(projectValue);
        if (projectValue) {
            var value = parseFloat(projectValue);
            $("#Number2ValueDisplay").html(formatCurrency(value) + '%');
        } else {
            $("#Number2ValueDisplay").text('');
        }
    }

});

$('#Text2').inputFilter(function (value) {
    if ($('#Type').val() === "Opportunity" || $('#Type').val() === "Lead") {
        return /^-?\d*$/.test(value);
    }
    return value;
});