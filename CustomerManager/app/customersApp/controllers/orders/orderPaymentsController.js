'use strict';

define(['app'], function (app) {

    var injectParams = ['$scope', '$routeParams', '$timeout', '$window', 'dataService'];

    var OrderPaymentsController = function ( $scope, $routeParams, $timeout, $window, dataService )
    {
        var vm = this,
            timer;
        vm.orderId = ($routeParams.orderid);
        

        init();

        function init() {

            dataService.getOrder(vm.orderId).then(function (order) {
                vm.order = order;
            }, processError);


        }

        function processError(error) {
            vm.errorMessage = error.data.message;
            startTimer();
        }


        function startTimer() {
            timer = $timeout(function () {
                $timeout.cancel(timer);
                vm.errorMessage = '';
                vm.updateStatus = false;
            }, 3000);
        }
        //function getCustomers() {
        //    dataService.getCustomers(orderId)
        //        .then(function (data) {
        //            vm.totalRecords = data.totalRecords;
        //            vm.customers = data.results;
        //            filterCustomersProducts('');
        //        }, function (error) {
        //            $window.alert(error.message);
        //        });
        //}

    }


    OrderPaymentsController.$inject = injectParams;

    app.register.controller('OrderPaymentsController', OrderPaymentsController);

});