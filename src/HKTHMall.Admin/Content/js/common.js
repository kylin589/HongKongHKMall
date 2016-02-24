var Tool = {};

Tool.ShowModal = function (url, param, title) {
    /// <summary>弹出表单</summary>
    /// <param name="url" type="string">url地址</param>  
    /// <param name="param" type="array">参数</param>
    /// <param name="title" type="string">标题</param>
    

    $(".modal-title").html(title);
    $("#modal-content").attr("action", url);
    $.ajax({
        type: "GET",
        url: url,
        data: param,
        beforeSend: function () {
        },
        success: function (result) {
            $("#modal-content").html(result);
            $('#modal-form').modal({ backdrop: 'static', show: true, width: 'auto' });

            Tool.RegisterForm();

            if (typeof (initFormSomething) == 'function') {
                initFormSomething();
            }

        },
        error: function () {
        },
        complete: function () {
        }
    });
}

Tool.ShowPrint = function (url, param, title) {
    /// <summary>弹出表单</summary>
    /// <param name="url" type="string">url地址</param>  
    /// <param name="param" type="array">参数</param>
    /// <param name="title" type="string">标题</param>


    //$(".modal-title").html(title);
    //$("#modal-content").attr("action", url);
    $.ajax({
        type: "GET",
        url: url,
        data: param,
        beforeSend: function () {
        },
        success: function (result) {

            bootbox.dialog({
                title: title,
                message: result,
                width:200,
                buttons: {
                    cancel: {
                        label: "Cancel",
                        className: "btn-default",
                    },
                    success: {
                        label: "Print",
                        className: "btn-default btn-success",
                        callback: function () {
                            Print();
                        }
                    }
                    
                }
            });


            //$("#modal-content").html(result);
            //$('#modal-form').modal({ backdrop: 'static', show: true, width: 'auto' });

            //Tool.RegisterForm();

            

        },
        error: function () {
        },
        complete: function () {
        }
    });
}


Tool.CloseModal = function () {
    /// <summary>关闭弹出框</summary>
    $('#modal-form').modal('hide');
    Tool.ClearForm($("#modal-content"));
}

Tool.ClearForm = function (modal) {
    /// <summary>清除表单数据</summary>
    /// <param name="modal" type="object">url地址</param>  
    modal.find(':input').not(':button, :submit, :reset').val('').removeAttr('checked').removeAttr('selected');
    try {
        modal.find('select').select2("destroy");
    } catch (e) {

    }
}

Tool.SaveModal = function (oTable) {
    /// <summary>保存表单</summary>
    /// <param name="oTable" type="object">bootstrap-table对象</param>  
    var url = $("#modal-content").attr("action");
    var $form = $("#modal-content");

    if (typeof (setFormValues) != "undefined") {
        setFormValues();
    }

    if (!$form.valid()) {
        return;
    }
    $('body').showLoading();
    $.ajax({
        type: "POST",
        url: url,
        data: $form.serialize(),
        success: function (data) {
            $('body').hideLoading();
            
            //判断返回值,若为Object类型,即操作成功
            var result = ((typeof data == 'object') && (data.constructor == Object));
            if (result) {
                // bootbox.alert(data.Messages[0]);
                Tool.Alert(data.Messages[0], 1500);//modified by jimmy,2015-7-23
                if (data.IsValid == false) {
                    return;
                }
                $('#modal-form').modal('hide');
                if (oTable) {
                    Tool.ReloadDataTable(oTable);
                }
                if (typeof (customCallBack) != "undefined") {
                    customCallBack();
                }
            }
            else {
                $("#modal-content").html(data);
            }
        }
    });
}


Tool.DeleteRecord = function (url, param, oTable) {
    /// <summary>删除操作</summary>
    /// <param name="url" type="string">url地址</param>  
    /// <param name="param" type="array">参数</param>
    /// <param name="oTable" type="string">bootstrap-table对象</param>


    bootbox.dialog({
        message: "Delete this record？",
        title: 'Message',
        buttons: {
            cancel: {
                label: "Cancel",
                className: "btn-default"
            },
            success: {
                label: "Confirm",
                className: "btn-primary",
                callback: function () {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: param,
                        beforeSend: function () {
                            //
                        },
                        success: function (result) {
                            // bootbox.alert(result.Messages[0]);
                            Tool.Alert(result.Messages[0], 1500);//modified by jimmy,2015-7-23
                            if (result.IsValid == true) {
                                if (oTable) {
                                    Tool.ReloadDataTable(oTable);
                                }
                                if (typeof (customCallBack) != "undefined") {
                                    customCallBack();
                                }
                            }
                        },
                        error: function () {
                            //
                        },
                        complete: function () {
                            //
                        }
                    });
                }
            }
        }
    });

}

