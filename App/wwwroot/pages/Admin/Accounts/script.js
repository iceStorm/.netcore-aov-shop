$(document).ready(() => {

    $('.btn-delete').each(function (index, elem) {

        $(this).click(function (event) {
            event.preventDefault();

            const accEmail = $(event.target.parentNode.parentNode.parentNode).find('td:nth-child(4)').text();
            console.log(event.target.parentNode, accEmail);

            $.confirm(`Xác nhận xoá: "${accEmail}" ?`, function () {
                location.href = event.target.getAttribute('href');
            });
        });
    });
});