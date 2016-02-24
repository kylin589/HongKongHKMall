$(function () {
    $.extend($.fn.bootstrapTable.defaults, {
        height: $(window).height(),
        striped: true,
        sidePagination: 'server',
        pagination: true,
        showRefresh: true,
        showColumns: true
    });
});
