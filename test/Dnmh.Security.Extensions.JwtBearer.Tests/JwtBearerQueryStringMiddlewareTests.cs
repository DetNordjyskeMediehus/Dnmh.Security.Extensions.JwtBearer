using Dnmh.Security.Extensions.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public sealed class JwtBearerQueryStringMiddlewareTests
    {
        [Fact]
        public void Constructor_NullRequestDelegate()
        {

            // Arrange

            var options = Options.Create(new JwtBearerQueryStringOptions());

            // Act

            var exception = Record.Exception(
                () => new JwtBearerQueryStringMiddleware(null, options)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullOptions()
        {

            // Arrange

            RequestDelegate next = (context) => Task.CompletedTask;

            // Act

            var exception = Record.Exception(
                () => new JwtBearerQueryStringMiddleware(next, null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task Invoke_NullHttpContext()
        {

            // Arrange

            var middleware = CreateMiddleware();

            // Act

            var exception = await Record.ExceptionAsync(
                () => middleware.Invoke(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task Invoke_NullQueryStringBehavior()
        {

            // Arrange

            var middleware = CreateMiddleware(
                options: new JwtBearerQueryStringOptions
                {
                    QueryStringBehavior = null
                }
            );

            var context = new DefaultHttpContext();
            var original = context.Request.QueryString.ToString();

            // Act

            await middleware.Invoke(context);

            // Assert

            Assert.Equal(original, context.Request.QueryString.ToString());
        }

        [Fact]
        public async Task Invoke_NonNullQueryStringBehavior()
        {

            // Arrange

            var behavior = new Mock<IJwtBearerQueryStringBehavior>();

            var options = new JwtBearerQueryStringOptions
            {
                QueryStringBehavior = behavior.Object
            };

            var middleware = CreateMiddleware(options: options);
            var context = new DefaultHttpContext();

            // Act

            await middleware.Invoke(context);

            // Assert

            behavior.Verify(
                mock => mock.Apply(context, options),
                Times.Once()
            );
        }

        private JwtBearerQueryStringMiddleware CreateMiddleware(
            RequestDelegate next = null,
            JwtBearerQueryStringOptions options = null)
        {

            return new JwtBearerQueryStringMiddleware(
                next ?? (context => Task.CompletedTask),
                Options.Create(options ?? new JwtBearerQueryStringOptions())
            );
        }
    }
}