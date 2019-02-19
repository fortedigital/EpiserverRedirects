using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    public class UrlRedirectsDtoModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var json = GetBody(controllerContext.HttpContext.Request);
            var dto = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);

            try
            {
                return new UrlRedirectsDto(
                    dto["oldUrl"], 
                    dto["newUrl"], 
                    ParseType(dto["type"]),
                    ParsePriority(dto["priority"]), 
                    ParseRedirectStatusCode(dto["redirectStatusCode"]));
            }
            catch
            {
                throw new Exception("Failed to parse json " + json);
            }
        }

        private static string GetBody(HttpRequestBase request)
        {
            var inputStream = request.InputStream;
            inputStream.Position = 0;

            using (var reader = new StreamReader(inputStream))
            {
                var body = reader.ReadToEnd();
                return body;
            }
        }

        private RedirectStatusCode ParseRedirectStatusCode(string val)
        {
            Enum.TryParse<RedirectStatusCode>(val, out var code);
            return code;
        }

        private int ParsePriority(string val)
        {
            return int.Parse(val);
        }

        private UrlRedirectsType ParseType(string val)
        {
            Enum.TryParse<UrlRedirectsType>(val, out var type);
            return type;
        }
    }
}