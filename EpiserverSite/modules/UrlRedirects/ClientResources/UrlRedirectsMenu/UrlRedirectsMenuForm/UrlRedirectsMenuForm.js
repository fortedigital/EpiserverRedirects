define("urlRedirectsMenu-form/UrlRedirectsMenuForm", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenuForm.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/TextBox",
    "dijit/form/Button",
    "dijit/form/ValidationTextBox",
    "dijit/form/NumberTextBox",
    "dijit/form/Select",
],

    function (
        declare,
        template,
        on,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        TextBox,
        Button,
        ValidationTextBox,
        NumberTextBox,
        Select,
    ) {

        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            id: "",

            postCreate: function () {
                on(this.saveButton, "click", () => this.onSaveClick(this._getModel()));
                on(this.cancelButton, "click", () => this.onCancelClick());
                on(this.deleteButton, "click", () => this.onDeleteClick());
                on(this.oldUrlTextBox, "change", () => this._isFormValid());
                on(this.newUrlTextBox, "change", () => this._isFormValid());
                on(this.priorityNumberTextBox, "change", () => this._isFormValid());
            },

            _isFormValid: function () {
                this.saveButton.set("disabled", !this.newUrlTextBox.isValid() || !this.oldUrlTextBox.isValid() || !this.priorityNumberTextBox.isValid());
            },

            updateView: function (model, mode) {
                this.id = model.id;
                this.duplicateAlert.hidden = true;

                if (mode === "edit") {
                    this._updateEditMode(model);
                }
                else if (mode === "add") {
                    this._updateAddMode();
                }
            },

            _updateEditMode: function (model) {
                this.oldUrlTextBox.set("value", model.oldUrl);
                this.newUrlTextBox.set("value", model.newUrl);
                this.priorityNumberTextBox.set("value", model.priority);
                this.typeSelect.set("value", model.type);

                this.deleteButton.set("disabled", false);
            },

            _updateAddMode: function () {
                this.oldUrlTextBox.set("value", "");
                this.newUrlTextBox.set("value", "");
                this.priorityNumberTextBox.set("value", 1);

                this.deleteButton.set("disabled", true);
                this.saveButton.set("disabled", true);
            },

            _getModel: function () {
                var model = {
                    oldUrl: this.oldUrlTextBox.get("value"),
                    newUrl: this.newUrlTextBox.get("value"),
                    priority: this.priorityNumberTextBox.get("value"),
                    type: this.typeSelect.get("value"),
                    id: this.id
                };

                return model;
            },

            onSaveClick(model) { },

            onDeleteClick() { },

            onCancelClick() { },

            showDuplicateMessage() {
                this.duplicateAlert.hidden = false;
                this.oldUrlTextBox.focus();
                this.oldUrlTextBox.set("state", "Error");
                this.oldUrlTextBox.displayMessage("Invalid value");
            }
        });
    });
