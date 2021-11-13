using System;
using Microsoft.AspNetCore.Builder;

namespace STOCMA.Utils
{
    public static class ODataQueryStringFixerExtensions
    {
        public static IApplicationBuilder UseODataQueryStringFixer(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ODataQueryStringFixer>();
        }
    }
}
