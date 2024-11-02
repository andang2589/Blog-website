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
        items: ['undo', 'redo', '|', 'selectAll', '|','Heading', 'fontSize', 'fontFamily', 'fontColor','fontBackgroundColor', '|', 'bold', 'italic', 'underline', '|', 'horizontalLine', 'highlight', '|', 'ImageInsert'/*, 'uploadImage'*/, '|', 'accessibilityHelp'],
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

ClassicEditor
    .create(document.querySelector('#editor'), editorConfig)
    .then(editor => {
        console.log(editor);
    })
    .catch(error => {
        console.error(error);
    });