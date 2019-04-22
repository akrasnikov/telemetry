using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Telegram
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramService telegramService = new TelegramService();
            telegramService.Start();
            //var rc = HostFactory.Run(host =>
            //{

            //    host.Service<TelegramService>(configurator =>
            //    {
            //        configurator.ConstructUsing(service => new TelegramService());
            //        configurator.WhenStarted(service => service.Start());
            //        configurator.WhenStopped(service => service.Stop());
            //    });

            //    host.StartAutomatically();
            //    host.RunAsLocalSystem();

            //    host.SetDescription(" Service управления медрепами");
            //    host.SetDisplayName(" Telegram Bot - Medrep ");
            //    host.SetServiceName(" Telegram Bot - Medrep ");
            //});

            //var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            //Environment.ExitCode = exitCode;

            //Console.WriteLine("Stop");
            Console.ReadLine();
        }
    }
}
