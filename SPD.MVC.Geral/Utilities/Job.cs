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
        public Notification Notification { get; set; }


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

        public Job WithNotification(string message, bool AlteracaoFunc, params string[] informations)
        {
            if (!AlteracaoFunc)
                this.Notification.Message = String.Format(CultureInfo.InvariantCulture, message, informations);
            else
                this.Notification.Message = message;


            this.Notification.AlteracaoPerfil = AlteracaoFunc;

            return this;
        }

        public Job OfWarningType()
        {
            this.Notification.Type = NotificationHub.NotificationType.WARNING;

            return this;
        }

        public Job OfDangerType()
        {
            this.Notification.Type = NotificationHub.NotificationType.DANGER;

            return this;
        }

        public Job NotifyAll()
        {
            this.Notification.For = NotificationHub.NotificationFor.All;

            return this;
        }

        public Job NotifyOthersExcept(int exceptUserID)
        {
            this.Notification.For = NotificationHub.NotificationFor.Others;

            this.Notification.UserID = exceptUserID;

            return this;
        }

        public Job NotifyUser(int userID)
        {
            this.Notification.For = NotificationHub.NotificationFor.User;

            this.Notification.UserID = userID;

            return this;
        }

        public Job NotifyUntil(long until)
        {
            if (this.CreatedTime.AddSeconds(until).Ticks <= this.Notification.CountdownLimit)
            {
                this.Notification.CountdownLimit = this.CreatedTime.AddSeconds(until).Ticks;
            }
            else
            {
                throw new Exception("The given parameter is greater than the countdown limit.");
            }

            return this;
        }
    }
}
