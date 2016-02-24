/*******************
* Copyright 2013, windy
* Date: 2014-10
public.js包含搜索、分类菜单、鼠标效果、星星、倒数秒、促销时间倒计时、幻灯片、输入文字个数
*******************/

$.fn.extend({
    search: function (options) {
        //return $(this).each(function() {
        var settings = $.extend({
            $btn: $("input[type=button]")
        }, options);
        var obj = $(this);
        var btnObj = settings.$btn;
        obj.blur(function () {
            var tmp = $.trim(obj.val());
            var txt = obj.attr("data-txt");
            if (tmp == "" || tmp == txt) {
                obj.val(txt).css({ color: "#ccc" });
            }
        })
        obj.focus(function () {
            var tmp = $.trim(obj.val());
            var txt = obj.attr("data-txt");
            if (tmp == txt || tmp == "") {
                obj.val("").css({ color: "#333" });
            }
        })
        obj.keypress(function (evt) {
            evt = (evt) ? evt : ((window.event) ? window.event : "")
            keyCode = evt.keyCode ? evt.keyCode : (evt.which ? evt.which : evt.charCode);
            if (keyCode == 13) {
                btnObj.trigger("click");
            }
        })
        btnObj.click(function () {
            var tmp = $.trim(obj.val());
            var txt = obj.attr("data-txt");
            if (tmp == txt || tmp == "") {
                return;
            } else {
                //$(this).submit();
                window.open(obj.attr("data-url") + encodeURIComponent(tmp), "");
            }
        })
        //})
    },
    star: function (options) { //每个星星宽为15
        return $(this).each(function () {
            setting = {
                starTotal: 5,  //默认星星总数
                showNum: 5,   //默认星星显示分数
                //showMode:1, // 1 为全星 0 为半星
                evt: false,  //是否打开鼠标评分事件
                showCur: true //是否跟随显示数字
            }
            if (options) { $.extend(setting, options) }
            var self = $(this);
            function toDecimal(tmp) {//保留1位小数
                var f = parseFloat(tmp);
                if (isNaN(f)) {
                    return false;
                }
                var s = (Math.round(tmp / 15 * 10) / 10).toString();
                var rs = s.indexOf('.');
                if (rs < 0) {
                    rs = s.length;
                    s += '.';
                }
                while (s.length <= rs + 1) {
                    s += '0';
                }
                return s;
            }
            //if(setting.showMode == 1){
            var inttegral = self.attr("rel"); //
            //}else{
            //var inttegral = Math.round(self.attr("rel"));//
            //}
            var totalWidth = setting.starTotal * 15; //计算星星总宽
            var showWidth = inttegral * 15; //计算显示星星总宽
            self.width(totalWidth + "px"); //设置星星总宽
            self.find(".starlist").width(showWidth + "px"); //设置显示星星宽

            //if (setting.showCur) {
            //var cur = "<div class='starshow'></div>";
            //self.append(cur);
            //}
            if (setting.evt) {
                self.css("cursor", "pointer");
                self.mousemove(function (e) {
                    e = e || window.event;
                    var x = e.layerX || e.offsetX;

                    // alert(x);
                    //if(setting.showMode == 1){
                    var intgl = toDecimal(x);
                    //}else{
                    //var intgl = Math.round(x / 15 *10)/10;
                    //}
                    $(this).find(".starlist").width(x + "px");
                    if (setting.showCur) {
                        $(this).find(".starshow").text((intgl)).show();
                        $(this).next(".txs").text(intgl)
                        $(this).find(".starshow").css("left", x + "px");
                    }
                })
                self.mouseleave(function (ev) {
                    $(this).find(".starshow").hide();
                    var intgl = toDecimal(showWidth);
                    $(this).next(".txs").text(intgl)
                    $(this).find(".starlist").width(showWidth + "px");
                })
                self.click(function (e) {
                    e = e || window.event;
                    var x = e.layerX || e.offsetX;
                    showWidth = x;
                    var intgl = toDecimal(x);
                    $(this).find(".starlist").width(showWidth + "px");
                    if (setting.showCur) {
                        $(this).find(".starshow").text(intgl).show();
                    }
                })
            }
        })
    },
    textTip: function (obj) {
        return $(this).each(function () {
            var setgings = {
                Least: 0,
                Max: 200,
                TipNumId: "txNum",
                TipTextId: "txNumTip",
                TipText: "请输入"
            }
            if (obj) { jQuery.extend(setgings, obj) }
            var evt = $(this)
            evt.bind("blur keyup change", function () {
                var infoObj = $("#" + setgings.TipNumId);
                var strText = evt.val();
                var strLen = getByteLen(strText);

                if (strLen < setgings.Max) {// 当默认值小于限制数时,已输入数为cur
                    if (strLen < setgings.Least && strLen > 0) {
                        $("#" + setgings.TipTextId).show();
                        $("#" + setgings.TipTextId).text("不能小于" + setgings.Least + "个字符");
                        infoObj.text(strLen);
                    } else {
                        //$("#" + setgings.TipTextId).text(setgings.TipText);
                        infoObj.text(strLen);
                    }
                } else {
                    infoObj.text(setgings.Max);
                    evt.val(getByteStr(strText, setgings.Max)); // 截取指定字节长度内的值
                }
            })
            //evt.blur(function() {$(this).css({"border":"1px solid #ccc"})})
            //evt.focus(function() {$(this).css({"border":"1px solid #ff9900"})})
            //evt.trigger("click");
            // 返回str的字节长度
            function getByteLen(str) {
                var byteLen = 0;
                var strLen = $.trim(str).length; // 去掉左右空格
                for (var i = 0; i < strLen; i++) {
                    if (str.charAt(i).match(/[^\x00-\xff]/g) != null) // 全角
                        byteLen += 2;
                    else
                        byteLen += 1;
                }
                return byteLen;
            }
            // 截取指定字节
            function getByteStr(str, max) {
                var result = "";
                var byteLen = 0;
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i).match(/[^\x00-\xff]/g) != null) // 全角
                        byteLen += 2;
                    else
                        byteLen += 1;
                    if (byteLen > max)
                        break;
                    result += str.charAt(i);
                }
                return result;
            }
        })
    },
    ToTalSecond: function (options) {
        seting = {
            time: 1000,
            src: ""
        }
        if (options) { $.extend(seting, options) }
        var evt = $(this);
        var second = evt.text();
        var t = setInterval(redirect, seting.time);
        function redirect() {
            if (second == 0) {
                clearInterval(t);
                window.location.href = seting.src;
            } else {
                evt.text(second--);
            }

        }
    },
    groupTime: function (options) {
        var self = $(this);
        var timer = null;
        var iRemain = self.attr("data-time");
        function toDecimal(num, digit)//保留n位小数
        {
            var str = '' + parseInt(num);
            while (str.length < digit) {
                str = '0' + str;
            }
            return str;
        }

        function updateTime() {
            /*
            var oDateNow=new Date();//现在时间
            var oDateEnd=new Date();//结速时间
            var  getEnd = self.attr("data-EndDate");//2013-13-13
            var  getNow = self.attr("data-StartDate");//2013-14-13
            */
            var iDay = 0;  //天
            var iHour = 0;  //小时
            var iMin = 0;  //秒
            var iSec = 0;  //微秒
            var sTime = "本次促销时间已过期"; //
            /*
            oDateEnd.setFullYear(getEnd.substring(0,4));
            oDateEnd.setMonth(getEnd.substring(5,7)-1);
            oDateEnd.setDate(getEnd.substring(8,10));
            oDateEnd.setHours(0);
            oDateEnd.setMinutes(0);
            oDateEnd.setSeconds(0);
            iRemain=(oDateEnd.getTime()-oDateNow.getTime())/1000;
            */
            iRemain--;
            var tmp = iRemain;
            if (iRemain <= 0) {
                clearInterval(timer);
                iRemain = 0;
                //alert('已过时间');
                if (options.end == "true") {
                    location.href = location.href;
                }
                sTime = "本次促销时间已过期";
                self.text(sTime);
                $(".buy a").addClass("quickbuy2");
                return;
            }
            iDay = parseInt(tmp / 86400);
            tmp %= 86400;
            iHour = parseInt(tmp / 3600);
            tmp %= 3600;
            iMin = parseInt(tmp / 60);
            tmp %= 60;
            iSec = tmp;
            sTime = options.txt + "&nbsp;<b>" + iDay + "</b>&nbsp;天&nbsp;<b>" + toDecimal(iHour, 2) + "</b>&nbsp;小时&nbsp;<b>" + toDecimal(iMin, 2) + "</b>&nbsp;分&nbsp;<b>" + toDecimal(iSec, 2) + "</b>&nbsp;秒" + "<b>";
            self.html(sTime);
        }
        timer = setInterval(updateTime, 1000);
    },
})


