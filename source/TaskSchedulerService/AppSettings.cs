using System.Collections.Generic;

namespace TaskSchedulerService
{
    public sealed class AppSettings
    {
        public IReadOnlyList<Job> Jobs { get; set; }
    }
}
