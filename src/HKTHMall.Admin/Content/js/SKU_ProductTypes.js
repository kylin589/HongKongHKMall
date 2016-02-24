var TH = {};

$(function () {

    $(window).keydown(function (event) {
        switch (event.keyCode) {
            case 13:
                return false;
                break;
        }
    });

    $('#u_grid').bootstrapTable({
        url: '/SKU_ProductTypes/List',
        queryParams: function (params) {
            return {
                PagedIndex: this.pageNumber - 1,
                PagedSize: this.pageSize,
                Name: $('#Name').val()
            };
        },
        sortOrder: 'desc',
        sortName: 'Name',
        columns: [
        { field: 'Name', title: 'Type Name', align: 'center', valign: 'middle', sortable: false },
        {
            field: 'UseExtend',
            title: 'Extended attributes',
            align: 'center',
            valign: 'middle',
            sortable: false,
            formatter: function (val) {
                return val === 1 ? 'Yes' : 'Not';
            }

        },
         {
             field: 'UseParameter',
             title: 'Detailed parameter',
             align: 'center',
             valign: 'middle',
             sortable: false,
             formatter: function (val) {
                 return val === 1 ? 'Yes' : 'Not';
             }

         },
        {
            field: 'SkuTypeId',
            title: 'Operation',
            align: 'center',
            valign: 'middle',
            formatter: function (val) {
                return [
                    '<a class="edit ml10" href="javascript:void(0)" title="Update">',
                    '<i class="glyphicon glyphicon-edit"></i>',
                    '</a>'
                ].join('');
            },
            events: {
                'click .edit': function (e, value, row, index) {
                    Tool.ShowModal('/SKU_ProductTypes/Create', { id: value }, 'Edit product type');
                }
            }
        }
        ]
    });

    //查询
    $('.js-search').click(function () {
        Tool.ReloadDataTable($('#u_grid'));
    });

    //添加、编辑
    $('.js_create').click(function () {
        var actionUrl = "/SKU_ProductTypes/Create";
        var param = {};
        Tool.ShowModal(actionUrl, param, "Add product type");
    });

    //提示消息
    $('[data-toggle="tooltip"]').tooltip();

    //上移规格值
    $('body').delegate('.js-up-item', 'click', function () {
        var $parentTr = $(this).parents('tr');
        var prevSize = $parentTr.prevAll("tr:visible").size();
        if (prevSize == 1) {
            return;
        }

        var $prevTr = $parentTr.prevAll("tr:visible").eq(0);
        $prevTr.insertAfter($parentTr);
        var $curDisplay = $('.js-DisplaySequence', $parentTr);
        var $prevDisplay = $('.js-DisplaySequence', $prevTr);

        var midV = $curDisplay.val();
        $curDisplay.val($prevDisplay.val());
        $prevDisplay.val(midV);

    });

    //下移规格值
    $('body').delegate('.js-down-item', 'click', function () {
        var $parentTr = $(this).parents('tr');
        var nextSize = $parentTr.nextAll("tr:visible").size();
        if (nextSize == 0) {
            return;
        }

        var $nextTr = $parentTr.nextAll("tr:visible").eq(0);
        $parentTr.insertAfter($nextTr);
        var $curDisplay = $('.js-DisplaySequence', $parentTr);
        var $nextDisplay = $('.js-DisplaySequence', $nextTr);

        var midV = $curDisplay.val();
        $curDisplay.val($nextDisplay.val());
        $nextDisplay.val(midV);

    });

    $('body').on('change', '.js-UsageMode', function() {
        var $this = $(this);
        if (parseInt($this.val()) == 3) {
            $this.parent('td').prev('td').children().hide();
        } else {
            $this.parent('td').prev('td').children().show();
        }
    });


});


