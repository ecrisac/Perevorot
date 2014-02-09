requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions',
        'jquery': '../Scripts/jquery-2.0.3.min',
        'jquery-ui': '../Scripts/jquery-ui-1.10.3.min',
        'knockout': '../Scripts/knockout-3.0.0',
        'knockout-jqueryui': '../Scripts/knockout-jqueryui.min',
        'i18next': '../Scripts/i18next.amd.withJQuery-1.7.1.min',
        'moment': '../Scripts/moment.min',
        'datatables': '../Scripts/DataTables-1.9.4/jquery.dataTables',
        'datatablesknockout': '../Scripts/DataTables-1.9.4/knockout-datatables',
        'infobar': 'viewmodels/infobar'
    }
});


define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'durandal/binder', 'i18next'],
// ReSharper disable InconsistentNaming
    function (system, app, viewLocator, binder, i18n) {
// ReSharper restore InconsistentNaming
    //>>excludeStart("build", true);
    //system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Perevorot';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: true
    });
    
    //TODO: Remove later to speed up loading
    setTimeout(appStart, 500);

    var i18NOptions = {
        fallbackLng: 'en',
        ns: {
            namespaces: ['shell', 'welcome'],
            defaultNs: 'shell'
        },
        resGetPath: '/App/locales/__lng__/__ns__.txt',
        useCookie: true,
        cookieName: 'lang',
        getAsync: false // prevents translations being done before resources are loaded
    };

    function appStart(parameters) {
        app.start().then(function () {

            i18n.init(i18NOptions, function () {

                //Call localization on view before binding.
                binder.binding = function (obj, view) {
                    $(view).i18n();
                };

                //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
                //Look for partial views in a 'views' folder in the root.
                viewLocator.useConvention();

                //Show the app by setting the root view model for our application with a transition.
                app.setRoot('viewmodels/shell', 'entrance');
            });
        });
    }
   
});