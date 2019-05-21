define("redirectsMenu/RootContainer", [
    "dojo/_base/declare",

    "dijit/_WidgetBase",
    "dijit/_Container",

    "redirectsMenu/RedirectsMenu"

],

    function (declare, _WidgetBase, _Container, RedirectsMenu) {
        return declare([_WidgetBase, _Container], {
            buildRendering: function () {

                this.inherited(arguments);

                var redirectsMenu = new RedirectsMenu(); 

                this.addChild(redirectsMenu);
            }
        });

    });