using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;

namespace AirportProxyService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine(
                    AppContext.BaseDirectory, "log-{Date}.txt"))
                .CreateLogger();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            Log.Logger.Information("Application started");
        }


        protected void Application_End()
        {
            Log.Logger.Information("Application ended");
            Log.CloseAndFlush();
        }
        
        protected void Application_Error()
        { 
            var ex = Server.GetLastError();
            Log.Logger.Error(ex, $"Global exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
        }
    }
}
