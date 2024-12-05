using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public class DefaultAuthenticationFailedContext : AuthenticationFailedContext
    {
        public DefaultAuthenticationFailedContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions())
        { }
    }
}