$(function () {
    /***总分类***/
    var classColor = (function () {
        //var arrColor = ["#b23107", "#c9033b", "#d26800", "#d25900", "#3165ac", "#23938e", "#004760", "#9b072f", "#0295d4", "#aa7700", "#4e8f01", "#019b53", "#ac6a00", "#644c3d"];
        var colorCbox = $("#hoverColor");
        $(".classLi").hover(function () {
            var self = $(this);
            var m = self.index();
            var color = self.attr("data-color");
            colorCbox.css("background", "#" + color + " url(../Content/images/cell.png) left top");
            //self.addClass("onli").find(".aone").css({ "background": arrColor[m], "border-bottom": "1px solid " + arrColor[m] });
            self.addClass("onli").find(".aone").css({ "background": "#" + color, "border-bottom": "1px solid #" + color });
            self.find(".showbg, .showClass, .way").show();
        }, function () {
            var self = $(this);
            self.removeClass("onli").find(".aone").css({ "background": "", "border-bottom": "" });
            colorCbox.css("background", "");
            self.find(".showbg, .showClass, .way").hide();
        })
        if ($(".contant").attr("data-index")) {
            $(".allmenu").show();
        } else {
            var allmenu = $(".allmenu");
            $(".menu-all").hover(function () {
                allmenu.show();
            }, function () {
                allmenu.hide();
            })
        }
    })()

//    $("#brandMeta").on("hover", ".brands li",
//     function ()
//     {
//       $(this).find("span").animate({ "height": "30px" }, { duration: 100, queue: false })
//    },
//     function () {
//        $(this).find("span").animate({ "height": "0" }, { duration: 100, queue: false })
//    });
$("#brandMeta").on("mouseover mouseout",".brands li",function(event){
 if(event.type == "mouseover"){
  //鼠标悬浮
  $(this).find("span").animate({ "height": "30px" }, { duration: 100, queue: false })
 }else if(event.type == "mouseout"){
  //鼠标离开
   $(this).find("span").animate({ "height": "0" }, { duration: 100, queue: false })
 }
})
//    $(".brands li").hover(function () {
//        $(this).find("span").animate({ "height": "30px" }, { duration: 100, queue: false })
//    }, function () {
//        $(this).find("span").animate({ "height": "0" }, { duration: 100, queue: false })
//    })

    //菜单
    var obj = $(".nav-menu-ul").attr("data-menu");
    $(".on" + obj).addClass("on");
    if (obj != "Index") {
    }

    //帮助菜单
    var helpobj = $("#helpLeft").attr("data-menu");
    $("." + helpobj).addClass("licur");

    //品牌荟萃换一换
    $(".resh").click(function () {
        var self = $(this);
        self.attr("disabled", "disabled");
        var cateId = self.attr("data-cateId");
        var total = self.attr("data-Total");
        var page = parseInt(self.attr("data-Page")) + 1;
        if (total == 1) {
            return;
        }
        if (total < page) { page = 1; }
        $.post("/Home/Partial_brandMeta", { cateId: cateId, top: page, quantity: 24 }, function (response) {
            $("#" + cateId).html(response);
            self.removeAttr("disabled");
            self.attr("data-Page", page);
        });
    })

    //商家联盟
    $(".buypro").click(function () {
        var self = $(this);
        var tmp = self.attr("data-display") || false;
        var id = self.attr("data-id");
        tmp = eval(tmp);
        if (tmp == false) {
            self.attr("data-display", true);
            $("#" + id).css("border-bottom", "1px solid #dbdbdb").find(".lincePro").slideUp("fast");
        } else {
            self.attr("data-display", false);
            $("#" + id).css("border-bottom", "1px solid #fff").find(".lincePro").slideDown("fast");
        }
    })
    $(".bail-1").hover(function () {
        $(this).prev().show(); //bail-tip
    }, function () {
        $(this).prev().hide();
    })

    //shop
    $(".store-list .listLi").on({
        mouseover: function () {
            $(this).addClass("liHover");
        },
        mouseout: function () {
            $(this).removeClass("liHover")
        }
    })
    $("#keyword").on({
        focus: function () {
            $(".labelkeyword").css("width", "0px");
        },
        blur: function () {
            if ($(this).val() == "") {
                $(".labelkeyword").css("width", "auto");
            }
        }
    })

    $(".ListBox .listLi").on({
        mouseover: function () {
            $(this).addClass("liHover");
        },
        mouseout: function () {
            $(this).removeClass("liHover");
        }
    })
    $(".item-li").on({
        mouseover: function () {
            $(this).css("border", "1px solid #cc3300")
        },
        mouseout: function () {
            $(this).css("border", "1px solid #dbdbdb")
        }
    })

    //一级分类
    $(".sublist-r .aleft").hover(function () {
        $(this).find(".sub-sale").animate({ "height": "80px", "padding-top": "20px" }, { duration: 300, queue: false });
    }, function () {
        $(this).find(".sub-sale").animate({ "height": "20px", "padding-top": "0" }, { duration: 300, queue: false });
    })


})
//详细滚动
/*function decScroll() {
    var decTitle = $(".decTitle");
    var stop = decTitle.offset().top;
    $(window).scroll(function () {
        if ($(this).scrollTop() > stop) {
            decTitle.css({ "position": "fixed", "top": "0" });
        } else {
            decTitle.css({ "position": "relative", "top": "0" });
        }
    })
}*/
//滚动分类楼层
var scrollObj = (function(){
    function posObj(obj) {
        this.obj = obj;
        this.str = [];
        //this.txt = obj.find("b>a").text();
    }
    posObj.prototype = {
        showScroll: function () {
            var self = this;
            $(window).bind("scroll load", function () {
                var thisObj = $(this);
                //console.log(thisObj.scrollTop())
                if (thisObj.scrollTop() > "500") {
                    self.scrollID.fadeIn();
                }else{
                    self.scrollID.fadeOut();
                }
            })
        },
        init: function () {
            var obj = $(this.obj);
            var scrollBox = "<div id='ScrollCate' style='display:none'><div class='scb'><a href='javascript:void(0)' class='top'></a></div>";
            for (var i = 0; i < obj.length; i++) {
                var txt = obj.eq(i).find(".ftxt").text();
                this.str.push(obj.eq(i).position().top);
                var classname = obj.eq(i).attr("data-scroll");
                scrollBox += "<div class='scb'><span>" + txt + "</span><a href='javascript:void(0)' class='" + classname + "'></a></div>"
            }
            scrollBox += "<div class='scb'><a href='javascript:void(0)' class='top'></a></div>"
            scrollBox += "</div>";
            $("body").append(scrollBox);
            this.scrollID = $("#ScrollCate");
            this.showScroll();
            this.btn();
        },
        scrollFun: function () {
            var self = this;
            var pageHeight = $(document).height();
            var modHeight = $(".modStyle").height();
            var sHeight = pageHeight - modHeight - $(".footer").height();
            $("#relative").css("height", modHeight + "px");
            $(window).bind("resize scroll load", function () {
                var stop = $(this).scrollTop();
                if (stop >= 156) {
                    $(".modStyle").css({ "position": "fixed", "top": "0px" });
                    if (stop >= sHeight) {
                        $(".modStyle").css({ "position": "absolute", "top": +(sHeight - 178) + "px" });
                    }
                } else {
                    $(".modStyle").css({ "position": "", "top": "0px" });
                }
            })
        },
        btn: function () {
            var self = this;
            var span = null;
            var has = false;
            self.scrollID.find("a").on({
                click: function () {
                    if ($(this).attr("class") == "top") {
                        var sTop = 0;
                    } else {
                        var index = $(this).parent(".scb").index()-1;
                        var sTop = (self.str)[index];
                    }
                    $('html, body').animate({ scrollTop: sTop + "px" }, { duration: 300, queue: false });
                },
                mouseover: function () {
                    if ($(this).attr("class") == "top") {
                        has = true;
                        return false;
                    } else {
                        has = false;
                        span = $(this).parent(".scb").find("span");
                        var sLeft = "-" + (span.outerWidth() - 1);
                        span.animate({ "left": sLeft + "px" }, { duration: 300, queue: false }, "swing")//.fadeIn();
                    }
                },
                mouseout: function () {
                    if (!has) {
                        span.animate({ "left": "0px" }, { duration: 300, queue: false }, "swing")//.fadeOut();
                    }
                }
            })
        }
    };
    return {
        scrollMenu:function(obj){
            new posObj(obj).init();//
        },
        scrollClass:function(obj){
            new posObj(obj).scrollFun();
        }
    }
})()
/******
**  2014.5.14 windy
**  picture slide
******/
var slideImg = (function(){
    function slide(obj,option) {
        var setting = {
            type: "swing",
            mode:"left",
            isAuto: false, //图片是否显示器宽
            time:3000,
            colors: {id:null,arr:[null]}//第一位颜色ID，第二个为颜色数组
        }
        this.options = $.extend(setting, option);
        this.obj = $(obj);
        this.i = 1; //计数;
        this.moveObj = this.obj.find("ul");
        this.num = this.obj.find("li").length; //总数
        this.oneWidth = this.obj.find("li").first().width(); //单个 宽度
        var fst = this.moveObj.find("li:first").clone()//clone第一个
        var lst = this.moveObj.find("li:last").clone()//clone最后一个
        this.moveObj.append(fst);
        this.moveObj.prepend(lst);
        this.moveObj.css({ left: -this.oneWidth + "px" }); //初始化位置
    }

    slide.prototype = {
        init: function () {
            if (this.options.isAuto) {
                var _this = this;
                $(window).bind("resize load", function () {
                    _this.oneWidth = $(document).width();
                    _this.obj.find("li").width(_this.oneWidth + "px");
                    var w = -_this.i * _this.oneWidth;
                    _this.moveObj.css({ left: w + "px" });
                })
                //this.moveObj.find("img").hide();
            }
            if (this.num > 1) {
                var html = "<div class='prev'>&lt;</div><div class='next'>&gt;</div>";
                html += "<div class='slidebtn'>";
                for (var n = 1; n <= this.num; n++) {
                    if (n == 1) {
                        html += "<div class='btn cur'></div>";
                    } else {
                        html += "<div class='btn'></div>";
                    }
                }
                html += "</div>"
                this.obj.find(".btns").html(html);
                this.btn = this.obj.find(".btn");
                this.nxt = this.obj.find(".next"); //下一个按钮
                this.pev = this.obj.find(".prev"); //上一个按钮
                this.btns();
            }
            if (this.options.colors.id != null && this.options.colors.arr != null) {
                $(this.options.colors.id).css("background", "#" + this.options.colors.arr[0] + " url(../Content/images/cell.png) left top");
            }
            
        },
        auto:function (n) {
            var self = this;
            var moveDiv = self.moveObj;
            if (moveDiv.is(":animated")) { return }
            if (self.i >= self.num && n == 1) {
                self.i = 0;
                moveDiv.css({ left: "0" });
            }
            if (self.i == 0 && n == -1) {
                self.i = self.num;
                var w = -self.i * self.oneWidth;
                moveDiv.css({ left: w + "px" });
            }
            //focusImg.clearQueue();//列队
            self.i = self.i + n;
            self.btn.removeClass("cur");          //btn
            self.btn.eq(self.i - 1).addClass("cur"); //btn
            if (this.options.colors.id != null && this.options.colors.arr != null) {
                var tmp = self.i - 1;
                if ( tmp == -1 ) {
                    tmp = self.num - 1;
                }
                $(this.options.colors.id).css("background", "#" + this.options.colors.arr[tmp])// + " url(../Content/images/cell.png) left top");
            }
            var mw = -(self.i * self.oneWidth);
            moveDiv.animate({ left: +mw + "px" }, 600, self.options.type)
        },
        btns:function(){
            var self = this;
            var t;
            this.btn.hover(function () {
                clearInterval(t);
            }, function () {
                t = setInterval(function () {
                    if(self.options.mode=="right"){
                        self.auto(-1)
                    }else{
                        self.auto(1)
                    }
                }, self.options.time)
            })
            this.pev.hover(function () {
                clearInterval(t);
            }, function () {
                t = setInterval(function () {
                    if(self.options.mode=="right"){
                        self.auto(-1)
                    }else{
                        self.auto(1)
                    }
                }, self.options.time)
            })
            this.nxt.hover(function () {
                clearInterval(t);
            }, function () {
                t = setInterval(function () {
                    if(self.options.mode=="right"){
                        self.auto(-1)
                    }else{
                        self.auto(1)
                    }
                }, self.options.time)
            })
             this.btn.click(function () {
                var tmp = $(this).index() + 1;
                self.i = 0;
                self.auto(tmp);
            })
            this.nxt.click(function () {
                self.auto(-1)
            })
            this.pev.click(function () {
                self.auto(1)
            })
            this.moveObj.hover(function () {
                clearInterval(t);
            }, function () {
                t = setInterval(function () {
                    if(self.options.mode=="right"){
                        self.auto(-1)
                    }else{
                        self.auto(1)
                    }
                }, self.options.time)
            })
            t = setInterval(function () {
                if(self.options.mode=="right"){
                    self.auto(-1)
                }else{
                    self.auto(1)
                }
            }, self.options.time);
        }
    }
    return {
        init:function(obj,option){
            var objs = $(obj);
            for(var i=0; i<objs.length; i++){
                new slide(objs[i],option).init();
            }
        }
    }
})()


