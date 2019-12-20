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
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        public class RoutesResponse
        {
            public string Routes { get; set; } = "";
            public string Login { get; set; } = "";
        }

        public const string RoutesName = "Api_GetRoutes";
        [HttpGet(Name = RoutesName)]
        public RoutesResponse GetRoutes()
            => new RoutesResponse
            {
                Routes = Url.AbsoluteRouteUrl(RoutesName),
                Login = Url.AbsoluteRouteUrl(UsersController.LoginName)
            };
    }
}
