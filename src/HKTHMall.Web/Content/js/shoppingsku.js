//保存最后的组合结果信息
var SKUResult = {};
//获得对象的key
function getObjKeys(obj) {
    if (obj !== Object(obj)) throw new TypeError('Invalid object');
    var keys = [];
    for (var key in obj)
        if (Object.prototype.hasOwnProperty.call(obj, key))
            keys[keys.length] = key;
    return keys;
}

//把组合的key放入结果集SKUResult && _database.Db.SKU_Product.IsUse == 1
function add2SKUResult(combArrItem, sku) {
    var key = combArrItem.join("_");
    if (SKUResult[key]) {//SKU信息key属性·
        SKUResult[key].count = sku.Stock;
        SKUResult[key].sku_productsid.push(sku.SKU_ProducId);
        SKUResult[key].prices.push(sku.HKPrice);
        SKUResult[key].marketprices.push(sku.MarketPrice);
        SKUResult[key].isuse.push(sku.IsUse);
    } else {
        SKUResult[key] = {
            count: sku.Stock,
            prices: [sku.HKPrice],
            marketprices: [sku.MarketPrice],
            sku_productsid: [sku.SKU_ProducId],
            isuse: [sku.IsUse]
        };
    }
}
/**
         * 从数组中生成指定长度的组合
         * 方法: 先生成[0,1...]形式的数组, 然后根据0,1从原数组取元素，得到组合数组
         */
function combInArray(aData) {
    if (!aData || !aData.length) {
        return [];
    }

    var len = aData.length;
    var aResult = [];

    for (var n = 1; n < len; n++) {
        var aaFlags = getCombFlags(len, n);
        while (aaFlags.length) {
            var aFlag = aaFlags.shift();
            var aComb = [];
            for (var i = 0; i < len; i++) {
                aFlag[i] && aComb.push(aData[i]);
            }
            aResult.push(aComb);
        }
    }

    return aResult;
}

/**
 * 得到从 m 元素中取 n 元素的所有组合
 * 结果为[0,1...]形式的数组, 1表示选中，0表示不选
 */
function getCombFlags(m, n) {
    if (!n || n < 1) {
        return [];
    }

    var aResult = [];
    var aFlag = [];
    var bNext = true;
    var i, j, iCnt1;

    for (i = 0; i < m; i++) {
        aFlag[i] = i < n ? 1 : 0;
    }

    aResult.push(aFlag.concat());

    while (bNext) {
        iCnt1 = 0;
        for (i = 0; i < m - 1; i++) {
            if (aFlag[i] == 1 && aFlag[i + 1] == 0) {
                for (j = 0; j < i; j++) {
                    aFlag[j] = j < iCnt1 ? 1 : 0;
                }
                aFlag[i] = 0;
                aFlag[i + 1] = 1;
                var aTmp = aFlag.concat();
                aResult.push(aTmp);
                if (aTmp.slice(-n).join("").indexOf('0') == -1) {
                    bNext = false;
                }
                break;
            }
            aFlag[i] == 1 && iCnt1++;
        }
    }
    return aResult;
}

//初始化得到结果集
function initSKU() {
    var i, j, skuKeys = getObjKeys(data);
    for (i = 0; i < skuKeys.length; i++) {
        var skuKey = skuKeys[i];//一条SKU信息key
        var sku = data[skuKey];	//一条SKU信息value
        var skuKeyAttrs = skuKey.split("_"); //SKU信息key属性值数组
        skuKeyAttrs.sort(function (value1, value2) {
            return parseInt(value1) - parseInt(value2);
        });

        //对每个SKU信息key属性值进行拆分组合
        var combArr = combInArray(skuKeyAttrs);
        for (j = 0; j < combArr.length; j++) {
            add2SKUResult(combArr[j], sku);
        }

        //结果集接放入SKUResult
        SKUResult[skuKeyAttrs.join("_")] = {
            count: sku.Stock,
            prices: [sku.HKPrice],
            isuse: [sku.IsUse],
            marketprices: [sku.MarketPrice],
            sku_productsid: [sku.SKU_ProducId]

        }
    }
}

var sku_attribute_count = parseInt($("div.ls_proinfo_3").attr("data-attribute-count"));
var sku_attribute_select_price = 0.00;

