/**
 * Bootstrap Table Chinese translation
 * Author: Zhixin Wen<wenzhixin2010@gmail.com>
 */
(function ($) {
    'use strict';

    $.fn.bootstrapTable.locales['zh-CN'] = {
        formatLoadingMessage: function () {
            return 'Loading data, please wait…';
        },
        formatRecordsPerPage: function (pageNumber) {
            return '每页显示 ' + pageNumber + ' 条记录';
        },
        formatShowingRows: function (pageFrom, pageTo, totalRows) {
            // return '显示第 ' + pageFrom + ' 到第 ' + pageTo + ' 条记录,总共 ' + totalRows + ' 条记录';
            return 'Showing records  ' + pageFrom + ' to  ' + pageTo + ', a total of ' + totalRows + ' records';
        },
        formatSearch: function () {
            return 'Search';
        },
        formatNoMatches: function () {
            return 'No matched record found';
        },
        formatPaginationSwitch: function () {
            return 'Hide / Show Page';
        },
        formatRefresh: function () {
            return 'Refresh';
        },
        formatToggle: function () {
            return 'Switching';
        },
        formatColumns: function () {
            return 'Row';
        }
    };

    //$.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-CN']);

})(jQuery);