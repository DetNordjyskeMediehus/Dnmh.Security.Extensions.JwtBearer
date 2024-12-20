using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;

namespace Dnmh.Security.Extensions.JwtBearer
{
    /// <summary>
    ///   An implementation of the <see cref="IJwtBearerQueryStringBehavior" /> that
    ///   removes the access token from the request's query string and replaces it
    ///   a string value that can be interpreted as the token being redacted.
    /// </summary>
    public sealed class RedactJwtBearerQueryStringBehavior : JwtBearerQueryStringBehaviorBase
    {
        /// <summary>
        ///   The default value that in place of the access token.
        /// </summary>
        public static string DefaultRedactedValue { get; } = "(REDACTED)";

        /// <summary>
        ///   The value this instance of <see cref="RedactJwtBearerQueryStringBehavior" />
        ///   will plug as the query string value for every instance where the token's query
        ///   string parameter name is found.
        /// </summary>
        public string RedactedValue { get; }

        /// <summary>
        ///   Instantiates a new instance of <see cref="RedactJwtBearerQueryStringBehavior" />
        ///   that will replace all instances of the access token in the query string with
        ///   the word '(REDACTED)' via the <see cref="JwtBearerQueryStringMiddleware" />.
        /// </summary>
        public RedactJwtBearerQueryStringBehavior() : this(DefaultRedactedValue) { }

        /// <summary>
        ///   Instantiates a new instance of <see cref="RedactJwtBearerQueryStringBehavior" />
        ///   that will replace all instances of the access token in the query string with
        ///   the string provided using the <paramref name="redactedValue" /> parameter
        ///   via the <see cref="JwtBearerQueryStringMiddleware" />.
        /// </summary>
        /// <param name="redactedValue">
        ///   The string that will be used in place of the access token via the
        ///   <see cref="JwtBearerQueryStringMiddleware" />.
        /// </param>
        public RedactJwtBearerQueryStringBehavior(string redactedValue)
        {
            RedactedValue = redactedValue;
        }

        /// <summary>
        ///   Identifies and redacts the access token if it was provided via the query string.
        /// </summary>
        protected override void ApplyImpl(
            HttpContext context,
            JwtBearerQueryStringOptions options)
        {

            if (string.IsNullOrWhiteSpace(options.QueryStringParameterName))
            {
                throw new ArgumentException(
                    $"The '{nameof(JwtBearerQueryStringOptions.QueryStringParameterName)}' " +
                    $"property on the '{nameof(options)}' parameter cannot be null or " +
                    $"whitespace.",
                    nameof(options)
                );
            }

            var request = context.Request;

            if (request.QueryString == QueryString.Empty)
            {
                return;
            }

            var queryString = QueryHelpers.ParseQuery(request.QueryString.Value);
            var parameterName = options.QueryStringParameterName;

            StringValues values;

            if (queryString.TryGetValue(parameterName, out values))
            {
                queryString[parameterName] = new StringValues(RedactedValue);

                request.QueryString = QueryString.Create(queryString);
            }
        }
    }
}