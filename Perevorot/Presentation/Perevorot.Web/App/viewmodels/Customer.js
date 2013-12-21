define(['durandal/app', 'knockout', 'jquery','knockout-jqueryui'], function (app, ko, jq) {
    var _self;
    var addCustomer = function () {
        
        _self = this;
        
        this.isOpen = ko.observable(false);
        this.customerName = ko.observable('Lorem ipsum dolor sit amet.');
        
        this.OpenAddCustomerDialog = function () {
            this.isOpen(true);
        };
        
        this.SaveCustomer = function () {
            console.log("sending data to server" + this.customerName());
            var data = { CustomerName: this.customerName() };
            jq.post("/Customer/Index", data, function(returnedData) {
                alert(returnedData);
            });
        };

    };
 
    return addCustomer;
});