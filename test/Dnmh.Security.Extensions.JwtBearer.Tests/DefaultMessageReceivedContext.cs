using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public class DefaultMessageReceivedContext : MessageReceivedContext
    {
        public DefaultMessageReceivedContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions())
        { }
    }
}
