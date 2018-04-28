$(document).ready(function ()
{

    $(document).on('click keyup', '.mis-checkboxes,.mis-adicionales', function () {
        calcular();
      
    });

});

function calcular()
{
    var tot = $('#total');
    tot.val(0);
    $('.mis-checkboxes,.mis-adicionales').each(function ()
    {
        if ($(this).hasClass('mis-checkboxes')) {
            tot.val(($(this).is(':checked') ? parseFloat($(this).attr('tu-attr-precio')) : 0) + parseFloat(tot.val()));
        }
        else
        {
            tot.val(parseFloat(tot.val()) + (isNaN(parseFloat($(this).val())) ? 0 : parseFloat($(this).val())));
        }
    });
    var totalParts = parseFloat(tot.val()).toFixed(2).split('.');
    
    document.getElementsById("total").value = tot.val('$' + totalParts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '.' + (totalParts.length > 1 ? totalParts[1] : '00'));
    $('#total').val(tot);
   
}



