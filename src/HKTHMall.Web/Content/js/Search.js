/// <reference path="jquery-1.9.1.min.js" />
//头搜索功能

$(function () {
    search();
})

function search()
{
    
   
    $('#btnHeadSearch').click(function () {
        var searchKey = $('#txtSearchKey').val();
        if(searchKey==undefined||searchKey==""||searchKey.trim()=="")
        {
            return;
        }
        window.location.href = "/Product/productlist?key=" + searchKey;
    });



}
