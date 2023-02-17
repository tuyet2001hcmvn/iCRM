// Tính thành tiền
function UnitPrice(row) {
    // Lấy giá trị số lượng, giá
    var Quantity = $("input[name='detail[" + row + "].Quantity']").val();
    var UnitPrice = $("input[name='detail[" + row + "].UnitPrice']").val();
    var TotalPrice = parseInt(Quantity) * parseInt(UnitPrice);
    $("input[name='detail[" + row + "].TotalPrice']").val(TotalPrice);
    $("#totalPrice_" + row).html(formatCurrency(TotalPrice));
}

function AccessoryTotalPrice() {
    SubTotal = 0;
    $(".detail-TotalPrice").each(function () {
        var dataRow = $(this).data("row");
        var TotalPrice = $("input[name='detail[" + dataRow + "].TotalPrice']").val();
        if (!TotalPrice) {
            TotalPrice = 0;
        }
        SubTotal += parseInt(TotalPrice);
        $("#totalPrice_" + dataRow).html(formatCurrency(parseInt(TotalPrice)));
    });
    $("#total").html(formatCurrency(SubTotal));
    $("#AccessoryTotalPrice").val(SubTotal);
    $("#accessoryTotalPrice").html(formatCurrency(SubTotal));

    var SaleOrderId = $("#SaleOrderMasterId").val();
    $.ajax({
        type: "POST",
        url: "/Sale/SaleOrder/GetTotalPrice/" + SaleOrderId,
        //data: data,
        success: function (jsonData) {
            $("#totalPrice").html(jsonData);
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
}

// Tính tổng tiền
function TotalPrice() {
    var SubTotal = 0;
    $(".detail-TotalPrice").each(function () {
        var dataRow = $(this).data("row");
        var TotalPrice = $("input[name='detail[" + dataRow + "].TotalPrice']").val();
        if (TotalPrice === "") {
            TotalPrice = 0;
        }
        SubTotal += parseInt(TotalPrice);
        $("#totalPrice_" + dataRow).html(formatCurrency(parseInt(TotalPrice)));
    });
    $("#total").html(formatCurrency(SubTotal));
    $("#AccessoryTotalPrice").val(SubTotal);
    $("#accessoryTotalPrice").html(formatCurrency(SubTotal));

    //Tính tổng tiền đơn hàng
    var Total = 0;
    //Tổng tiền = Giá bán lẻ + Tổng lệ phí + BH Cứu hộ xe máy + Phụ kiện (nếu có) - Chiết khấu (nếu có)
    var GiaBanLe = $("input[name='SalePrice']").val();
    var TongLePhi = $("input[name='FeeTotal']").val();
    //var BHTNDS = $("input[name='BHTNDS']").val();
    //var BHCHXM = $("input[name='BHCHXM']").val();
    var discount = 0;
    //0: VND
    //1: %
    var LoaiChietKhau = parseInt($("#DiscountType").val());
    var ChietKhau = $("input[name='Discount']").val();
    if (ChietKhau) {
        discount = parseInt(ChietKhau);
    }

    //Tính các phí bảo hiểm
    $(".phi-baohiem input[id='TotalPrice']").each(function () {
        var value = $(this).val();
        SubTotal += parseInt(value);
    });

    if (parseInt(TongLePhi) > 0) {
        Total = parseInt(GiaBanLe) + parseInt(TongLePhi) + SubTotal;
    }
    else {
        Total = parseInt(GiaBanLe) + SubTotal;
    }

    $("#subtotal").html(formatCurrency(Total));

    if (LoaiChietKhau === 0) {
        $("#discount-display").html(formatCurrency(parseInt(discount)));
        Total = Total - discount;
    }
    else {
        //$("#discount-price").html(formatCurrency(parseInt((discount / 100) * Total)));
        $("#discount-display").html(formatCurrency(parseInt((discount / 100) * Total)));
        Total = Total - parseInt((discount / 100) * Total);
    }
    
    $("input[name='Total']").val(Total);
    $("#totalPrice").html(formatCurrency(Total));
}

//format các loại giá
function formatPrice() {
    //Tiền mặt
    var cash = $("input[name=DownPaymentCash]").val();
    if (cash) {
        var cashInt = parseInt(cash);
        $("input[name=DownPaymentCash]").val(cashInt);
        $("#cash").html(formatCurrency(cashInt));
    }
    else {
        $("#cash").html("");
    }

    //Chuyển khoản
    var transfer = $("input[name=DownPaymentTransfer]").val();
    if (transfer) {
        var transferInt = parseInt(transfer);
        $("input[name=DownPaymentTransfer]").val(transferInt);
        $("#transfer").html(formatCurrency(transferInt));
    }
    else {
        $("#transfer").html("");
    }

    //Trả góp
    var installment = $("input[name=DownPayment]").val();
    if (installment) {
        var installmentInt = parseInt(installment);
        $("input[name=DownPayment]").val(installmentInt);
        $("#installment").html(formatCurrency(installmentInt));
    }
    else {
        $("#installment").html("");
    }

    //Số tiền còn lại
    var balanceDue = $("input[name=BalanceDue]").val();
    if (balanceDue) {
        var balanceDueInt = parseInt(balanceDue);
        $("input[name=BalanceDue]").val(balanceDueInt);
        $("#balance-due").html(formatCurrency(balanceDueInt));
    }

    //Giá bán lẻ dự toán
    var giaBanLe = $("input[name=SalePriceEstimated]").val();
    if (giaBanLe) {
        var giaBanLeInt = parseInt(giaBanLe);
        $("input[name=SalePriceEstimated]").val(giaBanLeInt);
        $("#sale-price").html(formatCurrency(giaBanLeInt));
    }

    //Chiết khấu
    //var chietKhau = $("input[name=Discount]").val();
    //var loaiChietKhau = parseInt($("#DiscountType").val());
    //var Total = $("input[name='TotalPrice']").val();
    //if (chietKhau) {
    //    var chietKhauInt = parseInt(chietKhau);
    //    $("input[name=Discount]").val(chietKhauInt);
    //    if (loaiChietKhau === 0) {
    //        $("#discount-price").html(formatCurrency(chietKhauInt));
    //    }
    //    else {
    //        $("#discount-price").html(formatCurrency(parseInt((chietKhauInt / 100) * Total)));
    //        $("#discount-display").html(formatCurrency(parseInt((chietKhauInt / 100) * Total)));
    //    }
    //}
    //else {
    //    $("#discount-price").html("");
    //}

    //Chiết khấu giá bán lẻ
    //var discountPercentSalePrice = $("input[name=DiscountPercentSalePrice]").val();
    //if (discountPercentSalePrice) {
    //    var discountPercentSalePriceInt = parseInt(discountPercentSalePrice);
    //    $("input[name=DiscountPercentSalePrice]").val(discountPercentSalePriceInt);
    //}

    var chietKhau = $("input[name=DiscountSalePrice]").val();
    var loaiChietKhau = parseInt($("#DiscountType").val());
    var giaBanLe2 = parseInt($("#SalePriceOriginal").val());
    if (chietKhau) {
        var chietKhauInt = parseInt(chietKhau);
        $("input[name=DiscountSalePrice]").val(chietKhauInt);
        if (loaiChietKhau === 0) {
            $("#discount-price").html(formatCurrency(chietKhauInt));
        }
        else {
            $("#discount-price").html(formatCurrency(parseInt((chietKhauInt / 100) * giaBanLe2)));
        }
    }
    else {
        $("#discount-price").html("");
    }

    //Chiết khấu theo từng phụ kiện
    $(".detail-Discount").each(function (index, element) {
        var discountAccessory = $("input[name='detail[" + index + "].Discount']").val();
        if (discountAccessory) {
            var discountAccessoryInt = parseInt(discountAccessory);
            $("input[name='detail[" + index + "].Discount']").val(discountAccessoryInt);
        }
    });
}

//Tính số tiền còn lại
function BalanceDueCalculate() {
    var balanceDue = 0;
    var totalPrice = 0;
    var cash = 0;
    var transfer = 0;
    var installment = 0;

    //Số tiền còn lại = Tổng tiền - (Tiền mặt + Chuyển khoản + Trả góp)

    //Tổng tiền
    if ($("#TotalPrice").val()) {
        totalPrice = parseInt($("#TotalPrice").val());
    }
    //Tiền mặt
    if ($("#DownPaymentCash").val()) {
        cash = parseInt($("#DownPaymentCash").val());
    }
    //Chuyển khoản
    if ($("#DownPaymentTransfer").val()) {
        transfer = parseInt($("#DownPaymentTransfer").val());
    }
    //Trả góp
    if ($("#DownPayment").val()) {
        installment = parseInt($("#DownPayment").val());
    }

    balanceDue = totalPrice - (cash + transfer + installment);
    $("#balance-due").html(formatCurrency(balanceDue));
    $("input[name=BalanceDue]").val(balanceDue);
}

//Hàm chuyển đổi nút chuyển kho
function TransferMaterialButtonDisplay() {
    var isGiuXe = $("#IsGiuXe").val();
    if (isGiuXe.toLowerCase() === "true") {
        $("#transfer-material").removeClass("btn-success").addClass("btn-warning");
        $("#transfer-material").html("<i class='fa fa-unlock' aria-hidden='true'></i>");
        $("#check-inventory").attr("disabled", true);
        $("#check-inventory").addClass("disabled");
    }
    else {
        $("#transfer-material").removeClass("btn-warning").addClass("btn-success");
        $("#transfer-material").html("<i class='fa fa-lock' aria-hidden='true'></i>");
        $("#check-inventory").attr("disabled", false);
        $("#check-inventory").removeClass("disabled");
        //$("#SerialNumber").val("");
        //$("#EngineNumber").val("");
        //$("#Batch").val("");
        //$("#DisplaySerialNumber").val("");
        //$("#DisplayEngineNumber").val("");

    }
}

//Hàm chọn xe để bán (chỉ chọn được 1 lần 1 xe)
function material_customMaterialAction(MaterialCode, MaterialName, MaterialUnit, MaterialGroupName, ProfitCenterName, ProductHierarchyName, LaborName, MaterialFreightGroupName, ExternalMaterialGroupName, TemperatureConditionName, Plant, WarehouseCode, Batch, Quantity, ChassisNumber, EngineNumber, Modal_InvoicePrice, Modal_RegistrationFeePrice, MinPrice, PreFix) {
    //Nếu cùng mã xe thì không dự toán lại giá
    var currentMaterial = $("#MaterialCode").val();
    var isSame = false;
    if (currentMaterial === MaterialCode) {
        isSame = true;
    }

    //Gán dữ liệu vào số khung, số máy
    $("#SerialNumber").val(ChassisNumber);
    $("#EngineNumber").val(EngineNumber);

    $("#DisplaySerialNumber").val(ChassisNumber);
    $("#DisplayEngineNumber").val(EngineNumber);

    $("#Plant").val(Plant);
    $("#WarehouseCode").val(WarehouseCode);
    $("#Batch").val(Batch);

    //Gán dữ liệu thông tin xe
    //1. Mã xe
    $("#MaterialCode").val(MaterialCode);
    $("#material-code").html(MaterialCode);
    //2. ĐVT
    $("#Unit").val(MaterialUnit);
    $("#unit").html(MaterialUnit);
    //3. Nhãn hiệu
    $("#ProfitCenter").val(ProfitCenterName);
    $("#profit-center").html(ProfitCenterName);
    //4. Loại xe
    $("#ProductHierarchy").val(ProductHierarchyName);
    $("#product-hierarchy").html(ProductHierarchyName);
    //5. Dòng xe
    $("#MaterialGroup").val(MaterialGroupName);
    $("#material-group").html(MaterialGroupName);
    //6. Phiên bản
    $("#Labor").val(LaborName);
    $("#labor").html(LaborName);
    //7. Màu sắc
    $("#MaterialFreightGroup").val(MaterialFreightGroupName);
    $("#material-freight-group").html(MaterialFreightGroupName);
    //8. Đời xe
    $("#ExternalMaterialGroup").val(ExternalMaterialGroupName);
    $("#external-material-group").html(ExternalMaterialGroupName);
    //9. Kiểu xe
    $("#TemperatureCondition").val(TemperatureConditionName);
    $("#temperature-condition").html(TemperatureConditionName);

    //Tính lại giá
    if (!isSame) {


        var materialCode = MaterialCode;
        var districtId = $("#DistrictId").val();
        var giaBanLe = MinPrice;
        if (giaBanLe) {
            $.ajax({
                type: "POST",
                url: "/Sale/SaleOrder/Estimated?MaterialCode=" + materialCode + "&DistrictId=" + districtId + "&GiaBanLe=" + giaBanLe,
                //data: data,
                success: function (jsonData) {
                    if (jsonData.Success === true) {
                        //1. Cập nhật giá bán lẻ
                        $("#sale-price-material").html(formatCurrency(jsonData.Data.SalePrice));
                        $("#SalePrice").val(jsonData.Data.SalePrice);

                        //giá bán lẻ chưa chiết khấu
                        $("#sale-price-after-discount").html(formatCurrency(jsonData.Data.SalePrice));
                        $("#SalePrice").val(jsonData.Data.SalePrice);

                        //2. Cập nhật giá tính thuế
                        $("#vat-price").html(formatCurrency(jsonData.Data.VATPrice));
                        $("#VATPrice").val(jsonData.Data.VATPrice);

                        //3. Cập nhật mức lệ phí
                        //$("#register-fee").html(formatCurrency(jsonData.Data.MucLePhi));
                        //var separate = DecimalSeparator();
                        //if (separate == '.') {
                        //    $("#RegisterFee").val(parseFloat(jsonData.Data.RegisterFee));
                        //}
                        //else {
                        //    var RegisterFee = jsonData.Data.RegisterFee.replace('.', ',');
                        //    $("#RegisterFee").val(RegisterFee);
                        //}
                        $("#register-fee").html(formatCurrency(jsonData.Data.MucLePhi));
                        $("#RegisterFee").val(parseFloat(jsonData.Data.RegisterFee));

                        //4. Cập nhật lệ phí trước bạ
                        $("#register-fee-total").html(formatCurrency(jsonData.Data.RegisterFeeTotal));
                        $("#RegisterFeeTotal").val(jsonData.Data.RegisterFeeTotal);

                        //5. Cập nhật lệ phí biển số
                        $("#license-price").html(formatCurrency(jsonData.Data.LicensePrice));
                        $("#LicensePrice").val(jsonData.Data.LicensePrice);

                        //6. Cập nhật BHTNDS
                        //$("#bh-tnds").html(formatCurrency(jsonData.Data.BHTNDS));
                        //$("#BHTNDS").val(jsonData.Data.BHTNDS);

                        //7. Cập nhật phí dịch vụ
                        $("#service-fee").html(formatCurrency(jsonData.Data.ServiceFee));
                        $("#ServiceFee").val(jsonData.Data.ServiceFee);

                        //8. Cập nhật tổng lệ phí
                        $("#fee-total").html(formatCurrency(jsonData.Data.FeeTotal));
                        $("#FeeTotal").val(jsonData.Data.FeeTotal);

                        //9. Cập nhật BHCHXM
                        //$("#bh-chxm").html(formatCurrency(jsonData.Data.BHCHXM));
                        //$("#BHCHXM").val(jsonData.Data.BHCHXM);

                        //Cập nhật các loại bảo hiểm
                        $(".baohiem").html("");
                        $(jsonData.Data.InsuranceList).each(function (item, index) {
                            $(".baohiem").append(
                                '<div class="form-group"><div class="col-md-12"><div class="col-md-4"><div class="label-wrapper"><label class="control-label ten-baohiem">' + index.InsuranceName + '</label></div></div> <div class="col-md-4"><div class="display-for text-right phi-baohiem"> <input type="hidden" name="detailInsurance[' + item + '].TotalPrice" id="TotalPrice" value="' + parseFloat(index.InsurancePrice) + '" /><input type="hidden" name="detailInsurance[' + item + '].AccessoryCode" value="' + index.InsuranceCode + '" /><input type="hidden" name="detailInsurance[' + item + '].AccessoryName" value="' + index.InsuranceName + '" />' + formatCurrency(index.InsurancePrice) + '</div></div></div></div>');
                        });

                        //Tính lại tổng tiền
                        //TotalPrice();

                        $("#totalPrice").html(jsonData.Data.TongCong);
                        $("#TotalPrice").val(jsonData.Data.TotalPrice);
                    }

                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        }
    }
    //Xóa chi tiết khuyến mãi và phụ kiện theo xe
    //$("#promotionDetailTable tbody").html("");
    //$("#promotionDetailTable tbody").html("");
    //close popup
    $("#divMaterialConfirm_" + PreFix).modal("hide");
    //$("#divMaterialSearch_" + PreFix).modal("hide");
}

//Thêm mới phụ tùng/phụ kiện vào lưới
function accessoryMaterial_customAccessoryAction(AccessoryCode, AccessoryName, Plant, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, Index, WarehouseName) {
    
    //Update số lượng theo mã phụ kiện
    var inputQuantity = parseInt($("#input-quantity-" + Index).val());
    //$(".detail-AccessoryCode").each(function (index, e) {
    //    if (e.value == AccessoryCode) {
    //        Quantity = parseInt($("input[name='detail[" + index + "].Quantity']").val()) + 1;
    //    }
    //});
    //Mã đơn hàng dạng Guid trên web
    var SaleOrderId = $("#SaleOrderMasterId").val();
    //Mã đơn hàng
    var SaleOrderCode = $("#SaleOrderMasterCode").val();
    //Mã chi nhánh
    var SaleOrg = $("#SaleOrg").val();
    //Tên khách hàng
    var CustomerName = $("#CustomerName").val();
    //Mã nhân viên
    var EmployeeCode = $("#EmployeeCode").val();

    if (inputQuantity > 0) {
        $.ajax({
            type: "POST",
            url: "/Sale/SaleOrder/InsertAccessory",
            //data: formData,
            //processData: false,
            //contentType: false,
            data: {
                AccessoryCode: AccessoryCode,
                AccessoryName: AccessoryName,
                AccessoryUnit: Unit,
                UnitPrice: AccessoryPrice,
                Quantity: inputQuantity,
                Plant: Plant,
                WarehouseCode: WarehouseCode,
                SaleOrderMasterId: SaleOrderId,
                SaleOrderId: SaleOrderId,
                SaleOrderCode: SaleOrderCode,
                SaleOrg: SaleOrg,
                CustomerName: CustomerName,
                EmployeeCode: EmployeeCode,
                WarehouseName: WarehouseName,
            },
            success: function (xhr, status, error) {
                if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                    alertModalPopupOnhand("#divModalOnhandWarning", xhr.Message);
                }
                else {
                    $("#accessoryDetailTable tbody").html(xhr);
                    //Sau khi thêm => Close 2 modal popup
                    $("#divAccessoryConfirm_" + PreFix).modal("hide");

                    AccessoryTotalPrice();

                    //TotalPrice();
                    formatPrice();
                    BalanceDueCalculate();

                    //Reset lại chiết khấu
                    $("input[name='detail[" + Index + "'].Discount").val("");
                    $(".select-detail-Discount").select2({
                        minimumResultsForSearch: Infinity,
                    });
                }

            },
            error: function (xhr, status, error) {
                alertPopup(false, xhr.responseText);
            }
        });
        $("#divAccessorySearch_" + PreFix).modal("hide");
    }
    else {
        alertModalPopupOnhand("#divModalOnhandWarning", "Vui lòng nhập số lượng lớn hơn 0!");
    }
}

//Autocomplete tìm kiếm và check tồn kho phụ tùng/phụ kiện
function SearchText_AccessoryCode() {
    $("#AccessoryCode2").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Sale/AccessorySaleOrder/GetAccessoryCode",
                data: JSON.stringify({ "AccessoryCode": $("#AccessoryCode2").val() }),
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.AccessoryCodeText, value: item.AccessoryCode, id: item.AccessoryName };
                    }));
                    //response(data);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        },
        //prevent another value except dropdown value
        change: function (event, ui) {
            if (ui.item === null) {
                $("#AccessoryCode2").val("");
                $("#AccessoryName2").val("");
                $("#AccessoryCode2").focus();
            }
        },
        select: function (event, ui) {
            $("#AccessoryCode2").val(ui.item.value);
            $("#AccessoryCode2").prop("disabled", true);
            $("#AccessoryName2").val(ui.item.id);

            $("#Quantity2").focus();

            $.ajax({
                type: "POST",
                url: "/Sale/AccessorySaleOrder/GetOnhandAcessory",
                data: {
                    AccessoryCode: ui.item.value
                },
                success: function (xhr, status, error) {
                    $("#WarehouseCode2").val(xhr.WarehouseCode);
                    $("#SumOfQuantity").val(xhr.SumOfQuantity);
                    $("#Location2").val(xhr.Location);
                    $("#Unit2").val(xhr.Unit);
                    $("#WarehouseName2").val(xhr.WarehouseName);
                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
        }
    });
}

//Xóa dữ liệu autocomplete phụ tùng/phụ kiện
$(document).on("click", "#btn-del-acc-2", function () {
    $("#AccessoryCode2").val("");
    $("#AccessoryCode2").prop("disabled", false);

    $("#AccessoryName2").val("");
    $("#WarehouseCode2").val("");
    $("#SumOfQuantity").val("");
    $("#Location2").val("");
    $("#Unit2").val("");
    $("#WarehouseName2").val("");
});

//Thêm vào lưới phụ tùng/phụ kiện
$(document).on("click", "#btn-add-acc-2", function () {
    var $btn = $(this);
    $btn.button('loading');

    var SaleOrderMasterId = $("#SaleOrderMasterId").val();
    var AccessoryCode = $("#AccessoryCode2").val();
    var AccessoryName = $("#AccessoryName2").val();
    var WarehouseCode = $("#WarehouseCode2").val();
    var Location = $("#Location2").val();
    var Unit = $("#Unit2").val();
    var Quantity = parseInt($("#Quantity2").val());
    var WarehouseName = $("#WarehouseName2").val();
    var SumOfQuantity = $("#SumOfQuantity").val();

    if (AccessoryCode === "") {
        $btn.button('reset');
        alertPopup(false, "Vui lòng chọn phụ tùng/phụ kiện trước khi thêm!");
    }
    else {
        if (Quantity > 0) {

            if (Quantity > SumOfQuantity) {
                $btn.button('reset');
                alertPopup(false, "Vui lòng nhập số lượng nhỏ hơn hoặc bằng số lượng tồn kho!");
            }

            var data = "AccessoryCode=" + AccessoryCode;
            data = data + "&AccessoryName=" + AccessoryName;
            data = data + "&WarehouseCode=" + WarehouseCode;
            data = data + "&Quantity=" + Quantity;
            data = data + "&AccessoryUnit=" + Unit;
            data = data + "&Location=" + Location;
            data = data + "&SaleOrderMasterId=" + SaleOrderMasterId;
            data = data + "&WarehouseName=" + WarehouseName;
            data = data + "&SumOfQuantity=" + SumOfQuantity;


            $.ajax({
                type: "POST",
                url: "/Sale/SaleOrder/InsertAccessory",
                data: data,
                success: function (xhr, status, error) {
                    $btn.button('reset');
                    if (xhr.Message !== "" && xhr.Message !== null && xhr.Message !== undefined) {
                        alertPopup(false, xhr.Message);
                    }
                    else {
                        $("#accessoryDetailTable tbody").html(xhr);
                        AccessoryTotalPrice();

                        //TotalPrice();
                        formatPrice();
                        BalanceDueCalculate();

                        $("#AccessoryCode2").val("");
                        $("#AccessoryCode2").prop("disabled", false);

                        $("#AccessoryName2").val("");
                        $("#WarehouseCode2").val("");
                        $("#SumOfQuantity").val("");
                        $("#Location2").val("");
                        $("#Unit2").val("");
                        $("#Quantity2").val("");
                        $("#WarehouseName2").val("");

                        $("#AccessoryCode2").focus();

                        //Reset lại chiết khấu
                        $("input[name='detail[" + Index + "'].Discount").val("");
                        $(".select-detail-Discount").select2({
                            minimumResultsForSearch: Infinity,
                        });
                    }
                },
                error: function (xhr, status, error) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                }
            });
        }
        else {
            $btn.button('reset');
            alertPopup(false, "Vui lòng nhập số lượng lớn hơn 0!");
        }
    }
});

//Hiển thị modal popup cập nhật số lượng phụ tùng/phụ kiện
$(document).on("click", ".btn-edit-acc", function () {
    var SaleOrderDetailId = $(this).data("id");

    $.ajax({
        type: "POST",
        url: "/Sale/SaleOrder/_AccessoryModal",
        data: {
            SaleOrderDetailId: SaleOrderDetailId
        },
        success: function (xhr, status, error) {
            $("#modalEditAccessory").html("");
            $("#modalEditAccessory").html(xhr);
            //Sau khi thêm => Close 2 modal popup
            $("#modalEditAccessory").modal("show");
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//Cập nhật số lượng + chuyển kho phụ tùng/phụ kiện
$(document).on("click", "#btn-confirm-update-accessory", function () {
    var SaleOrderDetailId = $("#modalEditAccessory #SaleOrderDetailId").val();
    var Quantity = parseInt($("#modalEditAccessory input[name='Quantity']").val());

    if (Quantity > 0) {
        $.ajax({
            type: "POST",
            url: "/Sale/SaleOrder/_UpdateAccessory",
            data: {
                SaleOrderDetailId: SaleOrderDetailId,
                Quantity: Quantity
            },
            success: function (xhr, status, error) {
                if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                    alertModalPopupOnhand("#divModalAccessoryWarning", xhr.Message);
                }
                else {
                    $("#modalEditAccessory").modal("hide");

                    $("#accessoryDetailTable tbody#accessoryDetailList").html("");
                    $("#accessoryDetailTable tbody#accessoryDetailList").html(xhr);
                    //TotalPrice();
                    AccessoryTotalPrice();
                    formatPrice();
                    BalanceDueCalculate();
                }
            },
            error: function (xhr, status, error) {
                $("#modalEditAccessory").modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        alertModalPopupOnhand("#divModalAccessoryWarning", "Vui lòng nhập số lượng lớn hơn 0!");
    }
});

//Thêm khuyến mãi vào lưới
function promotion_customAccessoryPromotionAction(AccessoryCode, AccessoryName, SearchPromotionId, Plant, WarehouseCode, Quantity, Unit, Location, PreFix, Index, WarehouseName) {
    var New_Quantity = parseInt($("#input-quantityPromotion-" + Index).val());
    var SaleOrderMasterId = $('#SaleOrderMasterId').val();

    if (WarehouseCode == "") {
        $("#divAccessoryPromotionConfirm_" + PreFix).modal("hide");
        alertPopup(false, "Khuyến mãi không có hàng!");
    }
    else {
        if (New_Quantity > 0) {
            $.ajax({
                type: "POST",
                url: "/Sale/SaleOrder/InsertAccessoryPromotion",
                data: {
                    SaleOrderMasterId: SaleOrderMasterId,
                    AccessoryCode: AccessoryCode,
                    AccessoryName: AccessoryName,
                    SearchPromotionId: SearchPromotionId,
                    Plant: Plant,
                    WarehouseCode: WarehouseCode,
                    Quantity: New_Quantity,
                    Unit: Unit,
                    Location: Location,
                    WarehouseName: WarehouseName,
                },
                success: function (xhr, status, error) {
                    if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                        alertModalPopupOnhand("#divModalPromotionOnhandWarning", xhr.Message);
                    }
                    else {
                        $("#promotionDetailTable tbody").html("");
                        $("#promotionDetailTable tbody").html(xhr);
                        $("#divAccessoryPromotionConfirm_" + PreFix).modal("hide");
                    }
                    //$("#divAccessoryPromotionConfirm_" + PreFix).modal("hide");

                },
                error: function (xhr, status, error) {
                    alertPopup(false, xhr.responseText);
                }
            });
            $("#divAccessoryPromotionSearch_" + PreFix).modal("hide");
        }
        else {
            alertModalPopupOnhand("#divModalPromotionOnhandWarning", "Vui lòng nhập số lượng lớn hơn 0!");
        }
    }
}

//Hiển thị modal popup cập nhật khuyến mãi
$(document).on("click", ".btn-edit-pro", function () {
    var SaleOrderDetailId = $(this).data("id");

    $.ajax({
        type: "POST",
        url: "/Sale/SaleOrder/_AccessoryPromotionModal",
        data: {
            SaleOrderDetailId: SaleOrderDetailId
        },
        success: function (xhr, status, error) {
            $("#modalEditAccessoryPromotion").html("");
            $("#modalEditAccessoryPromotion").html(xhr);
            //Sau khi thêm => Close 2 modal popup
            $("#modalEditAccessoryPromotion").modal("show");
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
        }
    });
});

//Cập nhật số lượng + chuyển kho khuyến mãi
$(document).on("click", "#btn-confirm-update-accessoryPromotion", function () {
    var SaleOrderDetailId = $("#modalEditAccessoryPromotion #SaleOrderDetailId").val();
    var Quantity = parseInt($("#modalEditAccessoryPromotion input[name='Quantity']").val());

    if (Quantity > 0) {
        $.ajax({
            type: "POST",
            url: "/Sale/SaleOrder/UpdateAccessoryPromotion",
            data: {
                SaleOrderDetailId: SaleOrderDetailId,
                Quantity: Quantity
            },
            success: function (xhr, status, error) {
                $("#modalEditAccessoryPromotion").modal("hide");
                if (xhr.Message != "" && xhr.Message != null && xhr.Message != undefined) {
                    alertPopup(false, xhr.Message);
                }
                else {
                    $("#promotionDetailTable tbody#promotionDetailList").html("");
                    $("#promotionDetailTable tbody#promotionDetailList").html(xhr);
                }
            },
            error: function (xhr, status, error) {
                $("#modalEditAccessoryPromotion").modal("hide");
                alertPopup(false, xhr.responseText);
            }
        });
    }
    else {
        alertModalPopupOnhand("#divModalAccessoryPromotionWarning", "Vui lòng nhập số lượng lớn hơn 0!");
    }
});


//alert popup inside modal popup
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
function alertModalPopupOnhand(divSelector, message) {
    setModalMessage(divSelector, message);
    setTimeout(function () {
        $(divSelector).show();
    }, 500)
}

//Hàm xử lý cập nhật trạng thái giữ xe
function transferMaterial() {
    var data = {
        SaleOrderMasterId: $("#SaleOrderMasterId").val(),
        SaleOrderMasterCode: $("#SaleOrderMasterCode").val(),
        SaleOrg: $("#SaleOrg").val(),
        CustomerName: $("#CustomerName").val(),
        MaterialCode: $("#MaterialCode").val(),
        Batch: $("#Batch").val(),
        Plant: $("#Plant").val(),
        WarehouseCode: $("#WarehouseCode").val(),
        Unit: $("#Unit").val(),
        IsGiuXe: $("#IsGiuXe").val(),
        //Thông tin xe
        EngineNumber: $("#EngineNumber").val(),
        SerialNumber: $("#SerialNumber").val(),
        ProfitCenter: $("#ProfitCenter").val(),
        ProductHierarchy: $("#ProductHierarchy").val(),
        MaterialGroup: $("#MaterialGroup").val(),
        MaterialFreightGroup: $("#MaterialFreightGroup").val(),
        Labor: $("#Labor").val(),
        ExternalMaterialGroup: $("#ExternalMaterialGroup").val(),
        TemperatureCondition: $("#TemperatureCondition").val(),
        ContainerRequirement: $("#ContainerRequirement").val()
    }
    var isGiuXe = $("#IsGiuXe").val();
    if (isGiuXe.toLowerCase() === "true") {
        isGiuXe = true;
    }
    else {
        isGiuXe = false;
    }
    //var isGiuXe = false;
    //if (data.IsGiuXe.toLowerCase() === "true") {
    //    isGiuXe = true;
    //}
    $.ajax({
        type: "POST",
        url: "/Sale/SaleOrder/TransferMaterial",
        data: data,
        success: function (jsonData) {
            if (jsonData.Success === true) {
                $("#divTransferModalPopup").modal("hide");
                alertPopup(true, jsonData.Data);
                $("#IsGiuXe").val(isGiuXe);
                //trigger nút lưu để lưu các thông tin thay đổi khi user đổi xe
                //$("#btn-save-continue").trigger("click");
                //Cập nhật các field hiển thị số khung, số máy nếu bỏ giữ xe
                if (isGiuXe === false) {
                    $("#DisplaySerialNumber").val("");
                    $("#DisplayEngineNumber").val("");
                }
            }
            else {
                $("#divTransferModalPopup").modal("hide");
                alertPopup(false, jsonData.Data);
                $("#IsGiuXe").val(!isGiuXe);
            }
            TransferMaterialButtonDisplay();
        },
        error: function (xhr, status, error) {
            alertPopup(false, xhr.responseText);
            $("#IsGiuXe").val(!isGiuXe);
            TransferMaterialButtonDisplay();
        }
    });
}

//Hàm xử lý hủy đơn hàng
function huyDonHang(controller) {
    var data = {
        SaleOrderMasterId: $("#SaleOrderMasterId").val(),
        SaleOrderMasterCode: $("#SaleOrderMasterCode").val(),
        CanceledNote: $("textarea[name='CanceledNote']").val(),
    };

    $.ajax({
        type: "POST",
        url: "/Sale/SaleOrder/CancelSaleOrder",
        data: data,
        success: function (jsonData) {
            $("#dynamicModal").modal("hide");
            if (jsonData.Success === true) {
                window.location.href = "/" + controller + "?message=" + jsonData.Data;
            }
            else {
                alertPopup(false, jsonData.Data);
            }
        },
        error: function (xhr, status, error) {
            $("#dynamicModal").modal("hide");
            alertPopup(false, xhr.responseText);
        }
    });

}

//Hàm hiển thị thông tin giá theo dự toán
function DisplayPriceInfomation() {
    var tongLePhi = parseFloat($("#FeeTotal").val());
    if (tongLePhi === 0) {
        $(".divHoTro").hide();
    }

    var bhtnds = parseFloat($("#BHTNDS").val());
    if (bhtnds === 0) {
        $("#divBHTNDS").hide();
    }

    var bhchxm = parseFloat($("#BHCHXM").val());
    if (bhchxm === 0) {
        $("#divBHCHXM").hide();
    }
}

function DecimalSeparator() {
    //var n = 1.1;
    //n = n.toLocaleString().substring(1, 2);
    //return n;

    var decSep = ".";

    try {
        // this works in FF, Chrome, IE, Safari and Opera
        var sep = parseFloat(3 / 2).toLocaleString().substring(1, 2);
        if (sep === '.' || sep === ',') {
            decSep = sep;
        }
    } catch (e) { }

    return decSep;
}
