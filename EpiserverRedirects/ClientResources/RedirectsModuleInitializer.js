define([
    // Dojo
    "dojo/_base/declare",
    //CMS
    "epi/_Module",
    "epi/routes"
], function(
    // Dojo
    declare,
    //CMS
    _Module,
    routes
) {
    return declare("redirects.RedirectsModuleInitializer", [_Module], {

        initialize: function () {

            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            //Register the store
            registry.create("redirectsComponent.redirectsStore", this._getRestPath("RedirectsComponentStore"));
            registry.create("redirectsMenu.redirectsStore", this._getRestPath("RedirectRuleStore"));
            registry.create("redirectsMenu.hostStore", this._getRestPath("HostStore/Get"));
            registry.create("redirectsMenu.hostStoreForFilter", this._getRestPath("HostStore/GetForFilter"));
            registry.create("redirectsMenu.contentProviders", this._getRestPath("ContentProviderStore/GetContentProviders"));
            registry.create("redirectsMenu.contentProvidersForFilter", this._getRestPath("ContentProviderStore/GetContentProvidersForFilter"));
        },
        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "Forte.EpiserverRedirects", storeName: name });
        }
   });
});