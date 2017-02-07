$(document).ready(function () {
    var userName = $("#hdnUserName").attr("value");
    $(".btnDiv").on("click", ".cancelBtn", function () {
        parent.$.fancybox.close();
    }).on("click", ".OKBtn", function () {
        Submin();
    });

    function Submin() {
        var oldPassword = $(".oldPassword").val();
        var newPassword = $(".newPassword").val();
        var newPasswordAgain = $(".newPasswordAgain").val();
        if (!oldPassword || !newPassword || !newPasswordAgain || oldPassword.length < 6 || newPassword.length < 6 || newPasswordAgain.length < 6 || oldPassword.length > 20 || newPassword.length > 20 || newPasswordAgain.length > 20) {
            $(".msgPassword").text("都不能为空！且长度必须在6~20位！")
        }
        else {
            if (newPassword != newPasswordAgain) {
                $(".msgPassword").text("新密码两次输入不相同！");
            }
            else {
                var url = "http://" + window.location.host + "/Ajax/PeopleManage.ashx";
                var postdata = { cmd: "changePassword", oldPassword: oldPassword, newPassword: newPassword};
                $.post(url, postdata, function (dataJson) {
                    if (dataJson.state == "success") {                      
                        parent.$.fancybox.close();
                        alert("修改成功,请重新登录");
                        Logout();
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

    function Logout() { }
});