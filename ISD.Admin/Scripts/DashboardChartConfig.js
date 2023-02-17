// Biểu đồ phân loại khách hàng
function createChartCustomerClassification() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartCustomerClassification',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: 'My First Dataset',
                        data: returnData.dataSet,
                        backgroundColor: bgColor,
                        hoverOffset: 4
                    }]
                };

                var config = {
                    type: 'pie',
                    data: data,
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                position: 'right',
                            },
                            title: {
                                display: true,
                                text: 'Tỉ lệ khách hàng theo loại'
                            },
                            datalabels: {
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (labelValue) {
                                    return labelValue + " %";
                                },
                                color: '#FFFFFF',
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + '%';
                                    }
                                }
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartCustomerClassification'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Tỉ lệ khách hàng theo nhóm KH
function createPieChartCustomerGroup() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetPieChartCustomerGroup',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31', '#A5A5A5', '#FFC000', '#4472C4', '#70AD47',
                    '#255E91', '#9E480E', '#636363', '#997300', '#264478', '#43682B', '#7CAFDD',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: 'My First Dataset',
                        data: returnData.dataSet,
                        backgroundColor: bgColor,
                        hoverOffset: 4
                    }]
                };

                var config = {
                    type: 'pie',
                    data: data,
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                position: 'right',
                            },
                            title: {
                                display: true,
                                text: 'Tỉ lệ khách hàng theo nhóm'
                            },
                            datalabels: {
                                color: '#FFFFFF',
                                formatter: function (labelValue) {
                                    return labelValue + " %";
                                },
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + '%';
                                    }
                                }
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#pieChartCustomerGroup'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Số lượng khách hàng theo nhóm
function createBarChartCustomerGroup() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetBarChartCustomerGroup',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số lượng khách hàng",
                        data: returnData.dataSet,
                        backgroundColor: bgColor
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Số lượng khách hàng theo nhóm'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'top',
                                font: {
                                    weight: 'bold'
                                },
                                formatter(labelValue) {
                                    return labelValue.toLocaleString() + " Khách hàng";
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (Khách hàng)';
                                    }
                                }
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#barChartCustomerGroup'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Tỉ lệ khách hàng theo khu vực
function createPieChartPercentCustomerByArea() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetPieChartPercentCustomerByArea',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31', '#70AD47', '#FFC000',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: 'My First Dataset',
                        data: returnData.dataSet,
                        backgroundColor: bgColor,
                        hoverOffset: 4
                    }]
                };

                var config = {
                    type: 'pie',
                    data: data,
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                position: 'right',
                            },
                            title: {
                                display: true,
                                text: 'Tỉ lệ khách hàng theo khu vực'
                            },
                            datalabels: {
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (labelValue) {
                                    return parseFloat(labelValue).toFixed(2) + " %";
                                },
                                color: '#FFFFFF',
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + '%';
                                    }
                                }
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#pieChartPercentCustomerByArea'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Số lượng khách hàng theo khu vực
function createBarChartCustomerByArea() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetBarChartCustomerByArea',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số lượng khách hàng",
                        data: returnData.dataSet,
                        backgroundColor: bgColor
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Số lượng khách hàng theo khu vực'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'top',
                                font: {
                                    weight: 'bold'
                                },
                                formatter(labelValue) {
                                    return labelValue.toLocaleString() + " Khách hàng";
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (Khách hàng)';
                                    }
                                }
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#barChartCustomerByArea'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Số lượng khách hàng theo TOP 10 tỉnh thành
function createChartCustomerByTop10Province() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartCustomerByTop10Province',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số lượng khách hàng",
                        data: returnData.dataSet,
                        backgroundColor: bgColor
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Top 10 Tỉnh thành có Số lượng khách hàng cao nhất'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'right',
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (dataLabels, context) {
                                    var dataNumb = dataLabels;
                                    var dataName = context.chart.data.labels[context.dataIndex]
                                    return " " + dataNumb.toLocaleString() /*+ " : " + dataName*/;
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (Khách hàng)';
                                    }
                                }
                            },
                        },
                        scales: {
                            //y: {
                            //    display: false,
                            //    offset: true
                            //},
                            x: {
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 5000;
                                }
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartCustomerByTop10Province'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Số lượng KH ghé thăm Showroom tháng này và tháng trước NPP
function createChartNumberCustomerVisitSRNPP() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartNumberCustomerVisitSRNPP',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Tháng hiện tại",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[0]
                    }, {
                        label: "Tháng trước",
                        data: returnData.dataSet2,
                        backgroundColor: bgColor[1]
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Số lượng khách hàng ghé thăm NPP/SRNQ tháng này và tháng trước'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'right',
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (dataLabels, context) {
                                    var dataNumb = dataLabels;
                                    var dataName = context.chart.data.labels[context.dataIndex]
                                    return "  " + (dataNumb == 0 ? "" : dataNumb) /*+ " : " + dataName*/;
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (Khách hàng)';
                                    }
                                }
                            },
                        },
                        scales: {
                            //y: {
                            //    display: false,
                            //    offset: true
                            //},
                            x: {
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 100;
                                }
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartNumberCustomerVisitSRNPP'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}
// Số lượng KH ghé thăm Showroom tháng này và tháng trước
function createChartNumberCustomerVisitSR() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartNumberCustomerVisitSR',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Tháng hiện tại",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[0]
                    }, {
                        label: "Tháng trước",
                        data: returnData.dataSet2,
                        backgroundColor: bgColor[1]
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Số lượng khách hàng ghé thăm Showroom tháng này và tháng trước'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'right',
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (dataLabels, context) {
                                    var dataNumb = dataLabels;
                                    var dataName = context.chart.data.labels[context.dataIndex]
                                    return " " + (dataNumb == 0 ? "" : dataNumb) /*+ " : " + dataName*/;
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (Khách hàng)';
                                    }
                                }
                            },
                        },
                        scales: {
                            //y: {
                            //    display: false,
                            //    offset: true
                            //},
                            x: {
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 100;
                                }
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartNumberCustomerVisitSR'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Thị hiệu kkhách hàng đến showroom
function createChartCustomerTastes() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartCustomerTastes',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số lượt Like SP (Ghé thăm) trên CRM",
                        data: returnData.dataSet,
                        backgroundColor: bgColor
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Thị hiếu khách hàng đến Showroom'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'right',
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (dataLabels, context) {
                                    var dataNumb = dataLabels;
                                    var dataName = context.chart.data.labels[context.dataIndex]
                                    return " " + dataNumb /*+ " : " + dataName*/;
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (lượt)';
                                    }
                                }
                            },
                        },
                        scales: {
                            //y: {
                            //    display: false,
                            //    offset: true
                            //},
                            x: {
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 5;
                                }
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartCustomerTastes'), config);
            } else {
                console.log("fail chart")
            }
        }
    });
}

