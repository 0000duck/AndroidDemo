$(document).ready(function () {
    BindKindEditor();
    $(".cancelBtn").on("click", function () { window.close()});
    function BindKindEditor(){
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="companyContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                readonlyMode : true,
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

});