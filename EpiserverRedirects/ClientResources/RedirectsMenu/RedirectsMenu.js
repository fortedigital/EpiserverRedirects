define("redirectsMenu/RedirectsMenu", [
    "dojo/_base/declare",
    "dojo/text!./RedirectsMenu.html",
    "dojo/on",
    "dojo/request/xhr",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/Button",
    "dojox/form/Uploader",
    "dijit/Dialog",
    "dijit/form/TextBox",

    "redirectsMenu/RedirectsMenuViewModel",
    "redirects/Moment",
    "redirectsMenu-grid/RedirectsMenuGrid",
    "redirectsMenu-form/RedirectsMenuEditSingleForm",
    "redirectsMenu-multipleForm/RedirectsMenuEditMultipleForm",

    "xstyle/css!./RedirectsMenu.css",
], function (
    declare,
    template,
    on,
    xhr,

    _WidgetBase,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    Button,
    Uploader,
    Dialog,
    TextBox,

    RedirectsMenuViewModel,
    moment,
    RedirectsMenuGrid,
    RedirectsMenuEditSingleForm,
    RedirectsMenuEditMultipleForm
) {
        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            redirectsMenuViewModel: null,
            selectedModels: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            postCreate: function () {
                this.redirectsMenuViewModel = new RedirectsMenuViewModel();
                this.selectedModels = [];

                this._initializeGrid();
                this._initializeForm();

                on(this.addButton, "click", this._onAddNewClick.bind(this));
                on(this.editButton, "click", this._onEditClick.bind(this));
                on(this.deleteButton, "click", this._onDeleteClick.bind(this));
                on(this.refreshButton, "click", this._refreshView.bind(this));
                on(this.clearAllButton, "click", this._onClearAllClick.bind(this));
                /*on(this.simulateFindButton, "click", this._onSimulateFindClick.bind(this));
                on(this.simulateResetButton, "click", this._onSimulateResetClick.bind(this));*/
                on(this.uploadFormSubmit, "click", this._onImportSubmit.bind(this));
                on(this.fileUploader, "change", this._onUploaderChange.bind(this));

                this.deleteButton.set('disabled', true);
                this.editButton.set('disabled', true);

                this.redirectsMenuViewModel.watch("mode", (name, oldValue, value) => {
                    if (value && value === "edit" || value === "add"){
                        this.redirectsMenuFormDialog.show();
                        this.redirectsMenuFormDialog.set("title", this.redirectsMenuViewModel.get("dialogTitle"));
                    } else {
                        this.redirectsMenuFormDialog.hide()
                    }
                    
                    if (value && value === "edit-multiple"){
                        this.redirectsMenuMultipleFormDialog.show()
                        this.redirectsMenuMultipleFormDialog.set("title", this.redirectsMenuViewModel.get("dialogTitle"));
                    } else {
                        this.redirectsMenuMultipleFormDialog.hide() 
                    }
                });

                this.redirectsMenuViewModel.watch("searchQueryModel", (name, oldValue, value) => {
                    this.redirectsMenuGrid.setQuery(value);
                });
            },

            _initializeGrid: function () {
                this.redirectsMenuGrid.init(this.redirectsMenuViewModel.store, this.redirectsMenuViewModel.hostStore, this.redirectsMenuViewModel.contentProvidersStore);
                this.redirectsMenuGrid.on('dgrid-select', this._onSelectedItemChange.bind(this));
                this.redirectsMenuGrid.on('dgrid-deselect', this._onDeselectedItemChange.bind(this));
                this.redirectsMenuGrid.on('.dgrid-content .dgrid-row:dblclick', this._onEditClick.bind(this));

                var searchQueryModel = this.redirectsMenuViewModel.get("searchQueryModel");
                this.redirectsMenuGrid.setQuery(searchQueryModel);

                this.redirectsMenuGrid.oldPattern.onSearchBoxChange = (newValue) => this._onSearchChange({ oldPattern: newValue });
                this.redirectsMenuGrid.newPattern.onSearchBoxChange = (newValue) => this._onSearchChange({ newPattern: newValue });
                this.redirectsMenuGrid.contentId.onSearchBoxChange = (newValue) => this._onSearchChange({ contentId: newValue });
                on(this.redirectsMenuGrid.contentProviders, "change", (newValue) => this._onSearchChange({ contentProviderId: newValue }));
                on(this.redirectsMenuGrid.redirectRuleType, "change", (newValue) => this._onSearchChange({ redirectRuleType: newValue }));
                on(this.redirectsMenuGrid.redirectType, "change", (newValue) => this._onSearchChange({ redirectType: newValue }));
                this.redirectsMenuGrid.priority.onSearchBoxChange = (newValue) => this._onSearchChange({ priority: newValue });
                on(this.redirectsMenuGrid.redirectOrigin, "change", (newValue) => this._onSearchChange({ redirectOrigin: newValue }));
                on(this.redirectsMenuGrid.isActive, "change", (newValue) => this._onSearchChange({ isActive: newValue }));
                on(this.redirectsMenuGrid.createdOnFrom, "change", (newValue) => this._onSearchChange({ createdOnFrom: this._parseToUtcDateTime(newValue) }));
                on(this.redirectsMenuGrid.createdOnTo, "change", (newValue) => this._onSearchChange({ createdOnTo: this._parseToUtcDateTime(newValue) }));
                this.redirectsMenuGrid.createdBy.onSearchBoxChange = (newValue) => this._onSearchChange({ createdBy: newValue });
                this.redirectsMenuGrid.notes.onSearchBoxChange = (newValue) => this._onSearchChange({ notes: newValue });
                on(this.redirectsMenuGrid.hostName, "change", (newValue) => this._onSearchChange({ hostName: newValue }));
            },

            _parseToUtcDateTime: function (localDate) {
                return moment(localDate).utc().format();
            },

            _initializeForm: function () {
                this.redirectsMenuEditSingleForm.onSaveClick = this._onSaveForm.bind(this);
                this.redirectsMenuEditSingleForm.onDeleteClick = this._onDeleteClick.bind(this);
                this.redirectsMenuEditSingleForm.onCancelClick = this._onCancelFormClick.bind(this);

                this.redirectsMenuEditMultipleForm.onSaveClick = this._onSaveForm.bind(this);
                this.redirectsMenuEditMultipleForm.onDeleteClick = this._onDeleteClick.bind(this);
                this.redirectsMenuEditMultipleForm.onCancelClick = this._onCancelFormClick.bind(this);
            },

            _updateGrid: function () {
                this.redirectsMenuGrid.refresh();
            },

            _onAddNewClick: function () {
                this.redirectsMenuViewModel.set("mode", "add");
                this.redirectsMenuEditSingleForm.updateView({}, this.redirectsMenuViewModel.get("mode"));
                this.redirectsMenuGrid.clearSelection();
            },

            _onDeleteClick: function () {
                this.redirectsMenuViewModel.set("mode", "");
                this.redirectsMenuViewModel.deleteRedirectRule(this.selectedModels.map(x => x.id)).then((response) => this._refreshView());
            },

            _onClearAllClick: function() {
                if (window.confirm("Do you really want to clear all redirect rules?")) {
                    this.redirectsMenuViewModel.clearRedirectRules().then((r) => this._refreshView());
                }
            },

            _onEditClick: function () {
                if(!this._isEditable(this.selectedModels)) return;
                
                if(this.selectedModels.length === 1){
                    this.redirectsMenuViewModel.set("mode", "edit");
                    this.redirectsMenuEditSingleForm.updateView(this.selectedModels[0], this.redirectsMenuViewModel.get("mode"));
                } else {
                    this.redirectsMenuViewModel.set("mode", "edit-multiple");
                    this.redirectsMenuEditMultipleForm.updateView(this.selectedModels, this.redirectsMenuViewModel.get("mode"));
                }
            },

            _onSelectedItemChange: function (event) {
                this.selectedModels.push(event.rows.map(x => x.data));
                this.selectedModels = this.selectedModels.flat()

                this.deleteButton.set('disabled', this.selectedModels.length === 0);
                this.editButton.set('disabled', !this._isEditable(this.selectedModels));
            },

            _onDeselectedItemChange: function (event) {
                this.selectedModels = this.selectedModels.filter(x => event.rows.find(r => r.id !== x.id));

                this.deleteButton.set('disabled', this.selectedModels.length === 0);
                this.editButton.set('disabled', !this._isEditable(this.selectedModels));
            },
            
            _isEditable: function(selectedModels){
                return selectedModels.length > 0;
            },

            _onSaveForm: function (model) {
                var mode = this.redirectsMenuViewModel.get("mode");
                if (mode === "edit" || mode === "edit-multiple") {
                    this.redirectsMenuViewModel.updateRedirectRule(model)
                        .then(response => this._refreshView(), error => this._handleError(error));
                } else if (mode === "add") {
                    this.redirectsMenuViewModel.addRedirectRule(model)
                        .then(response => this._refreshView(), error => this._handleError(error));
                }
            },

            _refreshView: function () {
                this._updateGrid();
                this.redirectsMenuViewModel.set("mode", "");
                this.redirectsMenuGrid.clearSelection();
                this.selectedModels = [];
                this.deleteButton.set("disabled", true);
                this.editButton.set("disabled", true);
            },

            _handleError: function (error) {
                this.redirectsMenuEditSingleForm.showDuplicateMessage();
            },

            _onCancelFormClick: function () {
                this.redirectsMenuViewModel.set("mode", "");
            },

            _onSearchChange: function (newValue) {
                var searchQueryModel = this.redirectsMenuViewModel.get("searchQueryModel");
                var newSearchQueryModel = Object.assign(searchQueryModel, newValue);

                this.redirectsMenuViewModel.set("searchQueryModel", newSearchQueryModel);
            },

            /*
             _onSimulateFindClick: function () {
                this._onSearchChange({ simulatedOldPattern: this.simulateOldPatternTextBox.get("value")});
             },
            
             _onSimulateResetClick: function () {
                this.simulateOldPatternTextBox.set("value", "");
                this._onSearchChange({ simulatedOldPattern: ""});
             },
             */
            
            _onUploaderChange: function (fileArray) {
                this.importStatus.innerText = fileArray && fileArray.length
                    ? fileArray[0].name
                    : "Select a file";
            },

            _onImportSubmit: function (event) {
                var statusLabel = this.importStatus;
                if (!this.fileUploader._files || !this.fileUploader._files.length) {
                    statusLabel.innerText = "Select a file";
                    return;
                }

                var formData = new FormData();
                formData.append('uploadedFile', this.fileUploader._files[0]);

                var xhrArgs = {
                    data: formData,
                    headers: {
                        'Content-Type': false
                    },
                    handleAs: "json"
                };
                statusLabel.innerText = "Uploading file...";
                var xhrRequest = xhr.post("/Forte.EpiserverRedirects/Import", xhrArgs)
                    .then(function (data) {
                        statusLabel.innerText = data.errorMessage || _getLocalDateTime(data.timeStamp) + " - Imported redirects: " + data.importedCount;

                        function _getLocalDateTime(utcDateTime) {
                            var localDateTime = moment(utcDateTime).local().format('DD-MM-YYYY HH:mm:ss');
                            return localDateTime.toString();
                        }
                    })
                    .otherwise(function () {
                        statusLabel.innerText = "Temporary server error or file is in invalid format";
                    })
            },
        });
    });
