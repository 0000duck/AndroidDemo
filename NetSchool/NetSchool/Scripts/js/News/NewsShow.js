$(document).ready(function () {
    BindKindEditor();

    function BindKindEditor(){
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="newsContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                readonlyMode : true,
                items: []
            });
            K('input[name=OKBtn]').click(function (e) {
                Submit(editor.html());
            });
            var autoheight = editor.edit.doc.body.scrollHeight;
            editor.edit.setHeight(autoheight);
        });
    }
    $(".cancelBtn").on("click", function () { window.close() });
});