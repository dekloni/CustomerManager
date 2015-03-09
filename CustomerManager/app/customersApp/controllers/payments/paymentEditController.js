'use strict';

define(['app'], function (app) {

    var injectParams = ['$scope', '$location', '$routeParams',
                        '$timeout', 'config', 'dataService', 'modalService'];

    var PaymentEditController = function ($scope, $location, $routeParams,
                                           $timeout, config, dataService, modalService) {

        var vm = this,
            paymentid = ($routeParams.paymentId) ? parseInt($routeParams.paymentId) : 0,
            timer,
            onRouteChangeOff;

        vm.customer = {};
        vm.states = [];
        vm.title = (paymentid > 0) ? 'Edit' : 'Add';
        vm.buttonText = (paymentid > 0) ? 'Update' : 'Add';
        vm.updateStatus = false;
        vm.errorMessage = '';

    }

    PaymentEditController.$inject = injectParams;

    app.register.controller('PaymentEditController', PaymentEditController);

});