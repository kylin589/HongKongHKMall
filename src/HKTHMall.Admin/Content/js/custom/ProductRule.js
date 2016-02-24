//加载数据
$(function () {
    var columns = [
           { field: "ProductId", title: "Product SN", align: 'center', valign: 'middle', sortable: false },
    { field: "ProductName", title: "Product name", align: 'center', valign: 'middle', sortable: false },
        { field: "SalePrice", title: "Promotion price", align: 'center', valign: 'middle', sortable: false },
      { field: "RuleName", title: "促销类型", align: 'center', valign: 'middle', sortable: false },
        { field: "PrdoctRuleName", title: "促销信息", align: 'center', valign: 'middle', sortable: false },
          { field: "Discount", title: "Discount", align: 'center', valign: 'middle', sortable: false },
           { field: "StarDate", title: "Start time", align: 'center', valign: 'middle', sortable: false, formatter: function (val) { return val.formatterString(true) } },
           { field: "EndDate", title: "End time", align: 'center', valign: 'middle', sortable: false, formatter: function (val) { return val.formatterString(true) } },
    {
        field: "ProductRuleId", title: "Operation", align: 'center', valign: 'middle',
        formatter: function (val) {
            return [
                    '<a class="edit ml10" href="javascript:void(0)" title="Update">',
                    '<i class="glyphicon glyphicon-edit"></i>',
                    '</a>',
                     '&nbsp;&nbsp;&nbsp;&nbsp;<a class="remove ml10" href="javascript:void(0)" title="Remove">',
                    '<i class="glyphicon glyphicon-remove"></i>',
                    '</a>'
            ].join('');
        },
        events: {
            //编辑
            'click .edit': function (e, value, row, index) {
                Tool.ShowModal('/ProductRule/Create', { id: value }, 'Edit promotion information');
            },
            //删除
            'click .remove': function (e, value, row, index) {
                Tool.DeleteRecord('/ProductRule/Delete', { productRuleId: value }, $('#u_grid'));
            }
        }
    }];
    iniData();
    //加载列表数据
    function iniData() {
        $('#u_grid').bootstrapTable({
            url: '/ProductRule/List',
            queryParams: function (params) {
                return {
                    ProductId: $('#ProductId').val().trim(),
                    SalesRuleId: $('#SalesRuleIds').val().trim(),
                    PagedIndex: this.pageNumber - 1,
                    PagedSize: this.pageSize
                };
            },
            sortOrder: 'asc',
            sortName: 'ProductRuleId',
            pagination: true,
            showRefresh: true,
            showColumns: true,
            striped: true,
            sidePagination: 'server',
            columns: columns
        });
    }
    //搜索事件
    $('.js-search').click(function () {
        Tool.ReloadDataTable($('#u_grid'));
    });
    //添加商品促销信息
    $('.js_create').click(function () {
        Tool.ShowModal("/ProductRule/Create", {}, "Add promotion information");
    });
    //触发促销规则下拉
    $("#SalesRuleIds").change(function () {
        Tool.ReloadDataTable($('#u_grid'));
    });

    //删除商品促销信息
    function DelProductRule(productRuleId) {
        var url = "/ProductRule/Delete?productRuleId=" + productRuleId;
        $.ajax({
            url: url,
            dataType: "text",
            success: function (data, status) {
                data = JSON.parse(data);
                Tool.ReloadDataTable($('#u_grid'));
                Tool.Alert(data.Messages, 1500);
            },
            error: function (data, status, e) {
                Tool.Alert("Delete failed！", 1500);
            }
        });
    };
})


//验证输入框
function validInput() {
    var isValid = true;
    var form = $("#modal-form");
    if (parseFloat(form.find("#ProductId").val().trim()) <= 0) {
        form.find("#ProductId").parent().addClass('has-error');
        Tool.Alert("Product number must greater than 0!", 1000);
        isValid = false;
        return;
    }
    else {
        form.find("#ProductId").parent().removeClass('has-error');
    }
    //等于2时,表示有折扣促销
    if (form.find("#SalesRuleId").val() == 2) {
        var r = /^(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/;
        if (!r.test(form.find("#Discount").val())) {
            Tool.Alert("Please input right discount", 1000);
            isValid = false;
            return;
        }
        if (parseFloat(form.find("#Discount").val().trim()) <= 0) {
            form.find("#Discount").parent().addClass('has-error');
            Tool.Alert("Please input discount greater than 0!", 1000);
            isValid = false;
            return;
        } else {
            form.find("#Discount").parent().removeClass('has-error');
        }
        if (form.find("#EndDate").val() < form.find("#StarDate").val()) {
            form.find("#EndDate").parent().addClass('has-error');
            Tool.Alert("End time must later than start time!", 1000);
            isValid = false;
            return;
        } else {
            form.find("#EndDate").parent().removeClass('has-error');
        }
    }
    return isValid;
}
//保存
function Save() {
    if (validInput()) {
        Tool.SaveModal($('#u_grid'));
    }

}