$(document).ready(function () {
    var editor;
    BindKindEditor();
    BtnClick();
    var id = $("#hdnID").val();
    var type = "addNews";
    if (id != "00000000-0000-0000-0000-000000000000") {
        type = "editNews";
        $(".title span").text("修改新闻");
        getNewsInfo(id);
    }
    function BtnClick() {
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
                    var url = "http://" + window.location.host + "/Ajax/NewsManage.ashx";
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
    function BindKindEditor() {

        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="newsContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                allowFileManager: true,
                //themeType : 'default',
                themeType: 'simple',
                items: [
                  ]
            });
            K('input[name=OKBtn]').click(function (e) {
                Submit(editor.html());
            });

        });
    }

    function getNewsInfo(id) {
        var url = "http://" + window.location.host + "/Ajax/NewsManage.ashx";
        var postdata = { cmd: "getInfo", id: id };
        $.post(url, postdata, function (dataJson) {
            if (dataJson.state == "success") {
                $(".txtTitle").val(dataJson.newsInfo.Title);
                editor.html(dataJson.newsInfo.Content);
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