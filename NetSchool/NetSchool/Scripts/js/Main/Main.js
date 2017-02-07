$(document).ready(function () {
    $(".contentLine").on("click", "p", function () {
        $(".contentLine .contentLine-active").removeClass("contentLine-active");
        $(this).addClass("contentLine-active");
    });
});