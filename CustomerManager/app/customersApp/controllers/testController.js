'use strict';

define(['app'], function (app) {

    //var injectParams = [];
    var injectParams = ['$scope', '$routeParams', '$timeout', '$window', 'dataService'];

    var TestController = function ($scope, $routeParams, $timeout, $window, dataService) {

        $scope.alerts = [
       { type: 'danger', msg: 'Oh snap! Change a few things up and try submitting again.' },
       { type: 'success', msg: 'Well done! You successfully read this important alert message.' }
        ];
        $scope.isCollapsed = false;

        // ui-bootrap for angular
        $scope.addAlert = function () {
            $scope.alerts.push({ msg: 'Another alert!', type: 'info' });
        }
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        // time
        //$scope.today = function () {
        //    $scope.dt = new Date();
        //};
        //$scope.today();

        //$scope.clear = function () {
        //    $scope.dt = null;
        //};

        // Disable weekend selection
        //$scope.disabled = function (date, mode) {
        //    return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        //};

        //$scope.toggleMin = function () {
        //    $scope.minDate = $scope.minDate ? null : new Date();
        //};
        //$scope.toggleMin();

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yyyy',
            startingDay: 1
        };

        $scope.formats = ['dd-MMMM-yyyy HH:mm', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];
    };
    
    TestController.$inject = injectParams;

    app.register.controller('TestController', TestController);

});