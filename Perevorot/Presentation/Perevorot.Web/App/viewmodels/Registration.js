define(['durandal/app', 'knockout', 'jquery', 'knockout-jqueryui'], function(app, ko, jq) {
    var self;
    var addCustomer = function() {

        self = this;
        self.isOpen = ko.observable(false);
        self.customerName = ko.observable();

        self.openRegistrationDialog = function () {
            self.isOpen(true);
        };

        self.SaveCustomer = function() {
            jq.ajax({
                url: "/Customer/AddNewCustomer",
                type: 'POST',
                cache: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    Name: self.customerName()
                }),
                success: function(returnedData) {
                    if (returnedData.Result == "Success") {
                        //cleanup and close window
                        self.customerName('');
                        self.isOpen(false);
                    } else {
                        alert(returnedData.Message);
                    }
                },
                error: function(jqXHR, exception) {
                    alert('Error happened while sending request:' + exception);
                }
            });
        };

    };

    return addCustomer;
});