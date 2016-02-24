////图片放大镜2.0
//(function ($) {
//    $.fn.jqueryzoom = function (options) {
//        var settings = {
//            xzoom: 200,//zoomed width default width
//            yzoom: 200,//zoomed div default width
//            offset: 10,	//zoomed div default offset
//            position: "right",//zoomed div default position,offset position is to the right of the image
//            lens: 1, //zooming lens over the image,by default is 1;
//            preload: 1
//        };

//        if (options) {
//            $.extend(settings, options);
//        }

//        var noalt = '';
//        $(this).hover(function () {

//            var imageLeft = this.offsetLeft;
//            var imageRight = this.offsetRight;
//            var imageTop = $(this).get(0).offsetTop;
//            var imageWidth = $(this).children('img').get(0).offsetWidth;
//            var imageHeight = $(this).children('img').get(0).offsetHeight;


//            noalt = $(this).children("img").attr("alt");

//            var bigimage = $(this).children("img").attr("jqimg");

//            $(this).children("img").attr("alt", '');

//            if ($(".margPic .zoomdiv").get().length == 0) {

//                $(this).after("<div class='zoomdiv'><img class='bigimg' src='" + bigimage + "'/></div>");


//                $(this).append("<div class='jqZoomPup'>&nbsp;</div>");

//            }


//            if (settings.position == "right") {

//                if (imageLeft + imageWidth + settings.offset + settings.xzoom > screen.width) {

//                    leftpos = imageLeft - settings.offset - settings.xzoom;

//                } else {

//                    leftpos = imageLeft + imageWidth + settings.offset;
//                }
//            } else {
//                leftpos = imageLeft - settings.xzoom - settings.offset;
//                if (leftpos < 0) {

//                    leftpos = imageLeft + imageWidth + settings.offset;

//                }

//            }

//            $(".margPic .zoomdiv").css({ top: imageTop, left: leftpos });

//            $(".margPic .zoomdiv").width(settings.xzoom);

//            $(".margPic .zoomdiv").height(settings.yzoom);

//            $(".margPic .zoomdiv").show();

//            if (!settings.lens) {
//                $(this).css('cursor', 'crosshair');
//            }
//            $(document.body).mousemove(function (e) {
//                mouse = new MouseEvent(e);
//                /*$("div.jqZoomPup").hide();*/
//                var bigwidth = $(".bigimg").get(0).offsetWidth;
//                var bigheight = $(".bigimg").get(0).offsetHeight;
//                var scaley = 'x';
//                var scalex = 'y';
//                if (isNaN(scalex) | isNaN(scaley)) {
//                    var scalex = (bigwidth / imageWidth);
//                    var scaley = (bigheight / imageHeight);
//                    $(".margPic .jqZoomPup").width((settings.xzoom) / scalex);
//                    $(".margPic .jqZoomPup").height((settings.yzoom) / scaley);
//                    if (settings.lens) {
//                        $(".margPic .jqZoomPup").css('visibility', 'visible');
//                    }
//                }
//                xpos = mouse.x - $(".margPic .jqZoomPup").width() / 2 - imageLeft;
//                ypos = mouse.y - $(".margPic .jqZoomPup").height() / 2 - imageTop;
//                if (settings.lens) {
//                    xpos = (mouse.x - $(".margPic .jqZoomPup").width() / 2 < imageLeft) ? 0 : (mouse.x + $("div.jqZoomPup").width() / 2 > imageWidth + imageLeft) ? (imageWidth - $("div.jqZoomPup").width() - 2) : xpos;
//                    ypos = (mouse.y - $(".margPic .jqZoomPup").height() / 2 < imageTop) ? 0 : (mouse.y + $("div.jqZoomPup").height() / 2 > imageHeight + imageTop) ? (imageHeight - $("div.jqZoomPup").height() - 2) : ypos;
//                }
//                if (settings.lens) {
//                    $(".margPic .jqZoomPup").css({ top: ypos, left: xpos });
//                }
//                scrolly = ypos;
//                $(".margPic .zoomdiv").get(0).scrollTop = scrolly * scaley;
//                scrollx = xpos;
//                $(".margPic .zoomdiv").get(0).scrollLeft = (scrollx) * scalex;
//            });
//        }, function () {
//            $(this).children("img").attr("alt", noalt);
//            $(document.body).unbind("mousemove");
//            if (settings.lens) {
//                $(".margPic .jqZoomPup").remove();
//            }
//            $(".margPic .zoomdiv").remove();
//        });
//        count = 0;
//        if (settings.preload) {
//            $('body').append("<div style='display:none;' class='jqPreload" + count + "'>sdsdssdsd</div>");
//            $(this).each(function () {
//                var imagetopreload = $(this).children("img").attr("jqimg");
//                var content = jQuery('.margPic .jqPreload' + count + '').html();
//                jQuery('.margPic .jqPreload' + count + '').html(content + '<img src=\"' + imagetopreload + '\">');
//            });
//        }
//    }

