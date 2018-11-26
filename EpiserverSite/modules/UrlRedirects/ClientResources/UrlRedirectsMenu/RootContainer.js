define([
    "dojo/_base/declare",

    "dijit/_WidgetBase",
    "dijit/_Container",

    "urlRedirectsMenu/UrlRedirectsMenu"

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