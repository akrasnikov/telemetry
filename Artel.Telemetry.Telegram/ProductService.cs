

using System;
using System.Collections.Generic;
using System.Linq;

using Artel.Telemetry.Domain.Models;
using Artel.Telemetry.Domain.BaseClasses;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Artel.Telemetry.Domain.Data;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Args;

namespace Artel.Telemetry.Telegram
{

    internal class ProductService
    {
        WorkflowState productState;
        Pharmacies product;

        Func<TelegramBotClient, Message, Task> iterator;

        public WorkflowState State { get => productState; set => productState = value; }

        public ProductService()
        {
            this.productState = WorkflowState.Default;
        }

        internal async Task FindDillerByLocationAsync(TelegramBotClient bot, Message message)
        {
            this.productState = WorkflowState.Find;
            ReplyKeyboardMarkup locationKeyboard = new ReplyKeyboardMarkup(new[] { KeyboardButton.WithRequestLocation("Ваша локация") })
            {
                Selective = true,
                ResizeKeyboard = true
            };
            var text = "Отправьте вашу локацию";
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: locationKeyboard);
        }

        internal async Task RootMenu(TelegramBotClient bot, Message message)
        {
            this.State = WorkflowState.Default;
            ReplyKeyboardMarkup rootKeyboard = new[]
            {
               new[] { "Проверить", "Назад" }
            };
            rootKeyboard.Selective = true;
            rootKeyboard.ResizeKeyboard = true;

            var text = "Введите штрих код";
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: rootKeyboard);

        }

        internal void CheckProductIterator(TelegramBotClient bot, Message message) => iterator(bot, message);

        internal async Task CheckProductByBarcode(TelegramBotClient bot, Message message)
        { 
            var text = "Введите штрих код";
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: cancelKeyboard);
        }

        internal async Task CancelProcedure(TelegramBotClient bot, Message message)
        {
            this.productState = WorkflowState.Default;
            iterator = CheckProductByBarcode;
            await RootMenu(bot, message);
        }
      
        private async Task SetPharmacyLocationAsync(TelegramBotClient bot, Message message)
        {
            string text = String.Empty;
            try
            {
                if (message.Type != MessageType.Location)
                {
                    text = "Отправьте локацию торговой точки";
                    await bot.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Html);
                    return;
                }
                product.Latitude = message.Location.Latitude;
                product.Longitude = message.Location.Longitude;
                product.DateCreate = DateTime.Now;
                product.IsNew = true;

                this.State = WorkflowState.Default;

                text = "Регистрация торговой точки передана на рассмотрение администратора ";

                using (IFarmacyRepository farmacyRepository = new PharmacyRepository())
                {
                    farmacyRepository.Create(product);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                text = "Ошибка регистрации";
            }
            finally
            {
                await bot.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Html);

                await RootMenu(bot, message);
                product = null;


            }
        }
    }
}
