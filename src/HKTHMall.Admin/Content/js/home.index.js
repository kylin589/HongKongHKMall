/// <summary>
///     Author:YiFan
///     Time:2015-07-01 10:00:02
///     Describe:框架js
///     Copyright:
/// </summary>
$(function() {
    changeTabsSettings();
    $(window).resize(function() {
        changeTabsSettings();
    });

    $(".sidebar-toggle").click(function() {
        var $this = $(this);
        var open_flag = $this.data("open") || "0";
        $this.data("open", open_flag == "0" ? "1" : "0");
        changeTabsSettings();

    });

    //右侧菜单栏 单击事件
    $(".js-sidebar").delegate("a", "click", function() {
        var $this = $(this);
        var id = $this.attr("data-id");
        var title = $this.attr("data-title");
        var url = $this.attr("data-url");

        addTab(id, title, url);

        //右侧菜单栏 设置
        $(".js-sidebar li").removeClass("active").removeClass("parent-active");
        $this.parent("li").addClass("active");
        $this.parents(".treeview-menu").show();
    });

    //关闭事件
    $(".js-nav-tabs").delegate(".js-nav-tab-close", "click", function() {
        var $this = $(this);
        var id = $this.attr("data-id");
        if (id) {
            var navTabId = "navTab" + id;
            var tabId = "tab" + id;
            var $nextEle = $("#" + navTabId).parent().next();
            var focusId = 0;
            if ($nextEle.size() == 0) {
                var $prevEle = $("#" + navTabId).parent().prev();
                if ($prevEle.size() != 0) {
                    focusId = $(".js-nav-tab", $prevEle).attr("data-id");
                }
            } else {
                focusId = $(".js-nav-tab", $nextEle).attr("data-id");
            }
            $("#" + navTabId).parent().remove();
            $("#" + tabId).remove();
            changeMenuFocus(focusId);

        }
    });

    //tabs 头单击事件
    $(".js-nav-tabs").delegate(".js-nav-tab", "click", function() {
        var $this = $(this);
        $(".js-sidebar li").removeClass("active");
        $this.tab("show");
        //$this.parents("li").addClass('active');
        $(".js-nav-tabs .js-nav-tab-close").hide();
        $this.next().show();
        changeMenuFocus($this.attr("data-id"));

    });
    $(".js-sidebar a[data-id='11']").click();
});

function addTab(id, title, url) {
    /// <summary>添加新的tab页</summary>
    /// <param name="id" type="string">标识Id</param>
    /// <param name="title" type="string">tab标题</param>
    /// <param name="url" type="string">tab Url地址</param>
    if (id && title && url) {

        var navTabId = "navTab" + id;
        var navTabCloseId = "navTabClose" + id;
        var tabId = "tab" + id;
        var tab = $("#" + tabId);
        var $tabs = $(".js-tab-content");
        var $navTabs = $(".js-nav-tabs");


        //新增tab
        if (tab && tab.size() == 0) {

            //限制打开数量
            if ($(".js-tab-iframe", $tabs).size() > 9) {
                // Tool.Alert("最多打开10个tab页,请先关闭一些！");
                Tool.Alert(" Up to 10 tab pages, please close some of the!");
                return;
            }

            $(".js-nav-tabs li").removeClass("active");
            $(".js-tab-content div").removeClass("active");
            $(".js-nav-tabs .js-nav-tab-close").hide();

            var newNavTab = "<li class='active'>";
            newNavTab += "<a href='#" + tabId + "' data-toggle='tab' class='custom-nav-tab js-nav-tab' id='" + navTabId + "' data-id='" + id + "' title='" + title + "'>" + title;
            //首页无需关闭
            if (id != 0) {
                newNavTab += "<a href='javascript:;' class='custom-nav-tab-close js-nav-tab-close' id='" + navTabCloseId + "' data-id='" + id + "' title='关闭'>x</a>";
            }
            newNavTab += "</a></li>";
            $(newNavTab).appendTo($navTabs);

            var newTab = "<div class='tab-pane active' id='" + tabId + "'>";
            newTab += "<iframe src='" + url + "' frameborder='0' id='iframe" + id + "' name='iframe" + id + "' class='tabs-panel js-tab-iframe'></iframe>";
            newTab += "</div>";
            $(newTab).appendTo($tabs);
            changeTabsSettings();
        } else {
            if (!$("#" + navTabId).parent().hasClass("active")) {
                $(".js-nav-tabs .js-nav-tab-close").hide();
                $(".js-nav-tabs li, .js-tab-content div").removeClass("active");
                $("#" + navTabId).parent().addClass("active");
                $("#" + tabId).addClass("active");
                $("#" + navTabCloseId).show();
                changeTabsSettings();
            }
        }
    }
}

function changeMenuFocus(id) {
    /// <summary>改变菜单聚焦样式</summary>
    /// <param name="id" type="int">聚焦参数Id</param>
    var focusSiderBarId = "siderbar" + id;
    var $focusSiderBar = $("#" + focusSiderBarId);
    var pid = $focusSiderBar.attr("data-pid");
    if (pid != 0) {
        var $parent = $("#siderbar" + pid);
        if ($parent.next("ul").is(":hidden")) {
            $parent.next("ul").show();
        }

    }
    $("#" + focusSiderBarId).click();
    changeTabsSettings();
}

function changeTabsSettings() {
    /// <summary>改变框架Tabs设置</summary>
    var height = $(window).height();
    var isOpen = $(".sidebar-toggle").data("open") == "1";
    var errorWidth = isOpen ? 115 : 295;
    var $tabs = $(".js-tab-content");
    $tabs.css({ height: (height - 160) + "px" });
    $(".js-tab-iframe").each(function(i, dom) {
        $(dom).css({ height: $tabs.height() + "px", width: $(window).width() - errorWidth + "px" });
        if (this.contentWindow.changePageSettings) {
            this.contentWindow.changePageSettings();
        }
    });
}