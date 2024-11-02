
var LoginModule = (function () {
    function SignUpRedirectBtn() {

        $(".auth-form__signBtn").on("click", function () {
            window.location.href = '/Admin/AdminUser/Create';
        });
    }




    return {
        "SignUpRedirectBtn": SignUpRedirectBtn
    }
})();

$('.auth-form-btn-primary').click(function (event) {
    /*event.preventDefault();*/

    var username = $('.auth-form__input[name="username"]').val();
    var password = $('.auth-form__input[name="password"]').val();

    // Gửi dữ liệu lên server bằng AJAX
    $.ajax({
        type: 'POST',
        url: 'loginJWT', // Đường dẫn đến controller
        data: {
            username: username, 
            password: password,
            rememberMe: true
        },
        success: function (response) {
            // Xử lý kết quả trả về từ controller
            console.log(response);
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi nếu có
            console.error(error);
        }
    });
});


