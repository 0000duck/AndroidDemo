$(document).ready(function () {
    $(".menuList").on("mouseenter", ".dynamicInfoItem", function () {
        $(".dynamicInfo").show();
    }).on("mouseenter", ".systemManageItem", function () {
        $(".systemManage").show();
    }).on("mouseenter", ".SAInfoItem", function () {
        $(".SAInfo").show();
    });
    $(".menuList").on("mouseleave", ".dynamicInfoItem", function () {
        $(".dynamicInfo").hide();
    }).on("mouseleave", ".systemManageItem", function () {
        $(".systemManage").hide();
    }).on("mouseleave", ".SAInfoItem", function () {
        $(".SAInfo").hide();
    });
    $(".menuList").on("click", ".newsMenu", function () {
        window.location.href = "http://" + window.location.host + "/News/NewsList";
    }).on("click", ".lawMenu", function () {
        window.location.href = "http://" + window.location.host + "/Law/LawList";
    }).on("click", ".noticeMenu", function () {
        window.location.href = "http://" + window.location.host + "/Notice/NoticeList";
    }).on("click", ".peopleMenu", function () {
        window.location.href = "http://" + window.location.host + "/People/PeopleList";
    }).on("click", ".examInfoMenu", function () {
        window.location.href = "http://" + window.location.host + "/EduExamInfo/ExamInfoList";
    }).on("click", ".payInfoMenu", function () {
        window.location.href = "http://" + window.location.host + "/EduPayInfo/PayInfoList";
    }).on("click", ".learningInfoMenu", function () {
        window.location.href = "http://" + window.location.host + "/EduLearningInfo/LearningInfoList";
    }).on("click", ".eduUserMenu", function () {
        window.location.href = "http://" + window.location.host + "/EduUser/UserList";
    }).on("click", ".eduCompanyMenu", function () {
        window.location.href = "http://" + window.location.host + "/EduCompany/CompanyList";
    }).on("click", ".Main", function () {
        window.location.href = "http://" + window.location.host + "/Main/Main";
    }).on("click", ".Export", function () {
        window.location.href = "http://" + window.location.host + "/Export/Export";
    });

    $(document).on("click", ".ALink-Logout", function () {
        var url = "http://" + window.location.host + "/Ajax/LoginManage.ashx";
        var postdata = { cmd: "login_out", rand: Math.random() };
        $.getJSON(url, postdata, function (dataJson) {
            if (dataJson.msg == "ok") {                      //该处主要是确定获取到了ajax数据
                window.location.href = "http://" + window.location.host;
            }
            else if (dataJson.state == "nologin") {
                noLogin();
            }
            else {
                alert(dataJson.msg);
            }
        }, "json");
    }).on("click", ".ALink-PasswordChange", function () {
        showDialog(600, 600, "http://" + window.location.host + "/People/PasswordChange"/*, function () {}*/);
    });
});
function noLogin() {
    window.location.href = "http://" + window.location.host;
}
function Logout() { }