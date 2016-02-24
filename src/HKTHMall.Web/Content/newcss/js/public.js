; (function ($) {
    $.fn.fonts = function (option) {
        option = $.extend({}, $.fn.fonts.option, option);
        return this.each(function () {
            var objString = $(this).text(),
                objLength = $(this).text().length,
                num = option.fontNum;
            if (objLength > num) {
                objString = $(this).text(objString.substring(0, num) + "···");
            }
        });
    }
    // default options
    $.fn.fonts.option = {
        fontNum: 100 //font num
    };

})(jQuery);
(function ($) {
    $.fn.jCarouselLite = function (o) {
        o = $.extend(
        {
            btnPrev: null,
            btnNext: null,
            btnGo: null,
            mouseWheel: false,
            auto: null,
            speed: 1000,
            easing: null,
            vertical: false,
            circular: true,
            visible: o.liNum,
            start: 0,
            scroll: o.liNum,
            beforeStart: null,
            afterEnd: null
        },
        o || {});
        return this.each(function () {
            if (o.visible >= 4) {
                o.visible = 4;
            }
            if (o.scroll >= 4) {
                o.scroll = 4;
            }
            var b = false, animCss = o.vertical ? "top" : "left", sizeCss = o.vertical ? "height" : "width";
            var c = $(this), ul = $("ul", c), tLi = $("li", ul), tl = tLi.length, v = o.visible;
            if (o.circular) {
                ul.prepend(tLi.slice(tl - v - 1 + 1).clone()).append(tLi.slice(0, v).clone());
                o.start += v
            }
            var f = $("li", ul), itemLength = f.length, curr = o.start;
            c.css("visibility", "visible");
            ul.css(
            {
                margin: "0", padding: "0", position: "relative", "list-style-type": "none", "z-index": "1"
            });
            c.css({
                overflow: "hidden", position: "relative", "z-index": "2", left: "0px"
            });
            var g = o.vertical ? height(f) : width(f);
            var h = g * itemLength;
            var j = g * v;
            f.css({
                width: f.outerWidth(), height: f.outerHeight()
            });
            ul.css(sizeCss, h + 10 + "px").css(animCss, -(curr * g));
            c.css(sizeCss, j + "px");
            if (o.btnPrev) {
                $(o.btnPrev).click(function () {
                    return go(curr - o.scroll);
                });
            }
            if (o.btnNext) {
                $(o.btnNext).click(function () {
                    return go(curr + o.scroll);
                });
            }
            if (o.btnGo) {
                $.each(o.btnGo, function (i, a) {
                    $(a).click(function () {
                        return go(o.circular ? o.visible + i : i);
                    })
                });
            }
            if (o.mouseWheel && c.mousewheel) {
                c.mousewheel(function (e, d) {
                    return d > 0 ? go(curr - o.scroll) : go(curr + o.scroll);
                });
            }
            if (o.auto) {
                setInterval(function () {
                    go(curr + o.scroll)
                },
                o.auto + o.speed);
            }
            function vis() {
                return f.slice(curr).slice(0, v);
            };
            function go(a) {
                if (!b) {
                    if (o.beforeStart) {
                        o.beforeStart.call(this, vis());
                    }
                    if (o.circular) {
                        if (a <= o.start - v - 1) {
                            ul.css(animCss, -((itemLength - (v * 2)) * g) + "px");
                            curr = a == o.start - v - 1 ? itemLength - (v * 2) - 1 : itemLength - (v * 2) - o.scroll
                        }
                        else if (a >= itemLength - v + 1) {
                            ul.css(animCss, -((v) * g) + "px");
                            curr = a == itemLength - v + 1 ? v + 1 : v + o.scroll
                        }
                        else {
                            curr = a;
                        }
                    }
                    else {
                        if (a < 0 || a > itemLength - v) {
                            return;
                        }
                        else {
                            curr = a;
                        }
                    }
                    b = true;
                    ul.animate(animCss == "left" ? {
                        left: -(curr * g)
                    }
                     : {
                         top: -(curr * g)
                     },
                    o.speed, o.easing, function () {
                        if (o.afterEnd) {
                            o.afterEnd.call(this, vis());
                        }
                        b = false;
                    });
                    if (!o.circular) {
                        $(o.btnPrev + "," + o.btnNext).removeClass("disabled");
                        $((curr - o.scroll < 0 && o.btnPrev) || (curr + o.scroll > itemLength - v && o.btnNext) || []).addClass("disabled")
                    }
                }
                return false;
            }
        })
    };
    function css(a, b) {
        return parseInt($.css(a[0], b)) || 0;
    };
    function width(a) {
        return a[0].offsetWidth + css(a, 'marginLeft') + css(a, 'marginRight');
    };
    function height(a) {
        return a[0].offsetHeight + css(a, 'marginTop') + css(a, 'marginBottom');
    }
})(jQuery);
$.fn.smartFloat = function () {
    var position = function (element) {
        element = $(element);
        var top = 0,
        pos = element.css("position");
        $(window).scroll(function () {
            if (top == 0) top = element.offset().top;
            var scrolls = $(window).scrollTop();
            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.addClass('.fadeInUp');
                    element.css({
                        position: "fixed",
                        top: '0px',
                        'background': '#fff',
                        'boxShadow': '0 2px 10px #ccc'
                    });
                    element.children().css({
                        'paddingTop': '15px',
                        'height': '55px',
                    });
                } else {
                    element.css({
                        top: scrolls,
                    });
                }
            } else {
                element.css({
                    position: 'static',
                    top: "",
                    'background': 'none',
                    'boxShadow': '0 0 0'
                });
                element.children().css({
                    'paddingTop': '26px',
                    'height': '71px',
                });
            }
        });
    };
    return $(this).each(function () {
        position(this);
    });
};
function placeholderSupport() {
    return 'placeholder' in document.createElement('input');
}
$(document).ready(function () {
    /*个人中心*/
    var _leftHeight = $(".ls_member_lf").height;
    var _rightHeight = $(".ls_member_rg").height;
    (_leftHeight > _rightHeight) ? $(".ls_member_rg").css('height', '_leftHeight') : $(".ls_member_lf").css('height', '_rightHeight');
    /*end*/
    //if (!placeholderSupport()) {   // 判断浏览器是否支持 placeholder
    //    $(document).ready(function () {
    //        //默认遍历循环添加placeholder
    //        $('[placeholder]').each(function () {
    //            $(this).parent().append("<span class='placeholder'>" + $(this).attr('placeholder') + "</span>");
    //        })
    //        $('[placeholder]').blur(function () {
    //            if ($(this).val() != "") {  //如果当前值不为空，隐藏placeholder
    //                $(this).parent().find('span.placeholder').hide();
    //            }
    //            else {
    //                $(this).parent().find('span.placeholder').show();
    //            }
    //        })
    //    });
    //}

    $(".forceSldier_2016").hover(function () {
        $(this).addClass('linkHover');
        $(this).find('i.dropIcon').addClass("fa-rotate-180");
        $(this).find('div.dropHide').slideDown();
    }, function () {
        $(this).removeClass('linkHover');
        $(this).find('i.dropIcon').removeClass("fa-rotate-180");
        $(this).find('div.dropHide').stop().slideUp();
    });
    //console.log($(".lSPager").width());



    $(".cateInner>li").each(function () {
        $(this).hover(function () {
            $(this).children("div.cateInnerHide").show();
        }, function () {
            $(this).children("div.cateInnerHide").stop().hide();
        });

    });
    var floor = new Swiper('.mContianer', {
        spaceBetween: 0,
        speed: 1000,
        simulateTouch: false,
        direction: 'horizontal',
        loop: true,
        // 如果需要前进后退按钮
        nextButton: $('.mContianer').find("div.swiper-button-next"),
        prevButton: $('.mContianer').find("div.swiper-button-prev"),
    });

    $(".priceLimited").fonts({ fontNum: 12 });
    $('.lazyBoomCover').each(function () {
        $('.boomName').fonts({ fontNum: 19 });
    });
    //$('.sectionLimited').fonts({ fontNum: 40 });
    $('.lbSearchHistory').each(function () {
        $('.scanLimited').fonts({ fontNum: 30 });
    });
    $('.sectionLimited').each(function () { $(this).fonts({ fontNum: 55 }); });
    $('.lazyBoomTitle li').bind('click', function () {
        $(this).addClass('active').siblings().removeClass('active');
        if ($(this).index() == 0) {
            $('.lazyBoomCover_1').show().each(function () {
                $('.lazyBoomMain_1').addClass('flipInX');
            });
            $('.lazyBoomCover_2').hide().each(function () {
                $('.lazyBoomMain_2').removeClass('flipInX');
            });
        } else if ($(this).index() == 1) {
            $('.lazyBoomCover_1').hide().each(function () {
                $('.lazyBoomMain_1').removeClass('flipInX');
            });
            $('.lazyBoomCover_2').show().each(function () {
                $('.lazyBoomMain_2').addClass('flipInX');
            });
        }
    });
    /*判断有没有下拉*/
    if ($(document).find('div').hasClass('main_index')) {
        //console.log('123');
        $('.cateCover').css('display', 'block');
        $("#floatSearch").smartFloat();
    } else {
        //console.log('222');
        $('.cateCover').css('display', 'none');
        $('.navCategorys').hover(function () {
            $('.cateCover').slideDown();
        }, function () {
            $('.cateCover').stop().slideUp();
        });
    }
    /*列表页*/
    //$(".lbSpector").each(function(){
    //    $(this).find("span").bind("click",function(){
    //        $(this).children("i").toggleClass("fa-rotate-90");
    //        $(this).next("dl").slideToggle();
    //    });
    //});

    $('.lbSpector>li').each(function () {
        $(this).children("span").click(function () {
            $(this).children('span.spector2016').children('i').toggleClass('fa-rotate-90');
            $(this).next("dl").slideToggle();
        });
    });

    $('.seeSpectorMore').click(function () {
        //alert("123");
        var _this = $(this);
        (_this.prevUntil("ul").height == '20') ? _this.prevUntil("ul").css('height', 'auto') : _this.prevUntil("ul").css('height', '20px');
    });


    var mySwiper = new Swiper('.swiper-container', {
        slidesPerView: 4,
        slidesPerGroup: 1,
        spaceBetween: 15,
        direction: 'horizontal',
        loop: true,
        // 如果需要分页器
        //pagination: '.swiper-pagination',
        // 如果需要前进后退按钮
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
    });
    $(".lbSpector>li").each(function () {
        if ($(this).children("dl").children("dd").children(".spectorSelected").length > 0)
        {
            $(this).find("i").addClass("fa-rotate-90");
            $(this).find("dl").slideDown();
        }
        //var isShow = $(this).data('show');
        //if (isShow == 1) {
        //    $(this).find("i").addClass("fa-rotate-90");
        //    $(this).find("dl").slideDown();
        //}
    });
    //var _sel = new Array();
    //_sel.push($('.spectorForce').length);
    //for(var i = )
    //for (var n in 5) {
    //    if (n > 2) {
    //        $('.spectorForce').eq(n).hide();
    //    }
    //}
    function hiddenMore() {
        for (var n = 0; n < $('.spectorForce').length; n++) {
            if (n > 1) {
                $('.spectorForce').eq(n).slideUp();
            }
        }
    }
    hiddenMore();
    $(".showlenMore").click(function () {
        var _this = $(this);
        if (_this.hasClass('showlenStep1')) {
            $('.spectorForce').slideDown();
            _this.children('span').html('隐藏筛选');
            _this.children('i').removeClass('fa-angle-double-down').addClass('fa-angle-double-up');
            _this.addClass('showlenStep2').removeClass('showlenStep1');
        } else if (_this.hasClass('showlenStep2')) {
            hiddenMore();
            _this.children('span').html('更多筛选');
            _this.children('i').removeClass('fa-angle-double-up').addClass('fa-angle-double-down');
            _this.addClass('showlenStep1').removeClass('showlenStep2');
        }
    });
    /*end*/
    $('.rankMethod').eq(0).children('a').css('color', '#ef5959');
    $('.rankMethod').eq(0).children('a').children('i').css('color', '#ef5959');
    $('.rankMethod').eq(0).children('span').addClass('onSelected');
    $('.rankMethod').click(function () {
        $('.rankMethod').css('border-right', '0');
        $(this).children('a').css('color', '#ef5959');
        $(this).children('a').children('i').css('color', '#ef5959');
        $(this).children('span').addClass('onSelected');
        $(this).siblings().children('a').css('color', '#4e4e4e');
        $(this).siblings().children('a').children('i').css('color', '#a3a3a3');
        $(this).siblings().children('span').removeClass('onSelected');
        if ($(this).index() != 3) { priceInit(); }
        if ($(this).index() == 4) {
            $(this).css('border-right', '1px solid #eee');
        }
    });
    function priceInit() {
        $('.priceRank').removeClass().addClass("rankMethod priceRank up");
        $('.priceRank').children('a').children('i').removeClass().addClass('fa-arrows-v');
    }
    function priceUp() {
        $('.priceRank').removeClass("down").addClass("up");
        $('.priceRank').children('a').children('i').removeClass().addClass('fa-long-arrow-up');
        $('.priceRank').removeClass("up").addClass("down");
    }
    function priceDown() {
        $('.priceRank').removeClass("up").addClass("down");
        $('.priceRank').children('a').children('i').removeClass().addClass('fa-long-arrow-down');
        $('.priceRank').removeClass("down").addClass("up");
    }
    $('.priceRank').click(function () {
        if ($(this).hasClass("up")) {
            priceUp();
        } else if ($(this).hasClass('down')) {
            priceDown();
        }
    });
    var _numcount = $(".specselMain li").length;
    (_numcount > 14) ? pinpaiMore() : $('.pinpaiMore').hide();
    function pinpaiMore() {
        $('.pinpaiMore').show().click(function () {
            if ($(".specselMain>ul").hasClass("hideAllPinpai")) {
                $(".specselMain>ul").removeClass("hideAllPinpai").addClass("showAllPinpai");
                $(this).children('span').html('收起');
            } else if ($(".specselMain>ul").hasClass("showAllPinpai")) {
                $(".specselMain>ul").removeClass("showAllPinpai").addClass("hideAllPinpai");
                $(this).children('span').html('更多');
            }
        });
    }

    $('.specToMore').click(function () {
        var hideEle = $(this).parent().prev();
        if (hideEle.height() == 20) {
            //console.log("123");
            hideEle.css('height', 'auto');
        } else {
            hideEle.css('height', '20px');
        }
    });
    /**/

});

