angular.module('RememberBox').controller('aplicacionController',['$scope', '$rootScope', '$http', '$log', '$uibModal', function ($scope, $rootScope, $http, $log, $uibModal) {
    this.accion = 0;
   
    this.verAccion = function (accion) {
        console.log(accion);
        return this.accion === accion;
    };

    this.cambiarAccion = function (accion) {
        this.accion = accion;
        console.log(this.accion);
    };

    this.hacerAccion = function () {
        switch (this.accion) {
            case 1:
                this.registrarPersonaAdministrativo();
                this.cambiarAccion(0);
                break;
            case 2:
                break;

            default:
                break;
        }
        this.cambiarAccion(0);
    };
}]);
