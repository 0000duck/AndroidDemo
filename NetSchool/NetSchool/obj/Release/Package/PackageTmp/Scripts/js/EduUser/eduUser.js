$(document).ready(function () {
    GetList(1);
    function GetList() {
        var searchTxt = "";
        var url = "http://" + window.location.host + "/Ajax/eduCompanyManage.ashx";
        var postdata = { cmd: "getListByCompany", strSearch: searchTxt };
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
function countList() {
    var searchTxt = $(".searchLTxt").val();
    var isCountByStamp = $(".isCountByStamp").is(':checked');
    var isCountByCompany = $(".isCountByCompany").is(':checked');
    var isCountByTime = $(".isCountByTime").is(':checked');
    var isCountByIdcard = $(".isCountByIdcard").is(":checked");
    var isCountByGender = $(".isCountByGender").is(":checked");
    var isCountByRoles = $(".isCountByRoles").is(":checked");
    var Year = $("#Y").is(':checked');
    var Month = $("#M").is(':checked');
    var Day = $("#D").is(':checked');
    var Hour = $("#H").is(':checked');
    var Minuter = $("#m").is(':checked');
    var Second = $("#S").is(':checked');
    url = "http://" + window.location.host + "/Ajax/eduUserManage.ashx";

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
    else if (isCountByCompany) {
        postdata = { cmd: "getListByCompany", strSearch: searchTxt, isCountByCompany: isCountByCompany };
    }
    else if (isCountByIdcard) {
        postdata = { cmd: "isCountByIdcard", strSearch: searchTxt, isCountByIdcard: isCountByIdcard };
    }
    else if (isCountByGender) {
        postdata = { cmd: "isCountByGender", strSearch: searchTxt, isCountByGender: isCountByGender };
    }
    else if (isCountByRoles) {
        postdata = { cmd: "isCountByRoles", strSearch: searchTxt, isCountByRoles: isCountByRoles };
    }
    else {
        postdata = { cmd: "getListByTime", strSearch: searchTxt, countStyle: style, isCountByTime: isCountByTime };
    }

    $.post(url, postdata, function (jsonData) {
        var dataList = jsonData.countList;
        if (jsonData.state == "success") {
            if (isCountByStamp) {
                var year = jsonData.year + "年度各月的信息统计";
                bindLine(dataList, year, jsonData.monthList)
            } else {
                bindPie(dataList);
            }
        }
        else if (jsonData.state == "nologin") {
            noLogin();
        }
    }, "json");
}


