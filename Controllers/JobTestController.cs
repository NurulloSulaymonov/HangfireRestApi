using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Mvc;
using webApi.Services;

namespace webApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        private readonly JobStorage _storage;
        //  private readonly IMonitoringApi _storage;

        public JobTestController(IJobTestService jobTestService,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager, JobStorage storage
            /*IMonitoringApi storage*/)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _storage = storage;
        }


        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));
            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());

            return Ok();
        }

        [HttpGet("/queues")]
        public List<QueueWithTopEnqueuedJobsDto> Queues()
        {
            var test  =   _storage.GetMonitoringApi().Queues().ToList();
            return test;
        }

        [HttpGet("/servers")]
        public List<QueueWithTopEnqueuedJobsDto> Servers()=> _storage.GetMonitoringApi().Queues().ToList();

        [HttpGet("/JobDetail{id}")]
        public JobDetailsDto Queues(string id) => _storage.GetMonitoringApi().JobDetails(id);

        [HttpGet("/GetStatistic")]
        public StatisticsDto GetJobs() => _storage.GetMonitoringApi().GetStatistics();
        
        [HttpGet("/EnqueuedJobs")]
        public JobList<EnqueuedJobDto> Queues(string queue,int from, int perPage) => _storage.GetMonitoringApi().EnqueuedJobs(queue,from, perPage);

        [HttpGet("/FetchedJobs")]
        public JobList<FetchedJobDto> FetchedJobs(string queue,int from, int perPage) => _storage.GetMonitoringApi().FetchedJobs(queue,from, perPage);

        [HttpGet("/ProcessingJobs")]
        public JobList<ProcessingJobDto> ProcessingJobs(int from, int count) => _storage.GetMonitoringApi().ProcessingJobs(from,count);
        
        [HttpGet("/ScheduledJobs")]
        public JobList<ScheduledJobDto> ScheduledJobs(int from, int count) => _storage.GetMonitoringApi().ScheduledJobs(from,count);

        [HttpGet("/SucceededJobs")]
        public JobList<SucceededJobDto> SucceededJobs(int from, int count) => _storage.GetMonitoringApi().SucceededJobs(from,count);
       
        [HttpGet("/FailedJobs")]
        public JobList<FailedJobDto> FailedJobs(int from, int count) => _storage.GetMonitoringApi().FailedJobs(from,count);
        
        [HttpGet("/DeletedJobs")]
        public JobList<DeletedJobDto> Queues(int from, int count) => _storage.GetMonitoringApi().DeletedJobs(from, count);
        
    }
}