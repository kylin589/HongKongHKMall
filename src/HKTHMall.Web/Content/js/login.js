$(document).ready(function () {
    var name = getCookie("ErrorAccount");
    if (name != null){
        $("#licode").show();
        $("#icode").hide();
    }
    else {
        $("#licode").hide();
    }
});


var codeFlag=true;
$("#c_phone").blur(function () {
    var phones = $.trim($("#c_phone").val());
    if (!new RegExp(/^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/).test(phones)) {
        $("#iphone").show();
        return false;
    }
    $("#iphone").hide();
});

$("#c_psw1").blur(function () {
    var pwd = $.trim($("#c_psw1").val());
    if (pwd=="") {
        $("#ipwd").show();
        return false;
    }
    $("#ipwd").hide();
});

$("#c_yys").blur(function () {
    checkCode();
});

function checkCode(){
    var code = $.trim($("#c_yys").val());
    if (code == "") {
        $("#icode").show();
        return false;
    }
    $.ajax({
        url: "/Login/VerifyCode",
        dataType: "json",
        type: "post",
        data: { "verifyCode": code },
        async: false,
        success: function (data) {
            if (data.rs == 0) {
                $("#icode").show();
                codeFlag = false;
            } else {
                $("#icode").hide();
                codeFlag = true;
            }
        }
    });
    return codeFlag;
}

$(".c_log_button").click(function () {
    login();
});

/*登录*/
function login() {
    var phone = $.trim($("#c_phone").val());
    var pwd = $.trim($("#c_psw1").val());
    var allThrough = true;
    var validateCode = "";
    if ($("#licode").is(":visible")) {
        codeFlag = checkCode();
        validateCode = $.trim($("#c_yys").val());
        if (validateCode == "" || !codeFlag) {
            $("#icode").show();
            allThrough = false;      
        } else {
            $("#icode").hide()
        }
    }
    if (!new RegExp(/^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/).test(phone)) {
        $("#iphone").show();
        allThrough = false;
    } else {
        $("#iphone").hide();
    }
    if (pwd == "") {
        $("#ipwd").show();
        allThrough = false;
    } else {
        $("#ipwd").hide();
    }
  
    if (!allThrough) {
        return false;
    }
    $("#denglu").attr("disabled", "disabled").html($commonLang.DURING_LOGIN);

    $.ajax
    ({
        url: "/Login/Index",
        dataType: "json",
        type: "POST",
        //传送请求数据
        data: { account: phone, password: pwd, IsJz: true, code: validateCode },
        success: function (data) {
            if (data.status == 0) {
                if (data.type == 1) {
                    $("#iphone").show().html(data.message);
                } else {
                    $("#ipwd").show().html(data.message);
                    if (data.times != undefined && data.times == 3) {
                        addCookie("ErrorAccount", phone, 30);
                        $("#licode").show();
                        $("#icode").hide();
                    } else if (data.times != undefined && data.times > 3) {
                        $("#ImgCode")[0].src = "/Login/MakeCode" + "?t=" + (new Date()).getTime();
                    }
                }               
                $("#denglu").removeAttr("disabled").html($commonLang.LOGIN);
                return false;
            } else {
                var name = getCookie("ErrorAccount");
                if (name != null) {
                    delCookie(name);
                }               
                var returnUrl = $.getUrlParam('ReturnUrl');
                if (returnUrl)
                    window.location.href = returnUrl;
                else
                    window.location.href = "/Home/Index";

            }
        }
    });
}


$("input").keyup(function (e) {
    var evn = e || window.event;
    if (evn.keyCode == 13) {
        login();
    }
});
//获取url参数
$.getUrlParam = function(name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}


function addCookie(objName, objValue, objHours) {
    var str = objName + "=" + escape(objValue);
    if (objHours > 0) { 
        var date = new Date();
        var ms = objHours * 60 * 1000;
        date.setTime(date.getTime() + ms);
        str += "; expires=" + date.toGMTString();
    }
    document.cookie = str;
}

function getCookie(name) {
    //获得cookie  
    var bikky = document.cookie;
     //alert(bikky);
    name += "=";
    var i = 0;
    while (i < bikky.length) {
        var offset = i + name.length;
        if (bikky.substring(i, offset) == name) {
            var endstr = bikky.indexOf(";", offset);
            if (endstr == -1) endstr = bikky.length;
            return unescape(bikky.substring(offset, endstr));
        }
        i = bikky.indexOf(" ", i) + 1;
        if (i == 0) break;
    }
    return null;
}

function delCookie(name){
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = "ErrorAccount=" + name + "; expires=" + date.toGMTString();
}