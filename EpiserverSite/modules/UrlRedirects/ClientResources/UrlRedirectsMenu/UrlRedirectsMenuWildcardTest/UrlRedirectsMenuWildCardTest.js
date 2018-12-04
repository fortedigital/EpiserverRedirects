define("urlRedirectsMenu-wildCardTest/UrlRedirectsMenuWildcardTest", [
    "dojo/_base/declare",
    "dojo/text!./UrlRedirectsMenuWildcardTest.html",
    "dojo/on",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/TextBox",
    "dijit/form/ValidationTextBox",
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
        ValidationTextBox,
        Button,
    ) {

        return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: template,
            urlRedirect: null,

            postCreate: function () {
                on(this.testWildcardButton, "click", () => this._onTestWildcardClick());
            },

            _onTestWildcardClick: function () {
                var testUrlTextBoxValue = this.testUrlTextBox.get("value"),
                    result = testUrlTextBoxValue.replace(new RegExp(this.urlRedirect.oldUrl), this.urlRedirect.newUrl);

                this.resultTextBox.set("value", result);
            },

            setUrlRedirect: function (urlRedirect) {
                this.urlRedirect = urlRedirect;
                this.testUrlTextBox.set("value", urlRedirect.oldUrl);
            }

        });
    });
