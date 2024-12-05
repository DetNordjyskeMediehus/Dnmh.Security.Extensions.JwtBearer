using Dnmh.Security.Extensions.JwtBearer;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public sealed class NullJwtBearerQueryStringBehaviorTests :
        JwtBearerQueryStringBehaviorBaseTests
    {
        [Fact]
        public void Apply_QueryStringUnaffected()
        {

            // Arrange

            var behavior = CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            context.Request.QueryString.Add(options.QueryStringParameterName, "Token");
            var queryString = context.Request.QueryString.ToString();

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Equal(queryString, context.Request.QueryString.ToString());
        }

        protected override IJwtBearerQueryStringBehavior CreateBehavior()
        {
            return new NullJwtBearerQueryStringBehavior();
        }
    }
}