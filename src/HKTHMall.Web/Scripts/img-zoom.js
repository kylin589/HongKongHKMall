/**
 * Created by liyingzheng on 2015/6/1.
 */

//==================图片详细页函数=====================
//鼠标经过预览图片函数
function preview(img) {
    $(".D_piclist .select").removeClass("select");
    $(img).addClass("select");

    $(".PicBig img").attr("src", $(img).children("img").attr("src"));
    $(".PicBig img").attr("data-img-max", "");
    $(".PicBig img").attr("data-img-max", $(img).children("img").attr("data-img-max"));
}

//图片放大镜效果
$(function () {
    preview(".D_piclist a:first");
    $(".D_piclist li a").hover(function () {
        preview($(this));
    });
    $(".jqzoom").mousemove(
        function (e) {
           
            var imgSrc = $("img.bigimg").attr("src");
            var imgMaxUrl = $(this).children("img").attr("data-img-max");
            //如果大图片地址不存在,就不处理。
            if (imgMaxUrl == null || imgMaxUrl == undefined || imgMaxUrl == '') {
                return;
            }
            if (imgSrc != imgMaxUrl) {
                $("img.bigimg").attr("src", imgMaxUrl);
            }
           
            //
            //========选取层==============
            xpos = e.pageX - $(this).offset().left - $("div.jqZoomPup").width() / 2;
            xpos = xpos < 0 ? 0 : xpos;
            xpos = xpos > ($(this).width() - $("div.jqZoomPup").width() - 1) ? ($(this).width() - $("div.jqZoomPup").width() - 1) : xpos;

            ypos = e.pageY - $(this).offset().top - $("div.jqZoomPup").height() / 2;
            ypos = ypos < 0 ? 0 : ypos;
            ypos = ypos > ($(this).height() - $("div.jqZoomPup").height()) ? ($(this).height() - $("div.jqZoomPup").height()) : ypos;

            $("div.jqZoomPup").css({ top: ypos, left: xpos });
            $("div.jqZoomPup").show();

            //========选取层============== end.
            var zoomLeftOffset = 10;
            //放大显示位置
            $("div.zoomdiv").css({ top: $("div.Deta_m").position().top, left: $(this).position().right + zoomLeftOffset });
 
            //显示图片部分
            $("div.zoomdiv").show();            
            $("img.bigimg").css({ top: -ypos * 2, left: -xpos * 2 - zoomLeftOffset });

        });
    $(".jqzoom").mouseleave(function () {
        $("div.jqZoomPup,div.zoomdiv").hide();
    });

    /*动态切换方法*/
    ScrollImg(".D_piclist", ".pic-forward", ".pic-backward", 72, 5);
});

function ScrollImg(mainObj, leftBtn, rightBtn, Width, PageSize) {
    /*动态切换方法*/
    Num = 0;
    Page = 1;
    Page2 = $(mainObj).children().size();
    //缩略图左右按钮点击切换
    $(leftBtn).click(function () {
        if (Num > 0) {
            Num--;
            scrollImg(Num);
            ChageButton(Num);
        }
    });
    $(rightBtn).click(function () {
        if (Num < (Page2 - Page * PageSize)) {
            Num++;
            scrollImg(Num);
            ChageButton(Num);
        }
    });
    if (Page2 > PageSize) {
        $(rightBtn).addClass("select");
    }
    //左右小按钮的状态
    function ChageButton(Num) {
        if (Num <= 0) {
            $(leftBtn).removeClass("select");
        } else {
            $(leftBtn).addClass("select");
        }
        if (Num >= Page2 - Page * PageSize) {
            $(rightBtn).removeClass("select");
        } else {
            $(rightBtn).addClass("select");
        }
    }
    function scrollImg(Num) {
        $(mainObj).animate({ left: "-" + Num * Width + "px" }, 200);
    }
}