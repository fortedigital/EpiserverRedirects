define("episerverRedirectsMenu/UrlRedirectsMenuViewModel", [
    // dojo
    "dojo/_base/declare",
    "dojo/Stateful",
    "epi/dependency",
], function (declare, Stateful, dependency) {

    return declare([Stateful], {
        store: null,

        dialogTitle: "",

        _dialogTitleGetter() {
            if (this.mode === "edit") {
                return "Edit Url Redirect";
            }

            if (this.mode === "add") {
                return "Add new Url Redirect";
            }
        },

        mode: "",

        _modeGetter: function () {
            return this.mode;
        },

        _modeSetter: function (value) {
            this.mode = value;
        },

        searchQueryModel: { oldUrlSearch: "", newUrlSearch: "", typeSearch: "", prioritySearch: "", redirectStatusCodeSearch: "" },

        _searchQueryModelGetter: function () {
            return this.searchQueryModel;
        },

        _searchQueryModelSetter: function (value) {
            this.searchQueryModel = value;
        },

        constructor: function () {
            var registry = dependency.resolve("epi.storeregistry");
            this.store = this.store || registry.get("episerverRedirectsMenu.urlRedirectsStore");
        },

        getUrlRewrites: function () {
            return this.store.query();
        },

        addUrlRewrite: function (newModel) {
            return this.store.add(newModel);
        },

        updateUrlRewrite: function (model) {
            return this.store.put(model);
        },

        deleteUrlRewrite: function (id) {
            return this.store.remove(id);
        },
    });
});
