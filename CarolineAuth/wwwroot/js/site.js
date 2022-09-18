$(document).ready(function(){
    $("#row").click(function() {
        const english = $("#english");
        if (english.css('visibility') === 'hidden') {
            english.css({'visibility': 'visible'});
        } else
        {
            english.css({'visibility': 'hidden'});
        }
    });
});