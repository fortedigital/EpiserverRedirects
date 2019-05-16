
define("redirectsComponent/RedirectsComponent",[
    "dojo/_base/declare",
    "dojo/text!./RedirectsComponent.html",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "redirectsComponent/RedirectsComponentGrid",

    "epi-cms/_ContentContextMixin",
    "epi/dependency",

    "xstyle/css!./RedirectsComponent.css"
],
    function (
        declare,
        template,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        RedirectsComponentGrid,

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
                this.store = this.store || registry.get("redirectsComponent.redirectsStore");

                this.redirectsComponentGrid.init(this.store);
            },

            _getResults: function () {
                this.redirectsComponentGrid.setQuery({
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