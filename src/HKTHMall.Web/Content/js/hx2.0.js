/*首页图片轮播开始*/
    var t = n = 0, count;
    $(document).ready(function () {
        count = $("div.cb_banner ul li").length;
        $("div.cb_banner ul li:not(:first-child)").hide();
        $("div.tunedot a:first").toggleClass("cur");
        $("div.tunedot a").click(function () {
            var i = $(this).attr("data-key") - 1;//获取Li元素内的值，即1，2，3，4
            n = i;
            if (i >= count) return;
            $("div.cb_banner ul li").filter(":visible").fadeOut(500).parent().children().eq(i).fadeIn(1000);
            $(this).toggleClass("cur");
            $(this).siblings().removeAttr("class");
        });
        t = setInterval("autoShowAD()", 6000);
    });

    $(".arowle").click(function () {
        var currentIndex = $("div.tunedot a[class='cur']").attr("data-key");
        if (currentIndex == 1) {
            $("div.tunedot a:last").click();
        }
        else {
            $("div.tunedot a").eq(currentIndex - 2).click();
        }
    });

    $(".arowri").click(function () {
        var currentIndex = $("div.tunedot a[class='cur']").attr("data-key");
        if (currentIndex == count) {
            $("div.tunedot a:first").click();
        }
        else {
            $("div.tunedot a").eq(currentIndex).click();
        }
    });

    function autoShowAD() {
        n = n >= (count - 1) ? 0 : ++n;
        $("div.tunedot a").eq(n).trigger('click');
    }
/*首页图片轮播结束*/



/*公共导航开始*/
$(function() {
	var bBtn = false;
	var bW=false;
	var navW=0;
	
	$('#Z_TypeList').hover(function() {
		$('.Z_MenuList').queue(function(next) {
			$(this).css({
				'display': 'block',
				'overflow':'hidden'
			});
			next();
		}).animate({
			'height': '+=420px'
		},
		300,
		function() {
			$('ul.Z_MenuList_ul>li').each(function() {
				
				$(this).hover(function() {
					
					$(this).queue(function(next) {
						
						var ins = $(this).index();
						$(this).addClass('menuItemColor');

						$('.subView').css({
							'width': 0,
							'display': 'none'
						});
						
						function toNavW(){
							if (!bBtn) {
							if(parseInt($('header').width())>=1200){
								bW=true;
							}else{
								bW=false;
							}
							
							navW=bW?975:980;
							
							$('.Z_SubList').addClass('box-shadow');
							$('.Z_SubList').stop().css({
								'display': 'block'
							}).animate({
								'width': navW
							});
							$('.subView').eq(ins).stop().css({
								'display': 'block'
								
							}).animate({
								'width': navW
							},
							function() {
								bBtn = true;
							});
						} else {
							$('.subView').eq(ins).stop().css({
								'display': 'block'
							}).animate({
								'width': navW
							},
							0);
						}
						}
						toNavW();
						$(document).bind('ready',toNavW);
						$(window).bind('resize',function(){
						    $(document).unbind('ready',toNavW);
							$(document).bind('ready',toNavW);
						});						
						next();
					});

					//$(this).find('h3,p a').css('color', '#fff');
				},
				function() {
					$(this).removeClass('menuItemColor');
				});

			});

		});

	},
	function() {
		$(this).queue(function(next) {
			bBtn = false;
			$(this).find('.Z_MenuList').stop().css({
				'height': 0,
				'display': 'none'
			});
			$('.subView').css({
				'width': 0,
				'display': 'none'
			});
			$('.Z_SubList').removeClass('box-shadow');
			$('.Z_SubList').css({
				'width': 0,
				'display': 'none'
			});
			$('ul.Z_MenuList_ul>li').each(function() {
				$(this).removeClass('menuItemColor');
				//$(this).find('h3').css('color', '#000');
				//$(this).find('p a').css('color', '#888');
			})

			next();

		});

	});

});
/*公共导航结束*/




