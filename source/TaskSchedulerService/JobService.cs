using Hangfire;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskSchedulerService
{
    public class JobService : IJobService
    {
        private readonly AppSettings _appSettings;

        public JobService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void Configure()
        {
            _appSettings.Jobs
                .Where(job => job.Inactive)
                .ToList()
                .ForEach(job => RecurringJob.RemoveIfExists(job.Id));

            _appSettings.Jobs
                .Where(job => job.Active)
                .ToList()
                .ForEach(job => RecurringJob.AddOrUpdate(job.Id, () => ExecuteAsync(job), job.Cron));
        }

        [AutomaticRetry(Attempts = 0)]
        public Task ExecuteAsync(Job job)
        {
            var request = new HttpRequestMessage(new HttpMethod(job.Method), job.Url);

            return new HttpClient().SendAsync(request);
        }
    }
}
