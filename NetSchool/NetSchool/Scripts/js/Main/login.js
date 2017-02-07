$(document).ready(function () {
    Fun_Login();
    Regeist_Url();
    if ($("#session").val() != null && $("#session").val() != "") {
        $(".loginBox").hide();
        $(".loginedBox").show();
    } else {
        $(".loginBox").show();
        $(".loginedBox").hide();
    }
    $(".mainImg").on("click", function () {
        window.open("http://dekanglai.edusoho.cn/");
    });
});
function Regeist_Url() {
    $(".title").on("click", ".newslist", function () {
        var id = '00000000-0000-0000-0000-000000000000';
        window.open("http://" + window.location.host + "/News/NewsList");
    }).on("click", ".lawlist", function () {
        var id = '00000000-0000-0000-0000-000000000000';
        window.open("http://" + window.location.host + "/Law/LawList");
    }).on("click", ".noticelist", function () {
        var id = '00000000-0000-0000-0000-000000000000';
        window.open("http://" + window.location.host + "/Notice/NoticeList");
    });

}
function Fun_Login() {
    $("#pswBox").keydown(function (event) {
        event = window.event || event;
        var keyCode = 0;
        if (event.which) {
            keyCode = event.which;
        }
        else {
            keyCode = event.keyCode;
        }
        if (keyCode == 13) {
            doLogin();
        }
    });
    $('#btn_login').click(function () {
        doLogin();
    });
    function doLogin() {
        var strUser = $('#loginBox').val();
        var strPwd = $('#pswBox').val();
        var url = "http://" + window.location.host + "/Ajax/LoginManage.ashx";
        var postdata = { cmd: "login_in", user: strUser, pwd: strPwd, rand: Math.random() };
        $.getJSON(url, postdata, function (dataJson) {
            if (dataJson.msg == "ok") {                    
                window.location.href = "http://" + window.location.host + "/Main/Main";
            }
            else {
                alert(dataJson.msg);
                testLine.showLoading({
                    text: '登录失败，请稍后重试...'
                });
            }
        }, "json");
    }
    $(".logoutBtn").click(function () {
        var url = "http://" + window.location.host + "/Ajax/LoginManage.ashx";
        var postdata = { cmd: "login_out" };
        $.post(url, postdata, function (dataJson) {
            if (dataJson.msg == "ok") {                      
                window.location.href = "http://" + window.location.host;
            }
            else {
                alert(dataJson.msg);
            }
        }, "json");
    });
}