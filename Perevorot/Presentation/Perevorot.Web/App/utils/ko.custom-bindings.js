define(['knockout'], function (ko) {
    
    ko.bindingHandlers.truncatedText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var value = ko.unwrap(valueAccessor());
            if (typeof value !== "string") {
                return;
            }

            console.log(value);
            var length = ko.unwrap(allBindingsAccessor().length) || ko.bindingHandlers.truncatedText.defaultLength;
            var truncatedValue = value.length > length + 2 ? value.substring(0, Math.min(value.length, length)) + "..." : value;
            $(element).attr("title", value);
            ko.bindingHandlers.text.update(element, function () { return truncatedValue; });
        },
        defaultLength: 100
    };

});