$(document).ready(function () {
    doReady();
    GetList(1);
    function bindPie(dataList, id) {
        var testPie = echarts.init(document.getElementById(id));
        testPie.showLoading({
            text: '正在努力的读取数据中...'
        });
        var option = setOptionPie(dataList);
        testPie.hideLoading();
        testPie.setOption(option);
    }
    function setOptionPie(data) {
        var legend_data = [];
        if (data && data.length > 0) {
            $.each(data, function (idx, d) {
                legend_data.push(d.name);
            });
        }
        var option = {
            title: {
                text: data.title || '',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            toolbox: {
                show: true,
                feature: {
                    magicType: {
                        show: true,
                        type: ['pie'],
                        option: {
                            funnel: {
                                x: '50%',
                                width: '50%',
                                funnelAlign: 'left',
                                max: 1000
                            }
                        }
                    },
                    restore: { show: true }
                }
            },
            legend: {
                orient: 'vertical',
                x: 'left',
                data: legend_data
            },
            calculable: true,
            series: [
                {
                    type: 'pie',
                    radius: '50%',
                    center: ['50%', '50%'],
                    data: data,
                    itemStyle: {
                        normal: {
                            label: {
                                show: true,
                                position: 'outer',
                                formatter: "{b}\n{d}%",
                            },
                            labelLine: {
                                show: true,
                            }
                        },
                        emphasis: {
                            label: {
                                show: true,
                                formatter: "{d}%"
                            }
                        }

                    }
                }
            ]
        };
        return option;
    }
    function doReady() {
        $(".jcDate").jcDate({
            IcoClass: "jcDateIco",
            Event: "click",
            Speed: 100,
            Left: 0,
            Top: 28,
            format: "-",
            Timeout: 100
        });
        $(".EduPayInfoOperation").on("click", ".searchButton", function () {
            GetList(1);
        }).on("click", ".countStatusBtn", function () {
            CountInfo("countStatus");
        }).on("click", ".countPaymentBtn", function () {
            CountInfo("countPayment");
        }).on("click", ".countTerritoryBtn", function () {
            CountInfo("countTerritory");
        }).on("click", ".listBtn", function () {
            CountInfo("cayment");
        });
    }

    function CountInfo(type) {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var serPayment = $(".serPayment").val();
        var serStatus = $(".serStatus").val();
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduPayInfoManage.ashx";
            var postdata = {
                cmd: type, serCompany: serCompany, serIDCard: serIDCard,
                serTerritory: serTerritory, serBeginTime: serBeginTime,
                serEndTime: serEndTime, serPayment: serPayment, serStatus: serStatus
            };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    if (type == "countStatus") {
                        var id = "pieBoxStatus";
                        bindPie(dataList, id);
                        $(".list-all").hide();
                        $(".pieBox-Cayment").hide();
                        $(".pieBox-Status").show();
                        
                    } else {
                        var id = "pieBoxCayment";
                        bindPie(dataList, id);
                        $(".list-all").hide();
                        $(".pieBox-Status").hide();
                        $(".pieBox-Cayment").show();
                    }
                }
                else if (jsonData.state == "nologin") {
                    noLogin();
                }
                else {
                    alert(jsonData.msg);
                }
            }, "json");
        }
    }

    function GetList(pageIndex) {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var serPayment = $(".serPayment").val();
        var serStatus = $(".serStatus").val();
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduPayInfoManage.ashx";
            var postdata = { cmd: "getList", pagesize: $("#hdnPageSize").val(), pageindex: pageIndex, serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime, serPayment: serPayment, serStatus: serStatus };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    $("#hdnCount").attr("value", jsonData.count);
                    var addItem = ""
                    var i = 0;
                    $(dataList).each(function () {
                        var trItem = '<tr class="' + (i % 2 == 0 ? "odd-row" : "even-row") + '"><td realid="' + $(this)[0].ID + '">' + (i + 1) + '</td><td realid="' + $(this)[0].idcard + '">' + $(this)[0].EduUserName + '</td><td realname="">' + $(this)[0].company + '</td><td realter="">' + $(this)[0].territory + '</td>'
                            + '<td>' + $(this)[0].Time.split(' ')[0] + '<br/>' + $(this)[0].Time.split(' ')[1] + '</td><td>' + $(this)[0].payMentName + '</td><td>' + $(this)[0].statusName + '</td></tr>'
                        addItem += trItem;
                        i++;
                    });
                    $(".tbody_ExamInfoList").children().remove();
                    $(".tbody_ExamInfoList").prepend(addItem);
                    AddPagin(pageIndex);
                    $(".list-all").show();
                    $(".pieBox-Status").hide();
                    $(".pieBox-Cayment").hide();
                }
                else if (jsonData.state == "nologin") {
                    noLogin();
                }
                else {
                    alert(jsonData.msg);
                }
            }, "json");
        }
    }

    function AddPagin(pageIndex) {
        pager = $("#Pagin").paginControl({
            pageIndex: pageIndex, pageSize: $("#hdnPageSize").attr("value"), recordCount: $("#hdnCount").attr("value"), onPageChanged: function (num, thisPaginControl) {
                GetList(num);
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        });
    }

});