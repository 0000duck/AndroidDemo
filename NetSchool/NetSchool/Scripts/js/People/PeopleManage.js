$(document).ready(function () {
    var editor;
    BtnClick();

    var type = "addPeople";
    var userName = $("#hdnUserName").val();

    if (userName != "") {
        type = "editPeople";
        $(".title span").text("修改管理员信息");
        getPeopleInfo(userName);
    }
    function BtnClick() {
        $(".btnDiv").on("click", ".cancelBtn", function () {
            parent.$.fancybox.close();
        }).on("click", ".OKBtn", function () {
            Submit();
        });
    }



    //else {
    //            if (1 == 1) {

    //    }

    function Submit(content) {
        var userName = $(".userName").val();
        var realName = $(".realName").val();
        var telephone = $(".telephone").val();
        var Email = $(".Email").val();
        if (!userName) {
            $(".msgPeople").text("用户名不能为空！");
        }
        else {
            if (userName.length > 20) {
                $(".msgPeople").text("用户名不能大于20字！");
            }
            else {
                if (!realName) {
                    $(".msgPeople").text("姓名不能为空！");
                }
                else {
                    if (telephone.length > 20) {
                        $(".msgPeople").text("电话不能大于20字！");
                    }
                    else {
                        if (Email.length > 50) {
                            $(".msgPeople").text("邮箱不能大于50字！");
                        }
                        else {
                            var url = "http://" + window.location.host + "/Ajax/PeopleManage.ashx";
                            var postdata = { cmd: type, userName: userName, realName: realName, telephone: telephone, Email: $(".Email").val(), permission: $(".sltPermission").val() };
                            $.post(url, postdata, function (dataJson) {
                                if (dataJson.state == "success") {                      //该处主要是确定获取到了ajax数据
                                    parent.$.fancybox.close();
                                }
                                else if (dataJson.state == "nologin") {
                                    noLogin();
                                }
                                else {
                                    alert(dataJson.msg);
                                }
                            }, "json");

                        }
                    }
                }
            }
        }
    }




    function getPeopleInfo(id) {
        var url = "http://" + window.location.host + "/Ajax/PeopleManage.ashx";
        var postdata = { cmd: "getInfo", userName: id };
        $.post(url, postdata, function (dataJson) {
            if (dataJson.state == "success") {
                $(".userName").val(dataJson.peopleInfo.UserName);
                $(".realName").val(dataJson.peopleInfo.RealName);
                $(".telephone").val(dataJson.peopleInfo.TelePhone);
                $(".Email").val(dataJson.peopleInfo.Email);
                $(".sltPermission").val(dataJson.peopleInfo.Permission);
            }
            else if (dataJson.state == "nologin") {
                noLogin();
            }
            else {
                alert(dataJson.msg);
            }
        }, "json");
    }
});