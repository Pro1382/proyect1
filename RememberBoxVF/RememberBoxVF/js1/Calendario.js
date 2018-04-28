//prueba
$(document).ready(function () {
    var events = [];
    $.ajax({
        type: "GET",
        url: "/home/GetEvents",
        success: function (data) {
            $.each(data, function (i, v) {
                events.push({
                    title: v.descripcion,
                    description: v.descripcion,
                    start: moment(v.fecha),
                    end: v.fecha != null ? moment(v.fecha) : null,
                    color: v.ThemeColor,
                    allDay: v.IsFullDay
                });
            })

            show_events(events);
        },
        error: function (error) {
            alert('failed');
        }
    })
})
//fin de la prueba


// Configurar el calendario con la fecha actual
$(document).ready(function () {
   

	var date = new Date();
	var today = date.getDate();

	// Establecer manejadores de clic para elementos DOM
	$(".right-button").click({date: date}, next_year);
	$(".left-button").click({date: date}, prev_year);
	$(".month").click({date: date}, month_click);
	$("#add-button").click({date: date}, new_event);
	// Establecer el mes actual como activo
	$(".months-row").children().eq(date.getMonth()).addClass("active-month");
	init_calendar(date);
	var events = check_events(today, date.getMonth()+1, date.getFullYear());
	show_events(events, months[date.getMonth()], today);
});

// Inicialice el calendario agregando las fechas HTML
function init_calendar(date) {
	$(".tbody").empty();
	$(".events-container").empty();
	var calendar_days = $(".tbody");
	var month = date.getMonth();
	var year = date.getFullYear();
	var day_count = days_in_month(month, year);
	var row = $("<tr class='table-row'></tr>");
	var today = date.getDate();
	// Establezca la fecha en 1 para encontrar el primer día del mes
	date.setDate(1);
	var first_day = date.getDay();

	// 35 + firstDay es la cantidad de elementos de fecha que se agregarán a la tabla de fechas
	// 35 es de (7 días en una semana) * (hasta 5 filas de fechas en un mes)
	for(var i=0; i<35+first_day; i++) {

		// Como algunos de los elementos estarán en blanco,
		// necesitamos calcular la fecha real del índice
		var day = i-first_day+1;

		// Si es domingo, haz una nueva fila
		if(i%7===0) {
			calendar_days.append(row);
			row = $("<tr class='table-row'></tr>");
		}
		// si el índice actual no es un día de este mes, déjelo en blanco
		if(i < first_day || day > day_count) {
			var curr_date = $("<td class='table-date nil'>"+"</td>");
			row.append(curr_date);
		}   
		else {
			var curr_date = $("<td class='table-date'>"+day+"</td>");
			var events = check_events(day, month+1, year);
			if(today===day && $(".active-date").length===0) {
				curr_date.addClass("active-date");
				show_events(events, months[month], day);
			}

			// Si esta fecha tiene algún evento, ajústelo con .event-date
			if(events.length!==0) {
				curr_date.addClass("event-date");
			}

			// Establecer el controlador onClick para hacer clic en una fecha
			curr_date.click({events: events, month: months[month], day:day}, date_click);
			row.append(curr_date);
		}
	}

	// Añadir la última fila y establecer el año actual
	calendar_days.append(row);
	$(".year").text(year);
}


// Obtenga la cantidad de días en un mes / año determinado
function days_in_month(month, year) {
	var monthStart = new Date(year, month, 1);
	var monthEnd = new Date(year, month + 1, 1);
	return (monthEnd - monthStart) / (1000 * 60 * 60 * 24);    
}


// Controlador de eventos para cuando se hace clic en una fecha
function date_click(event) {
	$(".events-container").show(250);
	$("#dialog").hide(250);
	$(".active-date").removeClass("active-date");
	$(this).addClass("active-date");
	show_events(event.data.events, event.data.month, event.data.day);
};


// Controlador de eventos para cuando se hace clic en un mes
function month_click(event) {
	$(".events-container").show(250);
	$("#dialog").hide(250);
	var date = event.data.date;
	$(".active-month").removeClass("active-month");
	$(this).addClass("active-month");
	var new_month = $(".month").index(this);
	date.setMonth(new_month);
	init_calendar(date);
}


// Controlador de eventos para cuando se hace clic en el botón derecho del año
function next_year(event) {
	$("#dialog").hide(250);
	var date = event.data.date;
	var new_year = date.getFullYear()+1;
	$("year").html(new_year);
	date.setFullYear(new_year);
	init_calendar(date);
}


// Controlador de eventos para cuando se hace clic en el botón izquierdo del año.
function prev_year(event) {
	$("#dialog").hide(250);
	var date = event.data.date;
	var new_year = date.getFullYear()-1;
	$("year").html(new_year);
	date.setFullYear(new_year);
	init_calendar(date);
}


