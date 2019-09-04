using System;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Castle.Facilities.Logging;
using EasyNetQ;
using EasyNetQ.NonGeneric;
using SuperRocket.Message;

namespace SuperRocket.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            //Bootstrapping ABP system
            using (var bootstrapper = AbpBootstrapper.Create<MyConsoleAppModule>())
            {
                bootstrapper.IocManager
                    .IocContainer
                    .AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

                bootstrapper.Initialize();

                //Getting a Tester object from DI and running it
                using (var tester = bootstrapper.IocManager.ResolveAsDisposable<Tester>())
                {
                    //tester.Object.Run();
                } //Disposes tester and all it's dependencies

                using (var bus = RabbitHutch.CreateBus("host=localhost"))
                {
                    bus.SubscribeAsync<TextMessage>("test", message => Task.Factory.StartNew(() =>
                    {
                        // Perform some actions here
                        // If there is a exception it will result in a task complete but task faulted which
                        // is dealt with below in the continuation
                    }).ContinueWith(task =>
                    {
                        if (task.IsCompleted && !task.IsFaulted)
                        {
                            // Everything worked out ok
                            using (var reportGenerator = bootstrapper.IocManager.ResolveAsDisposable<ReportGenerator>())
                            {
                                reportGenerator.Object.Run(message.Text);
                            } //Disposes tester and all it's dependencies
                            WriteTextMessageToConsole(message);
                        }
                        else
                        {
                            // Don't catch this, it is caught further up the hierarchy and results in being sent to the default error queue
                            // on the broker
                            throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                        }
                    }));

                    System.Console.WriteLine("Listening for messages. Hit <return> to quit.");
                    System.Console.ReadLine();
                }

                System.Console.WriteLine("Press enter to exit...");
                System.Console.ReadLine();
            }

        }

        static void WriteTextMessageToConsole(TextMessage textMessage)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"Got message {textMessage.Text}");
        }
    }
}