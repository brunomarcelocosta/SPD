using SPD.MVC.Geral.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SPD.MVC.Geral.Utilities
{
    public sealed class Job
    {
        private static readonly List<Job> CurrentJobs;

        private DateTime CreatedTime { get; set; }
        public DateTime IntervalTime { get; }
        public bool IsFinished { get; private set; }
        private event StartEventHandler StartEvent;

        static Job()
        {
            Job.CurrentJobs = new List<Job>();
        }

        public static Job ScheduleNewJob(long seconds, StartEventHandler startEvent)
        {
            Job.CurrentJobs.Add(new Job(seconds, startEvent));

            return Job.CurrentJobs.Last();
        }

        public static void StartReadyJobs(long ticks)
        {
            var jobs = Job.CurrentJobs.Where(internalJob => internalJob.IntervalTime.Ticks <= ticks).ToList();

            foreach (var job in jobs)
            {
                job.Start();
            }
        }

        private Job(long seconds, StartEventHandler startEvent)
        {
            this.CreatedTime = DateTime.Now;
            this.IntervalTime = this.CreatedTime.AddSeconds(seconds);
            this.IsFinished = false;
            this.StartEvent += startEvent;
        }

        private void Start()
        {
            if (Job.CurrentJobs.Remove(this))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "The job \"{0}\" was removed from the current job list.", this.IntervalTime.Ticks));

                if (this.StartEvent != null)
                {
                    this.StartEvent();

                    this.IsFinished = true;
                }
            }
        }           
    }
}
