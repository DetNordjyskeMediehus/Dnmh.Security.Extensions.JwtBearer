using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dnmh.Security.Extensions.JwtBearer.Tests
{
    public class DefaultJwtBearerAuthenticationScheme : AuthenticationScheme
    {
        public DefaultJwtBearerAuthenticationScheme() :
            base(JwtBearerDefaults.AuthenticationScheme,
                 null,
                 typeof(IAuthenticationHandler))
        { }
    }
}
