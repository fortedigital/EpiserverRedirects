require([
    "dojo/_base/declare",
    "dojo/text!./ClientResources/UrlRewritePlugin/UrlRedirectsMenu/UrlRedirectsMenu.html",
    "dojo/parser",
    "dojo/ready",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",

    "urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuViewModel"
], function (
    declare,
    template,
    parser,
    ready,

    _WidgetBase,
    _TemplatedMixin,

    UrlRedirectsMenuViewModel
) {
        declare("UrlRedirectsMenu", [_WidgetBase, _TemplatedMixin], {
            templateString: template,
            urlRedirectsMenuViewModel: null,

            buildRendering: function () {
                this.inherited(arguments);

            },

            postCreate: function () {
                this.urlRedirectsMenuViewModel = new UrlRedirectsMenuViewModel();

                this.urlRedirectsMenuViewModel.getAllUrls().then((s) => console.log(s));
            }
        });

        ready(function () {
            parser.parse();
        });
    });