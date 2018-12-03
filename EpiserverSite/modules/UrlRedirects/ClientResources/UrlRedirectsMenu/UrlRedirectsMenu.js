define("urlRedirectsMenu/UrlRedirectsMenu", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenu.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/Button",
    "dijit/Dialog",

    "urlRedirectsMenu/UrlRedirectsMenuViewModel",
    "urlRedirectsMenu/UrlRedirectsMenuGrid",
    "urlRedirectsMenu/UrlRedirectsMenuForm",

    "xstyle/css!./UrlRedirectsMenu.css"
], function (
    declare,
    template,
    on,

    _WidgetBase,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    Button,
    Dialog,

    UrlRedirectsMenuViewModel,
    UrlRedirectsMenuGrid,
    UrlRedirectsMenuForm
) {
        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            urlRedirectsMenuViewModel: null,
            selectedModel: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            postCreate: function () {
                this.urlRedirectsMenuViewModel = new UrlRedirectsMenuViewModel();
                
                this._initializeGrid();
                this._initializeForm();

                on(this.addButton, "click", this._onAddNewClick.bind(this));
                on(this.editButton, "click", this._onEditClick.bind(this));
                on(this.deleteButton, "click", this._onDeleteClick.bind(this));
                this.deleteButton.set('disabled', true);
                this.editButton.set('disabled', true);

                this.urlRedirectsMenuViewModel.watch("mode", (name, oldValue, value) => {
                    !value ? this.urlRedirectsMenuFormDialog.hide() : this.urlRedirectsMenuFormDialog.show();
                    this.urlRedirectsMenuFormDialog.set("title", this.urlRedirectsMenuViewModel.get("dialogTitle"));
                });

                this.urlRedirectsMenuViewModel.watch("searchQueryModel", (name, oldValue, value) => {
                    this.urlRedirectsMenuGrid.setQuery(value);
                });
            },

            _initializeGrid: function () {
                this.urlRedirectsMenuGrid.init(this.urlRedirectsMenuViewModel.store);
                this.urlRedirectsMenuGrid.on('dgrid-select', this._onSelectedItemChange.bind(this));
                this.urlRedirectsMenuGrid.on('.dgrid-content .dgrid-row:dblclick', (event) => this._onEditClick());

                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                this.urlRedirectsMenuGrid.setQuery(searchQueryModel);

                this.urlRedirectsMenuGrid.oldUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ oldUrlSearch: newValue });
                this.urlRedirectsMenuGrid.newUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ newUrlSearch: newValue });
                on(this.urlRedirectsMenuGrid.typeSearch, "change", (newValue) => this._onSearchChange({ typeSearch: newValue }));
                this.urlRedirectsMenuGrid.prioritySearch.onSearchBoxChange = (newValue) => this._onSearchChange({ prioritySearch: newValue });
            },

            _initializeForm: function () {
                this.urlRedirectsMenuForm.onSaveClick = this._onSaveForm.bind(this);
                this.urlRedirectsMenuForm.onDeleteClick = this._onDeleteClick.bind(this);
                this.urlRedirectsMenuForm.onCancelClick = this._onCancelFormClick.bind(this);
            },

            _updateGrid: function () {
                this.urlRedirectsMenuGrid.refresh();
            },

            _onAddNewClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "add");
                this.urlRedirectsMenuForm.updateView({}, this.urlRedirectsMenuViewModel.get("mode"));
                this.urlRedirectsMenuGrid.clearSelection();
            },

            _onDeleteClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "");
                this.urlRedirectsMenuViewModel.deleteUrlRewrite(this.selectedModel.id).then((response) => this._handleResponse(response));
            },

            _onEditClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "edit");
                this.urlRedirectsMenuForm.updateView(this.selectedModel, this.urlRedirectsMenuViewModel.get("mode"));
            },

            _onSelectedItemChange: function (event) {
                this.selectedModel = event.rows[0].data;
                var isSystemType = this.selectedModel.type === "system";

                this.deleteButton.set('disabled', !this.selectedModel);
                this.editButton.set('disabled', isSystemType);
            },

            _onSaveForm: function (model) {
                var mode = this.urlRedirectsMenuViewModel.get("mode");

                if (mode === "edit") {
                    this.urlRedirectsMenuViewModel.updateUrlRewrite(model)
                        .then(response => this._handleResponse(response), error => this._handleError(error));
                } else if (mode === "add") {
                    this.urlRedirectsMenuViewModel.addUrlRewrite(model)
                        .then(response => this._handleResponse(response), error => this._handleError(error));
                }
            },

            _handleResponse: function (reponse) {
                this._updateGrid();
                this.urlRedirectsMenuViewModel.set("mode", "");
                this.urlRedirectsMenuGrid.clearSelection();
                this.selectedModel = null;
                this.deleteButton.set('disabled', !this.selectedModel);
                this.editButton.set('disabled', !this.selectedModel);
            },

            _handleError: function (error) {
                this.urlRedirectsMenuForm.showDuplicateMessage();
            },

            _onCancelFormClick: function() {
                this.urlRedirectsMenuViewModel.set("mode", "");
            },

            _onSearchChange: function (newValue) {
                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                var newSearchQueryModel = Object.assign(searchQueryModel, newValue);

                this.urlRedirectsMenuViewModel.set("searchQueryModel", newSearchQueryModel);
            }
        });
    });