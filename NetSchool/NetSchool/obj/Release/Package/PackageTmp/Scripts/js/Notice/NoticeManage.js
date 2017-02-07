$(document).ready(function () {
    BindKindEditor();
    var editor;
    var id = $("#hdnID").val();
    var  type = "addNotice";
    if (id != "00000000-0000-0000-0000-000000000000") {
        type = "editNotice";
        $(".title span").text("修改通知公告");
        getNoticeInfo(id);
    }

    
    BtnClick();
    
    function BtnClick(){
        $(".btnDiv").on("click", ".cancelBtn", function () {
            parent.$.fancybox.close();
        });
    }

    function Submit(content) {
        var title = $(".txtTitle").val();
        if (!title) {
            alert("标题不能为空");
        }
        else {
            if (title.length > 20) {
                alert("标题字数不能超过20字");
            }
            else {
                if (!content) {
                    alert("内容不能为空");
                }
                else {
                    var url = "http://" + window.location.host + "/Ajax/NoticeManage.ashx";
                    var postdata = { cmd: type, id: id, title: title, content: escape(content) };
                    $.post(url, postdata, function (dataJson) {
                        if (dataJson.state == "success") {                      //该处主要是确定获取到了ajax数据
                            parent.$.fancybox.close();
                        }
                        else if (jsonData.state == "nologin") {
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

    function BindKindEditor(){
        
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="noticeContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                allowFileManager:true,
                items: [
                    'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'emoticons', 'image', 'link']
            });
            K('input[name=OKBtn]').click(function (e) {
                Submit(editor.html());
            });
        });
    }

    function getNoticeInfo(id)
    {
        var url = "http://" + window.location.host + "/Ajax/NoticeManage.ashx";
        var postdata = { cmd: "getInfo",id:id };
        $.post(url, postdata, function (dataJson) {
            if (dataJson.state == "success") {                     
                $(".txtTitle").val(dataJson.noticeInfo.Title);
                editor.html(dataJson.noticeInfo.Content);
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