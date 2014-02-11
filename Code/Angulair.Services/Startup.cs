using System.IO;
using System.Linq;
using System.Web;
using Owin;


namespace Angulair.Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use((ctx, next) =>
            {
                var output = ctx.Get<TextWriter>("host.TraceOutput");
                output.WriteLine(
                    "{0} {1}: {2}",
                    ctx.Request.Scheme,
                    ctx.Request.Method,
                    ctx.Request.Path);
                return next();
            });
            app.UseStaticFiles();
            app.MapSignalR();

            app.Run(ctx =>
            {
                ctx.Response.ContentType = "text/plain";
                return ctx.Response.WriteAsync("hello");
            });

        }
    }
}