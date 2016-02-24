//new CookieShopingCartHandleInterface().addToShoppingCart([520, 222]);

//$.cookie("hksj.hkmall.myshoppingcart.allgoods");
// 添加商品至购物车
//new MyShoppingCartBll().addToShoppingCart([520, 222]);
//new MyShoppingCartBll().addToShoppingCart([10084072,10082612,10082797,10083168,10083183,10019253, 10019479, "10082168", "10082213"]);
//new MyShoppingCartBll().addToShoppingCart([10100130],['1'],[2]);
//new MyShoppingCartBll().getGoodsCount()
function MyShoppingCartBll(option) {
    // 获取购物车操作接口:登录时为DB,未登录时为Cookie
    function getShoppingCartHandleInterface() {
        // 是否直接购买
        if (option && myCommonHelper.stringEqualIgnoreCase(option.IsOutrightPurchase, 1) == true) {
            var strGoodsId = myCommonHelper.getQueryStringByName("productid");
            var strSkuNumber = myCommonHelper.getQueryStringByName("skunumber");//SKU_ProducId
            var strCount = myCommonHelper.getQueryStringByName("count");
            strCount = strCount == undefined ? 1 : strCount == null ? 1 : strCount == "" ? 1 : strCount;
            if (myCommonHelper.isNullOrEmpty(strGoodsId) == false) {
                return new OutrightPurchaseShoppingCartHandleInterface(strGoodsId, strSkuNumber, strCount);
            }
        }

        var curLoginUser = myCommonHelper.getCur_YH_User();
        if (curLoginUser != null) {
            return new DbShoppingCartHandleInterface();
        } else {
            return new CookieShopingCartHandleInterface();
        }
    }

    // 获取以商家分组的商品信息
    this.getComWithGoodsData = function () {
        var rslt = getShoppingCartHandleInterface().getGoods();
        if (rslt == null) return arrCom;
        else return rslt;
        return arrCom;
    }

    // 添加商品
    this.addToShoppingCart = function (arrGoodsId, arrSkuNumber, arrCount, arrPrice) {
        return getShoppingCartHandleInterface().addToShoppingCart(arrGoodsId, arrSkuNumber, arrCount, arrPrice);
    }

    // 删除商品
    this.deleteGoodsInShoppingCart = function (arrCartsIdToDel) {
        getShoppingCartHandleInterface().deleteGoodsInShoppingCart(arrCartsIdToDel);
    }

    // 更新指定商品的数量,参数:[{ GoodsId: curGoodsId, Count: curCount }]
    this.updateGoodsCount = function (arrGoodsCount) {
        getShoppingCartHandleInterface().updateGoodsCount(arrGoodsCount);
    }

    // 更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态
    this.updateGoodsChecked = function (arrCartsId) {
        getShoppingCartHandleInterface().updateGoodsChecked(arrCartsId);
    };

    // 获取购物车中商品总数量
    this.getGoodsCount = function () {
        return getShoppingCartHandleInterface().getGoodsCount();
    };
}

function ShoppingCartHandleInterface() {

    // 添加商品
    function addToShoppingCart(arrGoodsId) { }

    // 删除商品
    function deleteGoodsInShoppingCart(arrCartsIdToDel) { }
}

