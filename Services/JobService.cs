using System.Diagnostics;
using Hangfire;

namespace webApi.Services;

public class JobService:IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
       
        RecurringJob.AddOrUpdate(() => Debug.WriteLine("Minutely Job"), Cron.Minutely);
       return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}