// MyShoppingCart.cshtml 页面与UI交互相关的逻辑
// 购物车和提交订单页面可共用此逻辑
function MyShoppingCartPresenter(jBodyContainer, option) {
    //var option = {
    //    IsOutrightPurchase: 1   // 是否直接购买,此时通过 QueryString 中的 GoodsId 显示商品信息
    //};

    var myShoppingCartBll = new MyShoppingCartBll(option);
    var jDom_divCartList = jBodyContainer.find("[member='divCartList']");
    var btn_CheckAllGoods = jBodyContainer.find("[member='CheckAllGoods']");

    // 页面加载
    this.pageLoad = function () {
        //页面数据展示
        setNodeValue();
        // 打开订单页面
        jBodyContainer.find("#btnGoToOrder").click(function () {

            if (myCommonHelper.getCur_YH_User() == null) {
                ($(this).hasClass("clicked")) ? removeClick($(this)) : dialogLogin_click($(this));
                return false;
            }


            var arrCheckedTr = getCheckedTr();
            if (arrCheckedTr != null && arrCheckedTr.length > 0) {
                var tag = false;
                var productType = false;
                $(arrCheckedTr).each(function () {
                    var value = parseInt($(this).find("[member=GoodsCount]").val());
                    var inventory = parseInt($.trim($(this).find("[member=StockQuantity]").html()));
                    if (value > inventory) {
                        tag = true;
                        $(this).find(".Pay-commt").css('display', 'block');
                    }
                    if (value == 0) {
                        productType = true;
                        return false;
                    }
                });
                if (tag) {
                    //alert("商品数量不能大于库存,请修改商品数量"); 
                    //mallbox.alert({ message: $commonLang.HOME_SHOPPINGCART_EDITPRODUCTCOUNT, modal: true });
                    var diaHtml = $(".dialogCover").html();
                    $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_EDITPRODUCTCOUNT);
                    ds.dialog({
                        content: diaHtml,
                        yesText: $commonLang.ORDER_LIST_SURE,
                        onyes: function () {
                            this.close();
                        }
                    });

                    return false;
                }
                if (productType) {
                    //mallbox.alert({ message: $commonLang.HOME_SHOPPINGCART_NOPRODUCT, modal: true });
                    var diaHtml = $(".dialogCover").html();
                    $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_NOPRODUCT);
                    ds.dialog({
                        content: diaHtml,
                        yesText: $commonLang.ORDER_LIST_SURE,
                        onyes: function () {
                            this.close();
                        }
                    });
                    return false;
                }
                var curLoginUser = myCommonHelper.getCur_YH_User();
                if (curLoginUser == null) {
                    location.href = "/money/OrderProcess/ShoppingOrder";//OrderSubmitView 未登录时
                }
                else {
                    location.href = "/money/Order/OrderInfo";
                }
            } else {
                //alert("请至少选择一个商品！");
                //alert($commonLang.HOME_SHOPPINGCART_ATLEASTONEPRODUCT);
                // mallbox.alert({ message: $commonLang.HOME_SHOPPINGCART_ATLEASTONEPRODUCT, modal: true });

                var diaHtml = $(".dialogCover").html();
                $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_ATLEASTONEPRODUCT);
                ds.dialog({
                    content: diaHtml,
                    yesText: $commonLang.ORDER_LIST_SURE,
                    onyes: function () {
                        this.close();
                    }
                });
            }
        });

        isNullDelCheckedItem();
    }


    function isNullDelCheckedItem() {
        if ($("[member=divCartList]").find("[member='trCartItem']").length > 0) {
            $("[member=DelCheckedItem]").show();
        } else {
            $("[member=DelCheckedItem]").hide();
            var nogoods = "<div class='l_main_a'><div class='l_shoppingCart_n'><p>" + $commonLang.HOME_SHOPPINGCART_CARTISNULL + "</p><br/><a class='l_shoppingCart_g' href='/'>" + $commonLang.HOME_SHOPPINGCART_GOSHOPPING + ">></a></div></div>";
            jBodyContainer.find("div.z_nav_095").after(nogoods);
            jBodyContainer.find("div.z_shopmain_095").remove();
        }
    }



    // 将数据绑定至UI
    function setNodeValue() {
        jDom_divCartList.html("");
        //获取数据
        var arrCom = myShoppingCartBll.getComWithGoodsData();

        // 找到代表一个商家所有商品的模板
        var jDom_divCartItemTb_Template = jBodyContainer.find("[member='divCartItemTb_Template']");
        var comHtml = jDom_divCartItemTb_Template.html();
        var isCakes = false;
        if (arrCom.length > 0) {
            jBodyContainer.find("#btnGoToOrder").removeClass("disabled");
        } else {
            //jBodyContainer.find("#btnGoToOrder").addClass("disabled");
            //var nogoods = "<div class='l_main_a'><div class='l_shoppingCart_n'><p>" + $commonLang.HOME_SHOPPINGCART_CARTISNULL + "</p><br/><a class='l_shoppingCart_g' href='/'>" + $commonLang.HOME_SHOPPINGCART_GOSHOPPING + ">></a></div></div>";
            //jBodyContainer.find("div.z_nav_095").after(nogoods);
            //jBodyContainer.find("div.z_shopmain_095").remove();
        }

        if (arrCom.length <= 0) {
            $(".specCart").hide();
            $(".m_pay_flase").show();
        }
        else {
            $(".m_pay_flase").hide();
        }


        for (var i = 0; i < arrCom.length; i++) {
            var curCom = arrCom[i];

            var jCom = jQuery(comHtml);
            // 设置商户名称
            jCom.find("[member='ComName']").text(curCom.ComName);
            // 设置商户链接
            //jCom.find("[member='ComHref']").attr("href", "/Home/sellerindex?sellerkey=" + curCom.ComId);
            jCom.find("[member='ComId']").val(curCom.ComId);

            // 找到代表一个商品的TR模板
            var itemHtml = jCom.find("[class='cartMain']:first").parent().html();
            jCom.find("[class='cartMain']").remove();

            var blnShowInvoice = false;
            for (var j = 0; j < curCom.Goods.length; j++) {

                var goodsInfo = curCom.Goods[j];
                // 每一个商品的TR
                var jGoodsMemInCom_Template = jQuery(itemHtml);
                jGoodsMemInCom_Template.find("[member='GoodsId']").val(goodsInfo.GoodsId);
                jGoodsMemInCom_Template.find("[member='CartsId']").val(goodsInfo.CartsId);
                var ProductName = "";
                if (goodsInfo.GoodsName.length > 38) {
                    ProductName = goodsInfo.GoodsName.substring(0, 19) + "<br/>" + goodsInfo.GoodsName.substring(19, 38) + "...";
                }
                else if (goodsInfo.GoodsName.length > 19) {
                    ProductName = goodsInfo.GoodsName.substring(0, 19) + "<br/>" + goodsInfo.GoodsName.substring(19, goodsInfo.GoodsName.length);
                }
                else
                    ProductName = goodsInfo.GoodsName;
                jGoodsMemInCom_Template.find("[member='GoodsName']").html(ProductName);
                jGoodsMemInCom_Template.find("[member='GoodsUnits']").text(goodsInfo.GoodsUnits.toFixed(2));
                jGoodsMemInCom_Template.find("[member='MarketPrice']").text(goodsInfo.MarketPrice.toFixed(2));
                jGoodsMemInCom_Template.find("[member='GoodsCount']").val(goodsInfo.Count);
                jGoodsMemInCom_Template.find("[member='ShowGoodsCount']").html(goodsInfo.Count);
                jGoodsMemInCom_Template.find("[member='GoodsTotalPrice']").text((goodsInfo.GoodsUnits * goodsInfo.Count).toFixed(2));
                jGoodsMemInCom_Template.find("[member='Pic']").attr("src", goodsInfo.Pic);
                jGoodsMemInCom_Template.find("[member='GoodsHref']").attr("href", "/Home/shopping/" + goodsInfo.GoodsId + ".html");
                jGoodsMemInCom_Template.find("[member='StockQuantity']").text(goodsInfo.StockQuantity);    // 库存
                jGoodsMemInCom_Template.find("[member='PostagePrice']").text(0);    // 邮费

                //订单页,特殊处理
                var stockError = jGoodsMemInCom_Template.find("[member='StockQuantity-msg']");    // 库存
                if (parseInt(goodsInfo.StockQuantity) < parseInt(goodsInfo.Count)) {
                    stockError.html($commonLang.HOME_SHOPPINGCART_NOTENOUGH).show();//'库存不足'
                } else if (parseInt(goodsInfo.Status) == 5) {
                    stockError.html($commonLang.HOME_SHOPPINGCART_PRODUCTUNDERSHELVES).show();//'商品已下架'
                } else {
                    stockError.hide();
                }

                if (goodsInfo.IsCakes == 1) {
                    isCakes = true;
                }

                var skuNumberValue = goodsInfo.ValueStr == null ? "" : goodsInfo.ValueStr.split(",");
                var skuValue = "";

                for (var k = 0; k < skuNumberValue.length; k++) {
                    skuValue += "<dd>" + skuNumberValue[k] + "</dd>";
                }
                jGoodsMemInCom_Template.find("[member='Sku']").html(skuValue);    // 显示Sku
                jGoodsMemInCom_Template.find("[member='SkuNumber']").html(goodsInfo.SkuNumber);    // SkuNumber


                if (goodsInfo.Count <= 1) {
                    // 隐藏减小按钮
                    var btnDecreaseItem = jGoodsMemInCom_Template.find("[member='DecreaseItem']");
                    btnDecreaseItem.addClass("tb-disable-reduce");
                }

                if (goodsInfo.Status != 4 || goodsInfo.StockQuantity <= 0) {
                    // 设置失效商品的显示效果
                    setGoodsDisabled(jGoodsMemInCom_Template, goodsInfo);
                }

                // 添加 商品TR
                //if (j > 0) jCom.find("[member='trCartItem']:last").after(jGoodsMemInCom_Template);
                //else {
                //    var jHeader = jCom.find("[member='divCartItemTb_Header']:last");
                //    if (jHeader.length > 0) jHeader.after(jGoodsMemInCom_Template);
                //    else {
                jCom.append(jGoodsMemInCom_Template);
                //    }
                //}


                if (myCommonHelper.stringEqualIgnoreCase(goodsInfo.IsDisabled, "1") == false && myCommonHelper.stringEqualIgnoreCase(goodsInfo.IsProvideInvoices, 1) == true) {
                    blnShowInvoice = true;
                }

            }

            //爆款提示语显示
            if (isCakes) {
                $("#divScrollBar_Content").find("tr:eq(2)").show();
            }

            // 任一商品可提供发票即显示发票控件
            //if (blnShowInvoice == true) {
            var j_IsProvideInvoices = jCom.find("[member='IsProvideInvoices']");
            j_IsProvideInvoices.show();
            var j_hreftoggleInvoiceContent = j_IsProvideInvoices.find("[member='hreftoggleInvoiceContent']");
            j_hreftoggleInvoiceContent.unbind("click");
            j_hreftoggleInvoiceContent.click(function () {
                $(this).parent().find("[member='spanInvoicesContent']").toggle();
            });
            //}
            //else {
            if (!blnShowInvoice) {
                //jCom.find("[member='IsProvideInvoices']").hide();
                j_IsProvideInvoices.find("[member='hreftoggleInvoiceContent']").hide();
                j_IsProvideInvoices.find("[member='spanInvoicesContent']").show();
                jCom.find("[member='IsProvideInvoices']").find(".invoices1").hide();
                jCom.find("[member='IsProvideInvoices']").find(".invoices2").show();
            };



            // 添加商家 TB
            jDom_divCartList.append(jCom);

            addEvent(jCom);
            $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_DELETEGOOD);

            // 删除选中的商品           
            jBodyContainer.find("[member='DelCheckedItem']").unbind("click");
            jBodyContainer.find("[member='DelCheckedItem']").click(function () {

                var diaHtml = $(".dialogCover").html();
                $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_DELETEGOOD)
                ds.dialog({
                    //title : '消息提示',
                    content: diaHtml,
                    yesText: $commonLang.ORDER_LIST_SURE,
                    onyes: function () {
                        // 找到选中的TR
                        var arrCheckedTr = getCheckedTr();
                        if (arrCheckedTr.length == 0) return;
                        // 删除TR
                        var arrCartsId_ToDel = [];
                        var arrGoodsId_ToDel = [];
                        var arrSkuNumber_ToDel = [];
                        $(arrCheckedTr).each(function (idx, mem) {
                            arrCartsId_ToDel.push($(mem).find("[member='CartsId']").val());
                            arrSkuNumber_ToDel.push($(mem).find("[member='SkuNumber']").text());
                            $(mem).remove();
                        });

                        // 删除购物车
                        myShoppingCartBll.deleteGoodsInShoppingCart(arrCartsId_ToDel);

                        // 删除没有商品的商家Table
                        jDom_divCartList.find("[member='divCartItemTb_Template']").each(function (idx, mem) {
                            if ($(mem).find("[member='trCartItem']").length == 0) {
                                $(mem).remove();
                            }
                        });
                        // 刷新总数
                        refreshCheckedTotalCount();
                        isNullDelCheckedItem();

                        refreshCart();//刷新购物车

                        this.close();

                    },
                    noText: $commonLang.ORDER_LIST_CANCEL,
                    onno: function () {
                        //console.log("你点击了取消！");
                        this.close();
                    },
                    //icon : "question.gif",
                });

            });

            // 选择所有商品
            btn_CheckAllGoods.unbind("click");
            btn_CheckAllGoods.click(function () {
                var isSelected = $(this)[0].checked == true;
                var arrChkItem = jDom_divCartList.find("[member='ChkItem']");
                jQuery.each(arrChkItem, function (idx, mem) {
                    $(mem)[0].checked = isSelected;
                });
                $("[member=CheckAllGoods]").each(function (index, ele) {
                    //$(ele).attr("checked", _check);
                    ele.checked = isSelected;
                });
                refreshCheckedTotalCount();//更新当前选中的行数
                refreshAllCheckedInCom();//更新商家选中按钮
                updateGoodsChecked();//更新商家的商品选中
            });
        }
    }

    // 设置失效商品的显示效果
    function setGoodsDisabled(jGoodsMemInCom_Template, goodsInfo) {
        //jGoodsMemInCom_Template.find("[member='ChkItem']").remove();
        jGoodsMemInCom_Template.find("[member='GoodsName']").removeClass("blue").css("color", "rgb(215,215,215)");
        jGoodsMemInCom_Template.find("[member='GoodsUnits']").removeClass("blue").css("color", "rgb(215,215,215)");
        jGoodsMemInCom_Template.find("[member='GoodsCount']").removeClass("blue").css("color", "rgb(215,215,215)");
        jGoodsMemInCom_Template.find("[member='GoodsTotalPrice']").removeClass("blue").css("color", "rgb(215,215,215)");

        jGoodsMemInCom_Template.find("[member='DecreaseItem']").unbind("click");
        jGoodsMemInCom_Template.find("[member='DecreaseItem']").addClass("tb-disable-reduce");
        jGoodsMemInCom_Template.find("[member='IncreaseItem']").unbind("click");
        jGoodsMemInCom_Template.find("[member='IncreaseItem']").addClass("tb-disable-reduce");
        if (goodsInfo.StockQuantity <= 0) {
            jGoodsMemInCom_Template.append("<div class=\"cartDisabled\">" + $commonLang.SHOPPINGCART_WUHUO + "</div>");
        } else {
            if (goodsInfo.Status != 4) {
                jGoodsMemInCom_Template.append("<div class=\"cartDisabled\">" + $commonLang.SHOPPINGCART_YIXIAJIA + "</div>");
            }
        }
        //jGoodsMemInCom_Template.find(".z_check").remove();
        //jGoodsMemInCom_Template.append('<div class="cartDisabled"><span><p>已失效</p></span></div>');

    }

    // 更新商品的选中状态,购物车中不在入参中的商品都更新为非选中状态
    function updateGoodsChecked() {
        var arr = getAllCheckedCartsId();
        if (arr == null) arr = [];
        myShoppingCartBll.updateGoodsChecked(arr);
    }

    // 获取所有选中的商品Id
    function getAllCheckedGoodsId() {
        var arrCheckedTr = getCheckedTr();
        var arrGoodsId = [];
        $(arrCheckedTr).each(function (i, m) {
            arrGoodsId.push($(m).find("[member='GoodsId']").val());
        });
        return arrGoodsId;
    }

    // 获取所有选中的商品Id
    function getAllCheckedCartsId() {
        var arrCheckedTr = getCheckedTr();
        var arrCartsId = [];
        $(arrCheckedTr).each(function (i, m) {
            arrCartsId.push($(m).find("[member='CartsId']").val());
        });
        return arrCartsId;
    }

    // 获取所有选中的商品SkuNumber
    function getAllCheckedGoodsSkuNumber() {
        var arrCheckedTr = getCheckedTr();
        var arrSkuNumber = [];
        $(arrCheckedTr).each(function (i, m) {
            arrSkuNumber.push($(m).find("[member='SkuNumber']").text());
        });
        return arrSkuNumber;
    }


    // 刷新各个商家内部的全选按钮
    function refreshAllCheckedInCom() {
        var arr_divCartItemTb = jDom_divCartList.find("[member='divCartItemTb']");

        // 刷新各个商家内部的全选按钮
        arr_divCartItemTb.each(function (idx, tb) {
            var arrChkItem = $(tb).find("[member='ChkItem']");
            var j_SelectAllGoodsInCom = $(tb).find("[member='SelectAllGoodsInCom']");

            if (j_SelectAllGoodsInCom.length > 0) {
                if (arrChkItem.length == 0) {
                    j_SelectAllGoodsInCom.checked = false;
                }
                else {
                    j_SelectAllGoodsInCom[0].checked =
                    ($.grep(arrChkItem,
                        function (chk, i) {
                            return chk.checked == false;
                        }).length == 0);
                }
            }
        });
    }

    // 刷新全局全选按钮
    function refreshChecked() {
        // 刷新全局全选按钮 //没有商家分组 暂时注释
        /*var check = ($.grep(jDom_divCartList.find("[member='SelectAllGoodsInCom']"), function (chkAllInCom, idx) {
            return chkAllInCom.checked == false;
        }).length == 0);*/
        var check = $.grep(jDom_divCartList.find("[member='ChkItem']"), function (mem, idx) { return $(mem)[0].checked == true; }).length == jDom_divCartList.find("[member='ChkItem']").length;
        for (var i = 0 ; i < btn_CheckAllGoods.length; i++) {
            btn_CheckAllGoods[i].checked = check;
        }

    }

    // 获取选中商品所有的TR
    function getCheckedTr() {
        var arrCheckedTr = $.grep(jDom_divCartList.find(".cartMain"),
                    function (mem, idx) {
                        var jChkItem = $(mem).find("[member='ChkItem']:first");
                        return jChkItem.length > 0 && jChkItem[0].checked == true;
                    });
        return arrCheckedTr;
    }

    // 更新选中的总数量显示
    function refreshCheckedTotalCount() {
        var arrCheckedTr = getCheckedTr();
        var iTotalCount = 0;
        var iTotalPrice = 0.00;
        var PostagePrice = 0.00;
        // 找到每一行的数量相加
        $(arrCheckedTr).each(function (idx, mem) {
            var curCount = parseInt($(mem).find("[member='GoodsCount']").val());
            var curGoodsTotalPrice = parseFloat($(mem).find("[member='GoodsTotalPrice']").text());
            var Postage = $(mem).find("[member='PostagePrice']").text();
            var curGoodsTotalPostagePrice = parseFloat(Postage == $commonLang.HOME_SHOPPINGCART_POSTAGE ? "0" : Postage == "" ? "0" : Postage);//"包邮"
            iTotalCount += curCount;
            iTotalPrice = addNum(iTotalPrice, curGoodsTotalPrice);
            PostagePrice = addNum(PostagePrice, curGoodsTotalPostagePrice);
        });
        //jBodyContainer.find("[member='AllGoodsCount']").text(iTotalCount);
        jBodyContainer.find("[member='AllGoodsCount']").text(jDom_divCartList.find("input[member='ChkItem']:checked").length);
        jBodyContainer.find("[member='AllGoodsPrice']").text(iTotalPrice.toFixed(2));
        jBodyContainer.find("[member='AllGoodsPostagePrice']").text(PostagePrice.toFixed(2));
        if (jBodyContainer.find("[member='AllGoodsPrice']").length > 1)
            jBodyContainer.find("[member='AllGoodsPrice']").eq(1).text(addNum(iTotalPrice, PostagePrice).toFixed(2));
        jBodyContainer.find("[member='AllGoodsCost']").each(function (i, dom) {
            $(dom).html(addNum(iTotalPrice, PostagePrice).toFixed(2));
        });

    }

    //自定义加法运算
    function addNum(num1, num2) {
        var sq1, sq2, m;
        try {
            sq1 = num1.toString().split(".")[1].length;
        }
        catch (e) {
            sq1 = 0;
        }
        try {
            sq2 = num2.toString().split(".")[1].length;
        }
        catch (e) {
            sq2 = 0;
        }
        m = Math.pow(10, Math.max(sq1, sq2));
        return (num1 * m + num2 * m) / m;
    }

    // 为一个商家区域添加页面交互事件
    function addEvent(jDom_Com) {
        // 全选按钮
        var jChk_SelectAllGoodsInCom = $(".cartTitle").find("[member='SelectAllGoodsInCom']"); //jDom_Com.find("[member='SelectAllGoodsInCom']");

        // 当前商家下所有商品复选按钮
        var arrChkItem = jDom_Com.find("[member='ChkItem']");

        // 点击全选当前商家的所有商品
        jChk_SelectAllGoodsInCom.click(function (args) {
            var isSelected = $(this)[0].checked == true;
            jQuery.each(arrChkItem, function (idx, mem) {
                $(mem)[0].checked = isSelected;
            });

            refreshCheckedTotalCount();
            refreshChecked();
            updateGoodsChecked();
        });

        // 点击一个商品前的复选框
        arrChkItem.click(function () {
            // 判断是否全选
            //var isAllChcked = jQuery.grep(arrChkItem, function (mem, idx) { return $(mem)[0].checked == true; }).length == arrChkItem.length;
            //jChk_SelectAllGoodsInCom[0].checked = isAllChcked; //没有商家分组 暂时注释 
            refreshCheckedTotalCount();
            refreshChecked();
            updateGoodsChecked();
        });

        // 所有商品TR
        var arrCartItem = jDom_Com.find(".cartMain");
        arrCartItem.each(function (idx, mem) {

            var curCartsId = $(mem).find("[member='CartsId']").val();
            // 当前行的商品Id
            var curGoodsId = $(mem).find("[member='GoodsId']").val();

            var curSkuNumber = $(mem).find("[member='SkuNumber']").text();
            // 数量
            var jDom_CurGoodsCount = $(mem).find("[member='GoodsCount']");
            // 总价
            var jDom_CurGoodsTotalPrice = $(mem).find("[member='GoodsTotalPrice']");
            // 单价
            var jDom_CurGoodsUnits = $(mem).find("[member='GoodsUnits']");
            // 库存
            var jDom_CurStockQuantity = $(mem).find("[member='StockQuantity']");
            var iStockQuantity = parseInt(jDom_CurStockQuantity.text());

            // 减少按钮
            var btnDecreaseItem = $(mem).find("[member='DecreaseItem']");
            btnDecreaseItem.click(function () {
                if ($(this).attr("class").indexOf("tb-disable-reduce") > -1) {
                    return;
                }

                var curCount = parseInt(jDom_CurGoodsCount.val()) - 1;
                if (curCount < 1) return;
                if (curCount == 1) {
                    $(this).addClass("tb-disable-reduce");
                }
                if ($(mem).find("[member='IncreaseItem']").hasClass("background-image")) {
                    $(mem).find("[member='IncreaseItem']").removeClass("tb-disable-reduce");
                }

                jDom_CurGoodsCount.val(curCount);
                jDom_CurGoodsTotalPrice.text((parseFloat(jDom_CurGoodsUnits.text()) * curCount).toFixed(2));
                // 更新购物车
                myShoppingCartBll.updateGoodsCount([{ GoodsId: curGoodsId, SkuNumber: curSkuNumber, Count: curCount }]);
                refreshCart();//刷新购物车
                refreshCheckedTotalCount();
                //获取运费
                getPostage();
            });

            // 增加按钮
            $(mem).find("[member='IncreaseItem']").click(function () {
                if ($(this).attr("class").indexOf("tb-disable-reduce") > -1) {
                    return;
                }
                var curCount = parseInt(jDom_CurGoodsCount.val()) + 1;
                if (curCount > iStockQuantity) {
                    $(this).addClass("tb-disable-reduce");
                    $(this).addClass("background-image");
                    //$(mem).find("[member='IncreaseItem']");
                    return;
                }

                if (curCount == 2) {
                    btnDecreaseItem.removeClass("tb-disable-reduce");
                }
                jDom_CurGoodsCount.val(curCount);
                jDom_CurGoodsTotalPrice.text((parseFloat(jDom_CurGoodsUnits.text()) * curCount).toFixed(2));
                // 更新购物车
                myShoppingCartBll.updateGoodsCount([{ GoodsId: curGoodsId, SkuNumber: curSkuNumber, Count: curCount }]);
                refreshCart();//刷新购物车
                refreshCheckedTotalCount();
                //获取运费
                getPostage();

            });

            // 删除按钮
            $(mem).find("[member='DelCurGoods']").click(function () {
                var diaHtml = $(".dialogCover").html();
                $(".dialogMain>p").text($commonLang.HOME_SHOPPINGCART_DELETEGOOD)
                ds.dialog({
                    //title : '消息提示',
                    content: diaHtml,
                    yesText: $commonLang.ORDER_LIST_SURE,
                    onyes: function () {
                        //console.log("你点击了确定！");
                        // 删除当前TR,若当前商家下已无商品,删除当前商家Table
                        $(mem).remove();
                        if (jDom_Com.find(".cartMain").length == 0) {
                            jDom_Com.remove();
                        }
                        myShoppingCartBll.deleteGoodsInShoppingCart([curCartsId]);
                        refreshCheckedTotalCount();
                        isNullDelCheckedItem();
                        refreshCart();//刷新购物车
                        //获取数据
                        var arrCom = myShoppingCartBll.getComWithGoodsData();
                        if (arrCom.length <= 0) {
                            $(".specCart").hide();
                            $(".m_pay_flase").show();
                        }
                        else {
                            $(".m_pay_flase").hide();
                        }
                        this.close();

                    },
                    noText: $commonLang.ORDER_LIST_CANCEL,
                    onno: function () {
                        //console.log("你点击了取消！");
                        this.close();
                    },
                    //icon : "question.gif",
                });
            });

            //订单商品数量文本框
            $(mem).find("[member=GoodsCount]").keyup(function () {
                if ($(this).val() == "") {
                    return;
                }
                var curCount = parseInt($(this).val());
                if (curCount > iStockQuantity) {
                    //alert("商品数量不能大于库存！");
                    //alert($commonLang.HOME_SHOPPINGCART_PRODUCTCOUNTOUTOFRANGE);
                    //mallbox.alert({ message: $commonLang.HOME_SHOPPINGCART_PRODUCTCOUNTOUTOFRANGE, modal: true });
                    curCount = iStockQuantity;//最大库存数
                    //return;
                }

                if (curCount > 1) {
                    $(this).parent().find("[member=DecreaseItem]").removeClass("tb-disable-reduce");
                    // $(mem).find("[member='IncreaseItem']").removeClass("tb-disable-reduce");
                }
                else {
                    $(this).parent().find("[member=DecreaseItem]").addClass("tb-disable-reduce");

                }
                if (curCount == 2) {
                    btnDecreaseItem.removeClass("tb-disable-reduce");
                }

                if (curCount >= iStockQuantity) {
                    $(this).parent().find("[member=IncreaseItem]").addClass("tb-disable-reduce");
                } else {

                    if ($(this).parent().find("[member='IncreaseItem']").css("background-image") != "none") {
                        $(this).parent().find("[member='IncreaseItem']").removeClass("tb-disable-reduce");
                    }
                }

                var SkuNumber = $(mem).find("[member='SkuNumber']").text();//找到最上级之后再修改   //$(this).parents("td.table-count").prevAll().find("[member='SkuNumber']").text();
                jDom_CurGoodsCount.val(curCount);
                jDom_CurGoodsTotalPrice.text((parseFloat(jDom_CurGoodsUnits.text()) * curCount).toFixed(2));
                // 更新购物车
                myShoppingCartBll.updateGoodsCount([{ GoodsId: curGoodsId, Count: curCount, SkuNumber: SkuNumber }]);
                refreshCart();//刷新购物车
                refreshCheckedTotalCount();

                //获取运费
                getPostage();
            });

            $(mem).find("[member=GoodsCount]").blur(function (e) {
                if ($(this).val() == "") {
                    $(this).val(1);
                    $(this).keyup();
                }
                else if ($(this).val() == "0" && $(this).parent().parent().find("[member=StockQuantity]")) {
                    $(this).val(1);
                    $(this).keyup();
                }

            });

            //$("[member=divCartList]").on("blur", "[member=GoodsCount]", function () {

            //});

        });
    }

    // 选择所有
    this.checkAll = function () {
        if (btn_CheckAllGoods.length === 0) return;
        btn_CheckAllGoods[0].checked = false; //火狐checkboxF5刷新会保留选中状态,为了确保所有被选中,先重置全选状态
        btn_CheckAllGoods[0].click();
    };
}

