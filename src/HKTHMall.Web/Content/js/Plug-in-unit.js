//jQuery("select,input").InputMsg();验证文本框
//jQuery('').IsAllThrough()验证是否全部通过
//jQuery("[data-time]").DjsTime();倒计时
(function ($) {
    //正则表达式
    var zzbds = {
        phone: { bds: /^1[3|4|5|7|8][0-9]\d{8}$/, msg: "手机号码格式不正确" },
        pwd: { bds: /[a-zA-z0-9]{8,16}/, msg: "8-16位字符,由字母和数字组成,区分大小写" },
        email: { bds: /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,4}$/, msg: "" },
        sfz: { bds: /^\d{15}$|(\d{17}[\\x,\X,\d]$)/, msg: "身份证号码不正确" },
        yyzz: { bds: /^\d{15}$/, msg: "请输入15位数字营业执照号" },
        swdjh: { bds: /(^\d{15}$)|(^\d{18}$)/, msg: "税务登记证号由15、18位数字组成" },
        qyzzjg: { bds: /^[\da-zA-Z]{8}$/, msg: "企业组织机构代码8位字母、数字组成" }
    };
    var settings = {
        /*InputMsg 方法参数列表*/
        EventType: "blur", //验证事件多个事件用,隔开 事件为jquery事件名称
        DataMsg: "data-msg",//默认提示语属性
        DataYz: "data-yz",//验证语句 格式 {bds:''},Contrast:'#id' Contrast对比控件ID bds正则表达式
        //Onchange:function()			如果是 下拉框做什么操作
        //TrueShow:function()		验证成功提示方法 可重写
        //FalseeShow:function() 验证失败提示方法 可重写
        //TrueFun:function() 验证失败提示方法 可重写
        //FalseFun:function() 验证失败提示方法 可重写

        /*DjsTime 方法参数列表*/
        attrName: "data-time", //时间属性 值为秒数 例如  <span data-aaa='500'></span>
        font: {},//颜色 时间高亮显示
        className: "",//类名 要为元素加入的 class
        TimeType: 1,//时间格式 1:0天0时0分0秒  2:0时0分0秒
        clearInterval: null
        //breakfun:function(){alert('倒计时完成了');}	 倒计时完成后 调用方法 可重写
    };
    var methods = {
        /*InputMsg 方法列表*/
        init: function (setting, obj) {
            jQuery.extend(settings, setting);
            jQuery(this).each(function (index, element) {
                var objthis = jQuery(element);
                var EventType = settings.EventType;
                if (objthis.is("select")) {
                    methods.Onchange(objthis);
                    EventType = "change";
                } else if (objthis.is(":checkbox")) {
                    EventType = "click";
                }
                objthis.live(EventType, function () {
                    methods.IsDan(objthis);
                });
            });
            return this;
        },
        TrueShow: function (obj) {
            if (settings.TrueShow) {
                settings.TrueShow.apply(this, arguments)
            }
        },
        FalseeShow: function (obj, msg) {
            //alert(msg);
            if (settings.FalseeShow) {
                settings.FalseeShow.apply(this, arguments)
            }
        },
        Onchange: function (obj) {
            if (settings.Onchange) {
                jQuery(obj).change(settings.Onchange)
            }
        },
        GetVal: function (obj) {

            var objthis = jQuery(obj);
            var text = null;

            if (objthis.is("select")) {
                text = objthis.get(0).selectedIndex == 0 ? false : true;
            }
            else if (objthis.is("input")) {
                switch (objthis.attr("type")) {
                    case "text":
                    case "password":
                    case "hidden":
                        text = objthis.val(); break;
                    case "checkbox":
                        text = objthis.is(":checked");
                        break;
                    default:
                        text = objthis.text(); break;
                }
            }
            else { text = objthis.html(); }

            return text;
        },
        IsDan: function (obj) {
            var msg = jQuery(obj).attr(settings.DataMsg);
            var text = methods.GetVal(obj);
            var YzData = eval('(' + jQuery(obj).attr(settings.DataYz) + ')');

            if (msg == undefined) {
                if (text == "") {
                    methods.TrueShow(obj);
                    return true;
                }
                if (YzData.bds) {
                    if (zzbds[YzData.bds]) {
                        YzData.bds = zzbds[YzData.bds].bds;
                        msg = zzbds[YzData.bds].msg;
                    }
                    try {

                        if (text.match(YzData.bds) != null) {
                            methods.TrueShow(obj);
                            return true;
                        } else {
                            msg = YzData.bdsmsg || zzbds[YzData.bds].msg || msg;
                            methods.FalseeShow(obj, msg);
                            return false;
                        }
                    }
                    catch (Exception) {
                        methods.FalseeShow(obj, '表达式错误');
                        throw "";
                    }
                }
                else {
                    methods.TrueShow(obj);
                    return true;
                }
            }
            if (text == "" || !text) {
                methods.FalseeShow(obj, msg);
                return false;
            }
            else if (typeof YzData == 'object') {
                if (YzData.bds) {
                    if (zzbds[YzData.bds]) {
                        msg = zzbds[YzData.bds].msg;
                        YzData.bds = zzbds[YzData.bds].bds;
                    }
                    try {
                        if (text.match(YzData.bds) != null) {
                            methods.TrueShow(obj);
                            return true;
                        } else {
                            msg = YzData.bdsmsg || msg;
                            methods.FalseeShow(obj, msg);
                            return false;
                        }
                    }
                    catch (e) {
                        methods.FalseeShow(obj, '表达式错误');
                        throw "bisds";
                    }
                } else if (YzData.Contrast) {
                    if (text != methods.GetVal(YzData.Contrast)) {
                        methods.FalseeShow(obj, msg);
                        return false;
                    } else {
                        methods.TrueShow(obj);
                        return true;
                    }
                } else {
                    alert("?");
                    return true;
                }
            }
            else {
                methods.TrueShow(obj);
                return true;
            }
        },
        IsAll: function (TrueFun, FalseFun) {
            var bool = true;
            jQuery(this).each(function (index, element) {
                if (!methods.IsDan(element)) {
                    bool = false;
                }
            });
            if (bool) { if (TrueFun && typeof TrueFun == "function") { TrueFun(); } }
            else { if (FalseFun && typeof FalseFun == "function") { FalseFun(); } }
            return bool;
        },

        /*InputMsg 方法列表*/
        breakfun: function (obj) {
            $(obj).hide();
            var _sttings = $(obj).data("data");
            window.clearInterval(_sttings.clearInterval);
            if (_sttings.breakfun) {
                _sttings.breakfun.apply(this, arguments)
            }
        },
        TimeJS: function (obj, DTimes, startTime) {
            var _sttings = $(obj).data("data");
            var guo = (new Date().getTime() - startTime) / 1000;
            var DTime = DTimes - parseInt(guo);
            if (DTime <= 0) {
                methods.breakfun(obj);
                return;
            }
            var mm = (parseInt(DTime / 60) % 60).toString(),
                ss = (DTime % 60).toString(),
                hh = (parseInt(DTime / 3600) % 24).toString(),
                day = parseInt(DTime / 86400).toString();
            jQuery(obj).html(methods.setClass(day, hh, mm, ss, _sttings));

        },
        setClass: function (day, h, m, s, setting) {
            if (setting.font) {
                if (setting.font.fontcolor) {
                    day = day.fontcolor(setting.font.fontcolor);
                    h = h.fontcolor(setting.font.fontcolor);
                    m = m.fontcolor(setting.font.fontcolor);
                    s = s.fontcolor(setting.font.fontcolor);
                }
                if (setting.font.fontsize) {
                    day = day.fontsize(setting.font.fontsize);
                    h = h.fontsize(setting.font.fontsize);
                    m = m.fontsize(setting.font.fontsize);
                    s = s.fontsize(setting.font.fontsize);
                }
            }
            //return day + "天" + h + "时" + m + "分" + s + "秒";
            return this.format(day) + ":" + this.format(h) + ":" + this.format(m) + ":" + this.format(s) + "";
        },
        format: function (i) {
            if (parseInt(i, 10) < 10) i = "0" + i;
            return i.toString();
        }
    };
    $.fn.InputMsg = function (method) {

        if (!method || typeof method === 'object') {
            return methods.init.apply(this, arguments);
        } else {
            return methods.IsAll.apply(this, arguments);
        }
    }
    $.fn.IsAllThrough = function () {
        return methods.IsAll.apply(this, arguments);
    };

    $.fn.zzbds = function (obj) {
        if (text.match(zzbds[obj].bds) != null) {
            return true;
        }
        return false;
    }

    $.fn.DjsTime = function (Times) {
        var $sttings = {}
        if (Times && typeof Times == "object") {
            jQuery.extend($sttings, settings, Times);
        }
        var startTime = new Date().getTime();
        jQuery(this).each(function (index, element) {
            var objthis = jQuery(element), time = parseInt(objthis.attr($sttings.attrName));
            objthis.data("data", $sttings);
            if (isNaN(time) || time <= 0) {
                methods.breakfun(element, index);
                return;
            }
            objthis.addClass($sttings.className);
            $sttings.clearInterval = setInterval(function () {
                methods.TimeJS(objthis, time, startTime, index);
            }, 555);
        });
        return this;
    }
    //只允许输入数字
    $.fn.InputInt = function (key) {
        jQuery(this).keydown(function (e) {
            var evn = e || window.event;
            if (evn.keyCode != 9 && evn.keyCode != 8 && !((evn.keyCode >= 96 && evn.keyCode <= 105) || (evn.keyCode >= 48 && evn.keyCode <= 57))) {
                return false;
            } else {
                if (evn.shiftKey) {
                    evn.preventDefault();
                    evn.stopPropagation();
                    return false;
                }
            }
        });
        return this;
    }

    /*分页开始*/

    var pgmethods = {
        AjaxFun: function (objdata, _pgsettings) {
            var txt = jQuery(_pgsettings.ajaxData).attr("data-xxx"), xata = txt.split("|");
            jQuery.post("/" + xata[xata.length - 1], objdata, function (data) {
                var s = jQuery("#" + xata[0]).offset().top - 38;
                jQuery("body,html").scrollTop(s)
                jQuery("#" + xata[0]).empty().append(data);
            });
        },
        HrefFun: function (obj, _pgsettings) {
            var s = getQueryString(_pgsettings.pgName);
            if (s == null) {
                var pd = window.location.search.split("=").length > 1 ? "&" : "?";
                window.location = window.location + pd + _pgsettings.pgName + "=" + obj;
            } else {
                var s = window.location.toString();
                var data = s.match(_pgsettings.pgName + '=[^&]*')
                window.location = s.replace(data, _pgsettings.pgName + "=" + obj);
            }
        },
        MsgFun: function () {
            alert("索引错误");
        }
    }
    $.fn.Page = function (settingsf, data) {
        var pgsettings = {
            prev: ".prev",
            next: ".next",
            Go: ".pggo",
            GoPg: ".pro_bk",
            pgName: "page",
            ajaxData: "ul[data-xxx]",
            PgCount: ".PgCount",
            PgIndex: ".PgIndex",
            type: 1,
            Form: null,
            isLoad: false
        };
        jQuery.extend(pgsettings, settingsf);
        $(document).on("keyup", pgsettings.GoPg, function (evn) { 
        //jQuery(pgsettings.GoPg).live("keyup", function (evn) {
            evn = evn || window.event;
            if (evn.keyCode == 13) {
                jQuery(this).parent().next().find("a").click();
            }
        });
        this.bind("click", function () {
            var objthis = jQuery(this), x = objthis.text(), pre = objthis.parent().parent().parent().parent();
            PgIndex = parseInt(pre.find(pgsettings.PgIndex).eq(0).val());
            if (isNaN(x)) {
                if (objthis.is(pgsettings.next)) {
                    if (objthis.is(".noprev")) return;
                    x = PgIndex + 1;
                }
                else if (objthis.is(pgsettings.prev)) {
                    if (objthis.is(".noprev")) return;
                    x = PgIndex - 1;
                }
                else if (objthis.is(pgsettings.Go)) {
                    x = parseInt(jQuery(pgsettings.GoPg, pre).eq(0).val())
                }
                else { return; }
            }
            var PgCount = parseInt(jQuery(pgsettings.PgCount, pre).val());
            if (x > PgCount || x < 1) {
                // pgmethods.MsgFun();
                // return;
                x = 1;
            }
            pre.find(pgsettings.PgIndex).val(x);
            switch (pgsettings.type) {
                case 1:
                    if (!data && typeof data != "object")
                        data = {};
                    jQuery.extend(data, { page: x });
                    pgsettings.ajaxData = $(this).parents("ul.tunepage");
                    pgmethods.AjaxFun(data, pgsettings);
                    break;
                case 2:
                    if (pgsettings.Form == null) {
                        jQuery("form:eq(1)").submit();
                    }
                    else { jQuery(pgsettings.Form).submit(); }
                    break;
                default:
                    pgmethods.HrefFun(x, pgsettings);
                    break;
            }
        })
    }

    $.fn.PNpage = function () {
        $(document).on("click", this, function () { 
       // jQuery(this).live("click", function () {
            var objthis = jQuery(this),
            PgIndex = parseInt(jQuery(pgsettings.PgIndex).eq(0).val()),
            PgCount = parseInt(jQuery(pgsettings.PgCount).eq(0).val());;
            if (objthis.is(".isnoprev")) { return; }
            else {
                if (objthis.is("next")) {
                    PgIndex += 1;
                } else if (objthis.is("prev")) {
                    PgIndex -= 1;
                }
                pgmethods.HrefFun(PgIndex);
            }
        });
    }
    /*分页结束*/


    /*图片切换开始*/
    var imgsetting = {
        left: ".tunele",//左
        right: ".tuneri",//右
        Label: "ul>li",//标签
        IsAutomatic: true,//是否自动切换
        time: 5000, //时间时间
        length: 0,
        index: 0,
        setTimeout: null

    }
    var imgmethods = {
        left: function (obj) {
            imgsetting.index -= 1;
            if (imgsetting.index <= 0) imgsetting.index = imgsetting.length;
            imgmethods.method(obj);
        },
        right: function (obj) {
            imgsetting.index += 1;
            if (imgsetting.index > imgsetting.length) imgsetting.index = 0;
            imgmethods.method(obj);
        },
        method: function (obj) {
            jQuery("div.banner3").css("background", jQuery(imgsetting.Label, obj).eq(imgsetting.index).attr("bkcolor"));
            jQuery(imgsetting.Label, obj).hide().eq(imgsetting.index).fadeIn();
            jQuery(".cur", obj).removeClass("cur");
            jQuery(".tunece>a", obj).eq(imgsetting.index).addClass("cur");
        }
    }
    $.fn.ImgSwitch = function (obj) {
        imgsetting.length = jQuery(imgsetting.Label, this).length - 1;
        jQuery.extend(imgsetting, obj);
        if (imgsetting.length < 1) return;
        var $this = this;
        if (imgsetting.IsAutomatic) {
            imgsetting.setTimeout = setInterval(function () { imgmethods.right($this) }, imgsetting.time);
        }
        jQuery(imgsetting.left).click(function () { imgmethods.left($this) })
        jQuery(imgsetting.right).click(function () { imgmethods.right($this) })
        jQuery(".tunece>a", $this).click(function () {
            imgsetting.index = jQuery(this).index();
            imgmethods.method($this);
            clearInterval(imgsetting.setTimeout);
            imgsetting.setTimeout = setInterval(function () { imgmethods.right($this) }, imgsetting.time);
        });
        $this.hover(function () { jQuery(imgsetting.left).show(); jQuery(imgsetting.right).show(); clearInterval(imgsetting.setTimeout); }, function () { jQuery(imgsetting.left).hide(); jQuery(imgsetting.right).hide(); imgsetting.setTimeout = setInterval(function () { imgmethods.right($this) }, imgsetting.time); })

    }
    /*图片切换结束*/


    /*jquery.scrollLoading.js开始*/
    /*用法
        <img data-url="网络地址" src="/images/load.jpg" >
        $("img[data-url]").scrollLoading();
     */
    $.fn.scrollLoading = function (options) {
        var defaults = {
            attr: "data-url"
        };
        var params = jQuery.extend({}, defaults, options || {});
        params.cache = [];
        jQuery(this).each(function (index, item) {
            var node = this.nodeName.toLowerCase(), url = jQuery(item).attr(params["attr"]);
            if (!url) { return; }
            //重组
            var data = {
                obj: jQuery(this),
                tag: node,
                url: url
            };
            params.cache.push(data);
        });

        //动态显示数据
        var loading = function () {
            var st = jQuery(window).scrollTop();
            if (document.documentElement && document.documentElement.scrollTop)
                st = document.documentElement.scrollTop;
            else {
                st = document.body.scrollTop;
            }
            var sth = st + jQuery(window).height();
            jQuery.each(params.cache, function (i, data) {
                var o = data.obj, tag = data.tag, url = data.url;
                if (o) {
                    post = o.offset().top; posb = post + o.height();
                    if ((post > st && post < sth) || (posb > st && posb < sth)) {
                        //在浏览器窗口内
                        if (tag === "img") {
                            //图片,改变src
                            o.attr("src", url);
                        } else {
                            o.load(url);
                        }
                        data.obj = null;
                    }
                }
            });
            return false;
        };

        //事件触发
        //加载完毕即执行
        loading();
        //滚动执行
        jQuery(window).bind("scroll", loading);
    };
    /*jquery.scrollLoading.js结束*/

    /*placeholder开始*/
    $.fn.placeholder = function () {
        if (typeof (Worker) != "undefined") { return; }
        if (window.applicationCache) { return; }
        if (sapn.IsLoad) { return; }
        sapn.IsLoad = true;
        $(this).each(function () {
            var _obj = $(this);
            var s = sapn.html(_obj);
            $("body").append(s);
            var _top = _obj.offset().top;
            var _left = _obj.offset().left;
            var _w = _obj.outerWidth();
            var _h = _obj.outerHeight();
            var val = _obj.val();
            var pleft = parseInt(_obj.css("padding-left"));
            var ptop = parseInt(_obj.css("padding-top"));
            var fint = parseInt(_obj.css("font-size"));
            _left = _left + pleft;
            s.click(function () { s.hide(); _obj.focus(); })
            s.css({
                top: _top + ptop, left: _left, height: _h, _width: _w, cursor: 'text', display: val == "" ? "" : "none",
                "line-height": _h + "px", "font-size": fint
            });
            if (_obj.val() != "") { s.css('display', 'none'); }
            _obj.focusin(function (e) {
                if (s.css('display') != 'none') {
                    s.css('display', 'none');
                }
            });
            _obj.focusout(function (e) {
                var val = _obj.val();
                if (val == '') {
                    s.css('display', '');
                }
            });
        });
    }
    var sapn = {
        IsLoad: false,
        html: function (obj) {
            var val = $(obj).attr("placeholder");
            return $('<span style="font-size:12px;position:absolute;color:#777777;">' + val + '</span>');
        }
    }
    /*placeholder结束*/
})(jQuery);
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}