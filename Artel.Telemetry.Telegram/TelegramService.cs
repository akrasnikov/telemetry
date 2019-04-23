using Artel.Telemetry.Common.Extension;
using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Data;
using Artel.Telemetry.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using Telegram.Bot.Types.InlineQueryResults;



namespace Artel.Telemetry.Telegram
{
    internal class TelegramService
    {

        TelegramBotClient bot;
        readonly string token = "813166674:AAEvj_It1UFXvtoP-JfbRkqrPETTmr8cIjM";

        ObjectCache memorycache;
        CacheItemPolicy policy;

        DebugInfo debug;

        public TelegramService()
        {
            bot = new TelegramBotClient(token);
            bot.OnMessage += BotOnMessageReceived;

            bot.OnInlineQuery += Bot_OnInlineQuery;
            bot.OnUpdate += Bot_OnUpdate;
           

            //NameValueCollection CacheSettings = new NameValueCollection(3);
            //CacheSettings.Add("CacheMemoryLimitMegabytes", Convert.ToString(100));
            //CacheSettings.Add("physicalMemoryLimitPercentage", Convert.ToString(49));  //set % here
            //CacheSettings.Add("pollingInterval", Convert.ToString("00:00:10"));
            //cache = new MemoryCache("TestCache", CacheSettings);

            debug = new ConsoleMessage();
            debug.Print("Start Telegram Service");
        }

        private void Bot_OnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            switch (e.Update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    break;
                case UpdateType.InlineQuery:
                    Console.WriteLine("UpdateType.InlineQuery:" + e.Update.InlineQuery.Id);
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    Console.WriteLine(e.Update.CallbackQuery.Data);
                    try
                    {
                        await bot.SendTextMessageAsync(
                            e.Update.CallbackQuery.Message.Chat.Id,
                            $"Your choice is: {e.Update.CallbackQuery.Data}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                default:
                    Console.WriteLine(e.Update);
                    break;
            }
        }
        internal void Start() => bot.StartReceiving(Array.Empty<UpdateType>());
        internal void Stop() => bot.StopReceiving();


        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                var message = messageEventArgs.Message;

                if (message == null) return; // Todo - логи 
                var chatId = message.Chat.Id.ToString();

                debug.Print($"BotOnMessageReceived: {message.Text} от {message.Chat.Id}");

                if (CustomerService.NotRegistered(message.Chat.Id))
                {
                    if (message.Type == MessageType.Contact) await CustomerService.Create(bot, message);
                    await CustomerService.DrawKeybordContact(bot, message);
                    return;
                }



                memorycache = MemoryCache.Default;
                //policy = new CacheItemPolicy();
                //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1); // Timeout регистрации аптеки   

                ProductService productService = memorycache[chatId] as ProductService;

                if (productService is null)
                {
                    productService = new ProductService();
                    memorycache.Set(chatId, productService, DateTime.Now.AddMinutes(10));
                }

                //if (message.Type == MessageType.Location) if (dillerService.State == DillerState.Find) await dillerService.GetPharmacyByLocationAsync(bot, message);


                if (message.Type == MessageType.Text)
                {
                    switch (message.Text.Split(' ').First())
                    {
                        case "Отменить":
                            await productService.CancelProcedure(bot, message);
                            return;
                    }

                    await productService.CheckProductByBarcode(bot, message);
                }

                switch (productService.State)
                {
                    case WorkflowState.Check:
                        productService.CheckProductIterator(bot, message);
                        break;
                    case WorkflowState.Default:
                        await productService.RootMenu(bot, message);
                        break;
                }
                //debug.Print($"Pharmacy service state: {pharmacyService.State}");
                //await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                //await bot.SendLocationAsync(message.Chat.Id, latitude: 40.7058316f, longitude: -74.2581888f);
            }
            finally
            {

            }
        }

    }
}
