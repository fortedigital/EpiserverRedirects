define("urlRedirectsComponent/UrlRedirectsComponentGrid", [
    "dojo/_base/declare",

    "dgrid/Grid",
    "dgrid/Keyboard",
    "dgrid/Selection",
    'dgrid/OnDemandGrid',

    "dijit/layout/_LayoutWidget"
],

    function (
        declare,

        Grid,
        Keyboard,
        Selection,
        OnDemandGrid,

        _LayoutWidget
    ) {

        return declare([_LayoutWidget], {
            _gridClass: declare([Grid, Selection, Keyboard, OnDemandGrid]),
            grid: null,

            postMixInProperties: function () {
                this.inherited(arguments);
            },

            buildRendering: function () {
                this.inherited(arguments);
            },

            init: function (store) {
                this.grid = new this._gridClass({
                    columns: {
                        oldUrl: "Old Url"
                    },
                    selectionMode: 'single',
                    cellNavigation: false,
                    store: store
                }, this.domNode);
                this.grid.set("showHeader", false);
            },

            setData: function (data) {
                this.grid.refresh();
                this.grid.renderArray(data);
            },

            setQuery: function (searchQueryModel) {
                this.grid.set("query", searchQueryModel);
            },
        });
    });
