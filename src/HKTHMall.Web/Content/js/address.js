var _htmlEDIT = $("#addressDialog");

function ValidateText() {
    $(".address_arr").hide();
    $("#pEmail").show();
    var count = 0;
    var txtReceiver = $("#txtReceiver").val();
    var slQu = $("#slQu").val();
    var txtDetailsAddress = $("#txtDetailsAddress").val();
    var txtMobile = $("#txtMobile").val();
    var txtPhone = $("#txtPhone").val();

    if (txtReceiver == "" || txtReceiver == null) {
        $("#pReceiver").show();
        count++;
    } else {
        $("#successReceiver").show();
    }
    if (slQu == "-1") {
        $("#pTHAreaID").show();
        count++;
    } else {
        $("#successTHAreaID").show();
    }
    if (txtDetailsAddress == "" || txtDetailsAddress == null) {
        $("#pDetailsAddress").show();
        count++;
    } else {
        $("#successDetailsAddress").show();
    }
    if (txtMobile == "" || txtMobile == null) {
        $("#pMobile").show();
        count++;
    }
 
    if (count > 0) {
        return false;
    } else {
        return true;
    }
}

function SelectChange(parentID, name) {
    var html = "<option value='-1'>-" + $commonLang.SIMPLE_SELECT + "-</option>";
    $.ajax({
        url: "/UserInfo/GetTHAreaSelect",
        async: false,
        data: {
            parentID: parentID,
            time: new Date().getTime()
        },
        dataType: "text",
        success: function (data, status) {
            data = JSON.parse(data);
            $.each(data, function (i, item) {
                html += "<option value='" + item["THAreaID"] + "'>" + item["AreaName"] + "</option>";
            });
            $("#" + name).html(html);
        }
    });
}

function DelUserAddress(userAddressId) {
    _htmlADD.html($commonLang.ACCOUNT_USERINFO_ADDRESS_AreYouDeleteAddress);
    ds.dialog({
        //title : '消息提示',
        content: _htmlADD,
        yesText: $commonLang.HOME_SHOPPING_SURE,
        onyes: function () {
            $.ajax({
                url: "/UserInfo/DelUserAddress",
                data: {
                    userAddressId: userAddressId,
                },
                dataType: "text",
                success: function (data, status) {
                    data = JSON.parse(data);
                    if (data.IsValid) {
                        showDailog(data.Messages);
                    } else {
                        showDailog(data.Messages);
                    }
                }
            });
            this.close();
        },
        noText: $commonLang.ACCOUNT_MY_COLLECTION_CANCEL,
        onno: function () {
            this.close();
        },

    });

}

function ShowEditAddressById(userAddressId) {
    ResetMessage(_htmlEDIT);
    $.ajax({
        url: "/UserInfo/GetUserAddressById",
        data: {
            userAddressId: userAddressId,
            time: new Date().getTime()
        },
        dataType: "text",
        success: function (data, status) {
            data = JSON.parse(data);
            var countryTHAreaID = "";
            var shengTHAreaID = "";
            var shiTHAreaID = "";
            var tHAreaID = "";
            $.each(data, function (i, item) {
                countryTHAreaID = item["CountryTHAreaID"];
                shengTHAreaID = item["ShengTHAreaID"];
                shiTHAreaID = item["ShiTHAreaID"];
                tHAreaID = item["THAreaID"];
                _htmlEDIT.find("#txtReceiver").val(item["Receiver"]);
                _htmlEDIT.find("#txtDetailsAddress").val(item["DetailsAddress"]);
                _htmlEDIT.find("#txtMobile").val(item["Mobile"]);
                _htmlEDIT.find("#txtEmail").val(item["Email"]);
                _htmlEDIT.find("#txtPhone").val(item["Phone"]);
                //$("#txtPostalCode").val(item["PostalCode"]);
                _htmlEDIT.find("#txtUserAddressId").val(item["UserAddressId"]);
                ds.dialog({
                    title: $commonLang.HOME_SHOPING_UPDATEADDRESS,
                    content: _htmlEDIT,
                    tijiao: function () {

                    }
                });
                $("#mask").show().fadeIn();
            });
            SelectChange(countryTHAreaID, "slSheng");
            SelectChange(shengTHAreaID, "slShi");
            SelectChange(shiTHAreaID, "slQu");
            _htmlEDIT.find("#slCountry").val(countryTHAreaID);
            _htmlEDIT.find("#slSheng").val(shengTHAreaID);
            _htmlEDIT.find("#slShi").val(shiTHAreaID);
            _htmlEDIT.find("#slQu").val(tHAreaID);
        }
    });
}

function UpdateUserAddressFlag(userAddressId, userId) {
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
                $("#ShippingAddress").hide().stop();
                $("#mask").hide().stop();
                showDailog(data.Messages);
            } else {
                mallbox.alert({ message: data.Messages });
            }
        }
    });
}

function ResetMessage(hHTML) {
    hHTML.find("#slCountry").val('-1');
    hHTML.find("#slSheng").html("<option value='-1'>-" + $commonLang.SIMPLE_SELECT + "-</option>");
    hHTML.find("#slShi").html("<option value='-1'>-" + $commonLang.SIMPLE_SELECT + "-</option>");
    hHTML.find("#slQu").html("<option value='-1'>-" + $commonLang.SIMPLE_SELECT + "-</option>");
    hHTML.find("input[type='text']").val("");
    hHTML.find(".address_arr").hide();
    hHTML.find(".true").hide();
    hHTML.find("#pEmail").show();
}

function CheckValue(name) {
    var value = $("#txt" + name).val();
    if (value == "" || value == null || value == "-1") {
        $("#p" + name).show();
        $("#success" + name).hide();
    } else {
        $("#success" + name).show();
        $("#p" + name).hide();
    }
}

//else {
//    if (!txtMobile.match(/^[0][6||8-9][0-9]{8}$/)) {
//        $("#pMobileError").show();
//        $("#successMobile").hide();
//        $("#pMobile").hide();
//    } else {
//        $("#successMobile").show();
//        $("#pMobile").hide();
//        $("#pMobileError").hide();
//    }
//}