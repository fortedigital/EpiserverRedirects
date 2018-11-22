require([
    "dojo/_base/declare",
    "dojo/text!./ClientResources/UrlRewritePlugin/UrlRedirectsMenu/UrlRedirectsMenu.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/Button",

    "urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuViewModel",
    "urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuGrid",
    "urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuForm",

    "xstyle/css!./ClientResources/UrlRewritePlugin/UrlRedirectsMenu/UrlRedirectsMenu.css"
], function (
    declare,
    template,
    on,

    _WidgetBase,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    Button,

    UrlRedirectsMenuViewModel,
    UrlRedirectsMenuGrid,
    UrlRedirectsMenuForm
) {
        declare("UrlRedirectsMenu", [_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            urlRedirectsMenuViewModel: null,
            selectedModel: null,

            buildRendering: function () {
                this.inherited(arguments);
            },

            postCreate: function () {
                this.urlRedirectsMenuViewModel = new UrlRedirectsMenuViewModel();
                this.urlRedirectsMenuViewModel.getUrlRewrites().then((results) => this.urlRedirectsMenuGrid.setData(results));

                this.urlRedirectsMenuGrid.on('dgrid-select', this._onSelectedItemChange.bind(this));
                this.urlRedirectsMenuForm.onSaveClick = this._onSaveForm.bind(this);

                on(this.addButton, "click", this._onAddNewClick.bind(this));
                on(this.deleteButton, "click", this._onDeleteClick.bind(this));
                this.deleteButton.set('disabled', true);
                this.urlRedirectsMenuForm.set("hidden", true);

                this.urlRedirectsMenuViewModel.watch("mode", (name, oldValue, value) => {
                    this.urlRedirectsMenuForm.set("hidden", !value);
                });
            },

            _onAddNewClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "add");
                this.urlRedirectsMenuForm.updateView({}, this.urlRedirectsMenuViewModel.get("mode"));
                this.urlRedirectsMenuGrid.clearSelection();
                this.deleteButton.set('disabled', true);
            },

            _onDeleteClick: function () {
                this.urlRedirectsMenuViewModel.set("mode", "");
                this.urlRedirectsMenuViewModel.deleteUrlRewrite(this.selectedModel.id);
            },

            _onSelectedItemChange: function (event) {
                this.urlRedirectsMenuViewModel.set("mode", "edit");
                this.selectedModel = event.rows[0].data;
                this.deleteButton.set('disabled', !this.selectedModel);

                this.urlRedirectsMenuForm.updateView(this.selectedModel, this.urlRedirectsMenuViewModel.get("mode"));
            },

            _onSaveForm: function (model) {
                var mode = this.urlRedirectsMenuViewModel.get("mode");

                if (mode === "edit") {
                    this.urlRedirectsMenuViewModel.updateUrlRewrite(model);
                } else if (mode === "add") {
                    this.urlRedirectsMenuViewModel.addUrlRewrite(model);
                }

                this.urlRedirectsMenuViewModel.set("mode", "");
            }
        });
    });