//})(jQuery);

function MouseEvent(e) {
    this.x = e.pageX;
    this.y = e.pageY;
}
//=====================全局函数========================
//Tab控制函数
function tabs(tabId, tabNum) {
    //设置点击后的切换样式
    $(tabId + " .tab li").removeClass("curr");
    $(tabId + " .tab li").eq(tabNum).addClass("curr");
    //根据参数决定显示内容
    $(tabId + " .tabcon").hide();
    $(tabId + " .tabcon").eq(tabNum).show();
}
//=====================全局函数========================

//==================图片详细页函数=====================
//鼠标经过预览图片函数
//function preview(img) {
//    $('#items li img').addClass('addborder').siblings().removeClass('addborder');
//    $("#preview .jqzoom img").attr("src", $(img).attr("src"));
//    $("#preview .jqzoom img").attr("jqimg", $(img).attr("bimg"));
//}

////图片放大镜效果
//$(function () {
//    $(".jqzoom").jqueryzoom({ xzoom: 380, yzoom: 410 });
//});

//图片预览小图移动效果,页面加载时触发
$(function () {
    var tempLength = 0; //临时变量,当前移动的长度
    var viewNum = 4; //设置每次显示图片的个数量
    var moveNum = 2; //每次移动的数量
    var moveTime = 300; //移动速度,毫秒
    var scrollDiv = $(".spec-scroll .items ul"); //进行移动动画的容器
    var scrollItems = $(".spec-scroll .items ul li"); //移动容器里的集合
    var moveLength = scrollItems.eq(0).height() * moveNum; //计算每次移动的长度
    var countLength = (scrollItems.length - viewNum) * scrollItems.eq(0).height(); //计算总长度,总个数*单个长度

    //下一张
    $(".spec-scroll .next").bind("click", function () {
        if (tempLength < countLength) {
            if ((countLength - tempLength) > moveLength) {
                scrollDiv.animate({ top: "-=" + moveLength + "px" }, moveTime);
                tempLength += moveLength;
            } else {
                scrollDiv.animate({ top: "-=" + (countLength - tempLength) + "px" }, moveTime);
                tempLength += (countLength - tempLength);
            }
        }
    });
    //上一张
    $(".spec-scroll .prev").bind("click", function () {
        if (tempLength > 0) {
            if (tempLength > moveLength) {
                scrollDiv.animate({ top: "+=" + moveLength + "px" }, moveTime);
                tempLength -= moveLength;
            } else {
                scrollDiv.animate({ top: "+=" + tempLength + "px" }, moveTime);
                tempLength = 0;
            }
        }
    });
});


//========Tab控制函数==========
function tabs(tabClass, tabNum) {
    //设置点击后的切换样式
    $(tabClass + "  li").children('a').removeClass("tab_on");
    $(tabClass + "  li").eq(tabNum).children('a').addClass("tab_on");
    //根据参数决定显示内容
    $("div.spp").hide().eq(tabNum).show();
}


$(function () {
    var productId = $("#productId").val();
    $("div.ClientAssess ul li").click(function () {
        tabs("div.ClientAssess ul", $(this).index());
    });
    //========Tab控制函数============end. 
    jQuery("ul li a[data-xxx]").click(function () {
        var txt = jQuery(this).attr("data-xxx"), xata = txt.split("|");
        jQuery.post("/" + xata[xata.length - 1], { key: productId, page: 1, commentLevel: xata[1] }, function (data) {
            jQuery("#" + xata[0]).empty().append(data);
        });
    });
    //jQuery("#pingjia").click();
    //jQuery('.tunepage li a').Page({ IsAjax: true }, { key: productId });


    $("div.ls_shoucang dl").click(function () {
        //if (jQuery("div.ls_shoucang i:first").hasClass("heart")) {
        addCollection(productId);
        $(this).children("i").addClass("zoomIn").css('background-position', ' -20px -1044px');
        //}
    });

    //刷新收藏
    refreshCollection(productId);
});

function refreshCollection(productId) {
    $.post("/Product/Iscollected", { key: productId }, function (data) {
        if (data.Data) {
            //jQuery("div.GoodsShare i:first").removeClass("heart").addClass("heart2");
            $("div.ls_shoucang a:first").empty().html($commonLang.HOME_SHOPPING_COLLECT);//"已收藏"
        }
    });
}
function addCollection(productId) {
    $.post("/Product/AddToCollection", { key: productId }, function (data) {
        if (data.Data == undefined) {
            window.location = '/Login/Index?ReturnUrl=' + encodeURIComponent(window.location.href);
        } else if (data.Data) {
            $("div.ls_shoucang a:first").html($commonLang.HOME_SHOPPING_COLLECT);//"已收藏"
        }
    });
}

//@*facebook分享插件*@
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en/sdk.js#xfbml=1&version=v2.3";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));