//初始化用户选择事件
function userInit() {
    if (sku_attribute_count > 0) {
        initSKU();
        $("div.yListr .ls_size li").each(function () {
            var self = $(this);
            var attr_id = self.attr('data-valueid');
            if (!SKUResult[attr_id]) {
                self.addClass("disabled").attr('disabled', 'disabled');
                //self.attr('disabled', 'disabled');
            }
        }).click(function () {
            var self = $(this);
            self.addClass("ls_on").siblings().removeClass("ls_on");
            //选中自己，兄弟节点取消选中
            //self.toggleClass('yListrclickem').siblings().removeClass('yListrclickem');
            //已经选择的节点
            var selectedObjs = $("div.yListr").find('.ls_on');
            if (selectedObjs.length) {
                //获得组合key价格
                var selectedIds = [];
                $(".ls_select>dd").html("");
                $(".ls_select").show();
                selectedObjs.each(function () {
                    selectedIds.push($(this).attr('data-valueid'));
                    //设置选择项
                    $(".ls_select>dd").append("&nbsp;<span>");
                    $(".ls_select>dd").append("\"");
                    $(".ls_select>dd").append($(this).find("a").attr("title"));
                    $(".ls_select>dd").append("\"");
                    $(".ls_select>dd").append("</span>");
                });
                selectedIds.sort(function (value1, value2) {
                    return parseInt(value1) - parseInt(value2);
                });
                var len = selectedIds.length;

                //用已选中的节点验证待测试节点 underTestObjs
                $("div.yListr li").not(selectedObjs).not(self).each(function () {
                    var siblingsSelectedObj = $(this).siblings('.ls_on');
                    var testAttrIds = [];//从选中节点中去掉选中的兄弟节点
                    if (siblingsSelectedObj.length) {
                        var siblingsSelectedObjId = siblingsSelectedObj.attr('data-valueid');
                        for (var i = 0; i < len; i++) {
                            (selectedIds[i] != siblingsSelectedObjId) && testAttrIds.push(selectedIds[i]);
                        }
                    } else {
                        testAttrIds = selectedIds.concat();
                    }
                    testAttrIds = testAttrIds.concat($(this).attr('data-valueid'));
                    testAttrIds.sort(function (value1, value2) {
                        return parseInt(value1) - parseInt(value2);
                    });
                    if (!SKUResult[testAttrIds.join('_')]) {
                        $(this).addClass("disabled").attr('disabled', 'disabled').removeClass('ls_on');
                        $("#buyNow,#addToCart").addClass("noStock");

                    } else {
                        //var skureItem = SKUResult[testAttrIds.join('_')];
                        if (SKUResult[selectedIds.join('_')].count < 0 || (SKUResult[selectedIds.join('_')].count > 0 && !SKUResult[selectedIds.join('_')].isuse[0])) {
                            $(this).addClass("disabled").attr('disabled', 'disabled').removeClass('ls_on');
                            $("#buyNow,#addToCart").addClass("noStock");
                            return false;
                        }
                        else {
                            $(this).removeClass("disabled").removeAttr('disabled');
                            $("#buyNow,#addToCart").removeClass("noStock");
                        }
                    }
                });
                //设置价格
                var prices = SKUResult[selectedIds.join('_')].prices;
                var maxPrice = Math.max.apply(Math, prices);
                var minPrice = Math.min.apply(Math, prices);

                //判断是否有爆款 
                var isExplosion = parseInt($("#intputpromotionProduct").val());

                //处理显示价格库存
                if (isExplosion > 0) {
                    var discount = parseFloat($("#intputpromotionProduct").attr("discount"));
                    var HKPrice = 0;
                    var marketPrice = maxPrice;
                    if (discount > 0) {
                        HKPrice = (marketPrice * discount);
                    } else {
                        HKPrice = marketPrice;
                    }
                    $("#HKPrice").text("$" + HKPrice.toFixed(2));
                    sku_attribute_select_price = HKPrice.toFixed(2);//保存商品價
                    $("#pCostprice").text("$" + marketPrice.toFixed(2));
                    //$("#pDiscount").text(((1 - discount) * 100).toFixed(0) + " % OFF");
                    $("#pDiscount").text(((1 - discount) * HKPrice).toFixed(2) + " OFF");
                } else {
                    var marketprices = SKUResult[selectedIds.join('_')].marketprices;
                    var maxmarketPrice = Math.max.apply(Math, marketprices);
                    var minmarketPrice = Math.min.apply(Math, marketprices);
                    $("#HKPrice").text("$" + maxPrice.toFixed(2));        //惠卡价
                    sku_attribute_select_price = maxPrice.toFixed(2);     //保存商品價
                    $("#pCostprice").text("$" + maxmarketPrice.toFixed(2));   //市场价
                    //$("#pDiscount").text(((1 - ((maxmarketPrice - maxPrice) / maxmarketPrice)) * 100).toFixed(0) + " % OFF");
                    $("#pDiscount").text((maxmarketPrice - maxPrice).toFixed(2) + " OFF");
                }

                //设置库存
                setSkuStock(SKUResult[selectedIds.join('_')].count);

                var selectCount = $('div.yListr li.ls_on').size();
                if (sku_attribute_count > 0 && sku_attribute_count == selectCount) {

                    SKU_ProducId = SKUResult[selectedIds.join('_')].sku_productsid.join("");
                    //处理sku到立即购买的按钮
                    var buyUrl = $("#buyNow").attr("href");
                    $("#buyNow").attr("href", buyUrl.replace(/skunumber=[\w- ]*&/, 'skunumber=' + SKU_ProducId + '&'));

                    //显示,因为有可能已经隐藏了。 
                    //判断框框是否有显示没有显示
                    if ($("div.yListr").hasClass("attention")) {
                        $('#gouwuButSure').show();
                    }
                }
                else if (sku_attribute_count > 0 && sku_attribute_count > selectCount) {
                    if ($("div.yListr").hasClass("attention")) {
                        $('#gouwuButSure').hide();
                    }
                }

            } else {
                ////设置默认价格(初始化到原来的价格)
                //$('#price').text('--');

                $('.red.prdStock').html($("#defualtCount").val());
                //设置属性状态
                $('div.yListr li').each(function () {
                    SKUResult[$(this).attr('data-valueid')] ? $(this).removeClass('disabled').removeAttr('disabled') : $(this).addClass('disabled').attr('disabled', 'disabled').removeClass('ls_on');
                });
            }
        });
    }
}
$(function () {
    userInit();
    check_total_stock();
});
userInit();