// 数据库购物车操作
function DbShoppingCartHandleInterface() {
    // 添加商品 只有一个的时候
    this.addToShoppingCart = function (arrGoodsId, arrSkuNumber, arrCount, arrPrice) {
        var ret_json = jQuery.parseJSON(myCommonHelper.getAjaxStringSync("/money/ShoppingCart/addToShoppingCart", "POST", { strGoodsIds: myCommonHelper.jsonObjToString(arrGoodsId), strSkuNumber: myCommonHelper.jsonObjToString(arrSkuNumber), strCount: myCommonHelper.jsonObjToString(arrCount) }));
        if (ret_json.Msg != "") {
            //alert(ret_json.Msg);
            mallbox.alert({ message: ret_json.Msg });
            return false;
        }
        return true;
    }

    // 删除商品
    this.deleteGoodsInShoppingCart = function (arrCartsIdToDel) {
        myCommonHelper.getAjaxStringSync("/money/ShoppingCart/deleteGoodsInShoppingCart", "POST", { strCartsIds: myCommonHelper.jsonObjToString(arrCartsIdToDel) });
    }

    // 获取以商家分组的商品信息
    this.getGoods = function () {
        return getGoodsGroupByCom_Private();
    }

    // 获取以商家分组的商品信息
    function getGoodsGroupByCom_Private(o) {
        if (o != 0) {
            var rslt = myCommonHelper.getAjaxStringSync("/money/ShoppingCart/getGoodsGroupByCom", "POST", { getChecked: window.getChecked || 0, userid: 1 });
            return myCommonHelper.asToJsonObj(rslt) || [];
        }
        else {
            var rslt = myCommonHelper.getAjaxStringSync("/money/ShoppingCart/getGoodsGroupByCom", "POST", { getChecked: 0 });
            return myCommonHelper.asToJsonObj(rslt) || [];
        }
    }

    // 更新指定商品的数量,参数:[{ GoodsId: curGoodsId,SkuNumber:Number Count: curCount }]
    this.updateGoodsCount = function (arrGoodsCount) {
        myCommonHelper.getAjaxStringSync("/money/ShoppingCart/updateGoodsCount", "POST", { arrGoodsCountString: myCommonHelper.jsonObjToString(arrGoodsCount) });
    };

    // 更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态
    this.updateGoodsChecked = function (arrCartsId) {
        myCommonHelper.getAjaxStringSync("/money/ShoppingCart/updateGoodsChecked", "POST", { strCartsIds: myCommonHelper.jsonObjToString(arrCartsId) });
    };

    // 获取购物车中商品总数量、总金额
    this.getGoodsCount = function () {
        var arrFromDb = getGoodsGroupByCom_Private(0);
        if (arrFromDb == null) return 0;
        var iTotalCount = 0;
        var totalAmount = 0.0;
        for (var i = 0; i < arrFromDb.length; i++) {
            //iTotalCount += parseInt(arrFromDb[i].Goods.length);
            for (var j = 0; j < arrFromDb[i].Goods.length; j++) {
                var floatValue = parseFloat(arrFromDb[i].Goods[j]["GoodsUnits"]);
                var count = arrFromDb[i].Goods[j]["Count"];
                iTotalCount += parseInt(count);
                totalAmount = totalAmount + floatValue * count;
            }

        }
        return { iTotalCount: iTotalCount, amount: totalAmount.toFixed(2) };
    };
}


