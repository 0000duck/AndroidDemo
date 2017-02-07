$(document).ready(function () {
    doReady();
    BindSearch();
    Operation();
    BindChkAll();
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
        $(".CompanyOperation").on("click", ".addCompanyBtn", function () {
            var id = '00000000-0000-0000-0000-000000000000';
            showDialog(1000, 680, "http://" + window.location.host + "/EduCompany/CompanyManage/" + id, function () { GetList(1) });
            //window.open("http://" + window.location.host + "/EduCompany/CompanyManage/"+id);
        }).on("click", ".delCompanyBtn", function () {
            var chkList = $('input:checkbox[name=chkn]:checked');
            if (chkList.length == 0) {
                alert("您没有选择需要删除的内容，请检后操作");
            }
            else {
                var allId = "";
                chkList.each(function () {
                    allId += $(this).attr("id") + ",";
                });
                allId = allId.substring(0, allId.length - 1);
                DeleteList(allId);
            }
        });
    }

    function DeleteList(allId) {
        var url = "http://" + window.location.host + "/Ajax/EduCompanyManage.ashx";
        var postdata = { cmd: "deleteCompany", id: allId };
        $.post(url, postdata, function (responseData) {
            if (responseData.state == "success") {
                var data = responseData.data;
                GetList(1);
            }
            else {
                alert(responseData.msg);
            }
        }, "json");
    }

    function Operation() {
        $(".tbody_companyList").on("click", ".companyShow", function () {
            var id = $(this).parent().parent().find(".chk").attr("id");
            window.open("http://" + window.location.host + "/EduCompany/CompanyShow/" + id);
        }).on("click", ".companyEdit", function () {
            var id = $(this).parent().parent().find(".chk").attr("id");
            showDialog(1000, 680, "http://" + window.location.host + "/EduCompany/CompanyManage/" + id, function () { GetList(1) });
        });
    }

    function BindSearch() {
        $(".searchButton").on("click", function () {
            var strTerritorySearch = $(".searchTerritoryTxt").val();
            var strYearSearch = $(".searchYearTxt").val();
            var strCompanySearch = $(".searchCompanyTxt").val();
            $(".hdnCompanySearch").attr("value", strCompanySearch);
            $(".paginControl").children().remove();
            $("#hdnTerritorySearch").attr("value", strTerritorySearch);
            $("#hdnYearSearch").attr("value", strYearSearch);
            GetList(1);
        });
    }

    function GetList(pageIndex) {
        var searchTxt = $("#hdnTerritorySearch").val();
        var strCompany = $(".searchCompanyTxt").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var url = "http://" + window.location.host + "/Ajax/EduCompanyManage.ashx";
        var postdata = {
            cmd: "getList", pagesize: $("#hdnPageSize").val(), pageindex: pageIndex, strSearch: searchTxt, strCompany: strCompany, serBeginTime: serBeginTime,
            serEndTime: serEndTime
        };
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    $("#hdnCount").attr("value", jsonData.count);
                    var addItem = ""
                    var i = 0;
                    $(dataList).each(function () {
                        var trItem = '<tr class="' + (i % 2 == 0 ? "odd-row" : "even-row") + '">'
                            + '<td><input type="checkbox" id="' + $(this)[0].ID + '" name="chkn" class="chk" /></td>'
                            + '<td>' + (i + 1) + '</td>'
                            + '<td><a class="companyShow aOperation">' + $(this)[0].company + '</a></td>'
                            + '<td>' + $(this)[0].territory + '</td>'
                            + '<td>' + $(this)[0].Time.split(' ')[0] + '<br/>' + $(this)[0].Time.split(' ')[1] + '</td>'
                            + '<td>' + $(this)[0].CreateTime.split(" ")[0] + '<br/>' + $(this)[0].CreateTime.split(" ")[1] + '</td>'
                            + '<td><a class="companyEdit aOperation">编辑</a></td></tr>'
                        addItem += trItem;
                        i++;
                    });
                    $(".tbody_companyList").children().remove();
                    $(".tbody_companyList").prepend(addItem);
                    AddPagin(pageIndex);
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

    function BindChkAll() {
        $(document).on("click", ".chk-all", function () {
            var checked = this.checked;
            $(".CompanyList").find("tbody :checkbox").each(function () {
                this.checked = checked;
            });
        })
        $(".CompanyList").on("click", ".chk", function () {
            $(".chk-all").removeAttr("checked");
        });
    }
});