$(function () {
    //显示"设置交易密码"面板
    $('.js-setPayPwd').on('click', function () {
        var $this = $(this);
        var type = $this.data('type');
        var $title = $('.js-setpassword-title');
        $title.text($title.data('text' + type));

        var $lnkText = $('.js-setPayPwdlnk-text');
        $lnkText.text($lnkText.data('text' + type));

        $('.js-setPayPwdPnl').show();
        $('#mask').show();
    });

    //关闭"设置交易密码"面板
    $('.js-closePayPwdPnl').on('click', function () {
        $('.js-setPayPwdPnl').hide();
        $('#mask').hide();
    });

    //支付类型选择
    $('input[name="paymentType"],input[name="paymentType1"]').on('change', function () {
        paymentTypeChange(this);
    });

    //余额保护
    $("#bala_change").click(function () {
        if ($(this).hasClass("z_default")) {
            $("#z_balance").html("******");
            $(this).removeClass("z_default").addClass("z_code");
        } else if ($(this).hasClass("z_code")) {
            $("#z_balance").html($('#paymentTypeForBalance').data('balance') + ' 港元');
            $(this).removeClass("z_code").addClass("z_default");
        }
    });

    //获取验证码
    $('.js-getcode').on('click', function () {
        getCode();
    });

    //验证码检测
    $("#phonecode").blur(function () {
        if ($("#phonecode").val() == "") {
            $("#phonetip").show().html("<em></em><ins>" + $commonLang.INPUT_PHONECODE + "</ins>");
            return;
        }
        checkPhoneCode();
    });

    //密码检测
    $("#npwd").blur(function () {
        if ($.trim($("#npwd").val()) == "") {
            $("#mmtip").show().html("<em></em><ins>" + $commonLang.MONEY_ORDER_INPUTPAYPASSWORD + "</ins>");
        }
        checkPwd();

    });

    //确认密码
    $("#rpwd").blur(function () {
        checkConfirmPwd();
    });

    //收货地址
    $("#Order_detail").click(function () {
        if ($(".l_lineItem_box").hasClass("dn")) {
            $(".l_lineItem_box").slideDown().removeClass("dn").addClass("db");
            $(this).removeClass("l_address_pm4").addClass("l_address_pm3");
        } else if ($(".l_lineItem_box").hasClass("db")) {
            $(".l_lineItem_box").slideUp().removeClass("db").addClass("dn");
            $(this).removeClass("l_address_pm3").addClass("l_address_pm4");
        }
    });

    //立即支付
    $('.js-Payment').click(function () {
        var $this = $(this);
        if ($this.data('issubmit') == "1") {
            return;
        }
        var $balanceDom = $('#paymentTypeForBalance');
        var checkBalance = $('#paymentTypeForBalance').prop('checked');
        var payChannel = parseInt($('input[name="paymentType"]:checked').val());
        var payChangel2 = parseInt($balanceDom.val());

        var requestData = {
            PayChannel: checkBalance ? payChangel2 : payChannel,       //第一种支付方式
            PayChannel2: checkBalance ? payChannel : payChangel2,                                                //第二种支付方式
            PaymentOrderId: $('#paymentOrderId').val(),
            PayPassword: $('.js-payPassword').val()
        }

        $('#PayChannel').val(requestData.PayChannel);
        $('#PayChannel2').val(requestData.PayChannel2);
        $('#PayPassword').val(requestData.PayPassword);

        if (parseInt(requestData.PayChannel) == 1) {
            if (!requestData.PayPassword) {
                mallbox.alert({ message: $commonLang.MONEY_ORDER_ORDERINFO_TRANPASSWORD, modal: true });
                return;
            }

            //验证交易密码
            $.post('/Money/Payment/ValidPayPassword', { PayPassword: requestData.PayPassword }, function (data) {
                if (data && data.IsValid) {
                    //余额比较
                   // if (parseFloat($('#paymentTypeForBalance').data('balance')) < parseFloat($('#paymentTypeForBalance').data('totalamount'))) {
                    //    mallbox.alert({ message: $commonLang.MONEY_ORDER_INSUFFICIENT_BALANCE, modal: true });
                    //} else {
                        return submitOrder($this);
                   // }

                } else {
                    mallbox.alert({ message: data.Messages[0], modal: true });
                    return false;
                }
            });

        } else {
            return submitOrder($this);
        }


    });

    paymentTypeChange($('input[name="paymentType"]:checked')[0]);

    refreshCart();
});

