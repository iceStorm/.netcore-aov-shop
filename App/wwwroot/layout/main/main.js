
$(document).ready(() => {

    $('#content').css('padding-top', $('#header').innerHeight() + 30);


    if ($('#content').hasScrollBar()) {
        let scrollbarWidth = getScrollbarWidth();
        let headerPadding = $('#header').css('padding-right');
        console.log(scrollbarWidth, headerPadding);
        $('#header').css('padding-right', parseInt(headerPadding) + scrollbarWidth);
    }
});


function getScrollbarWidth() {

    // Creating invisible container
    const outer = document.createElement('div');
    outer.style.visibility = 'hidden';
    outer.style.overflow = 'scroll'; // forcing scrollbar to appear
    outer.style.msOverflowStyle = 'scrollbar'; // needed for WinJS apps
    document.body.appendChild(outer);

    // Creating inner element and placing it in the container
    const inner = document.createElement('div');
    outer.appendChild(inner);

    // Calculating difference between container's full width and the child width
    const scrollbarWidth = (outer.offsetWidth - inner.offsetWidth);

    // Removing temporary elements from the DOM
    outer.parentNode.removeChild(outer);

    return scrollbarWidth;
}

(function ($) {
    $.fn.hasScrollBar = function () {
        return this[0] ? this[0].scrollHeight > this.innerHeight() : false;
    }
})(jQuery);