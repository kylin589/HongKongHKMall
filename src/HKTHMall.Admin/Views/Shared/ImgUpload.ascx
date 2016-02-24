<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%--<script src="../../Content/js/jquery/jquery-1.10.2.min.js"></script>
<script src="../../Content/js/ajaxfileupload.js"></script>--%>
<script type="text/javascript">

    function Getup() {
        var name = $("#upLoad").val();
        if (name == "") {
            alert("Select the file to upload.");
        } else {
            var AppType=1;
            try {
                AppType=  $('#Platform').val();
            } catch (e) {
                AppType = 1;
            }
            Tool.Alert("Is uploading, please be patient!");

            $.ajaxFileUpload(
            {
                url: '/Upload/UploadFileds?AppType=' + AppType,
                secureuri: false,
                fileElementId: "upLoad",
                fileTypeExts: '*.apk;*.ipa;',
                fileTypeDesc: 'Please select apk, ipa format',
                dataType: "json",

                success: function(data, status) {

                    
                    GetUrl(data); //使用该控件,请初始此方法
                },
                error: function(data, status, e) {
                    Tool.Alert("Upload failed!", 1500);
                    console.log( e);
                    
                }
            });
        }
    }

   
</script>



<input id="upLoad" name="upLoad" type="file"  value="Select picture" />

<input id="btnUpLoad" onclick="Getup()" type="button" value="Upload"/>
