using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Utilities
{
    public static class Extensions
    {
        public static string AbsoluteRouteUrl(
            this IUrlHelper urlHelper,
            string routeName,
            object? routeValues = null) =>
                urlHelper.RouteUrl(routeName, routeValues, urlHelper.ActionContext.HttpContext.Request.Scheme);
    }
}
