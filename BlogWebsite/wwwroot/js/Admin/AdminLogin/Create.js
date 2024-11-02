var CreateUserModule = (function () {

    /*Index View*/
    function GoCreateView() {
        $("#create-user-btn").on('click', function () {
            $.ajax({
                url: '/Admin/AdminUser/Create',
                type: 'GET',
                success: function (data) {
                    window.location.href = '/Admin/AdminUser/Create'
                },
                error: function (error) {
                    console.error('Có lỗi đã xảy ra: ', error);
                }
            })
        })
    }



    /*Create View*/
    function ReturnMainPage() {
        $("#return-btn").on('click', function () {
            $.ajax({
                url: '/Admin/AdminUser/Index',
                type: 'GET',
                success: function (data) {
                    window.location.href = '/Admin/AdminUser/Index';
                },
                error: function (error) {
                    console.error('Có lỗi đã xảy ra: ', error);
                }
            })
        })

    }



    return {
        "ReturnMainPage": ReturnMainPage,
        "GoCreateView": GoCreateView
    }
})();