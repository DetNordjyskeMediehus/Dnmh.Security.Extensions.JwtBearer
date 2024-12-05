using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public class DefaultJwtBearerChallengeContext : JwtBearerChallengeContext
    {
        public DefaultJwtBearerChallengeContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions(),
                 new AuthenticationProperties())
        { }
    }
}