// Cookie购物车操作
function CookieShopingCartHandleInterface() {
    // cookie 名称
    var strCookieName = "8hkhk.com.myshoppingcart.allgoods";
    this.getAllGoodsArrayFromCookie = getAllGoodsArrayFromCookie_Private;
    function getAllGoodsArrayFromCookie_Private() {
        var redata = $.cookie(strCookieName);
        return redata;
    }

    // 清空购物车
    this.clearShoppingCart = function () {
        $.removeCookie(strCookieName, { path: '/' });
    };
    // 购物车添加商品項
    this.addShoppingCartItem = function (jsondata, addToShoppingArray) {
        for (var i = 0; i < addToShoppingArray.length; i++) {
            if (addToShoppingArray[i]["GoodsId"] == jsondata.GoodsId && addToShoppingArray[i]["SkuNumber"] == jsondata.SkuNumber) {
                return;
            }
        }
        var obj = new Object();
        obj["GoodsId"] = jsondata.GoodsId;
        obj["SkuNumber"] = jsondata.SkuNumber;
        obj["Count"] = jsondata.Count;
        obj["Price"] = jsondata.Price;
        addToShoppingArray.push(obj);
    };


    // 添加商品
    this.addToShoppingCart = function (arrGoodsId, arrSkuNumber, arrCount, arrPrice) {
        //window.location = '/Login/Index?ReturnUrl=' + encodeURIComponent(window.location.href);
        var cartItemJson = { GoodsId: arrGoodsId, SkuNumber: arrSkuNumber, Count: arrCount, Price: arrPrice };
        var cartItem = JSON.stringify(cartItemJson);
        var addToShoppingArray = new Array();
        var getGoodsCount = this.getGoodsCount();
        if (getGoodsCount.iTotalCount == 0) {    //購物車cookie不存在
            this.addShoppingCartItem(cartItemJson, addToShoppingArray);
            $.cookie(strCookieName, JSON.stringify(addToShoppingArray), { expires: 1, path: '/' });
            return true;
        }

        var allgoodsStr = this.getAllGoodsArrayFromCookie();
        var allgoods = JSON.parse(allgoodsStr);

        //cookie購物存在商品
        var addflag = true;
        for (var i = 0; i < allgoods.length; i++) {
            if (allgoods[i]["GoodsId"] == arrGoodsId && allgoods[i]["SkuNumber"] == arrSkuNumber) {
                addflag = false;
                break;
            }
        }
        if (!addflag) {
            for (var i = 0; i < allgoods.length; i++) {
                if (allgoods[i]["GoodsId"] == arrGoodsId && allgoods[i]["SkuNumber"] == arrSkuNumber) {
                    allgoods[i]["Count"] = parseInt(allgoods[i]["Count"]) + parseInt(arrCount);
                    $.cookie(strCookieName, JSON.stringify(allgoods), { expires: 1, path: '/' });
                }
            }
        }
        else {
            this.addShoppingCartItem(cartItemJson, addToShoppingArray);
        }

        if (addToShoppingArray.length > 0) {
            for (var i = 0; i < addToShoppingArray.length; i++) {
                allgoods.push(addToShoppingArray[i]);
            }
            $.cookie(strCookieName, JSON.stringify(allgoods), { expires: 1, path: '/' });
        }
        return true;
    }

    // 删除商品
    this.deleteGoodsInShoppingCart = function (arrGoodsId) {
        var getGoodsCount = this.getGoodsCount();
        if (getGoodsCount == 0) {    //購物車cookie不存在         
            return false;
        }
        var allgoods = this.getGoods()[0].Goods;
        var addToShoppingArray = new Array();

        for (var i = 0; i < allgoods.length; i++) {
            var deladdflag = true;
            for (var j = 0; j < arrGoodsId.length; j++) {
                if (i == arrGoodsId[j]) {
                    deladdflag = false;
                    break;
                }
            }
            if (deladdflag) {
                this.addShoppingCartItem(allgoods[i], addToShoppingArray);
            }
        }
        $.cookie(strCookieName, JSON.stringify(addToShoppingArray), { expires: 1, path: '/' });
        //页面数据展示
        var myMyShoppingCart_Presenter = new MyShoppingCartPresenter($("body:first"));
        myMyShoppingCart_Presenter.pageLoad();
    }

    // 获取以商家分组的商品信息
    this.getGoods = function () {
        return new ShoppingCartHandleHelper().groupbyCom(getAllGoodsArrayFromCookie_Private());
    }

    // 更新指定商品的数量,参数:[{ GoodsId: curGoodsId, Count: curCount }]
    this.updateGoodsCount = function (arrGoodsCount, arrSkuNumber) {
    };


    // 更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态
    this.updateGoodsChecked = function (arrGoodsId, arrSkuNumber) {
    };

    // 获取购物车中商品总数量
    this.getGoodsCount = function () {
        var allgoods = this.getAllGoodsArrayFromCookie();
        if (allgoods == undefined || JSON.parse(allgoods).length <= 0) {
            return { iTotalCount: 0, amount: 0.00 };
        }
        var iTotalCount = 0;
        var totalAmount = 0.0;
        var allgoodsJson = JSON.parse(allgoods);
        $(allgoodsJson).each(function (i, item) {
            iTotalCount += parseInt(item["Count"]);
            totalAmount += parseInt(item["Count"]) * parseFloat(item["Price"]);
        });
        return { iTotalCount: iTotalCount, amount: totalAmount.toFixed(2) };
    };
}

