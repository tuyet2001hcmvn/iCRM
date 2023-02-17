(function () {
    'use strict';
    app.controller('accountancyController', function ($scope, $http, $injector, $sce) {

        // Fix 1.1: Select2 inside table not working
        $scope.onEnd = function () {
            console.log("done");
            //$("#accessoryDetailTable .select2").remove();
            //$("#accessoryDetailTable select").select2();
        };

        $scope.onEndService = function () {
            //$("#serviceListTable .select2").remove();
            //$("#serviceListTable select").select2();
        };

        $scope.TotalAmount = function () {
            var total = 0;
            angular.forEach($scope.ListLichSuThuTien, function (item) {
                //console.log(item);
                if (item.IsCanceled === false) {
                    total += item.Amount;
                }
            });
            //console.log(total);
            return total;
        };

        $scope.viewModel = {};
        $scope.test = { a: "123" };
        $scope.loading = false;

        $scope.editSelected = {};





        $scope.initial = function () {

            $scope.PaymentType = "0"; // ở server xài int mà ở đây xài string 
            $scope.PaymentMethod = "0";  // ở server xài int mà ở đây xài string 

            $scope.validChuyenKhoan = true;
            $scope.validTraGop_DonVi = true;
            $scope.validTraGop_SoHopDong = true;
            $scope.validTraGop_Amount = true;

            $scope.isModalOpen = false;
            $scope.hasError = false;
            $scope.errorMessage = "";

            $scope.Amount = 0;

            $scope.initializing = false;
        };

        $scope.openModalThuChi = function () {

            if ($scope.isModalOpen) {
                return;
            }

            var $btn = getTarget();
            btnLoading($btn, true);

            //$("#modalThuChi").modal("show");

            isdPost("/Accountancy/ReceiptVoucher/_ModalThuChi", {},
                function (ret) {
                    var $modalThuChiContent = $("#modalThuChiContent");
                    $modalThuChiContent.html(ret).promise().then(function () {
                        $("#modalThuChi").modal("show");
                        $scope.isModalOpen = true;
                        //Linh: Gán dữ liệu diễn giải vô lúc bật modal popup mới được nè chứ gán trước rồi bật popup nó không truyền dữ liệu vào
                        var paymentType = 0;
                        if ($("#chi").is(':checked')) {
                            paymentType = 1;
                        }
                        $scope.getDescription(paymentType);
                    });
                    btnLoading($btn, false);

                }, function (xhr) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                    btnLoading($btn, false);
                });
        };


        $scope.closeModalThuChi = function () {
            $("#modalThuChi").modal("hide");
        };

        var initializing = true;
        $scope.$watch("loading", function () {
            if (!initializing) {
                if ($scope.loading) {
                    $scope.buttonStorage = $("button:not(:disabled):not(.alert-close)");
                    $scope.buttonStorage.prop('disabled', true);
                } else {
                    $scope.buttonStorage.removeProp('disabled', false);
                    $scope.buttonStorage = null;
                }
            } else {
                initializing = false;
            }

        });

        $scope.$watch("PaymentMethod", function () {
            if (!initializing) {
                console.log("PaymentMethod change");
            } else {
                initializing = false;
            }

        });

        $scope.savePhieuThuChi = function (receiptVoucherId) {
            var $btn = getTarget();
            //console.log($scope);
            btnLoading($btn, true);
            $scope.loading = true;

            //Số tiền thu
            var amount = $("#Amount").val();

            //Loại tiền cọc
            var isCoc = $("input[name=isCoc]:checked").val();

            //if ($scope.PaymentMethod === "0") {

            //}

            var thuTienData = {};

            // TM
            if ($scope.PaymentMethod === "1") {

                if (!$scope.AccountCode) {
                    $scope.validChuyenKhoan = false;
                    btnLoading($btn, false);
                    $scope.loading = false;
                    return;
                }

                thuTienData.AccountCode = $scope.AccountCode;

            }

            // CK
            if ($scope.PaymentMethod === "2") {

                if (!$scope.Organization) {
                    $scope.validTraGop_DonVi = false;
                    btnLoading($btn, false);
                    $scope.loading = false;
                    return;
                }

                if (!$scope.ContractNumber) {
                    $scope.validTraGop_SoHopDong = false;
                    btnLoading($btn, false);
                    $scope.loading = false;
                    return;
                }

                thuTienData.ContractNumber = $scope.ContractNumber;
                thuTienData.Organization = $scope.Organization;
            }

            if (amount && parseInt(amount) === 0) {
                //alertPopup(false, "Vui lòng nhập số tiền thu khác 0");
                $scope.hasError = true;
                $scope.errorMessage = $sce.trustAsHtml("Vui lòng nhập số tiền thu khác 0");
                btnLoading($btn, false);
                $scope.loading = false;
                return;
            }

            if (amount && parseInt(amount) < 0) {
                //alertPopup(false, "Vui lòng nhập số tiền thu hợp lệ");
                $scope.hasError = true;
                $scope.errorMessage = $sce.trustAsHtml("Vui lòng nhập số tiền thu hợp lệ");
                btnLoading($btn, false);
                $scope.loading = false;
                return;
            }

            if (amount > $scope.BalanceDueThucTe && $scope.PaymentType !== "1") {
                console.log("Amount", amount);
                $scope.hasError = true;
                $scope.errorMessage = $sce.trustAsHtml("Số tiền thu <b>(" + formatCurrency(amount) + ")</b> không được vượt quá số tiền còn lại <b>(" + formatCurrency($scope.BalanceDueThucTe) + ")</b>");
                btnLoading($btn, false);
                $scope.loading = false;
                //alertPopup(false, "Số tiền thu <b>(" + formatCurrency(amount) + ")</b> không được vượt quá số tiền còn lại <b>(" + formatCurrency($scope.BalanceDueThucTe) + ")</b>");
                return;
            }

            if (amount) {
                $("#Amount-error").hide();
                //Mã đơn hàng Web GUID
                var saleOrderId = $("#SaleOrderId").val();
                //Mã đơn hàng code
                var saleOrderCode = $("#SaleOrderCode").val();
                //Mã Chi nhánh
                var saleOrg = $("#SaleOrg").val();
                //Số hợp đồng trả góp
                var contractNumber = $("#ContractNumber").val();
                //Tài khoản trả góp
                var organization = $("#Organization").val();
                //Tài khoản chuyển khoản
                var accountCode = $("#AccountCode").val();
                //Diễn giải
                var description = $("#Description").val();
                var descriptionId = $("#DescriptionId").val();
                //Phương thức thanh toán
                //var paymentMethod = $scope.PaymentMethod;
                //GUID
                //var receiptVoucherId = $("input[name='ReceiptVoucherId']").val();

                var data = {
                    SaleOrderId: saleOrderId,
                    SaleOrderCode: saleOrderCode,
                    SaleOrg: saleOrg,

                    //AccountCode: $scope.AccountCode,


                    //ContractNumber: $scope.ContractNumber,
                    //Organization: $scope.Organization,

                    ...thuTienData,

                    Description: description,
                    DescriptionId: descriptionId,
                    Amount: amount,
                    PaymentMethod: $scope.PaymentMethod,
                    PaymentType: $scope.PaymentType,
                    ReceiptVoucherId: receiptVoucherId,
                    isCoc: isCoc,
                };

                //console.log(data);
                //$btn.button('reset');
                //return;
                $.ajax({
                    type: "POST",
                    url: "/Accountancy/ReceiptVoucher/SaveSaleOrderReceipt",
                    data: data,
                    success: function (jsonData) {
                        //$btn.button('reset');
                        console.log(jsonData);
                        if (jsonData.Success === true) {

                            angular.extend($scope, JSON.parse(jsonData.Data)); // Gán tất cả viewmodel từ server vào context của AngularJS (input đã có sẵn ng-model)
                            $scope.$apply();

                            $scope.closeModalThuChi();
                            alertPopup(true, jsonData.Message);
                            //var thisUrl = document.location.pathname
                            //window.location = thisUrl + window.location.search + "&message=" + jsonData.Data;
                            //location.reload();
                            btnLoading($btn, false);
                            $scope.loading = false;
                        }
                        else {
                            //if (jsonData.Data != null && jsonData.Data != "") {

                            //}
                            btnLoading($btn, false);
                            alertPopup(false, jsonData.Data);
                            $scope.loading = false;
                            $scope.closeModalThuChi();
                        }
                    },
                    error: function (xhr, status, error) {
                        btnLoading($btn, false);
                        $scope.closeModalThuChi();
                        $scope.loading = false;
                        alertPopup(false, xhr.responseText);
                    }
                });
            }
            else {
                $scope.loading = false;
                btnLoading($btn, false);
                $scope.validTraGop_Amount = false;
            }
        };

        $scope.huyPhieuThuChi = function () {
            //alert("$scope.Type:", $scope.Type);
            console.log("$scope.Type:", $scope.Type);

            return false;
        };


        $scope.chuyenXeLapRapConfirm = function () {
            var header = "Xác nhận chuyển xe đi lắp ráp";
            var content = "Bạn có chắc chắn muốn chuyển xe này đi lắp ráp?";
            var strSubmitFunc = "chuyenXeLapRap()";
            var btnText = "Có";
            ISD.CreateModal(header, content, strSubmitFunc, btnText);
        };

        $scope.chuyenXeLapRap = function () {
            var $btn = getTarget();
            //console.log($scope);
            btnLoading($btn, true);
            $scope.loading = true;





            var postData = {
                SaleOrderId: $scope.SaleOrderId
            };

            isdPost("/Accountancy/ReceiptVoucher/ChuyenXeLapRap", postData,
                function (ret) {
                    //alert(ret.Data);
                    if (ret.IsSuccess === true) {
                        ISD.ClearModal();
                        btnLoading($btn, false);
                        $scope.loading = false;
                        alertPopup(true, ret.Message);
                    } else {
                        alertPopup(false, ret.Message);
                        ISD.ClearModal();
                        btnLoading($btn, false);
                        $scope.loading = false;
                    }

                }, function (xhr) {
                    ISD.ClearModal();
                    btnLoading($btn, false);
                    $scope.loading = false;
                    alertPopup(false, xhr.responseText);
                });

        };

        $scope.chayHoSoConfirm = function () {
            var header = "Xác nhận chạy hồ sơ trước bạ";
            var content = "Bạn có chắc chắn muốn chạy hồ sơ trước bạ cho xe này không?";
            var strSubmitFunc = "chayHoSo()";
            var btnText = "Có";
            ISD.CreateModal(header, content, strSubmitFunc, btnText);
        }

        $scope.chayHoSo = function () {
            var $btn = getTarget();
            //console.log($scope);
            btnLoading($btn, true);
            $scope.loading = true;

            var postData = {
                SaleOrderId: $scope.SaleOrderId
            };

            isdPost("/Accountancy/ReceiptVoucher/ChayHoSo", postData,
                function (ret) {
                    //alert(ret.Data);
                    if (ret.IsSuccess === true) {
                        ISD.ClearModal();
                        btnLoading($btn, false);
                        $scope.loading = false;
                        alertPopup(true, ret.Message);
                    } else {
                        alertPopup(false, ret.Message);
                        ISD.ClearModal();
                        btnLoading($btn, false);
                        $scope.loading = false;
                    }

                }, function (xhr) {
                    ISD.ClearModal();
                    btnLoading($btn, false);
                    $scope.loading = false;
                    alertPopup(false, xhr.responseText);
                });
        }

        $scope.getDescription = function (paymentType) {
            var postData = {
                PaymentType: paymentType
            };
            isdPost("/Accountancy/ReceiptVoucher/GetDescription", postData,
                function (ret) {
                    //Xóa các option cũ
                    $('#DescriptionId')
                        .children('option:not(:first)')
                        .remove()
                    //Thêm option mới vào
                    if (ret && ret.length > 0) {
                        for (var i = 0; i < ret.length; i++) {
                            $('#DescriptionId').append('<option value="' + ret[i].id + '">' + ret[i].name + '</option>');
                        }

                    }

                }, function (xhr) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                    btnLoading($btn, false);
                });
        };

        $scope.openModalHuyPhieuThu = function (Amount, ReceiptVoucherId) {
            var receiptType = "phiếu thu";
            if (Amount < 0) {
                receiptType = "phiếu chi";
            }

            var header = "Xác nhận hủy " + receiptType;
            var content = "Bạn có chắc chắn muốn hủy " + receiptType + " này không?";
            var strSubmitFunc = "xacNhanHuyPhieu('" + ReceiptVoucherId + "')";
            var btnText = "Có";
            ISD.CreateModal(header, content, strSubmitFunc, btnText);
        };

        $scope.xacNhanHuyPhieu = function (ReceiptVoucherId) {
            var $btn = getTarget();
            btnLoading($btn, true);

            const postData = {
                ReceiptVoucherId: ReceiptVoucherId
            };
            
            isdPost("/Accountancy/ReceiptVoucher/DestroySaleOrderReceipt", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.Success === true) {
                        btnLoading($btn, false);
                        ISD.ClearModal();
                        location.reload();
                        ISD.alertPopup(true, ret.Message);

                        //window.location.href = location.pathname + "?message=" + ret.Message;

                    } else {
                        ISD.ClearModal();
                        alertPopup(false, ret.Message);
                    }
                }, function (xhr) {
                    console.log(xhr);
                    btnLoading($btn, false);
                    ISD.ClearModal();
                    alertPopup(false, xhr.responseText);
                });
        }

        $scope.init = function (viewModel, accountCodeList, organizationList) {
            $scope.viewModel = angular.extend({}, viewModel); // Lưu lại viewModel load từ server để lát post lên, ko post nguyên $scope
            //$scope.viewModel = viewModel;
            angular.extend($scope, viewModel); // Gán tất cả viewmodel từ server vào context của AngularJS (input đã có sẵn ng-model)
            //angular.extend($scope, accountCodeList); // Gán tất cả viewmodel từ server vào context của AngularJS (input đã có sẵn ng-model)
            //angular.extend($scope, organizationList); // Gán tất cả viewmodel từ server vào context của AngularJS (input đã có sẵn ng-model)
            //console.log($scope);

            //$scope.AccountCodeList = @Html.Json((List<ISDSelectStringItem>)ViewBag.AccountCodeList);
            //$scope.OrganizationList = @Html.Json((List<ISDSelectStringItem>)ViewBag.OrganizationList);

            $scope.AccountCodeList = accountCodeList ? JSON.parse(accountCodeList) : [];
            $scope.OrganizationList = organizationList ? JSON.parse(organizationList) : [];


            $scope.PaymentMethod = "0";  // ở server xài int mà ở đây xài string 

            // Sử dụng: view model có gì thì ở client có cái đó

        };



        $scope.initial();
    });

})();

