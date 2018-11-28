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
                this.urlRedirectsMenuGrid.init(this.urlRedirectsMenuViewModel.store);
                this.urlRedirectsMenuGrid.on('dgrid-select', this._onSelectedItemChange.bind(this));

                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                this.urlRedirectsMenuGrid.setQuery(searchQueryModel);

                this.urlRedirectsMenuGrid.oldUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ oldUrlSearch: newValue });
                this.urlRedirectsMenuGrid.newUrlSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ newUrlSearch: newValue });
                on(this.urlRedirectsMenuGrid.typeSearch, "change", (newValue) => this._onSearchChange({ typeSearch: newValue }));
                this.urlRedirectsMenuGrid.contentIdSearch.onSearchBoxChange = (newValue) => this._onSearchChange({ contentIdSearch: newValue });

                this._updateGrid();

                this.urlRedirectsMenuForm.onSaveClick = this._onSaveForm.bind(this);

                on(this.addButton, "click", this._onAddNewClick.bind(this));
                on(this.editButton, "click", this._onEditClick.bind(this));
                on(this.deleteButton, "click", this._onDeleteClick.bind(this));
                this.deleteButton.set('disabled', true);
                this.editButton.set('disabled', true);

                this.urlRedirectsMenuViewModel.watch("mode", (name, oldValue, value) => {
                    !value ? this.urlRedirectsMenuFormDialog.hide() : this.urlRedirectsMenuFormDialog.show();
                });

                this.urlRedirectsMenuViewModel.watch("searchQueryModel", (name, oldValue, value) => {
                    this.urlRedirectsMenuGrid.setQuery(value);
                });
            },

            _updateGrid: function () {
                this.urlRedirectsMenuGrid.refresh();
            },

            _onAddNewClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "add");
                this.urlRedirectsMenuFormDialog.set("title", this.urlRedirectsMenuViewModel.get("dialogTitle"));
                this.urlRedirectsMenuFormDialog.show();
                this.urlRedirectsMenuForm.updateView({}, this.urlRedirectsMenuViewModel.get("mode"));
                this.urlRedirectsMenuGrid.clearSelection();
                this.deleteButton.set('disabled', true);
            },

            _onDeleteClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "");
                this.urlRedirectsMenuViewModel.deleteUrlRewrite(this.selectedModel.id).then(() => this._updateGrid());
            },

            _onEditClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "edit");
                this.urlRedirectsMenuFormDialog.show();
                this.urlRedirectsMenuFormDialog.set("title", this.urlRedirectsMenuViewModel.get("dialogTitle"));
                this.urlRedirectsMenuForm.updateView(this.selectedModel, this.urlRedirectsMenuViewModel.get("mode"));
            },

            _onSelectedItemChange: function (event) {
                this.selectedModel = event.rows[0].data;
                this.deleteButton.set('disabled', !this.selectedModel);
                this.editButton.set('disabled', !this.selectedModel);
            },

            _onSaveForm: function (model) {
                var mode = this.urlRedirectsMenuViewModel.get("mode");

                if (mode === "edit") {
                    this.urlRedirectsMenuViewModel.updateUrlRewrite(model).then(response => this._handleResponse(response));
                } else if (mode === "add") {
                    this.urlRedirectsMenuViewModel.addUrlRewrite(model).then(response => this._handleResponse(response));
                }
            },

            _handleResponse: function (reponse) {
                if (reponse) {
                    this._updateGrid();
                    this.urlRedirectsMenuViewModel.set("mode", "");
                } else {
                    this.urlRedirectsMenuForm.showDuplicateMessage();
                }
            },

            _onSearchChange: function (newValue) {
                var searchQueryModel = this.urlRedirectsMenuViewModel.get("searchQueryModel");
                var newSearchQueryModel = Object.assign(searchQueryModel, newValue);

                this.urlRedirectsMenuViewModel.set("searchQueryModel", newSearchQueryModel);
            }
        });
    });