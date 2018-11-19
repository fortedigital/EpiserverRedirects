
define([
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsComponent.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "alloy/UrlRedirectsGrid",

    "epi-cms/_ContentContextMixin",
    "epi/dependency",

    "xstyle/css!./UrlRedirectsComponent.css"
],
    function (
        declare,
        template,
        on,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        UrlRedirectsGrid,

        _ContentContextMixin,
        dependency
    ) {
        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin, _ContentContextMixin], {
            templateString: template,
            searchText: "",
            currentContextId: 0,

            itemChanged: function (id, item) {
                this.inherited(arguments);

                this._setCurrentContextId(id);
                this._getResults();
            },

            postCreate: function () {
                this.search.onSearchBoxChange = this._onSearchChange.bind(this);

                var registry = dependency.resolve("epi.storeregistry");
                this.store = this.store || registry.get("alloy.urlRewriteStore");
            },

            _getResults: function () {
                this.store.query({
                    contextId: this.currentContextId,
                    filter: this.searchText
                }).then(result => {
                    this.urlRedirectsGrid.setData(result);
                });
            },

            _onSearchChange: function (newSearchText) {
                this.searchText = newSearchText;
                this._getResults();
            },

            _setCurrentContextId(newCurrentContextId) {
                this.currentContextId = newCurrentContextId.includes("_") ? newCurrentContextId.split("_")[0] : newCurrentContextId;
            }
        });
    });