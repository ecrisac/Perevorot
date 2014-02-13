/* ********************

Datatables Custom Binding for Knockout.js

******************** */
define(["knockout","jquery"], function (ko,$) {
    /* Custom binding */
    ko.bindingHandlers.dataTable = {
        init: function (element, valueAccessor) {
            var binding = ko.utils.unwrapObservable(valueAccessor());

            // If the binding is an object with an options field,
            // initialise the dataTable with those options. 
            if (binding.options) {
                $(element).dataTable(binding.options);
            }
        },
        update: function (element, valueAccessor) {
            var binding = ko.utils.unwrapObservable(valueAccessor());

            // If the binding isn't an object, turn it into one. 
            if (!binding.data) {
                binding = {
                    data: valueAccessor()
                };
            }
            
            if (!$(element).dataTable().fnSettings()) {
                // Clear table
                $(element).dataTable().fnClearTable();

                // Rebuild table from data source specified in binding
                $(element).dataTable().fnAddData(binding.data());
            }
        }
    };
});