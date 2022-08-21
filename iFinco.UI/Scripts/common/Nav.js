

var activeli = $('.active-nav').val();
$(activeli).addClass('active');


$('.has-child > a').click(function () {
    if ($(this).closest('.has-child').find('.child-nav:visible').length > 0) {
        $(this).closest('.has-child').find('.child-nav').slideUp('slow');
    } else {
        $('.child-nav').slideUp('slow')
        $(this).closest('.has-child').find('.child-nav').slideDown('slow')
    }
})