
function consoleLog(strMsg) {
    try {
        console.log(strMsg);
    }
    catch (e) {

    }
}

function CommonHelper() {
    this.stringEqualIgnoreCase = function (obj1, obj2) {
        return stringEqualIgnoreCase_Private(obj1, obj2);
    };

    function stringEqualIgnoreCase_Private(obj1, obj2) {
        if (obj1 == null && obj2 == null) return true;
        return singleToString_Private(obj1).toLowerCase() == singleToString_Private(obj2).toLowerCase();
    }

    this.singleToString = function (obj) {
        return singleToString_Private(obj);
    };

    function singleToString_Private(obj) {
        if (obj == undefined) return "";
        if (obj == null) return "";
        return obj + "";
    }

    this.getQueryStringValue = function (strUrl, strKey) {
        var idx = strUrl.indexOf("?");
        if (idx >= 0) {
            strUrl = strUrl.substring(idx + 1);
        }
        var arrKeyValue = strUrl.split("&");
        for (var i = 0; i < arrKeyValue.length; i++) {
            var key = arrKeyValue[i].substring(0, arrKeyValue[i].indexOf("="));
            var value = arrKeyValue[i].substring(arrKeyValue[i].indexOf("=") + 1);

            if (stringEqualIgnoreCase_Private(key, strKey) == true) {
                return value;
            }
        }

        return "";
    };

    this.trim = function (strSrc) {
        return singleToString_Private(strSrc).replace(/(^\s+)|(\s+$)/g, "");
    };

    // 删除前后指定字符
    this.trimPattern = function (strSrc, strPattern) {
        var reg = new RegExp("(^[" + strPattern + "]+)|([" + strPattern + "]+$)", "g");
        return singleToString_Private(strSrc).replace(reg, "");
    };

    this.isNullOrEmpty = function (strSrc) {
        return singleToString_Private(strSrc) == "";
    };

    // 转换为JSON对象,无异常方法
    this.asToJsonObj = function (strSrc) {
        try {
            if (strSrc == undefined || strSrc == null) return null;
            return JSON.parse(strSrc);
        } catch (ex) {
            consoleLog("CommonHelper.asToJsonObj(" + strSrc + ")");
            consoleLog(ex.description);
            consoleLog(ex.stack);
        }
        return null;
    };

    // JSON对象转换为字符串
    this.jsonObjToString = function (jsonObj) {
        try {
            return JSON.stringify(jsonObj);
        } catch (ex) {

        }
        return "";
    };


    // 判断是否数字:整数、小数、负数
    this.isNum = function (strSrc) {
        var strNew = strSrc + "";

        // 是否小数、整数（不能匹配一位整数）
        var isNum = /^[-]?\d+[.]?\d+$/g.test(strNew);
        // 是否整数（可匹配一位整数）
        var isInteger = /^[-]?\d+$/g.test(strNew);

        return isNum || isInteger;
    };

    // 是否整数
    this.isInteger = function (strSrc) {
        var strNew = strSrc + "";
        if (strNew == "0") return true;
        var isInt = /^[-]?[1-9]+\d*$/g.test(strNew);
        return isInt;
    };
    // 是否整数
    this.isInteger = function (strSrc) {
        var strNew = strSrc + "";
        if (strNew == "0") return true;
        var isInt = /^[-]?[1-9]+\d*$/g.test(strNew);
        return isInt;
    };

    // 是否正整数
    this.isPositiveInt = function (strSrc) {
        var strNew = strSrc + "";
        if (strNew == "0") return true;
        var isInt = /^[1-9]+\d*$/g.test(strNew);
        return isInt;
    };

    // 获取当前登录的用户
    this.getCur_YH_User = function () {
        var rslt = $.ajax("/Login/getCur_YH_User", { type: "POST", async: false });
        if (rslt.responseText == "") { rslt.responseText = null; }
        return myCommonHelper.asToJsonObj(rslt.responseText);
    };

    // 同步请求
    this.getAjaxStringSync = function (url, type, data) {
        var rslt = $.ajax(url, { type: type, async: false, data: data });
        return rslt.responseText;
    }


    // 将字符串前面补0;iLength-补0后保留的长度; 例如 ("1",2)--> "01"
    this.paddingZeroOnLeft = function (src, iLength) {
        return paddingZeroOnLeft_Private(src, iLength);
    }

    function paddingZeroOnLeft_Private(src, iLength) {
        var sbZero = "";
        for (var i = 0; i < iLength; i++) {
            sbZero += "0";
        }
        var strRslt = sbZero + src;
        return strRslt.substring(strRslt.length - iLength);
    }

    // 将Date对象转换为字符串 yyyy-MM-dd HH:mm:ss
    this.dtFormat = function (dt) {
        return dt.getFullYear() + "-" + paddingZeroOnLeft_Private(dt.getMonth() + 1, 2) + "-" + paddingZeroOnLeft_Private(dt.getDate(), 2) + " " +
            paddingZeroOnLeft_Private(dt.getHours(), 2) + ":" + paddingZeroOnLeft_Private(dt.getMinutes()) + ":" + paddingZeroOnLeft_Private(dt.getSeconds());
    }

    // 根据QueryString参数名称获取值
    this.getQueryStringByName = function (name) {
        //http://hsj69106.blog.51cto.com/1017401/656440/
        var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
        if (result == null || result.length < 1) {
            return "";
        }
        return result[1];
    }

    // 判断一个DOM是否不在浏览器的可视区域内
    this.isNotVisual=function(jDom) {
        return jQuery(window).scrollTop() > (jDom.offset().top + jDom.outerHeight()) ||
            jQuery(window).scrollTop() < (jDom.offset().top - jQuery(window).height());
    }
}

var myCommonHelper = new CommonHelper();