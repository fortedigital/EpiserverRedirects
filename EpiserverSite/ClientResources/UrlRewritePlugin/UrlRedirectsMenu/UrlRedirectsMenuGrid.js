define("urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuGrid", [
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
                        oldUrl: "Old Url",
                        newUrl: "New Url",
                        type: "Type",
                        contentId: "ContentId"
                    },
                    selectionMode: 'single',
                    cellNavigation: false
                }, this.domNode);
            },

            setData: function (data) {
                this.grid.refresh();
                this.grid.renderArray(data);
            },

            clearSelection: function () {
                this.grid.clearSelection();
            }
        });
    });