//关闭按钮事件
$('.yListr .closered').find('a[name="closered"]').click(function () {
    $("div.yListr").removeClass("attention");
    $("#buyNow,#addToCart").show();//显示,因为有可能已经隐藏了。
    $('.yListr .closered').hide();
    $('.yListr .gouwuBut').hide();
});

//检查总库存为0时,购买按钮的处理
function check_total_stock() {
    var total_stock = parseInt($("div.SelectClass").attr("data-total-stock"));
    var skutotalstock = parseInt($("span.prdStock").text());
    if (total_stock <= 0) {
        $("#buyNow,#addToCart").removeClass("disabled").addClass("disabled");
        $("#buyNow,#addToCart").addClass("noStock");
    }
    else if (skutotalstock <= 0) {
        $("#buyNow,#addToCart").removeClass("disabled").addClass("disabled");
        $("#buyNow,#addToCart").addClass("noStock");
    }

}

//==========购买量处理============
$("input.tb-text").InputInt();
$("input.tb-text").change(function () {
    addBuyCount(0);
});


$("a.tb-reduce").click(function () {
    addBuyCount(-1);
});
$("a.tb-increase").click(function () {
    addBuyCount(1);
});


function addBuyCount(buyCount) {
    var qtyMax = $("input.tb-text").attr("data-max");
    var maxBuyCount = parseInt(qtyMax);
    //maxBuyCount = explosion_SingleQuotaAmount > 0 && maxBuyCount > explosion_SingleQuotaAmount ? explosion_SingleQuotaAmount : maxBuyCount;//爆款购买量限制使用。

    var currCount = parseInt($("input.tb-text").val());
    currCount = isNaN(currCount) ? 1 : currCount;
    currCount = currCount + buyCount;
    currCount = (currCount > maxBuyCount) ? maxBuyCount : currCount;
    if (qtyMax > 0) {
        currCount = (currCount <= 0) ? 1 : currCount;
    } else {
        currCount = (currCount < 0) ? 0 : currCount;
    }

    $("input.tb-text").val(currCount);
    var buyUrl = $("#buyNow").attr("href");
    $("#buyNow").attr("href", buyUrl.replace(/count=[0-9]*/, 'count=' + currCount));
}

//加载处理SKU选择。 
var currenSku = ''; //当前SKU
var SKU_ProducId = 0;   //SKUid


