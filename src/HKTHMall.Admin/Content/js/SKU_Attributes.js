$(function () {

    $('#u_grid').bootstrapTable({
        url: '/SKU_Attributes/List',
        queryParams: function (params) {
            return {
                PagedIndex: this.pageNumber - 1,
                PagedSize: this.pageSize,
                AttributeName: $('#AttributeName').val()
            };
        },
        sortOrder: 'desc',
        sortName: 'AttributeName',
        columns: [
        { field: 'AttributeName', title: 'specification name', align: 'center', valign: 'middle', sortable: false },
        {
            field: 'AttributeType',
            title: 'Spec type',
            align: 'center',
            valign: 'middle',
            sortable: false,
            formatter: function (val) {
                return val === 1 ? 'Picture' : 'Word';
            }

        },
        {
            field: 'AttributeId',
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
                    Tool.ShowModal('/SKU_Attributes/Create', { id: value }, 'Edit Product specification');
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
        var actionUrl = "/SKU_Attributes/Create";
        var param = {};
        Tool.ShowModal(actionUrl, param, "Add Product specification");
    });

    //添加规格值
    $('#modal-form').delegate('.js-add-item', 'click', function () {
        var type = $('#modal-form #AttributeType').val();

        var lastTr = $('.js-tb-items tbody > tr:last');
        var lastTrDataStr = lastTr.data('option').replace(/"/g, "#").replace(/'/g, '"').replace(/#/g, "'");
        var lastTrData = $.parseJSON(lastTrDataStr);
        var data =
        {
            DisplaySequence: lastTrData.DisplaySequence + 1,
            Index: lastTrData.Index + 1,
            IsImage: parseInt(type) === 1

        };

        var tplContent = $('#itemTpl').html();
        var newItemSource = Mustache.render(tplContent, data);
        $(newItemSource).insertAfter(lastTr);
    });

    //删除规格值
    $('#modal-form').delegate('.js-remove-item', 'click', function () {
        var $parentTr = $(this).parents('tr');
        var $valueId = $('.js-ValueId', $parentTr);

        if ($valueId && parseInt($valueId.val()) > 0) {
            $.get("/SKU_Attributes/ValueIsUsed", { valueId: $valueId.val() }, function (data) {
                if (data && !data.IsValid) {
                    $parentTr.hide();
                    $('.js-RowStatus', $parentTr).val(-1);
                } else {
                    Tool.Alert(data.Messages[0], 1500);
                }
            });
        } else {
            $parentTr.hide();
            $('.js-RowStatus', $parentTr).val(-1);
        }


    });

    //上移规格值
    $('#modal-form').delegate('.js-up-item', 'click', function () {
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
    $('#modal-form').delegate('.js-down-item', 'click', function () {
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

    //上传图片
    $('#modal-form').delegate('.js-upload-item', 'click', function () {
        var $parentTr = $(this).parents('tr');
        var $file = $('.js-item-file', $parentTr);
        var selector = '#' + $file.attr('id');
        Tool.ImageUpload(selector, callBackImage);

    });

    //选择图片
    $('#modal-form').delegate('.js-item-file', 'change', function () {
        var $parentTr = $(this).parent();
        $('.js-choice-item', $parentTr).removeClass("btn-default").addClass('btn-success');
    });

    //选择图片
    $('#modal-form').delegate('.js-choice-item', 'click', function () {
        var $parentTr = $(this).parents('tr');
        $('.js-item-file', $parentTr).click();
    });


    //改变类型,删除规格值
    $('#modal-form').delegate('#AttributeType', 'change', function () {
        $('.js-tb-items tr:first').nextAll('tr').remove();
    });

});

function callBackImage(data) {
    var $file = $(data.Selector);
    var $parentTr = $file.parents('tr');
    $('.js-item-image', $parentTr).attr('src', Tool.RootImage + data.Data);
    $('.js-ImageUrl', $parentTr).val(data.Data);
    $('.js-choice-item', $parentTr).addClass("btn-default").removeClass('btn-success');
}

//验证规格值数据
function validInputData() {
    ///<summary>验证文本框输入</summary>

    var regValue = new RegExp(/[$&\\<>',%]/); //括号,小数点,横线,斜线,加号,乘号,空格,逗号,分号,冒号,波浪线
    var values = [];    //规格值集合
    var isValid = true; //是否通过验证
    var efficaciousItemCount = 0;
    var type = parseInt($('#modal-form #AttributeType').val());

    $('.js-tr-item').each(function (i, dom) {
        if (!isValid) {
            return;
        }
        var $ValueStr = $(dom).find('.js-ValueStr');
        var $RowStatus = $(dom).find('.js-RowStatus');
        var $ImageUrl = $(dom).find('.js-ImageUrl');

        //只有有效数据才验证数据合法性
        if (parseInt($RowStatus.val()) == 0) {
            //规格值不能为空
            var value = $ValueStr.val();
            if (!value || value == '') {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Input specification value!", 1000);
                isValid = false;
            } else if (values.indexOf(value) != -1) {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Specification value cannot repeat！", 1000);
                isValid = false;

            } else if (regValue.test(value)) {
                $ValueStr.parent().addClass('has-error');
                Tool.Alert("Special character including $ & \\ < > \' , \%  is available", 1000);
                isValid = false;
            } else if (type === 1 && !$ImageUrl.val()) {
                Tool.Alert("Upload specification value picture", 1000);
                isValid = false;
            } else {
                values.push(value);
                $ValueStr.parent().removeClass('has-error').addClass('has-success');
            }
            efficaciousItemCount++;
        }

    });

    if (efficaciousItemCount === 0) {
        Tool.Alert("Setup specification value！", 1500);
        isValid = false;
    }

    return isValid;
}

//保存
function Save() {
    if ($("#modal-content").valid() && validInputData()) {
        Tool.SaveModal($('#u_grid'));
    }
}

