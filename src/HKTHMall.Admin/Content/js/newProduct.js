
var defaultopts = {

};

var viewModel = function(options) {
   
    defaultopts = $.extend(defaultopts, options);

    var self = this;

    //分类1
    self.categories1 = ko.observableArray([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);
    self.selectd_category1 = ko.observable(0);

    //分类2
    self.categories2 = ko.observableArray([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);
    self.selectd_category2 = ko.observable(0);
    //分类3
    self.categories3 = ko.observableArray([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);
    self.selectd_category3 = ko.observable(0);
    //品牌
    self.brands = ko.observableArray([{ Brand_LangModel: { BrandName: "Choose" }, BrandID: 0 }]);
    self.selectd_brand = ko.observable(defaultopts.brandId);
    //规格属性
    self.sku_attrs = ko.observableArray();
    //商品sku
    self.sku_products = ko.observableArray(defaultopts.skuProducts);
    self.sku_products.remove(function(item) {
        return !item.SkuName;
    });
    //SKU图片
    self.sku_pics = ko.observableArray(defaultopts.skuAttrs);
    //属性和参数
    self.sku_items = ko.observableArray(defaultopts.skuItmes);

    self.supplierId = ko.observable(defaultopts.supplerId);

    self.supplierName = ko.observable(defaultopts.supplerName);
    
    
    self.selfss = ko.observableArray(defaultopts.fares);

    self.faseId = ko.observable(defaultopts.templateId);
    

    self.selectSupplier = function() {

        bootbox.dialog({
            title: "Select Supplier",
            message: "<table id=\"supplier_grid\"></table>",
            buttons: {
                success: {
                    label: "Confirm",
                    className: "btn-primary",
                    callback: function() {
                        var rows = $("#supplier_grid").bootstrapTable("getSelections");
                        if (rows.length !== 0) {
                            debugger;
                            self.supplierId(rows[0].SupplierId);
                            self.supplierName(rows[0].SupplierName);
                            $(this).dialog("close");
                        }
                    }
                },
                close: {
                    label: "Close",
                    className: "btn-default",
                    callback: function() {
                        $(this).dialog("close");
                    }
                }
            }
        });

        $("#supplier_grid").bootstrapTable({
            url: "/Suppliers/List",
            queryParams: function(params) {
                return {
                    PagedIndex: this.pageNumber - 1,
                    PagedSize: this.pageSize,
                };
            },
            sortOrder: "desc",
            cache: false,
            height: "100%",
            striped: true,
            pagination: true,
            pageSize: 5,
            pageList: [5],
            //search: true,
            showColumns: false,
            showRefresh: false,
            singleSelect: true,
            minimumCountColumns: 2,
            clickToSelect: true,
            sidePagination: "server",
            columns: [
                {
                    field: "state", checkbox: true, align: "center", valign: "middle", sortable: true,
                    formatter: function (value, row, index) {
                        if (index == 0) {
                            return {checked: true};
                        } else {
                            return {checked: false};
                        }
                    }
                },//update by liujc
                {field: "SupplierName", title: "Supplier Name", align: "center", valign: "middle", sortable: false },
                {field: "ShortName", title: "Short Name", align: "center", valign: "middle", sortable: false },
                {field: "Address", title: "Address", align: "center", valign: "middle", sortable: false },
                {field: "LinkMan", title: "Link Man", align: "center", valign: "middle", sortable: false },
                {field: "Telephone",title: "Telephone",align: "center",valign: "middle",sortable: false},
                {field: "Mobile",title: "Mobile",align: "center",valign: "middle",sortable: false},
                {field: "Remark", title: "Remark", align: "center", valign: "middle", sortable: false },
            ]
        });
    };
    ///初始categories1数据
    self.load = function() {

        $.ajax({
            url: "/Category/GetCategoryByParentId",
            data: { id: 0 },
            async: false,
            cache: false,
            dataType: "json",
            type: "post",
            success: function(result) {
                if (result.IsValid) {
                    self.categories1(result.Data);
                    self.categories1.unshift({
                        CategoryId: 1,
                        CategoryLanguageModel: {
                            CategoryName: "Choose"
                        }
                    });
                    self.initCategory();
                } else {
                    $.messager.alert("Wrong information", result.Messages.join("<br/>"));
                }
            }
        });
        self.selectd_category1(0);
        if (defaultopts.pics) {
            $("#upload_img_grid").bootstrapTable("load", defaultopts.pics);
        }
    }; //初始加载数据
    self.initCategory = function() {
        if (defaultopts.categoryId != 0) {

            $.post("/Category/GetParentCategoryListByChildernCategoryId", { id: defaultopts.categoryId }, function(result) {
                if (result.IsValid) {

                    $(result.Data).each(function() {
                        if (this.CategoryId != 0) {
                            self.categories3.push({
                                CategoryLanguageModel: {
                                    CategoryName: this.CategoryName
                                },
                                CategoryId: this.CategoryId
                            });
                        }
                    });
                    self.selectd_category1(result.Data[0].ToCategoryId);

                    var newArray = $(result.Data).uniqueArray("ParentCategoryId");

                    $(newArray).each(function() {
                        self.categories2.push({
                            CategoryId: this.ParentCategoryId,
                            CategoryLanguageModel: {
                                CategoryName: this.ParentCategoryName
                            }
                        });
                    });

                    self.selectd_category2(result.Data[0].parentId);

                    self.selectd_category3(defaultopts.categoryId);

                    self.loadbrand(defaultopts.categoryId);

                    self.get_attr(defaultopts.categoryId);
                }
            });
        }
    }; ///初始分类
    self.loadbrand = function(selectdId) {
        $.post("/Brand/GetBrandByCategoryById", { id: selectdId }, function(result) {
            if (result.IsValid) {
                self.brands(result.Data);
                self.brands.unshift({ Brand_LangModel: { BrandName: "Choose" }, BrandID: 0 });
                if (defaultopts.brandId) {
                    self.selectd_brand(defaultopts.brandId);
                } else {
                    $(".category select").eq(3).val(0);
                }
            } else {
                $.messager.alert("Wrong information", result.Messages.join("<br/>"));
            }
        });
    }; //品牌
    self.categories1_change = function() {
        var selectdId = self.selectd_category1();
        self.categories2([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);
        self.categories3([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);

        if (selectdId == 0) {
            return;
        }

        $.ajax({
            url: "/Category/GetCategoryByParentId",
            data: { id: selectdId },
            async: false,
            cache: false,
            dataType: "json",
            type: "post",
            success: function(result) {
                if (result.IsValid) {
                    self.categories2(result.Data);
                    self.categories2.unshift({
                        CategoryId: 0,
                        CategoryLanguageModel: {
                            CategoryName: "Choose"
                        }
                    });
                } else {
                    $.messager.alert("Wrong information", result.Messages.join("<br/>"));
                }
            }
        });
        self.selectd_category2(0);
        self.selectd_category3(0);
        $(".category select").eq(1).val(0);

    }; //选择分类1
    self.categories2_change = function () {
        var selectdId = self.selectd_category2();
        self.categories3([{ CategoryLanguageModel: { CategoryName: "Choose" }, CategoryId: 0 }]);
        self.selectd_category3(0);
        if (selectdId == 0) {
            return;
        }

        $.ajax({
            url: "/Category/GetCategoryByParentId",
            data: { id: selectdId },
            async: false,
            cache: false,
            dataType: "json",
            type: "post",
            success: function (result) {
                if (result.IsValid) {
                    self.categories3(result.Data);
                    self.categories3.unshift({
                        CategoryId: 0,
                        CategoryLanguageModel: {
                            CategoryName: "Choose"
                        }
                    });
                } else {
                    $.messager.alert("Wrong information", result.Messages.join("<br/>"));
                }
            }
        });

        self.selectd_category3(0);
        $(".category select").eq(2).val(0);
    }; //选择分类2
    self.categories3_change = function () {
        //加载品牌
        self.sku_products([]);
        self.sku_pics([]);
        self.sku_items([]);
        var selectdId = self.selectd_category3();
        self.loadbrand(selectdId);
        self.get_attr(selectdId);
    }; //选择分类3
    self.get_attr = function(selectdId) {
        //加载属性和参数
        $.post("/SKU_ProductTypes/GetSku_ProductTypesByCategoryId", { id: selectdId }, function(result) {
            if (result.IsValid) {

                self.sku_attrs([]);

                $(result.Data).each(function() {
                    $(this.SKU_ProductTypeAttributeModel).each(function() {
                        var data = this.SKU_AttributesModel;
                        data.Selectd = 0;
                        $(data.SKU_AttributeValuesModels).each(function() {
                            var attr = this;
                            attr.IsCheckd = false;
                            $(self.sku_items()).each(function() {
                                if (attr.ValueId === this.ValueId && data.UsageMode == 0) {
                                    attr.IsCheckd = true;
                                    attr.SKU_ProductAttributesId = this.SKU_ProductAttributesId;
                                }
                                if (attr.ValueId === this.ValueId && data.UsageMode == 2) {
                                    data.Selectd = this.ValueId;
                                    self.sku_items.remove(function(item) {
                                        return item.ValueId == data.Selectd;
                                    });
                                    attr.SKU_ProductAttributesId = this.SKU_ProductAttributesId;
                                }
                                if (attr.ValueId === this.ValueId && data.UsageMode == 3) {
                                    attr.ValueStr = this.ValueStr;
                                }
                            });
                            $(self.sku_pics()).each(function() {
                                if (attr.ValueId === this.ValueId) {
                                    attr.IsCheckd = true;
                                    attr.SKU_ProductAttributesId = this.SKU_ProductAttributesId;
                                }
                            });
                        });
                    });

                    self.sku_attrs.push(this);
                });

                //刷新checkbox样式
                self.refreshCheckbox("input[type=\"checkbox\"]");

                //规格属性添加删除
                self.productAddAndremove();

                //扩展属性和参数添加删除
                self.itemsAddAndremove();

            } else {
                $.messager.alert("Wrong information", result.Messages.join("<br/>"));
            }
        });
    }; //扩展属性和参数添加删除
    
    self.itemsAddAndremove = function() {
        $(".items input[type=\"checkbox\"]").on("ifChecked", function() {
            var item = $(this).data("options");
            self.sku_items.push(item);
        }).on("ifUnchecked", function() {
            var skuitem = $(this).data("options");
            self.sku_items.remove(function(item) {
                return item.ValueId == skuitem.ValueId;
            });
        });
    }; //规格属性添加删除
    self.productAddAndremove = function() {
        $(".spec input[type=\"checkbox\"]").on("ifChecked", function() {

            var sku_img = $(this).data("options");
            sku_img = $.extend(sku_img, {
                SKU_ProductAttributesId: null,
                ImageUrl: null
            });

            self.sku_pics.push(sku_img);

            var arrayInfor = [];
            if (!self.getSku(arrayInfor)) {
                return;
            }

            //添加sku
            self.addSku(arrayInfor);

            self.refreshCheckbox(".sku_product input[type=\"checkbox\"]");

            //取消sku选择
        }).on("ifUnchecked", function() {

            var sku_img = $(this).data("options");

            self.sku_pics.remove(function(item) {
                return item.ValueId == sku_img.ValueId;
            });

            var arrayInfor = [];
            if (!self.getSku(arrayInfor)) {
                self.sku_products([]);
                return;
            }

            self.removeSku(arrayInfor);

        });
    }; //获取sku
    self.getSku = function(arrayInfor) {
        var isGroupChecked = true;
        $(".spec").each(function() {
            var checkboxs = $(this).find("input[type=\"checkbox\"]:checked");
            //判断是否全选
            if (checkboxs.length == 0) {
                isGroupChecked = false;
                return;
            }
            var infos = new Array();
            $(checkboxs).each(function() {
                var options = $(this).data("options");
                infos.push({ 'SKUStr': options.ValueId, 'SkuName': options.ValueStr });
            });
            arrayInfor.push(infos);
        });
        return isGroupChecked;
    }; //添加sku
    self.addSku = function(arrayInfor) {
        //组合sku                                
        var skus = $(arrayInfor).doExchange();
        $(skus).each(function() {
            var sku = this;
            if (!$(self.sku_products()).IsArray("SKUStr", sku.SKUStr)) {
                //构建SKU
                var skudata = {
                    PurchasePrice: $("#PurchasePrice").val(),
                    MarketPrice: $("#MarketPrice").val(),
                    HKPrice: $("#HKPrice").val(),
                    ProductCode: $("#ArtNo").val(),
                    Stock: 0,
                    SKU_ProducId: null,
                    IsUseBool: true
                };
                var data = $.extend(skudata, sku);
                self.sku_products.push(data);
            }
        });
    }; //删除sku
    self.removeSku = function(arrayInfor) {
        //组合sku     
        var skus = $(arrayInfor).doExchange();
        $(self.sku_products()).each(function() {
            var loDsku = this;
            if (!$(skus).IsArray("SKUStr", loDsku.SKUStr)) {
                self.sku_products.remove(function(item) {
                    return item.SKUStr == loDsku.SKUStr;
                });
            }
        });
    };
    self.refreshCheckbox = function (e) {
        if ($(e).eq(0).attr('id') == "_check_dis")
            return;
        $(e).iCheck({
            checkboxClass: "icheckbox_square-blue",
            radioClass: "iradio_square-blue",
            increaseArea: "20%"
        });
    }; //初始加载数据
    self.load();
    
    self.updateAttr = function (place, e) {
        var options = $(e.target).hide().data("options");
        var $input = $('<input type="text" class="form-control" />');
        $input.appendTo($(e.target).parent()).focus();
        $input.blur(function () {
            if ($(this).val()) {
                options.ValueStr = $(this).val();
                $(e.target).data("options", options).text(options.ValueStr);
            }
            $(this).remove();
            $(e.target).show();
        });
    }

    //选择图片
    $("#product_form").delegate(".js-choice-item", "click", function() {
        var selector = "#" + $(this).parent()
            .children(".js-item-file")
            .attr("id");
        $(selector).uploadFile(function(e, data) {
            if (data.result.IsValid) {
                var url = Tool.RootImage + data.result.Data;
                $(this).parent().find(".js-item-image").attr("src", url);
                $(this).parent().find(".image-url").val(data.result.Data);
                var index = selector.replace("#imageItem_", "");
                var pics = self.sku_pics();
                pics[index].ImageUrl = data.result.Data;
            } else {
                $.messager.alert("Wrong information", data.result.Messages.join(""));
            }
        });

        $(this).parent()
            .children(".js-item-file")
            .click();
    });

    $("#product_form").delegate(".number", "focus", function() {
        $(this).onlyNum();
    });

    $("#product_form").delegate(".double", "focus", function() {
        $(this).onlyDouble();
    });

    var editors = [];

    // 初始化editor
    KindEditor.ready(function(K) {

        $(".ke textarea").each(function() {
            var editor = K.create(this, {
                //cssPath: '../plugins/code/prettify.css',
                uploadJson: "/KindEditor/UploadImage?userId=" + Tool.UserID, //wuyf 放开 例子在views/banner/KindEditor页面
                showRemote: false,
                //fileManagerJson: '../asp.net/file_manager_json.ashx',
                allowFileManager: true,
                minHeight: 300,
                syncType: "auto",
            });
            if ($("#Status").val() === "4") {
                editor.readonly(true);
            }
            editors.push(editor);
        });
    });

    // 表单提交
    $("#p_f_submit").click(function() {

        $(this).blur();

        var b = true;
        $(".sku_pric").each(function() {

            var hk_price = Number($(this).find(".sku_HKPrice").val());
            var purchase_price = Number($(this).find(".sku_PurchasePrice").val());

            if (hk_price < purchase_price) {
                b = false;
                $.messager.alert("Tips", $(this).find(".sku_name").text() + ":Huika price shall not be low than purchase price");
                return;
            }
        });

        if (!b) {
            return;
        }


        var b1 = true;
        $(".sku_pric").each(function() {

            var hk_price = Number($(this).find(".sku_HKPrice").val());
            var marketPrice = Number($(this).find(".sku_marketPrice").val());

            if (hk_price > marketPrice) {
                b1 = false;
                $.messager.alert("Tips", $(this).find(".sku_name").text() + ":Hui card price is not greater than the market price");
                return;
            }
        });

        if (!b1) {
            return;
        }

        if ($(".sku_pric").length > 0) {
            if (Number($("#HKPrice").val()) < Number($("#PurchasePrice").val())) {
                $.messager.alert("Tips", "Huika price shall not be low than purchase price");
                return;;
            }

            if (Number($("#HKPrice").val()) > Number($("#MarketPrice").val())) {
                $.messager.alert("Tips", "Hui card price is not greater than the market price");
                return;
            }
        }

        var msg = "";
        if ($(".spec").length > 1) {
            $(".spec").each(function() {

                var checkboxs = $(this).find("input[type=\"checkbox\"]:checked");
                //判断是否全选
                if (checkboxs.length == 0) {
                    msg = "Please choose the product specification:" + $(this).children().first().text();
                    return;
                }
            });
        }

        if (msg.length > 0) {
            $.messager.alert("Tips", msg);
            return;
        }

        if (self.selectd_category3() == 0) {
            $.messager.alert("Tips", "Please select category");
            return;
        }

        if (self.selectd_brand() == 0) {
            $.messager.alert("Tips", "Please select brand");
            return;
        }

        var pics = $("#upload_img_grid").bootstrapTable("getData");
        if (pics.length == 0) {
            $.messager.alert("Tips", "Upload at least one product picture");
            return;
        }

        if (self.supplierId() == 0) {
            $.messager.alert("Tips", "Please select business");
            return;
        }

        //update by liujc
        //if ($("#Product_LangModels_0__ProductName").val().length == 0) {
        //    $.messager.alert("Tips", "Please enter product title in Thai");
        //    return;
        //}

        if ($("#Product_LangModels_0__ProductName").val().length == 0) {
            $.messager.alert("Tips", "Please enter product title in Chinese");
            return;
        }

        if ($("#Product_LangModels_1__ProductName").val().length == 0) {
            $.messager.alert("Tips", "Enter English title maximum 100 characters");
            return;
        }
        //add by liujc
        if ($("#Product_LangModels_2__ProductName").val().length == 0) {
            $.messager.alert("Tips", "Please enter product title in Chinese(HK)");
            return;
        }


        //if ($("#Product_LangModels_0__ProductName").val().length > 200) {
        //    $.messager.alert("Tips", "Enter Thai title maximum 200 characters");
        //    return;
        //}

        if ($("#Product_LangModels_0__ProductName").val().length > 200) {
            $.messager.alert("Tips", "Enter Chinese title maximum 200 characters");
            return;
        }

        if ($("#Product_LangModels_1__ProductName").val().length > 200) {
            $.messager.alert("Tips", "Enter English title maximum 200 characters");
            return;
        }
 
        if ($("#Product_LangModels_2__ProductName").val().length > 200) {
            $.messager.alert("Tips", "Enter Chinese(HK) title maximum 200 characters");
            return;
        }

        //if ($("#Product_LangModels_0__Subheading").val().length > 400) {
        //    $.messager.alert("Tips", "Enter Thai sub title maximum 400 characters");
        //    return;
        //}

        if ($("#Product_LangModels_0__Subheading").val().length > 400) {
            $.messager.alert("Tips", "Enter Chinese sub title maximum 400 characters");
            return;
        }

        if ($("#Product_LangModels_1__Subheading").val().length > 400) {
            $.messager.alert("Tips", "Enter English sub title maximum 400 characters");
            return;
        }

        if ($("#Product_LangModels_2__Subheading").val().length > 400) {
            $.messager.alert("Tips", "Enter Chinese(HK) sub title maximum 400 characters");
            return;
        }

        if (Number($("#HKPrice").val()) < Number($("#PurchasePrice").val())) {
            $.messager.alert("Tips", "Huika price shall not be low than purchase price");
            return;
        }

        $(pics).each(function(i) {
            $("<input type=\"hidden\"/>")
                .attr("name", "ProductPicModels[" + i + "].ProductPicId")
                .val(this.ProductPicId)
                .appendTo("#product_form");
            $("<input type=\"hidden\"/>")
                .attr("name", "ProductPicModels[" + i + "].PicUrl")
                .val(this.PicUrl)
                .appendTo("#product_form");
            $("<input type=\"hidden\"/>")
                .attr("name", "ProductPicModels[" + i + "].sort")
                .val(this.sort)
                .appendTo("#product_form");
        });

        $(".items option:selected").each(function() {
            var item = $(this).data("options");
            if (item) {
                self.sku_items.push(item);
            }
        });

        $('.usageMode3 label').each(function () {
            var item = $(this).data("options");
            self.sku_items.remove(function (i) {
                return i.ValueId == item.ValueId;
            });
            if (item) {
                self.sku_items.push(item);
            }
        });

        $(self.sku_items()).each(function(i) {
            $("<input type=\"hidden\">")
                .attr("name", "SKU_SKUItemsModels[" + i + "].AttributeId")
                .val(this.AttributeId)
                .appendTo("#product_form");
            $("<input type=\"hidden\">")
                .attr("name", "SKU_SKUItemsModels[" + i + "].ValueId")
                .val(this.ValueId)
                .appendTo("#product_form");
            $("<input type=\"hidden\">")
                .attr("name", "SKU_SKUItemsModels[" + i + "].ValueStr")
                .val(this.ValueStr)
                .appendTo("#product_form");
            $("<input type=\"hidden\">")
                .attr("name", "SKU_SKUItemsModels[" + i + "].AttributeGroup")
                .val(this.AttributeGroup)
                .appendTo("#product_form");
            $("<input type=\"hidden\">")
                .attr("name", "SKU_SKUItemsModels[" + i + "].SKU_SKUItemsId")
                .val(this.SKU_SKUItemsId)
                .appendTo("#product_form");
        });

        var i = 0;
        $(self.groupParameters()).each(function () {
            $(this.Data()).each(function() {
                $("<input type=\"hidden\">")
                .attr("name", "ProductParametersModels[" + i + "].PName")
                .val(this.Key)
                .appendTo("#product_form");
                $("<input type=\"hidden\">")
                    .attr("name", "ProductParametersModels[" + i + "].PValue")
                    .val(this.Val)
                    .appendTo("#product_form");
                $("<input type=\"hidden\">")
                    .attr("name", "ProductParametersModels[" + i + "].GroupName")
                    .val(this.GroupName)
                    .appendTo("#product_form");
                $("<input type=\"hidden\">")
                    .attr("name", "ProductParametersModels[" + i + "].Sort")
                    .val(this.DisplayOrder)
                    .appendTo("#product_form");
                i++;
            });
        });

        $(editors).each(function() {
            this.sync();
        });

        var validator = $("#product_form").validate();

        if (validator.form()) {
            $("#product_form").submit();
            $(this).attr("disabled", true);
        }
    });


    //new 参数
    self.groupParameters = ko.observableArray([]);
    $(defaultopts.parameters).each(function () {
        var para = { Key: ko.observable(''), Val: ko.observable(''), Data: ko.observableArray([]), GroupName: '' };
        $(this).each(function() {
            if (!para.GroupName) {
                para.GroupName = this.GroupName;
            }
            para.Data.push({
                GroupName: this.GroupName,
                Key: this.PName,
                Val: this.PValue,
                DisplayOrder: this.Sort
            });
        });
        self.groupParameters.push(para);
    });

    self.tabclick = function (index,place, e) {
        $('span.tab-close').remove();
        var $close = $('<span class="tab-close" title="关闭">x</span>');
        $(e.currentTarget).append($close);
        $close.click(function () {
            debugger;
            self.groupParameters.remove(place);
            //var params = self.groupParameters();
            //self.groupParameters(params);
            //$('.nav-tabs li:first').click();
        });
    }

    //扩展参数
    self.addParameterGroupName = function () {
        bootbox.dialog({
            title: "Add Parameter",
            message: '<input type="text" class="form-control" id="group_name" placeholder="Parameter Group Name"/>',
            buttons: {
                confirm: {
                    label: "Confirm",
                    className: "btn-success",
                    callback: function () {
                        $('#group_name').val();
                        self.groupParameters.push({
                            GroupName: $('#group_name').val(),
                            Key: ko.observable(''),
                            Val: ko.observable(''),
                            Data: ko.observableArray([])
                        });

                        $(this).dialog("close");
                    }
                },
                close: {
                    label: "Close",
                    className: "btn-default",
                    callback: function () {
                        $(this).dialog("close");
                    }
                }
            }
        });
    }

    self.addparameter = function (index, place, e) {
        if (place.Key().length == 0 || place.Val().length==0) {
            $.messager.alert('Tips', 'Key and Value must be filled');
            return;
        }
        place = $.extend(place, { DisplayOrder: place.Data().length + 1 });
        place.Data.push({ GroupName: place.GroupName, Key: place.Key(), Val: place.Val(), DisplayOrder: place.DisplayOrder });
        $('#parameter_grid' + index).bootstrapTable('load', place.Data());
        place.Key('');
        place.Val('');
    }
};