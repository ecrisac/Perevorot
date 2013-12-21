define(['plugins/router', 'durandal/app', 'i18next', 'jquery'], function (router, app, i18n, $) {
    return {
        router: router,
        search: function () {
            //It's really easy to show a message box.
            //You can add custom options too. Also, it returns a promise for the user's response.
            app.showMessage('Search not yet implemented...');
        },
        languages: ['en', 'ro'],
        changeLanguage: function (data) {
            document.cookie = 'lang=' + data;
            location.reload();
        },
        activate: function () {
            router.map([
                { route: '', title: 'Welcome', moduleId: 'viewmodels/welcome', nav: true },
                { route: 'perevorot', title: 'Perevorot', moduleId: 'viewmodels/perevorot', nav: true },
                { route: 'Customer', title: 'Customer', moduleId: 'viewmodels/customer', nav: true }
            ]).buildNavigationModel();
            return router.activate();
        }
    };
});