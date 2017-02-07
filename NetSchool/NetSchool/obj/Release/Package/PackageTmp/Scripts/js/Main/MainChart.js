$(document).ready(function () {
    countLearn();
})
function countLearn() {
    var url = "http://" + window.location.host + "/Ajax/eduLearningManage.ashx";
    var myDate = new Date();
    var postdata = { cmd: "getListByStamp", strYear: myDate.getFullYear() };
    countList(url, postdata);
}
function countExam() {
    var url = "http://" + window.location.host + "/Ajax/eduExamInfoManage.ashx";
    var myDate = new Date();
    var postdata = { cmd: "getListByStamp", strYear: myDate.getFullYear() };
    countList(url, postdata);
}
function countPayInfo() {
    var url = "http://" + window.location.host + "/Ajax/eduPayInfoManage.ashx";
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 1;
    var beginTime = year + "-" + month + "-01";
    var endTime = "";
    switch (month) {
        case 1, 3, 5, 7, 8, 10, 12:
            endTime = year + "-" + month + "-31";
            break;
        case 4, 6, 9, 11:
            endTime = year + "-" + month + "-30";
            break;
        default:
            if (month / 4 == 0) {
                endTime = year + "-" + month + "-29";
            } else {
                endTime = year + "-" + month + "-28";
            }
            break;
    }
    var postdata = { cmd: "countStatus", serBeginTime: beginTime, serEndTime: endTime };
    countListPie(url, postdata);
}
function countListPie(url, postdata) {
    $("#pieBox").show();
    $("#lineBox").hide();
    $.post(url, postdata, function (jsonData) {
        var dataList = jsonData.list;
        if (jsonData.state == "success") {
            bindPie(dataList);
        }
    }, "json");
}
function countList(url, postdata) {
    $("#pieBox").hide();
    $("#lineBox").show();
    $.post(url, postdata, function (jsonData) {
        var dataList = jsonData.countList;
        if (jsonData.state == "success") {
            var year = jsonData.year + "年度各月的信息统计";
            var type = "bar";
            bindLine(dataList, year, jsonData.monthList, type);
        }
    }, "json");
}