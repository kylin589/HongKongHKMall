using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace HKTHMall.WebApi.Common.Para
{
    [DataContract]
    public class Para
    {
      //  private static Logger logger = LogManager.GetCurrentClassLogger();
        //需要加密的参数
        private string _userName;
        private string _userId;
        private string _phone;
        private string _passWord;
        private string _repeatPwd;
        private string _oldPassWord;
        private string _productId;
        private string _activityId;
        private sbyte _registerSource;
        private string _key;
        private string _validateCode;
        private string _payPwd;
        private string _amount;
        private string _amountByBalance;
        private string _amountByByOnline;
        private string _amountByOnline;
        private string _amountByDou;
        private int _areaID;
        private string _account;
        private string _signature;
        private string _reportId;
        private string _loginId;
        private string _productNo;
        private string merchantId;
        private int _lang;
        private string _ShoppingCartId;
        /// <summary>
        ///  设置加密（0.加密 1.不加密） 戴勇军 修改
        /// </summary>

        [DataMember(Name = "forTest")]
        public static int ForTest = Convert.ToInt32(ConfigurationManager.AppSettings["ForTest"]);



        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember(Name = "account")]
        public string Account
        {
            set { _account = value; }
            get { return (ForTest == 1) ? _account : RSADecrypt("", _account); }
        }

        /// <summary>
        /// 商品编号
        /// </summary>
        [DataMember(Name = "productNo")]
        public string ProductNo
        {
            set { _productNo = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_productNo)) ? "0" : _productNo) : RSADecrypt("", ((string.IsNullOrEmpty(_productNo)) ? "0" : _productNo)); }
        }

        /// <summary>
        /// 登录id
        /// </summary>
        [DataMember(Name = "loginId")]
        public string LoginId
        {
            set { _loginId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_loginId)) ? "0" : _loginId) : RSADecrypt("", ((string.IsNullOrEmpty(_loginId)) ? "0" : _loginId)); }
        }

        /// <summary>
        /// 账号id 戴勇军 修改
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId
        {
            set { _userId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_userId)) ? "0" : _userId) : RSADecrypt("", ((string.IsNullOrEmpty(_userId)) ? "0" : _userId)); }
        }

        /// <summary>
        /// 被举报人ID
        /// </summary>
        [DataMember(Name = "reportId")]
        public string ReportId
        {
            set { _reportId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_reportId)) ? "0" : _reportId) : RSADecrypt("", ((string.IsNullOrEmpty(_reportId)) ? "0" : _reportId)); }
        }

        /// <summary>
        /// 商品ID 戴勇军 修改
        /// </summary>
        [DataMember(Name = "productId")]
        public string ProductId
        {
            set { _productId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_productId)) ? "0" : _productId) : RSADecrypt("", ((string.IsNullOrEmpty(_productId)) ? "0" : _productId)); }
        }


        /// <summary>
        /// 活动ID
        /// </summary>
        [DataMember(Name = "activityId")]
        public string ActivityId
        {
            set { _activityId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(_activityId)) ? "0" : _activityId) : RSADecrypt("", ((string.IsNullOrEmpty(_activityId)) ? "0" : _activityId)); }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember(Name = "userName")]
        public string UserName
        {
            set { _userName = value; }
            get { return (ForTest == 1) ? _userName : RSADecrypt("", _userName); }
        }

        /// <summary>
        ///支付密码
        /// </summary>
        [DataMember(Name = "payPwd")]
        public string PayPwd
        {
            set { _payPwd = value; }
            get { return (ForTest == 1) ? _payPwd : RSADecrypt("", _payPwd); }
        }

        [DataMember(Name = "password")]
        public string PassWord
        {
            set { _passWord = value; }
            get { return (ForTest == 1) ? _passWord : RSADecrypt("", _passWord); }
        }

        [DataMember(Name = "repeatPwd")]
        public string RepeatPwd
        {
            set { _repeatPwd = value; }
            get { return (ForTest == 1) ? _repeatPwd : RSADecrypt("", _repeatPwd); }
        }

        [DataMember(Name = "oldPassword")]
        public string OldPassWord
        {
            set { _oldPassWord = value; }
            get { return (ForTest == 1) ? _oldPassWord : RSADecrypt("", _oldPassWord); }
        }

        /// <summary>
        /// 电话号码
        /// </summary>
        [DataMember(Name = "phone")]
        public string Phone
        {
            set { _phone = value; }
            get { return (ForTest == 1) ? _phone : RSADecrypt("", _phone); }
        }

        /// <summary>
        ///提现金额
        /// </summary>
        [DataMember(Name = "amount")]
        public string Amount
        {
            set { _amount = value; }
            get { return (ForTest == 1) ? _amount : RSADecrypt("", _amount); }
        }

        /// <summary>
        ///余额支付金额
        /// </summary>
        [DataMember(Name = "amountByBalance")]
        public string AmountByBalance
        {
            set { _amountByBalance = value; }
            get { return (ForTest == 1) ? _amountByBalance : RSADecrypt("", _amountByBalance); }
        }

        /// <summary>
        ///网银支付金额
        /// </summary>
        [DataMember(Name = "amountByByOnline")]
        public string AmountByByOnline
        {
            set { _amountByByOnline = value; }
            get { return (ForTest == 1) ? _amountByByOnline : RSADecrypt("", _amountByByOnline); }
        }

        /// <summary>
        ///网银支付金额
        /// </summary>
        [DataMember(Name = "amountByOnline")]
        public string AmountByOnline
        {
            set { _amountByOnline = value; }
            get { return (ForTest == 1) ? _amountByOnline : RSADecrypt("", _amountByOnline); }
        }

        /// <summary>
        ///网银支付金额
        /// </summary>
        [DataMember(Name = "amountByDou")]
        public string AmountByDou
        {
            set { _amountByDou = value; }
            get { return (ForTest == 1) ? _amountByDou : RSADecrypt("", _amountByDou); }
        }

        /// <summary>
        /// 推荐码
        /// </summary>
        [DataMember(Name = "recommendCode")]
        public string RecommendCode { get; set; }


        /// <summary>
        /// 注册来源//来源(1：网站，2：Android，3-IOS)
        /// </summary>
        [DataMember(Name = "registerSource")]
        public sbyte RegisterSource { get; set; }

        /// <summary>
        /// 登录地址
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        [DataMember(Name = "ip")]
        public string IP { get; set; }
        /// <summary>
        /// 短信流程验证码
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        [DataMember(Name = "typeId")]
        public string TypeId { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember(Name = "pageNo")]
        public string PageNo { get; set; }

        /// <summary>
        /// 每页数据行数
        /// </summary>
        [DataMember(Name = "pageSize")]
        public string PageSize { get; set; }

        /// <summary>
        /// 银行ID
        /// </summary>
        [DataMember(Name = "bankId")]
        public string BankId { get; set; }
        /// <summary>
        /// 支行名称
        /// </summary>
        [DataMember(Name = "subbranchBank")]
        public string SubbranchBank { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [DataMember(Name = "realName")]
        public string RealName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [DataMember(Name = "cardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [DataMember(Name = "idNumber")]
        public string IDNumber { get; set; }

        /// <summary>
        /// 地址ID
        /// </summary>
        [DataMember(Name = "addressId")]
        public string AddressId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [DataMember(Name = "receiverName")]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [DataMember(Name = "receiverAddress")]
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// 惠粉用户所在地区ID
        /// </summary>
        [DataMember(Name = "areaID")]
        public int areaID { get; set; }


        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember(Name = "postCode")]
        public string PostCode { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember(Name = "receiverPhone")]
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 固话
        /// </summary>
        [DataMember(Name = "receiverTelephone")]
        public string ReceiverTelephone { get; set; }

        /// <summary>
        /// 收货区域ID
        /// </summary>
        [DataMember(Name = "receiverAreaID")]
        public string ReceiverAreaID { get; set; }

        /// <summary>
        /// App浏览地址
        /// </summary>
        [DataMember(Name = "appAddress")]
        public String AppAddress { get; set; }

        /// <summary>
        /// 银行卡ID
        /// </summary>
        [DataMember(Name = "cardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember(Name = "validateCode")]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        [DataMember(Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 举报原因（1.色情，2.骚扰广告，3.侮辱诋毁，4.诈骗钱财，5.其他）
        /// </summary>
        [DataMember(Name = "reasonType")]
        public int ReasonType { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember(Name = "contactWay")]
        public string ContactWay { get; set; }

        /// <summary>
        /// 验证状态
        /// </summary>
        [DataMember(Name = "validateStatus")]
        public int ValidateStatus { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        [DataMember(Name = "merchantId")]
        public string MerchantId
        {
            set { merchantId = value; }
            get { return (ForTest == 1) ? ((string.IsNullOrEmpty(merchantId)) ? "0" : merchantId) : RSADecrypt("", ((string.IsNullOrEmpty(merchantId)) ? "0" : merchantId)); }
        }


        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMember(Name = "orderNumber")]
        public long OrderNumber { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember(Name = "keyword")]
        public string Keyword { get; set; }


        /// <summary>
        /// 评论星级1-5(默认是5)
        /// </summary>
        [DataMember(Name = "commentLevel")]
        public string CommentLevel { get; set; }

        /// <summary>
        /// 服务态度星级1-5(默认是5)
        /// </summary>
        [DataMember(Name = "serviceLevel")]
        public string ServiceLevel { get; set; }

        /// <summary>
        /// 发货速度星级1-5(默认是5)
        /// </summary>
        [DataMember(Name = "speedLevel")]
        public string SpeedLevel { get; set; }

        /// <summary>
        /// 评论内容(500字，可以为空)
        /// </summary>
        [DataMember(Name = "commentContent")]
        public string CommentContent { get; set; }

        /// <summary>
        /// 是否匿名 1-是 0否（默认0）
        /// </summary>
        [DataMember(Name = "isAnonymous")]
        public string IsAnonymous { get; set; }

        /// <summary>
        /// 商品分类ID
        /// </summary>
        [DataMember(Name = "categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 0-为不做筛选 1-最终交易价从低到高 2最终交易价从高到底 3-折扣从高到底 4-折扣从低到高
        /// </summary>
        [DataMember(Name = "priceType")]
        public string PriceType { get; set; }

        /// <summary>
        /// 价格范围
        /// </summary>
        [DataMember(Name = "rangeOfPrice")]
        public string RangeOfPrice { get; set; }

        /// <summary>
        /// 品牌ID
        /// </summary>
        [DataMember(Name = "brandId")]
        public string BrandId { get; set; }

        /// <summary>
        /// sku属性
        /// </summary>
        [DataMember(Name = "skuAttribute")]
        public string SkuAttribute { get; set; }

        /// <summary>
        /// 销量类型 
        /// 0不做筛选 1从低到高 2 从高到底
        /// </summary>
        [DataMember(Name = "saleCountType")]
        public string SaleCountType { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }


        /// <summary>
        /// 距离类型 
        /// 0-不做筛选 1 离我最近 2离我最远
        /// </summary>
        [DataMember(Name = "distanceType")]
        public string DistanceType { get; set; }

        /// <summary>
        /// 浏览类型: 
        /// 0-不做筛选 1-用户浏览记录
        /// </summary>
        [DataMember(Name = "browseType")]
        public string BrowseType { get; set; }

        /// <summary>
        /// 分类最后更新时间
        /// </summary>
        [DataMember(Name = "updateTime")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 0：取一二级 1：取二三级
        /// </summary>
        [DataMember(Name = "classifyType")]
        public string ClassifyType { get; set; }

        /// <summary>
        /// 一级分类Id
        /// </summary>
        [DataMember(Name = "classifyId")]
        public string ClassifyId { get; set; }


        /// <summary>
        /// 充值订单类型:1-预付款  2-支付尾款
        /// </summary>
        [DataMember(Name = "orderType")]
        public string OrderType { get; set; }

        /// <summary>
        /// 支付方式1：余额支付；2：网银支付；3：余额+网银(混合)
        /// </summary>
        [DataMember(Name = "payType")]
        public string PayType { get; set; }

        /// <summary>
        /// 支付渠道0.余额支付,1.连连支付 2.银联, 3.支付宝 6.微信
        /// </summary>
        [DataMember(Name = "payChannel")]
        public string PayChannel { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember(Name = "orderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [DataMember(Name = "orderStatus")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        [DataMember(Name = "packageName")]
        public string PackageName { get; set; }

        /// <summary>
        /// 版本编号
        /// </summary>
        [DataMember(Name = "versionNo")]
        public string VersionNo { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [DataMember(Name = "nickName")]
        public string NickName { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [DataMember(Name = "signature")]
        public string signature { get; set; }

        /// <summary>
        /// 订购数量 戴勇军 修改
        /// </summary>
        [DataMember(Name = "count")]
        public string Count { get; set; }

        /// <summary>
        /// 惠信币金额
        /// </summary>
        [DataMember(Name = "vouchers")]
        public string Vouchers { get; set; }

        /// <summary>
        /// 发票类型 0 没有发票 1个人 2 公司
        /// </summary>
        [DataMember(Name = "receiptType")]
        public string ReceiptType { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember(Name = "receiptText")]
        public string ReceiptText { get; set; }

        /// <summary>
        /// 1:有余额支付   0:没有余额支付
        /// </summary>
        [DataMember(Name = "isUseBalance")]
        public string IsUseBalance { get; set; }

        /// <summary>
        /// 支付类型:1-连连 2-银联
        /// </summary>
        [DataMember(Name = "payChunnel")]
        public string PayChunnel { get; set; }

        /// <summary>
        /// 是否附近列表 0-否 1-是
        /// </summary>
        [DataMember(Name = "isNearby")]
        public string IsNearby { get; set; }


        /// <summary>
        /// Url
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// idArry
        /// </summary>
        [DataMember(Name = "idArry")]
        public string IdArry { get; set; }

        /// <summary>
        /// 列表编号 0:正在拼购;1:今日新单;2:提前预告;
        /// </summary>
        [DataMember(Name = "listNum")]
        public string ListNum { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>

        [DataMember(Name = "recordId")]
        public string RecordId { get; set; }

        /// <summary>
        /// 提醒Id
        /// </summary>
        [DataMember(Name = "remindId")]
        public string RemindId { get; set; }

        /// <summary>
        /// 是否需要子分类（0:不需要; 1:需要;）
        /// </summary>
        [DataMember(Name = "isSubclassification")]
        public string IsSubclassification { get; set; }

        /// <summary>
        /// 距离数
        /// </summary>
        [DataMember(Name = "distanceNum")]
        public string DistanceNum { get; set; }

        /// <summary>
        /// 热门类型 
        /// 0:人气（默认）;1:最新;
        /// </summary>
        [DataMember(Name = "hotType")]
        public string HotType { get; set; }

        /// <summary>
        /// 剩余人次类型 
        /// 0:不加入筛选;1:由多到少;2:由少到多
        /// </summary>
        [DataMember(Name = "surplusType")]
        public string SurplusType { get; set; }

        /// <summary>
        /// 总需要人次类型 
        /// 0:不加入筛选;1:由多到少;2:由少到多
        /// </summary>
        [DataMember(Name = "totalType")]
        public string TotalType { get; set; }

        /// <summary>
        /// 一元拼宝活动编号 
        /// </summary>
        [DataMember(Name = "onePieceId")]
        public string OnePieceActivityId { get; set; }

        /// <summary>
        /// 晒单ID
        /// </summary>
        [DataMember(Name = "showId")]
        public int ShowId { get; set; }

        /// <summary>
        /// 验证图片完整性，如果多张图，则逗号隔开，大写
        /// </summary>
        [DataMember(Name = "validateKey")]
        public string ValidateKey { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }


        /// <summary>
        /// 充值类型:0-充值金额  2-购买抵用金
        /// </summary>
        [DataMember(Name = "rechargeType")]
        public byte RechargeType { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember(Name = "sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember(Name = "age")]
        public string Age { get; set; }
        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        public string sku { get; set; }
        /// <summary>
        /// 购买数量  戴勇军 修改
        /// </summary>
        [DataMember(Name = "buyNum")]
        public string BuyNum { get; set; }
        /// <summary>
        /// 操作:1 修改数量，2 删除
        /// </summary>
        [DataMember(Name = "action")]
        public int Action { get; set; }

        /// <summary>
        /// 购物车物品ID数组:1,2,4,6
        /// </summary>

        [DataMember(Name = "cartIdArr")]
        public List<CartIdArr> CartArr { get; set; }

        /// <summary>
        /// 订单的商家信息
        /// </summary>
        [DataMember(Name = "merchants")]
        public List<MerchantForSubmit> Merchants { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember(Name = "createDt")]
        public long CreateDt { get; set; }


        /// <summary>
        /// 类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉
        /// </summary>
        [DataMember(Name = "gtype")]
        public int Gtype { get; set; }

        /// <summary>
        /// 消费编号
        /// </summary>
        [DataMember(Name = "jcNumber")]
        public int JcNumber { get; set; }


        /// <summary>
        /// 联系方式信息列表
        /// </summary>
        [DataMember]
        public string[] key_list { get; set; }

        /// <summary>
        /// 搜索内容
        /// </summary>
        [DataMember]
        public string searchName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public string orderNum { get; set; }

        /// <summary>
        /// 浏览记录编号
        /// </summary>
        [DataMember(Name = "browseId")]
        public string BrowseId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// 编辑购物车的商品ID列表
        /// </summary>
        [DataMember(Name = "product")]
        public string Product { get; set; }

        /// <summary>
        /// 购物车ID
        /// </summary>
        [DataMember(Name = "ShoppingCartId")]
        public string ShoppingCartId { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [DataMember(Name = "phoneNum")]
        public string PhoneNum { set; get; }

        /// <summary>
        /// APPID
        /// </summary>
        [DataMember(Name = "appId")]
        public string AppId { set; get; }

        /// <summary>
        /// 图片路径，多张图片|线分隔
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { set; get; }


        /// <summary>
        /// 行业分类ID
        /// </summary>
        [DataMember(Name = "industryId")]
        public string IndustryId { set; get; }

        /// <summary>
        /// 行业分类名称
        /// </summary>
        [DataMember(Name = "industryName")]
        public string IndustryName { set; get; }

        /// <summary>
        ///0离我最近(默认) 1人气最高
        /// </summary>
        [DataMember(Name = "isSorting")]
        public string IsSorting { set; get; }      

        /// <summary>
        /// 语言：1：中文，2：英文,3:泰文 戴勇军 修改
        /// </summary>
        [DataMember(Name = "lang")]
        public int lang { set; get; }

        ///<summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            try
            {
                publickey = @"<RSAKeyValue><Modulus>wVwBKuePO3ZZbZ//gqaNuUNyaPHbS3e2v5iDHMFRfYHS/bFw+79GwNUiJ+wXgpA7SSBRhKdLhTuxMvCn1aZNlXaMXIOPG1AouUMMfr6kEpFf/V0wLv6NCHGvBUK0l7O+2fxn3bR1SkHM1jWvLPMzSMBZLCOBPRRZ5FjHAy8d378=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                byte[] cipherbytes;
                rsa.FromXmlString(publickey);
                cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

                return Convert.ToBase64String(cipherbytes);
            }
            catch (Exception ex)
            {
               // logger.Error("RSAEncrypt:" + content, ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            if (null == content || "0".Equals(content))
            {
                return content;
            }
            else
            {
                try
                {
                    content = content.Replace(" ", "+").Replace("\\","");
                    privatekey = @"<RSAKeyValue><Modulus>wVwBKuePO3ZZbZ//gqaNuUNyaPHbS3e2v5iDHMFRfYHS/bFw+79GwNUiJ+wXgpA7SSBRhKdLhTuxMvCn1aZNlXaMXIOPG1AouUMMfr6kEpFf/V0wLv6NCHGvBUK0l7O+2fxn3bR1SkHM1jWvLPMzSMBZLCOBPRRZ5FjHAy8d378=</Modulus><Exponent>AQAB</Exponent><P>64ZxmWRaS8jXsVhv1IOQh+4dD9z9jfa9BAWDPvQykHcLUKE1h1jGoOTf6xby+4Wmb9FXdXifNj1WnJAwD1LGfw==</P><Q>0isr6Q0S01fL9HkOdrf5EJRIehhl4KZtFwEnEreNCg7PnDUlwVM9Uw+bGKrCzy0ZT1pbry9DkWLPY0srK9DGwQ==</Q><DP>DKoaCal/wXt3Pa4HtWGtr+F55pR3fd66ozC4sfXnkiUUkq1Yd4Kqi5RDBh0hy6yQGosjLMnjpcL+mUSXkPteeQ==</DP><DQ>g4/U1/mAHF5sZShWnoiB2BgK2qtlMuDbjzgAfp36Ix6sZat7a+6wh8tQGnvioRApNNxqYlqi4GLLUevfJXl2wQ==</DQ><InverseQ>kDJPNy+K90v4dAwUbREsx8fJAy3k0QAEy5Jk+Mq0ZIVzfTZ6tX4W+J1N8VwpM0uZcV+1nZiLu4E3ePaZgZQWig==</InverseQ><D>B3Dc8qO6lVU2l8tib8qtBYYc7wDvqXXP6Iub8A1Yb3YBgpXDfUydEmqhR9wEA5g9T9EYkfxGIbhsV0N/ke82aQriEBug4sUsRHiqfpfyW+MH1AHi71Z4qpu3GtjPuFEwKlCVDunK8xOn0cqYEs/SMnODJnbYMmtlcnfFic8PwQE=</D></RSAKeyValue>";
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    byte[] cipherbytes;
                    rsa.FromXmlString(privatekey);
                    cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                    return Encoding.UTF8.GetString(cipherbytes);
                }
                catch (Exception ex)
                {
                    HttpContext httpContext = HttpContext.Current;
                    // 获得前一个异常的实例
                    StringBuilder errorBuilder = new StringBuilder("请求地址：" + httpContext.Request.Url.AbsoluteUri + ",原始请求地址：" + httpContext.Request.RawUrl);
                    //UserAgent userAgent = UserAgentHelper.GetUserAgent(httpContext.Request.UserAgent);
                    //if (userAgent != null)
                    //{
                    //    errorBuilder.Append(",请求设备信息:" + JsonHelper.JsonSerializeObject(userAgent));
                    //    errorBuilder.Append(",UserAgent:" + httpContext.Request.UserAgent);
                    //}
                    try
                    {
                        string contentType = httpContext.Request.ContentType;
                        string[] contentTypeArray = new string[] { "application/json", "text/xml", "application/x-www-form-urlencoded", "application/xml" };
                        //if (Utils.InArray(contentType, contentTypeArray))
                        //{
                        //    var stream = httpContext.Request.InputStream;
                        //    using (var streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                        //    {
                        //        errorBuilder.Append(",请求内容：" + streamReader.ReadToEnd());
                        //    }
                        //}
                    }
                    catch (System.Exception exception)
                    {

                    }
                    //errorBuilder.Append("RSADecrypt:" + content + ",异常信息:" + ex.Message);
                    //logger.Error(errorBuilder.ToString());
                    return content;
                }
            }
        }




    }


    public class CartIdArr
    {
        [DataMember(Name = "productId")]
        public string ProductId { get; set; }

        [DataMember(Name = "sku")]
        public string SKU { get; set; }

        [DataMember(Name = "buyNum")]
        public string BuyNum { get; set; }
    }

    public class MerchantForSubmit
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [DataMember]
        public string merchantID { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember]
        public string receiptText { get; set; }

        /// <summary>
        /// 留言
        /// </summary>
        [DataMember]
        public string remark { get; set; }
    }

    public class ReqUserMsg
    {
        public string phone { get; set; }
        public string email { get; set; }
    }
    public class SearchUser
    {
        public int isPhone { get; set; }
        public List<ReqUserMsg> data { get; set; }
    }

}