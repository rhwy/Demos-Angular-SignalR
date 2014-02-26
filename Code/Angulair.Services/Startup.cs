using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Nancy;
using Owin;

namespace Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use((ctx, next) =>
            {
                var output = ctx.Get<TextWriter>("host.TraceOutput");
                output.WriteLine("{0} {1}: {2}", ctx.Request.Scheme, ctx.Request.Method, ctx.Request.Path);
                return next();
            });
            GlobalHost.DependencyResolver.Register(typeof(VoteHub),
                () => new VoteHub(new QuestionRepository()));

            app.UseStaticFiles();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.MapSignalR();
            app.Map("/signalr", map => map.UseCors(CorsOptions.Value)
                 .RunSignalR(new HubConfiguration
                 {
                     EnableJSONP = true
                 }));

            app.UseNancy(options =>
                    options.PerformPassThrough = context =>
                        context.Response.StatusCode == HttpStatusCode.NotFound);
            
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello World!");
            });
        }

        private static readonly Lazy<CorsOptions> CorsOptions = new Lazy<CorsOptions>(() => new CorsOptions
        {
            PolicyProvider = new CorsPolicyProvider
            {
                PolicyResolver = context =>
                {
                    var policy = new CorsPolicy();
                    // Only allow CORS for these sites:
                    policy.Origins.Add("http://localhost:5000");
                    policy.Origins.Add("http://run.jsbin.com");
                    policy.AllowAnyMethod = true;
                    policy.AllowAnyHeader = true;
                    policy.SupportsCredentials = true;
                    return Task.FromResult(policy);
                }
            }
        });
    }

    [HubName("votes")]
    public class VoteHub : Hub
    {
        private IQuestionRepository repo;
        public VoteHub(IQuestionRepository repo)
        {
            this.repo = repo;
        }

        public void VoteUp(string id)
        {
            int current = repo.VoteUp(id);
            Clients.All.updateQuestionVoteUp(id, current);
        }

        public void VoteDown(string id)
        {
            int current = repo.VoteDown(id);
            Clients.All.updateQuestionVoteDown(id, current);
        }

        
    }


    public class ChatHub : Hub
    {

        public void Send(string name, string message)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new HubException("sender name can't not be empty");
            }
            Clients.Others.broadcastMessage(name, message);
            Clients.Caller.sent();
        }
    }

    
}