/*侧边栏开始*/
	$(window).scroll(function() {
		if($(this).scrollTop() > 580){
			$(".indexBroadside").fadeIn(500);
		}else{
			$(".indexBroadside").fadeOut(500);
			}
		
	});
	//侧边栏弹出框
	$("#User-login").hover(function(){
		$("div.navUserInfo").eq(0).fadeIn(500);
		},function(){
			$("div.navUserInfo").eq(0).fadeOut(500);
			});
	$("div.close").click(function(){
		$(this).parent().fadeOut(500);
		});
	$("#consult").hover(function(){
		$("div.navUserInfo").eq(1).fadeIn(500);
		},function(){
			$("div.navUserInfo").eq(1).fadeOut(500);
			});

/*侧边栏结束*/	
	
/*爆款推荐轮播*/
   (function($){$.fn.jCarouselLite=function(o){o=$.extend({btnPrev:null,btnNext:null,btnGo:null,mouseWheel:false,auto:null,speed:1000,easing:null,vertical:false,circular:true,visible:3,start:0,scroll:3,beforeStart:null,afterEnd:null},o||{});return this.each(function(){var b=false,animCss=o.vertical?"top":"left",sizeCss=o.vertical?"height":"width";var c=$(this),ul=$("ul",c),tLi=$("li",ul),tl=tLi.size(),v=o.visible;if(o.circular){ul.prepend(tLi.slice(tl-v-1+1).clone()).append(tLi.slice(0,v).clone());o.start+=v}var f=$("li",ul),itemLength=f.size(),curr=o.start;c.css("visibility","visible");f.css({overflow:"hidden",float:o.vertical?"none":"left"});ul.css({margin:"0",padding:"0",position:"relative","list-style-type":"none","z-index":"1"});c.css({overflow:"hidden",position:"relative","z-index":"2",left:"0px"});var g=o.vertical?height(f):width(f);var h=g*itemLength;var j=g*v;f.css({width:f.width(),height:f.height()});ul.css(sizeCss,h+"px").css(animCss,-(curr*g));c.css(sizeCss,j+"px");if(o.btnPrev)$(o.btnPrev).click(function(){return go(curr-o.scroll)});if(o.btnNext)$(o.btnNext).click(function(){return go(curr+o.scroll)});if(o.btnGo)$.each(o.btnGo,function(i,a){$(a).click(function(){return go(o.circular?o.visible+i:i)})});if(o.mouseWheel&&c.mousewheel)c.mousewheel(function(e,d){return d>0?go(curr-o.scroll):go(curr+o.scroll)});if(o.auto)setInterval(function(){go(curr+o.scroll)},o.auto+o.speed);function vis(){return f.slice(curr).slice(0,v)};function go(a){if(!b){if(o.beforeStart)o.beforeStart.call(this,vis());if(o.circular){if(a<=o.start-v-1){ul.css(animCss,-((itemLength-(v*2))*g)+"px");curr=a==o.start-v-1?itemLength-(v*2)-1:itemLength-(v*2)-o.scroll}else if(a>=itemLength-v+1){ul.css(animCss,-((v)*g)+"px");curr=a==itemLength-v+1?v+1:v+o.scroll}else curr=a}else{if(a<0||a>itemLength-v)return;else curr=a}b=true;ul.animate(animCss=="left"?{left:-(curr*g)}:{top:-(curr*g)},o.speed,o.easing,function(){if(o.afterEnd)o.afterEnd.call(this,vis());b=false});if(!o.circular){$(o.btnPrev+","+o.btnNext).removeClass("disabled");$((curr-o.scroll<0&&o.btnPrev)||(curr+o.scroll>itemLength-v&&o.btnNext)||[]).addClass("disabled")}}return false}})};function css(a,b){return parseInt($.css(a[0],b))||0};function width(a){return a[0].offsetWidth+css(a,'marginLeft')+css(a,'marginRight')};function height(a){return a[0].offsetHeight+css(a,'marginTop')+css(a,'marginBottom')}})(jQuery);



$(function() {
$("#botton-scroll").jCarouselLite({
btnNext: ".next",
btnPrev: ".prev"
});
});

$(function () {
$('#top-menu li').hover(
function () {$('ul', this).slideDown(200);}, 
function () {$('ul', this).slideUp(200);
});
});

$(function () {
$(".click").click(function(){
$("#panel").slideToggle("slow");
$(this).toggleClass("active"); return false;
}); 
});

$(function () {
$('.fade').hover(
function() {$(this).fadeTo("slow", 0.5);},
function() {$(this).fadeTo("slow", 5);
});
});
/*爆款推荐结束*/	