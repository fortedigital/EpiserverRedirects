define("episerverRedirectsMenu-grid/UrlRedirectsMenuGrid", [
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
    'dijit/Menu',
    'dijit/MenuItem',

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
        Select,
        Menu,
        MenuItem
    ) {

        return declare([_LayoutWidget], {
            _gridClass: declare([Grid, Pagination, Selection, Keyboard, CompoundColumns]),
            grid: null,

            oldUrlSearch: null,
            newUrlSearch: null,
            typeSearch: null,
            redirectStatusCodeSearch: null,
            prioritySearch: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            init: function (store) {
                this.oldUrlSearch = new SearchBox();
                this.newUrlSearch = new SearchBox();
                this.typeSearch = this._createTypeSelect();
                this.redirectStatusCodeSearch = this._createRedirectStatusCodeSelect();
                this.prioritySearch = new SearchBox();

                this.grid = new this._gridClass({
                    columns: [
                        {
                            renderHeaderCell: (node) => {
                                return this._getSearchDomNode(this.oldUrlSearch);
                            },
                            children: [
                                { field: 'oldUrl', label: 'Old Url' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this._getSearchDomNode(this.newUrlSearch);
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
                                return this._getSearchDomNode(this.prioritySearch);
                            },
                            children: [
                                { field: 'priority', label: 'Priority' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                return this.redirectStatusCodeSearch.domNode;
                            },
                            children: [
                                { field: 'redirectStatusCode', label: 'Redirect Type', renderCell: (object, value, node) => node.append(this._getRedirectStatusCodeText(value)) }
                            ]
                        }
                    ],
                    selectionMode: 'single',
                    allowTextSelection: true,
                    cellNavigation: false,
                    store: store,
                    pagingLinks: 1,
                    pagingTextBox: true,
                    firstLastArrows: true,
                    pageSizeOptions: [10, 25, 50]
                }, this.domNode);

                this.grid.set("queryOptions", { sort: [{ attribute: "oldUrl", descending: false }] });

                var gridMenu = new Menu({
                    targetNodeIds: [ this.grid.domNode ],
                    selector: '.dgrid-content .dgrid-row'
                });

                var cellContext = null;

                var menuItem = new MenuItem({
                    label: "Copy to clipboard", onClick: dojo.hitch(this, function(){
                        if(cellContext){
                            this._copyCellTextToClipboard(cellContext);
                        }
                    })
                });

                gridMenu.addChild(menuItem);

                window["gridMenu"] = gridMenu;

                this.grid.on('.dgrid-content .dgrid-cell:contextmenu', dojo.hitch(this, function (event) {
                    cellContext = this.grid.cell(event);
                }));
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

            _createTypeSelect: function() {
                return new Select({
                    name: "typeSelect",
                    options: [
                        { label: "All", value: "" },
                        { label: "System", value: "System" },
                        { label: "Manual", value: "Manual" },
                        { label: "ManualWildcard", value: "ManualWildcard" }
                    ]
                });
            },

            _copyCellTextToClipboard: function(cell) {
                var el = document.createElement('textarea');
                el.value = cell.element.innerText;

                document.body.appendChild(el);

                el.focus();
                el.select();

                try
                {
                    document.execCommand('copy');
                }
                catch (error)
                {
                    console.log("Unable to copy to clipboard");
                }

                document.body.removeChild(el);
            },

            _createRedirectStatusCodeSelect: function () {
                return new Select({
                    name: "redirectStatusCodeSelect",
                    options: [
                        { label: "All", value: "" },
                        { label: "Permanent", value: 301 },
                        { label: "Temporary", value: 302 }
                    ]
                });
            },

            _getSearchDomNode: function (searchBox) {
                var searchContainer = domConstruct.create("div", { "class": "epi-gadgetInnerToolbar" });
                searchContainer.appendChild(searchBox.domNode);

                return searchContainer;
            },

            _getRedirectStatusCodeText: function (statusCode) {
                switch (statusCode) {
                    case 301:
                        return "Permanent";
                    case 302:
                        return "Temporary";
                    default:
                        return statusCode;
                }
            }
        });
    });
