define([
    // Dojo
    "dojo",
    "dojo/_base/declare",
    //CMS
    "epi/_Module",
    "epi/dependency",
    "epi/routes"
], function (
    // Dojo
    dojo,
    declare,
    //CMS
    _Module,
    dependency,
    routes
) {
   return declare("alloy.UrlRedirectsModuleInitializer", [_Module], {

        initialize: function () {

            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            //Register the store
            registry.create("alloy.urlRewriteStore", this._getRestPath("urlRewriteStore"));
        },
        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "app", storeName: name });
        }
   });
});