// Controlador de eventos para hacer clic en el nuevo botón de evento
function new_event(event) {

	// si una fecha no está seleccionada, entonces no hagas nada
	if($(".active-date").length===0)
		return;

	// eliminar la entrada de error rojo al hacer clic
	$("input").click(function(){
		$(this).removeClass("error-input");
	})

	// entradas vacías y ocultar eventos
	$("#dialog input[type=text]").val('');
	$("#dialog input[type=number]").val('');
	$(".events-container").hide(250);
	$("#dialog").show(250);
	// Manejador de eventos para el botón cancelar
	$("#cancel-button").click(function() {
		$("#name").removeClass("error-input");
		$("#count").removeClass("error-input");
		$("#dialog").hide(250);
		$(".events-container").show(250);
	});


    //pruebas
	var dato1 = new Array();

	$("#enviardato").unbind().click({ date: event.data.date }, function () {

	    dato1.push($('#dato1').val());

	    var dato2 = $('#dato2').val();
	    var dato3 = $('#dato3').val();
	    var date = event.data.date;
	    var day = parseInt($(".active-date").html());
	    //dato1 = $('#dato1').val();
	    if (dato2 == date || dato2 == day) {
	        var dato2 = $('#dato2').val();
	        var date = dato2;
	        var day = dato2;

	        new_event_json(dato1, dato2, date, day);
	        date.setDate(day);
	        init_calendar(date);
	    }
	    else {

	        new_event_json(dato1, dato3, date, day);
	        date.setDate(day);
	        init_calendar(date);
	        //saco el valor accediendo al id del input = nombre
	        //alert(dato1);
	        //alert(dato2);
	    }

	});

    //fin de pruebas




	// Manejador de eventos para el botón Aceptar
	$("#ok-button").unbind().click({date: event.data.date}, function() {
		var date = event.data.date;
		var name = $("#name").val().trim();
		var count = parseInt($("#count").val().trim());
		var day = parseInt($(".active-date").html());

		// Validación de formulario básico
		if(name.length === 0) {
			$("#name").addClass("error-input");
		}
		else if(isNaN(count)) {
			$("#count").addClass("error-input");
		}
		else {
			$("#dialog").hide(250);
			console.log("new event");
			new_event_json(name, count, date, day);
			date.setDate(day);
			init_calendar(date);
		}
	});


   
}


// Agrega un evento json a event_data
function new_event_json(dato1, dato2, date, day) {
	var event = {
		"occasion": dato1,
		"invited_count": dato2,
		"year": date.getFullYear(),
		"month": date.getMonth()+1,
		"day": day
	};
	event_data["events"].push(event);
}


// Mostrar todos los eventos de la fecha seleccionada en vistas de tarjeta
function ccc(events, month, day) {



	// Clear the dates container
	$(".events-container").empty();
	$(".events-container").show(250);
	console.log(event_data["events"]);
	// Si no hay eventos para esta fecha, notifique al usuario
	if(events.length===0) {
		var event_card = $("<div class='event-card'></div>");
		var event_name = $("<div class='event-name'>No hay eventos para hoy  "+day+" "+month+".</div>");
		$(event_card).css({ "border-left": "10px solid #FF1744" });
		$(event_card).append(event_name);
		$(".events-container").append(event_card);
	}
	else {

		// Revisa y agrega cada evento como una tarjeta al contenedor de eventos
		for(var i=0; i<events.length; i++) {
			var event_card = $("<div class='event-card'></div>");
			var event_name = $("<div class='event-name'>"+events[i]["occasion"]+":</div>");
			var event_count = $("<div class='event-count'>"+events[i]["invited_count"]+" </div>");
			if(events[i]["cancelled"]===true) {
				$(event_card).css({
					"border-left": "10px solid #FF1744"
				});
				event_count = $("<div class='event-cancelled'>Cancelado</div>");
			}
			$(event_card).append(event_name).append(event_count);
			$(".events-container").append(event_card);
		}
	}
}


// Comprueba si una fecha específica tiene algún evento
function check_events(day, month, year) {
	var events = [];
	for(var i=0; i<event_data["events"].length; i++) {
		var event = event_data["events"][i];
		if(event["day"]===day &&
			event["month"]===month &&
			event["year"]===year) {
				events.push(event);
			}
	}
	return events;
}

// Datos dados para eventos en formato JSON
var event_data = {
	"events": [
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
		{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10
	},
		{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10
	},
		{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10
	},
		{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10,
		"cancelled": true
	},
	{
		"occasion": " Repeated Test Event ",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 10
	},
	{
		"occasion": " Test Event",
		"invited_count": 120,
		"year": 2017,
		"month": 5,
		"day": 11
	}
	]
};

const months = [ 
	"Enero", 
	"Febrero", 
	"Marzo", 
	"Abril", 
	"Mayo", 
	"Junio", 
	"Julio", 
	"Agosto", 
	"Septiembre", 
	"Octubre", 
	"Noviembre", 
	"Diciembre" 
];