// HELPER TODO: REFACTOR LATER

/**
 * Select cái elemnt hiện tại của event và prevent default để ko bị submit
 * @return {selector} Selector của target
 */
function getTarget() {
    event.preventDefault(); // Xài cái này nếu là <button> vì nếu click vào nó sẽ submit
    return $(event.currentTarget);
}

function isdPost(postUrl, dataObj, success, err) {
    $.ajax({
        type: "POST",
        url: postUrl,
        data: dataObj,
        //dataType: "json",
        success: function (ret) {
            if (success) {
                success(ret);
            }

        },
        error: function (xhr) {
            if (err) {
                err(xhr);
            }
        }
    });
}
function chuyenXeLapRap() {
    var $scope = getControllerScope();
    $scope.chuyenXeLapRap();
}

function chayHoSo() {
    var $scope = getControllerScope();
    $scope.chayHoSo();
}

function xacNhanHuyPhieu(ReceiptVoucherId) {
    var $scope = getControllerScope();
    $scope.xacNhanHuyPhieu(ReceiptVoucherId);
}

function isdPostMvc(postUrl, dataObj, success, err) {
    $.ajax({
        type: "POST",
        url: postUrl,
        data: dataObj,
        processData: false,
        contentType: false,
        success: function (ret) {
            if (success) {
                success(ret);
            }

        },
        error: function (xhr, status, error) {
            if (err) {
                err(xhr);
            }
        }
    });
}


function btnLoading($target, loading) {

    var oldData = $target.html();

    if (loading) {
        $target.button('loading'); // Bootstrap 3: Disables the button and changes the button text to "loading..."
        $target.prop("disabled", true);
    } else {
        $target.button('reset');
        $target.prop("disabled", false);
        //$target.removeProp("disabled");
    }
}

function getControllerScope() {
    var sel = $("#accountancyController");
    return angular.element(sel).scope();
}