//DLD PC 弹出层提示 windy v1.1
var popup = (function () {
    function pop(id, option) {
        var setting = {
            $className: "", //自定义样式popup, confirm
            $width: "400", //
            $height: "auto", //
            $position: true, //自动居中为 true, [null, null];
            $title: null,//标题
            $contant: "<div class='popContant'>跳出信息提示</div>", //弹出信息放HTML
            $opacity: ["0,0,0,0.5"], //背景
            $confirm: { enter: null, cancel: null }, //{enter:"确认",cancel:"取消"} 
            $time: 1000, //关闭自动  false
            $callback: null, //回调函数
            $callEnter: null //enter 回调函数  返回 true关闭 or false不关闭，
            //$callCancel: null// cancel回调函数
        }
        this.setting = $.extend(setting, option);
        this.obj = id;
        this.objTitle = null;
        this.win = $(window);
        this.sHeight = this.win.height();
        this.sWidth = this.win.width();
        var tmp = $(document).height();
        if (tmp > this.sHeight) {
            this.bgHeight = tmp;
        } else {
            this.bgHeight = this.sHeight;
        }
    }
    pop.prototype = {
        tempEnter: null,
        //tempCancel:null,
        close: function () {
            var self = this;
            self.obj.css("opacity", "0");
            //console.log(self.objTitle)
            if (self.objTitle != null) {
                self.objTitle.css("opacity", "0");
            }
            if (self.tempEnter != null) {
                self.tempEnter.off();
            }
            /*
            if(self.tempCancel != null){
            self.tempCancel.off();
            }*/
            setTimeout(function () {
                self.obj.remove();
                if (self.objTitle != null) {
                    self.objTitle.remove();
                }
                delete pop();
            }, 200);
            if (typeof self.setting.$contant == "object") {
                self.setting.$contant.css("display", "none");
            }
        },
        pos: function (self) {
            var set = this.setting.$position;
            if (set == true) {
                var pw = (this.sWidth - self.outerWidth()) / 2;
                var ph = (this.sHeight - self.outerHeight()) / 2// + this.win.scrollTop();
                return [pw, ph];
            } else {
                return [set[0], set[1]];
            }
        },
        isRemove: function () {
            var time = this.setting.$time;
            if (time == false) {
                if (this.setting.$callback && (this.setting.$callback instanceof Function)) {
                    this.setting.$callback.call(this, this); 
                }
            } else {
                var self = this;
                setTimeout(function () {
                    self.close();
                    if (self.setting.$callback && (self.setting.$callback instanceof Function)) {
                        self.setting.$callback.call(this, self);
                    }
                }, time)
            }
        },
        enterClick: function () {
            var self = this;
            $(".popEnter").on("click", _enter = function () {
                self.tempEnter = $(this)
                self.tempEnter.off();
                if (self.setting.$callEnter && (self.setting.$callEnter instanceof Function)) {
                    if (self.setting.$callEnter.call(this, self)) {
                        self.close();
                    } else {
                        self.tempEnter.on("click", _enter);
                    }
                }
            })
        },
        cancelClick: function () {
            var self = this;
            $(".popCancel").on("click", function () {
                //self.tempCancel = $(this);
                $(this).off();
                self.close();
            });
        },
        confirm: function () {
            var cfmHTML = "";
            var cfm = this.setting.$confirm;
            cfmHTML += "<div class='popBtns' style='text-align:center'>"
            if (cfm.enter != null) {
                cfmHTML += "<button class='popEnter'>" + cfm.enter + "</button>";
            }
            if (cfm.cancel != null) {
                cfmHTML += "<button class='popCancel'>" + cfm.cancel + "</button>";
            }
            cfmHTML += "</div>";
            return cfmHTML;
        },
        inst: function () {
            var zIndex = 9999;
            if ($(".popup").length) { zIndex = zIndex + $(".popup").length }
            var opa = this.setting.$opacity;
            var IEbg8 = ["19", "33", "4c", "66", "7f", "99", "b2", "c8", "e5"];
            var HTML = "<div id='" + this.obj + "' class='popup " + this.setting.$className + "'";
            //HTML += "style='position:fixed;left:0;top:0;z-index:9999;width:100%;background:rgba(" + opa[0] + ");*background:none;filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#" + IEbg8[opa[0].slice(8)] + "000000,endColorstr=#" + IEbg8[opa[0].slice(8)] + "000000)'>";
            HTML += "style='position:fixed;left:0;top:0;z-index:" + zIndex + ";width:100%;background:rgba(" + opa[0] + ");'>";

            if (typeof this.setting.$contant != "object") {
                HTML += "<div class='popBox' style='display:inline-block;*display:inline;*zoom:1'>";
                HTML += "<div class='popContant'>" + this.setting.$contant + "</div>";
                if (this.setting.$confirm.enter != null || this.setting.$confirm.cancel != null) {
                    HTML += this.confirm();
                } else {
                    this.isRemove();
                }
                HTML += "</div></div>";
                if (this.setting.$title != null) {
                    HTML += "<div id='t_" + this.obj + "' class='popTitle' style=''><div class='pop_cb_bottom'><b>" + this.setting.$title + "</b><a class='popclose' title='关闭'>╳</a></div></div>";
                }
                $("body").append(HTML);
                this.objTitle = $("#t_" + this.obj);

                this.obj = $("#" + this.obj);
                var chindArr = this.pos(this.obj.children());
       
                this.obj.css({ "height": this.bgHeight, "opacity": "1" });
                this.obj.children().on("click", function (e) {
                    e.stopPropagation();
                }).css({ "position": "absolute", "left": chindArr[0] + "px", "top": chindArr[1] + "px" });
                //this.obj.children().css({ "position": "absolute", "left": chindArr[0] + "px", "top": chindArr[1] + "px" });
                this.objTitle.css({ "position": "fixed", "left": chindArr[0] + "px", "top": chindArr[1] - this.objTitle.height() + "px", "z-index": "10001", "width": +this.obj.children().outerWidth() + "px" });

            } else {
                HTML += "</div>";
                if(this.setting.$title != null){
                    HTML += "<div id='t_" + this.obj + "' class='popTitle' style=''><div class='pop_cb_bottom'><b>" + this.setting.$title + "</b><a class='popclose' title='关闭'>╳</a></div></div>";
                }
                $("body").append(HTML);
                this.isRemove();
                this.setting.$contant.css({"display":"block","width": this.setting.$width + "px"});
                var chindArr = this.pos(this.setting.$contant);
                this.objTitle = $("#t_" + this.obj);
                this.obj = $("#" + this.obj);
                this.obj.css({ "height": this.bgHeight + "px", "opacity": "1" });
                
                this.setting.$contant.on("click", function (e) {
                    e.stopPropagation();
                }).css({ "position": "fixed", "left": chindArr[0] + "px", "top": chindArr[1] + "px", "z-index": "10000" });
                this.objTitle.css({ "position": "fixed", "left": chindArr[0] + "px", "top": chindArr[1] - this.objTitle.height() + "px", "z-index": "10001", "width": + this.setting.$width + "px" });
            }
            if (this.setting.$confirm.enter != null) {
                this.enterClick();
            }
            if (this.setting.$confirm.cancel != null) {
                this.cancelClick();
            }
            var self = this;
            self.obj.on("click", function (e) {
                e.stopPropagation();
                self.close();
            });
            if (self.objTitle != null) {
                self.objTitle.on("click", ".popclose", function () {
                    self.close();
                });
            }
        }
    }
    return {
        init: function (option) {
            var timestamp = new Date().getTime();
            timestamp = new pop(timestamp, option);
            timestamp.inst();
            return timestamp;
        }
    }
})()