//获取当前运费 
function getPostage() {

    if ($("#HiddAreaId").length == 0)
        return;
    //当是直接购买的时候
    var strGoodsId = myCommonHelper.getQueryStringByName("productid");

    var source = strGoodsId == "" ? 0 : 1;//来源
    var product = [];

    //所有商家
    var _this = $("[member=divCartList]").find("[member=divCartItemTb]");//页面所有

    $(_this).each(function () {
        //当前商家商品
        var o = $(this).find("table").find("[member=trCartItem]");
        $(o).each(function () {
            var goods = {
                GoodsId: $(this).find("[member=GoodsId]").val(),
                SkuNumber: $(this).find("[member=SkuNumber]").text(),
                Count: $(this).find("[member=GoodsCount]").val()
            };
            product.push(goods);
        });
    });

    var areaid = $("#HiddAreaId").val();
    $.post("/money/OrderProcess/OrderPostage",
      {
          areaId: areaid,
          goodsInfor: myCommonHelper.jsonObjToString(product),
          source: source
      },
      function (data) {
          $("[member=divCartList] > [member=divCartItemTb]").each(function () {
              var price = 0;
              var fee = "";
              for (var i = 0; i < data.length; i++) {
                  if (data[i].MerchantID != $(this).find("[member=ComId]").val()) {
                      continue;
                  }
                  if (data[i].Price == -1) {
                      fee = $commonLang.HOME_SHOPPINGCART_POSTAGE;//"包邮"
                      break;
                  }
                  if ($.trim($(this).find("[member=ComId]").val()) == data[i].MerchantID) {
                      price += data[i].Price;//.toFixed(2);
                  }
              }
              price = price.toFixed(2);
              if (fee != "") {
                  price = 0;
              }
              $(this).find("[member=SumPostagePrice]").text(fee == "" ? price : 0);

              var length = $(this).find("[member=trCartItem]").length;
              //跨行 .find("td:last") 最后一个td  by wuyaozheng
              $(this).find("[member=trCartItem]").eq(0).find("td:last").attr("rowspan", length);
              $(this).find("[member=trCartItem]").eq(0).find("[member=PostagePrice]").text(fee == "" ? price : fee);

              var SumPostagePrice = 0;
              //算订单运费费用
              $("[member=divCartList]").find("[member=SumPostagePrice]").each(function () {
                  SumPostagePrice += parseFloat($(this).text());
              });

              //var AllGoodsPostagePrice = parseFloat($("[member=AllGoodsPostagePrice]").text());
              $("[member=AllGoodsPostagePrice]").text(SumPostagePrice.toFixed(2));

              //支付金额
              var AllGoodsPrice = parseFloat($("[member=AllGoodsPrice]").eq(0).text()) + SumPostagePrice;
              $("[member=AllGoodsPrice]").eq(1).text(AllGoodsPrice);

              //删除
              if (length > 1)
                  for (var i = 1; i < length; i++) {
                      $(this).find("[member=trCartItem]").eq(i).find("[member=PostagePrice]").parent().remove();
                  }

          });
      }, "json");

}
