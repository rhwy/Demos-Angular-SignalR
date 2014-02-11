using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;


namespace Angulair.Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(ctx =>
            {
                ctx.Response.ContentType = "text/plain";
                return ctx.Response.WriteAsync("hello");
            });
            
        }
    }
}