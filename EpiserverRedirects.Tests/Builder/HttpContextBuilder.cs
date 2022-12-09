using System;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder
{
    public class HttpContextBuilder
    {
        private readonly Mock<HttpContext> _httpContextBaseMock = new Mock<HttpContext>();
        private readonly Lazy<Mock<HttpResponse>> _httpResponseMock = new Lazy<Mock<HttpResponse>>(() => new Mock<HttpResponse>());
        private readonly Lazy<Mock<HttpRequest>> _httpRequestMock = new Lazy<Mock<HttpRequest>>(() => new Mock<HttpRequest>());

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
            _httpResponseMock.Value.SetupGet(res => res.Headers).Returns(new HeaderDictionary());
            return this;
        }

        public HttpContext Build() => _httpContextBaseMock.Object;
    }
}