//设置显示当前sku库存
function setSkuStock(temp_stock) {
    $("span.prdStock").text(temp_stock);
    $("input.tb-text").attr("data-max", temp_stock);
    var minValue = temp_stock == 0 ? 0 : 1;
    $("input.tb-text").attr("value", minValue);
    var currCount = parseInt($("input.tb-text").val());
    if (currCount == 0) {
        if (temp_stock > 0) {
            addBuyCount(1);
        } else {
            addBuyCount(0);
        }
    } else {
        addBuyCount(0);
    }
}



//初始化按钮事件
$(function () {
    $("#addToCart").click(function (e) {
        if (checkBuyButton(this) == false) {
            return false;
        }
        else {
            //var cookinfo = getCookie("UserID");
            //if (cookinfo != null) {
            addCart(e);
            //} else {
            //    ($("#addToCart").hasClass("clicked")) ? removeClick($(this)) : dialogLogin_click($(this));
            //    return false;
            //}
        }
    });




    $("#buyNow").click(function () {
        if (checkBuyButton(this) == false) {
            return false;
        }

        var cookinfo = getCookie("UserID");
        if (cookinfo == null) {
            ($("#buyNow").hasClass("clicked")) ? removeClick($(this)) : dialogLogin_click($(this));
            return false;
        }



        if (/count=0/.exec($("#buyNow").attr("href"))) {
            return false;
        }
        if (/skunumber=0/.exec($("#buyNow").attr("href"))) {
            $("#buyNow").attr("href", "javascript:void(0)");
            return false;
        }
        return true;
    });

    //选中确定按钮
    $('#gouwuButSure').click(function (e) {
        $("div.yListr").removeClass("attention");
        //$("#buyNow,#addToCart").show();//显示,因为有可能已经隐藏了。
        $('.yListr .closered').hide();
        if ($("#buyNow").hasClass('action')) {
            $("#buyNow").show();
        }
        if ($("#addToCart").hasClass('action')) {
            $("#addToCart").show();
        }
        $("#buyNow,#addToCart").removeClass("action");
        $("div.yListr").removeClass("attention");

        $(this).hide();
        $('.yListr .gouwuBut').hide();
    });
});

//检查用户是否已登录，略安全
function checkUserLogin() {
    var curLoginUser = myCommonHelper.getCur_YH_User();
    if (curLoginUser != null) {
        return false;
    }
    else {
        return true;
    }
}


function checkBuyButton(btn) {
    if ($(btn).hasClass('disabled')) {
        return false;
    }

    var selectCount = $('div.yListr li.ls_on').size();
    if (selectCount == undefined || selectCount <= 0) {
        $("div.yListr").removeClass("attention").addClass("attention");
        $('.yListr .closered').show();
        return false;
    }


    if ($(btn).hasClass('action')) {
        $("#buyNow,#addToCart").removeClass("action");
        $("div.yListr").removeClass("attention");
        $("#buyNow,#addToCart").show();//显示,因为有可能已经隐藏了。
        $('.yListr .closered').hide();
        return false;
    }

    //库存不足
    if ($(btn).hasClass('noStock')) {
        return false;
    }

    //登录判断
    //if (checkUserLogin()) {
    //    $("#buyNow,#addToCart").unbind();
    //    $("#buyNow,#addToCart").attr("href", "javascript:void(dialogLogin_click())");
    //}


    if (sku_attribute_count > 0 && sku_attribute_count > selectCount) {
        $("div.yListr").removeClass("attention").addClass("attention");
        $("#buyNow,#addToCart").addClass("action");
        $("#buyNow,#addToCart").hide();//隐藏掉购买按钮 action
        $('.yListr .closered').show();
        check_total_stock();//总库存为0 时，提示库存不足

        return false;
    } else if (sku_attribute_count == 0) {
        //没有属性处理。。。
        SKU_ProducId = parseInt($("#defualtSKU").val());
        if (SKU_ProducId < 0) {
            SKU_ProducId = 0;
            //alert("库存异常无法下单");
            mallbox.alert({ message: $commonLang.HOME_SHOPPING_EXCEPTION });
            return false;
        } else {
            //处理sku到立即购买的按钮
            var buyUrl = $("#buyNow").attr("href");
            $("#buyNow").attr("href", buyUrl.replace(/skunumber=[\w- ]*&/, 'skunumber=' + SKU_ProducId + '&'));
        }
    }


    return true;
}

