define("urlRewritePlugin-urlRedirectsComponent/UrlRedirectsComponentGrid", [
    "dojo/_base/declare",
    "dojo/when",

    "dojo/aspect",


    "dgrid/Grid",
    "dgrid/Keyboard",
    "dgrid/Selection",

    "dijit/layout/_LayoutWidget"
],

    function (
        declare,
        when,

        aspect,

        Grid,
        Keyboard,
        Selection,


        _LayoutWidget
    ) {

        return declare([_LayoutWidget], {
            _gridClass: declare([Grid, Selection, Keyboard]),
            grid: null,

            postMixInProperties: function () {
                this.inherited(arguments);
            },

            buildRendering: function () {
                this.inherited(arguments);

                this._setupGrid();
            },

            _setupGrid: function () {
                this.grid = new this._gridClass({
                    columns: {
                        oldUrl: "Old Url"
                    },
                    selectionMode: 'single',
                    cellNavigation: false
                }, this.domNode);
                this.grid.set("showHeader", false);
            },

            setData: function (data) {
                this.grid.refresh();
                this.grid.renderArray(data);
            }
        });
    });
