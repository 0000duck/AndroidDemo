$(document).ready(function () {
    BtnClick();
    var id = $("#hdnID").val();
    var type = "addCompany";
    if (id != "00000000-0000-0000-0000-000000000000") {
        type = "editCompany";
        $(".title span").text("修改公司信息");
        getCompanyInfo(id);
    }
    function BtnClick() {
        $(".btnDiv").on("click", ".cancelBtn", function () {
            parent.$.fancybox.close();
        }).on("click", ".OKBtn", function () {
            Submit();
        });
    }
    function Submit() {
        var company = $(".txtCompany").val();
        var territory = $(".txtTerritory").val();
        if (!company) {
            $(".msgConpany").text("公司名称不能为空！");
        }
        else if (!territory) {
            $(".msgConpany").text("所属区域不能为空！");
        }
        else {
            var url = "http://" + window.location.host + "/Ajax/EduCompanyManage.ashx";
            var postdata = { cmd: type, id: id, company: company, territory: territory };
            $.post(url, postdata, function (dataJson) {
                if (dataJson.state == "success") {                      
                    parent.$.fancybox.close();;
                }
                else {
                    alert(dataJson.msg);
                }
            }, "json");
        }
    }
    function getCompanyInfo(id) {
        var url = "http://" + window.location.host + "/Ajax/EduCompanyManage.ashx";
        var postdata = { cmd: "getInfo", id: id };
        $.post(url, postdata, function (dataJson) {
            if (dataJson.state == "success") {
                $(".txtCompany").val(dataJson.companyInfo.Company);
                $(".txtTerritory").val(dataJson.companyInfo.Territory);
            }
            else {
                alert(dataJson.msg);
            }
        }, "json");
    }
});