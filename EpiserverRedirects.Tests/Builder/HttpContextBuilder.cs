using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder
{
    public class HttpContextBuilder
    {
        private readonly Mock<HttpContextBase> _httpContextBaseMock = new Mock<HttpContextBase>();
        private readonly Lazy<Mock<HttpResponseBase>> _httpResponseMock = new Lazy<Mock<HttpResponseBase>>(() => new Mock<HttpResponseBase>());
        private readonly Lazy<Mock<HttpRequestBase>> _httpRequestMock = new Lazy<Mock<HttpRequestBase>>(() => new Mock<HttpRequestBase>());

        public HttpContextBuilder WithRequest()
        {
            _httpContextBaseMock.Setup(c => c.Request).Returns(_httpRequestMock.Value.Object);
            return this;
        }

        public HttpContextBuilder WithResponse()
        {
            _httpContextBaseMock.Setup(ctx => ctx.Response).Returns(_httpResponseMock.Value.Object);
            return this;
        }

        public HttpContextBuilder WithResponseHeaders()
        {
            _httpResponseMock.Value.SetupGet(res => res.Headers).Returns(new NameValueCollection());
            return this;
        }

        public HttpContextBase Build() => _httpContextBaseMock.Object;
    }
}
