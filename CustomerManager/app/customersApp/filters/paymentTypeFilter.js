'use strict';

define(['app'], function (app) {

    var nameCityStateFilter = function () {

        return function (customers, filterValue) {
            if (!filterValue) return customers;

            var matches = [];
            filterValue = filterValue.toLowerCase();
            for (var i = 0; i < customers.length; i++) {
                var cust = customers[i];
                if (cust.paymentType.toLowerCase().indexOf(filterValue) > -1) {
                    matches.push(cust);
                }
            }

            return matches;
        };
    };

    app.filter('paymentTypeFilter', nameCityStateFilter);

});