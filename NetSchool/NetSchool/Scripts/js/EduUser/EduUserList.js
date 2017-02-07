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
        $(".UserOperation").on("click", ".addUserBtn", function () {
            var id = '00000000-0000-0000-0000-000000000000';
            showDialog(1000, 680, "http://" + window.location.host + "/EduUser/UserManage/" + id, function () { GetList(1) });
            //window.open("http://" + window.location.host + "/EduUser/UserManage/"+id);
        }).on("click", ".delUserBtn", function () {
            var chkList = $('input:checkbox[name=chkn]:checked');
            if (chkList.length == 0) {
                alert("您没有选择需要删除的内容，请检后查操作");
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
        var url = "http://" + window.location.host + "/Ajax/EduUserManage.ashx";
        var postdata = { cmd: "deleteUser", id: allId };
        $.post(url, postdata, function (responseData) {
            if (responseData.state == "success") {
                var data = responseData.data;
                GetList(1);
            }
            else if (responseData.state == "nologin") {
                noLogin();
            }
            else {
                alert(responseData.msg);
            }
        }, "json");
    }

    function Operation() {
        $(".tbody_userList").on("click", ".userShow", function () {
            var id = $(this).parent().parent().find(".chk").attr("id");
            window.open("http://" + window.location.host + "/EduUser/UserShow/" + id);
        }).on("click", ".userEdit", function () {
            var id = $(this).parent().parent().find(".chk").attr("id");
            showDialog(1000, 680, "http://" + window.location.host + "/EduUser/UserManage/" + id, function () { GetList(1) });
            //window.open("http://" + window.location.host + "/EduUser/UserManage/" + id);
        });
    }

    function BindSearch() {
        $(".searchButton").on("click", function () {
            $(".paginControl").children().remove();
            $("#hdnNicknameSearch").attr("value", $(".searchNickname").val());
            $("#hdnCompanySearch").attr("value", $(".searchCompany").val());
            $("#hdnRolesSearch").attr("value", $(".searchRoles").val());
            $("#hdnGenderSearch").attr("value", $(".searchGender").val());
            $("#hdnIdcardSearch").attr("value", $(".searchIdcard").val());
            $("#hdnYearSearch").attr("value", $(".searchYearTxt").val());
            GetList(1);
        });
        $(".searchTxt").keydown(function (event) {
            event = window.event || event;
            var keyCode = 0;
            if (event.which) {
                keyCode = event.which;
            }
            else {
                keyCode = event.keyCode;
            }
            if (keyCode == 13) {
                var strSearch = $(this).val();
                $("#hdnSearch").val(strSearch);
                GetList(1);
            }
        });
    }
    function BingRoles(rolelist) {
        var addItem = '<option value="">不限</option>';
        $(rolelist).each(function () {
            var trItem = "";
            var temp=$(this)[0]["roles"];
            if ($("#hdnRolesSearch").val() != "") {
                if(temp==null&&$("#hdnRolesSearch").val()== "null"){
                    trItem = '<option value="' + temp + '" selected ="selected">' + temp + '</option>';
                }else{
                    if ($("#hdnRolesSearch").val() == temp)
                        trItem = '<option value="' + temp + '" selected ="selected">' + temp + '</option>';
                    else
                        trItem = '<option value="' + temp + '">' + temp + '</option>';
                }
            }else {
                trItem = '<option value="' + temp + '">' + temp + '</option>';
            }
            addItem += trItem;
        });
        $(".searchRoles").children().remove();
        $(".searchRoles").prepend(addItem);
    }
    function GetList(pageIndex) {
        var searchCompanyTxt = $("#hdnCompanySearch").val();
        var searchRolesTxt = $("#hdnRolesSearch").val();
        var searchGenderTxt = $("#hdnGenderSearch").val();
        var searchIdcardTxt = $("#hdnIdcardSearch").val();
        //var searchYearTxt = $("#hdnYearSearch").val();
        var serBeginTime = $(".serBeginTime").val();
        var serEndTime = $(".serEndTime").val();
        var searchNicknameTxt = $("#hdnNicknameSearch").val();
        var url = "http://" + window.location.host + "/Ajax/EduUserManage.ashx";
        var postdata = {
            cmd: "getList", pagesize: $("#hdnPageSize").val(),
            pageindex: pageIndex, strCompanySearch: searchCompanyTxt,
            strIdcardSearch: searchIdcardTxt, strRolesSearch: searchRolesTxt,
            strGenderSearch: searchGenderTxt, serBeginTime: serBeginTime,
            serEndTime:serEndTime,strNicknameSearch: searchNicknameTxt
        };
        if (new Date(serEndTime.replace("-", "/").replace("-", "/")) < new Date(serBeginTime.replace("-", "/").replace("-", "/"))) {
            alert("结束时间不能小于开始时间，请检查后从新操作");
        }
        else {
            $.post(url, postdata, function (jsonData) {
                if (jsonData.state == "success") {
                    dataList = jsonData.list;
                    $("#hdnCount").attr("value", jsonData.count);
                    var addItem = "";
                    var i = 0;
                    $(dataList).each(function () {
                        var trItem = '<tr class="' + (i % 2 == 0 ? "odd-row" : "even-row") + '">'
                            + '<td><input type="checkbox" id="' + $(this)[0].ID + '" name="chkn" class="chk" /></td>'
                            + '<td>' + (i + 1) + '</td>'
                            + '<td><a class="userShow aOperation">' + $(this)[0].nickname + '</a></td>'
                        + '<td>' + $(this)[0].idcard + '</td>'
                        + '<td>' + $(this)[0].Sex + '</td>'
                        + '<td>' + $(this)[0].roles + '</td>'
                            + '<td>' + $(this)[0].company + '</td>'
                            + '<td>' + $(this)[0].Time.split(' ')[0] + '<br/>' + $(this)[0].Time.split(' ')[1] + '</td>'
                            //+ '<td>' + $(this)[0].CreateTime.split(" ")[0] + '<br/>' + $(this)[0].CreateTime.split(" ")[1] + '</td>'
                            + '<td><a class="userEdit aOperation">编辑</a></td></tr>'
                        addItem += trItem;
                        i++;
                    });
                    $(".tbody_userList").children().remove();
                    $(".tbody_userList").prepend(addItem);
                    AddPagin(pageIndex);
                    BingRoles(jsonData.rolesList);
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

    function BindChkAll() {
        $(document).on("click", ".chk-all", function () {
            var checked = this.checked;
            $(".UserList").find("tbody :checkbox").each(function () {
                this.checked = checked;
            });
        })
        $(".UserList").on("click", ".chk", function () {
            $(".chk-all").removeAttr("checked");
        });
    }
});