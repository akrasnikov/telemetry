using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramBotClient bot;
            bot = new TelegramBotClient("683303941:AAG6fKpEl7a_Wyh1exIp9aoiOCLw-3hq2z4");
            bot.StartReceiving();
            var me = bot.GetMeAsync().Result;
            Console.WriteLine($"Hello! My name is {me.FirstName}");
            Console.ReadLine();
        }
    }
}
