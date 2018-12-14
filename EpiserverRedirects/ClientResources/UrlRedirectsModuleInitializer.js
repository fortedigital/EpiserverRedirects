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
    return declare("episerverRedirects.UrlRedirectsModuleInitializer", [_Module], {

        initialize: function () {

            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            //Register the store
            registry.create("episerverRedirectsComponent.urlRedirectsStore", this._getRestPath("UrlRedirectsComponentStore"));
            registry.create("episerverRedirectsMenu.urlRedirectsStore", this._getRestPath("UrlRedirectsMenuStore"));
        },
        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "EpiserverRedirects", storeName: name });
        }
   });
});