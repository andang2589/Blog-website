var AdminCategoryModule = (function () {


    function ToCreateCategory() {
        $('.create-cate-btn').on('click', function () {
            window.location.href = '/Admin/AdminCategory/CreateCategory';
        })
    }
    function ReturnCateIndex() {
        $('.return-cates-index').on('click', function () {
            window.location.href = '/Admin/AdminCategory/Index';
        })
    }
    function DelCategoryBtn() {
        $('.del-category-btn').on('click', function () {
            var categoryId = $(this).data('id');
            $.ajax({
                url: '/Admin/AdminCategory/DeleteCategory?id=' + categoryId,
                type: 'DELETE',
                success: function () {
                    $('.row-' + categoryId).remove();
                    showAlert();
                },
                error: function (xhr, status, error) {
                    console.error('Xóa không thành công:', error);
                    alert('Đã xảy ra lỗi!'); // Thông báo lỗi
                }
            })
        })
    }
    function showAlert() {
        $('#successAlert').fadeIn(); // Hiện alert
        setTimeout(function () {
            $('#successAlert').fadeOut(); // Ẩn alert sau 3 giây
        }, 10000);
    }
    return {
        "ReturnCateIndex": ReturnCateIndex,
        "ToCreateCategory": ToCreateCategory,
        "DelCategoryBtn": DelCategoryBtn
    }
})();