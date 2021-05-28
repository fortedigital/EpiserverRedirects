define("redirectsMenu/RedirectsMenuViewModel", [
    // dojo
    "dojo/_base/declare",
    "dojo/Stateful",
    "epi/dependency",
], function (declare, Stateful, dependency) {

    var clearAllGuid = "00000000-0000-0000-0000-000000000000";
    var clearAllDuplicatesGuid = "00000000-0000-0000-0000-000000000001";

    return declare([Stateful], {
        store: null,

        dialogTitle: "",

        _dialogTitleGetter() {
            if (this.mode === "edit") {
                return "Edit Redirect Rule";
            }

            if (this.mode === "add") {
                return "Add new Redirect Rule";
            }
        },

        mode: "",

        _modeGetter: function () {
            return this.mode;
        },

        _modeSetter: function (value) {
            this.mode = value;
        },

        searchQueryModel: { oldPattern: "", newPattern: "", redirectRuleType: "", redirectType: ""},

        _searchQueryModelGetter: function () {
            return this.searchQueryModel;
        },

        _searchQueryModelSetter: function (value) {
            this.searchQueryModel = value;
        },

        constructor: function () {
            var registry = dependency.resolve("epi.storeregistry");
            this.store = this.store || registry.get("redirectsMenu.redirectsStore");
        },

        getRedirectRules: function () {
            return this.store.query();
        },

        addRedirectRule: function (newModel) {
            return this.store.add(newModel);
        },

        updateRedirectRule: function (model) {
            return this.store.put(model);
        },

        deleteRedirectRule: function (id) {
            return this.store.remove(id);
        },
        
        clearRedirectRules: function() {
            return this.store.remove(clearAllGuid);
        },

        removeDuplicateRules: function () {
            return this.store.remove(clearAllDuplicatesGuid);
        }
    });
});
