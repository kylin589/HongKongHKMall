
jQuery(function () {
    var pathname = window.location.pathname;
    $("div.mamain-lef div.manage-menu ul li a").each(function () {

        if ($(this).attr("href") == pathname) {
            $(this).addClass("current");
        }
    });
});
var alertdata = { isshow: false, alserarray: [], arrayfun: [] };
window.alert = function (str, fun) {
    alertdata.alserarray.push(str);
    alertdata.arrayfun.push(fun);
    showtest();
}
function showtest() {
    if (alertdata.isshow) { return; }
    var msg = "";
    if (alertdata.alserarray.length > 0) {
        msg = alertdata.alserarray[0];
    } else { return; }
    $("#footerShangj-re").text(msg);
    $("#footermask").show();
    $("#footerShangj-Che").show();
    alertdata.isshow = true;
}
function closemask(callback) {
    alertdata.isshow = false;
    $("#footermask").hide();
    $("#footerShangj-Che").hide();
    if (typeof (callback) == "function") {
        callback();
    }
    if (alertdata.arrayfun.length > 0) {
        if (typeof alertdata.arrayfun[0] == "function") {
            var s = alertdata.arrayfun[0];
            s();
        }
        alertdata.arrayfun.shift();
    }
    if (alertdata.alserarray.length > 0) {
        alertdata.alserarray.shift();
        if (alertdata.alserarray.length > 0) {
            showtest();
        }
    }
}
//分享 shl
function setFenX(title, content, url) {
    content = content.length > 200 ? content.substring(0, 200) + "......" : content;
    bShare.entries = [];
    bShare.addEntry({
        url: url ? url : window.location,
        title: "【" + title + "】",
        summary: content,
        vUid: '', vEmail: '', vTag: 'BSHARE'
    });
    bShare.isReady = false;
    bShare.completed = false;
    bShare.start();
}

//倒计时有时差 shl
function dmiao(obj, time, fun) {
    $(obj).text(time);
    if (time <= 0) { fun(); return; }
    time -= 1;
    setTimeout(function () {
        dmiao(obj, time, fun);
    }, 1000);
}