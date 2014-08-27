'use strict';

/* Controllers */
angular.module('smsApp.controllers', [function () {
}])
.controller('BindingController', ['$scope', function ($scope) {
    $scope.model = {};
    $scope.model.myInt = 6;

    $scope.addOne = function () {
        $scope.model.myInt++;
    }
}])
.controller('SmsController', ['$scope', '$http', function ($scope, $http) {
    //We define the model
    $scope.model = {};

    //We define the allMessages array in the model 
    //that will contain all the messages sent so far
    $scope.model.allMessages = [];

    //The error if any
    $scope.model.errorMessage = undefined;

    //We initially load data so set the isAjaxInProgress = true;
    $scope.model.isAjaxInProgress = true;

    //Load all the messages
    $http({
        url: '/api/smsresource',
        method: "GET"
    }).
        success(function (data, status, headers, config) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.model.allMessages = data;

            //We are done with AJAX loading
            $scope.model.isAjaxInProgress = false;
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            $scope.model.errorMessage = "Error occurred status:" + status;

            //We are done with AJAX loading
            $scope.model.isAjaxInProgress = false;
        });

    $scope.delete = function (id) {
        //We are making an ajax call so we set this to true
        $scope.model.isAjaxInProgress = true;
        $http({
            url: '/api/smsresource/' + id,
            method: "DELETE"
        }).
              success(function (data, status, headers, config) {
                  // this callback will be called asynchronously
                  // when the response is available
                  $scope.model.allMessages = data;

                  //We are done with AJAX loading
                  $scope.model.isAjaxInProgress = false;
              }).
              error(function (data, status, headers, config) {
                  // called asynchronously if an error occurs
                  // or server returns response with an error status.
                  $scope.model.errorMessage = "Error occurred status:" + status;

                  //We are done with AJAX loading
                  $scope.model.isAjaxInProgress = false;
              });
    }

    $scope.sendMessage = function () {
        $scope.model.errorMessage = undefined;

        var message = '';

        if($scope.model.message != undefined)
            message = $scope.model.message.trim();

        if ($scope.model.phoneNumber == undefined ||
            $scope.model.phoneNumber == '' ||
            $scope.model.phoneNumber.length < 10 ||
            $scope.model.phoneNumber[0] != '+') {
                $scope.model.errorMessage = "You must enter a valid phone number in international format. Eg: +44 7778 609466";
                return;
        }

        if (message.length == 0) {
            $scope.model.errorMessage = "You must specify a message!";
            return;
        }

        //We are making an ajax call so we set this to true
        $scope.model.isAjaxInProgress = true;

        $http({
            url: '/api/smsresource',
            method: "POST",
            data: { to: $scope.model.phoneNumber, message: $scope.model.message }
        }).
              success(function (data, status, headers, config) {
                  // this callback will be called asynchronously
                  // when the response is available
                  $scope.model.allMessages = data;

                  //We are done with AJAX loading
                  $scope.model.isAjaxInProgress = false;
              }).
              error(function (data, status, headers, config) {
                  // called asynchronously if an error occurs
                  // or server returns response with an error status.
                  $scope.model.errorMessage = "Error occurred status:" + status;

                  //We are done with AJAX loading
                  $scope.model.isAjaxInProgress = false;
              });
    }
}])
.controller('HelpController', ['$scope', function ($scope) {

}]);