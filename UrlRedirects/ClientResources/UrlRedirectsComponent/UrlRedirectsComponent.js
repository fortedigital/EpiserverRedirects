
define([
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsComponent.html",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "urlRedirectsComponent/UrlRedirectsComponentGrid",

    "epi-cms/_ContentContextMixin",
    "epi/dependency",

    "xstyle/css!./UrlRedirectsComponent.css"
],
    function (
        declare,
        template,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        UrlRedirectsComponentGrid,

        _ContentContextMixin,
        dependency
    ) {
        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin, _ContentContextMixin], {
            templateString: template,
            searchText: "",
            currentContentId: 0,

            itemChanged: function (id, item) {
                this.inherited(arguments);

                this._setCurrentContentId(id);
                this._getResults();
            },

            postCreate: function () {
                this.search.onSearchBoxChange = this._onSearchChange.bind(this);

                var registry = dependency.resolve("epi.storeregistry");
                this.store = this.store || registry.get("urlRedirectsComponent.urlRedirectsStore");

                this.urlRedirectsComponentGrid.init(this.store);
            },

            _getResults: function () {
                this.urlRedirectsComponentGrid.setQuery({
                    contentId: this.currentContentId,
                    filter: this.searchText
                });
            },

            _onSearchChange: function (newSearchText) {
                this.searchText = newSearchText;
                this._getResults();
            },

            _setCurrentContentId(newCurrentContentId) {
                this.currentContentId = newCurrentContentId.includes("_") ? newCurrentContentId.split("_")[0] : newCurrentContentId;
            }
        });
    });