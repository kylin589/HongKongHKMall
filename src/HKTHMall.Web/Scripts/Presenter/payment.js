var isSetPwd = false;
$(function () {
    var isSubmit;
    switchPwd();

    var _html = $('#dSetPwdWin');
    $("#aSetPwd").click(function () {
        ds.dialog({
            title: HOME_PAYMENT_SETPAYPWD,
            content: _html,
            yesText: HOME_SHOPPING_SURE,
            onyes: function myfunction() {
                //this.close();
                setPayPwd(this);
                return false;
            }
        });
    });

    $("#aPayNow").click(function () {

        if (isSubmit) {
            return false;
        }
        var $balance = $('.js-pay-balance');        //余额支付按钮
        var isCheckBalance = $balance.prop('checked');  //是否选中余额支付
        var payChannel = parseInt($('input[name="pay_method"]:checked').val());    //选中的支付方式
        var $balancePay = parseInt($balance.val());     //余额支付


        $('#PayChannel').val(isCheckBalance ? $balancePay : payChannel);        //选中余额支付 PayChannel为余额支付
        $('#PayChannel2').val(isCheckBalance ? payChannel : $balancePay);       //选中余额支付 PayChannel2为paymentType选中的支付,如果余额足够，此字段就是余额支付的值


        if (checkPayParameter()) {
            $("#PayPassword").val($("#PayPwd").val());
            $("#aPayNow").remove();





            isSubmit = true;
            $("#formPay").submit();
        }
    });

    //支付类型选择
    $('input[name="pay_method"],input[name="pay_method2"]').on('change', function () {
        paymentTypeChange(this);
    });

    paymentTypeChange($('input[name="pay_method"]:checked')[0]);

});

function switchPwd() {
    if (isSetPwd) {
        $(".ls_set_pay2").hide();
        $(".ls_set_pay1").show();
    } else {
        $(".ls_set_pay1").hide();
        $(".ls_set_pay2").show();
    }
}

function checkPayParameter() {

    if ($("#PayChannel").val() == "") {
        layer.msg(HOME_SHOPING_SELECTPAYWAY);
        return false;
    } else if ($("#PayChannel").val() == "1") { //余额
        if (!isSetPwd) {
            layer.msg(HOME_SHOPING_NOTSETPAYWAY);
            return false;
        } else if ($("#PayPwd").val().length < 6) {
            layer.msg(HOME_SHOPING_INPUTTRANSPWD);
            return false;
        } 
    } else if ($("#PayChannel").val() == "2") { //PayPal

    }
    return true;
}
function setPayPwd(win) {
    if (checkpwd()) {
        $.post("/UserInfo/SetPayPassword", { pwd: $("#setpwd").val(), cpwd: $("#setpwd2").val() }, function (data) {
            if (data.rs == 1) {
                isSetPwd = true;
                switchPwd();
                layer.msg(data.msg);
                win.close();
            } else {
                layer.msg(data.msg);
            }
        });
    }
}
function checkpwd() {
    var pwd = $("#setpwd").val(),
        pwd2 = $("#setpwd2").val();
    if (pwd.length < 6 || pwd2.length > 16) {
        layer.msg(HOME_SHOPING_PLEASESETPAYPWD);
        return false;
    }
    if (pwd != pwd2) {
        layer.msg(VERIFYCODE_TWO_PWD_NOT_SAME);
        return false;
    }
    return true;
}


function paymentTypeChange(obj) {
    /// <summary>更改支付方式</summary>
    /// <param name="obj" type="Object">当前支付方式</param>
    var $this = $(obj);

    if ($this.length == 0) {
        $('.js-pay-paypal').attr('checked', true);
        $this = $('input[name="pay_method2"]:checked');     //混合支付
    }

    //余额支付对象
    var $balance = $('.js-pay-balance');

    var isBalanceChecked = $balance.prop('checked');
    var isBalanceCheckbox = $balance.prop('type') === 'checkbox';
    var totalAmount = parseFloat($balance.data('totalamount'));  //订单总金额
    var userBalance = parseFloat($balance.data('balance'));      //用户余额

    switch (parseInt($this.val())) {
        case 1:
            switchPwd();
            $(".ls_set_pay").slideDown(400);
            //余额不足
            if (totalAmount > userBalance) {
                $('input[name="pay_method"]').nextAll("span").fadeOut(400);

                if (isBalanceChecked) {
                    var differenceAmount = totalAmount - userBalance;       //余额支付差额 =订单金额-用户余额
                    $(".js-differenceAmount").html(differenceAmount.toFixed(2));       //余额支付差额
                    $('.js-pay-mixture-msg').show();
                    $('input[name="pay_method"]:checked').nextAll("span").fadeIn(400);
                } else {
                    $('.js-pay-mixture-msg').hide();
                    $(".ls_set_pay").slideUp(400);
                }

            }
            break;
        default:

            //混合支付
            if (isBalanceCheckbox && isBalanceChecked) {
                $('.js-pay-mixture-msg').show();    //显示混合支付提示语
                $('input[name="pay_method"]').nextAll("span").fadeOut(400);     //隐藏所有支付的支付金额
                $this.nextAll("span").fadeIn(400);    //显示当前支付的支付金额

            } else {
                $('.js-pay-mixture-msg').hide();   //隐藏混合支付提示语
                $('input[name="pay_method"]').nextAll("span").fadeOut(400);     //隐藏所有支付的支付金额
                $(".ls_set_pay").slideUp(400);
            }
            break;
    }
}
