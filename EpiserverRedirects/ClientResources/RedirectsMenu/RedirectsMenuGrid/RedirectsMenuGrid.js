define("redirectsMenu-grid/RedirectsMenuGrid", [
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
    "dijit/form/DateTextBox",

    "redirects/Moment",
    "xstyle/css!./RedirectsMenuGrid.css"
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
        DateTextBox,
        moment
    ) {

        return declare([_LayoutWidget], {
            _gridClass: declare([Grid, Pagination, Selection, Keyboard, CompoundColumns]),
            grid: null,

            oldPattern: null,
            newPattern: null,
            contentId: null,
            redirectRuleType: null,
            redirectType: null,
            priority: null,
            redirectOrigin: null,
            isActive: null,
            createdOnFrom: null,
            createdOnTo: null,
            createdBy: null,
            notes: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            init: function (store) {
                this.oldPattern = new SearchBox();
                this.newPattern = new SearchBox();
                this.contentId = new SearchBox();
                this.redirectRuleType = this._createRedirectRuleTypeSelect();
                this.redirectType = this._createRedirectTypeSelect();
                this.priority = new SearchBox();
                this.redirectOrigin = this._createRedirectOriginSelect();
                this.isActive = this._createIsActiveSelect();
                this.createdOnFrom = this._createCreatedOnFromFilter();
                this.createdOnTo = this._createCreatedOnToFilter();
                this.createdBy = new SearchBox();
                this.notes = new SearchBox();

                this.grid = new this._gridClass({
                    columns: [
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.oldPattern));
                            },
                            children: [
                                { field: 'oldPattern', label: 'Old pattern' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.newPattern));
                            },
                            children: [
                                { field: 'newPattern', label: 'New pattern' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.contentId));
                            },
                            children: [
                                { field: 'contentId', label: 'Content Id' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this.redirectRuleType.domNode);
                            },
                            children: [
                                { field: 'redirectRuleType', label: 'Redirect Rule Type', renderCell: (object, value, node) => node.append(this._getRedirectRuleTypeText(value)) }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this.redirectType.domNode);
                            },
                            children: [
                                { field: 'redirectType', label: 'Redirect Type', renderCell: (object, value, node) => node.append(this._getRedirectTypeText(value)) }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.priority));
                            },
                            children: [
                                { field: 'priority', label: 'Priority' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this.redirectOrigin.domNode);
                            },
                            children: [
                                { field: 'redirectOrigin', label: 'Redirect Origin', renderCell: (object, value, node) => node.append(this._getRedirectOriginText(value)) }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this.isActive.domNode);
                            },
                            children: [
                                { field: 'isActive', label: 'Is active', renderCell: (object, value, node) => node.append(this._getIsActiveText(value)) }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.colSpan = 2;
                                node.appendChild(this.createdOnFrom.domNode);
                                return this.createdOnTo.domNode;
                            },
                            children: [
                                { field: 'createdOn', label: 'Created on', colSpan: 2, renderCell: (object, value, node) => node.append(this._getLocalDateTime(value)) }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.createdBy));
                            },
                            children: [
                                { field: 'createdBy', label: 'Created by' }
                            ]
                        },
                        {
                            renderHeaderCell: (node) => {
                                node.appendChild(this._getSearchDomNode(this.notes));
                            },
                            children: [
                                { field: 'notes', label: 'Notes' }
                            ]
                        },
                    ],
                    selectionMode: 'single',
                    cellNavigation: false,
                    store: store,
                    pagingLinks: 1,
                    pagingTextBox: true,
                    firstLastArrows: true,
                    pageSizeOptions: [10, 25, 50]
                }, this.domNode);

                this.grid.set("queryOptions", { sort: [{ attribute: "createdOn", descending: true }] });
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

            _createRedirectRuleTypeSelect: function () {
                return new Select({
                    name: "redirectRuleTypeSelect",
                    options: [
                        { label: "All", value: 0 },
                        { label: "ExactMatch", value: 1 },
                        { label: "Regex", value: 2 },
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

            _createRedirectOriginSelect: function () {
                return new Select({
                    name: "redirectOriginSelect",
                    options: [
                        { label: "All", value: 0 },
                        { label: "System", value: 1 },
                        { label: "Manual", value: 2 },
                        { label: "Import", value: 3 }
                    ]
                });
            },

            _createIsActiveSelect() {
                return new Select({
                    name: "isActiveSelect",
                    options: [
                        { label: "All", value: "" },
                        { label: "Active", value: "true" },
                        { label: "Not active", value: "false" }
                    ]
                });
            },

            _createCreatedOnFromFilter() {
                return new DateTextBox({
                    name: "createdOnFrom",
                    style: "width: 49%;float:left"
                });
            },

            _createCreatedOnToFilter() {
                return new DateTextBox({
                    name: "createdOnTo",
                    style: "width: 49%;float:right"
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
            },

            _getRedirectRuleTypeText: function (redirectRuleType) {
                switch (redirectRuleType) {
                    case 1:
                        return "ExactMatch";
                    case 2:
                        return "Regex";
                    default:
                        return redirectRuleType;
                }
            },

            _getRedirectOriginText: function (redirectOrigin) {
                switch (redirectOrigin) {
                    case 1:
                        return "System";
                    case 2:
                        return "Manual";
                    case 3:
                        return "Import";
                    default:
                        return redirectOrigin;
                }
            },

            _getIsActiveText: function (isActive) {
                return isActive ? "True" : "False";
            },

            _getLocalDateTime: function (utcDateTime) {
                var localDateTime = moment(utcDateTime).local().format('DD-MM-YYYY HH:mm:ss');
                return localDateTime.toString();
            }
        });
    });