Tool.OperationRecord = function (url, param, Message, oTable) {
    /// <summary>删除操作</summary>
    /// <param name="url" type="string">url地址</param>  
    /// <param name="param" type="array">参数</param>
    /// <param name="oTable" type="string">bootstrap-table对象</param>


    bootbox.dialog({
        message: Message,
        buttons: {
            cancel: {
                label: "Cancel",
                className: "btn-default"
            },
            success: {
                label: "Sure",
                className: "btn-primary",
                callback: function () {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: param,
                        beforeSend: function () {
                            //
                        },
                        success: function (result) {
                            // bootbox.alert(result.Messages[0]);
                            Tool.Alert(result.Messages[0], 1500);//modified by wuyf,2015-7-24
                            if (result.IsValid == true) {
                                if (oTable) {
                                    Tool.ReloadDataTable(oTable);
                                }
                                if (typeof (customCallBack) != "undefined") {
                                    customCallBack();
                                }
                            }
                        },
                        error: function () {
                            //
                        },
                        complete: function () {
                            //
                        }
                    });
                }
            }
        }
    });

}

Tool.ReloadDataTable = function (oTable) {
    /// <summary>刷新表格</summary>
    /// <param name="obj" type="string">bootstrap-table对象</param>  
    oTable.bootstrapTable('refresh');
}



Tool.RegisterForm = function () {
    /// <summary>注册验证脚本</summary>
    $('#modal-content').removeData('validator');
    $('#modal-content').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('#modal-content');
}

Tool.Alert = function (message, time, callback) {
    /// <summary>弹窗</summary>
    /// <param name="message" type="string">显示文本</param>  
    /// <param name="time" type="int">多长时间自动关闭,默认为0,不自动关闭</param>  
    /// <param name="callback" type="Funcation">返回函数</param>  
    var buttons = {
        ok: {
            label: 'Sure',
            callback: function () {
                if (typeof callback == 'function') {
                    callback();
                }
            }
        }
    };
    //var alertObj = bootbox.alert({ message: message, buttons: buttons });

    var alertObj = bootbox.dialog(
    {
        title: 'Message',
        message: message,
        buttons: buttons
    });
    if (time) {
        var _timer = setInterval(function () {
            alertObj.modal("hide");
            if (typeof callback == 'function') {
                callback();
            }
            window.clearInterval(_timer);
        }, 2000);
    }

}

Tool.ImageUpload = function (selector, callback) {
    /// <summary>图片上传</summary>
    /// <param name="selector" type="string">文件域选择器</param>  
    /// <param name="callback" type="function">上传成功后的回调函数</param> 
    var value = $(selector).val();
    if (value == "") {
        Tool.Alert("Please select an image to upload！", 1500);
        return;
    }

    $.ajaxFileUpload(
        {
            url: '/Upload/UploadImage',              //用于文件上传的服务器端请求地址
            secureuri: false,                       //是否需要安全协议,一般设置为false
            fileElementId: selector.substring(1),   //文件上传域的ID
            dataType: 'json',                       //返回值类型 一般设置为json
            success: function (data, status)        //服务器成功响应处理函数
            {
                //上传失败
                if (data && !data.IsValid) {
                    Tool.Alert(data.Messages[0], 2000);
                } else if (data && data.IsValid) {
                    if (typeof callback == "function") {
                        data.Selector = selector;
                        callback(data);
                    } else {
                        Tool.Alert('Upload successful');
                    }
                } else {
                    Tool.Alert('upload failed');
                }
            },
            error: function (data, status, e)       //服务器响应失败处理函数
            {
                Tool.Alert('upload failed' + e + '！');
            }
        }
    );
    return false;
}

