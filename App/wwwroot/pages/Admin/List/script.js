
$(document).ready(() => {


    $('.btn-delete').each(function (index, elem) {

        $(this).click(function (event) {
            event.preventDefault();


            const accLoginName = $(event.target.parentNode.parentNode.parentNode).find('td:nth-child(2)').text();
            console.log(event.target.parentNode, accLoginName);

            $.confirm(`Xác nhận xoá: "${accLoginName}" ?`, function () {
                location.href = event.target.getAttribute('href');
            });
        });

    });
});