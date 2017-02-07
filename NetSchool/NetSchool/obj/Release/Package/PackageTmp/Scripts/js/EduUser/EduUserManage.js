$(document).ready(function () {
    var id = $("#hdnID").val();
    var type = "addUser";
    if (id != "00000000-0000-0000-0000-000000000000") {
        type = "editUser";
        $(".title span").text("修改人员信息");
        getUserInfo(id);
    }
    BtnClick();
    function BtnClick() {
        $(".btnDiv").on("click", ".cancelBtn", function () {
            parent.$.fancybox.close();;
        }).on("click", ".OKBtn", function () {
            Submit();
        });
    }
    function Submit() {
        var nickname = $(".txtNickname").val();
        var idcard = $(".txtIdcard").val();
        var company = $(".txtCompany").val();
        var roles = $(".txtRoles").val();
        var gender = $(".txtGender").val();
        if (!nickname) {
            $(".msgUser").text("人员名称不能为空！");
        }
        else {
            if (!idcard) {
                $(".msgUser").text("身份证号不能为空！");
            }
            else {
                if (!company) {
                    $(".msgUser").text("所属公司不能为空！")
                }
                else {
                    var url = "http://" + window.location.host + "/Ajax/EduUserManage.ashx";
                    var postdata = {
                        cmd: type, id: id, nickname: nickname, idcard: idcard,
                        company: company, roles: roles, gender: gender
                    };
                    $.post(url, postdata, function (dataJson) {
                        if (dataJson.state == "success") {                      //该处主要是确定获取到了ajax数据
                            parent.$.fancybox.close();;
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
        function getUserInfo(id) {
            var url = "http://" + window.location.host + "/Ajax/EduUserManage.ashx";
            var postdata = { cmd: "getInfo", id: id };
            $.post(url, postdata, function (dataJson) {
                if (dataJson.state == "success") {
                    $(".txtNickname").val(dataJson.userInfo.NickName);
                    $(".txtIdcard").val(dataJson.userInfo.Idcard);
                    $(".txtCompany").val(dataJson.userInfo.Company);
                    $(".txtRoles").val(dataJson.userInfo.Roles);
                    $(".txtGender").val(dataJson.userInfo.Gender);
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