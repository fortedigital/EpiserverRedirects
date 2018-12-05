define("urlRedirectsMenu-grid/UrlRedirectsMenuGrid", [
    "dojo/_base/declare",
    "dojo/dom-construct",

    "dgrid/Keyboard",
    "dgrid/Selection",
    'dgrid/Grid',
    'dgrid/extensions/Pagination',
    'dgrid/extensions/CompoundColumns',

    'epi/shell/widget/SearchBox',

    "dijit/layout/_LayoutWidget",
    "dijit/form/Select",

    "xstyle/css!./UrlRedirectsMenuGrid.css"
],

    function (
        declare,
        domConstruct,

        Keyboard,
        Selection,
        Grid,
        Pagination,
        CompoundColumns,

        SearchBox,

        _LayoutWidget,
        Select
    ) {

        return declare([_LayoutWidget], {
            _gridClass: declare([Grid, Pagination, Selection, Keyboard, CompoundColumns]),
            grid: null,

            oldUrlSearch: null,
            newUrlSearch: null,
            typeSearch: null,
            prioritySearch: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            init: function (store) {
                this.oldUrlSearch = new SearchBox();
                this.newUrlSearch = new SearchBox();
                this.typeSearch = this.createTypeSelect();
                this.prioritySearch = new SearchBox();

                this.grid = new this._gridClass({
                    columns: [
                        {
                            renderHeaderCell: (node) => {
                                return this.getSearchDomNode(this.oldUrlSearch);
                            },
                            children: [
                                { field: 'oldUrl', label: 'Old Url' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.getSearchDomNode(this.newUrlSearch);
                            },
                            children: [
                                { field: 'newUrl', label: 'New Url' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.typeSearch.domNode;
                            },
                            children: [
                                { field: 'type', label: 'Type' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.getSearchDomNode(this.prioritySearch);
                            },
                            children: [
                                { field: 'priority', label: 'Priority' }
                            ]
                        }
                    ],
                    selectionMode: 'single',
                    cellNavigation: false,
                    store: store,
                    pagingLinks: 1,
                    pagingTextBox: true,
                    firstLastArrows: true,
                    pageSizeOptions: [10, 25, 50]
                }, this.domNode);

                this.grid.set("queryOptions", { sort: [{ attribute: "oldUrl", descending: false }] });
            },

            clearSelection: function () {
                this.grid.clearSelection();
            },

            refresh: function () {
                this.grid.refresh();
            },

            setQuery: function (searchQueryModel) {
                this.grid.set("query", searchQueryModel);
            },

            createTypeSelect: function() {
                return new Select({
                    name: "typeSelect",
                    options: [
                        { label: "all", value: "" },
                        { label: "system", value: "system" },
                        { label: "manual", value: "manual" },
                        { label: "manual-wildcard", value: "manual-wildcard" }
                    ]
                });
            },

            getSearchDomNode: function (searchBox) {
                var searchContainer = domConstruct.create("div", { "class": "epi-gadgetInnerToolbar" });
                searchContainer.appendChild(searchBox.domNode);

                return searchContainer;
            }
        });
    });
