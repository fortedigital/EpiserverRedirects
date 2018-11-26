define("urlRedirectsMenu/UrlRedirectsMenuForm", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenuForm.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/TextBox",
    "dijit/form/Button",
],

    function (
        declare,
        template,
        on,

        _WidgetBase,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        TextBox,
        Button
    ) {

        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            id: "",

            postCreate: function () {
                this.contentIdTextBox.set("disabled", true);
                this.typeTextBox.set("disabled", true);
                on(this.saveButton, "click", () => this.onSaveClick(this._getModel()));
            },

            updateView: function (model, mode) {
                this.id = model.id;
                if (mode === "edit") {
                    this._updateEditMode(model);
                }
                else if (mode === "add") {
                    this._updateAddMode();
                }
            },

            _updateEditMode: function (model) {
                var isCustomType = model.type === "custom";
                this.titleForm.innerText = "Edit Url Redirect";

                this.oldUrlTextBox.set("disabled", !isCustomType);
                this.newUrlTextBox.set("disabled", !isCustomType);
                this.saveButton.set("disabled", !isCustomType);

                this.oldUrlTextBox.set("value", model.oldUrl);
                this.newUrlTextBox.set("value", model.newUrl);
                this.contentIdTextBox.set("value", model.contentId);
                this.typeTextBox.set("value", model.type);

            },

            _updateAddMode: function () {
                this.titleForm.innerText = "Add new Url Redirect";
                this.typeTextBox.set("value", "custom");

                this.oldUrlTextBox.set("value", "");
                this.newUrlTextBox.set("value", "");
                this.contentIdTextBox.set("value", "");

                this.oldUrlTextBox.set("disabled", false);
                this.newUrlTextBox.set("disabled", false);
                this.saveButton.set("disabled", false);
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

            }
        });
    });
