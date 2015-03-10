﻿'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$filter', '$window',
                        '$timeout', 'authService', 'dataService', 'modalService'];

    var PaymentsController = function ($location, $filter, $window,
        $timeout, authService, dataService, modalService) {

        var vm = this;

        vm.customers = [];
        vm.payments = [];
        vm.filteredCustomers = [];
        vm.filteredPayments = [];
        vm.filteredCount = 0;
        vm.orderby = 'OrderId';
        vm.reverse = false;
        vm.searchText = null;
        vm.cardAnimationClass = '.card-animation';

        //paging
        vm.totalRecords = 0;
        vm.pageSize = 10;
        vm.currentPage = 1;

        vm.pageChanged = function (page) {
            vm.currentPage = page;
            getPaymentsSummary();
        };

        vm.deleteCustomer = function (id) {
            if (!authService.user.isAuthenticated) {
                $location.path(authService.loginPath + $location.$$path);
                return;
            }

            var cust = getCustomerById(id);
            var custName = cust.firstName + ' ' + cust.lastName;

            var modalOptions = {
                closeButtonText: 'Cancel',
                actionButtonText: 'Delete Customer',
                headerText: 'Delete ' + custName + '?',
                bodyText: 'Are you sure you want to delete this payment?'
            };

            modalService.showModal({}, modalOptions).then(function (result) {
                if (result === 'ok') {
                    dataService.deleteCustomer(id).then(function () {
                        for (var i = 0; i < vm.customers.length; i++) {
                            if (vm.customers[i].id === id) {
                                vm.customers.splice(i, 1);
                                break;
                            }
                        }
                        filterCustomers(vm.searchText);
                    }, function (error) {
                        $window.alert('Error deleting payment: ' + error.message);
                    });
                }
            });
        };

        vm.setOrder = function (orderby) {
            //alert('asdasd');
            if (orderby === vm.orderby) {
                vm.reverse = !vm.reverse;
            }
            vm.orderby = orderby;
        };

        vm.alert = function () {
            alert('alerted!');
        };

        vm.DisplayModeEnum = {
            Card: 0,
            List: 1
        };

        vm.changeDisplayMode = function (displayMode) {
            switch (displayMode) {
                case vm.DisplayModeEnum.Card:
                    vm.listDisplayModeEnabled = false;
                    break;
                case vm.DisplayModeEnum.List:
                    vm.listDisplayModeEnabled = true;
                    break;
            }
        };

        vm.navigate = function (url) {
            $location.path(url);
        };

        //vm.setOrder = function (orderby) {
        //    if (orderby === vm.orderby) {
        //        vm.reverse = !vm.reverse;
        //    }
        //    vm.orderby = orderby;
        //};

        vm.searchTextChanged = function () {
            filterCustomers(vm.searchText);
        };

        function init() {
            //createWatches();
            getPaymentsSummary();
        }

        //function createWatches() {
        //    //Watch searchText value and pass it and the customers to nameCityStateFilter
        //    //Doing this instead of adding the filter to ng-repeat allows it to only be run once (rather than twice)
        //    //while also accessing the filtered count via vm.filteredCount above

        //    //Better to handle this using ng-change on <input>. See searchTextChanged() function.
        //    vm.$watch("searchText", function (filterText) {
        //        filterCustomers(filterText);
        //    });
        //}

        function getPaymentsSummary() {
            dataService.getPaymentsSummary(vm.currentPage - 1, vm.pageSize)
            .then(function (data) {
                vm.totalRecords = data.totalRecords;
                vm.payments = data.results;
                filterCustomers(''); //Trigger initial filter

                $timeout(function () {
                    vm.cardAnimationClass = ''; //Turn off animation since it won't keep up with filtering
                }, 1000);

            }, function (error) {
                $window.alert('Sorry, an error occurred: ' + error.data.message);
            });
        }

        function filterCustomers(filterText) {
            vm.filteredPayments = $filter("paymentTypeFilter")(vm.payments, filterText);
            vm.filteredCount = vm.filteredPayments.length;
        }

        function getCustomerById(id) {
            for (var i = 0; i < vm.customers.length; i++) {
                var cust = vm.customers[i];
                if (cust.id === id) {
                    return cust;
                }
            }
            return null;
        }

        init();
    };

   PaymentsController.$inject = injectParams;

   app.register.controller('PaymentsController', PaymentsController);

});