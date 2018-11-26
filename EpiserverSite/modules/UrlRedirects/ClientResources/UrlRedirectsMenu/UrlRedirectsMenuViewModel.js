﻿define("urlRedirectsMenu/UrlRedirectsMenuViewModel", [
    // dojo
    "dojo/_base/declare",
    "dojo/Stateful",
    "epi/dependency",
], function (declare, Stateful, dependency) {

    return declare([Stateful], {
        store: null,

        mode: "",

        _modeGetter: function () {
            return this.mode;
        },

        _modeSetter: function (value) {
            this.mode = value;
        },

        searchQueryModel: { oldUrlSearch: "", newUrlSearch: "", typeSearch: "", contentIdSearch: "" },

        _searchQueryModelGetter: function () {
            return this.searchQueryModel;
        },

        _searchQueryModelSetter: function (value) {
            this.searchQueryModel = value;
        },

        constructor: function () {
            var registry = dependency.resolve("epi.storeregistry");
            this.store = this.store || registry.get("urlRedirectsMenu.urlRedirectsStore");
        },

        getUrlRewrites: function () {
            return this.store.query();
        },

        addUrlRewrite: function (newModel) {
            return this.store.add(newModel);
        },

        updateUrlRewrite: function (model) {
            return this.store.put(model, { id: model.id});
        },

        deleteUrlRewrite: function (id) {
            return this.store.remove(id);
        },
    });
});
