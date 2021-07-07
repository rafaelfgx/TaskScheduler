using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskSchedulerService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder application, IJobService jobService)
        {
            application.UseHsts();
            application.UseHttpsRedirection();
            application.UseRouting();
            application.UseResponseCompression();
            application.UseEndpoints(endpoints => endpoints.MapControllers());
            application.UseHangfireDashboard();
            jobService.Configure();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration.Get<AppSettings>());
            services.AddSingleton<IJobService, JobService>();
            services.AddResponseCompression();
            services.AddControllers();
            services.AddHangfire(hangfire => hangfire.UseSqlServerStorage(_configuration.GetConnectionString("Database")));
            services.AddHangfireServer();
        }
    }
}
