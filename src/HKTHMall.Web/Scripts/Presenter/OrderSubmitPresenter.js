var flag = true;

function OrderSubmitPresenter(jBodyContainer, option) {
    /// <summary>订单提交页面与UI交互相关的逻辑</summary>
    /// <param name="jBodyContainer" type="selector"></param>
    /// <param name="option" type="object"></param>

    var jDom_divCartList = jBodyContainer.find("[member='divCartList']");

    var j_BtnSubmitOrder = jBodyContainer.find("#btnSOrder");

    this.pageLoad = function () {
        /// <summary>页面加载</summary>


        $('#closePayResult').click(function () {
            $('#PayResult').hide();
        });

        // 提交订单按钮
        j_BtnSubmitOrder.on('click', function () {
            var $this = $(this);
            if ($this.data('issubmit') == "1") {
                showDailog($commonLang.MONEY_ORDER_SAMEORDER, false);//请不要重复提交订单

                return;
            }
            var orderInfos = extractOrderData();
            if (orderInfos == null || orderInfos.length == 0) {
                showDailog($commonLang.MONEY_ORDER_ORDERNOPRODUCT, false);//没有商品,不能提交！
                return;
            }
            var gcount = 0
            for (var i = 0; i < orderInfos.length; i++) {
                if (orderInfos[i].Goods != null && orderInfos[i].Goods.length != 0) {
                    gcount += orderInfos[i].Goods.length;
                }
            }
            if (gcount == 0) {
                showDailog($commonLang.MONEY_ORDER_ORDERNOPRODUCT, false);//没有商品,不能提交！
                return;
            }
            var addOrderInfo = {
                "ReceiverAddressId": $('.addDefaultOn').next('input[name="userAddress"]').val(),           // 收货地址Id
                "PayChannel": 4,                             //默认为omise支付
                "PurchaseType": myCommonHelper.getQueryStringByName("productid") ? 1 : 0,  //购买类型               
                "MerchantViews": orderInfos
            };
            if (!addOrderInfo.ReceiverAddressId) {

                $("#showDialog").html($commonLang.MONEY_ORDER_ADDRECEIVEADDRESS);
                ds.dialog({
                    title: $commonLang.MONEY_ORDER_INFO,
                    content: _htmlADD,
                    yesText: $commonLang.HOME_SHOPPING_SURE,
                    onyes: function () {
                        //console.log("你点击了确定！");
                        this.close();
                        ShowAdd();
                    }
                });
                return;
            }

            if (!addOrderInfo.PayChannel) {
                showDailog($commonLang.MONEY_ORDER_SELECTCHANNEL, false);//请选择支付方式
                return;
            }
            addOrderInfo.PayType = "2";      //第三方支付:2
            submitOrder($this, addOrderInfo);
        });
        $(".dialogSubmit").click(function () {
            if (ValidateText()) {
                $.ajax({
                    url: "/UserInfo/CreateAddress",
                    data: {
                        userAddressId: $("#txtUserAddressId").val(),
                        Receiver: $("#txtReceiver").val(),
                        THAreaID: $("#slQu").val(),
                        DetailsAddress: $("#txtDetailsAddress").val(),
                        Mobile: $("#txtMobile").val(),
                        Phone: $("#txtPhone").val(),
                        Email: $("#txtEmail").val(),
                        //PostalCode: $("#txtPostalCode").val(),
                        Time: new Date().getTime()
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data, status) {
                        //data = JSON.parse(data);
                        if (data.IsValid) {
                            location.reload();
                            return false;
                        } else {
                            showDailog(data.Messages, false);
                        }
                    }
                });
            }
        });
        $("#slCountry").change(function () {
            $("#slQu").html("<option value='-1'>-" + $commonLang.MONEY_ORDER_SELECTPLEASE + "-</option>");
            SelectChange($(this).val(), "slSheng");
        });
        $("#slSheng").change(function () {
            SelectChange($(this).val(), "slShi");
        });
        $("#slShi").change(function () {
            SelectChange($(this).val(), "slQu");
        });

        $("#closeBut").click(function () {
            ResetMessage();
            $("#ShippingAddress").hide().stop();
            $("#mask").hide().stop();
        });

        //弹窗可移动
        $("#ShippingAddress,.js-setPayPwdPnl").draggable();

        $("#btnSOrder").bind("click", function () {
            $(".bor").addClass("border_red");
        });

        $(".bor").bind("click", function () {
            $(".bor").removeClass("border_red");
        });



        //初始化地址
        initAddress($('.addDefaultOn').next());

        //初始化订单数据
        this.loadOrderData();
    }

    function extractOrderData() {
        /// <summary>提取订单数据</summary>
        /// <returns type="Object">订单数据</returns>
        var orderInfos = [];

        var merchantView = {
            "MerchantID": jBodyContainer.find("[member='ComId']").val(),    // 商家Id
            "Remark": "",                                              //备注
            "Goods": []
        };
        jDom_divCartList.find("[member='uiCartItem']").each(function (i, item) {
            var good = {
                "ProductID": $(item).find("[member='GoodsId']").val(), // 商品ID
                "SkuNumber": $.trim($(item).find("[member='skuNum']").val()), // 商品skuID
                "ProductNumber": $(item).find("[member='ShowGoodsCount']").html() // 商品成交数量
            };
            merchantView.Goods.push(good);
        });
        orderInfos.push(merchantView);

        return orderInfos;
    }

    this.loadOrderData = function () {
        /// <summary>将数据绑定至UI</summary>

        jDom_divCartList.html("");

        //获取数据
        var orderData = getOrderData();


        if (!orderData) {
            return;
        }

        // 找到代表一个商家所有商品的模板       
        var comHtml = jDom_divCartList;
        var isCakes = false;
        if (orderData.length > 0) {
            jBodyContainer.find("#btnGoToOrder").removeClass("disabled");
        } else {
            jBodyContainer.find("#btnGoToOrder").addClass("disabled");
        }
        for (var i = 0; i < orderData.length; i++) {
            var curCom = orderData[i];
            var jCom = jQuery(comHtml);
            jBodyContainer.find("[member='AllGoodsPostagePrice']").html(parseFloat(curCom.ExpressMoney).toFixed(2));
            jBodyContainer.find("[member='ComId']").val(curCom.ComId);


            // 找到代表一个商品的TR模板
            alert($(".orderMainSpec[member='uiCartItem']").html());
            var itemHtml = $(".orderMainSpec[member='uiCartItem']")[0].outerHTML;

            var blnShowInvoice = false;
            var totalPrice = 0.00;
            var totalCount = 0;
            for (var j = 0; j < curCom.Goods.length; j++) {

                var goodsInfo = curCom.Goods[j];

                // 每一个商品的TR
                var jGoodsMemInCom_Template = jQuery(itemHtml);
                jGoodsMemInCom_Template.css("visibility", "visible");
                var ProductName = "";
                if (goodsInfo.GoodsName.length > 38) {
                    ProductName = goodsInfo.GoodsName.substring(0, 19) + "<br/>" + goodsInfo.GoodsName.substring(19, 38) + "...";
                }
                else if (goodsInfo.GoodsName.length > 19) {
                    ProductName = goodsInfo.GoodsName.substring(0, 19) + "<br/>" + goodsInfo.GoodsName.substring(19, goodsInfo.GoodsName.length);
                }
                else {
                    ProductName = goodsInfo.GoodsName;
                }
                jGoodsMemInCom_Template.find("[member='GoodsId']").val(goodsInfo.GoodsId);
                jGoodsMemInCom_Template.find("[member='GoodsName']").html(ProductName);
                jGoodsMemInCom_Template.find("[member='GoodsUnits']").html(goodsInfo.GoodsUnits.toFixed(2));
                jGoodsMemInCom_Template.find("[member='ShowGoodsCount']").html(goodsInfo.Count);
                totalCount += parseInt(goodsInfo.Count);
                jGoodsMemInCom_Template.find("[member='GoodsTotalPrice']").text((goodsInfo.GoodsUnits * goodsInfo.Count).toFixed(2));
                totalPrice += goodsInfo.GoodsUnits * goodsInfo.Count;
                jGoodsMemInCom_Template.find("[member='Pic']").attr("src", goodsInfo.Pic);
                jGoodsMemInCom_Template.find("[member='GoodsHref']").attr("href", "/Home/shopping/" + goodsInfo.GoodsId + ".html");
                //jGoodsMemInCom_Template.find("[member='StockQuantity']").text(goodsInfo.StockQuantity);    // 库存
                //jGoodsMemInCom_Template.find("[member='PostagePrice']").text(0);    // 邮费
                jGoodsMemInCom_Template.find("[member='skuNum']").val(goodsInfo.SkuNumber);
                //订单页,特殊处理
                var stockError = jGoodsMemInCom_Template.find("[member='StockQuantity-msg']");    // 库存
                if (parseInt(goodsInfo.StockQuantity) < parseInt(goodsInfo.Count)) {
                    stockError.html($commonLang.HOME_SHOPPINGCART_NOTENOUGH).show();//'库存不足'
                } else if (parseInt(goodsInfo.Status) == 5) {
                    stockError.html($commonLang.HOME_SHOPPINGCART_PRODUCTUNDERSHELVES).show();//'商品已下架'
                } else {
                    stockError.hide();
                }

                var skuNumberValue = goodsInfo.ValueStr == null ? "" : goodsInfo.ValueStr.split(",");
                var skuNumberKey = goodsInfo.AttributeName == null ? "" : goodsInfo.AttributeName.split(",");
                var skuValue = "";
                for (var k = 0; k < skuNumberValue.length; k++) {
                    skuValue += "<dd><span>" + skuNumberKey[k] + "：</span><span>" + skuNumberValue[k] + "</span></dd>";
                }
                jGoodsMemInCom_Template.find("[member='Sku']").html(skuValue);    // 显示Sku
                // 添加 商品TR              
                jCom.append(jGoodsMemInCom_Template);
                //if (myCommonHelper.stringEqualIgnoreCase(goodsInfo.IsDisabled, "1") == false
                //    && myCommonHelper.stringEqualIgnoreCase(goodsInfo.IsProvideInvoices, 1) == true) {
                //    blnShowInvoice = true;
                //}

            }
            jBodyContainer.find("[member='AllGoodsPrice']").html(totalPrice.toFixed(2));
            jBodyContainer.find("[member='AllGoodsCost']").html((totalPrice + curCom.ExpressMoney).toFixed(2));
            jBodyContainer.find("[member='AllGoodsCount']").html(curCom.Goods.length);
            $('#BackNow').html(totalPrice.toFixed(2));
            //var j_IsProvideInvoices = jCom.find("[member='IsProvideInvoices']");
            //j_IsProvideInvoices.show();

            //var j_hreftoggleInvoiceContent = j_IsProvideInvoices.find("[member='hreftoggleInvoiceContent']");
            //j_hreftoggleInvoiceContent.unbind("click");
            //j_hreftoggleInvoiceContent.click(function () {
            //    $(this).parent().find("[member='spanInvoicesContent']").toggle();
            //});

            //if (!blnShowInvoice) {
            //    j_IsProvideInvoices.find("[member='hreftoggleInvoiceContent']").hide();
            //    j_IsProvideInvoices.find("[member='spanInvoicesContent']").show();
            //    jCom.find("[member='IsProvideInvoices']").find(".invoices1").hide();
            //    jCom.find("[member='IsProvideInvoices']").find(".invoices2").show();
            //};

        }
    }

    function getOrderData() {
        /// <summary>获取订单数据</summary>
        /// <returns type="Object">订单数据</returns>
        var orderData = null;

        //获取收货地址 ，如果收货地址不为空，则获取运费
        var userAddressId = $('.addDefaultOn').next('input[name="userAddress"]').val();

        if (option && myCommonHelper.stringEqualIgnoreCase(option.IsOutrightPurchase, 1) == true) {

            var strGoodsId = myCommonHelper.getQueryStringByName("productid");

            var strSkuNumber = myCommonHelper.getQueryStringByName("skunumber"); //SKU_ProducId

            var strCount = myCommonHelper.getQueryStringByName("count");

            strCount = strCount == undefined ? 1 : strCount == null ? 1 : strCount == "" ? 1 : strCount;

            if (myCommonHelper.isNullOrEmpty(strGoodsId) == false) {

                orderData = getOutrightPurchaseOrderData([{ GoodsId: strGoodsId, SkuNumber: strSkuNumber, Count: strCount }], userAddressId).Data;
                if (orderData && orderData.length > 0) {
                    orderData[0].Goods[0].Count = strCount;
                }
                return orderData;
            }
        }
        orderData = myCommonHelper.asToJsonObj(myCommonHelper.getAjaxStringSync("/money/Order/OrderData", "Get", { userAddressId: userAddressId }));
        return orderData;
    }

    function getOutrightPurchaseOrderData(goods, userAddressId) {
        /// <summary>获取立即购买数据</summary>
        /// <param name="goods" type="array">请求数据</param>
        /// <param name="userAddressId" type="float">城市Id</param>
        /// <returns type="array">立即购买数据</returns>

        var productIds = [];            //商品Id集合
        var skuIds = [];                //商品sku集合
        var counts = [];                //商品sku集合

        $(goods).each(function (idx, mem) {
            productIds.push(mem.GoodsId);
            skuIds.push(mem.SkuNumber);
            counts.push(mem.Count);
        });

        if (productIds.length == 0) {
            return [];
        }

        //请求数据
        var requestData = {
            productIds: myCommonHelper.jsonObjToString(productIds),
            skuIds: myCommonHelper.jsonObjToString(skuIds),
            counts: myCommonHelper.jsonObjToString(counts),
            userAddressId: userAddressId
        };

        //获取立即购买数据
        var rslt = $.ajax("/money/Order/OutrightPurchaseOrderData", { type: "Get", async: false, data: requestData });

        var ret_json = jQuery.parseJSON(rslt.responseText);

        if (ret_json.Msg != "") {
            mallbox.alert({ message: ret_json.Msg });
        }
        return myCommonHelper.asToJsonObj(rslt.responseText);
    }

    function refreshCheckedTotalCount() {
        /// <summary>刷新统计数据</summary>
        var arrCheckedTr = jDom_divCartList.find("[member='trCartItem']");
        var iTotalCount = 0;
        var iTotalPrice = 0.00;
        var postagePrice = 0.00;

        //计算总运费
        jDom_divCartList.find("[member='expressMoney']").each(function (i, dom) {
            postagePrice += parseFloat($(dom).text());
        });


        // 找到每一行的数量相加
        $(arrCheckedTr).each(function (idx, mem) {
            var curCount = parseInt($(mem).find("[member='GoodsCount']").val());        //当前数量
            var curGoodsTotalPrice = parseFloat($(mem).find("[member='GoodsTotalPrice']").text());      //当前商品金额
            iTotalPrice = addNum(iTotalPrice, curGoodsTotalPrice);                                      //总金额
            iTotalCount += curCount;

        });

        jBodyContainer.find("[member='AllGoodsCount']").text(jDom_divCartList.find(":checkbox[member='ChkItem']").length);
        jBodyContainer.find("[member='AllGoodsCount']:first").text(jDom_divCartList.find("[member='trCartItem']").length - 1);
        jBodyContainer.find("[member='AllGoodsPrice']").text(iTotalPrice.toFixed(2));           //商品总金额
        jBodyContainer.find("[member='AllGoodsPostagePrice']").text(postagePrice.toFixed(2));  //总运费
        jBodyContainer.find("[member='AllGoodsCost']").html(addNum(iTotalPrice, postagePrice).toFixed(2));      //应付金额
        //jBodyContainer.find("[member='BackNow']").html(iTotalPrice.toFixed(2));      //返现金额
        //$('#BackNow').html(iTotalPrice);
        //alert(iTotalPrice);
    }

    function addNum(num1, num2) {
        /// <summary>自定义加法运算</summary>
        /// <param name="num1" type="float"></param>
        /// <param name="num2" type="float"></param>
        /// <returns type=""></returns>
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

}

function refreshOrder(paymentOrderId, $btn) {
    var timer = window.setInterval(function () {
        $.get('/money/order/OrderProcessResult?paymentOrderId=' + paymentOrderId, {}, function (data) {
            if (data) {

                switch (parseInt(data.Status)) {
                    case 0:
                        window.clearInterval(timer);
                        document.location = '/money/Payment/PaymentAction?paymentOrderId=' + paymentOrderId;
                        break;
                    case -1:
                        window.clearInterval(timer);
                        if (data.Messages.length > 0) {
                            showDailog(data.Messages[0], false);
                        }
                        $('>span', $btn).html($btn.data('text'));
                        $btn.data('issubmit', 0);
                        break;
                    case -2:
                        window.clearInterval(timer);
                        showDailog($commonLang.MONEY_ORDER_ADDRECEIVEADDRESS, true);//请填写收货地址
                        $('>span', $btn).html($btn.data('text'));
                        $btn.data('issubmit', 0);
                        break;
                    case -3:
                        window.clearInterval(timer);
                        showDailog($commonLang.HOME_SHOPPINGCART_NOTENOUGH, true);//库存不足
                        //myMyShoppingCart_Presenter.pageLoad();
                        $('>span', $btn).html($btn.data('text'));
                        $btn.data('issubmit', 0);
                        break;
                    case -4:
                        window.clearInterval(timer);
                        showDailog($commonLang.HOME_SHOPPINGCART_PRODUCTUNDERSHELVES, true); //商品已下架
                        //myMyShoppingCart_Presenter.pageLoad();
                        $('>span', $btn).html($btn.data('text'));
                        $btn.data('issubmit', 0);
                        break;
                }

            }
        });
    }, 3000);
}

function initAddress($this) {
    /// <summary>初始化收件地址</summary>
    /// <param name="$this" type="Object">当前地址（单选按钮）</param>

    if ($this && $this.length > 0) {
        $('#js-sure-address').html($this.data('address'));
        $('#js-sure-receiverinfo').html($this.data('receiverinfo'));
        $('#js-detail-address').show();
        //$('.js-address').parent().removeClass('redColor');
        //$this.parent().addClass('redColor');
    }
}

/**********交易密码***********/
var ticket = 1;
function getCode() {
    ///<summary>获取验证码</summary>
    if (ticket == 1) {
        document.getElementById("code").disabled = false;
    }
    ticket++;
    if (false == document.getElementById("code").disabled) {
        var seconds = new Date().getTime();
        var obj = parseInt(seconds / 1000);
        timtout(obj, 120);
        $.post("/UserInfo/SendPhoneMsg", function (data) {
            if (data.rs == 1) {
            } else {
                mallbox.alert({ message: data.msg, modal: true });
            }
        });
    }
}

function timtout(obj, time) {
    ///<summary>超时重新发送</summary>
    var s = time - (parseInt(new Date().getTime() / 1000) - obj);
    if (s <= 0) {
        document.getElementById("code").disabled = false;
        $("#code").html($commonLang.LOGIN_GETPASSWORD_SENDAGAIN);//重新发送
        return;
    }
    document.getElementById("code").disabled = true;

    $("#code").html(s + "s"); //秒后重新获取
    setTimeout(function () { timtout(obj, time) }, 500);
}

function checkPhoneCode() {
    ///<summary>验证短信验证码</summary>

    var code = $.trim($("#phonecode").val());
    if (code == "") {
        $("#phonetip").show().html("<em></em><ins>" + $commonLang.INPUT_PHONECODE + "</ins>");
        return false;
    }
    if (!new RegExp(/^\d{6}$/).test(code)) {
        $("#phonetip").show().html("<em></em><ins>" + $commonLang.LOGIN_GETPASSWORD_CORRECTCODE + "</ins>");
        return false;
    }

    $.ajax({
        url: "/UserInfo/PhoneVerificationCode",
        dataType: "json",
        type: "post",
        data: { "code": code },
        async: false,
        success: function (data) {
            if (data.rs == 1) {
                $("#phonetip").hide();
            }
            else {
                $("#phonetip").show().html("<em></em><ins>" + data.msg + "</ins>");
            }
        }
    });
}

function checkPwd() {
    ///<summary>检测交易密码</summary>

    var tegNum = /^\d{8,16}$/;
    var tegLetter = /^[a-zA-Z]{8,16}$/;
    var pattern = /^[!@@#$%^&*()_+|={}?><\-\]\\[\/]{8,16}$/;

    var $this = $("#npwd");

    var password = $this.val();
    if (password == "") {
        $("#mmtip").show().html("<em></em><ins>" + $commonLang.MONEY_ORDER_INPUTPAYPASSWORD + "</ins>");
        return false;
    }

    if (password.length < 8 || password.length > 16 || new RegExp(/[^\x00-\xff]|\s/).test(password)) {
        $("#mmtip").show().html("<em></em><ins>" + $commonLang.PAYPASSWORD_PWDFORMAT + "</ins>");
        return false;
    }
    if (tegNum.test(password) || tegLetter.test(password) || pattern.test(password)) {
        $("#mmtip").show().html("<em></em><ins>" + $commonLang.LOGIN_GETPASSWORD_PWDATLEASTINCLUDE + "</ins>");
        return false;
    }
    $.ajax({
        url: "/UserInfo/PlyPassExist",
        dataType: "json",
        type: "post",
        data: { "StrPlyPass": password, "type": 1 },
        async: false,
        success: function (data) {
            if (data.rs == 1) {
                $("#mmtip").hide();
            }
            else {
                $("#mmtip").show().html("<em></em><ins>" + data.msg + "</ins>");

            }
        }

    });
}

function checkConfirmPwd() {
    ///<summary>检查确认密码</summary>
    var $this = $("#rpwd");
    var pwd = $("#npwd").val();
    var rpwd = $("#rpwd").val();
    if ($.trim(rpwd) == "") {
        $("#qmmtip").show().html("<em></em><ins>" + $commonLang.LOGIN_GETPASSWORD_PWDCONFIRM + "</ins>");
    } else {
        if ($.trim(rpwd) != $.trim(pwd)) {
            $("#qmmtip").show().html("<em></em><ins>" + $commonLang.LOGIN_GETPASSWORD_PWDDIFFERENT + "</ins>");
            return false;
        }
        else {
            $("#qmmtip").hide();
        }
    }
}

function submitPayPwd() {
    ///<summary>提交交易密码</summary>
    checkPhoneCode();
    checkPwd();
    checkConfirmPwd();

    var yzm = $("#phonetip").is(":hidden");
    var npwd = $("#mmtip").is(":hidden");
    var cpwd = $("#qmmtip").is(":hidden");

    if (yzm && npwd && cpwd) {
        $(".js-setPayPwdlnk").attr("href", 'javascript:;');
        $.post("/UserInfo/SetPayPassword",
               {
                   code: $("#phonecode").val(),
                   pwd: $("#npwd").val(),
                   cpwd: $("#rpwd").val()
               },
               function (data) {
                   if (data.rs == 1) {
                       mallbox.alert({
                           message: data.msg,
                           modal: true,
                           callback: function () {
                               $('.js-balanceInfoPnl1').show();
                               $('.js-balanceInfoPnl2').hide();
                               $('.js-setPayPwdPnl').hide();
                               $("#mask").hide();
                               $('#paymentTypeForBalance').data('issetpaypwd', 1);
                           }
                       });
                   }
                   else {
                       mallbox.alert({ message: data.msg, modal: true });
                       $("#submit").attr("href", "javascript:void(submitPayPwd())"); //使按钮能够被点击
                   }
               });
    }
    else {
        $("#submit").attr("href", "javascript:void(submitPayPwd)"); //使按钮能够被点击
        return false;
    }
};

function submitOrder($this, addOrderInfo) {
    ///<summary>提交订单</summary>
    ///<param  name="$this" type="object" >提交按钮</param>
    ///<param name="addOrderInfo" type="object">订单数据</param>
    $this.data('issubmit', 1);                    //标记不能重复提交
    $('>span', $this).html($commonLang.MONEY_ORDER_SUBMITING);
    $.post("/money/Order/GenerateOrder", { "orderData": myCommonHelper.jsonObjToString(addOrderInfo) }, function (data) {
        if (data) {
            flag = true;
            refreshOrder(data.paymentOrder, $this);
        } else {
            mallbox.alert({ message: $commonLang.MONEY_ORDER_SYSTEMERROR });//系统繁忙,请稍候再试
        }
    });
    flag = false;
}

function UpdateUserAddressFlagWeb(userAddressId, userId) {
    $.ajax({
        url: "/UserInfo/UpdateUserAddressFlag",
        data: {
            userAddressId: userAddressId,
            txtUserId: userId,
            time: new Date().getTime()
        },
        dataType: "text",
        success: function (data, status) {
            data = JSON.parse(data);
            if (data.IsValid) {
                location.reload();
            } else {
                mallbox.alert({ message: data.Messages });
            }
        }
    });
}
var _htmlADDRES = $("#addressDialog");
function ShowAdd() {
    ResetMessage(_htmlADDRES);
    $("#txtUserAddressId").val("0");
    $.post("/Money/Order/GetCurrentUserEmail", null, function (data) { $("#txtEmail").val(data.Email); }, "json");
    // $("#fromAddress")[0].reset();
    ds.dialog({
        title: '添加收货人信息',
        content: _htmlADDRES,
        tijiao: function () {

        }
    });
    $("#mask").show().fadeIn();

}
