

var config = {
    pageSize: parseInt($('#changeViewRow').val()),
    pageIndex: 1
};
var configMember = {
    pageSize: $('#changeViewRowMember').val(),
    pageIndex: 1
};
var configMemberExternal = {
    pageSize: $('#changeViewRowMemberExternal').val(),
    pageIndex: 1
};

var Type = $('#Type').val();

String.prototype.toCamelCase = function (str) {
    return str
        .replace(/\s(.)/g, function ($1) { return $1.toUpperCase(); })
        .replace(/\s/g, '')
        .replace(/^(.)/, function ($1) { return $1.toLowerCase(); });
};
var ISDNET5 = {
    init: function () {
        ISDNET5.registerEvent();
    },
    registerEvent: function () {
        $('#btn-search').off('click').on('click', function () {
            var typeSearch = $('#btn-search').data('searchtype');
            var controller = $('#btn-search').data('controller');
            ISDNET5.initSearch(controller, typeSearch, true);
        });
        //$('.btn-delete').off('click').on('click', function () {
        //    ISDNET5.Delete("Marketing/Unfolow", $(this).data("id"));
        //});
        $('#changeViewRow').change(function () {
            var typeSearch = $('#btn-search').data('searchtype');
            var controller = $('#btn-search').data('controller');
            config.pageSize = $('#changeViewRow').val();
            ISDNET5.initSearch(controller, typeSearch, true);
        });
        $('#changeViewRowMember').change(function () {
            configMember.pageSize = $('#changeViewRowMember').val();
            ISDNET5.loadMember(currentController, true);
        });
        $('#changeViewRowMemberExternal').change(function () {
            configMemberExternal.pageSize = $('#changeViewRowMemberExternal').val();
            ISDNET5.loadExternalMember(currentController, true);
        });
        //$('#btn-save').off('click').on('click', function () {
        //    ISDNET5.edit(true);
        //});
        //$('#btn-export').off('click').on('click', function () {
        //    ISDNET5.downloadFile();
        //});
    },
    initSearch: function (controller, searchType, changePageSize) {
        loading2()
        ISD.ProgressBar.showPleaseWait();
        var url = net5apidomain + 'api/' + controller + 's/Search?';
        var data1 = $("#frmSearch").serializeArray();
        $.each(data1, function (i, val) {
            if (i === 0) {
                url = url + val.name + '=' + val.value;
            }
            else {
                url = url + '&' + val.name + '=' + val.value;
            }
        });
        url = url + '&pageIndex=' + config.pageIndex + '&pageSize=' + config.pageSize;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.isSuccess) {
                    var data = res.data.list;
                    $('#tblData').html("");
                    switch (searchType) {
                        case "TargetGroup":
                            ISDNET5.targetGroupRender(data);
                            break;
                        case "Member":
                            ISDNET5.memberRender(data);
                            break;
                        case "Content":
                            ISDNET5.contentRender(data);
                            break;
                        case "Campaign":
                            ISDNET5.campaignRender(data);
                            break;
                        case "Unfollow":
                            ISDNET5.unfollowRender(data);
                            break;
                        case "QuestionBank":
                            ISDNET5.questionRender(data);
                            break;
                        case "FavoriteReport":
                            ISDNET5.favoriteReportRender(data);
                            break;
                        case "StockTransferRequest":
                            ISDNET5.StockTransferRequestRender(data);
                            break;
                        case "TemplateAndGiftTargetGroup":
                            ISDNET5.TemplateAndGiftTargetGroupRender(data);
                            break;
                        case "TemplateAndGiftCampaign":
                            ISDNET5.TemplateAndGiftCampaignRender(data);
                            break;
                        default:
                    }
                    ISDNET5.paging(res.data.totalRow, function () {
                        ISDNET5.initSearch(controller, searchType, false);
                    }, config.pageSize, changePageSize);
                    if (res.data.totalRow === 0) {
                        $('#tblCount').html("Không tìm thấy dòng nào phù hợp");
                    }
                    else {
                        var rowInPage = parseInt(Object.keys(data).length);
                        var count = "Đang xem " + (config.pageIndex * config.pageSize - config.pageSize + 1) + " đến " + (((config.pageIndex - 1) * config.pageSize) + rowInPage) + " trong tổng số " + res.data.totalRow + " mục"
                        $('#tblCount').html(count);
                    }

                }
                else {
                    alertPopup(false, res.message);
                }
            },
            error: function (res) {
                alertPopup(false, res.responseJSON.message);
            }
        });
    },
    favoriteReportRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                PageUrl: item.pageUrl,
                //IsFavorite: item.isFavorite == true ? "<input class='star' type='checkbox'  checked>" : "<input class='star' type='checkbox'>",
                IsFavorite: item.isFavorite === true ? "<i data-item-name='" + item.reportName + "' data-isfavorite='true' data-pageId='" + item.pageId + "' style='color:orange;font-size:25px' class=\"fa fa-star star\"></i>" : "<i data-item-name='" + item.reportName + "' data-isfavorite='false' data-pageId='" + item.pageId + "' style='color:#333333;font-size:24px' class=\"fa fa-star-o star\"></i>",
                ReportName: item.reportName
            });
        });
        $('#tblData').html(html);
    },
    targetGroupRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                Id: item.id,
                TargetGroupCode: item.targetGroupCode,
                TargetGroupName: item.targetGroupName,
                CreateBy: item.createBy,
                InternalCustomerQuantity: item.internalCustomerQuantity,
                ExternalCustomerQuantity: item.externalCustomerQuantity,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A'),
                Actived: item.actived === true ? "<i class=\"fa fa-check true-icon\"></i>" : " <i class=\"fa fa-close false-icon\"></i>"
            });
        });
        $('#tblData').html(html);
    },
    questionRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                QuestionBankCode: item.questionBankCode,
                Question: item.question,
                Answer: item.answer,
                AnswerC: item.answerC,
                AnswerB: item.answerB,
                CreateBy: item.createBy,
                Id: item.id,
                Actived: item.actived,
                QuestionCategoryName: item.questionCategoryName,
                DepartmentName: item.departmentName,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A')
            });
        });
        $('#tblData').html(html);
    },
    unfollowRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                Id: item.id,
                Email: item.email
            });
        });
        $('#tblData').html(html);
    },
    contentRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                Id: item.id,
                ContentCode: item.contentCode,
                ContentName: item.contentName,
                CreateBy: item.createBy,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A'),
                Actived: item.actived === true ? "<i class=\"fa fa-check true-icon\"></i>" : " <i class=\"fa fa-close false-icon\"></i>"
            });
        });
        $('#tblData').html(html);
    },
    campaignRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                Id: item.id,
                CampaignCode: item.campaignCode,
                CampaignName: item.campaignName,
                ContentName: item.contentName,
                ContentId: item.contentId,
                TargetGroupName: item.targetGroupName,
                TargetGroupId: item.targetGroupId,
                StatusName: item.statusName,
                ScheduledToStart: moment(item.scheduledToStart).format('DD/MM/YYYY hh:mm:ss A'),
                CreateBy: item.createBy,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A')
            });
        });
        $('#tblData').html(html);
    },
    StockTransferRequestRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: (config.pageIndex - 1) * config.pageSize + i + 1,
                Id: item.id,
                StockTransferRequestCode: item.stockTransferRequestCode,
                StoreName: item.storeName,
                CompanyName: item.companyName,
                FromStockName: item.fromStockName,
                ToStockName: item.toStockName,
                DocumentDate: moment(item.documentDate).format('DD/MM/YYYY'),
                Note: item.note,
                Actived: item.actived === true ? "Đang mở" : "Đã đóng",
                CreateBy: item.createByName,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A')
            });
        });
        $('#tblData').html(html);
    },
    TemplateAndGiftTargetGroupRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: i + 1,
                Id: item.id,
                TargetGroupCode: item.targetGroupCode,
                TargetGroupName: item.targetGroupName,
                CreateBy: item.createBy,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A'),
                Actived: item.actived === true ? "<i class=\"fa fa-check true-icon\"></i>" : " <i class=\"fa fa-close false-icon\"></i>"
            });
        });
        $('#tblData').html(html);
    },
    TemplateAndGiftCampaignRender: function (data) {
        var html = '';
        var template = $('#data-template').html();
        $.each(data, function (i, item) {
            html += Mustache.render(template, {
                i: i + 1,
                Id: item.id,
                TemplateAndGiftCampaignCode: item.templateAndGiftCampaignCode,
                TemplateAndGiftCampaignName: item.templateAndGiftCampaignName,
                CreateBy: item.createByName,
                CreateTime: moment(item.createTime).format('DD/MM/YYYY hh:mm:ss A'),
                TemplateAndGiftTargetGroupId: item.templateAndGiftTargetGroupId,
                TemplateAndGiftTargetGroupName: item.templateAndGiftTargetGroupName
            });
        });
        $('#tblData').html(html);
    },
    paging: function (totalRow, callback, pageSize, changePageSize) {
        if (totalRow === 0) {
            return;
        }
        var totalPage = Math.ceil(totalRow / pageSize);
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: '<span aria-hidden="true">&laquo;</span>',
            next: '',
            prev: '',
            last: '<span aria-hidden="true">&raquo;</span>',
            visiblePages: 10,
            onPageClick: function (event, page) {
                config.pageIndex = page;
                configMember.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    pagingExternalMember: function (totalRow, callback, pageSize, changePageSize) {
        if (totalRow === 0) {
            return;
        }
        var totalPage = Math.ceil(totalRow / pageSize);
        if ($('#pagination2 a').length === 0 || changePageSize === true) {
            $('#pagination2').empty();
            $('#pagination2').removeData("twbs-pagination");
            $('#pagination2').unbind("page");
        }
        $('#pagination2').twbsPagination({
            totalPages: totalPage,
            first: '<span aria-hidden="true">&laquo;</span>',
            next: '',
            prev: '',
            last: '<span aria-hidden="true">&raquo;</span>',
            visiblePages: 10,
            onPageClick: function (event, page) {
                configMemberExternal.pageIndex = page;

                setTimeout(callback, 200);
            }
        });
    },
    loadMember: function (controller, changePageSize) {
        loading2();
        var id = $("#Id").val();
        var hasEmail = ('true' === $('#hasEmail').val());
        var distinctEmail = ('true' === $('#distinctEmail').val());
        var url = net5apidomain + 'api/' + controller + 's/Members/' + id + '?';
        //var url = 'https://localhost:44367/' + 'api/Marketing/TargetGroups/Members/' + id + '?';
        url = url + '&hasEmail=' + hasEmail + '&distinctEmail=' + distinctEmail + '&pageIndex=' + configMember.pageIndex + '&pageSize=' + configMember.pageSize;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.isSuccess) {
                    if (res.data.totalRow !== 0) {
                        var data = res.data.list;
                        var html = '';
                        var template = $('#data-template').html();
                        $('#tblData').html("");
                        $('#totalMember').html("(" + res.data.totalRow + ")");
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                i: (configMember.pageIndex - 1) * configMember.pageSize + i + 1,
                                ProfileId: item.profileId,
                                ProfileCode: item.profileCode === -1 ? "" : item.profileCode,
                                ProfileForeignCode: item.profileForeignCode,
                                ProfileName: item.profileName,
                                Phone: item.phone,
                                Email: item.email,
                                Address: item.address,
                            });
                        });
                        $('#tblData').html(html);
                        ISDNET5.paging(res.data.totalRow, function () {
                            ISDNET5.loadMember(controller, false);
                        }, configMember.pageSize, changePageSize);
                        var rowInPage = parseInt(Object.keys(data).length);
                        var count = "Đang xem " + (configMember.pageIndex * configMember.pageSize - configMember.pageSize + 1) + " đến " + (((configMember.pageIndex - 1) * configMember.pageSize) + rowInPage) + " trong tổng số " + res.data.totalRow + " mục"
                        $('#tblCount').html(count);
                    }
                }
                else {
                    alertPopup(false, res.responseJSON.message);
                }
            },
            error: function (res) {
                alertPopup(false, res.responseJSON.message);
            }
        });
    },
    loadExternalMember: function (controller, changePageSize) {
        loading2();
        var id = $("#Id").val();
        var url = net5apidomain + 'api/' + controller + 's/ExternalMembers/' + id + '?';
        //var url = 'https://localhost:44367/' + 'api/Marketing/TargetGroups/Members/' + id + '?';
        url = url + 'pageIndex=' + configMemberExternal.pageIndex + '&pageSize=' + configMemberExternal.pageSize;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.isSuccess) {
                    if (res.data.totalRow !== 0) {
                        var data = res.data.list;
                        var html = '';
                        var template = $('#data-template2').html();
                        $('#tblData2').html("");
                        $('#totalMember2').html("(" + res.data.totalRow + ")");
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                i: (configMemberExternal.pageIndex - 1) * configMemberExternal.pageSize + i + 1,
                                ProfileId: item.profileId,
                                ProfileCode: item.profileCode === -1 ? "" : item.profileCode,
                                ProfileForeignCode: item.profileForeignCode,
                                ProfileName: item.profileName,
                                Email: item.email,
                                Phone: item.phone,
                                Address: item.address,
                            });
                        });
                        $('#tblData2').html(html);
                        ISDNET5.pagingExternalMember(res.data.totalRow, function () {
                            ISDNET5.loadExternalMember(controller, false);
                        }, configMemberExternal.pageSize, changePageSize);
                        var rowInPage = parseInt(Object.keys(data).length);
                        var count = "Đang xem " + (configMemberExternal.pageIndex * configMemberExternal.pageSize - configMemberExternal.pageSize + 1) + " đến " + (((configMemberExternal.pageIndex - 1) * configMemberExternal.pageSize) + rowInPage) + " trong tổng số " + res.data.totalRow + " mục"
                        $('#tblCount2').html(count);
                    }
                }
                else {
                    alertPopup(false, res.responseJSON.message);
                }
            },
            error: function (res) {
                alertPopup(false, res.responseJSON.message);
            }
        });
    },
    downloadFile: function () {
        loading2();
        var url = net5apidomain + 'api/Marketing/TargetGroups/Members/Download';
        $.ajax({
            url: url,
            type: 'GET',
            success: function () {
                window.location.href = url;
            }
        });
    },
    hasEmail: function () {
        var hasEmail = ('true' === $('#hasEmail').val());
        if (hasEmail) {
            $('#hasEmail').val(false);
            ISDNET5.loadMember(currentController, true);
        }
        else {
            $('#hasEmail').val(true);
            ISDNET5.loadMember(currentController, true);
        }

    },
    distinctEmail: function () {
        var distinctEmail = ('true' === $('#distinctEmail').val());
        if (distinctEmail) {
            $('#distinctEmail').val(false);
            ISDNET5.loadMember(currentController, true);
        }
        else {
            $('#distinctEmail').val(true);
            ISDNET5.loadMember(currentController, true);
        }

    }
};
ISDNET5.CreateInitial = function (controller) {
    var saveToApiUrl = net5apidomain + 'api/' + controller + "s";
    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        var isToEditMode = false;
        ISDNET5.SaveData(saveToApiUrl, controller, Type, "#frmCreate", "POST", this, isContinue, isToEditMode, "CREATE");
    });
    $(document).on("click", "#btn-save-edit", function () {
        var isContinue = true;
        var isToEditMode = true;
        ISDNET5.SaveData(saveToApiUrl, controller, Type, "#frmCreate", "POST", this, isContinue, isToEditMode, "CREATE");
    });
};
ISDNET5.Delete = function (controller, id) {
    loading2();
    var url = net5apidomain + 'api/' + controller + "s/" + id;
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                alertModalPopup(true, res.message);
                ISDNET5.initSearch(controller, "Unfollow", false);
            }
        }
    });
};
ISDNET5.LoadSaleOrg = function (selectSaleOrg) {
    loading2();
    var url = net5apidomain + "api/Marketing/Contents/Stores";
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var storeHtml = '';
                $.each(res.data, function (i, item) {

                    $('#SaleOrg').append(new Option(item.storeName, item.storeName));

                    //     storeHtml = storeHtml + '<option value="' + item.storeName + '" data-id="' + item.storeName + '" > ';

                });
                //     $("SaleOrg").val("val2");
                //  $('#stores').html(storeHtml);
            }
            if (selectSaleOrg !== null && selectSaleOrg !== "" && selectSaleOrg !== undefined) {
                $('#SaleOrg').val(selectSaleOrg).change();
            }
        }
    });
};
ISDNET5.LoadQuestionCategory = function (id) {
    loading2();
    var url = net5apidomain + "api/MasterData/QuestionBanks/QuestionCategory";
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {

                $.each(res.data, function (i, item) {

                    $('#QuestionCategoryId').append(new Option(item.questionCategoryName, item.questionCategoryId));

                });
            }
            if (id !== null) {
                $('#QuestionCategoryId').val(id).change();
            }
        }
    });
}
ISDNET5.LoadDepartment = function (id) {
    loading2();
    var url = net5apidomain + "api/MasterData/QuestionBanks/Department";
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {

                $.each(res.data, function (i, item) {

                    $('#DepartmentId').append(new Option(item.departmentName, item.departmentId));

                });
            }
            if (id !== null) {
                $('#DepartmentId').val(id).change();
            }
        }
    });
}
ISDNET5.LoadEmailAccount = function () {
    loading2();
    var url = net5apidomain + "api/Marketing/Contents/EmailAccounts";
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var emailAccountHtml = '';
                $.each(res.data, function (i, item) {
                    // emailAccountHtml = emailAccountHtml + '<option value="' + item.address + '" data-id="' + item.id + '" > ';
                    $('#FromEmailAccountId').append(new Option(item.address, item.id));
                    $('#Domain').append(new Option(item.domain, item.id));
                });
                //html(emailAccountHtml);
            }
        }
    });
}
function LoadTemplateAndGiftTargetGroup(id) {
    loading2();
    $.ajax({
        url: net5apidomain + "api/Marketing/TemplateAndGiftTargetGroups",
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                $.each(res.data, function (i, item) {
                    $('#TemplateAndGiftTargetGroupId').append(new Option(item.targetGroupName, item.id));
                });
            }
            if (id !== null) {
                $('#TemplateAndGiftTargetGroupId').val(id).change();
            }

        }

    });
}
ISDNET5.GetCampaignReportById = function (id) {
    loading2();
    var url = net5apidomain + "api/Marketing/Campaigns/Report/" + id;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var listLabel = [];
                var data = [];
                $("#report-label p").each(function () {
                    listLabel.push($(this).text());
                });
                $.each(res.data, function (i, item) {
                    data.push(item);
                });
                var ctx = document.getElementById('myChart');
                Chart.defaults.global.defaultFontSize = 14;
                Chart.defaults.global.defaultFontFamily = "'Arial', sans-serif";
                Chart.defaults.global.defaultFontColor = '#444444';
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: listLabel,
                        datasets: [{
                            label: '',
                            data: data,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        },
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        legend: { display: false },
                        title: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true,
                                    stepSize: 100
                                }
                            }]
                        }
                    }
                });
            }
        }
    });
}
ISDNET5.LoadContent = function (id) {
    loading2();
    var url = net5apidomain + "api/Marketing/Contents?Type="+Type;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var html = '';
                $.each(res.data, function (i, item) {
                    // html = html + '<option value="' + item.contentName + '" data-id="' + item.id + '" > ';
                    $('#ContentId').append(new Option(item.contentName, item.id));
                });
                //  $('#contents').html(html);
                //console.log($("#ContentId").val());
            }
            if (id !== null && id !== "" && id !== undefined) {
                $('#ContentId').val(id).change();
            }
        }

    });
}
ISDNET5.LoadTargetGroup = function (id) {
    loading2();
    var url = net5apidomain + "api/Marketing/TargetGroups?Type=" + Type;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var html = '';
                $.each(res.data, function (i, item) {
                    //      html = html + '<option value="' + item.targetGroupName + '" data-id="' + item.id + '" > ';
                    $('#TargetGroupId').append(new Option(item.targetGroupName, item.id));
                });
                //  $('#targetGroups').html(html);
            }
            if (id !== null && id !== "" && id !== undefined) {
                $('#TargetGroupId').val(id).change();
            }
        }
    });
}
ISDNET5.LoadCampaignStatus = function (id) {
    loading2();
    var url = net5apidomain + "api/Marketing/Campaigns/Status";
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                var html = '';
                $.each(res.data, function (i, item) {
                    //   html = html + '<option value="' + item.catalogTextVi + '" data-id="' + item.catalogId + '" > ';
                    $('#Status').append(new Option(item.catalogTextVi, item.catalogId));
                });
                //      $('#status').html(html);
            }
            if (id !== null && id !== "" && id !== undefined) {
                $('#Status').val(id).change();
            }

        }
    });
}
ISDNET5.LoadFavoriteReport = function (accountId) {
    loading2();
    var url = net5apidomain + "api/MasterData/FavoriteReports/" + accountId;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data.list;
                $('#tblFavoriteReport').html("");
                var html = '';
                var template = $('#data-template').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        i: i + 1,
                        PageUrl: item.pageUrl,
                        //IsFavorite: item.isFavorite == true ? "<input class='star' type='checkbox'  checked>" : "<input class='star' type='checkbox'>",
                        IsFavorite:"<i data-item-name='" + item.reportName + "' data-isfavorite='true' data-pageId='" + item.pageId + "' style='color:orange;font-size:25px' class=\"fa fa-star star\"></i>",
                        ReportName: item.reportName
                    });
                });
                $('#tblFavoriteReport').html(html);
                if (data.length === 0) {
                    $('#tblFavoriteReportCount').html("Không có dữ liệu");
                }
                else {
                    $('#tblFavoriteReportCount').html("");
                }

            }
            else {
                alertPopup(false, res.message);
            }
        },
        error: function (res) {
            alertPopup(false, res.responseJSON.message);
        }
    });
}
ISDNET5.LoadQuestionForPopup = function (id) {
    loading2();
    var getByIdUrl = net5apidomain + 'api/MasterData/QuestionBanks/' + id;
    $.ajax({
        url: getByIdUrl,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data;
                var form = $("#frmEdit");
                var disabled = form.find(':input:disabled').removeAttr('disabled');
                var serialized = form.serializeArray();
                disabled.attr('disabled', 'disabled');
                $.each(serialized, function (i, val) {
                    $('#frmEdit #' + val.name).val(data[String.prototype.toCamelCase(val.name)]);
                });

                if (data['questionCategoryId'] !== null) {
                    //  $('#TargetGroupId').val(data['targetGroupId']).change();
                    ISDNET5.LoadQuestionCategory(data['questionCategoryId']);
                }
                var createBy = ' Được tạo bởi ' + data.createBy + ' vào lúc ' + moment(data.createTime).format('DD/MM/YYYY hh:mm:ss A');
                var lastEditBy = 'Cập nhật lần cuối bởi ' + data.lastEditBy + ' vào lúc ' + moment(data.lastEditTime).format('DD/MM/YYYY hh:mm:ss A');
                $('#createBy').html(createBy);
                $('#lastEditBy').html(lastEditBy);
            }
            else {
                alertPopup(false, res.message);
            }
        }
    });
};
ISDNET5.EditInitial = function (controller) {
    loading2();
    var id = $('#Id').val();
    var type = null;
    var getByIdUrl = net5apidomain + 'api/' + controller + "s/" + id;
    var saveToApiUrl = net5apidomain + 'api/' + controller + "s/" + id;
    $.ajax({
        url: getByIdUrl,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess) {
                var data = res.data;
                var form = $("#frmEdit");
                var disabled = form.find(':input:disabled').removeAttr('disabled');
                var serialized = form.serializeArray();
                disabled.attr('disabled', 'disabled');
                $.each(serialized, function (i, val) {
                    if (val.name === 'actived') {
                        if (data[String.prototype.toCamelCase(val.name)] !== true && data[String.prototype.toCamelCase(val.name)] !== false) {
                            $('#' + val.name).val(data[String.prototype.toCamelCase(val.name)]);
                        }
                        if (data[String.prototype.toCamelCase(val.name)] === true) {
                            $("input[name=Actived][value='True']").prop("checked", true);
                        }
                        else {
                            $("input[name=Actived][value='False']").prop("checked", true);
                        }
                    }
                    else if (val.name === 'CatalogCode') {
                        if (data[String.prototype.toCamelCase(val.name)] !== 'Email' && data[String.prototype.toCamelCase(val.name)] !== 'SMS') {
                            $("input[type=radio][name=CatalogCode][value='Email']").prop("checked", true);
                        }
                        if (data[String.prototype.toCamelCase(val.name)] === 'Email') {
                            $("input[type=radio][name=CatalogCode][value='Email']").prop("checked", true);
                        }
                        else {
                            $("input[type=radio][name=CatalogCode][value='SMS']").prop("checked", true);
                        }
                        $('input[type="radio"][name="CatalogCode"]:checked').trigger("click");
                    }
                    else if (val.name === 'EmailType' && data[String.prototype.toCamelCase(val.name)] !== '' && data[String.prototype.toCamelCase(val.name)] !== undefined && data[String.prototype.toCamelCase(val.name)] !== null ) {
                        if (data[String.prototype.toCamelCase(val.name)] !== 'DefaultEmail' && data[String.prototype.toCamelCase(val.name)] !== 'PersonalEmail') {
                            $("input[type=radio][name=EmailType][value='DefaultEmail']").prop("checked", true);
                        }
                        if (data[String.prototype.toCamelCase(val.name)] === 'DefaultEmail') {
                            $("input[type=radio][name=EmailType][value='DefaultEmail']").prop("checked", true);
                        }
                        else {
                            $("input[type=radio][name=EmailType][value='PersonalEmail']").prop("checked", true);
                        }
                        $('input[type="radio"][name="EmailType"]:checked').trigger("click");

                    }
                    else {
                        
                        //else {
                        $('#' + val.name).val(data[String.prototype.toCamelCase(val.name)]);
                        //}
                        if (val.name == "Content") {
                            
                            setTimeout(function () {
                                $('#' + val.name).val(data[String.prototype.toCamelCase(val.name)]);
                                $('#Content').trigger("change");
                            }, 500);
                        }
                        else {
                            $('#' + val.name).val(data[String.prototype.toCamelCase(val.name)]);
                        }
                       
                    }
                });
                if (data['saleOrg'] !== null && data['saleOrg'] !== "" && data['saleOrg'] !== undefined) {
                     $('#SaleOrg').val(data['saleOrg']).change();
                   // ISDNET5.LoadSaleOrg(data['saleOrg']);
                }

                //if (data['status'] != null) {
                //   // $('#Status').val(data['status']).change();
                //    ISDNET5.LoadCampaignStatus(data['status']);
                //}
                if (data['contentId'] !== null && data['contentId'] !== "" && data['contentId'] !== undefined) {
                    //console.log($("#ContentId").val());
                    ISDNET5.LoadContent(data['contentId']);
                    //$('#ContentId').val(data['contentId']).change();
                }
                if (data['companyCode'] !== null && data['companyCode'] !== "" && data['companyCode'] !== undefined) {
                    $('#CompanyCode').val(data['companyCode']).change();
                }
                if (data['targetGroupId'] !== null && data['targetGroupId'] !== "" && data['targetGroupId'] !== undefined) {
                    //  $('#TargetGroupId').val(data['targetGroupId']).change();
                    ISDNET5.LoadTargetGroup(data['targetGroupId']);
                }
                if (data['questionCategoryId'] !== null && data['questionCategoryId'] !== "" && data['questionCategoryId'] !== undefined) {
                    //  $('#TargetGroupId').val(data['targetGroupId']).change();
                    ISDNET5.LoadQuestionCategory(data['questionCategoryId']);
                }
                if (data['departmentId'] !== null && data['departmentId'] !== "" && data['departmentId'] !== undefined) {
                    //  $('#TargetGroupId').val(data['targetGroupId']).change();
                    ISDNET5.LoadDepartment(data['departmentId']);
                }
                if (data['emailType'] !== null && data['emailType'] !== "" && data['emailType'] !== undefined) {
                    $('#EmailType').val(data['emailType']).change();
                }
                if (data['sentFrom'] !== null && data['sentFrom'] !== "" && data['sentFrom'] !== undefined) {
                    $('select[id="SentFrom"]').val(data['sentFrom']).change();
                }
                if (data['fromEmailAccountId'] !== null && data['fromEmailAccountId'] !== "" && data['fromEmailAccountId'] !== undefined) {
                    $('#FromEmailAccountId').val(data['fromEmailAccountId']).change();
                    $('#Domain').val(data['fromEmailAccountId']).change();
                }


                if (data['catalogCode'] !== null && data['catalogCode'] !== "" && data['catalogCode'] !== undefined) {
                    $('#CatalogCode').val(data['catalogCode']).change();
                    $('#CatalogCode').attr("disabled", "disabled");
                    //$('#CatalogCode').val(data['catalogCode']);
                }
                if (data['content'] !== null && data['content'] !== "" && data['content'] !== undefined) {
                    $('select[id="Content"]').val(data['content']).change();
                    //$('#CatalogCode').val(data['catalogCode']);
                }

                //if (data['templateAndGiftTargetGroupId'] !== null) {
                //    LoadTemplateAndGiftTargetGroup(data['templateAndGiftTargetGroupId']);
                //}


                if (data['statusName'] !== "undefined" && data['statusName'] === 'Chưa gửi') {
                    $("input[name=IsImmediately]").val(false);
                    $('#ScheduledToStart').prop('disabled', false);
                    $("#IsImmediately").prop('checked', false);
                }
                else {

                    $("input[name=IsImmediately]").val(true);
                    $("input[name=IsImmediately]").prop('disabled', true);
                    $('#ScheduledToStart').prop('disabled', true);
                    $("#IsImmediately").prop('checked', true);
                    $('#IsImmediately').prop('disabled', true);
                }

                var createBy = ' Được tạo bởi ' + data.createBy + ' vào lúc ' + moment(data.createTime).format('DD/MM/YYYY hh:mm:ss A');
                var lastEditBy = 'Cập nhật lần cuối bởi ' + data.lastEditBy + ' vào lúc ' + moment(data.lastEditTime).format('DD/MM/YYYY hh:mm:ss A');
                $('#createBy').html(createBy);
                $('#lastEditBy').html(lastEditBy);
            }
            else {
                //var resObj = JSON.parse(res);
                alertPopup(false, res.Message);
            }
        },
        error: function (res) {
            var resObj = JSON.parse(res);
            alertPopup(false, resObj.Message);
        }
    });
    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        var isToEditMode = false;
        ISDNET5.SaveData(saveToApiUrl, controller, Type, "#frmEdit", "PUT", this, isContinue, isToEditMode, "EDIT");
    });

    $(document).on("click", "#btn-save-edit", function () {
        var isContinue = true;
        var isToEditMode = true;
        ISDNET5.SaveData(saveToApiUrl, controller, Type, "#frmEdit", "PUT", this, isContinue, isToEditMode, "EDIT");
    });
};
ISDNET5.SaveData = function (saveToApiUrl, controller, type, form, httpMethod, e, isContinue, isToEditMode, actionType) {
    //var local = "https://localhost:44367/api/Marketing/Campaigns";
    var $btn = $(e);
    var frm = $(form),
        formParams = frm.serializeArray();
    if (frm.valid()) {
        loading2();
        var obj = {};
        $.each(formParams, function (i, val) {
            obj[val.name] = val.value;
        });
        if (actionType === "CREATE") {
            obj["CreateBy"] = currentUser;
        }
        else {
            obj["LastEditBy"] = currentUser;
        }
        $.ajax({
            type: "POST"/*httpMethod*/,
            url: saveToApiUrl,
            data: JSON.stringify(obj),
            processData: false,
            contentType: "application/json",
            success: function (res) {
                $btn.button('reset');
                if (res.isSuccess === true) {
                    alertPopup(true, res.message);
                    //if (actionType == "EDIT" && controller == "Marketing/TargetGroup") {               
                    //    ISDNET5.ReloadDataChange($('#Id').val())
                    //}
                    if (isContinue === true) {
                        if (isToEditMode) {
                            if (actionType == "EDIT") {
                                var id = formParams[0]["value"]
                                window.location.href = "/" + controller + "/Edit/" + id + "?Type=" + type;
                            } else {
                                window.location.href = "/" + controller + "/Edit/" + res.data.id + "?Type=" + type;
                            }
                        }
                        else {
                            window.location.href = "/" + controller + "/?Type=" + type;
                        }
                    }
                    else {
                        window.location.href = "/" + controller + "/?Type=" + type;
                    }
                }
                else {
                    var resObj = JSON.parse(res);
                    alertPopup(false, resObj.Message);
                }
            },
            error: function (res) {
                $btn.button('reset');
                var resObj = JSON.parse(res);
                alertPopup(false, resObj.Message);
            }
        });
    }
    else {
        var validator = frm.validate();
        var arr = [];
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            arr.push(value);
        });
        if (arr.length > 0) {
            alertPopup(false, arr);
        }
        $btn.button('reset');
    }
};
//ISDNET5.ReloadDataChange = function (id) {
//    $.ajax({
//        type: 'GET',
//        url: '@Url.Action("_Load", "ChangeDataLog")',
//        data: id,
//        processData: false,
//        contentType: "application/json",
//        success: function (res) {
//            $("#tab-thay-doi").html(res);
//        }
//    });
//}
ISDNET5.InitCkeditorForEmailContent = function () {
    CKEDITOR.replace('Content', {
        height: 700,
        allowedContent: true
    });
    CKEDITOR.instances.Content.on('change', function () {
        var data = CKEDITOR.instances.Content.getData();
        $('#Content').val(data);
    });
    function readTextFile(file, callback, encoding) {
        var reader = new FileReader();
        reader.addEventListener('load', function (e) {
            callback(this.result);
        });
        if (encoding) reader.readAsText(file, encoding);
        else reader.readAsText(file);
    }
    function fileChosen(input, output) {
        if (input.files && input.files[0]) {
            readTextFile(
                input.files[0],
                function (str) {
                    output.setData(str);
                    output.updateElement();
                }
            );
        }
    }
    $(document).on('change', '#addFile', function () {
        var result = $("#addFile").text();
        fileChosen(this, CKEDITOR.instances.Content);
    });
};
ISDNET5.UploadFile = function (controller) {
    $(document).on("click", "#btn-importExcel", function () {
        var $btn = $(this);
        var type = $("#btn-import-external").data("type");
        $("#btn-import-external").data("type", "");
        $btn.button('loading');
        var id = $('#Id').val();
        var url = net5apidomain + 'api/' + controller + 's/Members/' + id + '?type=' + $("#Type").val();
        if (type === "external") {
            url = net5apidomain + 'api/Marketing/TargetGroups/ExternalMembers/' + id;
        }
        //alert
        $(".modal-alert-message").html("");
        $(".modalAlert").hide();

        //$("#importexcel-window").modal("show");

        ISD.ProgressBar.showPleaseWait(); //show dialog
        var file = document.getElementById('importexcelfile').files[0];
        var formData = new FormData();
        formData.append("importexcelfile", file);
        loading2();
        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                $btn.button('reset');
                console.log(response);
                if (response.isSuccess === true) {
                    alertModalPopup(true, response.message);
                    setTimeout(function () {
                        $("#importexcelfile").val("");
                    }, 3000);
                    ISD.ProgressBar.showPleaseWait();
                    if (type === "external") {
                        ISDNET5.loadExternalMember(controller, true);
                    } else {
                        ISDNET5.loadMember(controller, true);
                    }
                    
                }
                else {
                    var resObj = JSON.parse(response);
                    alertModalPopup(false, resObj.Message);
                }
            },
            error: function (response) {
                $btn.button('reset');
                alertModalPopup(false, response);
            }
        });
    });
}
ISDNET5.Unsubscribe = function (id) {
    loading2();
    var url = net5apidomain + 'api/Marketing/Emails/Unsubscribe/' + id;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.isSuccess === true) {
                $('#unsubscribe').hide();
                $('.subtitle').hide();
                $('.title').html('Sorry to bother you!');
            }
        }
    });
};

