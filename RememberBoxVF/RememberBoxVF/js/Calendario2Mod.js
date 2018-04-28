"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require('@angular/core');
var common_1 = require('@angular/common');
var forms_1 = require('@angular/forms');
var angular_calendar_1 = require('angular-calendar');
var ng_bootstrap_1 = require('@ng-bootstrap/ng-bootstrap');
var module_1 = require('../demo-utils/module');
var component_1 = require('./component');
var DemoModule = (function () {
    function DemoModule() {
    }
    DemoModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                forms_1.FormsModule,
                ng_bootstrap_1.NgbModalModule.forRoot(),
                angular_calendar_1.CalendarModule.forRoot(),
                module_1.DemoUtilsModule
            ],
            declarations: [component_1.DemoComponent],
            exports: [component_1.DemoComponent]
        })
    ], DemoModule);
    return DemoModule;
}());
exports.DemoModule = DemoModule;
