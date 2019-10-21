using SBA.AzBar.Hangfire;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using Umbraco.Web;

[assembly: OwinStartup("UmbracoStandardOwinStartup", typeof(UmbracoStandardOwinStartup))]
namespace SBA.AzBar.Hangfire
{
    public class UmbracoStandardOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            //ensure the default options are configured
            base.Configuration(app);

            // Configure hangfire
            var options = new SqlServerStorageOptions { PrepareSchemaIfNecessary = true };
            const string umbracoConnectionName = Umbraco.Core.Constants.System.UmbracoConnectionName;
            var connectionString = System.Configuration
                .ConfigurationManager
                .ConnectionStrings[umbracoConnectionName]
                .ConnectionString;

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(connectionString, options)
                .UseConsole();

            // Give hangfire a URL and start the server           
            var dashboardOptions = new DashboardOptions { Authorization = new[] { new UmbracoAuthorizationFilter() } };
            app.UseHangfireDashboard("/hangfire", dashboardOptions);
            app.UseHangfireServer();

            // Schedule jobs
            var scheduler = new ScheduleHangfireJobs();
            scheduler.ScheduleStaleContent();
        }
    }
}