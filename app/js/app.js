'use strict'

// Declare app level module which depends on filters, and services
angular.module('smsApp', ['smsApp.controllers', function () {
}])
//Configure the routes
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/binding', {
        templateUrl: '/app/partials/bindingexample.html',
        controller: 'BindingController'
    });

    $routeProvider.when('/sms', {
        templateUrl: '/app/partials/smsview.html',
        controller: 'SmsController'
    });

    $routeProvider.when('/help', {
        templateUrl: '/app/partials/help.html',
        controller: 'HelpController'
    });

    $routeProvider.otherwise({ redirectTo: '/sms' });
}]);