function words_deal() {
    var curLength = $(".complainArea").val().length;
    if (curLength > 500) {
        var num = $(".complainArea").val().substr(0, 500);
        $(".complainArea").val(num);
    }
    else {
        $(".limited").text(500 - $(".complainArea").val().length);
    }
}
//列表页获取url参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};

function getCookie(name) {
    //获得cookie
    var bikky = document.cookie;
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

$(function () {
    var a = $(".ds_dialog_title h3").html();
    if (a == null || a.length == 0) {
        $(".ds_dialog_title").css("border", "none");
    }
});



function removeClick(btn1) {
    setTimeout(function () {
        btn1.removeClass("clicked");
    }, 500);
}

function dialogLogin_click(btn2) {
    $(btn2).addClass("clicked");
    var retUrl = window.location.href;
    if (retUrl.toLowerCase().indexOf("login") != -1) {
    } else {
        $.post("/Login/_Login", { isDialog: true }, function (data) {
            $("#dialogForLogin").empty();
            $("#dialogForLogin").html(data).show();
            onlong($('.dialogMain'));
        });
        return false;
    }
}





function onlong(_html) {
    var c_html = _html;
    //var mask = "<div class='mask2016'></div>";
    //$("body").append(mask);
    //$("body").append(c_html);

    ds.dialog({
        content: c_html,
        tijiao: function () {
            if (email != "" && pwd != "") {
                $.ajax({
                    url: "/home/index",
                    type: "Post",
                    data: {
                        account: email,
                        passWord: pwd,
                        IsJz: $("#m_login_rad").is(":checked")
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data.status == 1) {
                            $("#errorStr").css("display", "none");
                            //登陆成功
                            window.location.href = "/home/index";
                        } else {
                            $("#errorStr").css("display", "block");
                        }
                    }
                })
            }
            else {
                $("#errorStr").css("display", "block");
            }
        }
    });


}


;$(function () {
        //商品列表交互
        $('.ls_list_sc_txt').hide();
        $('ls_list_sc').click(function () {
            $('.ls_list_sc_txt').fadeIn(400);
           
        });
        $('.ls_addToCart_txt').hide();
        $('ls_addToCart').click(function () {
            $('.ls_addToCart_txt').fadeIn(400);
           
        });
    })
