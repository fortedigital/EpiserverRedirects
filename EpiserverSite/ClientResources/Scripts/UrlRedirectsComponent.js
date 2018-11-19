
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
            currentContentGuid: null,

            itemChanged: function (id, item) {
                this.inherited(arguments);

                this.currentContentGuid = item.contentGuid;
                this._getResults(item.contentGuid);
            },

            postCreate: function () {
                on(this.search, "change", this._onSearchChange.bind(this));

                var registry = dependency.resolve("epi.storeregistry");
                this.store = this.store || registry.get("alloy.urlRewriteStore");
            },

            _getResults: function (id) {
                this.store.query({
                    id: id,
                    filter: this.searchText
                }).then(result => {
                    this.urlRedirectsGrid.setData(result);
                });
            },

            _onSearchChange: function (newSearchText) {
                this.searchText = newSearchText;
                this._getResults(this.currentContentGuid);
            }
        });
    });