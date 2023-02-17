(function () {
    'use strict';

    //app.filter('mvcDate', ['$filter', $filter =>
    //    (date, format, timezone) =>
    //        date && $filter('date')(date.slice(0, 10), format, timezone)
    //]);

    //// repeat end For fix 1.1
    //// Usage <tr ng-repeat="item in Accessories" repeat-end="onEnd()">
    //app.directive("repeatEnd", function () {
    //    return {
    //        restrict: "A",
    //        link: function (scope, element, attrs) {
    //            if (scope.$last) {
    //                scope.$eval(attrs.repeatEnd);
    //            }
    //        }
    //    };
    //});

    //// <tr ng-repeat="item in Consults | reverse" ng-cloak>
    //app.filter('reverse', function () {
    //    return function (items) {
    //        return items.slice().reverse();
    //    };
    //});

    // Add attr numbers-only
    /// USAGE: <input type="text" ng-model="val" numbers-only class="form-control" />
    app.directive('numbersOnly', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    if (text) {
                        var transformedInput = text.replace(/[^0-9]/g, '');

                        if (transformedInput !== text) {
                            ngModelCtrl.$setViewValue(transformedInput);
                            ngModelCtrl.$render();
                        }
                        return transformedInput;
                    }
                    return undefined;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    });

    app.controller('xuLyDichVuController', function ($scope, $http, $injector) {

        // Fix 1.1: Select2 inside table not working
        $scope.onEnd = function () {
            $("#accessoryDetailTable .select2").remove();
            $("#accessoryDetailTable select").select2();
        };

        $scope.onEndService = function () {
            $("#serviceListTable .select2").remove();
            $("#serviceListTable select").select2({
                allowClear: true,
                placeholder: "- Chọn -"

            });
        };

        $scope.idKhan = "b7fcc934-64c3-49fe-8c14-403aede59d37";

        $scope.viewModel = {};
        $scope.test = { a: "123" };
        $scope.loading = false;

        $scope.editSelected = {};
        $scope.editServiceSelected = {};

        $scope.clonedEdit = {};
        $scope.clonedEditService = {};
        $scope.OldYeuCauPhuTung = $scope.Consults;
        $scope.NewYeuCauPhuTung = [];

        $scope.DepositTotalPrice = null;

        // Test only
        $scope.initial = function () {

            $scope.TrangThai = [
                { "TrangThai": 1, "Value": "Mới thêm" },
                { "TrangThai": 2, "Value": "Đã tạo SO" },
                { "TrangThai": 3, "Value": "Đã thu tiền" }
            ];

            $scope.SOHeader = [
                "STT", "Số phiếu SO", "Biển số xe", "Tên khách hàng", "NV Tiếp nhận", "Trạng thái", "Chức năng"
            ],

                $scope.SOData = [
                    { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Bùi Văn Sơn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                    { "SONumber": "2908", "Plate": "70H1-112.34", "FullName": "Võ Văn Việt", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Bùi Văn Sơn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                    { "SONumber": "2908", "Plate": "70H1-112.34", "FullName": "Võ Văn Việt", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Bùi Văn Sơn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                    { "SONumber": "2908", "Plate": "70H1-112.34", "FullName": "Võ Văn Việt", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Bùi Văn Sơn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                    { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                    { "SONumber": "2908", "Plate": "70H1-112.34", "FullName": "Võ Văn Việt", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                ];

            $scope.SODataKeToan = [
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
                { "SONumber": "2906", "Plate": "59E1-123.11", "FullName": "Trần Đức Trung", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 1 },
                { "SONumber": "2907", "Plate": "70E1-222.56", "FullName": "Lê Ngọc Tuấn", "UserTiepNhan": "Lê Thanh Tùng", "TrangThai": 2 },
            ];

            $scope.SO = {
                "SOMaster": {
                    "SONumber": "2906",
                    "BienSo": "70E1-222.56",
                    "SoKM": "26000",
                    "SoDienThoai": "0987123456",
                    "NgaySuaChua": "18/02/2019",
                    "HoTenKhachHang": "Lê Ngọc Tuấn",
                    "TinhThanhPho": "",
                    "HuyenXa": "",
                    "NhanVienTiepNhan": "Lê Thanh Tùng"
                },
                "SODetails": [
                    { "CongViec": "Nhớt", "PhuTung": "Nhớt", "TienPhuTung": 77000 },
                    { "CongViec": "NSD", "PhuTung": "NSD", "TienPhuTung": 215000 },
                    { "CongViec": "Ốp đầu trước", "PhuTung": "Ốp đầu trước", "TienPhuTung": 119000 },
                    { "CongViec": "Ốp đầu sau", "PhuTung": "Ốp đầu sau", "TienPhuTung": 65000 },
                    { "CongViec": "Cụm đèn pha", "PhuTung": "Cụm đèn pha", "TienPhuTung": 488000 },
                    { "CongViec": "Pass tăng sên", "PhuTung": "Pass tăng sên", "TienPhuTung": 17000 },
                    { "CongViec": "MTD", "PhuTung": "MTD", "TienPhuTung": 10000 }
                ]
            };

        };

        $scope.init = function (viewModel, DepositTotalPrice) {
            $scope.viewModel = angular.extend({}, viewModel); // Lưu lại viewModel load từ server để lát post lên, ko post nguyên $scope
            //$scope.viewModel = viewModel;
            angular.extend($scope, viewModel); // Gán tất cả viewmodel từ server vào context của AngularJS (input đã có sẵn ng-model)
            // Sử dụng: view model có gì thì ở client có cái đó
            $scope.AccessoriesMapping = angular.copy($scope.Accessories);

            if (DepositTotalPrice) {
                $scope.DepositTotalPrice = parseFloat(DepositTotalPrice);
            }
        };

        $scope.printSO = function (sonumber) {
            $("#modalPrint").modal("show");
        };

        $scope.updateYeuCauPhuTung = function () {
            console.log("updateYeuCauPhuTung");
            $scope.OldYeuCauPhuTung = $scope.Consults;
        };

        $scope.checkYeuCauPhuTung = function () {
            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                }
            };

            isdPost("/Service/ServiceOrder/CheckYeuCauPhuTung", postData,
                function (ret) {
                    $scope.Consults = ret;
                    $scope.NewYeuCauPhuTung = ret;
                    //console.log(ret);
                    $scope.$apply();
                }, function (xhr) {
                    //alertPopup(false, xhr.responseText);
                });
        };

        $scope.printThuTien = function (sonumber) {
            $("#modalPrintThuTien").modal("show");
        };

        $scope.totalPhuTung = function () {
            var total = 0;

            for (var i = 0; i < $scope.SO.SODetails.length - 1; i++) {
                total += $scope.SO.SODetails[i].TienPhuTung;
            }
        };

        $scope.openLichSuSuaChua = function () {
            //var $modalLichSu = $('#modalLichSu');
            //$modalLichSu.modal('show');
            var $btn = getTarget();
            btnLoading($btn, true);

            isdPost("/Service/ServiceOrder/_LichSuSuaChua", {},
                function (ret) {
                    var $modalLichSuContent = $("#modalLichSuContent");
                    $modalLichSuContent.html(ret).promise().then(function () {
                        $("#modalLichSu").modal("show");
                    }); // data: return PartialView();
                    btnLoading($btn, false);
                }, function (xhr) {
                    $btn.button('reset');
                    alertPopup(false, xhr.responseText);
                    btnLoading($btn, false);
                });
        };

        $scope.openNewTabThongTinXe = function () {
            //alert('openNewTabThongTinXe');
            window.open("/Service/VehicleInfo/Edit/" + $scope.VehicleInfo.VehicleId);
        };

        $scope.openModalCongViec = function () {
            var $btn = getTarget();
            btnLoading($btn, true);

            OpenModalAccessorySearch(false, 1, $scope.ServiceOrder.SaleOrg, false, "service_", "#serviceComponentContainer", function () {
                btnLoading($btn, false);
            });

            //btnLoading($btn, false);
        };

        $scope.openModalPhuTung = function () {
            var $btn = getTarget();
            btnLoading($btn, true);

            OpenModalAccessorySearch(false, 0, $scope.ServiceOrder.SaleOrg, true, "accessory_", "#accessoryComponentContainer", function () {
                btnLoading($btn, false);
            });

        };

        $scope.openModalPhuTungUrgent = function () {
            var $btn = getTarget();
            btnLoading($btn, true);

            OpenModalAccessorySearch(false, 0, $scope.ServiceOrder.SaleOrg, true, "accessoryUrgent_", "#accessoryComponentContainerUrgent", function () {
                btnLoading($btn, false);
            });

        };

        $scope.openModalEditAccessory = function (ServiceOrderDetailAccessoryId) {
            //var $modalEditAccessoryContent = $("#modalEditAccessoryContent");
            //$modalLichSuContent.html(ret).promise().then(function () {
            var findObj = $scope.Accessories.find(item => item.ServiceOrderDetailAccessoryId === ServiceOrderDetailAccessoryId);

            console.log(findObj.FixingTypeId);
            $scope.editSelected = findObj;
            console.log($scope.editSelected.FixingTypeId);
            angular.copy(findObj, $scope.clonedEdit);
            //console.log($scope.clonedEdit);
            //console.log($scope.editSelected.FixingTypeId);

            var $btn = getTarget();
            btnLoading($btn, true);

            // Fix nếu mở 1 lần mà mở thêm 1 lần nữa, select sẽ bị null
            if (!$scope.isModalEditCreated) {
                isdPost("/Service/ServiceOrder/_EditAccessory", {},
                    function (ret) {
                        var $modalLichSuContent = $("#modalEditAccessoryContent");
                        $modalLichSuContent.html(ret).promise().then(function () {
                            $("#modalEditAccessory").modal("show");
                            $scope.isModalEditCreated = true;
                        }); // data: return PartialView();
                        btnLoading($btn, false);

                    }, function (xhr) {
                        $btn.button('reset');
                        alertPopup(false, xhr.responseText);
                        btnLoading($btn, false);
                    });
            } else {
                $("#modalEditAccessory").modal("show");
                btnLoading($btn, false);
            }

            //}); // data: return PartialView();
        };

        $scope.openModalEditService = function (ServiceOrderDetailServiceId) {
            //var $modalEditAccessoryContent = $("#modalEditAccessoryContent");
            //$modalLichSuContent.html(ret).promise().then(function () {
            var findObj = $scope.Services.find(item => item.ServiceOrderDetailServiceId === ServiceOrderDetailServiceId);

            console.log(findObj);
            $scope.editServiceSelected = findObj;
            console.log($scope.editServiceSelected.FixingTypeId);
            angular.copy(findObj, $scope.clonedEditService);
            //console.log($scope.clonedEdit);
            //console.log($scope.editSelected.FixingTypeId);

            var $btn = getTarget();
            btnLoading($btn, true);

            // Fix nếu mở 1 lần mà mở thêm 1 lần nữa, select sẽ bị null
            if (!$scope.isModalEditServiceCreated) {
                isdPost("/Service/ServiceOrder/_EditService", {},
                    function (ret) {
                        var $modalLichSuContent = $("#modalEditServiceContent");
                        $modalLichSuContent.html(ret).promise().then(function () {
                            $("#modalEditService").modal("show");
                            $scope.isModalEditServiceCreated = true;
                        }); // data: return PartialView();
                        btnLoading($btn, false);

                    }, function (xhr) {
                        $btn.button('reset');
                        alertPopup(false, xhr.responseText);
                        btnLoading($btn, false);
                    });
            } else {
                $("#modalEditService").modal("show");
                btnLoading($btn, false);
            }

            //}); // data: return PartialView();
        };

        $scope.closeModal = function () {
            $("#divAccessoryConfirm_accessory_").modal("hide");
            $("#divAccessorySearch_service_").modal("hide");
            $("#divAccessoryConfirm_accessoryUrgent_").modal("hide");

            ISD.ClearModal();
        };

        $scope.addService = function (AccessoryCode, AccessoryName, AccessoryPrice) {

            console.log(AccessoryPrice);


            if (AccessoryPrice === null || AccessoryPrice === "" || AccessoryPrice === undefined) {
                //alert("Vui lòng cập nhật giá của công việc");
                //return;

                //Những dịch vụ không có giá: mặc định để giá = 0
                AccessoryPrice = 0;
            }


            var $btn = getTarget();
            btnLoading($btn, true);

            // Không cho thêm trùng
            //var findIndex = $scope.Services.findIndex(item => item.AccessoryCode === AccessoryCode);
            //if (findIndex !== -1) {
            //    alert("Công việc đã thêm vào danh sách");
            //    btnLoading($btn, false);
            //    return;
            //}

            var postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    AccessoryCode,
                    AccessoryName,
                    AccessoryPrice
                }
            };

            isdPost("/Service/ServiceOrder/AddService", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        $scope.Services.push(ret.Data);
                        $scope.$apply();
                        $scope.closeModal();
                        btnLoading($btn, false);
                        TotalServicePrice();
                        //alertPopup(true, ret.Message);
                    } else {
                        btnLoading($btn, false);
                        alertPopup(false, ret.Message);
                        $scope.closeModal();
                    }

                }, function (xhr) {
                    btnLoading($btn, false);
                    $scope.closeModal();
                    alertPopup(false, xhr.responseText);
                });

        };

        $scope.removeService = function (ServiceOrderDetailServiceId) {
            var $btn = getTarget();
            btnLoading($btn, true);
            var postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailServiceId
                }
            };

            isdPost("/Service/ServiceOrder/RemoveService", postData,
                function (ret) {
                    if (ret.IsSuccess === true) {
                        var findIndex = $scope.Services.findIndex(item => item.ServiceOrderDetailServiceId === ServiceOrderDetailServiceId);
                        $scope.Services.splice(findIndex, 1);
                        $scope.$apply();
                        TotalServicePrice();
                        //alertPopup(true, ret.Message);
                    } else {
                        alertPopup(false, ret.Message);
                    }
                    btnLoading($btn, false);

                }, function (xhr) {
                    btnLoading($btn, false);
                    alertPopup(false, xhr.responseText);
                });
        };

        $scope.totalService = function () {
            var total = 0;
            angular.forEach($scope.Services, function (item) {
                total += item.AccessoryPrice;
            });
            //gán vào hidden để tính tổng tiền => tính chiết khấu
            $("#DichVu").val(total);
            return total;
        };
        
        $scope.addAccessory = function (AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, AvailableQuantity, Unit, Location, InputQuantity, WarehouseName) {

            //Ở tab phụ tùng khẩn có thể add phụ tùng có tồn kho
            var tabPhuTungKhan = $('.tab-content').find('.active').attr('id'); 
            if (!AvailableQuantity || tabPhuTungKhan === 'tab2' || AvailableQuantity < InputQuantity) {
                //alert("Phụ tùng đã hết tồn kho");
                $scope.themPhuTungKhanConfirm(AccessoryCode, AccessoryName, AccessoryPrice, InputQuantity, AvailableQuantity);
                return;
            }


            //// Không cho thêm trùng
            //var findIndex = $scope.Accessories.findIndex(item => item.AccessoryCode === AccessoryCode);

            //if (findIndex !== -1) {
            //    alert("Phụ tùng đã thêm vào danh sách");
            //    btnLoading($btn, false);
            //    return;
            //}

            var $btn = getTarget();
            btnLoading($btn, true);

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    AccessoryCode,
                    AccessoryName,
                    AccessoryPrice,
                    WarehouseCode,
                    Quantity: InputQuantity,
                    Unit,
                    Location,
                    WarehouseName
                }
            };

            // divAccessorySearch_accessory_
            // divAccessoryConfirm_accessory_

            isdPost("/Service/ServiceOrder/AddAccessory", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        $scope.Accessories.push(ret.Data);
                        $scope.$apply();
                        $scope.closeModal();
                        TotalServicePrice();
                        //alertPopup(true, ret.Message);
                    } else {
                        $scope.closeModal();
                        alertPopup(false, ret.Message);
                    }
                    btnLoading($btn, false);
                }, function (xhr) {
                    btnLoading($btn, false);
                    $scope.closeModal();
                    alertPopup(false, xhr.responseText);
                });
        };

        $scope.addAccessoryUrgent = function (AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, AvailableQuantity, Unit, Location, InputQuantity) {

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    AccessoryCode,
                    AccessoryName,
                    AccessoryPrice,
                    WarehouseCode,
                    Quantity: InputQuantity,
                    Unit,
                    Location
                }
            };

            isdPost("/Service/ServiceOrder/AddAccessoryUrgent", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        $scope.Accessories.push(ret.Data);
                        $scope.$apply();
                        $scope.closeModal();
                        //alertPopup(true, ret.Message);
                        TotalServicePrice();
                    } else {
                        $scope.closeModal();
                        alertPopup(false, ret.Message);
                    }
                    btnLoading($btn, false);
                }, function (xhr) {
                    btnLoading($btn, false);
                    $scope.closeModal();
                    alertPopup(false, xhr.responseText);
                });
        };

        //Button "Check tồn kho"
        $scope.selectUrgent = function (ServiceOrderDetailAccessoryId) {
            $scope.selectUrgentId = ServiceOrderDetailAccessoryId;
        };

        $scope.transferAccessoryUrgent = function (AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, Quantity, Unit, Location, InputQuantity) {

            console.log(AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, Quantity, Unit, Location, InputQuantity);

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailAccessoryId: $scope.selectUrgentId,
                    AccessoryCode,
                    AccessoryName,
                    AccessoryPrice,
                    WarehouseCode,
                    Quantity: InputQuantity,
                    Unit,
                    Location
                }
            };

            isdPost("/Service/ServiceOrder/TransferAccessoryUrgent", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {

                        var findIndex = $scope.Accessories.findIndex(item => item.ServiceOrderDetailAccessoryId === $scope.selectUrgentId);
                        $scope.Accessories.splice(findIndex, 1);
                        $scope.Accessories.push(ret.Data);
                        $scope.$apply();

                        $scope.closeModal();
                        alertPopup(true, ret.Message);
                        TotalServicePrice();
                    } else {
                        $scope.closeModal();
                        alertPopup(false, ret.Message);
                    }
                }, function (xhr) {
                    $scope.closeModal();
                    alertPopup(false, xhr.responseText);
                });
        };

        $scope.removeAccessory = function (ServiceOrderDetailAccessoryId) {
            var $btn = getTarget();
            btnLoading($btn, true);
            isdPost("/Service/ServiceOrder/RemoveAccessory", {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailAccessoryId
                }
            },
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        var findIndex = $scope.Accessories.findIndex(item => item.ServiceOrderDetailAccessoryId === ServiceOrderDetailAccessoryId);

                        var findRefObj = $scope.Services.find(item => item.AccessoryIdReference === ServiceOrderDetailAccessoryId);
                        if (findRefObj) {
                            findRefObj.AccessoryIdReference = null;
                        }

                        $scope.Accessories.splice(findIndex, 1);
                        $scope.$apply();
                        TotalServicePrice();
                        //alertPopup(true, ret.Message);
                    } else {
                        alertPopup(false, ret.Message);
                    }
                    btnLoading($btn, false);
                },
                function (xhr) {
                    btnLoading($btn, false);
                    alertPopup(false, xhr.responseText);
                });
        };

        $scope.updateAccessory = function (ServiceOrderDetailAccessoryId) {
            var findObj = $scope.Accessories.find(item => item.ServiceOrderDetailAccessoryId === ServiceOrderDetailAccessoryId);

            //console.log(findObj.Quantity);
            //console.log(findObj.FixingTypeId);

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailAccessoryId,
                    Quantity: findObj.Quantity,
                    FixingTypeId: findObj.FixingTypeId,
                    Note: findObj.Note
                }
            };

            var $btn = getTarget();
            btnLoading($btn, true);

            isdPost("/Service/ServiceOrder/UpdateAccessory", postData,
                function (ret) {
                    console.log(ret);
                    //$scope.Accessories.push(ret.Data);
                    if (ret.IsSuccess === true) {
                        findObj = ret.Data;
                        $scope.clonedEdit = ret.Data; // modal cloase
                        $scope.$apply();
                        $("#modalEditAccessory").modal("hide");
                        btnLoading($btn, false);
                        TotalServicePrice();
                        //alertPopup(true, ret.Message);
                    } else {
                        $("#modalEditAccessory").modal("hide");
                        btnLoading($btn, false);
                        alertPopup(false, ret.Message);
                    }

                }, function (xhr) {
                    $("#modalEditAccessory").modal("hide");
                    btnLoading($btn, false);
                    alertPopup(false, xhr.responseText);
                });

        };

        $scope.updateService = function (ServiceOrderDetailServiceId) {
            var findObj = $scope.Services.find(item => item.ServiceOrderDetailServiceId === ServiceOrderDetailServiceId);

            //console.log(findObj.Quantity);
            //console.log(findObj.FixingTypeId);

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailServiceId,
                    FixingTypeId: findObj.FixingTypeId,
                    AccessoryPrice: findObj.AccessoryPrice,
                    Note: findObj.Note
                }
            };

            var $btn = getTarget();
            btnLoading($btn, true);

            isdPost("/Service/ServiceOrder/UpdateService", postData,
                function (ret) {
                    console.log(ret);
                    //$scope.Accessories.push(ret.Data);
                    if (ret.IsSuccess === true) {
                        findObj = ret.Data;
                        $scope.clonedEditService = ret.Data; // modal cloase
                        $scope.$apply();
                        $("#modalEditService").modal("hide");
                        btnLoading($btn, false);
                        //alertPopup(true, ret.Message);
                        TotalServicePrice();
                    } else {
                        //$("#modalEditService").modal("hide");
                        //btnLoading($btn, false);
                        //alertPopup(false, ret.Message);

                        btnLoading($btn, false);
                        $("#divModalServiceError .modal-alert-message").html("");
                        $("#divModalServiceError .modal-alert-message").html(ret.Message);
                        $('#divModalServiceError').show();
                    }

                }, function (xhr) {
                    $("#modalEditService").modal("hide");
                    btnLoading($btn, false);
                    alertPopup(false, xhr.Message);
                });

        };
        
        $scope.serviceReferenceChange = function (ServiceOrderDetailServiceId) {
            console.log("serviceReferenceChange");
            var findObj = $scope.Services.find(item => item.ServiceOrderDetailServiceId === ServiceOrderDetailServiceId);
            var findObjExists = $scope.Services.find(item => item.AccessoryIdReference === findObj.AccessoryIdReference && item.ServiceOrderDetailServiceId !== ServiceOrderDetailServiceId);
            //debugger

            //console.log();

            // Không thêm trùng
            if (findObjExists) {
                findObj.AccessoryIdReference = null;
                $("#" + ServiceOrderDetailServiceId + " select").trigger("change");
                alert("Phụ tùng đã được map vào công việc khác");
                $scope.$apply();
                return;
            }

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    ServiceOrderDetailServiceId,
                    AccessoryIdReference: findObj.AccessoryIdReference
                }
            };

            isdPost("/Service/ServiceOrder/ServiceReferenceChange", postData,
                function (ret) {
                    console.log(ret);
                    //findObj.AccessoryIdReferenceSelected = findObj.AccessoryIdReference;
                    //$scope.$apply();
                    //var findIndex = $scope.AccessoriesMapping.findIndex(item => item.ServiceOrderDetailAccessoryId === findObj.AccessoryIdReference);
                    //$scope.AccessoriesMapping.splice(findIndex, 1);
                }, function (xhr) {
                    alertPopup(false, xhr.responseText);
                });

        };
        
        $scope.accessoryServiceTypeChange = function (AccessoryCode) {
            var findObj = $scope.Accessories.find(item => item.AccessoryCode === AccessoryCode);

            //const postData = {
            //    input: {
            //        ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
            //        AccessoryCode,
            //        FixingTypeId: findObj.FixingTypeId
            //    }
            //};

            // console.log(postData);

            // isdPost("/Service/ServiceOrder/UpdateAccessoryServiceType", postData,
            //    function (ret) {
            //        console.log(ret);
            //        $scope.$apply();
            //    }, function (xhr) {
            //        $btn.button('reset');
            //        alertPopup(false, xhr.responseText);
            //    });
        };
        
        $scope.testModal = function () {
            alert(123);
            //$scope.$apply();
            $scope.History.push({
                CreateDateData: "2019-06-03T09:21:41.94",
                CreateDate: "03/06/2019",
                FixingTypeName: "Sửa chữa",
                SaleOrgName: "Gia Hòa 1",
                CompanyName: null
            });
        };

        $scope.xacNhan = function () {
            var $btn = getTarget();
            btnLoading($btn, true);

            //console.log($scope.viewModel);
            //console.log($scope.originalViewModel);

            //$http.post("/Service/ServiceOrder/Edit", $scope.viewModel)
            //    .then(function (data) {
            //        console.log(data);
            //    });

            //$http.post("/Service/ServiceOrder/Edit", $scope.viewModel)
            //    .then(function (data) {
            //        console.log(data);
            //    });
            var $myForm = $("#frmEdit");

            var formData = new FormData();
            var formParams = $myForm.serializeArray();

            $.each(formParams, function (i, val) {
                formData.append(val.name, val.value);
            });

            var uniquekey = {
                name: "ServiceOrderId",
                value: $scope.ServiceOrder.ServiceOrderId
            };
            formData.append(uniquekey.name, uniquekey.value);

            //var postData = {
            //    input: $scope.viewModel,
            //    //input: $myForm.serialize(),
            //    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId
            //};

            //for (var value of formData.values()) {
            //    console.log(value);
            //}

            //console.log(formParams);

            //console.log("$myForm.valid()", $myForm.valid());

            isdPostMvc("/Service/ServiceOrder/Edit", formData,
                function (ret) {
                    btnLoading($btn, false);
                    console.log(ret);
                    if (!ret) {
                        ISD.alertPopup(false, "Lỗi");
                    } else {
                        console.log("ret", ret);
                        //if (ret.Data.length > 0 && ret.Data[0].ErrorMessage == "" || ret.Data[0].ErrorMessage == null || ret.Data[0].ErrorMessage == undefined) {
                        //    ISD.alertPopup(true, ret.Data);
                        //}
                        //else {
                        //    ISD.alertPopup(false, ret.Data);
                        //}

                        if (ret.IsSuccess) {
                            ISD.alertPopup(true, ret.Data);
                        }
                        else {
                            ISD.alertPopup(false, ret.Message);
                        }
                    }

                }, function (xhr) {
                    btnLoading($btn, false);
                    ISD.alertPopup(false, xhr.responseText);
                });

            //$.ajax({
            //    type: "POST",
            //    url: "/Service/ServiceOrder/Edit",
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    success: function (jsonData) {
            //        console.log(jsonData.Data);
            //        //$btn.button('reset');
            //        //if (jsonData.Success === true) {
            //        //    if (isContinue === true) {
            //        //        if (jsonData.Data !== null) {
            //        //            alertPopup(true, jsonData.Data);
            //        //        }
            //        //    }
            //        //    else {
            //        //        //window.location.href = "/" + controller;
            //        //        window.location.href = "/" + controller + "?message=" + jsonData.Data;
            //        //    }
            //        //}
            //        //else {
            //        //    if (jsonData.Data != null && jsonData.Data != "") {
            //        //        alertPopup(false, jsonData.Data);
            //        //    }
            //        //}
            //    },
            //    error: function (xhr, status, error) {
            //        $btn.button('reset');
            //        alertPopup(false, xhr.responseText);
            //    }
            //});

            //$.ajax({
            //    type: "POST",
            //    url: "/Service/ServiceOrder/Edit",
            //    data: data,
            //    success: function (ret) {
            //        console.log(ret);
            //    },
            //    error: function (xhr, status, error) {
            //        alertPopup(false, xhr.responseText);
            //    }
            //});
        };

        $scope.xacNhanHoanThanhConfirm = function () {

            var header = "Xác nhận hoàn thành";
            var content = "Bạn có chắc chắn muốn xác nhận hoàn thành phiếu dịch vụ <b>" + $scope.ServiceOrder.ServiceOrderCode + "</b>?";
            var strSubmitFunc = "xacNhanHoanThanh()";
            var btnText = "Có";
            ISD.CreateModal(header, content, strSubmitFunc, btnText);
        };

        $scope.xacNhanHoanThanh = function () {

            var $btn = getTarget();
            btnLoading($btn, true);

            const postData = {
                ServiceOrderId: $scope.ServiceOrder.ServiceOrderId
            };


            isdPost("/Service/ServiceOrder/XacNhanHoanThanh", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        btnLoading($btn, false);
                        ISD.ClearModal();
                        //location.reload();
                        //ISD.alertPopup(true, ret.Message);

                        window.location.href = location.pathname + "?message=" + ret.Message;

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
        };

        $scope.huyPhieuDichVuConfirm = function () {

            var header = "Xác nhận hủy phiếu";
            //var content = "Bạn có chắc chắn muốn hủy phiếu dịch vụ <b>" + $scope.ServiceOrder.ServiceOrderCode + "</b>?";
            var content = '<div class="form-group text-center"><div class="row" style="margin-bottom:10px;">Bạn có chắc chắn muốn hủy phiếu dịch vụ <b>"' + $scope.ServiceOrder.ServiceOrderCode + '"</b>? </div> <div class="row"><div class="col-md-4 text-right"><div class="label-wrapper"><label class="control-label" for="CanceledNote">Lý do hủy</label></div></div><div class="col-md-6"><textarea name="CanceledNote" style="width:100%;height:70px;"></textarea></div></div></div>';
            var strSubmitFunc = "huyPhieuDichVu()";
            var btnText = "Có";
            ISD.CreateModal2(header, content, strSubmitFunc, btnText);
        };

        $scope.huyPhieuDichVu = function () {
            var $btn = getTarget();
            btnLoading($btn, true);

            const postData = {
                ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                CanceledNote: $("textarea[name='CanceledNote']").val(),
            };

            isdPost("/Service/ServiceOrder/HuyPhieuDichVu", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        btnLoading($btn, false);
                        ISD.ClearModal();
                        //location.reload();
                        ISD.alertPopup(true, ret.Message);

                        setTimeout(function () {
                            window.location = backUrl;
                        }, 1000);

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
        };

        $scope.themPhuTungKhanConfirm = function (AccessoryCode, AccessoryName, AccessoryPrice, Quantity, AvailableQuantity) {

            //var header = "Thêm phụ tùng khẩn";
            var header = "Thêm phụ tùng";
            //var content = "Phụ tùng không có trong kho. Bạn có muốn thêm phụ tùng khẩn?";
            var content = "Phụ tùng không có trong kho. Bạn muốn thêm phụ tùng khẩn hay đặt hàng phụ tùng?";
            if (AvailableQuantity && AvailableQuantity > 0) {
                content = "Bạn muốn thêm phụ tùng khẩn hay đặt hàng phụ tùng?";
            }
            var strSubmitFunc = "themPhuTungKhan('" + AccessoryCode + "', '" + AccessoryName + "', '" + AccessoryPrice + "', '" + Quantity + "')";
            var btnText = "Có";
            var radioBtn = [
                { Name: "FixingTypeId", Value: "FixingTypeUrgentGuid", Label: "Phụ tùng khẩn" },
                { Name: "FixingTypeId", Value: "FixingTypePreOrderGuid", Label: "Đặt hàng phụ tùng" },
            ];
            ISD.CreateModal(header, content, strSubmitFunc, btnText, radioBtn);
        };

        $scope.themPhuTungKhan = function (AccessoryCode, AccessoryName, AccessoryPrice, Quantity) {

            var $btn = getTarget();
            btnLoading($btn, true);

            var fixingType = $("input[name='FixingTypeId']:checked").val();

            const postData = {
                input: {
                    ServiceOrderId: $scope.ServiceOrder.ServiceOrderId,
                    AccessoryCode,
                    AccessoryName,
                    AccessoryPrice,
                    Quantity,
                    FixingType: fixingType,
                }
            };

            isdPost("/Service/ServiceOrder/AddAccessoryUrgent", postData,
                function (ret) {
                    console.log(ret);
                    if (ret.IsSuccess === true) {
                        $scope.Accessories.push(ret.Data);
                        $scope.$apply();
                        $scope.closeModal();
                        //alertPopup(true, ret.Message);
                        TotalServicePrice();
                    } else {
                        $scope.closeModal();
                        alertPopup(false, ret.Message);
                    }
                }, function (xhr) {
                    console.log(xhr);
                    btnLoading($btn, false);
                    ISD.ClearModal();
                    alertPopup(false, xhr.responseText);
                });
        };

        $scope.isHaveUrgent = function () {
            var ret = ($scope.Accessories.filter(function (item) { return item.WarehouseCode === null ? true : false; }).length) > 0;
            return ret;
        };

        $scope.isHaveDatHang = function () {
            var ret = ($scope.Accessories.filter(function (item) {
                return item.FixingTypeId.toUpperCase() === '85799C95-7D60-489F-A453-A05A9E242BDB' ? true : false;
            }).length) > 0;
            console.log("$scope.Accessories", $scope.Accessories);
            console.log("isHaveDatHang", ret);
            return ret;
        };

        $scope.totalAccessory = function () {
            var total = 0;
            //for (var i = 0; i < $scope.Services.length; i++) {
            //    var item = $scope.Services[i];
            //    total += item.AccessoryPrice;
            //}
            angular.forEach($scope.Accessories, function (item) {
                //console.log(item);
                //if (item.FixingTypeId !== $scope.idKhan) {
                if (item.WarehouseCode !== null) {
                    total += item.AccessoryPrice * item.Quantity;
                }
            });
            //console.log(total);
            //gán vào hidden để tính tổng tiền => tính chiết khấu
            $("#PhuTung").val(total);
            return total;
        };

        $scope.totalAccessoryUrgent = function () {
            var total = 0;
            //for (var i = 0; i < $scope.Services.length; i++) {
            //    var item = $scope.Services[i];
            //    total += item.AccessoryPrice;
            //}
            angular.forEach($scope.Accessories, function (item) {
                //console.log(item);
                if (item.WarehouseCode === null) {
                    total += item.AccessoryPrice * item.Quantity;
                }
            });
            //console.log(total);

            //total = total + (!isNaN(parseFloat($scope.DepositTotalPrice)) ? parseFloat($scope.DepositTotalPrice) : 0);
            $("#PhuTungKhan").val(total);
            return total;
        };

        $scope.changeServiceFixingType = function (ServiceOrderDetailServiceId) {
            var findObj = $scope.Services.find(item => item.ServiceOrderDetailServiceId === ServiceOrderDetailServiceId);

            if (findObj.FixingTypeId === '611acc27-3b2a-43a9-a3cf-b2fd1649f926') {
                $("#modalEditService div#AccessoryPrice").hide();
            }
            else {
                $("#modalEditService div#AccessoryPrice").show();
            }
        };
        
        $scope.isDuplicateAccessory = function (AccessoryCode, type) {
            var number = 0;

            // 1: Accessory
            if (type === 1) {
                $scope.Accessories.forEach(function (item, index) {
                    if (item.AccessoryCode === AccessoryCode) {
                        number += 1;
                    }
                });
            } else {
                $scope.Services.forEach(function (item, index) {
                    if (item.AccessoryCode === AccessoryCode) {
                        number += 1;
                    }
                });
            }



            return number > 1;
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
        //$target.prop("disabled", loading);
    } else {
        $target.button('reset');
        //$target.removeProp("disabled");
    }
}

function getControllerScope() {
    var sel = $("#xuLyDichVuController");
    return angular.element(sel).scope();
}

//     accessory_customAccessoryAction('PHUTUNGCHIN', 'Ốp Pô xe Airblade', '1000', '1012', '10079', 'BOO', 'PT001-0001', '666666', 'accessory_', '0')

function service_customAccessoryAction(AccessoryCode, AccessoryName, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix) {
    console.log(AccessoryCode, AccessoryName, AccessoryPrice);
    var $scope = getControllerScope();

    $scope.addService(AccessoryCode, AccessoryName, AccessoryPrice);
}

function accessory_customAccessoryAction(AccessoryCode, AccessoryName, Plant, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, index, WarehouseName) {

    var InputQuantity = parseInt($("#input-quantity-" + index).val());

    console.log(AccessoryCode, AccessoryName, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, InputQuantity, WarehouseName);
    var $scope = getControllerScope();

    $scope.addAccessory(AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, Quantity, Unit, Location, InputQuantity, WarehouseName);
}

function accessoryUrgent_customAccessoryAction(AccessoryCode, AccessoryName, Plant, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, index) {

    var InputQuantity = parseInt($("#input-quantity-" + index).val());

    console.log(AccessoryCode, AccessoryName, WarehouseCode, Quantity, Unit, Location, AccessoryPrice, PreFix, InputQuantity);
    var $scope = getControllerScope();

    $scope.transferAccessoryUrgent(AccessoryCode, AccessoryName, AccessoryPrice, WarehouseCode, Quantity, Unit, Location, InputQuantity);
}

function checkYeuCauPhuTung() {
    var $scope = getControllerScope();
    $scope.checkYeuCauPhuTung();
}

function xacNhanHoanThanh() {
    var $scope = getControllerScope();
    $scope.xacNhanHoanThanh();
}

function huyPhieuDichVu() {
    var $scope = getControllerScope();
    $scope.huyPhieuDichVu();
}

function themPhuTungKhan(AccessoryCode, AccessoryName, AccessoryPrice, Quantity) {
    var $scope = getControllerScope();
    $scope.themPhuTungKhan(AccessoryCode, AccessoryName, AccessoryPrice, Quantity);
}

function changeServiceFixingType(ServiceOrderDetailServiceId) {
    var $scope = getControllerScope();
    $scope.changeServiceFixingType(ServiceOrderDetailServiceId);
}