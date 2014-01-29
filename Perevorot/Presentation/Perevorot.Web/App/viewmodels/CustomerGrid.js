define(["knockout", "jquery", "datatables", "datatablesknockout"], function(ko, $) {

    /* Object code */
    function customerRow(id, name, creationDate) {
        var self = this;
        self.id = id;
        self.name = name;
        self.creationDate = ko.observable(creationDate);

        var completedFields = Math.floor(Math.random() * 21);
        var registeredCalls = Math.floor(Math.random() * 6);
        var details = "";
        self.completedFields = ko.observable(completedFields);
        self.registeredCalls = ko.observable(registeredCalls);
        self.details = details;


        //self.link = ko.computed(function() {
        //    return "/#user/" + self.id;
        //});

        self.nameWithLink = ko.computed(function() {
            return '<a href="#' + self.id + '">' + self.name + '</a>';
        });
        
        self.creationDate = ko.computed(function () {
            return '<b>' + self.creationDate() + '</b>';
        });
        
        self.completedFields = ko.computed(function () {
            return '<span>' + self.completedFields() + '</span>';
        });
        
        self.registeredCalls = ko.computed(function () {
            return '<span>' + self.registeredCalls() + '</span>';
        });
        
        self.details = ko.computed(function () {
            return '<span class="glyphicon glyphicon-cog"></span>';
        });

        //self.actions = ko.computed(function() {
        //    return '<a class="btn btn-danger" data-bind="click: function() {removeMember(' + self.id + ')}">' + '<i class="icon-minus-sign"></i>' + '</a>';
        //});
    }

    var getData = function () {
        var resultArray = [];
        var actionUrl = "../Customer/GetCustomers";
        $.ajax({
            type: "POST",
            async :false,
            url: actionUrl,
            //data: $("#loginForm").serialize(), // serializes the form's elements.
            success: function(data) {
                $.each(data, function (index, value) {
                    resultArray.push(new customerRow(value.Id, value.CustomerName, value.CreationDate));
                });
                
            }
        });
        return resultArray;
    };

    /* View model */
    var customerViewModel = {        
        customers: ko.observableArray([])
    };

    customerViewModel.customerTableRows = ko.computed(function() {
        var self = this;
        var dataFromServer = getData();        

        self.customers(dataFromServer);

        var finalArray = new Array();
        for (var i = 0; i < self.customers().length; i++) {
            var rowArray = new Array();
            rowArray[0] = self.customers()[i].nameWithLink();
            rowArray[1] = self.customers()[i].creationDate();
            rowArray[2] = self.customers()[i].completedFields();
            rowArray[3] = self.customers()[i].registeredCalls();
            rowArray[4] = self.customers()[i].details();
            finalArray.push(rowArray);
        }
       
        return finalArray;
    }, customerViewModel);



    //customerViewModel.removeMember = function(id) {
    //    var self = this;

    //    self.customers.remove(function(groupMember) {
    //        return groupMember.id == id;
    //    });
    //};


    return customerViewModel;
});