//弹出确认，取消对话框
var alertConfirm = function (confirmMsg, ajaxFun) {
    popup.init({
        $className: "confirm",
        $position: true, //自动居中为 true, [null, null];
        $width: "600",
        $contant: "<div class='alert-confirm'>" + confirmMsg + "</div>", //弹出信息放HTML
        $opacity: ["0,0,0,0.5"], //背景
        $confirm: { enter: "确认", cancel: "取消" }, //{enter:"确认",cancel:"取消"} 
        $time: false, //关闭自动  false
        $callEnter: function (obj) {
            ajaxFun();
            obj.close();
        } //回调函数
    })
}

//弹出成功提示信息
var alertScuess = function (confirmMsg) {
    popup.init({
        $className: "confirm",
        $position: true, //自动居中为 true, [null, null];
        $width: "600",
        $contant: "<div class='alert-sucess'>" + confirmMsg + "</div>", //弹出信息放HTML
        $opacity: ["0,0,0,0.5"], //背景
        $confirm: { enter: "确认" }, //{enter:"确认",cancel:"取消"} 
        $time: false,//关闭自动  false
        $callEnter: function (obj) {
            obj.close();
        } //回调函数
    })
}
//弹出错误提示信息
var alertError = function (confirmMsg) {
    popup.init({
        $className: "confirm",
        $position: true, //自动居中为 true, [null, null];
        $width: "600",
        $contant: "<div class='alert-error'>" + confirmMsg + "</div>", //弹出信息放HTML
        $opacity: ["0,0,0,0.5"], //背景
        $confirm: { enter: "确认" }, //{enter:"确认",cancel:"取消"} 
        $time: false,//关闭自动  false
        $callEnter: function (obj) {
            obj.close();
        } //回调函数
    })
}