using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace SBA.AzBar.Hangfire
{
    public class ScheduleHangfireJobs
    {
        public void ScheduleStaleContent()
        {
            RecurringJob.AddOrUpdate(() => StaleContent(null), "0 0 * * *");
        }

        public void StaleContent(PerformContext context)
        {
            context.SetTextColor(ConsoleTextColor.DarkMagenta);
            context.WriteLine("Checking for stale content!");
        }
    }
}