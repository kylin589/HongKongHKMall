$(function () {
    if ($('#logindialog').length > 0) {
        $('#logindialog').click(function () {
            ($(this).hasClass("clicked")) ? removeClick($(this)) : dialogLogin_click($(this));
        });
    }
})


//验证邮箱
//function CheckEmail() {
//    var email = $("#email").val();
//    alert(email);
//    if (/^\w+([-+.]\w+)*@@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email) == false || email == '') {
//        $("#errorStr").css("display", "block");
//        return false;
//    } else {
//        $("#errorStr").css("display", "none");
//        return true;
//    }
//}

//验证邮箱
function CheckEmail() {
    var email = $("#email").val();
    var filter = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;

    if (email == '') {
        $("#errorStr").html('请填写邮箱！');
        $("#errorStr").css("display", "block");
        return false;
    }
    else if (!filter.test(email)) {
        $("#errorStr").html('请填写正确的邮箱！');
        $("#errorStr").css("display", "block");
    }

    else {
        $("#errorStr").html('请填写正确的邮箱！');
        $("#errorStr").css("display", "none");
        return true;
    }
}








