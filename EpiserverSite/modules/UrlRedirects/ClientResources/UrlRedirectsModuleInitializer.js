define([
    // Dojo
    "dojo/_base/declare",
    //CMS
    "epi/_Module",
    "epi/routes"
], function (
    // Dojo
    declare,
    //CMS
    _Module,
    routes
) {
    return declare("urlRedirect.UrlRedirectsModuleInitializer", [_Module], {

        initialize: function () {

            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            //Register the store
            registry.create("urlRedirectsComponent.urlRedirectsStore", this._getRestPath("UrlRedirectsComponentStore"));
            registry.create("urlRedirectsMenu.urlRedirectsStore", this._getRestPath("UrlRedirectsMenuStore"));
        },
        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "UrlRedirects", storeName: name });
        }
   });
});