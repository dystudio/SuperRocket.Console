using System;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using Abp.Events.Bus;
using Abp.BackgroundJobs;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.IO;

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
            List<User> users = new List<User>();
            User user = new User();
            user.Id = Guid.NewGuid();
            user.Name = "David";

            users.Add(user);

            user = new User();
            user.Id = Guid.NewGuid();
            user.Name = "Jack";
            users.Add(user);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");

            ExcelUtility.ExportToExcel(users, path, message, true);

            System.Console.WriteLine("Finished ReportGenerator.Run() on {0} successfully!", DateTime.Now.ToString());
        }
    }
}