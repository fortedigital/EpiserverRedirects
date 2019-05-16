define("redirectsMenu-grid/UrlRedirectsMenuGrid", [
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

            oldPattern: null,
            newPattern: null,
            redirectRuleType: null,
            redirectType: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            init: function (store) {
                this.oldPattern = new SearchBox();
                this.newPattern = new SearchBox();
                this.redirectRuleType = this._createRedirectRuleTypeSelect();
                this.redirectType = this._createRedirectTypeSelect();

                this.grid = new this._gridClass({
                    columns: [
                        {
                            renderHeaderCell: (node) => {
                                return this._getSearchDomNode(this.oldPattern);
                            },
                            children: [
                                { field: 'oldPattern', label: 'Old pattern' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this._getSearchDomNode(this.newPattern);
                            },
                            children: [
                                { field: 'newPattern', label: 'New pattern' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.redirectRuleType.domNode;
                            },
                            children: [
                                { field: 'redirectRuleType', label: 'Redirect Rule Type' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.redirectType.domNode;
                            },
                            children: [
                                { field: 'redirectType', label: 'Redirect Type', renderCell: (object, value, node) => node.append(this._getRedirectTypeText(value)) }
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

                this.grid.set("queryOptions", { sort: [{ attribute: "oldPattern", descending: false }] });
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

            _createRedirectRuleTypeSelect: function() {
                return new Select({
                    name: "redirectRuleTypeSelect",
                    options: [
                        { label: "All", value: 0},
                        { label: "ExactMatch", value: 1 },
                        { label: "Regex", value: 2 },
                        { label: "Wildcard", value: 3 }
                    ]
                });
            },

            _createRedirectTypeSelect: function () {
                return new Select({
                    name: "redirectTypeSelect",
                    options: [
                        { label: "All", value: 0 },
                        { label: "Permanent", value: 1 },
                        { label: "Temporary", value: 2 }
                    ]
                });
            },

            _getSearchDomNode: function (searchBox) {
                var searchContainer = domConstruct.create("div", { "class": "epi-gadgetInnerToolbar" });
                searchContainer.appendChild(searchBox.domNode);

                return searchContainer;
            },

            _getRedirectTypeText: function (statusCode) {
                switch (statusCode) {
                    case 1:
                        return "Permanent";
                    case 2:
                        return "Temporary";
                    default:
                        return statusCode;
                }
            }
        });
    });
