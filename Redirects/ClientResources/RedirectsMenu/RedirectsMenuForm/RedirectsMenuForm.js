define("redirectsMenu-form/RedirectsMenuForm", [
    "dojo/_base/declare",
    "dojo/text!./RedirectsMenuForm.html",
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
                on(this.oldPatternTextBox, "change", () => this._isFormValid());
                on(this.newPatternTextBox, "change", () => this._isFormValid());
            },

            _isFormValid: function () {
                this.saveButton.set("disabled", !this.newPatternTextBox.isValid() || !this.oldPatternTextBox.isValid());
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
                this.oldPatternTextBox.set("value", model.oldPattern);
                this.newPatternTextBox.set("value", model.newPattern);
                this.redirectRuleTypeSelect.set("value", model.redirectRuleType);
                this.redirectTypeSelect.set("value", model.redirectType);

                this.deleteButton.set("disabled", false);
            },

            _updateAddMode: function () {
                this.oldPatternTextBox.set("value", "");
                this.newPatternTextBox.set("value", "");

                this.deleteButton.set("disabled", true);
                this.saveButton.set("disabled", true);
            },

            _getModel: function () {
                var model = {
                    oldPattern: this.oldPatternTextBox.get("value"),
                    newPattern: this.newPatternTextBox.get("value"),
                    redirectRuleType: this.redirectRuleTypeSelect.get("value"),
                    redirectType: this.redirectTypeSelect.get("value"),
                    id: this.id
                };

                return model;
            },

            onSaveClick(model) { },

            onDeleteClick() { },

            onCancelClick() { },

            showDuplicateMessage() {
                this.duplicateAlert.hidden = false;
                this.oldPatternTextBox.focus();
                this.oldPatternTextBox.set("state", "Error");
                this.oldPatternTextBox.displayMessage("Invalid value");
            }
        });
    });
