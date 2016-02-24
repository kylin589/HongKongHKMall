/// <summary>
/// Author:YiFan
/// Time:2014-12-31 10:24:02
/// Describe:js扩展文件
/// .NET Version:4.0
/// Version:1.0.0
/// Copyright:
/// </summary>

//自定义弹出框
jQuery.fn.extend({
    serializeJson: function () {
        /// <summary>将表单数据序列成json数据</summary>
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    },
    //$('.modal-dialog').drags({ handle: ".modal-header" });
    drags: function (opt) {
        opt = $.extend({
            handle: "",
            cursor: "move"
        }, opt);
        var $selected = this;
        var $elements = (opt.handle === "") ? this : this.find(opt.handle);

        $elements.css('cursor', opt.cursor).on("mousedown", function (e) {
            var pos_y = $selected.offset().top - e.pageY,
              pos_x = $selected.offset().left - e.pageX;
            $(document).on("mousemove", function (e) {
                $selected.offset({
                    top: e.pageY + pos_y,
                    left: e.pageX + pos_x
                });
            }).on("mouseup", function () {
                $(this).off("mousemove"); // Unbind events from document                
            });
            e.preventDefault(); // disable selection
        });
        return this;
    }
});

$(function () {
    jQuery.extend(String.prototype, {
        isPositiveInteger: function () {
            return (new RegExp(/^[1-9]\d*$/).test(this));
        },
        isInteger: function () {
            return (new RegExp(/^\d+$/).test(this));
        },
        isNumber: function (value, element) {
            return (new RegExp(/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/).test(this));
        },

        trim: function () {
            return this.replace(/(^\s*)|(\s*$)|\r|\n/g, "");
        },
        trans: function () {
            return this.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"');
        },
        replaceAll: function (os, ns) {
            return this.replace(new RegExp(os, "gm"), ns);
        },
        skipChar: function (ch) {
            if (!this || this.length === 0) { return ''; }
            if (this.charAt(0) === ch) { return this.substring(1).skipChar(ch); }
            return this;
        },


        isValidPwd: function () {
            return (new RegExp(/^([_]|[a-zA-Z0-9]){6,32}$/).test(this));
        },

        isValidMail: function () {
            return (new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(this.trim()));
        },

        isSpaces: function () {
            for (var i = 0; i < this.length; i += 1) {
                var ch = this.charAt(i);
                if (ch != ' ' && ch != "\n" && ch != "\t" && ch != "\r") { return false; }
            }
            return true;
        },
        isPhone: function () {
            return (new RegExp(/(^([0-9]{3,4}[-])?\d{3,8}(-\d{1,6})?$)|(^\([0-9]{3,4}\)\d{3,8}(\(\d{1,6}\))?$)|(^\d{3,8}$)/).test(this));
        },
        isURL: function () {
            return (new RegExp(/^[a-zA-z]+:\/\/(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$/).test(this));
        },
        sub: function (len) {
            var str = '';
            if (this.length > len) {
                str = this.substring(0, len) + '...';
            }
            return str;
        },
        gblen: function () {
            /// <summary>字符串长度,中文为两个字节</summary>
            var l = 0;
            var a = this.split("");
            for (var i = 0; i < a.length; i++) {
                if (a[i].charCodeAt(0) < 299) {
                    l++;
                }
                else
                    l += 2;
            }
            return l;
        },
        formatterString: function (isShort) {
            //if (!this) {
            //    return '';
            //}
            var cellval = this;
            var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

            var hour = date.getHours();
            hour = hour < 10 ? '0' + hour : hour;

            var minute = date.getMinutes();
            minute = minute < 10 ? '0' + minute : minute;

            var seconds = date.getSeconds();
            seconds = seconds < 10 ? '0' + seconds : seconds;
            return date.getFullYear() + "-" + month + "-" + currentDate + (isShort ? "" : (" " + hour + ":" + minute + ":" + seconds));
        }
    });

    //日期格式化
    jQuery.extend(Date.prototype, {
        formatterString: function () {
            var date = this;
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

            var hour = date.getHours();
            hour = hour < 10 ? '0' + hour : hour;

            var minute = date.getMinutes();
            minute = minute < 10 ? '0' + minute : minute;

            var seconds = date.getSeconds();
            seconds = seconds < 10 ? '0' + seconds : seconds;
            return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute + ":" + seconds;
        }
    });

});