function initFormSomething() {
    ///<summary>初始化某些东西</summary>

    $('#js-nav-tabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });


    $('input[type="radio"].js-chk-UseExtend').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    }).on('ifChecked', function (event) {
        if ($(this).data('value') == 1) {
            $('#tab_UseExtend').removeAttr('style');
            $('#nav_tab_UseExtend').show();
        } else {
            $('#tab_UseExtend').hide();
            $('#nav_tab_UseExtend').hide();
        }
    });

    $('input[type="radio"].js-chk-UseParam').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    }).on('ifChecked', function (event) {
        if ($(this).data('value') == 1) {
            $('#tab_UseParam').removeAttr('style');
            $('#nav_tab_UseParam').show();
        } else {
            $('#tab_UseParam').hide();
            $('#nav_tab_UseParam').hide();
        }
    });

}


function bindCollapseClick() {
    ///<summary>绑定手风琴展示事件</summary>

    $("#standardAttrValues").on('show.bs.collapse', function () {
        var $currentDiv = $(arguments[0].target);
        if ($.trim($currentDiv.find('div.box-body').html()) != '') {
            return true;
        } else {
            //optionData.type  0:文字 1:图片
            var optionData = $.parseJSON($currentDiv.data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'"));

            $.get('/SKU_Attributes/ValuesData', { id: optionData.id }, function (items) {
                var tplSelector = optionData.type == 0 ? '#attrValuesTextTpl' : '#attrValuesImageTpl';
                var data = {
                    Items: items
                }

                var valuesTpl = $(tplSelector).html();
                var valuesSource = Mustache.render(valuesTpl, data);

                $currentDiv.find('div.box-body').html(valuesSource);
                $currentDiv.find('div.overlay').remove();
            });
            return true;
        }
    });
}


function selectorStandardAttr() {
    ///<summary>添加规格属性</summary>

    var attributeIdsString = '';
    var trs = $('.js-tb-Standard>tbody>tr');
    trs.each(function (i, dom) {
        var $RowStatus = $(dom).find('.js-RowStatus');
        if (parseInt($RowStatus.val()) === 0) {
            attributeIdsString += ',' + $(dom).find('.js-AttributeId').val() + '&' + $(dom).find('.js-ValueId').val();
        }
    });
    if (attributeIdsString) {
        attributeIdsString = attributeIdsString.substr(1);
    }

    $.get('/SKU_Attributes/Selector', { paramsData: attributeIdsString }, function (data) {
        if (data) {
            bootbox.dialog({
                title: "Choose Spec",
                message: data,
                buttons: {
                    success: {
                        label: "Save",
                        className: "btn-default",
                        callback: addStandardAttr
                    }
                }
            });

            $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
                checkboxClass: 'icheckbox_flat-blue',
                radioClass: 'iradio_flat-blue'
            });
            bindCollapseClick();

        } else {
            Tool.Alert('Getting specification template failed！');
        }
    });
}