// Tổng hợp thị hiếu sản phẩm
function createChartStatisticLikeViewProduct() {
    $.ajax({
        type: "GET",
        url: '/Chart/GetChartStatisticLikeViewProduct',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Tổng số lượng lượt view và like",
                        data: returnData.dataSet,
                        backgroundColor: bgColor
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Tổng hợp thị hiếu sản phẩm'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'right',
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (dataLabels, context) {
                                    var dataNumb = dataLabels;
                                    var dataName = context.chart.data.labels[context.dataIndex]
                                    return " " + dataNumb /*+ " : " + dataName*/;
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.label + ': ' + tooltipItem.formattedValue + ' (lượt)';
                                    }
                                }
                            },
                        },
                        scales: {
                            //y: {
                            //    display: false,
                            //    offset: true
                            //},
                            x: {
                                afterDataLimits(scaleVlue) {
                                    scaleVlue.max += 5;
                                },
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartStatisticLikeViewProduct'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Khiếu nại lỗi sản phẩm - theo nhóm hàng
function createChartProductIssue() {
    $.ajax({
        type: "GET",
        url: '/Chart/chartProductIssue',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số ca bảo hành",
                        data: returnData.dataSetLine,
                        backgroundColor: bgColor[0],
                        order: 1,
                        yAxisID: 'left'
                    }, {
                        label: "Giá trị bảo hành",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[1],
                        borderColor: bgColor[1],
                        type: 'line',
                        yAxisID: 'right',
                        order: 0
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        interaction: {
                            mode: 'index',
                            intersect: false,
                        },
                        stacked: false,
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Tổng hợp xử lý khiếu nại theo nhóm hàng'
                            },
                            datalabels: {
                                anchor: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                align: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (labelValue, labelDataSet) {
                                    if (labelDataSet.datasetIndex == 0)
                                        return labelValue + " Ca";
                                    if (labelDataSet.datasetIndex == 1)
                                        return labelValue.toLocaleString() + " VNĐ";
                                },
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        if (tooltipItem.datasetIndex === 1)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' VNĐ';
                                        if (tooltipItem.datasetIndex === 0)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' Ca';
                                    }
                                }
                            },
                        },
                        scales: {
                            right: {
                                type: 'linear',
                                display: true,
                                position: 'right',
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 1000000;
                                },
                            },
                            left: {
                                type: 'linear',
                                display: true,
                                position: 'left',
                                grid: {
                                    display: false
                                },
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 5;
                                },
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartProductIssue'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Khiếu nại lỗi sản phẩm - theo lỗi thường gặp
function createchartProductIssue2() {
    $.ajax({
        type: "GET",
        url: '/Chart/chartProductIssue2',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số ca bảo hành",
                        data: returnData.dataSetLine,
                        backgroundColor: bgColor[0],
                        order: 1,
                        yAxisID: 'left'
                    }, {
                        label: "Giá trị bảo hành",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[1],
                        borderColor: bgColor[1],
                        type: 'line',
                        yAxisID: 'right',
                        order: 0
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        interaction: {
                            mode: 'index',
                            intersect: false,
                        },
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Tổng hợp xử lý khiếu nại theo lỗi thường gặp'
                            },
                            datalabels: {
                                anchor: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                align: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (labelValue, labelDataSet) {
                                    if (labelDataSet.datasetIndex == 0)
                                        return labelValue + " Ca";
                                    if (labelDataSet.datasetIndex == 1)
                                        return labelValue.toLocaleString() + " VNĐ";
                                },
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        if (tooltipItem.datasetIndex === 1)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' VNĐ';
                                        if (tooltipItem.datasetIndex === 0)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' Ca';
                                    }
                                }
                            },
                        },
                        scales: {
                            right: {
                                type: 'linear',
                                display: true,
                                position: 'right',
                                grid: {
                                    display: false
                                },

                                //afterDataLimits(scaleValue) {
                                //    scaleValue.max += 1000000;
                                //},
                            },
                            left: {
                                type: 'linear',
                                display: true,
                                position: 'left',
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 1;
                                },
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartProductIssue2'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Báo cáo tổng hợp điểm trưng bày
function createchartShowroom() {
    $.ajax({
        type: "GET",
        url: '/Chart/chartShowroom',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Số lượng",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[0],
                        order: 1,
                        yAxisID: 'left'
                    }, {
                        label: "Giá trị",
                        data: returnData.dataSetLine,
                        backgroundColor: bgColor[1],
                        borderColor: bgColor[1],
                        type: 'line',
                        yAxisID: 'right',
                        order: 0
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        interaction: {
                            mode: 'index',
                            intersect: false,
                        },
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Tổng hợp điểm trưng bày'
                            },
                            datalabels: {
                                color: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "#FFFFFF";
                                    }
                                },
                                anchor: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                align: function (dataSet) {
                                    if (dataSet.datasetIndex == 0) {
                                        return "center";
                                    }
                                    if (dataSet.datasetIndex == 1) {
                                        return "end";
                                    }
                                },
                                font: {
                                    weight: 'bold'
                                },
                                formatter: function (labelValue, labelDataSet) {
                                    if (labelDataSet.datasetIndex == 1)
                                        return labelValue.toLocaleString() + " VNĐ";
                                    if (labelDataSet.datasetIndex == 0)
                                        return labelValue + " Điểm";
                                },
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        if (tooltipItem.datasetIndex === 0)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' Điểm';
                                        if (tooltipItem.datasetIndex === 1)
                                            return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' VNĐ';
                                    }
                                }
                            },
                        },
                        scales: {
                            right: {
                                type: 'linear',
                                display: true,
                                position: 'right',
                                grid: {
                                    display: false
                                },
                            },
                            left: {
                                type: 'linear',
                                display: true,
                                position: 'left',
                                afterDataLimits(scaleVlue) {
                                    scaleVlue.max += 100;
                                },
                            },
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartShowroom'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}

// Báo cáo tổng hợp tồn Catalog
function createchartCatalog() {
    $.ajax({
        type: "GET",
        url: '/Chart/chartCatalog',
        success: function (returnData) {
            if (returnData.success == true) {
                var bgColor = ['#5B9BD5', '#ED7D31',];

                var data = {
                    labels: returnData.labels,
                    datasets: [{
                        label: "Tồn Kho",
                        data: returnData.dataSet,
                        backgroundColor: bgColor[0],
                        order: 1,
                    }]
                };

                var config = {
                    type: 'bar',
                    data: data,
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Tổng hợp tồn catalog'
                            },
                            datalabels: {
                                anchor: 'end',
                                align: 'end',
                                font: {
                                    weight: 'bold'
                                },
                                formatter(labelValue) {
                                    return " " + (labelValue == 0 ? "" : labelValue.toLocaleString());
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        return tooltipItem.dataset.label + ': ' + tooltipItem.formattedValue + ' Cái';
                                    }
                                }
                            },
                        },
                        scales: {
                            x: {
                                afterDataLimits(scaleValue) {
                                    scaleValue.max += 100;
                                },
                            }
                        }
                    },
                    plugins: [ChartDataLabels]
                };

                new Chart($('#chartCatalog'), config);
            } else {
                console.log("fail chart");
            }
        }
    });
}