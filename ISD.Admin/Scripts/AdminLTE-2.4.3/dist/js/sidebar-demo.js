$(document).ready(function () {
    // Expand sub menu
    var pathName = window.location.pathname;
    var currentPage = pathName.slice(pathName.lastIndexOf('/') + 1)
    var $currentA = $(".sidebar-menu ul li a[href='" + currentPage + "']");
    $currentA.parent().addClass('active');
    $currentA.closest('.treeview-menu').show();
    $currentA.closest('.treeview').addClass('menu-open');
});