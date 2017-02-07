<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuControl.ascx.cs" Inherits="NetSchool.Controls.menuControls" %>
<link href="../CSS/control/menuControl.css" rel="stylesheet" />
<script src="~/Scripts/jquery-1.10.2.min.js"></script>
<script src="../Scripts/js/control/menuControl.js"></script>
<div class="menuList">
    <div class="menuTab fl">首页</div>
    <div class="menuTab fl">统计分析</div>
    <div class="menuTab dynamicInfoItem fl">
        动态信息
                <div class="menuItemBox dynamicInfo">
                    <div class="menuItem newsMenu">新闻资讯</div>
                    <div class="menuItem lawMenu">法律法规</div>
                    <div class="menuItem NoticeMenu">通知公告</div>
                    <div></div>
                </div>
        <div class="clear"></div>
    </div>
    <div class="menuTab fl">企业管理</div>
    <div class="menuTab fl">人员管理</div>
    <div class="menuTab fl systemManageItem fl">
        系统管理
                <div class="menuItemBox systemManage">
                    <div class="menuItem UserMenu">系统人员</div>
                    <div></div>
                </div>
    </div>
</div>