function addStandardAttr() {
    ///<summary>添加规格属性</summary>

    $('body').showLoading();
    var $attributeIds = $('#standardAttrValues input:checkbox');
    var $trs = $('.js-tb-Standard tr');
    $attributeIds.each(function (i, dom) {
        var $curentDom = $(dom);
        if ($curentDom.attr('disabled') != 'disabled') {
            var tempData = $.parseJSON($(dom).data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'"));
            //选中该规格
            if ($curentDom.is(':checked')) {
                var isExist = false;
                $trs.each(function (j, tr) {
                    var $AttributeId = $(tr).find('.js-AttributeId');
                    var $RowStatus = $(tr).find('.js-RowStatus');
                    //表示第一次添加的,已存在,不允许删除
                    if ($AttributeId.val() == tempData.AttributeId && $RowStatus.val() == '0') {
                        isExist = true;
                        return;
                        //已存在,但是被删除了,直接拿来显示,无需添加
                    } else if ($AttributeId.val() == tempData.AttributeId && $RowStatus.val() == '-1') {
                        $RowStatus.val(0);
                        $(tr).show();
                        isExist = true;
                        return;
                    }
                });
                //不存在,直接添加
                if (!isExist) {
                    var lastTr = $('.js-tb-Standard tr:last');
                    var lastTrDataStr = lastTr.data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'");
                    var lastTrData = $.parseJSON(lastTrDataStr);

                    tempData.DisplaySequence = lastTrData.DisplaySequence + 1;
                    tempData.Index = lastTrData.Index + 1;


                    var tplContent = $('#standardProductTypeAttrTpl').html();
                    var newSource = Mustache.render(tplContent, tempData);
                    $(newSource).insertAfter(lastTr);
                }
                //没有选中,如果是刚刚添加的,需要删除添加的
            } else {
                $trs.each(function (j, tr) {
                    var $AttributeId = $(tr).find('.js-AttributeId');
                    var $RowStatus = $(tr).find('.js-RowStatus');
                    if ($AttributeId.val() == tempData.AttributeId) {
                        if ($RowStatus.val() == 0) {
                            $RowStatus.val(-1);
                            $(tr).hide();
                        }
                    }
                });
            }
        }
    });
    $('body').hideLoading();
    return true;
}


function addProductTypeAttr(type) {
    ///<summary>添加商品类型属性</summary>
    ///<param name="type" type="int">类型 1:扩展属性 2:详细参数</param>

    var lastTr = type === 1 ? $('.js-tb-UseExtend tr:last') : $('.js-tb-UseParam tr:last');
    var lastTrDataStr = lastTr.data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'");
    var lastTrData = $.parseJSON(lastTrDataStr);
    var data =
    {
        DisplaySequence: lastTrData.DisplaySequence + 1,
        Index: lastTrData.Index + 1,
        IsUseParam: type === 2,
        Type: type,
        Prefix: type === 1 ? "UseExtendAttributeModels" : "UseParamAttributeModels"

    };
    var tplContent = $('#productTypeAttrTpl').html();
    var newItemSource = Mustache.render(tplContent, data);
    $(newItemSource).insertAfter(lastTr);
};


function removeProductTypeAttr(obj) {
    ///<summary>删除商品类型属性值</summary>
    ///<param name="obj" type="object">触发者</param>

    var $parentTr = $(obj).parents('tr');
    var $ptypeId = $('.js-SKU_ProductTypeAttributeId', $parentTr);
    var $IsUse = $('#IsUse');

    if (parseInt($IsUse.val()) === 1 && $ptypeId && parseInt($ptypeId.val()) > 0) {
        Tool.Alert('This type is quoted by product category, cannot be  deleted！', 1500);
        return false;
    } else {
        $parentTr.hide();
        $('.js-p-RowStatus', $parentTr).val(-1);
        $('.js-RowStatus', $parentTr).val(-1);
        return true;
    }
}

/*region 属性值 */

function showAttrValue(obj, type, index) {
    ///<summary>属性值列表</summary>
    ///<param name="obj" type="object">触发者</param>
    ///<param name="type" type="int">类型 0:扩展属性 2:详细参数</param>
    ///<param name="index" type="int">当前行索引</param>

    TH.Index = index;                       //当前行索引
    switch (type) {
        case 1:
            TH.AttrValuePrefix =
    'UseExtendAttributeModels[' + index + '].SKU_AttributesModel.SKU_AttributeValuesModels';//属性值前缀,用于组装属性值数据
            TH.TableSelector = '.js-tb-UseExtend';
            break;
        default:
            TH.AttrValuePrefix =
    'UseParamAttributeModels[' + index + '].SKU_AttributesModel.SKU_AttributeValuesModels';//属性值前缀,用于组装属性值数据
            TH.TableSelector = '.js-tb-UseParam';
            break;
    }



    var $this = $(obj);
    var $parentTr = $(obj).parents('tr');
    var data = {
        Items: []
    };


    $('.js-attrValues', $parentTr).each(function (i, dom) {
        var $ValueId = $(dom).find('.js-ValueId');
        var $AttributeId = $(dom).find('.js-AttributeId');
        var $ValueStr = $(dom).find('.js-ValueStr');
        var $RowStatus = $(dom).find('.js-RowStatus');
        var $DisplaySequence = $(dom).find('.js-DisplaySequence');

        //一组规格值 属性
        var tempItemData =
        {
            ValueId: $ValueId.val(),
            AttributeId: $AttributeId.val() == "" ? 0 : $AttributeId.val(),
            ValueStr: $ValueStr.val(),
            RowStatus: $RowStatus.val(),
            DisplaySequence: $DisplaySequence.val(),
            Index: i,
            Id: $(dom).attr('id')
        }
        data.Items.push(tempItemData);
    });



    var tpl = $('#attrValuesTpl').html();
    var attValuesSource = Mustache.render(tpl, data);
    bootbox.dialog({
        title: "Add attribute value",
        message: attValuesSource,
        buttons: {
            success: {
                label: "Save",
                className: "btn-default",
                callback: function () {
                    if (validAttrValueData()) {
                        return refreshAttrValue();
                    } else {
                        return false;
                    }

                }
            }
        }
    });


}


function addAttrValue() {
    ///<summary>添加属性值</summary>
    $('body').showLoading();
    var lastTr = $('.js-tb-attrValues tr:last');
    var lastTrDataStr = lastTr.data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'");
    var lastTrData = $.parseJSON(lastTrDataStr);
    var data =
    {
        DisplaySequence: lastTrData.DisplaySequence + 1,
        Index: lastTrData.Index + 1,
        AttributeId: lastTrData.AttributeId
    };

    var tplContent = $('#attrValueTpl').html();
    var newItemSource = Mustache.render(tplContent, data);
    $(newItemSource).insertAfter(lastTr);
    $('body').hideLoading();
}


function removeAttrValue(obj) {
    ///<summary>删除属性值</summary>
    ///<param name="obj" type="object">触发者</param>

    var $parentTr = $(obj).parents('tr');
    var $valueId = $('.js-ValueId', $parentTr);
    var $IsUse = $('#IsUse');

    if (parseInt($IsUse.val()) === 1 && $valueId && parseInt($valueId.val()) > 0) {
        Tool.Alert('This type is quoted by product category, cannot be  deleted！', 1500);
        return false;
    } else {
        $parentTr.hide();
        $('.js-RowStatus', $parentTr).val(-1);
        return true;
    }
}


function validAttrValueData() {
    ///<summary>验证文本框输入</summary>

    var regValue = new RegExp(/[$&<>',;]/); //括号,小数点,横线,斜线,加号,乘号,空格,逗号,分号,冒号,波浪线
    var values = [];    //属性值集合
    var isValid = true; //是否通过验证
    var efficaciousItemCount = 0;

    $('.js-tb-attrValues tr').each(function (i, dom) {
        if (!isValid) {
            return;
        }
        var $ValueStr = $(dom).find('.js-ValueStr');
        var $RowStatus = $(dom).find('.js-RowStatus');

        //只有有效数据才验证数据合法性
        if (parseInt($RowStatus.val()) == 0) {
            //属性值不能为空
            var value = $ValueStr.val();
            if (!value || value == '') {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Please enter attribute value!", 1000);
                isValid = false;
            } else if (values.indexOf(value) != -1) {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Attribute value cannot repeat！", 1000);
                isValid = false;

            } else if (regValue.test(value)) {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Special character including  $ &  < > ' , ;  is unavailable in attribute value！", 1000);
                isValid = false;
            } else {
                values.push(value);
                $ValueStr.parent().removeClass('has-error').addClass('has-success');
            }
            efficaciousItemCount++;
        }

    });

    if (efficaciousItemCount === 0) {
        Tool.Alert("Please setup attribute value！", 1500);
        isValid = false;
    }

    return isValid;
}


function refreshAttrValue() {
    ///<summary>刷新属性数据</summary>

    $('body').showLoading();
    var $trs = $('.js-tb-attrValues tr');
    if ($trs && $trs.size() > 0) {
        var data = {
            Items: []
        };


        var attrValuesSource = '';
        var valueStr = '';
        var index = 0;

        var isValid = true;

        $trs.each(function (i, dom) {


            var $ValueId = $(dom).find('.js-ValueId');
            var $DisplaySequence = $(dom).find('.js-DisplaySequence');
            var $AttributeId = $(dom).find('.js-AttributeId');
            var $RowStatus = $(dom).find('.js-RowStatus');
            var $ValueStr = $(dom).find('.js-ValueStr');

            if (parseInt($RowStatus.val()) == 0 || parseInt($ValueId.val()) > 0) {
                if (parseInt($RowStatus.val()) == 0 && !$ValueStr.val()) {
                    Tool.Alert("Please input attribute value", 1000);
                    isValid = false;
                    return;
                }
                var tempData = {
                    ValueId: $ValueId.val(),
                    DisplaySequence: $DisplaySequence.val(),
                    AttributeId: $AttributeId.val(),
                    RowStatus: $RowStatus.val(),
                    ValueStr: $ValueStr.val(),
                    Prefix: TH.AttrValuePrefix + '[' + index + ']'
                }
                data.Items.push(tempData);

                if (parseInt($RowStatus.val()) == 0) {
                    valueStr += ',' + $ValueStr.val();
                }
                index++;
            }

        });
        if (isValid) {


            var $CurrentTr = $('tr.js-tr-item', $(TH.TableSelector)).eq(TH.Index);
            $CurrentTr.find('.js-attrValues').remove();

            //储存值模版
            var attrValueItemTpl = $('#attrValueItemTpl').html();

            //储存值源码
            attrValuesSource = Mustache.render(attrValueItemTpl, data);
            $CurrentTr.find('td.js-td-attrValues').append(attrValuesSource);
            valueStr = valueStr.substr(1);
            $(".js-ValueString-Tips", $CurrentTr).tooltip('hide').attr('data-original-title', valueStr).tooltip('fixTitle').tooltip('show');
            $('.js-ValueString', $CurrentTr).val(valueStr);
        }
        $('body').hideLoading();
        return isValid;
    }
}

/*endregion 属性值 */

function Save() {
    ///<summary>保存</summary>
    if (validFormData()) {
        Tool.SaveModal($('#u_grid'));
    }
}

function validFormData() {
    ///<summary>验证表单输入</summary>

    $('.nav-tabs a:first').tab('show');
    if (!$("#modal-content").valid()) {
        return false;
    }

    if (!validStandardData()) {
        return false;
    }
    if ($('.js-tb-UseExtend').size() != 0 && $("input[name='UseExtend']:checked").val() == 1) {
        if (!validProductTypeAttrData(1)) {
            return false;
        }
    }
    if ($('.js-tb-UseParam').size() != 0 && $("input[name='UseParameter']:checked").val() == 1) {
        if (!validProductTypeAttrData(2)) {
            return false;
        }
    }
    return true;
}

function validStandardData() {
    ///<summary>验证规格数据</summary>

    $('.nav-tabs a').eq(1).tab('show');

    var isValid = true;
    var $trs = $('.js-tb-Standard tr');
    if ($trs && $trs.size() > 1) {
        $trs.each(function (i, dom) {

            var $AttributeId = $(dom).find('.js-AttributeId');
            var $RowStatus = $(dom).find('.js-RowStatus');

            //只有有效数据才验证数据合法性
            if (parseInt($RowStatus.val()) == 0) {
                if (!$AttributeId.val()) {
                    Tool.Alert("Please select specification！", 1000);
                    isValid = false;
                    return;
                }
            }
        });
    } else {
        Tool.Alert("Please select specification！", 1000);
        isValid = false;
    }
    return isValid;
}

function validProductTypeAttrData(type) {
    ///<summary>验证商品类型属性数据</summary>
    ///<param name="type" type="int">类型 1:扩展属性 2:详细参数</param>

    $('.nav-tabs a').eq(type == 1 ? 2 : 3).tab('show');

    var selector = type == 1 ? '.js-tb-UseExtend tr' : '.js-tb-UseParam tr';

    var prefix = type == 1 ? 'Extended attributes' : 'Detailed parameter';

    var isValid = true;

    var paramDatas = [];        //参数组
    var $trs = $(selector);
    if ($trs && $trs.size() > 1) {

        var regValue = new RegExp(/[$&\\<>']/); //new RegExp('^[\u4E00-\u9FA5A-Za-z0-9]+$');
        var regText = new RegExp(/[$&\\<>']/); //括号,小数点,横线,斜线,加号,乘号,空格,逗号,分号,冒号,波浪线
        var regOrder = new RegExp('^[1-9]\d*|0$');
        $trs.each(function (i, dom) {

            if (!isValid) {
                return false;
            }

            if (i == 0) {
                return;
            }

            var $AttributeName = $(dom).find('.js-p-AttributeName');
            var $RowStatus = $(dom).find('.js-p-RowStatus');
            var $ValueString = $(dom).find('.js-ValueString');
            var $UsageMode = $(dom).find('.js-UsageMode');
            var $DisplaySequence = $(dom).find('.js-p-DisplaySequence');

            //如果是扩展参数特殊处理
            var groupName = type == 1 ? "1" : $(dom).find('.js-AttributeGroup').val();

            //只有有效数据才验证数据合法性
            if (parseInt($RowStatus.val()) == 0) {

                //详细参数
                if (type == 2) {
                    var $AttributeGroup = $(dom).find('.js-AttributeGroup');
                    if (!$AttributeGroup.val()) {
                        $AttributeGroup.parent().addClass('has-error');
                        Tool.Alert("Please enter parameter set name！", 1000);
                        isValid = false;
                        return;
                    } else if (regText.test($AttributeGroup.val())) {
                        $AttributeGroup.parent().addClass('has-error');
                        Tool.Alert("Special character including  $ & \\ < > \' , \  is unavailable in parameter  set！", 1000);
                        isValid = false;
                        return;
                    } else {
                        $AttributeGroup.parent().removeClass('has-error').addClass('has-success');
                    }
                }

                //记录参数组名
                if (!isRepeatGroupName(paramDatas, groupName)) {
                    var paramData = {
                        groupName: groupName,
                        paramNames: []
                    }
                    paramDatas.push(paramData);
                }


                if (!$AttributeName.val()) {
                    $AttributeName.parent().addClass('has-error');
                    Tool.Alert('Please enter the ' + prefix + ' name！', 1000);
                    isValid = false;
                    return;
                } else if (regValue.test($AttributeName.val())) {
                    $AttributeName.parent().addClass('has-error');
                    Tool.Alert(prefix + 'Special character including  $ & \\ < > \' , \  is unavailable！', 1000);
                    isValid = false;
                    return;
                } else if (isRepeatGroupParamName(paramDatas, groupName, $AttributeName.val())) {
                    $AttributeName.parent().addClass('has-error');
                    Tool.Alert(prefix + 'Name cannot repeat！', 1000);
                    isValid = false;
                    return;
                }
                else {
                    pushGroupParamName(paramDatas, groupName, $AttributeName.val());
                    $AttributeName.parent().removeClass('has-error').addClass('has-success');
                }

                if (!$ValueString.val() && $UsageMode.val()!=3) {
                    Tool.Alert('Please set up' + prefix + ' value！', 1000);
                    isValid = false;
                    return;
                }
                if (!$UsageMode.val()) {
                    Tool.Alert("Please select product publishment display type ！", 1000);
                    isValid = false;
                    return;
                }
                if (!regOrder.test($DisplaySequence.val())) {
                    Tool.Alert("Please enter positive integer to sort product！", 1000);
                    isValid = false;
                    return;
                }




            }
        });
    } else {
        Tool.Alert('Please add ' + prefix + '！', 1000);
        isValid = false;
    }

    return isValid;


}

function isRepeatGroupParamName(objs, groupName, paramName) {
    ///<summary>是否重复参数组参数名</summary>
    ///<param name="objs">参数组</param>
    ///<param name="groupName">参数组名</param>
    ///<param name="paramName">参数名</param>
    for (var i in objs) {
        if (objs[i].groupName == groupName && objs[i].paramNames.indexOf(paramName) != -1) {
            return true;
        }
    }
    return false;
}

function isRepeatGroupName(objs, groupName) {
    ///<summary>是否重复参数组名</summary>
    ///<param name="objs">参数组</param>
    ///<param name="groupName">参数组名</param>
    for (var i in objs) {
        if (objs[i].groupName == groupName) {
            return true;
        }
    }
    return false;
}

function pushGroupParamName(objs, groupName, paramName) {
    ///<summary>是否重复参数组参数名</summary>
    ///<param name="objs">参数组</param>
    ///<param name="groupName">参数组名</param>
    ///<param name="paramName">参数名</param>
    for (var i in objs) {
        if (objs[i].groupName == groupName) {
            objs[i].paramNames.push(paramName);
        }
    }
    return false;
}