//检查权限
Tool.CheckPermission = function (result) {
    //alert(1);
    if (result.status == "NoLanding") {

        Tool.Alert('Not logged in! Please login system!', 1000, function () {
            window.location = '/AC_User/login';
        });

        //layer.alert('未登录！请登录系统!', 7, function loginBack() { window.location = '/AC_User/login'; });
        return false;
    }
    if (result.status == "NOT_AUTHORIZED") {
        //layer.alert('无权限操作！请联系系统管理员!', 7);
        Tool.Alert('No permission to operate! Please contact your system administrator!')
        return false;
    }
    return true;
}
function CheckPermissions(result) {
    if (result.status == "NoLanding") {

        Tool.Alert('Not logged in! Please login system!', 1000, function () {
            window.location = '/AC_User/login';
        });

        //layer.alert('未登录！请登录系统!', 7, function loginBack() { window.location = '/AC_User/login'; });
        return false;
    }
    if (result.status == "NOT_AUTHORIZED") {
        //layer.alert('无权限操作！请联系系统管理员!', 7);
        Tool.Alert('No permission to operate! Please contact your system administrator!')
        return false;
    }
    return true;
}

$(function () {
    /** 
* 从对象数组中删除属性为objPropery,值为objValue元素的对象 
* @param Array arrPerson 数组对象 
* @param String objPropery 对象的属性 
* @param String objPropery 对象的值 
* @return Array 过滤后数组 
*/
    $.fn.removeArray = function (objPropery, objValue) {
        var arrPerson = this;
        return $.grep(arrPerson, function (cur, i) {
            return cur[objPropery] !== objValue;
        });
    }
    /** 
* 从对象数组中获取属性为objPropery,值为objValue元素的对象 
* @param Array arrPerson 数组对象 
* @param String objPropery 对象的属性 
* @param String objPropery 对象的值 
* @return Array 过滤后的数组 
*/
    $.fn.getArray = function (objPropery, objValue) {
        var arrPerson = this;
        return $.grep(arrPerson, function (cur, i) {
            return cur[objPropery] === objValue;
        });
    }

    $.fn.IsArray = function (objPropery, objValue) {
        var arrPerson = this;
        var b = false;
        $(arrPerson).each(function () {
            if (this[objPropery] == objValue) {
                b = true;
                return true;
            }
        });
        return b;
    }

    ///去掉重复
    $.fn.uniqueArray = function (objPropery) {
        var array = this;
        var values = [];
        return $.grep(array, function (ctx, i) {
            if ($.inArray(ctx[objPropery], values) !== -1) {
                return false;
            }
            values.push(ctx[objPropery]);
            return true;
        });
    }
    /** 
    * 显示对象数组信息 
    * @param String info 提示信息 
    * @param Array arrPerson 对象数组 
    */
    $.fn.showPersonInfo = function (info) {
        var arrPerson = this;
        $.each(arrPerson, function (index, callback) {
            info += "Person id:" + arrPerson[index].id + " name:" + arrPerson[index].name + " sex:" + arrPerson[index].sex + " age:" + arrPerson[index].age + "\r\t";
        });
    }

    ///dialog 表单
    $.fn.hkdialog = function (options) {
        var opts = {
            submittitle: 'Save',
            closetitle: 'Close',
            $dialog: this,
            submit: function () {
                return true;
            }
        }

        opts = $.extend(opts, options);
        if (opts.$dialog) {
            opts.$dialog.dialog("destroy");
        }
        opts.$dialog.dialog({
            title: opts.title,
            onClose: function () {
                opts.$dialog.dialog("destroy");
            },
            buttons: [
                {
                    text: opts.submittitle
                    , 'class': "btn-success"
                    , click: function () {
                        //debugger 
                        $(this).blur();
                        if (opts.submit()) {
                            $(opts.form).hkform({
                                type: 'post',
                                success: opts.success
                            });
                        }
                    }
                },
                {
                    text: opts.closetitle
                    , 'class': 'btn-primary'
                    , click: function () {
                        opts.$dialog.dialog("destroy");
                    }
                }
            ]
        });

        //opts.$dialog.on('hidden.bs.modal', function () {
        //    debugger;
        //});
        $.get(opts.url, opts.data, function (html) {
            opts.$dialog.html(html);
        });
        return opts.$dialog;
    }
    //表单
    $.fn.hkform = function (options) {
        var opts = {
            type: 'get',
            dataType: 'json',
            async:false,
            $form: this
        }
        opts = $.extend(opts, options);
        var rul = opts.$form.attr('action');
        $.ajax({
            type: opts.type,
            dataType: opts.dataType,
            url: rul,
            data: opts.$form.serialize(),
            success: opts.success
        });
        return opts.$form;
    }

    $.fn.doExchange = function () {
        var doubleArrays = this;
        var len = doubleArrays.length;
        if (len >= 2) {
            var arr1 = doubleArrays[0];
            var arr2 = doubleArrays[1];
            var len1 = doubleArrays[0].length;
            var len2 = doubleArrays[1].length;
            var newlen = len1 * len2;
            var temp = new Array(newlen);
            var index = 0;
            for (var i = 0; i < len1; i++) {
                for (var j = 0; j < len2; j++) {
                    temp[index] = { 'SkuName': arr1[i].SkuName + "," + arr2[j].SkuName, 'SKUStr': arr1[i].SKUStr + "_" + arr2[j].SKUStr }
                    index++;
                }
            }
            var newArray = new Array(len - 1);
            newArray[0] = temp;
            if (len > 2) {
                var _count = 1;
                for (var i = 2; i < len; i++) {
                    newArray[_count] = doubleArrays[i];
                    _count++;
                }
            }
            //console.log(newArray);
            return $(newArray).doExchange();
        } else {
            return doubleArrays[0];
        }
    }

    $.fn.uploadFile = function (done) {
        this.fileupload({
            url: '/Upload/UploadImage',
            dataType: 'json',
            autoUpload: true,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 9990000,
            sequentialUploads: false,
            disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
            previewMaxWidth: 100,
            previewMaxHeight: 100,
            previewCrop: true,
            done: done
        });
    }

    // ----------------------------------------------------------------------
    // <summary>
    // 限制只能输入数字
    // </summary>
    // ----------------------------------------------------------------------
    $.fn.onlyNum = function () {
        $(this).keypress(function (event) {
            var eventObj = event || e;
            var keyCode = eventObj.keyCode || eventObj.which;
            if ((keyCode >= 48 && keyCode <= 57))
                return true;
            else
                return false;
        }).focus(function () {
            //禁用输入法
            this.style.imeMode = 'disabled';
        }).bind("paste", function () {
            //获取剪切板的内容
            var clipboard = window.clipboardData.getData("Text");
            if (/^\d+$/.test(clipboard))
                return true;
            else
                return false;
        });
    };

    // ----------------------------------------------------------------------
    // <summary>
    // 限制只能输入双精度数字
    // </summary>
    // ----------------------------------------------------------------------
    $.fn.onlyDouble = function() {
        var $input = $(this);
        $(this).keypress(function(event) {
            var eventObj = event || e;
            var keyCode = eventObj.keyCode || eventObj.which;
            if ((keyCode >= 48 && keyCode <= 57) || keyCode == 46) {
                if (keyCode == 46 && $input.val().indexOf('.') != -1) {
                    return false;
                }
                if ($input.val().indexOf('.') != -1 && $input.val().substring($input.val().indexOf('.')).length > 2) {
                    return false;
                }
                return true;
            }
            else
                return false;
        }).focus(function() {
            //禁用输入法
            this.style.imeMode = 'disabled';
        }).bind("paste", function() {
            //获取剪切板的内容
            var clipboard = window.clipboardData.getData("Text");
            if (/^\d+$/.test(clipboard))
                return true;
            else
                return false;
        });
    };
    
    $.fn.stringToBytes = function() {
        var ch, st, re = [];
        for (var i = 0; i < this.length; i++) {
            ch = this.charCodeAt(i);  // get char   
            st = [];                 // set up "stack"  
            do {
                st.push(ch & 0xFF);  // push byte to stack  
                ch = ch >> 8;          // shift value down by 1 byte  
            }
            while (ch);
            // add stack contents to result  
            // done because chars have "wrong" endianness  
            re = re.concat(st.reverse());
        }
        // return an array of bytes  
        return re;
    }

});