define("redirectsMenu/RootContainer", [
    "dojo/_base/declare",

    "dijit/_WidgetBase",
    "dijit/_Container",

    "redirectsMenu/UrlRedirectsMenu"

],

    function (declare, _WidgetBase, _Container, UrlRedirectsMenu) {
        return declare([_WidgetBase, _Container], {
            buildRendering: function () {

                this.inherited(arguments);

                var urlRedirectsMenu = new UrlRedirectsMenu(); 

                this.addChild(urlRedirectsMenu);
            }
        });

    });