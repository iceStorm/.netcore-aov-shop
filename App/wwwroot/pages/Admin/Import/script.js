
$(document).ready(() => {

    if ($('#Password').val() == '') {
        $('.form-control').each(function (index, elem) {

            $(this).val('');
        });
    }
    
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


FilePond.parse(document.body);
document.getElementById('file').addEventListener('change', onChange);