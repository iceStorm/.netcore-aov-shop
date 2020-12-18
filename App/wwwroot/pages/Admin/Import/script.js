
$(document).ready(() => {

    if ($('#Password').val() == '') {
        $('.form-control').each(function (index, elem) {

            if (!$(this).is('select'))
                $(this).val('');
        });
    }


    FilePond.registerPlugin(FilePondPluginFileValidateType);
    FilePond.create(document.querySelector('input[type="file"]'), {
        labelIdle: `Kéo thả file Dữ liệu vào khu vực này -- hoặc <span class="filepond--label-action">Duyệt file</span>`,
        acceptedFileTypes: ['application/json'],
        fileValidateTypeDetectType: (source, type) => new Promise((resolve, reject) => {

            // Do custom type detection here and return with promise

            resolve(type);
        })
    });


    document.getElementById('file').addEventListener('change', onChange);
});


function onChange(event) {
    var reader = new FileReader();
    reader.onload = onReaderLoad;
    reader.readAsText(event.target.files[0]);
}

function onReaderLoad(event) {
    /*console.log(event.target.result);*/
    var obj = JSON.parse(event.target.result);
    assignData(obj);
}

function assignData(obj) {
    $('#RankName').val(obj.rank);
    $('#Price').val(obj.cash);
    $('#LoginName').val(obj.loginName);
    $('#Password').val('TempPassword');

    $('#SkinsCount').val(obj.skins);
    $('#GoldsCount').val(obj.golds);
    $('#HeroesCount').val(obj.heroes);
    $('#ImageUrls').val(obj.thumbnailUrl + '\n' + obj.detailPhotos.join('\n'));


    $('input[type=submit]').show();
}
