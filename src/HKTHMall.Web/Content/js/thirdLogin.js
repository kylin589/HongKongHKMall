var isRequestLogin = false;
// This is called with the results from from FB.getLoginStatus().
function statusChangeCallback(response) {
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook.
        FacebookLoginAPI();
    } else if (response.status === 'not_authorized') {
        // The person is logged into Facebook, but not your app.
        document.getElementById('status').innerHTML = 'Please log ' + 'into this app.';
    } else {
        // The person is not logged into Facebook, so we're not sure if
        // they are logged into this app or not.
        //document.getElementById('status').innerHTML = 'Please log ' + 'into Facebook.';
    }
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

window.fbAsyncInit = function () {
    FB.init({
        //appId: '523278857828617',//测试环境
        appId: '1172545032773389',//正式环境
        xfbml: true,
        version: 'v2.5'
    });
    setLock(false);
};


//加载
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


//登陆
function FacebookLoginAPI() {
    FB.api('/me', function (response) {
        console.log("response:" + response);
        if (response) {
            loginThirdUser(response.id, response.name, 3);
        }
    });

}

//第三方登陆
function loginThirdUser(id, name, type) {
    if (id == null || name == null) {
        return;
    }
    $.ajax({
        url: "/login/ThirdLogin", type: "POST", data: { id: id, name: name, sourceType: type }, success: function (data) {
            if (data.status) {
                window.location.href = "/";
            } else {
                alert(data.msg);
            }
        }
    });
}
setLock(true);

$(function () {
    $(".m_login_face").click(function () {
        if (!isRequestLogin) {
            setLock(true);
            FB.getLoginStatus(function (response) {
                if (response.status === 'connected') {
                    FacebookLoginAPI();
                    setLock(false);
                } else {
                    FB.login(function (response) {
                        if (response.status == 'connected') {
                            FacebookLoginAPI();
                        }
                        setLock(false);
                    });
                }
            });
        }
        //if (!isRequestLogin) {
        //    setLock(true);
        //    FB.login(function (response) {
        //        if (response.status == 'connected') {
        //            FacebookLoginAPI();
        //        }
        //        setLock(false);
        //    });
        //}
    });
})

function setLock(lock) {
    if (lock) {
        isRequestLogin = true;
        $("#sFaceBook").html("Loading...");
    } else {
        isRequestLogin = false;
        $("#sFaceBook").html("Facebook");
    }
}