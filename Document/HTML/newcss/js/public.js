;(function($) {  
    $.fn.fonts = function(option){
        option = $.extend({},$.fn.fonts.option,option);
        return this.each(function(){
        var objString = $(this).text(),
            objLength = $(this).text().length,
            num = option.fontNum;
        if(objLength > num){
            objString = $(this).text(objString.substring(0,num) + "···");
        }
       });
    }
    // default options
    $.fn.fonts.option = {
    fontNum:100 //font num
    };
    
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
                        'background':'#fff',
                        'boxShadow':'0 2px 10px #ccc'
                    });
                    element.children().css({
                    	'paddingTop':'15px',
                    	'height':'55px',
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
                    'background':'none',
                    'boxShadow':'0 0 0'
                });
                element.children().css({
                	'paddingTop':'26px',
                	'height':'71px',
                });
            }
        });
    };
    return $(this).each(function () {
        position(this);
    });
};
$(document).ready(function() {


	$("#floatSearch").smartFloat();


    $(".forceSldier_2016").hover(function(){
		$(this).addClass('linkHover');
		$(this).find('i.dropIcon').addClass("fa-rotate-180");
		$(this).find('div.dropHide').slideDown();
	},function(){
		$(this).removeClass('linkHover');
		$(this).find('i.dropIcon').removeClass("fa-rotate-180");
		$(this).find('div.dropHide').stop().slideUp();
	});
	//console.log($(".lSPager").width());


	$(".cateInner>li").each(function(){
		$(this).hover(function(){
			$(this).children("div.cateInnerHide").show();
		},function(){
			$(this).children("div.cateInnerHide").stop().hide();
		});
		
	});


	




	$(".priceLimited").fonts({fontNum:12});
    $('.lazyBoomCover').each(function(){
        $('.boomName').fonts({fontNum:19});
    });
	$('.lbSearchHistory').each(function(){
        $('.scanLimited').fonts({fontNum:30});
    });
    $('.lazyBoomTitle li').bind('click',function(){
        $(this).addClass('active').siblings().removeClass('active');
        if($(this).index()==0){
            $('.lazyBoomCover_1').show().each(function(){
                $('.lazyBoomMain_1').addClass('flipInX');
            });
            $('.lazyBoomCover_2').hide().each(function(){
                $('.lazyBoomMain_2').removeClass('flipInX');
            });
        }else if($(this).index()==1){
            $('.lazyBoomCover_1').hide().each(function(){
                $('.lazyBoomMain_1').removeClass('flipInX');
            });
            $('.lazyBoomCover_2').show().each(function(){
                $('.lazyBoomMain_2').addClass('flipInX');
            });
        }
    });
	/*判断有没有下拉*/
	if($(document).find('div').hasClass('main_index')){
		//console.log('123');
		$('.cateCover').css('display','block');
	}else{
		//console.log('222');
		$('.cateCover').css('display','none');
		$('.navCategorys').hover(function(){
			$('.cateCover').slideDown();
		},function(){
			$('.cateCover').stop().slideUp();
		});
	}
	/*列表页*/
    $(".lbSpector").each(function(){
        $(this).find("span").bind("click",function(){
            $(this).children("i").toggleClass("fa-rotate-90");
            $(this).next("dl").slideToggle();
        });
    });

	$(".lbSpector>li").each(function(){
		var isShow = $(this).data('show');
		console.log(isShow);
		if (isShow == 1) {
			$(this).find("i").addClass("fa-rotate-90");
            $(this).find("dl").slideDown();
		}
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
	$('.rankMethod').eq(0).children('a').css('color','#ef5959');
	$('.rankMethod').eq(0).children('a').children('i').css('color','#ef5959');
	$('.rankMethod').eq(0).children('span').addClass('onSelected');
	$('.rankMethod').click(function(){
		$('.rankMethod').css('border-right','0');
		$(this).children('a').css('color','#ef5959');
		$(this).children('a').children('i').css('color','#ef5959');
		$(this).children('span').addClass('onSelected');
		$(this).siblings().children('a').css('color','#4e4e4e');
		$(this).siblings().children('a').children('i').css('color','#a3a3a3');
		$(this).siblings().children('span').removeClass('onSelected');
		if($(this).index()!=3){priceInit();}
		if($(this).index() == 4){
			$(this).css('border-right','1px solid #eee');
		}
	});
	function priceInit(){
		$('.priceRank').removeClass().addClass("rankMethod priceRank up");
		$('.priceRank').children('a').children('i').removeClass().addClass('fa-arrows-v');
	}
	function priceUp(){
		$('.priceRank').removeClass("down").addClass("up");
		$('.priceRank').children('a').children('i').removeClass().addClass('fa-long-arrow-up');
		$('.priceRank').removeClass("up").addClass("down");
	}
	function priceDown(){
		$('.priceRank').removeClass("up").addClass("down");
		$('.priceRank').children('a').children('i').removeClass().addClass('fa-long-arrow-down');
		$('.priceRank').removeClass("down").addClass("up");
	}
	$('.priceRank').click(function(){
		if($(this).hasClass("up")){
			priceUp();
		}else if($(this).hasClass('down')){
			priceDown();
		}
	});

	$('.pinpaiMore').click(function(){
		if($(".specselMain>ul").hasClass("hideAllPinpai")){
			$(".specselMain>ul").removeClass("hideAllPinpai").addClass("showAllPinpai");
			$(this).children('span').html('收起');
		}else if($(".specselMain>ul").hasClass("showAllPinpai")){
			$(".specselMain>ul").removeClass("showAllPinpai").addClass("hideAllPinpai");
			$(this).children('span').html('更多');
		}
	});

	$('.seeSpectorMore').click(function(){
		//alert("123");
		var _this = $(this);
		(_this.prevUntil("ul").height == '20')?_this.prevUntil("ul").css('height','auto'):_this.prevUntil("ul").css('height','20px');
	});


	$('.specToMore').click(function(){
		var hideEle = $(this).parent().prev();
		if(hideEle.height() == 20){
			//console.log("123");
			hideEle.css('height','auto');
		}else{
			hideEle.css('height','20px');
		}
	});
    /**/
	
});

function words_deal()
	{
	   var curLength=$(".complainArea").val().length;
	   if(curLength>500)
	   {
	        var num=$(".complainArea").val().substr(0,500);
	        $(".complainArea").val(num);
	   }
	   else
	   {
	        $(".limited").text(500-$(".complainArea").val().length);
	   }
}
//列表页获取url参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}


