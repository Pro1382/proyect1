angular.module('RememberBox', ['ngRoute','ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'ui.bootstrap.datetimepicker', 'rzModule', 'pretty-checkable']).constant('uiDatetimePickerConfig', {
    dateFormat: 'yyyy-MM-dd HH:mm',
    defaultTime: '00:00:00',
    html5Types: {
        date: 'yyyy-MM-dd',
        'datetime-local': 'yyyy-MM-ddTHH:mm:ss.sss',
        'month': 'yyyy-MM'
    },
    initialPicker: 'date',
    reOpenDefault: false,
    enableDate: true,
    enableTime: true,
    buttonBar: {
        show: true,
        now: {
            show: true,
            text: 'Ahora'
        },
        today: {
            show: true,
            text: 'Hoy'
        },
        clear: {
            show: true,
            text: 'Limpiar'
        },
        date: {
            show: true,
            text: 'Fecha'
        },
        time: {
            show: true,
            text: 'Hora'
        },
        close: {
            show: true,
            text: 'Cerrar'
        }
    },
    closeOnDateSelection: true,
    closeOnTimeNow: true,
    appendToBody: false,
    altInputFormats: [],
    ngModelOptions: {},
    saveAs: true,
    readAs: true
});