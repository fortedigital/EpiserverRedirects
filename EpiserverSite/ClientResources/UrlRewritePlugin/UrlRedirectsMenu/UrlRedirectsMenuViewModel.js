define("urlRewritePlugin-urlRedirectsMenu/UrlRedirectsMenuViewModel", [
    // dojo
    "dojo/_base/declare",
    "dojo/request/xhr"
], function (declare, xhr) {
    
    return declare([], {
        storeUrl: "modules/app/Stores/urlRewriteStore",

        getAllUrls: function () {
            return xhr(`${this.storeUrl}/getAll`, {
                handleAs: "json",
                method: "GET"
            });
        }
    });
});
