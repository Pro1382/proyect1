var app = angular.module('myApp', ['ui.calendar']);
app.controller('myNgController', ['$scope', '$http', 'uiCalendarConfig', function ($scope, $http, uiCalendarConfig) {
    
    $scope.SelectedEvent = null;
    var isFirstTime = true;

    $scope.evento = [];
    $scope.eventSources = [$scope.evento];


    //Load events from server
    $http.get('/home/GetEvents', {
        cache: true,
        params: {}
    }).then(function (data) {
        $scope.evento.slice(0, $scope.evento.length);
        angular.forEach(data.data, function (value) {
            $scope.evento.push({
                title: value.descripcion,
                description: value.descripcion,
                start: new Date(parseInt(value.fecha.substr(6))),
                end: new Date(parseInt(value.fecha.substr(6))),
                allDay: value.estado,
                stick: true
            });
        });
    });

    //configure calendar
    $scope.uiConfig = {
        calendar: {
            height: 450,
            editable: true,
            displayEventTime: false,
            header: {
                left: 'month basicWeek basicDay agendaWeek agendaDay',
                center: 'title',
                right:'today prev,next'
            },
            eventClick: function (event) {
                $scope.SelectedEvent = event;
            },
            eventAfterAllRender: function () {
                if ($scope.evento.length > 0 && isFirstTime) {
                    //Focus first event
                    uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', $scope.evento[0].start);
                    isFirstTime = false;
                }
            }
        }
    };

}])