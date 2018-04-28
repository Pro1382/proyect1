

function dato() {

    var datoSeleccionado = new Array();

        
    $("#check:checked").each(function () {
        datoSeleccionado.push($(this).val());
    });

    // alert("Datos seleccionados => " + datoSeleccionado);
    $('#serv').val(datoSeleccionado);
}

