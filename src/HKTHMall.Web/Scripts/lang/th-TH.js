﻿
var commonLang = ({
    HOME_SHOPING_UPDATEADDRESS: "Update shipping address",
    ACCOUNT_MY_ACCOUNTRECHARGE_INPUTCORRECTAMOUNT: "กรุณาใส่จำนวนเงินที่คุณต้องการให้ถูกต้อง",
    ACCOUNT_MY_ACCOUNTRECHARGE_SUBMITTED: "บันทึกข้อมูลอยู่…",
    ACCOUNT_MY_ACCOUNTRECHARGE_THIRDPARTY: "กรุณาเลือกช่องทางการเติมเงินออนไลน์",
    ACCOUNT_MY_ACCOUNTRECHARGE_TOPAMOUNT: "วงเงินที่เติมตั้งแต่ 100บาทขึ้นไป  แต่ไม่เกิน1000000บาท",
    ACCOUNT_MY_ORDERACTION_DNOTRESUBMIT: "กรุณาอย่าทำซ้ำ",
    ACCOUNT_MY_ORDERACTION_SURECANCEL: "คุณต้องการยกเลิกการสั่งซื้อ ?",
    ACCOUNT_MY_ORDERACTION_CANCELSUCCESS: "ยกเลิกการสั่งซื้อสำเร็จ",
    ACCOUNT_MY_ORDERACTION_RECEIVING: "คุณต้องการรับสินค้าหรือไม่?",
    ACCOUNT_MY_ORDERACTION_RECEIVING_SUCCESS: "ยืนยันรับของสำเร็จ",
    ACCOUNT_MY_ORDERACTION_SYSTEMBUSY: "ระบบไม่ว่าง  โปรดลองอีกครั้งในภายหลัง",
    ACCOUNT_MY_ORDERCOMPLAINT_COMPLAINTFAIL: "แจ้งเรื่องร้องเรียนล้มเหลว    กรุณาลองอีกครั้ง",
    ACCOUNT_MY_ORDERCOMPLAINT_INPUTCONTENT: "กรุณาระบุข้อร้องเรียน",
    ACCOUNT_MY_ORDERCOMPLAINT_OUTOFRANGE: "ต้องไม่เกิน 500 ตัวอักษร  คุณป้อนเกิน{0} ตัวอักษร",
    ACCOUNT_My_OrderComplaints_ContentERRORPrompt: "ต้องไม่เกิน 500 ตัวอักษร  คุณป้อนเกิน{0} ตัวอักษร",
    ACCOUNT_My_OrderComplaints_SAVAERRORPROMPT: "แจ้งเรื่องร้องเรียนล้มเหลว    กรุณาลองอีกครั้ง",
    ACCOUNT_My_OrderComplaints_thecomplaint: "กรุณาระบุข้อร้องเรียน",
    ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL: "ดำเนินการล้มเหลว  กรุณาลองอีกครั้ง",
    ACCOUNT_MY_ORDERRETURNPRODUCT_OUTOFRANGE: "ไม่เกิน 200 ตัวอักษร  คุณป้อนเกิน{0} ตัวอักษร",
    ACCOUNT_MY_ORDERRETURNPRODUCT_QUANTITY: "กรุณากรอกจำนวนเงินที่ได้รับคืน",
    ACCOUNT_MY_ORDERRETURNPRODUCT_REASON: "กรุณาเลือกสาเหตุการคืนเงิน",
    ACCOUNT_MY_ORDERRETURNPRODUCT_WRONGQUANTITY: "จำนวนสินค้าส่งคืนต้องน้อยกว่าจำนวนสินค้าที่สั่ง",
    ACCOUNT_My_OrderReturnProductInfo_complaintError: "ไม่เกิน 200 ตัวอักษร  คุณป้อนเกิน{0} ตัวอักษร",
    ACCOUNT_My_OrderReturnProductInfo_Quantity: "กรุณากรอกจำนวนเงินที่ได้รับคืน",
    ACCOUNT_My_OrderReturnProductInfo_QuantityError: "จำนวนสินค้าส่งคืนต้องน้อยกว่าจำนวนสินค้าที่สั่ง",
    ACCOUNT_My_OrderReturnProductInfo_Reason: "กรุณาเลือกสาเหตุการคืนเงิน",
    ACCOUNT_My_OrderReturnProductInfo_RRORPROMPT: "ดำเนินการล้มเหลว  กรุณาลองอีกครั้ง",
    ACCOUNT_MY_TRADECOMMENT_ATLEASTCOMMENT: "อย่างน้อยแสดงความคิดเห็นหนึ่ง",
    ACCOUNT_MY_TRADECOMMENT_COMMENTLEVEL: "กรุณาให้คะแนนความพอใจ",
    ACCOUNT_MY_TRADECOMMENT_COMMENTSUCCESS: "แสดงความคิดเห็นสำเร็จ",
    ACCOUNT_MY_TRADECOMMENT_MAXLENGTH: "ไม่เกิน500ตัวตัวอักษร",
    ACCOUNT_MY_TRADECOMMENT_SCORE: "นาที",
    ACCOUNT_MY_TRADECOMMENT_SUBMITCOMMENT: "บันทึกความคิดเห็นสำเร็จ",
    ACCOUNT_MY_TRADECOMMENT_SUBMITFAIL: "ระบบไม่ว่าง  โปรดแสดงความคิดเห็นในภายหลังขอบคุณค่ะ!",
    ACCOUNT_MY_WEALTH_COPYCONTENT: "กรุณากดปุ่ม Ctrl + C เพื่อคัดลอกข้อมูล",
    ACCOUNT_MY_WEALTH_COPYSUCCESS: " ก๊อปปี้สำเร็จ",
    ACCOUNT_MY_WEALTH_CORRECTAMOUNTWITHDRAWAL: "กรุณากรอกจำนวนเงินถอนที่ถูกต้อง",
    ACCOUNT_MY_WEALTH_EMPTYMONEY: "กรุณากรอกจำนวนเงินถอนที่ถูกต้อง",
    ACCOUNT_MY_WEALTH_ENTERMONEY: "กรุณากรอกจำนวนเงินที่ถอน",
    ACCOUNT_MY_WEALTH_ENTERWITHDRAWALAMOUNT: "กรุณากรอกจำนวนเงินที่ถอน",
    ACCOUNT_MY_WEALTH_FAIL: "ถอนเงินล้มเหลว",
    ACCOUNT_MY_WEALTH_MONEYOUTOFRANGE: "จำนวนเงินที่ถอนต้องน้อยกว่ายอดเงินคงเหลือ",
    ACCOUNT_MY_WEALTH_NOCASHBALANCE: "จำนวนเงินที่ถอนต้องน้อยกว่ายอดเงินคงเหลือ",
    ACCOUNT_MY_WEALTH_RIGHTNOW: "ถอนเงินทันที",
    ACCOUNT_MY_WEALTH_THEREBALANCE: "ไม่พบยอดคงเหลือในบัญชี",
    ACCOUNT_MY_WEALTH_USERNOTEXIST: "ไม่พบยอดคงเหลือในบัญชี",
    ACCOUNT_MY_WEALTH_WAIT: "ถอนเงินอยู่…   กรุณารอสักครู่",
    ACCOUNT_MY_WEALTH_WITHDRAWALAMOUNTOPERATIONFAILED: "ถอนเงินล้มเหลว",
    ACCOUNT_MY_WEALTH_WITHDRAWALWAIT: "ถอนเงินอยู่  กรุณารอสักครู่",
    ACCOUNT_USERINFO_ADDRESS_AreYouDeleteAddress: "ต้องการลบที่อยู่สำหรับการจัดส่งนี้หรือไม่",
    ACCOUNT_USERINFO_ADDRESS_AreYouSetDefaultAddress: "ต้องการตั้งป็นที่อยู่สำหรับการจัดส่งหลักหรือไม่",
    ACCOUNT_USERINFO_ADDRESS_edit: "เขียนความคิดเห็น",
    ACCOUNT_USERINFO_INDEX_CHECKLOGINEMAIL: "ขออภัยค่ะ  ไม่พบหน้าเข้าสู่ระบบอีเมล์ที่เกี่ยวข้อง    กรุณาเช็คดูอีเมล์เอง ",
    ACCOUNT_USERINFO_INDEX_CORRECTEMAILADDRESS: "กรุณากรอกที่อยู่อีเมล์ที่ถูกต้อง",
    ACCOUNT_USERINFO_INDEX_ENTEREMAILADDRESS: "กรุณากรอกที่อยู่อีเมล์",
    ACCOUNT_USERINFO_INDEX_ENTERFULLBIRTHDAY: "กรุณากรอกวันเดือนปีเกิดที่ถูกต้อง",
    ACCOUNT_USERINFO_INDEX_ENTERNICKNAME: "กรุณากรอกนามแฝง",
    ACCOUNT_USERINFO_INDEX_ILLEGALCHARACTERS: "นามแฝงประกอบด้วยอักขระที่ผิดกฎหมาย",
    ACCOUNT_USERINFO_INDEX_NICKNAMEMAXIMUM: "นามแฝงต้องไม่เกิน 20 อักขระ",
    ACCOUNT_USERINFO_INDEX_SELECTADDRESS: "กรุณาเลือกที่อยู่",
    ACCOUNT_USERINFO_INDEX_SELECTGENDER: "กรุณาเลือกเพศ",
    ACCOUNT_USERINFO_INDEX_SELECTIMAGES: "กรุณาเลือกไฟล์รูปภาพแบบ jpg,gif,png,jpeg",
    ACCOUNT_USERINFO_INDEX_SENDIN: "ดำเนินการอยู่",
    ACCOUNT_USERINFO_INDEX_SENDSUCCESS: "บันทึกสำเร็จ",
    ACCOUNT_USERINFO_INDEX_UPLOADFAILED: "อัปโหลดล้มเหลว",
    ACCOUNT_USERINFO_PWDLENGTH: "รหัสผ่านประกอบด้วย 8-20 ตัวอักษร",
    ACCOUNT_USERINFO_PWDSUCCESS: "แก้ไขเรียบร้อยแล้ว  กรุณาเข้าสู่ระบบใหม่",
    Add: "เพิ่ม",
    Edit: "เขียนความเห็น",
    HOME_ACTIVE_DAY: "วัน",
    HOME_ACTIVE_DESCRIBED: "แย่งซื้อภายในเวลาจำกัด",
    HOME_ACTIVE_HOUR: "ชั่วโมง",
    HOME_ACTIVE_MINUTE: "นาที",
    HOME_ACTIVE_SECOND: "วินาที",
    HOME_SHOPPING_BUYCOUNT: "กรุณาเพิ่มจำนวนสินค้าที่สั่ง",
    HOME_SHOPPING_COLLECT: "จัดเก็บเป็นรายการโปรดแล้ว",
    HOME_SHOPPING_EXCEPTION: "สินค้าคงคลังผิดปกติ  ไม่สามารถสั่งซื้อได้",
    HOME_SHOPPINGCART_ATLEASTONEPRODUCT: "ต้องเลือกอย่างน้อยหนึ่งรายการ",
    HOME_SHOPPINGCART_EDITPRODUCTCOUNT: "จำนวนสินค้าที่สั่งต้องน้อยกว่าสินค้าคงคลัง  กรุณาแก้ไขจำนวนสินค้า",
    HOME_SHOPPINGCART_NOPRODUCT: "จำนวนสินค้าที่เลือกต้องไม่เป็น0",
    HOME_SHOPPINGCART_NOTENOUGH: "สินค้าคงคลังขาดแคลน",
    HOME_SHOPPINGCART_POSTAGE: "จัดส่งฟรี",
    HOME_SHOPPINGCART_PRODUCTCOUNTOUTOFRANGE: "จำนวนสินค้าที่สั่งต้องน้อยกว่าสินค้าคงคลัง",
    HOME_SHOPPINGCART_PRODUCTUNDERSHELVES: "สินค้าถูกลบออกแล้ว",
    INPUT_KEYWORDS: "กรุณากรอกคำหลัก",
    LOGIN_GETPASSWORD_CORRECTCODE: "กรุณากรอกรหัสยืนยันที่ถูกต้อง",
    LOGIN_GETPASSWORD_GETAGAIN: "หลัง{0}วินาที รับข้อความใหม่",
    LOGIN_GETPASSWORD_PWDATLEASTINCLUDE: "รหัสผ่านต้องประกอบด้วยตัวอักษรอังกฤษ ตัวเลข",
    LOGIN_GETPASSWORD_PWDCONFIRM: "กรุณากรอกรหัสผ่านที่ยืนยันแล้ว",
    LOGIN_GETPASSWORD_PWDDIFFERENT: "รหัสผ่านไม่ถูกต้อง",
    LOGIN_GETPASSWORD_PWDFORMAT: "รหัสผ่านประกอบด้วยภาษาอังกฤษ ตัวเลข และ อักขระพิเศษ  8- 16 ตัวอักษร โดยแยกตัวอักษรเลขกับตัวอักษรใหญ่",
    LOGIN_GETPASSWORD_SENDAGAIN: "ส่งอีกคั้ง",
    LOGIN_GETPASSWORD_UNREGIST: "หมายเลขโทรศัพท์นี้ยังไม่สมัคร",
    LOGIN_GETPASSWORD_WRONGCODE: "รหัสยืนยันไม่ถูกต้อง",
    LOGIN_GETSUCCESS_SECOND: "วินาที",
    LOGIN_INDEX_CORRECTACCOUNT: "กรุณากรอกหมายเลขโทรศัพท์",
    LOGIN_INDEX_CORRECTACCOUNTPWD: "กรุณากรอกหมายเลขโทรศัพท์และรหัสผ่าน",
    LOGIN_INDEX_CORRECTPHONE: "กรุณากรอกหมายเลขโทรศัพท์ที่ถูกต้อง",
    LOGIN_INDEX_CORRECTPWD: "กรุณากรอกรหัสผ่าน",
    Modify: "แก้ไข",
    MONEY_ORDER_ADDRECEIVEADDRESS: "กรุณาเพิ่มที่อยู่สำหรับการจัดส่ง",
    MONEY_ORDER_AGREENOTRETURN: "โปรดยอมรับชำระเงินมัดจำก่อน (ไม่คืน)",
    MONEY_ORDER_DELETEFAIL: "ลบข้อมูลล้มเหลว กรุณารีเฟรชอีกครั้ง",
    MONEY_ORDER_EDIT: "เขียนความเห็น",
    MONEY_ORDER_INFO: "คำเตือน",
    MONEY_ORDER_INPUTCHECKOUTTEXT: "กรุณากรอกซื่อ นามสกุลผู้รับเงินลงในใบแจ้งหนี้",
    MONEY_ORDER_INPUTPAYPASSWORD: "กรุณากรอกรหัสชำระเงิน",
    MONEY_ORDER_NETERROR: "เชื่อมต่อเครือข่ายล้มเหลว   กรุณาตรวจสอบสถานะการเชื่อมต่ออินเทอร์เน็ตหรือ login ภายหลัง   ขอบคุณค่ะ",
    MONEY_ORDER_ORDERINFO_DELETEFAILED: "ลบข้อมูลล้มเหลว กรุณารีเฟรชอีกครั้ง",
    MONEY_ORDER_ORDERINFO_DEPOSIT: "โปรดยอมรับชำระเงินมัดจำก่อน (ไม่คืน)",
    MONEY_ORDER_ORDERINFO_GOODSORDER: "สินค้าบางชิ้นในรายการสั่งซื้อเป็นสินค้าขาดแคลน",
    MONEY_ORDER_ORDERINFO_GOODSSHELVES: "สินค้าบางชิ้นในรายการการสั่งซื้อถูกลบออกแล้ว",
    MONEY_ORDER_ORDERINFO_HAVESHELVES: "สินค้าถูกลบออกแล้ว",
    MONEY_ORDER_ORDERINFO_INSUFFICIENT: "สินค้าคงคลังขาดแคลน",
    MONEY_ORDER_ORDERINFO_INVOICE: "กรุณากรอกซื่อ นามสกุลผู้รับเงินลงในใบแจ้งหนี้",
    MONEY_ORDER_ORDERINFO_NETWORKSTATE: "เชื่อมต่อเครือข่ายล้มเหลว   กรุณาตรวจสอบสถานะการเชื่อมต่ออินเทอร์เน็ตหรือ login ภายหลัง   ขอบคุณค่ะ",
    MONEY_ORDER_ORDERINFO_NOTORDER: "กรุณาอย่าสั่งซ้ำ",
    MONEY_ORDER_ORDERINFO_PAYMENTMETHOD: "กรุณาเลือกช่องทางการชำระเงิน",
    MONEY_ORDER_ORDERINFO_QUANTITY: "จำนวนสินค้าต้องไม่เป็น 0",
    MONEY_ORDER_ORDERINFO_SHIPPADDRESS: "กรุณาเลือกที่อยู่สำหรับการจัดส่ง",
    MONEY_ORDER_ORDERINFO_SYSTEMBUSY: "ระบบไม่ว่าง  โปรดลองอีกครั้งในภายหลัง",
    MONEY_ORDER_ORDERINFO_TRANPASSWORD: "กรุณากรอกรหัสชำระเงิน",
    MONEY_ORDER_ORDERINFO_UNPAIDORDERS: "เฉพาะการสั่งซื้อที่ค้างชำระเท่านั้นสามารถชำระใหม่",
    MONEY_ORDER_ORDERNOPRODUCT: "ยังไม่มีสินค้า ดำเนินการต่อไปไม่ได้",
    MONEY_ORDER_ORDERPRODUCTZERO: "จำนวนสินค้าต้องไม่เป็น 0",
    MONEY_ORDER_OTHERAREA: "อื่นๆ",
    MONEY_ORDER_SAMEORDER: "กรุณาอย่าสั่งซ้ำ",
    MONEY_ORDER_SELECTCHANNEL: "กรุณาเลือกช่องทางการชำระเงิน",
    MONEY_ORDER_SELECTPLEASE: "กรุณาเลือก",
    MONEY_ORDER_SELECTRECEIVEADDRESS: "กรุณาเลือกที่อยู่สำหรับการจัดส่ง",
    MONEY_ORDER_SUBMITING: "บันทึกข้อมูลอยู่…",
    MONEY_ORDER_SYSTEMERROR: "ระบบไม่ว่าง  โปรดลองอีกครั้งในภายหลัง",
    MONEY_ORDER_SETPAYPASSWORD: "กรุณาตั้งรหัสชำระเงิน",
    MONEY_ORDER_INSUFFICIENT_BALANCE: "ยอดเงินคงเหลือไม่พอ",
    ORDER_LIST_JUMPTO: "กลับสู่หน้า...",
    ORDER_LIST_NEXTPAGE: "หน้าถัดไป",
    ORDER_LIST_PAGE: "หน้า",
    ORDER_LIST_PREVIOUSPAGE: "เพจก่อนหน้า",
    ORDER_LIST_SURE: "ยืนยัน",
    ORDER_LIST_CANCEL: "การยกเลิก",
    ORDER_LIST_TO: "ไป",
    ORDERRETURN_ATMOSTMONEY: "สูงสุด{0}",
    PLEASE_SELECT: "กรุณาเลือก",
    REGISTER_INDEX_AGGREMENT: "กรุณายอมรับข้อตกลงและเงื่อนไขการเป็นสมาชิก huika",
    REGISTER_INDEX_CORRECTEMAIL: "กรุณากรอกที่อยู่อีเมล์ที่ถูกต้อง",
    REGISTER_INDEX_INVITERNOTEXIST: "ผู้เชิญชวนไม่ถูกต้อง",
    REGISTER_INDEX_INVITERPHONEWRONG: "เบอร์โทรศัพท์มือถือของผู้เชิญชวนไม่ถูกต้อง",
    REGISTER_INDEX_REGISTED: "หมายเลขโทรศัพท์นี้ได้สมัครแล้ว",
    HOME_SHOPPINGCART_DELETEGOOD: "คุณต้องการลบสินค้านี้ ?",
    HOME_SHOPPINGCART_DELETEALLGOODS: "คุณแน่ใจว่าต้องการลบรายการที่เลือก ?",
    ACCOUNT_MY_ORDERPRODUCTLISTT2_CONFIMREFUND: "ต้องการถอนคำร้องขอหรือไม่",
    LOGIN: "เข้าระบบ",
    DURING_LOGIN: "loginอยู่...",
    REGISTERNOW: "สมัครทันที",
    UPLOAD_TEXT: "ภาพที่อัปโหลด",
    UPLOAD_FLASH_CONTROL: "คุณยังไม่ได้ติดตั้งโปรแกรม FLASH จึงไม่สามารถอัปโหลดภาพ! กรุณาติดตั้งแล้วลองใหม่อีกครั้ง",
    UPLOAD_SELECT_LIMIT: "จำนวนไฟล์เกินขีดจำกัด สูงสุดไม่เกิน 1ไฟล์",
    UPLOAD_SIZE_LIMIT: "ขนาดไฟล์ไม่เกิน{0} กรุณาอัปโหลดอีกครั้ง",
    UPLOAD_WROMH_SIZE: "ขนาดไฟล์ผิดปกติ",
    UPLOAD_WRONG_TYPE: "ลักษณะไฟล์ไม่ถูกต้อง",
    UPLOAD_TIP_ONE: "รองรับไฟล์ที่jpg,png,gif,jpeg ขนาดไฟล์ไม่เกิน2 M",
    UPLOAD_TIP_TWO: "แนะนำขนาดไฟล์ 300*300 พิกเซล",
    INPUT_PHONECODE: "กรุณากรอกรหัสยืนยัน",
    INPUT_OLD_LOGINPWD: "กรุณากรอกรหัสผ่านปัจจุบัน",
    INPUT_NEW_LOGINPWD: "กรุณากรอกรหัสผ่าน",
    INPUT_OLD_JIAOYIMIMA: "กรุณากรอกรหัสชำระเงินปัจจุบัน",
    PAYPASSWORD_PWDFORMAT: "รหัสผ่านประกอบด้วยภาษาอังกฤษ ตัวเลข และ อักขระพิเศษ  8- 16 ตัวอักษร โดยแยกตัวอักษรเลขกับตัวอักษรใหญ่",
    HOME_INDEX_START: "เริ่ม",
    HOME_INDEX_SHENGYU: "เหลือ",
    HOME_INDEX_OVER: "สิ้นสุด",
    SUBMIT_SUCCESS: "ส่งเรียบร้อยแล้ว",
    SUBMIT_FAIL: "การส่งล้มเหลว",
    ENETR_CONSU_CONTENT: "กรุณากรอกเนื้อหาการให้คำปรึกษา",
    LOGIN_FIRST: "กรุณาเข้าสู่ระบบ",
    SHOPPINGCART_WUHUO: "หมดสต็อก",
    SHOPPINGCART_YIXIAJIA: "ถูกลบแล้ว",
    YEAR: "ปี",
    MONTH: "เดือน",
    DAY: "วัน",
    LanguageID: "3",
    HOME_SHOPPINGCART_CARTISNULL: "ตะกร้าของคุณว่างเปล่า  ไปดูสินค้าที่คุณชื่นชอบ",
    HOME_SHOPPINGCART_GOSHOPPING: "ไปช้อปปิ้ง",
    MESSAGEBOX_SURE: "ยืนยัน",
    MESSAGEBOX_CANCEL: "ยกเลิก",
    SIMPLE_SELECT: "กรุณาเลือก",
});
$commonLang = commonLang;



