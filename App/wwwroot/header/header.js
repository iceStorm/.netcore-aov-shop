

$(document).ready(() => {

    $('.category-item').each(function (index, elem) {

        let subMenu = $(this).find('.sub-menu');
        let subMenuWidth = $(subMenu).innerWidth();

        $(subMenu).css('left', -(subMenuWidth / 2 - ($(this).innerWidth() / 2)));
    });



    /* hide the user-dropdown menu if click outside */
    $(document).mouseup(function (e) {
        var container = $("#user-area");

        // if the target of the click isn't the container nor a descendant of the container
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            $('#chb-user-dropdown').prop('checked', false);
        }
    });


    /* change dropdown triangle background when hover on first user-item-link element */
    $('#user-dropdown .user-dropdown-item:first-child .user-dropdown-link').mouseover(function (e) {
        $(this).parent().parent().addClass('first-item-hovered');
    });
    $('#user-dropdown .user-dropdown-item:first-child .user-dropdown-link').mouseout(function (e) {
        $(this).parent().parent().removeClass('first-item-hovered');
    });
});