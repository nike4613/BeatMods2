using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BeatMods2.Results 
{
    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/418

    public static class TeapotExtension 
    {
        public static TeapotResult ImATeapot(this ControllerBase self)
            => new TeapotResult();
    }
    public class TeapotResult : StatusCodeResult
    {
        public TeapotResult() : base(418) 
        {
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            using var writer = new StreamWriter(context.HttpContext.Response.Body);
            await writer.WriteLineAsync("I can't brew coffee! I'm a teapot!");

            await base.ExecuteResultAsync(context);
        }
    }
}