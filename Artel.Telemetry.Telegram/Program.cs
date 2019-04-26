using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Artel.Telemetry.Telegram
{
    class Program
    {
        static void Main(string[] args)
        {
            //TelegramService telegramService = new TelegramService();
           
            var rc = HostFactory.Run(host =>
            {

                host.Service<TelegramService>(configurator =>
                {
                    configurator.ConstructUsing(service => new TelegramService());
                    configurator.WhenStarted(service => service.Start());
                    configurator.WhenStopped(service => service.Stop());
                });

                host.StartAutomatically();
                host.RunAsLocalSystem();

                host.SetDescription(" Service управления  дилерами");
                host.SetDisplayName(" Telegram Bot - Artel Product ");
                host.SetServiceName(" Telegram Bot - Artel Product ");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

            Console.WriteLine("Stop");
            Console.ReadLine();
        }
    }
}