function submitOrder(obj) {
    ///<summary>支付提交</summary>
    ///<param  name="obj" type="object" >支付按钮</param>

    var $this = $(obj);
    $this.data('issubmit', 1).css({ 'opacity': '.5', 'cursor': 'no-drop' });;                    //标记不能重复提交
    $('#btnPaymentOrder').click();
    return true;
}

////刷新购车库存
//function refreshCart() {
//    var cartCount = new MyShoppingCartBll().getGoodsCount();
//    //$(".z_count1").text(cartCount.iTotalCount);
//}



function paymentTypeChange(obj) {
    /// <summary>余额支付</summary>
    /// <param name="obj" type="Object">当前支付方式</param>
    var $this = $(obj);

    if ($this.length == 0) {
        $('#paymentTypeForOmise').attr('checked', true);
        $this = $('input[name="paymentType1"]:checked');
    }

    //余额支付对象
    var $balanceDom = $('#paymentTypeForBalance');

    var isBalanceChecked = $balanceDom.prop('checked');
    var isBalanceCheckbox = $balanceDom.prop('type') === 'checkbox';
    var totalAmount = parseFloat($balanceDom.data('totalamount'));  //订单总金额
    var userBalance = parseFloat($balanceDom.data('balance'));      //用户余额

    if (userBalance == 0 && totalAmount>=20) {
        $('#paymentTypeForOmise').attr('checked', true);
        return;
    }

    if (totalAmount < 20) {
        $('#paymentTypeForPayPal').attr('checked', true);
    }


    $('input[name="paymentType"]').parent().removeClass('address_bt2');

    $this.parent().addClass('address_bt2');

    switch (parseInt($this.val())) {
        case 1:
            var isSetPayPwd = parseInt($this.data('issetpaypwd')) == 1;
            if (isSetPayPwd) {
                $('.js-balanceInfoPnl1').show();
            } else {
                $('.js-balanceInfoPnl2').show();
            }
            $('.js-balance').slideDown();

            //余额不足
            if (totalAmount > userBalance) {

                var differenceAmount = totalAmount - userBalance;

                //已经选中，需要变更为单选按钮
                if (isBalanceChecked) {
                    //$balanceDom.attr({ 'type': 'checkbox', 'name': 'paymentType1' });
                    $("#paymentTypeForOmise").nextAll('div').addClass('z_paybala');
                    $("#paymentTypeForPayPal").nextAll('div').addClass('z_paybala');
                    for (var i = 0; i < 2; i++) {
                        $(".z_color").eq(i).html(differenceAmount.toFixed(2) + '港元'); //信用卡需要支付的金额
                    }

                    $("#z_minus").html(differenceAmount.toFixed(2) + '港元');       //余额不足,请再选择以上一种方式支付：62159บาท
                    $("#z_nobala").removeClass("z_paybala2");

                    if (parseInt($('input[name="paymentType"]:checked').val()) == 5) {
                        $("#paymentTypeForOmise").click();                  //默认选中omise
                    }
                    

                    $("#paymentTypeForCOD").attr('disabled', 'disabled');   //禁用货到付款
                    $(".z_cash>label").css({ 'color': '#666666' });


                    $("#paymentTypeForOmise").click(function () {
                        $(".z_paybala").eq(0).animate({ 'opacity': 'show' });
                        $(".z_paybala").eq(1).animate({ 'opacity': 'hide' });
                    });

                    $("#paymentTypeForPayPal").click(function () {
                        $(".z_paybala").eq(1).animate({ 'opacity': 'show' });
                        $(".z_paybala").eq(0).animate({ 'opacity': 'hide' });
                    });

                   

                } else {
                    //$balanceDom.attr({ 'type': 'radio', 'name': 'paymentType' });
                    $('.js-balance').slideUp();
                    //$('#paymentTypeForOmise').click();

                    $("#paymentTypeForCOD").attr('disabled', false);        //启用货到付款
                    $(".z_cash>label").css({ 'color': '#191919' });

                    $("#z_nobala").addClass("z_paybala2");
                    $("#paymentTypeForOmise").nextAll('div').removeClass('z_paybala').hide();       //隐藏信用卡支付金额
                    $("#paymentTypeForPayPal").nextAll('div').removeClass('z_paybala').hide();




                }
            }
            break;
        default:
            if (!isBalanceCheckbox) {
                $('.js-balance').slideUp();
            }
            break;
    }
}

