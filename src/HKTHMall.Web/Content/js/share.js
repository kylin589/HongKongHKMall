//facebook初始化
window.fbAsyncInit = function () {
    FB.init({
        appId: '1172545032773389',//正式
        //appId: '181741635499666',//测试
        //appId: '912090465506339',//正式
        status: true,
        cookie: true,
        xfbml: true,
        oauth: true,
        version: 'v2.5' // use version 2.2
    });
};

// Load the SDK asynchronously
//英文：en_US
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.async = true;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function shareToTwitter(desc,url)
{
    window.open('http://twitter.com/home/?status='.concat(encodeURIComponent(desc)).concat(' ').concat(encodeURIComponent(url)));
}
function shareToGooglePlus(url)
{
    window.open('https://plus.google.com/share?url='.concat(encodeURIComponent(url)))
}

function shareToPlurk(desc,url)
{
    window.open('http://www.plurk.com/?qualifier=shares&status='.concat(encodeURIComponent(url)).concat(' ').concat('&#40;').concat(encodeURIComponent(desc)).concat('&#41;'))
}

function shareToYoutube(desc, url)
{

}