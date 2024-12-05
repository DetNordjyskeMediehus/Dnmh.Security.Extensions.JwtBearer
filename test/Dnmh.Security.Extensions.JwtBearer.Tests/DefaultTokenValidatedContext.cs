using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public class DefaultTokenValidatedContext : TokenValidatedContext
    {
        public DefaultTokenValidatedContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions())
        { }
    }
}