ISDNET5.MarkFavoriteReportHandle = function (accountId,e) {
    var isfavorite = e.data('isfavorite');
    var itemName = "";
    var title = "";
    var text = "";
   // console.log(isfavorite + "=>" + !isfavorite);
    if (isfavorite === false) {
        itemName = e.data("item-name");
        $("#divDeletePopup").modal("show");
        //set title
        title = 'THÊM "' + itemName.toUpperCase() + '"';
        $("#divDeletePopup .modal-title").html(title);
        //set text
        text = 'Bạn có muốn thêm "' + '"' + itemName + '" vào danh sách báo cáo yêu thích?'
        $("#divDeletePopup .alert-message").html(text);
        $(document).off("click", "#btn-confirm-delete");
        $(document).on("click", "#btn-confirm-delete", function () {
            loading2();
            var url = net5apidomain + 'api/MasterData/FavoriteReports';
            var obj = {
                accountId : accountId,
                pageId : e.data('pageid')
            };
            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify(obj),
                processData: false,
                contentType: "application/json",
                success: function (res) {
                    if (res.isSuccess === true) {
                        isfavorite = true;
                        e.removeClass('fa-star-o').addClass('fa-star').css('color', 'orange');
                        e.data('isfavorite', isfavorite);
                        ISDNET5.LoadFavoriteReport(accountId);
                        ISDNET5.initSearch("MasterData/FavoriteReport", "FavoriteReport", false);
                    }
                }
            });
            $("#divDeletePopup").modal("hide");
        });     
    }
    else {
        itemName = e.data("item-name");
        $("#divDeletePopup").modal("show");
        //set title
        title = 'XÓA "' + itemName.toUpperCase() + '"';
        $("#divDeletePopup .modal-title").html(title);
        //set text
        text = 'Bạn chắc chắn muốn xóa "' + '"' + itemName + '" khỏi danh sách báo cáo yêu thích?'
        $("#divDeletePopup .alert-message").html(text);
        //Show new text
        $("#divDeletePopup .alert-message").html(text);
        $(document).off("click", "#btn-confirm-delete");
        $(document).on("click", "#btn-confirm-delete", function () {
            var url = net5apidomain + 'api/MasterData/FavoriteReports/Delete';
            var obj = {
                accountId: accountId,
                pageId: e.data('pageid')
            };
            loading2();
            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify(obj),
                processData: false,
                contentType: "application/json",
                success: function (res) {
                    if (res.isSuccess === true) {
                        isfavorite = false;
                        e.removeClass('fa-star').addClass('fa-star-o').css('color', '#333333');
                        e.data('isfavorite', isfavorite);
                        ISDNET5.LoadFavoriteReport(accountId);
                        ISDNET5.initSearch("MasterData/FavoriteReport", "FavoriteReport", false);
                    }                  
                }
            });
            $("#divDeletePopup").modal("hide");
        });        
    }

}
