$(document).ready(() => {

    FilePond.registerPlugin(
        FilePondPluginImagePreview,
        FilePondPluginFileValidateType,
        FilePondPluginFileEncode
    );


    FilePond.create(document.querySelector('input[type="file"]'), {
        labelIdle: `Kéo thả hình ảnh vào khu vực này -- hoặc <span class="filepond--label-action">Duyệt file</span>`,
        acceptedFileTypes: ['image/png', 'image/jpeg'],
        fileValidateTypeDetectType: (source, type) => new Promise((resolve, reject) => {
            // Do custom type detection here and return with promise
            resolve(type);
        })
    });


    FilePond.setOptions({
        server: {
            url: location.origin + '/Account',
            process: '/UploadAvatar',
            revert: '/RemoveAvatar',
        }
    });


    $('.filepond').on('FilePond:addfile', function (e) {
        console.log('file added event', e);
    });
});