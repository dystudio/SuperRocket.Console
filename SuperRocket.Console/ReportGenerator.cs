using System;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using Abp.Events.Bus;
using Abp.BackgroundJobs;
using SuperRocket.Orchard.Job;

namespace SuperRocket.Console
{
    //Entry class of the test. It uses constructor-injection to get a repository and property-injection to get a Logger.
    public class ReportGenerator : ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IEventBus _eventBus;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ReportGenerator(
            IEventBus eventBus,
            IBackgroundJobManager backgroundJobManager
            )
        {
            _eventBus = eventBus;
            _backgroundJobManager = backgroundJobManager;

            Logger = NullLogger.Instance;
        }

        public void Run(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Started ReportGenerator.Run() on {0} successfully!", DateTime.Now.ToString());

            System.Console.WriteLine($"Generate report from message: {message}");

            System.Console.WriteLine("Finished ReportGenerator.Run() on {0} successfully!", DateTime.Now.ToString());
        }
    }
}