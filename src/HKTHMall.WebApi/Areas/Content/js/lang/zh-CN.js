﻿
var commonLang = ({
    ACCOUNT_MY_ACCOUNTRECHARGE_INPUTCORRECTAMOUNT: "请输入正确的充值金额",
    ACCOUNT_MY_ACCOUNTRECHARGE_SUBMITTED: "正在提交中,请勿多次点击",
    ACCOUNT_MY_ACCOUNTRECHARGE_THIRDPARTY: "请选择第三方充值！",
    ACCOUNT_MY_ACCOUNTRECHARGE_TOPAMOUNT: "充值金额必须处于100-1000000฿之间！",
    ACCOUNT_MY_ORDERACTION_DNOTRESUBMIT: "请不要重复操作",
    ACCOUNT_MY_ORDERACTION_SURECANCEL: "你确定要取消订单吗？",
    ACCOUNT_MY_ORDERACTION_CANCELSUCCESS: "取消订单成功",
    ACCOUNT_MY_ORDERACTION_RECEIVING: "你确定收货吗？",
    ACCOUNT_MY_ORDERACTION_RECEIVING_SUCCESS: "确认收货操作成功!",
    ACCOUNT_MY_ORDERACTION_SYSTEMBUSY: "系统繁忙,请稍后再试",
    ACCOUNT_MY_ORDERCOMPLAINT_COMPLAINTFAIL: "投诉失败,请重试",
    ACCOUNT_MY_ORDERCOMPLAINT_INPUTCONTENT: "请填写投诉内容",
    ACCOUNT_MY_ORDERCOMPLAINT_OUTOFRANGE: "最多可以输入500个字符,你已超出{0}个字符",
    ACCOUNT_My_OrderComplaints_ContentERRORPrompt: "最多可以输入500个字符,你已超出{0}个字符",
    ACCOUNT_My_OrderComplaints_SAVAERRORPROMPT: "投诉失败,请重试",
    ACCOUNT_My_OrderComplaints_thecomplaint: "请填写投诉内容",
    ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL: "操作失败,请重试",
    ACCOUNT_MY_ORDERRETURNPRODUCT_OUTOFRANGE: "最多可以输入200个字符,你已超出{0}个字符",
    ACCOUNT_MY_ORDERRETURNPRODUCT_QUANTITY: "请填写退货数量",
    ACCOUNT_MY_ORDERRETURNPRODUCT_REASON: "请选择退款原因",
    ACCOUNT_MY_ORDERRETURNPRODUCT_WRONGQUANTITY: "退货数量不能大于购买数量",
    ACCOUNT_My_OrderReturnProductInfo_complaintError: "最多可以输入200个字符,你已超出{0}个字符",
    ACCOUNT_My_OrderReturnProductInfo_Quantity: "请填写退货数量",
    ACCOUNT_My_OrderReturnProductInfo_QuantityError: "退货数量不能大于购买数量",
    ACCOUNT_My_OrderReturnProductInfo_Reason: "请选择退款原因",
    ACCOUNT_My_OrderReturnProductInfo_RRORPROMPT: "操作失败,请重试",
    ACCOUNT_MY_TRADECOMMENT_ATLEASTCOMMENT: "至少输入一款商品评论",
    ACCOUNT_MY_TRADECOMMENT_COMMENTLEVEL: "请选择评价星级",
    ACCOUNT_MY_TRADECOMMENT_COMMENTSUCCESS: "评论成功",
    ACCOUNT_MY_TRADECOMMENT_MAXLENGTH: "最多可以输入500个字",
    ACCOUNT_MY_TRADECOMMENT_SCORE: "分",
    ACCOUNT_MY_TRADECOMMENT_SUBMITCOMMENT: "评论已提交",
    ACCOUNT_MY_TRADECOMMENT_SUBMITFAIL: "服务器繁忙,请稍后再评论,谢谢",
    ACCOUNT_MY_WEALTH_COPYCONTENT: "请Ctrl+C复制信息",
    ACCOUNT_MY_WEALTH_COPYSUCCESS: "复制成功",
    ACCOUNT_MY_WEALTH_CORRECTAMOUNTWITHDRAWAL: "请输入正确的提现金额",
    ACCOUNT_MY_WEALTH_EMPTYMONEY: "请输入正确的提现金额",
    ACCOUNT_MY_WEALTH_ENTERMONEY: "请输入提现金额",
    ACCOUNT_MY_WEALTH_ENTERWITHDRAWALAMOUNT: "请输入提现金额",
    ACCOUNT_MY_WEALTH_FAIL: "提现金额操作失败",
    ACCOUNT_MY_WEALTH_MONEYOUTOFRANGE: "提现余额不能大于用户余额",
    ACCOUNT_MY_WEALTH_NOCASHBALANCE: "提现余额不能大于用户余额",
    ACCOUNT_MY_WEALTH_RIGHTNOW: "立即提现",
    ACCOUNT_MY_WEALTH_THEREBALANCE: "系统中不存在用户余额信息",
    ACCOUNT_MY_WEALTH_USERNOTEXIST: "系统中不存在用户余额信息",
    ACCOUNT_MY_WEALTH_WAIT: "正在提现中,请等候…",
    ACCOUNT_MY_WEALTH_WITHDRAWALAMOUNTOPERATIONFAILED: "提现金额操作失败",
    ACCOUNT_MY_WEALTH_WITHDRAWALWAIT: "正在提现中,请等候",
    ACCOUNT_USERINFO_ADDRESS_AreYouDeleteAddress: "确定删除该收货地址吗",
    ACCOUNT_USERINFO_ADDRESS_AreYouSetDefaultAddress: "确定要设为默认收货地址吗",
    ACCOUNT_USERINFO_ADDRESS_edit: "编辑",
    ACCOUNT_USERINFO_INDEX_CHECKLOGINEMAIL: "抱歉!未找到对应的邮箱登录地址,请自己登录邮箱查看邮件",
    ACCOUNT_USERINFO_INDEX_CORRECTEMAILADDRESS: "请输入正确的邮箱地址",
    ACCOUNT_USERINFO_INDEX_ENTEREMAILADDRESS: "请先输入邮箱地址",
    ACCOUNT_USERINFO_INDEX_ENTERFULLBIRTHDAY: "请输入完整的生日",
    ACCOUNT_USERINFO_INDEX_ENTERNICKNAME: "请输入昵称",
    ACCOUNT_USERINFO_INDEX_ILLEGALCHARACTERS: "昵称中包含非法字符",
    ACCOUNT_USERINFO_INDEX_NICKNAMEMAXIMUM: "昵称最大字符长度为20字符",
    ACCOUNT_USERINFO_INDEX_SELECTADDRESS: "请选择地址",
    ACCOUNT_USERINFO_INDEX_SELECTGENDER: "请选择性别",
    ACCOUNT_USERINFO_INDEX_SELECTIMAGES: "请选择jpg,gif,png,jpeg格式图片",
    ACCOUNT_USERINFO_INDEX_SENDIN: "发送中",
    ACCOUNT_USERINFO_INDEX_SENDSUCCESS: "发送成功",
    ACCOUNT_USERINFO_INDEX_UPLOADFAILED: "上传失败",
    ACCOUNT_USERINFO_PWDLENGTH: "密码长度为8至20位",
    ACCOUNT_USERINFO_PWDSUCCESS: "修改成功,请重新登录！",
    Add: "添加",
    Edit: "编辑",
    HOME_ACTIVE_DAY: "天",
    HOME_ACTIVE_DESCRIBED: "限时抢购",
    HOME_ACTIVE_HOUR: "时",
    HOME_ACTIVE_MINUTE: "分",
    HOME_ACTIVE_SECOND: "秒",
    HOME_SHOPPING_BUYCOUNT: "请添加购买数量",
    HOME_SHOPPING_COLLECT: "已收藏",
    HOME_SHOPPING_EXCEPTION: "库存异常无法下单",
    HOME_SHOPPINGCART_ATLEASTONEPRODUCT: "请至少选择一个商品",
    HOME_SHOPPINGCART_EDITPRODUCTCOUNT: "商品数量不能大于库存,请修改商品数量",
    HOME_SHOPPINGCART_NOPRODUCT: "选择的商品数量不能为0",
    HOME_SHOPPINGCART_NOTENOUGH: "库存不足",
    HOME_SHOPPINGCART_POSTAGE: "包邮",
    HOME_SHOPPINGCART_PRODUCTCOUNTOUTOFRANGE: "商品数量不能大于库存",
    HOME_SHOPPINGCART_PRODUCTUNDERSHELVES: "商品已下架",
    INPUT_KEYWORDS: "请输入关键字",
    LOGIN_GETPASSWORD_CORRECTCODE: "请输入正确的手机验证码",
    LOGIN_GETPASSWORD_GETAGAIN: "秒后重新获取",
    LOGIN_GETPASSWORD_PWDATLEASTINCLUDE: "密码至少包含字母和数字",
    LOGIN_GETPASSWORD_PWDCONFIRM: "请输入确认密码",
    LOGIN_GETPASSWORD_PWDDIFFERENT: "两次密码不相同",
    LOGIN_GETPASSWORD_PWDFORMAT: "密码由8-20位数字、字母或特殊字符组成,区分大小写",
    LOGIN_GETPASSWORD_SENDAGAIN: "重新发送",
    LOGIN_GETPASSWORD_UNREGIST: "该手机号码没有注册",
    LOGIN_GETPASSWORD_WRONGCODE: "输入的验证码错误",
    LOGIN_GETSUCCESS_SECOND: "秒",
    LOGIN_INDEX_CORRECTACCOUNT: "请输入手机号码",
    LOGIN_INDEX_CORRECTACCOUNTPWD: "请输入手机号码和密码",
    LOGIN_INDEX_CORRECTPHONE: "请输入正确的手机号码",
    LOGIN_INDEX_CORRECTPWD: "请输入密码",
    Modify: "修改",
    MONEY_ORDER_ADDRECEIVEADDRESS: "请添加收货地址",
    MONEY_ORDER_AGREENOTRETURN: "请先同意支付定金（不退）",
    MONEY_ORDER_DELETEFAIL: "删除失败！请刷新重试",
    MONEY_ORDER_EDIT: "编辑",
    MONEY_ORDER_INFO: "提示",
    MONEY_ORDER_INPUTCHECKOUTTEXT: "请填写发票抬头",
    MONEY_ORDER_INPUTPAYPASSWORD: "请输入交易密码",
    MONEY_ORDER_NETERROR: "网络连接失败,请检查网络状态,或稍后访问,谢谢",
    MONEY_ORDER_ORDERINFO_DELETEFAILED: "删除失败！请刷新重试！",
    MONEY_ORDER_ORDERINFO_DEPOSIT: "请先同意支付定金（不退）",
    MONEY_ORDER_ORDERINFO_GOODSORDER: "订单中有库存不足商品",
    MONEY_ORDER_ORDERINFO_GOODSSHELVES: "订单中有已下架商品",
    MONEY_ORDER_ORDERINFO_HAVESHELVES: "商品已下架",
    MONEY_ORDER_ORDERINFO_INSUFFICIENT: "库存不足",
    MONEY_ORDER_ORDERINFO_INVOICE: "请填写发票抬头",
    MONEY_ORDER_ORDERINFO_NETWORKSTATE: "网络连接失败,请检查网络状态,或稍后访问,谢谢！",
    MONEY_ORDER_ORDERINFO_NOTORDER: "请不要重复提交订单",
    MONEY_ORDER_ORDERINFO_PAYMENTMETHOD: "请选择支付方式",
    MONEY_ORDER_ORDERINFO_QUANTITY: "商品数量不能为0",
    MONEY_ORDER_ORDERINFO_SHIPPADDRESS: "请选择收货地址",
    MONEY_ORDER_ORDERINFO_SYSTEMBUSY: "系统繁忙,请稍候再试",
    MONEY_ORDER_ORDERINFO_TRANPASSWORD: "请输入交易密码",
    MONEY_ORDER_ORDERINFO_UNPAIDORDERS: "只有未支付的订单才能重新支付",
    MONEY_ORDER_ORDERNOPRODUCT: "没有商品,不能提交!",
    MONEY_ORDER_ORDERPRODUCTZERO: "商品数量不能为0",
    MONEY_ORDER_OTHERAREA: "其他",
    MONEY_ORDER_SAMEORDER: "请不要重复提交订单",
    MONEY_ORDER_SELECTCHANNEL: "请选择支付方式",
    MONEY_ORDER_SELECTPLEASE: "请选择",
    MONEY_ORDER_SELECTRECEIVEADDRESS: "请选择收货地址",
    MONEY_ORDER_SUBMITING: "提交中...",
    MONEY_ORDER_SYSTEMERROR: "系统繁忙,请稍候再试",
    ORDER_LIST_JUMPTO: "跳转到...",
    ORDER_LIST_NEXTPAGE: "下一页",
    ORDER_LIST_PAGE: "页",
    ORDER_LIST_PREVIOUSPAGE: "上一页",
    ORDER_LIST_SURE: "确定",
    ORDER_LIST_CANCEL: "取消",
    ORDER_LIST_TO: "到",
    ORDERRETURN_ATMOSTMONEY: "最多{0}",
    PLEASE_SELECT: "请选择",
    REGISTER_INDEX_AGGREMENT: "请同意惠卡用户注册协议",
    REGISTER_INDEX_CORRECTEMAIL: "请输入正确的邮箱地址",
    REGISTER_INDEX_INVITERNOTEXIST: "邀请人不存在",
    REGISTER_INDEX_INVITERPHONEWRONG: "推荐人手机号不正确",
    REGISTER_INDEX_REGISTED: "该手机号码已注册",
    HOME_SHOPPINGCART_DELETEGOOD: "确定删除该商品吗？",
    HOME_SHOPPINGCART_DELETEALLGOODS: "确定删除选中的商品吗？",
    ACCOUNT_MY_ORDERPRODUCTLISTT2_CONFIMREFUND:"确定要撤消申请吗？"
});
$commonLang = commonLang;


