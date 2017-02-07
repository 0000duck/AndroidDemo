(function ($) {
    $.fn.paginControl = function (options) {
        var defaults = {
            pageIndex: 0,
            pageSize: 30,
            recordCount: 0,
            onPageChanged: null
        }
        var isMore = false;
        var pageCount = 1;
        var choosePagin = null;
        var options = $.extend(defaults, options);

        var pagers = [];
        this.each(function () {
            var pagin = $(this);
            pagin.html("");
            if (!pagin.hasClass("paginControl")) {
                pagin.addClass("paginControl");
            }

            function choosePage(pageIndex) {
                pagin.setPageIndex(pageIndex);

                if (options.onPageChanged && typeof (options.onPageChanged) == "function") {
                    options.onPageChanged(pageIndex, pagin);
                }
            }

            pagin.currentPageIndex = options.pageIndex;
            pagin.choosePage = choosePage;

            pagin.setPageIndex = function (pageIndex) {
                pagin.currentPageIndex = pageIndex;

                var nowPagin = pagin.find(".page-selected");
                nowPagin.removeClass("page-selected");
                nowPagin.addClass("page")
                if (isMore == true) {
                    if (pageIndex > 4 && pageIndex < pageCount - 3) {
                        pagin.find(".paginLeft").show();
                        pagin.find(".paginRight").show();
                        choosePagin = pagin.find(".paginThird");
                        choosePagin.attr("rel", pageIndex);
                        choosePagin.text(pageIndex);
                        pagin.find(".paginFirst").attr("rel", pageIndex - 2);
                        pagin.find(".paginFirst").text(pageIndex - 2);
                        pagin.find(".paginSecond").attr("rel", pageIndex - 1);
                        pagin.find(".paginSecond").text(pageIndex - 1);
                        pagin.find(".paginFourth").attr("rel", pageIndex + 1);
                        pagin.find(".paginFourth").text(pageIndex + 1);
                        pagin.find(".paginFifth").attr("rel", pageIndex + 2);
                        pagin.find(".paginFifth").text(pageIndex + 2);
                    }
                    if (pageIndex <= 4) {
                        pagin.find(".paginLeft").hide();
                        pagin.find(".paginRight").show();
                        pagin.find(".paginFirst").attr("rel", "2");
                        pagin.find(".paginFirst").text("2");
                        pagin.find(".paginSecond").attr("rel", "3");
                        pagin.find(".paginSecond").text("3");
                        pagin.find(".paginThird").attr("rel", "4");
                        pagin.find(".paginThird").text("4");
                        pagin.find(".paginFourth").attr("rel", "5");
                        pagin.find(".paginFourth").text("5");
                        pagin.find(".paginFifth").attr("rel", "6");
                        pagin.find(".paginFifth").text("6");
                        switch (pageIndex) {
                            case 1:
                                choosePagin = pagin.find(".paginBegin");
                                break;
                            case 2:
                                choosePagin = pagin.find(".paginFirst");
                                break;
                            case 3:
                                choosePagin = pagin.find(".paginSecond");
                                break;
                            case 4:
                                choosePagin = pagin.find(".paginThird");
                                break;
                        }
                    }
                    if (pageIndex >= pageCount - 3) {
                        pagin.find(".paginLeft").show();
                        pagin.find(".paginRight").hide();
                        pagin.find(".paginFirst").attr("rel", pageCount - 5);
                        pagin.find(".paginFirst").text(pageCount - 5);
                        pagin.find(".paginSecond").attr("rel", pageCount - 4);
                        pagin.find(".paginSecond").text(pageCount - 4);
                        pagin.find(".paginThird").attr("rel", pageCount - 3);
                        pagin.find(".paginThird").text(pageCount - 3);
                        pagin.find(".paginFourth").attr("rel", pageCount - 2);
                        pagin.find(".paginFourth").text(pageCount - 2);
                        pagin.find(".paginFifth").attr("rel", pageCount - 1);
                        pagin.find(".paginFifth").text(pageCount - 1);
                        switch (pageIndex) {
                            case pageCount - 3:
                                choosePagin = pagin.find(".paginThird");
                                break;
                            case pageCount - 2:
                                choosePagin = pagin.find(".paginFourth");
                                break;
                            case pageCount - 1:
                                choosePagin = pagin.find(".paginFifth");
                                break;
                            case pageCount:
                                choosePagin = pagin.find(".paginEnd");
                                break;
                        }
                    }
                    if (choosePagin) {
                        choosePagin.removeClass("page");
                        choosePagin.addClass("page-selected");
                    }
                }
                else {
                    nowPagin.removeClass("page-selected");
                    nowPagin.addClass("page");
                    switch (pageIndex) {
                        case 7:
                            pagin.find(".pagin7").removeClass("page");
                            pagin.find(".pagin7").addClass("page-selected");
                            break;
                        case 6:
                            pagin.find(".pagin6").removeClass("page");
                            pagin.find(".pagin6").addClass("page-selected");
                            break;
                        case 5:
                            pagin.find(".pagin5").removeClass("page");
                            pagin.find(".pagin5").addClass("page-selected");
                            break;
                        case 4:
                            pagin.find(".pagin4").removeClass("page");
                            pagin.find(".pagin4").addClass("page-selected");
                            break;
                        case 3:
                            pagin.find(".pagin3").removeClass("page");
                            pagin.find(".pagin3").addClass("page-selected");
                            break;

                        case 2:
                            pagin.find(".pagin2").removeClass("page");
                            pagin.find(".pagin2").addClass("page-selected");
                            break;
                        case 1:
                            pagin.find(".pagin1").removeClass("page");
                            pagin.find(".pagin1").addClass("page-selected");
                            break;
                    }
                }
            }

            function prevPage() {
                var nowPagin = pagin.find(".page-selected");
                var prevNum = Number($(nowPagin).attr("rel")) - 1;
                if (isMore == true) {
                    if (prevNum < 1) {
                        prevNum++;
                        return;

                    }
                    else {
                        if (prevNum >= 4 && prevNum <= pageCount - 4) {
                            if (prevNum == 4) {
                                pagin.find(".paginLeft").hide();
                                pagin.find(".paginRight").show();
                            } else {
                                pagin.find(".paginLeft").show();
                                pagin.find(".paginRight").show();
                            }
                            choosePagin = pagin.find(".paginThird");
                            choosePagin.attr("rel", prevNum);
                            choosePagin.text(prevNum);
                            pagin.find(".paginFirst").attr("rel", prevNum - 2);
                            pagin.find(".paginFirst").text(prevNum - 2);
                            pagin.find(".paginSecond").attr("rel", prevNum - 1);
                            pagin.find(".paginSecond").text(prevNum - 1);
                            pagin.find(".paginFourth").attr("rel", prevNum + 1);
                            pagin.find(".paginFourth").text(prevNum + 1);
                            pagin.find(".paginFifth").attr("rel", prevNum + 2);
                            pagin.find(".paginFifth").text(prevNum + 2);
                        }
                        if (prevNum > pageCount - 4) {
                            pagin.find(".paginLeft").show();
                            pagin.find(".paginRight").hide();
                            switch (prevNum) {
                                case pageCount - 3:
                                    pagin.find(".paginThird").removeClass("page");
                                    pagin.find(".paginThird").addClass("page-selected");
                                    break;
                                case pageCount - 2:
                                    pagin.find(".paginFourth").removeClass("page");
                                    pagin.find(".paginFourth").addClass("page-selected");
                                    break;
                                case pageCount - 1:
                                    pagin.find(".paginFifth").removeClass("page");
                                    pagin.find(".paginFifth").addClass("page-selected");
                                    break;
                                case pageCount:
                                    pagin.find(".paginEnd").removeClass("page");
                                    pagin.find(".paginEnd").addClass("page-selected");
                                    break;
                            }
                            nowPagin.removeClass("page-selected");
                            nowPagin.addClass("page");
                        }
                        if (prevNum < 4) {
                            pagin.find(".paginLeft").hide();
                            pagin.find(".paginRight").show();
                            switch (prevNum) {
                                case 4:
                                    pagin.find(".paginThird").removeClass("page");
                                    pagin.find(".paginThird").addClass("page-selected");
                                    break;
                                case 3:
                                    pagin.find(".paginSecond").removeClass("page");
                                    pagin.find(".paginSecond").addClass("page-selected");
                                    break;
                                case 2:
                                    pagin.find(".paginFirst").removeClass("page");
                                    pagin.find(".paginFirst").addClass("page-selected");
                                    break;
                                case 1:
                                    pagin.find(".paginBegin").removeClass("page");
                                    pagin.find(".paginBegin").addClass("page-selected");
                                    break;
                            }
                            nowPagin.removeClass("page-selected");
                            nowPagin.addClass("page");
                        }
                    }
                }
                else {
                    if (prevNum < 1) {
                        prevNum++;
                        return;
                    }
                    else {
                        nowPagin.removeClass("page-selected");
                        nowPagin.addClass("page");
                        switch (prevNum) {
                            case 6:
                                pagin.find(".pagin6").removeClass("page");
                                pagin.find(".pagin6").addClass("page-selected");
                                break;
                            case 5:
                                pagin.find(".pagin5").removeClass("page");
                                pagin.find(".pagin5").addClass("page-selected");
                                break;
                            case 4:
                                pagin.find(".pagin4").removeClass("page");
                                pagin.find(".pagin4").addClass("page-selected");
                                break;
                            case 3:
                                pagin.find(".pagin3").removeClass("page");
                                pagin.find(".pagin3").addClass("page-selected");
                                break;

                            case 2:
                                pagin.find(".pagin2").removeClass("page");
                                pagin.find(".pagin2").addClass("page-selected");
                                break;
                            case 1:
                                pagin.find(".pagin1").removeClass("page");
                                pagin.find(".pagin1").addClass("page-selected");
                                break;
                        }
                    }
                }

                if (options.onPageChanged && typeof (options.onPageChanged) == "function") {
                    pagin.currentPageIndex = prevNum;
                    options.onPageChanged(prevNum, pagin);
                }
            }

            function nextPage() {
                var nowPagin = pagin.find(".page-selected");
                var nextNum = Number($(nowPagin).attr("rel")) + 1;

                if (isMore == true) {
                    if (nextNum > pageCount) {
                        nextNum--;
                        return;
                    }
                    else {
                        if (nextNum >= 5 && nextNum <= pageCount - 3) {
                            if (nextNum == pageCount - 3) {
                                pagin.find(".paginLeft").show();
                                pagin.find(".paginRight").hide();
                            }
                            else {
                                pagin.find(".paginLeft").show();
                                pagin.find(".paginRight").show();
                            }
                            choosePagin = $(".paginThird");
                            choosePagin.attr("rel", nextNum);
                            choosePagin.text(nextNum);
                            pagin.find(".paginFirst").attr("rel", nextNum - 2);
                            pagin.find(".paginFirst").text(nextNum - 2);
                            pagin.find(".paginSecond").attr("rel", nextNum - 1);
                            pagin.find(".paginSecond").text(nextNum - 1);
                            pagin.find(".paginFourth").attr("rel", nextNum + 1);
                            pagin.find(".paginFourth").text(nextNum + 1);
                            pagin.find(".paginFifth").attr("rel", nextNum + 2);
                            pagin.find(".paginFifth").text(nextNum + 2);
                        }
                        if (nextNum < 5) {
                            pagin.find(".paginLeft").hide();
                            pagin.find(".paginRight").show();
                            switch (nextNum) {
                                case 1:
                                    pagin.find(".paginBegin").removeClass("page");
                                    pagin.find(".paginBegin").addClass("page-selected");
                                    break;
                                case 2:
                                    pagin.find(".paginFirst").removeClass("page");
                                    pagin.find(".paginFirst").addClass("page-selected");
                                    break;
                                case 3:
                                    pagin.find(".paginSecond").removeClass("page");
                                    pagin.find(".paginSecond").addClass("page-selected");
                                    break;
                                case 4:
                                    pagin.find(".paginThird").removeClass("page");
                                    pagin.find(".paginThird").addClass("page-selected");
                                    break;
                            }
                            nowPagin.removeClass("page-selected");
                            nowPagin.addClass("page");
                        }
                        if (nextNum > pageCount - 3) {
                            pagin.find(".paginLeft").show();
                            pagin.find(".paginRight").hide();
                            switch (nextNum) {
                                case pageCount - 2:
                                    pagin.find(".paginFourth").removeClass("page");
                                    pagin.find(".paginFourth").addClass("page-selected");
                                    break;
                                case pageCount - 1:
                                    pagin.find(".paginFifth").removeClass("page");
                                    pagin.find(".paginFifth").addClass("page-selected");
                                    break;
                                case pageCount:
                                    pagin.find(".paginEnd").removeClass("page");
                                    pagin.find(".paginEnd").addClass("page-selected");
                                    break;
                            }
                            nowPagin.removeClass("page-selected");
                            nowPagin.addClass("page");
                        }
                    }
                }
                else {
                    if (nextNum > pageCount) {
                        nextNum--;
                        return;
                    }
                    else {
                        nowPagin.removeClass("page-selected");
                        nowPagin.addClass("page");
                        switch (nextNum) {
                            case 2:
                                pagin.find(".pagin2").removeClass("page");
                                pagin.find(".pagin2").addClass("page-selected");
                                break;
                            case 3:
                                pagin.find(".pagin3").removeClass("page");
                                pagin.find(".pagin3").addClass("page-selected");
                                break;
                            case 4:
                                pagin.find(".pagin4").removeClass("page");
                                pagin.find(".pagin4").addClass("page-selected");
                                break;
                            case 5:
                                pagin.find(".pagin5").removeClass("page");
                                $(".pagin5").addClass("page-selected");
                                break;

                            case 6:
                                pagin.find(".pagin6").removeClass("page");
                                pagin.find(".pagin6").addClass("page-selected");
                                break;
                            case 7:
                                pagin.find(".pagin7").removeClass("page");
                                pagin.find(".pagin7").addClass("page-selected");
                                break;
                        }
                    }
                }
                if (options.onPageChanged) {
                    pagin.currentPageIndex = nextNum;
                    options.onPageChanged(nextNum, pagin);
                }
            }

            $(pagin).ready(function () {
                pageCount = Math.ceil(options.recordCount / options.pageSize);
                if (pageCount > 7) {
                    isMore = true;
                    var item = '<div class="clearfloat" style="margin: auto"><span style="float: left">共 <span>' + options.recordCount + '</span> 条记录</span>';
                    item += '<div class="div-page">'
                            + '<div id="" class="page2 pageRollPrev">上一页</div>'
                            + '<div id="" rel="1" class="page-selected paginBegin">1</div>'
                            + '<div id=""  class="page1 paginLeft">...</div>'
                            + '<div id="" rel="2" class="page paginFirst">2</div>'
                            + '<div id="" rel="3" class="page paginSecond">3</div>'
                            + '<div id="" rel="4" class="page paginThird">4</div>'
                            + '<div id="" rel="5" class="page paginFourth">5</div>'
                            + '<div id="" rel="6" class="page paginFifth">6</div>'
                            + '<div id="" class="page1 paginRight">...</div>'
                            + '<div id="" class="page paginEnd" rel="' + pageCount + '" >' + pageCount + '</div>'
                            + '<div id="" class="page2 pageRollNext">下一页</div></div><span style="float: left">到第 <input class="txtJumpPafe" type="text"/> 页</span><div class="OKjumpPage">确定</div></div>';

                    pagin.find(".pageItem:eq(4)")
                }
                else {
                    if (pageCount <= 1) {
                        $(pagin).find(".div-page").hide();
                        pagin.prepend('<div class="clearfloat" ><span style="float: left;font-size:14px" >共检索到 <span>' + options.recordCount + '</span> 条记录</span></div>');
                        return;
                    }

                    isMore = false;
                    var item = '<div class="clearfloat" style="margin: auto"><span style="float: left">共 <span>' + options.recordCount + '</span> 条记录</span>';
                    item += '<div class="div-page"><div id="" class="page2 pageRollPrev">上一页</div><div  id="" class="page-selected pagin1" rel="1">1</div>';
                    for (i = 2; i <= pageCount; i++) {
                        item += '<div id="" class="page pagin' + i + '" rel="' + i + '">' + i + '</div>';
                    }
                    item += '<div id="" class="page2 pageRollNext">下一页</div></div><span style="float: left">到第 <input class="txtJumpPafe" type="text"/> 页</span><div class="OKjumpPage">确定</div></div>'
                }
                pagin.prepend(item);
                pagin.off("click", ".page").on("click", ".page", function () {
                    choosePage(Number($(this).attr("rel")));
                });

                pagin.off("click", ".pageRollNext").on("click", ".pageRollNext", function () {
                    nextPage();
                });
                pagin.off("click", ".pageRollPrev").on("click", ".pageRollPrev", function () {
                    prevPage();
                });
                pagin.find(".paginRight").show();
                pagin.find(".txtJumpPafe").keypress(function () {
                    var eventObj = event || e;
                    var keyCode = eventObj.keyCode || eventObj.which;
                    if (keyCode >= 48 && keyCode <= 57)
                        return true;
                    else
                        return false;
                }).blur(function () {
                    if (!$(this).val() || (Number($(this).val()) <= pageCount && Number($(this).val())) >= 1) {
                        return true;
                    }
                    else {
                        alert("超过所选范围,或者输入内容不规范");
                        $(this).val("");
                    }
                }).keyup(function () {
                    if (event.keyCode == 13) {
                        if ($(this).val() == "" || !$(this).val()) {
                        }
                        else {
                            if ((Number($(this).val()) <= pageCount && Number($(this).val())) >= 1) {
                                choosePage(Number($(this).val()));
                            }
                            else {
                                alert("超过所选范围,或者输入内容不规范");
                            }
                            $(this).val("");
                        }
                    }
                });
                pagin.off("click", ".OKjumpPage").on("click", ".OKjumpPage", function () {
                    if (pagin.find(".txtJumpPafe").val() == "" || !pagin.find(".txtJumpPafe").val()) {
                    }
                    else {
                        if ((Number(pagin.find(".txtJumpPafe").val()) <= pageCount && Number(pagin.find(".txtJumpPafe").val())) >= 1) {
                            choosePage(Number(pagin.find(".txtJumpPafe").val()));
                        }
                        else {
                            alert("超过所选范围,或者输入内容不规范");
                        }
                        pagin.find(".txtJumpPafe").val("");
                    }
                });

                pagin.setPageIndex(options.pageIndex);
            });

            pagers.push(pagin);

            if (options.pageIndex == 0) {
                pagin.find(".page-selected").removeClass("page-selected").addClass("page");
                pagin.find(".pagin1,.paginBegin").removeClass("page").addClass("page-selected");
            }
        });

        if (pagers.length == 1) {
            return pagers[0];
        }

        return pagers;
    }
})($);