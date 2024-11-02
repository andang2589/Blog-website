var AdminUserModule = (function () {
    function DeleteBtn() {

        var userId /*= $('.delete-user-btn').data('id')*/

        $('.delete-user-btn').on('click', function () {
            userId = $(this).data('id');
        })
        $('.delete-confirm-btn').on('click', function () {
            /*var userId = $(this).data('id');*/
            $.ajax({
                url: '/Admin/AdminUser/DeleteUser?userId=' + userId,
                type: 'DELETE',
                data: { userId: userId },
                success: function (response) {
                    $('.row-' + userId).remove();
                    $('#staticBackdrop').modal('hide');
                },
                error: function (xhr, status, error) {
                    console.error('Xóa không thành công:', error);
                    alert('Đã xảy ra lỗi!'); // Thông báo lỗi
                }
            })
            /*window.location.href = '/Admin/AdminUser/DeleteUser?userId=' + userId;*/
        });
    }
    return {
        "DeleteBtn": DeleteBtn
    }
})();