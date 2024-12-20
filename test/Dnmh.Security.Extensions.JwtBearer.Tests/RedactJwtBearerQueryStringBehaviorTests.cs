using Microsoft.AspNetCore.Http;
using System;
using Xunit;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public sealed class RedactJwtBearerQueryStringBehaviorTests : JwtBearerQueryStringBehaviorBaseTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Apply_NullQueryStringParameterNameInOptions(string name)
        {

            // Arrange

            var behavior = CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions { QueryStringParameterName = name };
            context.Request.QueryString = new QueryString("?access_token=token");

            // Act

            var exception = Record.Exception(
                () => behavior.Apply(context, options)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

            Assert.Equal(
                "The 'QueryStringParameterName' property on the " +
                "'options' parameter cannot be null or whitespace. " +
                "(Parameter 'options')",
                exception.Message
            );
        }

        [Fact]
        public void Apply_QueryStringNotPresent()
        {

            // Arrange

            var behavior = CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            var queryString = new QueryString("?ParameterName=Value1&ParameterName=Value2");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Equal(queryString, context.Request.QueryString);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("access_token")]
        [InlineData("custom-parameter")]
        public void Apply_QueryStringAppliedOnce(string parameterName)
        {

            // Arrange

            var behavior = CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions
            {
                QueryStringParameterName =
                    parameterName ??
                    JwtBearerQueryStringOptions.DefaultQueryStringParameterName
            };

            const string token = "MyToken";
            var queryString = new QueryString($"?{options.QueryStringParameterName}={token}");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains(
                $"{options.QueryStringParameterName}={RedactJwtBearerQueryStringBehavior.DefaultRedactedValue}",
                context.Request.QueryString.Value
            );

            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        [Fact]
        public void Apply_QueryStringAppliedMultipleTimes()
        {

            // Arrange

            var behavior = CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            const string token = "MyToken";
            var queryString = new QueryString($"?access_token={token}1&access_token={token}2");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains(
                "access_token=" + RedactJwtBearerQueryStringBehavior.DefaultRedactedValue,
                context.Request.QueryString.Value
            );

            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Apply_NullRedactionValuesSupported(string redactedValue)
        {

            // Arrange

            var behavior = CreateBehavior(redactedValue);

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            const string token = "MyOtherToken";
            var queryString = new QueryString($"?access_token={token}");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains("access_token=" + redactedValue, context.Request.QueryString.Value);
            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        protected override IJwtBearerQueryStringBehavior CreateBehavior()
        {
            return CreateBehavior(RedactJwtBearerQueryStringBehavior.DefaultRedactedValue);
        }

        private IJwtBearerQueryStringBehavior CreateBehavior(string redactedValue)
        {
            return new RedactJwtBearerQueryStringBehavior(redactedValue);
        }
    }
}