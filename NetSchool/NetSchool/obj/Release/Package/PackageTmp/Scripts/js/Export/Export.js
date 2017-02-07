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
        $(".ExamInfoOperation").on("click", ".searchButton", function () {
            GetList(1);
        }).on("click", ".exportExcel", function () {
            ExportExcel();
        }).on("click", ".exportAccess", function () {
            ExportAccess();
        });
    }

    function ExportExcel() {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var serStatus = "finished";
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduExamInfoManage.ashx";
            var postdata = { cmd: "exportExcel", serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime, serStatus: serStatus };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    var url = jsonData.dUrl;
                    window.open(url)
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
    function ExportAccess() {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var serStatus = "finished";
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduExamInfoManage.ashx";
            var postdata = { cmd: "exportAccess", serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime, serStatus: serStatus };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    var url = jsonData.dUrl;
                    window.open(url)
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
    function CountInfo(type) {
        var serCompany = $(".serCompany").val();
        var serIDCard = $(".serIDCard").val();
        var serTerritory = $(".serTerritory").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var serStatus = "finished";
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduExamInfoManage.ashx";
            var postdata = { cmd: type, serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime, serStatus: serStatus };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    bindPie(dataList);
                    $(".list-all").hide();
                    $(".pieBox-Status").show();
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
        var serStatus = "finished";
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduExamInfoManage.ashx";
            var postdata = { cmd: "getList", pagesize: $("#hdnPageSize").val(), pageindex: pageIndex, serCompany: serCompany, serIDCard: serIDCard, serTerritory: serTerritory, serBeginTime: serBeginTime, serEndTime: serEndTime, serStatus: serStatus };
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    $("#hdnCount").attr("value", jsonData.count);
                    var addItem = ""
                    var i = 0;
                    $(dataList).each(function () {
                        var trItem = '<tr class="' + (i % 2 == 0 ? "odd-row" : "even-row") + '"><td realid="' + $(this)[0].ID + '">' + (i + 1) + '</td><td realid="' + $(this)[0].idcard + '">' + $(this)[0].EduUserName + '</td><td realname="">' + $(this)[0].company + '</td><td realter="">' + $(this)[0].territory + '</td><td>' + $(this)[0].Begin.split(' ')[0] + '<br/>' + $(this)[0].Begin.split(' ')[1] + '</td><td>' + ($(this)[0].endTime == "0" ? "" : $(this)[0].End.split(' ')[0]) + '<br/>' + ($(this)[0].endTime == "0" ? "" : $(this)[0].End.split(' ')[1]) + '</td><td>' + $(this)[0].statusName + '</td></tr>'
                        addItem += trItem;
                        i++;
                    });
                    $(".tbody_ExamInfoList").children().remove();
                    $(".tbody_ExamInfoList").prepend(addItem);
                    AddPagin(pageIndex);
                    $(".list-all").show();
                    $(".pieBox-Status").hide();
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