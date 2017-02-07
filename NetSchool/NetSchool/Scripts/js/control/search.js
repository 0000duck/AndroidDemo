$(document).ready(function () {
    $(".searchTxt").on("focus", function () {
        $(this).next(".searchBtn").addClass("searchBtn-focus")
    }).on("blur", function () {
        $(this).next(".searchBtn").removeClass("searchBtn-focus")
    });
})