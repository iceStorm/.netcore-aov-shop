

$(document).ready(() => {

    $('.category-item').each(function (index, elem) {

        let subMenu = $(this).find('.sub-menu');
        let subMenuWidth = $(subMenu).innerWidth();

        $(subMenu).css('left', -(subMenuWidth / 2 - ($(this).innerWidth() / 2)));
    });



    /*$(document).mouseup(function (e) {
        console.log(e);
        var container = $(".category-item");

        // if the target of the click isn't the container nor a descendant of the container
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            $('#chb-user-dropdown').prop('checked', false);
        }
    });*/
});