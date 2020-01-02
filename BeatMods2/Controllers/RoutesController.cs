using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BeatMods2.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using BeatMods2.Utilities;

namespace BeatMods2.Controllers
{
    [Route("api/routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        public const string RoutesName = "Api_GetRoutes";
        [HttpGet(Name = RoutesName)]
        public object GetRoutes()
            => new {
                Routes = Url.AbsoluteRouteUrl(RoutesName),
                Users = UsersController.PublicRoutes(Url)
            };
    }
}