// 直接购买时的“购物车”操作
function OutrightPurchaseShoppingCartHandleInterface(strGoodsId, SkuNumbers, Counts) {
    // 添加商品
    this.addToShoppingCart = function (arrGoodsId) {

    }

    // 删除商品
    this.deleteGoodsInShoppingCart = function (arrGoodsId) {

    }

    // 获取以商家分组的商品信息
    this.getGoods = function () {
        //myCommonHelper.getQueryStringByName("GoodsId")
        return new ShoppingCartHandleHelper().groupbyCom([{ GoodsId: strGoodsId, SkuNumber: SkuNumbers, Count: Counts }]);
    }

    // 获取以商家分组的商品信息
    function getGoodsGroupByCom_Private() {
        var rslt = myCommonHelper.getAjaxStringSync("/money/ShoppingCart/getGoodsGroupByCom", "POST", { getChecked: window.getChecked || 0 });
        return myCommonHelper.asToJsonObj(rslt) || [];
    }

    // 更新指定商品的数量,参数:[{ GoodsId: curGoodsId, Count: curCount }]
    this.updateGoodsCount = function (arrGoodsCount) {

    };

    // 更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态
    this.updateGoodsChecked = function (arrCartsId) {

    };

    // 获取购物车中商品总数量
    this.getGoodsCount = function () {
        return 0;
    };
}


// Helper类
//这个方法应该是以前没有登录的情况下也可以进来查看商品 后面改成不登录不可查看商品详细 Ryan 2015/06/01
function ShoppingCartHandleHelper() {

    // 将商品数组转换为以商家分组后的数组；cookie/db中只存商品id,需要从后台取商品其它信息
    this.groupbyCom = function (arrGoodsFromShoppingCart) {
        var arrGoodsFromShoppingCartObj = JSON.parse(arrGoodsFromShoppingCart);
        // 从服务器获取商品其它信息
        var arrGoodsInfo = getGoodsInfoAjax(arrGoodsFromShoppingCartObj).Data;

        // 将购物车中的属性补充进去        
        for (var i = 0; i < arrGoodsInfo.length; i++) {
            var curGoods = arrGoodsInfo[i];
            if (curGoods.Goods.length > 0) {
                $(curGoods.Goods).each(function (index, item) {
                    var arrInCart = $.grep(arrGoodsFromShoppingCartObj, function (mem, idx) {
                        return myCommonHelper.stringEqualIgnoreCase(item.GoodsId, mem.GoodsId) == true
                            && myCommonHelper.stringEqualIgnoreCase(item.SkuNumber, mem.SkuNumber) == true;
                    });
                    if (arrInCart.length > 0) {
                        item.CartsId = [index];
                        item.AddToShoppingCartTime = arrInCart[0].AddToShoppingCartTime;
                        item.Count = arrInCart[0].Count;
                        item.SkuNumber = arrInCart[0].SkuNumber;
                        item.IsChecked = arrInCart[0].IsChecked;
                        item.Price = arrInCart[0].Price;
                    }
                });

            }
        }

        return arrGoodsInfo;
    };

    function getGoodsInfoAjax(arrGoods) {

        var arrGoodsId = [];
        var arrSkuNumber = [];
        $(arrGoods).each(function (idx, mem) { arrGoodsId.push(mem.GoodsId); arrSkuNumber.push(mem.SkuNumber) });
        if (arrGoodsId.length == 0) return [];
        var rslt = $.ajax("/home/GetShoppingCarProductList", { type: "POST", async: false, data: { productIds: arrGoodsId.join(","), productSkuIds: arrSkuNumber.join(",") } });

        var ret_json = jQuery.parseJSON(rslt.responseText);
        if (ret_json.Message != "") {
            alert({ message: ret_json.Message });
            history.go(-1);
        }
        return myCommonHelper.asToJsonObj(rslt.responseText);
    }

    // 合并两个数组,相同商品处理逻辑:添加时间取最近,数量相加,IsChecked做OR操作
    function combineShoppingCartItem(arrShoppingCartItem1, arrShoppingCartItem) { }
}

