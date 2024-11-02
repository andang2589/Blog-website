var AdminBlogModule = (function () {
    function EditBtn() {
        $(".edit-btn").on('click', function () {
            var postId = $(this).data('id');
            window.location.href = "/Admin/AdminBlog/UpdatePost?id=" + postId;
            //$.ajax({
            //    url: "/Admin/AdminBlog/UpdatePost?id=" + postId,
            //    type: 'GET',
            //    success: function (data) {
            //        window.location.href = "/Admin/AdminBlog/UpdatePost?id=" + postId;
            //    },
            //    error: function (xhr, status, error) {
            //        console.error("Có lỗi xảy ra: ", error);
            //    }
            //});
        });
    }

    function PassDataId() {

        //$('#edit-btn').on('click', function () {
        //    const postId = this.getAttribute('data-id');

        //    const url = `/Admin/AdminBlog/GetUpdatePost?id=${postId}`;
        //    $.ajax({
        //        url: url,
        //        type: 'GET',
        //        success: function()
        //    })
        //})

        const urlParams = new URLSearchParams(window.location.search);
        const postId = urlParams.get('id');
        
        if (postId) {
            const url = `/Admin/AdminBlog/GetUpdatePost?id=${postId}`;
            fetch(url)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.text(); // Lấy nội dung dưới dạng văn bản
                })
                .then(data => {
                    escapedContentFromDatabase = data; // Lưu dữ liệu vào biến                   
                    return ClassicEditor.create(document.querySelector('#editor'), editorConfig);
                })
                .then(editor => {
                    console.log(editor);
                    window.editor = editor; // Lưu editor vào window
                    editor.setData(contentFromDatabase); // Đặt nội dung vào CKEditor
                })
                .catch(error => {
                    console.error('Lỗi:', error);
                });
        }
        
    }

    function CreatePostBtn() {
        $('#create-post-btn').on('click', function () {
            window.location.href = '/Admin/AdminBlog/CreatePost'
        });
    }

    function DeletePostBtn() {
        var postId
        var postTitle
        $('.delete-post-btn').on('click', function () {
            postId = $(this).data('id');
            postTitle = $(this).data('title');
            $('#postTitle').text(postTitle);
        })
        $('.delete-confirm-btn').on('click', function () {
            $.ajax({
                url: '/Admin/AdminBlog/DeletePost?id=' + postId,
                type: 'DELETE',
                success: function () {
                    $('.row-' + postId).remove();
                    $('#staticBackdrop').modal('hide');
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
        "EditBtn": EditBtn,
        "CreatePostBtn": CreatePostBtn,
        "PassDataId": PassDataId,
        "DeletePostBtn": DeletePostBtn
    }
})();