//提交购物
function addCart(event) {
    //var src = $("#img_" + (event.srcElement ? event.srcElement : event.target).name).attr("src");
    //var cart = $('#shoppingCart').offset(), flyer = $("<img class='u-flyer' src='" + src + "'/>");
    var strProductId = (event.srcElement ? event.srcElement : event.target).name;
    var productId = Number(strProductId);
    //获取购买数量
    var buyCount = parseInt($("input.tb-text").val());
    buyCount = isNaN(buyCount) ? 1 : buyCount;
    if (buyCount <= 0) {
        //alert("请添加购买数量！"); 
        mallbox.alert({ message: $commonLang.HOME_SHOPPING_BUYCOUNT });
        return false;
    }
    var ret = new MyShoppingCartBll().addToShoppingCart(productId, SKU_ProducId, buyCount, sku_attribute_select_price);//添加
    if (!ret)
        return false;

    var offset = $("#end").offset();
    var addcar = $(this);
    var img = $("#img_" + (event.srcElement ? event.srcElement : event.target).name).attr("src");
    var flyer = $('<img class="u-flyer" src="' + img + '">');
    flyer.fly({
        start: {
            left: event.pageX, //开始位置（必填）#fly元素会被设置成position: fixed
            top: event.pageY //开始位置（必填）
        },
        end: {
            left: offset.left + 10, //结束位置（必填）
            top: offset.top + 10, //结束位置（必填）
            width: 0, //结束时宽度
            height: 0 //结束时高度
        },
        onEnd: function () { //结束回调
            refreshCart();//刷新
        }
    });


    //flyer.fly({
    //    start: {
    //        left: event.clientX,
    //        top: event.clientY - 50
    //    },
    //    end: {
    //        left: cart.left,
    //        top: cart.top - (document.documentElement.scrollTop || document.body.scrollTop || 0),
    //        width: 20,
    //        height: 20
    //    },
    //    onEnd: function () { //结束回调
    //        $("#msg").hide();
    //        this.destory(); //移除dom
    //        jQuery("div", $('#shoppingCart')).css("right", 72).show().animate({ right: 42 });
    //        if ($('#shoppingCart').is("._st")) {
    //            $('#shoppingCart').addClass("hover");
    //        }
    //        refreshCart();//刷新
    //    }
    //});

}


//==============爆款处理================
var isExplosion = parseInt($("#intputpromotionProduct").val());

var leftsecond = parseInt($("#intputpromotionProduct").attr("datatime"));
var objTimer = null;
function ShowCountDown() {
    if (leftsecond <= 0) {
        $("#CountdownTime").hide();
        if (objTimer != null) {
            window.clearInterval(objTimer);
        }
        location.reload();
    }
    else {
        var hour = Math.floor((leftsecond) / 3600);
        var minute = Math.floor((leftsecond - hour * 3600) / 60);
        var second = Math.floor(leftsecond - hour * 3600 - minute * 60);
        var txtTime = (hour < 10 ? '0' + hour : hour) + "&nbsp;:&nbsp;" + (minute < 10 ? '0' + minute : minute) + "&nbsp;:&nbsp;" + (second < 10 ? '0' + second : second);
        $("#CountdownTime time").html(txtTime);
        //if (leftsecond <= 10)//最后10秒,提示。
        //{//提示:当前商品爆款活动结束后将恢复正常价格。
        //    $("div.fc8.text-midd.HK-detaj.clearfix div.js_bubble").show();
        //}
    }
    leftsecond--;
}

if (isExplosion > 0) {
    $("#CountdownTime").show();
    objTimer = window.setInterval(function () { ShowCountDown(); }, 1000);
}
//===================爆款处理===================end.

String.prototype.trimStart = function (trimStr) {
    if (!trimStr) { return this; }
    var temp = this;
    while (true) {
        if (temp.substr(0, trimStr.length) != trimStr) {
            break;
        }
        temp = temp.substr(trimStr.length);
    }
    return temp;
};
String.prototype.trimEnd = function (trimStr) {
    if (!trimStr) { return this; }
    var temp = this;
    while (true) {
        if (temp.substr(temp.length - trimStr.length, trimStr.length) != trimStr) {
            break;
        }
        temp = temp.substr(0, temp.length - trimStr.length);
    }
    return temp;
};
String.prototype.trim = function (trimStr) {
    var temp = trimStr;
    if (!trimStr) { temp = " "; }
    return this.trimStart(temp).trimEnd(temp);
};