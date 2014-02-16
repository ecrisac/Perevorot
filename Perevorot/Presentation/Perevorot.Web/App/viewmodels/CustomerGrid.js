define(["knockout", "jquery", "moment", "datatables", "datatablesknockout", "datatablespagination"], function (ko, $, moment) {
  
    /* View model */
    var customerViewModel = {        
        self: this,
        customers: ko.observableArray([]),
        searchedCustomerName: ko.observable()      
    };

    customerViewModel.search = function() {
        customerViewModel.table.fnFilter(customerViewModel.searchedCustomerName(), 0);
    };
    
    customerViewModel.onInitComplete = function () {
        customerViewModel.table = this;
        var data = this.fnGetData();
        customerViewModel.customers(data);
    };

    customerViewModel.dateRenderer = function (date) {
        return moment(date.aData.CreationDate).format(window.DateTimeFormat);
    };
    
    customerViewModel.getData = function (sSource, aoData, fnCallback, oSettings) {
        if ($.trim(customerViewModel.searchedCustomerName())=="") {
            var resp = {};
            resp.aaData = [];
            resp.sEcho = oSettings.iDraw;
            resp.iTotalRecords = 0;
            resp.iTotalDisplayRecords = 0;
            fnCallback(resp);
            return;
        }        

        oSettings.jqXHR = $.ajax({
            "dataType": 'json',
            "type": "POST",
            "url": sSource,
            "data": aoData,
            "success": fnCallback
        });
    };

    return customerViewModel;
});
