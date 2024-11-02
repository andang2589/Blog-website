var PostModule = (function () {
    function passCommentPostId() {
        var replyModal = $('#replyModal');

        replyModal.on('show.bs.modal', function (e) {
            var button = e.relatedTarget;
            var commentId = $(button).data('comment-id');
            var postId = $(button).data('blog-id');
            replyModal.find('.modal-body input[name="ReplyToCommentId"]').val(commentId);
            replyModal.find('.modal-body [name="BlogPostID"]').val(postId);
        });
    }

    return {
        "passCommentPostId": passCommentPostId
    }
})();

