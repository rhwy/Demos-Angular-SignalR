using Nancy;

namespace Services
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get[@"/(.*)"] = p => View["index"];
            Get[@""] = p => View["index"];

            Get[@"/admin"] = p => View["admin"];
        }
    }
}