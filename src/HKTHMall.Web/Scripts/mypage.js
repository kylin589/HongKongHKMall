//数据加载方法统一为GetDataList
function GetPageHtml(pagedIndex, pagedSize, totalCount) {
    var tempNum = 3;
    var html = "";
    var total = (totalCount % pagedSize) > 0 ? (Math.floor(totalCount / pagedSize) + 1) : Math.floor(totalCount / pagedSize);
    if (totalCount > pagedSize) {
        var buttonHtml = "<li class='dib'><a href='#' class='g6 PageBut " + (pagedIndex == 0 ? "bgf0" : "") + "'" + (pagedIndex > 0 ? "onclick='GetDataList(" + (pagedIndex - 1) + "," + pagedSize + ")'" : "") + ">" + (pagedIndex == 0 ? "" : "<<") + "上一页</a></li>&nbsp;";
        if (total <= tempNum * 2) {
            for (var i = 0; i < total; i++) {
                html += "<li class='dib'><a href='#' class='g6 PageNum " + (pagedIndex == i ? "bgf0" : "") + "' " + (pagedIndex != i ? "onclick='GetDataList(" + i + "," + pagedSize + ")'" : "") + ">" + (i + 1) + "</a></li>&nbsp;";
            }
        } else {
            var starttop = pagedIndex - tempNum >= 0 ? pagedIndex - tempNum : 0;
            for (var i = starttop; i < pagedIndex; i++) {
                html += "<li class='dib'><a href='#' class='g6 PageNum " + (pagedIndex == i ? "bgf0" : "") + "' " + (pagedIndex != i ? "onclick='GetDataList(" + i + "," + pagedSize + ")'" : "") + ">" + (i + 1) + "</a></li>&nbsp;";
            }
            for (var i = pagedIndex; i < pagedIndex + tempNum; i++) {
                if (i < total) {
                    html += "<li class='dib'><a href='#' class='g6 PageNum " + (pagedIndex == i ? "bgf0" : "") + "' " + (pagedIndex != i ? "onclick='GetDataList(" + i + "," + pagedSize + ")'" : "") + ">" + (i + 1) + "</a></li>&nbsp;";
                }
            }
            var startboot = (pagedIndex + tempNum) >= (total - tempNum) ? (pagedIndex + tempNum) : total - tempNum;
            if (startboot < total) {
                if (startboot != (pagedIndex + tempNum)) {
                    html += "<li class='dib'><a href='#' class='g6 PageEll'>...</a></li>&nbsp;";
                }
            } else {
                var tempHtml = "";
                for (var i = 0; i < tempNum ; i++) {
                    tempHtml += "<li class='dib'><a href='#' class='g6 PageNum " + (pagedIndex == i ? "bgf0" : "") + "' " + (pagedIndex != i ? "onclick='GetDataList(" + i + "," + pagedSize + ")'" : "") + ">" + (i + 1) + "</a></li>&nbsp;";
                }
                tempHtml += "<li class='dib'><a href='#' class='g6 PageEll'>...</a></li>&nbsp;";
                html = tempHtml + html;
            }
            for (var i = startboot; i < total; i++) {
                html += "<li class='dib'><a href='#' class='g6 PageNum " + (pagedIndex == i ? "bgf0" : "") + "' " + (pagedIndex != i ? "onclick='GetDataList(" + i + "," + pagedSize + ")'" : "") + ">" + (i + 1) + "</a></li>&nbsp;";
            }
        }
        html += "<li class='dib'><a href='#' class='g6 PageBut " + ((pagedIndex + 1) >= total ? "bgf0" : "") + "'" + ((pagedIndex + 1) < total ? "onclick='GetDataList(" + (pagedIndex + 1) + "," + pagedSize + ")'" : "") + ">下一页" + ((pagedIndex + 1) < total ? ">>" : "") + "</a></li>";
        html = buttonHtml + html;
    }
    html += "<li class='dib f14 g9'>共" + total + "页&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;到第</li>";
    html += "<li class='dib f14 g9'><input type='text' value='" + (pagedIndex + 1) + "' onkeyup='PageKeyUp()'  maxlength='4' class='couNum'>页<input type='button' value='确定' onclick='SearchPage()' class='PageSure'></li>";
    $(".EvaluationPage").html(html);

}

function SearchPage() {
    GetDataList(parseInt($(".couNum").val() - 1), pagedSize);
}

function PageKeyUp() {
    $(".couNum").val($(".couNum").val().replace(/[^\d]/g, ''));
}