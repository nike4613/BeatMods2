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
            public string Login { get; set; } = "";
        }

        [HttpGet]
        public RoutesResponse GetRoutes()
            => new RoutesResponse
            {
                Login = Url.AbsoluteRouteUrl(UsersController.LoginName)
            };
    }
}
