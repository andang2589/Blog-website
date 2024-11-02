import {
    ClassicEditor,
    AccessibilityHelp,
    Autosave,
    Bold,
    Essentials,
    Italic,
    Mention,
    Paragraph,
    SelectAll,
    Undo,
    Font,
    FontFamily,
    FontSize,
    FontColor,
    FontBackgroundColor,
    Heading,
    Underline,
    Highlight,
    HorizontalLine,
    Image,
    ImageCaption,
    ImageResize,
    ImageStyle,
    ImageToolbar,
    LinkImage,
    ImageInsert,
    Base64UploadAdapter,
    ImageUpload
} from 'ckeditor5';



const editorConfig = {
    toolbar: {
        items: ['undo', 'redo', '|', 'selectAll', '|', 'Heading', 'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', '|', 'bold', 'italic', 'underline', '|', 'horizontalLine', 'highlight', '|', 'ImageInsert'/*, 'uploadImage'*/, '|', 'accessibilityHelp'],
        shouldNotGroupWhenFull: false
    },
    placeholder: 'Type or paste your content here!',
    plugins: [AccessibilityHelp, Autosave, Bold, Essentials, Italic, Mention, Paragraph, SelectAll, Undo, Font, FontFamily, FontSize, FontColor, FontBackgroundColor, Heading, Underline, Highlight, HorizontalLine, Image, ImageCaption, ImageResize, ImageStyle, ImageToolbar, LinkImage, ImageInsert, Base64UploadAdapter, ImageUpload],
    SimpleUpload: {
        uploadUrl: '~/Pictures/assets/images'

    },
    mention: {
        feeds: [
            {
                marker: '@',
                feed: [
                    /* See: https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html */
                ]
            }
        ]
    },

    image: {
        insert: {
            // This is the default configuration, you do not need to provide
            // this configuration key if the list content and order reflects your needs.
            integrations: ['upload', 'assetManager', 'url']
        }
    },

    initialData: "<h2>Congratulations on setting up CKEditor 5! 🎉  </h2>"
};

//let escapedContentFromDatabase = '';


//fetch('/BlogPost/UpdatePost?id=1')
//    .then(response=>response.json())
//    .then(data => {
//        escapedContentFromDatabase = data;

//    })


//function unescapeHtml(escapedHtml) {
//    const textArea = document.createElement('textarea');
//    textArea.innerHTML = escapedHtml;
//    return textArea.value;
//}


//let contentFromDatabase = unescapeHtml(escapedContentFromDatabase);

//ClassicEditor
//    .create(document.querySelector('#editor'), editorConfig)
//    .then(editor => {
//        console.log(editor);
//        console.log(window);
//        window.editor = editor;
//        editor.setData(contentFromDatabase);
//    })
//    .catch(error => {
//        console.error(error);
//    });


//fetch('/BlogPost/UpdatePost?id=1')
//    .then(response=>response.json())
//    .then(data => {
//        contentFromDatabase = data;
//        return ClassicEditor.create(document.querySelector('#editor'), editorConfig);
//    })
//    .then(editor => {
//        window.editor = editor;
//        editor.setData(contentFromDatabase);
//    })
//    .catch(err => {
//        console.error(err);
//    })






let escapedContentFromDatabase = '';
let contentFromDatabase = ''; // Khai báo biến ở đây
//document.getElementById('#edit-btn').addEventListener('click', function () {
//    const postId = this.getAttribute('data-id');

//    const url = `/Admin/AdminBlog/GetUpdatePost?id=${postId}`;
//    // Lấy dữ liệu từ server
//    fetch(url)
//        .then(response => {
//            if (!response.ok) {
//                throw new Error(`HTTP error! status: ${response.status}`);
//            }
//            return response.text(); // Lấy nội dung dưới dạng văn bản
//        })
//        .then(data => {
//            escapedContentFromDatabase = data; // Lưu dữ liệu vào biến

//            // Chuyển đổi ký tự escape về HTML
//            /*contentFromDatabase = unescapeHtml(escapedContentFromDatabase);*/
//            //contentFromDatabase = contentFromDatabase.replace(/\\u003C/g, '<')
//            //    .replace(/\\u003E/g, '>')
//            //    .replace(/\\u0022/g, '"');
//            contentFromDatabase = escapedContentFromDatabase;

//            /*document.body.innerHTML += contentFromDatabase;*/

//            // Khởi tạo CKEditor sau khi có dữ liệu
//            return ClassicEditor.create(document.querySelector('#editor'), editorConfig);
//        })
//        .then(editor => {
//            console.log(editor);
//            window.editor = editor; // Lưu editor vào window
//            editor.setData(contentFromDatabase); // Đặt nội dung vào CKEditor
//        })
//        .catch(error => {
//            console.error('Lỗi:', error);
//        });

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
            editor.setData(escapedContentFromDatabase); // Đặt nội dung vào CKEditor
        })
        .catch(error => {
            console.error('Lỗi:', error);
        });
}


// Hàm để chuyển đổi ký tự escape về HTML
function unescapeHtml(escapedHtml) {
    const textArea = document.createElement('textarea');
    textArea.innerHTML = escapedHtml;
    return textArea.value;
}

var AdminUpdatePostModuleSeNd = (function () {
    function ReturnBtn() {
        $('.return-post-index').on('click', function () {
            window.location.href = '/Admin/AdminBlog/index'
        })
    }

    return {
        "ReturnBtn": ReturnBtn
    }
})();
export const AdminUpdatePostModuleSeNdAd = AdminUpdatePostModuleSeNd
