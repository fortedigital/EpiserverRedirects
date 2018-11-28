define("urlRedirectsMenu/UrlRedirectsMenuForm", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenuForm.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/TextBox",
    "dijit/form/Button",
    "dijit/form/ValidationTextBox"
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
        ValidationTextBox
    ) {

        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            id: "",

            postCreate: function () {
                this.contentIdTextBox.set("disabled", true);
                this.typeTextBox.set("disabled", true);
                on(this.saveButton, "click", () => this.onSaveClick(this._getModel()));
                on(this.oldUrlTextBox, "change", () => this._isFormValid());
                on(this.newUrlTextBox, "change", () => this._isFormValid());
            },

            _isFormValid: function () {
                this.saveButton.set("disabled", !this.newUrlTextBox.isValid() || !this.oldUrlTextBox.isValid());
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
                var isCustomType = model.type === "custom";

                this.oldUrlTextBox.set("disabled", !isCustomType);
                this.newUrlTextBox.set("disabled", !isCustomType);
                this.saveButton.set("disabled", !isCustomType);

                this.oldUrlTextBox.set("value", model.oldUrl);
                this.newUrlTextBox.set("value", model.newUrl);
                this.contentIdTextBox.set("value", model.contentId);
                this.typeTextBox.set("value", model.type);

            },

            _updateAddMode: function () {
                this.typeTextBox.set("value", "custom");

                this.oldUrlTextBox.set("value", "");
                this.newUrlTextBox.set("value", "");
                this.contentIdTextBox.set("value", "");

                this.oldUrlTextBox.set("disabled", false);
                this.newUrlTextBox.set("disabled", false);
                this.saveButton.set("disabled", true);
            },

            _getModel: function () {
                var model = {
                    oldUrl: this.oldUrlTextBox.get("value"),
                    newUrl: this.newUrlTextBox.get("value"),
                    contentId: this.contentIdTextBox.get("value"),
                    type: this.typeTextBox.get("value"),
                    id: this.id
                };

                return model;
            },

            onSaveClick(model) {

            },

            showDuplicateMessage() {
                this.duplicateAlert.hidden = false;
                this.oldUrlTextBox.focus();
                this.oldUrlTextBox.set("state", "Error");
                this.oldUrlTextBox.displayMessage("Invalid value");
            }
        });
    });
