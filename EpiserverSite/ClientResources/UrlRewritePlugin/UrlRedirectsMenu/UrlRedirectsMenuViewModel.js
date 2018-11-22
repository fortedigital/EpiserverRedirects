define("urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuViewModel", [
    // dojo
    "dojo/_base/declare",
    "dojo/Stateful",
    "dojo/request"
], function (declare, Stateful, request) {

    return declare([Stateful], {
        storeUrl: "modules/app/Stores/UrlRedirectsStore",
        mode: "",

        _modeGetter: function () {
            return this.mode;
        },

        _modeSetter: function (value) {
            this.mode = value;
        },

        getUrlRewrites: function () {
            return request(`${this.storeUrl}`, {
                handleAs: "text",
                method: "GET"
            }).then(this._toJson);
        },

        addUrlRewrite: function (newModel) {
            return request(`${this.storeUrl}`, {
                handleAs: "text",
                method: "POST",
                data: newModel
            }).then(this._toJson);
        },

        updateUrlRewrite: function (model) {
            return request(`${this.storeUrl}`, {
                handleAs: "text",
                method: "PUT",
                data: model
            }).then(this._toJson);
        },

        _toJson: function (text, a) {
            console.log(text);
            text = text.replace("{}&&", "");
            return new Promise((resolve) => resolve(JSON.parse(text)));
        }
    });
});
