define("urlRedirectsMenu/UrlRedirectsMenu", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenu.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/Button",
    "dijit/Dialog",
    "dijit/form/TextBox",

    "urlRedirectsMenu/UrlRedirectsMenuViewModel",
    "urlRedirectsMenu-grid/UrlRedirectsMenuGrid",
    "urlRedirectsMenu-form/UrlRedirectsMenuForm",

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
    TextBox,

    UrlRedirectsMenuViewModel,
    UrlRedirectsMenuGrid,
    UrlRedirectsMenuForm,
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
                on(this.refreshButton, "click", this._refreshView.bind(this));
                on(this.simulateFindButton, "click", this._onSimulateFindClick.bind(this));
                on(this.simulateResetButton, "click", this._onSimulateResetClick.bind(this));
                this.deleteButton.set('disabled', true);
                this.editButton.set('disabled', true);

                this.urlRedirectsMenuViewModel.watch("mode", (name, oldValue, value) => {
                    !value ? this.urlRedirectsMenuFormDialog.hide() : this.urlRedirectsMenuFormDialog.show();
                    this.urlRedirectsMenuFormDialog.set("title", this.urlRedirectsMenuViewModel.get("dialogTitle"));
                    this.deleteButton.set('disabled', !value);
                });

                this.urlRedirectsMenuViewModel.watch("searchQueryModel", (name, oldValue, value) => {
                    this.urlRedirectsMenuGrid.setQuery(value);
                });
            },

            _initializeGrid: function () {
                this.urlRedirectsMenuGrid.init(this.urlRedirectsMenuViewModel.store);
                this.urlRedirectsMenuGrid.on('dgrid-select', this._onSelectedItemChange.bind(this));
                this.urlRedirectsMenuGrid.on('.dgrid-content .dgrid-row:dblclick', this._onEditClick.bind(this));

                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                this.urlRedirectsMenuGrid.setQuery(searchQueryModel);

                this.urlRedirectsMenuGrid.oldUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ oldUrlSearch: newValue });
                this.urlRedirectsMenuGrid.newUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ newUrlSearch: newValue });
                on(this.urlRedirectsMenuGrid.typeSearch, "change", (newValue) => this._onSearchChange({ typeSearch: newValue }));
                on(this.urlRedirectsMenuGrid.redirectStatusCodeSearch, "change", (newValue) => this._onSearchChange({ redirectStatusCodeSearch: newValue }));
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
                this.urlRedirectsMenuViewModel.deleteUrlRewrite(this.selectedModel.id).then((response) => this._refreshView());
            },

            _onEditClick: function () {
                if (this.selectedModel.type === "System") return;

                this.urlRedirectsMenuViewModel.set("mode", "edit");
                this.urlRedirectsMenuForm.updateView(this.selectedModel, this.urlRedirectsMenuViewModel.get("mode"));
            },

            _onSelectedItemChange: function (event) {
                this.selectedModel = event.rows[0].data;

                this.deleteButton.set('disabled', !this.selectedModel);
                this.editButton.set('disabled', this.selectedModel.type === "System");
            },

            _onSaveForm: function (model) {
                var mode = this.urlRedirectsMenuViewModel.get("mode");

                if (mode === "edit") {
                    this.urlRedirectsMenuViewModel.updateUrlRewrite(model)
                        .then(response => this._refreshView(), error => this._handleError(error));
                } else if (mode === "add") {
                    this.urlRedirectsMenuViewModel.addUrlRewrite(model)
                        .then(response => this._refreshView(), error => this._handleError(error));
                }
            },

            _refreshView: function () {
                this._updateGrid();
                this.urlRedirectsMenuViewModel.set("mode", "");
                this.urlRedirectsMenuGrid.clearSelection();
                this.selectedModel = null;
                this.deleteButton.set("disabled", !this.selectedModel);
                this.editButton.set("disabled", !this.selectedModel);
            },

            _handleError: function (error) {
                this.urlRedirectsMenuForm.showDuplicateMessage();
            },

            _onCancelFormClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "");
            },

            _onSearchChange: function (newValue) {
                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                var newSearchQueryModel = Object.assign(searchQueryModel, newValue);

                this.urlRedirectsMenuViewModel.set("searchQueryModel", newSearchQueryModel);
            },

            _onSimulateFindClick: function () {
                this._onSearchChange({ simulatedOldUrl: this.simulateOldUrlTextBox.get("value")});
            },

            _onSimulateResetClick: function () {
                this.simulateOldUrlTextBox.set("value", "");
                this._onSearchChange({ simulatedOldUrl: ""});
            }
        });
    });