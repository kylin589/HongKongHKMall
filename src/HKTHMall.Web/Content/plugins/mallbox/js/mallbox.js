(function (root, factory) {
    "use strict";
    if (typeof define === "function" && define.amd) {
        define(["jquery"], factory);
    } else if (typeof exports === "object") {
        module.exports = factory(require("jquery"));
    } else {
        root.mallbox = factory(root.jQuery);
    }
}(this, function init($, undefined) {
    /*var template = "<div><div class='mallbox' style='width: 300px; z-index: 9999; top: 38%;'>" +
        "<div class=' mallbox-main'>" +
        "<div class='mallbox-tit'>" +
        "<h2 class='float_l mallbox-title' style='font-size:14px;'>提示</h2><a href='javascript:;' class='close'></a>" +
        "</div>" +
        "<div class='mallbox-re' style=' background-color:white;'>" +
        "<p style='text-align:center;padding-top:10px;' class='mallbox-body' id='footermallbox-re'></p>" +
        "<p class='add-delte'><input type='button' value='取消'class='btn input-bd cancel hide'><input type='button' value='确定'class='redbutton btn input-bd ok' id='confirmButton'></p>" +
        "</div>" +
        "</div>" +
        "</div>" +
        "<div class='mallbox-mask mask' style='z-index: 98; opacity: 0.6; '></div></div>";*/
    var template = "<div class='mallbox'><div class='cb_add_tc'>" +
        "<div class='cb_popTitle mallbox-tit'>" +
        "<div class='cb_pop_cb_bottom'><b class='mallbox-title'>提示</b><span class='popclose close'>×</span></div>" +
        "</div>" +
        "<div class='cb_add_tc_bt mallbox-body'></div>" +
        "<div class='cb_add_tc_nr'>" +
        "<a href='javascript:;' class='cb_add_tc_nr1 ok'>确定</a>" +
        "<a href='javascript:;' class='cb_add_tc_nr2 cancel hide'>取消</a>" +
        "</div>" +
        "</div>" +
        "<div class='mallbox-mask mask' style='z-index: 98; opacity: 0.6; '></div></div>" +
        "</div>";
        

    var defaults = {
        locale: "zh_CN",
        container: "body",
        modal: true,
        type: 'alert'
    };

    /**
  * @private
  */
    function _t(key) {
        var locale = locales[defaults.locale];
        return locale ? locale[key] : locales.zh_CN[key];
    }

    var exports = {};

    exports.setDefaults = function () {
        var values = {};

        if (arguments.length === 2) {
            // allow passing of single key/value...
            values[arguments[0]] = arguments[1];
        } else {
            // ... and as an object too
            values = arguments[0];
        }

        $.extend(defaults, values);
    };

    exports.dialog = function (options) {

        options = $.extend({}, defaults, options);
        var dialog = $(template);
        dialog.find('.mallbox-body').html(options.message);
        
        dialog.find('.mallbox-title').html(_t("TITLE"));
        dialog.find('.ok').html(_t("OK"));
        dialog.find('.cancel').html(_t("CANCEL"));
        switch (options.type) {
            case 'alert':
                break;
            case 'confirm':
                dialog.find('.cancel').css("display", "inline-block");
                dialog.on("click", ".cancel", function (e) {
                    dialog.remove();
                });
                break;
        }

        $(options.container).append(dialog);

        dialog.find('.mask').css("display", (options.modal ? 'block' : 'none'));
        dialog.on("click", ".close", function (e) {
            dialog.remove();
        });

        dialog.on("click", ".ok", function (e) {
            dialog.remove();
            if (options.callback && typeof (options.callback) == 'function') {
                options.callback.call(this);
            }
        });

        return dialog;
    }

    exports.alert = function (options) {
        exports.dialog(options);
    }

    exports.confirm = function (options) {
        options.type = 'confirm';
        exports.dialog(options);
    }

    var locales = {
        zh_CN: {
            CANCEL: "取消",
            OK: "确认",
            TITLE: "提示"
        },
        th_TH: {
            CANCEL: "ไม่",
            OK: "ใช่",
            TITLE: "คำเตือน"
        },
        en_US: {
            CANCEL: "Cancel",
            OK: "OK",
            TITLE: "Message"
        }
    };

    exports.setLocale = function (name) {
        return exports.setDefaults("locale", name);
    };

    return exports;

   //mallbox.alert({message:'test',modal:true})
}));



