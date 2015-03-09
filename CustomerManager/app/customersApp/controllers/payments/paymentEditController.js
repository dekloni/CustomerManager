'use strict';

define(['app'], function (app) {

    var injectParams = ['$scope', '$location', '$routeParams',
                        '$timeout', 'config', 'dataService', 'modalService'];


    var paymentEditController = function ($scope, $location, $routeParams,
                                           $timeout, config, dataService, modalService) {

        var vm = this,
            paymentid = ($routeParams.paymentId) ? parseInt($routeParams.paymentId) : 0,
            timer,
            onRouteChangeOff;

        //vm.customer = {};
        //vm.states = [];
        vm.title = (paymentid > 0) ? 'Edit' : 'Add';
        vm.buttonText = (paymentid > 0) ? 'Update' : 'Add';
        vm.updateStatus = false;
        vm.errorMessage = '';

        var init = function () {

            if (paymentid > 0) {
                dataService.getPayment(paymentid).then(function(payment) {
                    vm.payment = payment;
                }, processError);
            }

        };

        init();


        function processError(error) {
            vm.errorMessage = error.data.message;
            startTimer();
        }


        function startTimer() {
            timer = $timeout(function () {
                $timeout.cancel(timer);
                vm.errorMessage = '';
                vm.updateStatus = false;
            },10000);
        }


        function processSuccess() {
            //$scope.editForm.$dirty = false;
            vm.updateStatus = true;
            vm.title = 'Edit';
            vm.buttonText = 'Update';
            startTimer();
        }


        vm.savePayment = function () {
            if ($scope.editForm.$valid) {
                if (!vm.payment.id) {
                    dataService.insertPayment(vm.payment).then(processSuccess, processError);
                }
                else {
                    dataService.updatePayment(vm.payment).then(processSuccess, processError);
                }
            }
        };



    }

    paymentEditController.$inject = injectParams;

    app.register.controller('PaymentEditController', paymentEditController);

});