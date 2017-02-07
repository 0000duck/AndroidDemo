$(document).ready(function () {
    doReady();
    GetList(1);

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
        $(".LearningInfoOperation").on("click", ".searchButton", function () {
            GetList(1);
        }).on("click", ".countTerritoryBtn", function () {
            CountInfo("countTerritory");
        });
    }
     function CountInfo(type) {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduLearningManage.ashx";
            var postdata = {
                cmd: type, serCompany: serCompany, serIDCard: serIDCard,
                serTerritory: serTerritory, serBeginTime: serBeginTime,
                serEndTime: serEndTime
            };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                        var id = "pieBoxTerritory";
                        bindPie(dataList, id);
                        $(".list-all").hide();
                        $(".pieBox-Territory").show();
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
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduLearningManage.ashx";
            var postdata = { cmd: "getList", pagesize: $("#hdnPageSize").val(), pageindex: pageIndex, serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    $("#hdnCount").attr("value", jsonData.count);
                    var addItem = ""
                    var i = 0;
                    $(dataList).each(function () {
                        var trItem = '<tr class="' + (i % 2 == 0 ? "odd-row" : "even-row") + '"><td realid="' + $(this)[0].ID + '">' + (i + 1) + '</td><td realid="' + $(this)[0].idcard + '">' + $(this)[0].EduUserName + '</td><td realname="">' + $(this)[0].company + '</td><td realter="">' + $(this)[0].territory + '</td>'
                            + '<td>' + $(this)[0].Time.split(' ')[0] + '<br/>' + $(this)[0].Time.split(' ')[1] + '</td></tr>'
                        addItem += trItem;
                        i++;
                    });
                    $(".tbody_LearningInfoList").children().remove();
                    $(".tbody_LearningInfoList").prepend(addItem);
                    AddPagin(pageIndex);
                    $(".list-all").show();
                    $(".pieBox-Territory").hide();
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
                                formatter: "{b}\n{d}%",//在饼状图上显示百分比
                                /*textStyle : {
                                    color : 'rgba(30,144,255,0.8)',
                                    align : 'center',
                                    baseline : 'middle',
                                    fontFamily : '微软雅黑',
                                    fontSize : 30,
                                    fontWeight : 'bolder'
                                }*///自定义饼图上字体样式
                            },
                            labelLine: {
                                show: true,
                            }
                        },
                        emphasis: {
                            label: {
                                show: true,
                                formatter: "{d}%"//鼠标移动到饼状图上显示百分比
                            }
                        }

                    }
                }
            ]
        };
        return option;
    }
});