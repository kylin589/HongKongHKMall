function UpdatePager(pageIndex, pageSize, count, container, funcName, _Type) {
    var pageSum = Math.ceil(count / pageSize); //总页数
    UpdatePager(pageIndex, pageSum, container, funcName, _Type);
}

function UpdatePager(pageIndex, pageCount, container, funcName, _Type) {
    var PagerHtml = "";
    var RecordCount = pageCount;
    var curPage = pageIndex;
    if (typeof (_Type) == "object" && _Type) {
        _Type = JSON.stringify(_Type);
    }
    //var PageSize = pageSize;
    var pageSum = pageCount; //总页数
    if (pageSum <= 1) {
        container.html("")
        return;
    }
    var lihtml = "<li><a class='cut' href='javascript:;'>{0}</a></li>";
    if (RecordCount > 0) {
        var pageMaxSum = 2; //分页栏当前页按钮或左或右最多显示的按钮
        if (curPage < 1) curPage = 1;
        if (curPage > pageSum) curPage = pageSum;
        PagerHtml += "<ul>";
        //上一页
        if (1 != curPage) {
            if (_Type) {
                PagerHtml += "<li><a class='secPrevPage' href='javascript:;' onclick='" + funcName + "(" + (pageIndex - 1) + "," + _Type + ");'><i class='fa-angle-left'></i>" + $commonLang.ORDER_LIST_PREVIOUSPAGE + "</a></li>";//上一页
            }
            else {
                PagerHtml += "<li><a class='secPrevPage' href='javascript:;' onclick='" + funcName + "(" + (pageIndex - 1) + ");'><i class='fa-angle-left'></i>" + $commonLang.ORDER_LIST_PREVIOUSPAGE + "</a></li>";//上一页
            }
        }
        var startIndex = 1;
        var endIndex = pageSum;
        if (pageSum > pageMaxSum * 2 + 1) {
            if (curPage - pageMaxSum > 0) {
                if (curPage + pageMaxSum < pageSum) {
                    startIndex = curPage - pageMaxSum;
                    endIndex = curPage + pageMaxSum;
                }
                else {
                    startIndex = pageSum - pageMaxSum * 2;
                }
            }
            else {
                endIndex = pageMaxSum * 2 + 1;
            }
        }
        if (pageSum > 1) {
            if (startIndex == 2) {
                PagerHtml += GetPagerNum(1, pageIndex, funcName, _Type);
                endIndex--;
            }
            else if (startIndex > 2) {
                PagerHtml += GetPagerNum(1, pageIndex, funcName, _Type);
                PagerHtml += "<li><a href='javascript:;'>...</a></li>";
            }
            //页码
            for (var i = startIndex; i <= endIndex; i++) {
                if (curPage == i) {
                    PagerHtml += "<li><a href='javascript:;' class=\"secPageOn\">" + curPage + "</a></li>";
                }
                else {
                    PagerHtml += GetPagerNum(i, pageIndex, funcName, _Type);
                }
            }
            if (endIndex < pageSum - 1) {
                PagerHtml += "<li><a href='javascript:;'>...</a></li>";
                PagerHtml += GetPagerNum(pageSum, pageIndex, funcName, _Type);
            }
            else if (endIndex < pageSum) {
                if (curPage == pageSum) {
                    PagerHtml += "<li><a href='javascript:;' class=\"secPageOn\">" + curPage + "</a></li>";
                }
                else
                    PagerHtml += GetPagerNum(pageSum, pageIndex, funcName, _Type);
            }
        }
        //下一页
        if (curPage != pageSum && pageSum > 1) {
            if (_Type) {
                PagerHtml += "<li><a class='secNextPage' href='javascript:;' onclick='" + funcName + "(" + (pageIndex + 1) + "," + _Type + ");'>" + $commonLang.ORDER_LIST_NEXTPAGE + "<i class='fa-angle-right'></i></a></li>";//下一页
            }
            else {
                PagerHtml += "<li><a class='secNextPage' href='javascript:;' onclick='" + funcName + "(" + (pageIndex + 1) + ");'>" + $commonLang.ORDER_LIST_NEXTPAGE + "<i class='fa-angle-right'></i></a></li>";//下一页
            }

        }
        PagerHtml += "</ul>";
    }
    container.html(PagerHtml);
}

