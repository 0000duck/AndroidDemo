$(document).ready(function () {
    GetList(1);
    function GetList() {
        var searchTxt = "";
        var url = "http://" + window.location.host + "/Ajax/eduCompanyManage.ashx";
        var postdata = { cmd: "getListByTerritory", strSearch: searchTxt };
        $.post(url, postdata, function (jsonData) {
            var dataList = jsonData.list;
            if (jsonData.state == "success") {
                bindPie(dataList);
            }
            else if (jsonData.state == "nologin") {
                noLogin();
            }
        }, "json");
    }
})
function bindPie(dataList) {
    var testPie = echarts.init(document.getElementById("pieBox"));
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
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel'],
                    option: {
                        funnel: {
                            x: '25%',
                            width: '50%',
                            funnelAlign: 'left',
                            max: 1000
                        }
                    }
                }
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
                center: ['50%', '60%'],
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
function countList() {
    var searchTxt = $(".searchLTxt").val();
    var isCountByStamp = $(".isCountByStamp").is(':checked');
    var isCountByTerritory = $(".isCountByTerritory").is(':checked');
    var isCountByTime = $(".isCountByTime").is(':checked');
    var Year = $("#Y").is(':checked');
    var Month = $("#M").is(':checked');
    var Day = $("#D").is(':checked');
    var Hour = $("#H").is(':checked');
    var Minuter = $("#m").is(':checked');
    var Second = $("#S").is(':checked');
    url = "http://" + window.location.host + "/Ajax/eduCompanyManage.ashx";

    if (Year) {
        var style = "Y";
    }
    if (Month) {
        var style = "M";
    }
    if (Day) {
        var style = "D";
    }
    if (Hour) {
        var style = "H";
    }
    if (Minuter) {
        var style = "mi";
    }
    if (Second) {
        var style = "S";
    }
    var postdata;
    if (isCountByStamp) {
        postdata = { cmd: "getListByStamp", strSearch: searchTxt };
    }
    else {
        postdata = { cmd: "getListByTime", strSearch: searchTxt, countStyle: style, isCountByTerrtiory: isCountByTerritory, isCountByTime: isCountByTime };
    }

    $.post(url, postdata, function (jsonData) {
        var dataList = jsonData.list;
        if (jsonData.state == "success") {
            bindPie(dataList);
        }
        else if (jsonData.state == "nologin") {
            noLogin();
        }
    }, "json");
}

