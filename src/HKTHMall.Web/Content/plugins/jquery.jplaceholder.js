var JPlaceHolder = {
    //检测
    _check: function () {
        return 'placeholder' in document.createElement('input');
    },
    //初始化
    init: function () {
        if (!this._check()) {
            this.fix();
        }
    },
    //修复
    fix: function () {
        //jQuery(':input[placeholder]').each(function (index, element) {
        //    var self = $(this), txt = self.attr('placeholder');
        //    self.wrap($('<div></div>').css({ position: 'relative', zoom: '1', border: 'none', background: 'none', padding: 'none', margin: 'none' }));
        //    var pos = self.position(), h = self.outerHeight(true), paddingleft = self.css('padding-left');
        //    var holder = $('<span></span>').text(txt).css({ position: 'absolute', left: pos.left + 4, top: pos.top + 8, height: h, lienHeight: h, paddingLeft: paddingleft, color: '#aaa' }).appendTo(self.parent());
        //    self.focusin(function (e) {
        //        holder.hide();
        //    }).focusout(function (e) {
        //        if (!self.val()) {
        //            holder.show();
        //        }
        //    });
        //    holder.click(function (e) {
        //        holder.hide();
        //        self.focus();
        //    });
        //    if (self.val().length > 0) {
        //        holder.hide();
        //    } 
        //});
        jQuery(':input[placeholder]').each(function (index, element) {
            var self = $(this), txt = self.attr('placeholder'), atrValue = self.attr('type');

            //密码文本框
            if (atrValue == "password") {
                self.attr("type", "text");
                self.attr("pwd", "true");
            } else {
                self.attr("pwd", "false");
            }

            self.focusin(function (e) {
                var atrpwd = self.attr('pwd');
                if (atrpwd == "true") {
                    self.attr("type", "password");
                } else {
                    self.attr("type", "text");
                }
                self.val("");
            }).focusout(function (e) {
                if (!self.val()) {
                    self.attr("type", "text");
                    self.val(txt);
                }
            });
            if (self.val().length > 0) {
                self.val("");
            }
            else {
                self.val(txt);

            }

        });
    }
};
//执行
jQuery(function () {
    JPlaceHolder.init();
});