function GetPagerNum(PageNo, PageIndex, funcName, _Type) {
    if (typeof (_Type) == "object" && _Type) {
        _Type = JSON.stringify(_Type);
    }
    return _Type ? "<li><a href='javascript:;' class='" + (PageNo == PageIndex ? "secPageOn" : "") + "' " + (PageNo == PageIndex ? "" : " onclick='" + funcName + "(" + PageNo + "," + _Type + ");' ") + " >" + PageNo + "</a></li>"
        : "<li><a href='javascript:;' class='" + (PageNo == PageIndex ? "secPageOn" : "") + "' " + (PageNo == PageIndex ? "" : " onclick='" + funcName + "(" + PageNo + ")' ") + " >" + PageNo + "</a></li>";
}



function UpdatePager1(pageIndex, pageSize, count, container, funcName, _Type) {
    var pageSum = Math.ceil(count / pageSize); //总页数
    UpdatePager1(pageIndex, pageSum, container, funcName, _Type);
}

function UpdatePager1(pageIndex, pageCount, container, funcName, _Type) {
    var PagerHtml = "";
    var RecordCount = pageCount;
    var curPage = pageIndex;
    if (typeof (_Type) == "object" && _Type) {
        _Type = JSON.stringify(_Type);
    }
    //var PageSize = pageSize;
    var pageSum = pageCount; //总页数
    if (pageSum <= 1) {
        container.html("")
        return;
    }
    var lihtml = "<li><a class='cut' href='javascript:;'>{0}</a></li>";
    if (RecordCount > 0) {
        var pageMaxSum = 2; //分页栏当前页按钮或左或右最多显示的按钮
        if (curPage < 1) curPage = 1;
        if (curPage > pageSum) curPage = pageSum;
        PagerHtml += "<ul>";
        //上一页
        if (1 != curPage) {
            if (_Type) {
                PagerHtml += "<li><a  href='javascript:;' onclick='" + funcName + "(" + (pageIndex - 1) + "," + _Type + ");'>" + $commonLang.ORDER_LIST_PREVIOUSPAGE + "</a></li>";//上一页
            }
            else {
                PagerHtml += "<li><a  href='javascript:;' onclick='" + funcName + "(" + (pageIndex - 1) + ");'>" + $commonLang.ORDER_LIST_PREVIOUSPAGE + "</a></li>";//上一页
            }
        }
        var startIndex = 1;
        var endIndex = pageSum;
        if (pageSum > pageMaxSum * 2 + 1) {
            if (curPage - pageMaxSum > 0) {
                if (curPage + pageMaxSum < pageSum) {
                    startIndex = curPage - pageMaxSum;
                    endIndex = curPage + pageMaxSum;
                }
                else {
                    startIndex = pageSum - pageMaxSum * 2;
                }
            }
            else {
                endIndex = pageMaxSum * 2 + 1;
            }
        }
        if (pageSum > 1) {
            if (startIndex == 2) {
                PagerHtml += GetPagerNum(1, pageIndex, funcName, _Type);
                endIndex--;
            }
            else if (startIndex > 2) {
                PagerHtml += GetPagerNum(1, pageIndex, funcName, _Type);
                PagerHtml += "<li><a href='javascript:;'>...</a></li>";
            }
            //页码
            for (var i = startIndex; i <= endIndex; i++) {
                if (curPage == i) {
                    PagerHtml += "<li class=\"ls_page_on\"><a href='javascript:;' >" + curPage + "</a></li>";
                }
                else {
                    PagerHtml += GetPagerNum(i, pageIndex, funcName, _Type);
                }
            }
            if (endIndex < pageSum - 1) {
                PagerHtml += "<li><a href='javascript:;'>...</a></li>";
                PagerHtml += GetPagerNum(pageSum, pageIndex, funcName, _Type);
            }
            else if (endIndex < pageSum) {
                if (curPage == pageSum) {
                    PagerHtml += "<li><a href='javascript:;' >" + curPage + "</a></li>";
                }
                else
                    PagerHtml += GetPagerNum(pageSum, pageIndex, funcName, _Type);
            }
        }
        //下一页
        if (curPage != pageSum && pageSum > 1) {
            if (_Type) {
                PagerHtml += "<li><a  href='javascript:;' onclick='" + funcName + "(" + (pageIndex + 1) + "," + _Type + ");'>" + $commonLang.ORDER_LIST_NEXTPAGE + "<i ></i></a></li>";//下一页
            }
            else {
                PagerHtml += "<li><a  href='javascript:;' onclick='" + funcName + "(" + (pageIndex + 1) + ");'>" + $commonLang.ORDER_LIST_NEXTPAGE + "<i ></i></a></li>";//下一页
            }

        }
        PagerHtml += "</ul><span class='clear clearfix'></span>";
    }
    container.html(PagerHtml);
}