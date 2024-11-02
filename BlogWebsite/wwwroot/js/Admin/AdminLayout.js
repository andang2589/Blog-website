var AdminLayoutModule = (function () {
    function navigateSidebar() {

        

        $('#main-sidebar-link').on('click', function () {
            window.location.href = '/Admin/AdminMain/index';
        })
        $('#user-sidebar-link').on('click', function () {
            window.location.href = '/Admin/AdminUser/index';
        })

        $('#post-sidebar-link').on('click', function () {
            window.location.href = '/Admin/AdminBlog/index';
        })

        $('#category-sidebar-link').on('click', function () {
            window.location.href = '/Admin/AdminCategory/index';
        })

        $('#role-sidebar-link').on('click', function () {
            window.location.href = '/Admin/AdminRole/ListRoles';
        })

        $('#sign-out-btn').on('click', function () {
            window.location.href = '/Admin/AdminLogin/Logout';
        })
    }

    return {
        "navigateSidebar": navigateSidebar
    }
})();

