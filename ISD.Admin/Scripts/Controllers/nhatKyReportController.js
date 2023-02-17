app.controller('nhatKyReport', function ($scope, $sce, $timeout) {
    // Initial Data
    $scope.tangList = [];
    $scope.canList = [];

    $scope.masterMapData = [];

    $scope.listDuAn = listDuAnServer;

    $scope.reportData = [];

    $scope.totalTang = 33;
    $scope.totalCan = 22;

    $scope.selectedToaId = '';
    $scope.selectedDuAnId = '';

    // Hàm convert text để render ra HTML
    // Sử dung: <span ng-bind-html="$parent.showData(can.CategoryName)"></span>
    $scope.showData = function (text) {
        return $sce.trustAsHtml(text);
    };

    $scope.loadReport = function (toaId, duAnId, classId) {

        $scope.selectedToaId = toaId;
        $scope.selectedDuAnId = duAnId;
        $scope.selectedClassId = classId;

        console.log($scope.selectedToaId);
        console.log($scope.selectedDuAnId);
        console.log($scope.selectedClassId);

        var formData = {
            ToaId: toaId,
            ClassId: classId
        };

        loading(true);

        $.ajax({
            type: "POST",
            url: urlLoadReportByToa,
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data, status, xhr) {
                //console.log(data);
                $scope.reportData = data.Result;
                $scope.filterData = data.FilterData;
                $scope.selectedClassId = classId;

                $scope.$apply();

                loading(false);
            },
            error: function (xhr, status, error) {
                console.log(xhr, status, error);
                loading(false);
            }
        });

    };

    $scope.openReportDetail = function (tangId, canId, canName) {

        var formData = {
            DuAnId: $scope.selectedToaId,
            ToaId: $scope.selectedDuAnId,
            CanId: canId
        };

        console.log(formData);
        $("#DuAn").val($scope.selectedDuAnId);
        $("#Toa").val($scope.selectedToaId);
        $("#Tang").val(tangId);
        $("#Can").val(canId);
        $("#categoryName").html(canName);
        $("#divViewMedia").modal("show");
    };

    $scope.initial = function () {


        var mapSize = 20;




        for (var i = 1; i <= mapSize; i++) {

            var childList = [];

            for (var j = 1; j < mapSize; j++) {
                childList.push({
                    j: j
                });
            }

            $scope.masterMapData.push({
                i: i,
                childList: childList
            });
            //for (var j = 1; j <= mapSize; j++) {

            //    $scope.masterMapData.push(
            //        {
            //            i: i,
            //            j: j
            //        }
            //    );

            //}

        }

        //for (var i = 0; i < $scope.totalTang; i++) {

        //    var tang = {
        //        Id : i,
        //        Name: (i + 1000).toString()
        //    };

        //    $scope.tangList.push(tang);
        //}

        //for (var j = 0; j < $scope.totalCan; j++) {
        //    var can = {
        //        Id: i,
        //        Name: (i + 2000).toString()
        //    };
        //    $scope.canList.push(can);
        //}
    };

    $scope.removeThau = function (index, type) {


        if (type === 0) {
            $scope.diaryViewModel.ThauPhuList.splice(index, 1);
        } else {
            $scope.diaryViewModel.ThoKhoanList.splice(index, 1);
        }
        //$scope.$apply();

        // TODO
    };

    $scope.openThauSelect = function (type) {

        setTimeout(function () {
            if (!$(".with-search").data('select2')) {
                $(".with-search").select2();
            }
        }, 100);


        if (type === 0) {
            $scope.isSelectThauPhu = true;
        } else {
            $scope.isSelectThoKhoan = true;
        }
    };

    $scope.backThauSelect = function () {
        $scope.initial();
    };

    $scope.addThau = function (type) {

        $scope.thauPhuSelectedId = $("#thauPhuSelect").val();
        $scope.thoKhoanSelectedId = $("#thoKhoanSelect").val();



        if (type === 0) {
            console.log($scope.thauPhuSelectedId);

            if (!$scope.thauPhuSelectedId) {
                alert("Vui lòng chọn thầu phụ");
                return;
            }

            var selectedItem = $scope.thauPhuList.find(function (item) {
                return item.ThauPhuId === $scope.thauPhuSelectedId;
            });


            var newModel = {
                Id: selectedItem.ThauPhuId,
                Name: selectedItem.ThauPhuName,
                WorkerQuantity: $scope.workerThauPhuQuantity
            };

            if ($scope.diaryViewModel.ThauPhuList) {
                var isDuplicate = $scope.diaryViewModel.ThauPhuList.map(function (item) {
                    return item.Id;
                }).indexOf(newModel.Id);


                if (isDuplicate === -1) {
                    $scope.diaryViewModel.ThauPhuList.push(newModel);
                } else {
                    var found = $scope.diaryViewModel.ThauPhuList.filter(function (item) {
                        return item.Id === newModel.Id;
                    });

                    found[0].WorkerQuantity = newModel.WorkerQuantity;
                }


            } else {
                $scope.diaryViewModel.ThauPhuList = [];
                $scope.diaryViewModel.ThauPhuList.push(newModel);
            }


            $scope.backThauSelect();

        } else {
            console.log($scope.thoKhoanSelectedId);

            if (!$scope.thoKhoanSelectedId) {
                alert("Vui lòng chọn thợ khoán");
                return;
            }

            var selectedItem2 = $scope.thoKhoanList.find(function (item) {
                return item.ThoKhoanId === $scope.thoKhoanSelectedId;
            });


            var newModel2 = {
                Id: selectedItem2.ThoKhoanId,
                Name: selectedItem2.ThoKhoanName,
                WorkerQuantity: $scope.workerThoKhoanQuantity
            };

            if ($scope.diaryViewModel.ThoKhoanList) {
                var isDuplicate2 = $scope.diaryViewModel.ThoKhoanList.map(function (item) {
                    return item.Id;
                }).indexOf(newModel2.Id);


                if (isDuplicate2 === -1) {
                    $scope.diaryViewModel.ThoKhoanList.push(newModel2);
                } else {
                    var found2 = $scope.diaryViewModel.ThoKhoanList.filter(function (item) {
                        return item.Id === newModel2.Id;
                    });

                    found2[0].WorkerQuantity = newModel2.WorkerQuantity;
                }


            } else {
                $scope.diaryViewModel.ThoKhoanList = [];
                $scope.diaryViewModel.ThoKhoanList.push(newModel2);
            }


            $scope.backThauSelect();

        }

    };

    $scope.saveDiary = function () {
        $scope.isLoading = true;

        //type: "POST",
        //url: '@Url.Action("_EditDiaryRequestModal")',
        //data: JSON.stringify(formData),
        //contentType: "application/json; charset=utf-8",

        $.ajax({
            type: "POST",
            url: saveEditDiaryRequestUrl,
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify($scope.diaryViewModel),
            success: function (ret) {
                ////console.log(result);
                //$scope.pinList.push(result);
                //$scope.resetNewPin();
                //$scope.selectedPin = {};
                console.log(ret);

                if (ret.Success === true) {
                    window.location.href = window.location.pathname + "?message=" + ret.Data;
                } else {
                    alert(ret.Data);
                    $scope.isLoading = false;
                    $scope.$apply(); // Force the AngularJS $scope to update
                }


            },
            error: function (xhr, status, p3, p4) {
                $scope.isLoading = false;
                $scope.$apply();
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                alert(err);
            }
        });

    };


    $scope.initial();

    $scope.descriptionTimer = null;

    $scope.getDescriptionStop = function () {
        $timeout.cancel($scope.descriptionTimer);
    };


    $scope.getDescription = function (canId) {

        console.log(angular.element(event.currentTarget));

        //console.log(canId);

        var formData = {
            CanId: canId,
        };

        //loading(true);

        $scope.descriptionTimer = $timeout(function () {
            $.ajax({
                type: "POST",
                url: urlGetDescriptionByCan,
                data: JSON.stringify(formData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, status, xhr) {
                    //console.log(data);

                    //$scope.description = data;
                    var description = "";
                    if (data !== null && data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            description += "- " + data[i].CategoryName + ": " + data[i].CompletedWorkWithFormat + "\n";
                        }
                    }
                    else {
                        description = "Chưa thiết lập danh mục";
                    }
                    $scope.dynamicPopover = {
                        content: description,
                    };

                    $scope.$apply();

                    //loading(false);
                },
                error: function (xhr, status, error) {
                    console.log(xhr, status, error);
                    //loading(false);
                }
            });
        }, 300);
    };

});

function getControllerScope() {
    var sel = $("#myEditDiaryRequestController");
    return angular.element(sel).scope();
}