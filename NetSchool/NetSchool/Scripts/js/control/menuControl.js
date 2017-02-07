$(".menuList").ready(function () {
    $(".menuList").on("mouseenter", ".dynamicInfoItem", function () {
        $(".dynamicInfo").show();
    }).on("mouseenter", ".systemManageItem", function () {
        $(".systemManage").show();
    });
    $(".menuList").on("mouseleave", ".dynamicInfoItem", function () {
        $(".dynamicInfo").hide();
    }).on("mouseleave", ".systemManageItem", function () {
        $(".systemManage").hide();
    });
    $(".menuList").on("click", ".newsMenu", function () {
        window.location.href = "http://" + window.location.host + "/News/NewsList";
    }).on("click", ".lawMenu", function () {
        window.location.href = "http://" + window.location.host + "/Law/LawList";
    }).on("click", ".NoticeMenu", function () {
        window.location.href = "http://" + window.location.host + "/Notice/NoticeList";
    }).on("click", ".UserMenu", function () {
        window.location.href = "http://" + window.location.host + "/People/PeopleList";
    });
});