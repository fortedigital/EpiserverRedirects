define("redirectsMenu-multipleForm/RedirectsMenuEditMultipleForm", [
    "dojo/_base/declare",
    "dojo/text!./RedirectsMenuEditMultipleForm.html",
    "dojo/on",
    "dojo",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/TextBox",
    "dijit/form/Button",
    "dijit/form/ValidationTextBox",
    "dijit/form/NumberTextBox",
    "dijit/form/Select",
    "dijit/form/Textarea",
    "redirects/Moment",
    "epi/dependency",
        "dojo/data/ObjectStore",
        "dojo/store/Memory",
],

    function (
        declare,
        template,
        on,
        dojo,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        TextBox,
        Button,
        ValidationTextBox,
        NumberTextBox,
        Select,
        TextArea,
        moment,
        dependency,
        ObjectStore,
    ) {

        var allHosts = "7F757A26-1B69-486A-B335-41472ABE724A";
        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            id: "",
            store: null,
            hostSelect: null,
            contentProvidersStore: null,
            contentProvidersSelect: null,
            label: null,
            
            allModels: null,
            
            postCreate: function () {
                var registry = dependency.resolve("epi.storeregistry");
                this.store = this.store || registry.get("redirectsMenu.hostStore");
                var os = new ObjectStore({ objectStore: this.store });
                this.hostSelect = new Select({store: os, labelAttr: "name", class: "form-input"}, "domainInputSelectorMultipleForm");
                this.label = dojo.create("label", {innerHTML:"Host:", style: "float:left"}, "domainLabelSelectorMultipleForm");
                this.contentProvidersStore = this.contentProvidersStore || registry.get("redirectsMenu.contentProviders");
                let contentProvidersOs = new ObjectStore({ objectStore: this.contentProvidersStore });

                this.contentProvidersSelect = new Select({ store: contentProvidersOs, labelAttr: "name", class: "form-input" }, "contentProvidersSelectMultipleForm");

                on(this.saveButton, "click", () => this.onSaveClick(this._getModel()));
                on(this.cancelButton, "click", () => this.onCancelClick());
                on(this.deleteButton, "click", () => this.onDeleteClick());
                on(this.priorityTextBox, "change", () => this._isFormValid());
            },

            _isFormValid: function () {
                this.saveButton.set("disabled", (!this.newPatternTextBox.get("value") && !this.contentIdTextBox.get("value")) || !this.oldPatternTextBox.isValid() || !this.priorityTextBox.isValid());
            },

            updateView: function (model, mode) {
                // TODO Edit mode
                this.allModels = model;
                const firstItem = model[0]; 
                this.id = firstItem.id;
                this.duplicateAlert.hidden = true;

                if (mode === "edit") {
                    this._updateEditMode(firstItem);
                }
                else if (mode === "add") {
                    this._updateAddMode();
                }
            },

            _updateEditMode: function (model) {
                this.contentProvidersSelect.set("value", model.contentProviderId);
                this.redirectRuleTypeSelect.set("value", model.redirectRuleType);
                this.redirectTypeSelect.set("value", model.redirectType);
                this.priorityTextBox.set("value", model.priority);
                this.isActiveSelect.set("value", model.isActive.toString());

                this.deleteButton.set("disabled", false);

                this.hostSelect.set("value", model.hostId === null ? allHosts : model.hostId);
                document.getElementById("createdOnInputDiv").style.display = 'block';
                document.getElementById("createdByInputDiv").style.display = 'block';
            },

            _getLocalDateTime: function (utcDateTime) {
                var localDateTime = moment(utcDateTime).local().format('DD-MM-YYYY HH:mm:ss');
                return localDateTime.toString();
            },

            _updateAddMode: function () {
                this.priorityTextBox.set("value", 100);
                this.deleteButton.set("disabled", true);
                this.saveButton.set("disabled", true);
                this.hostSelect.set("value", allHosts);
                document.getElementById("createdOnInputDiv").style.display = 'none';
                document.getElementById("createdByInputDiv").style.display = 'none';
            },

            _getModel: function () {
                return {
                    redirectRules: this.allModels.map(m => ({
                        ...m,
                        contentProviderKey: this.contentProvidersSelect.get("value"),
                        redirectRuleType: this.redirectRuleTypeSelect.get("value"),
                        redirectType: this.redirectTypeSelect.get("value"),
                        priority: this.priorityTextBox.get("value"),
                        isActive: this.isActiveSelect.get("value"),
                        hostId: this.hostSelect.get("value"),
                    }))
                };
            },
            onSaveClick(model) { },

            onDeleteClick() { },

            onCancelClick() { },

            showDuplicateMessage() {
                this.duplicateAlert.hidden = false;
            }
        });
    });
