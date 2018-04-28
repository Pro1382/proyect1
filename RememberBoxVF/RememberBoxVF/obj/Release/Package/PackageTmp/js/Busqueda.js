
$(document).ready(function () {

    (function ($) {

        $('#buscar').keyup(function () {

            var rex = new RegExp($(this).val(), 'i');
            $('.buscar1 tr').hide();
            $('.buscar1 tr').filter(function () {
                return rex.test($(this).text());
            }).show();

        })

    }(jQuery));

});

$(document).ready(function () {

    (function ($) {

        $('#buscarc').keyup(function () {

            var rex = new RegExp($(this).val(), 'i');
            $('.buscar2 tr').hide();
            $('.buscar2 tr').filter(function () {
                return rex.test($(this).text());
            }).show();

        })

    }(jQuery));

});