define([
        "dojo/_base/declare",
        "dijit/_Widget",
        "dijit/_TemplatedMixin",
        "dijit/_WidgetsInTemplateMixin"
    ],
    function (
        declare,

        _Widget,
        _TemplatedMixin,
        _WidgetsInTemplateMixin
    ) {

        return declare([_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], {
            templateString: "<div>Test</div>"
            // templateString: dojo.cache("/urlrewritecomponent")
        });
    });