

using System;
using System.Collections.Generic;
using System.Linq;

using Artel.Telemetry.Domain.Model;
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
        Diller product;

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
           

            var text = "Введите штрих код";
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: null);

        }

        internal void CheckProductIterator(TelegramBotClient bot, Message message) => iterator(bot, message);

        internal async Task CheckProductByBarcode(TelegramBotClient bot, Message message)
        {
            try
            {
                var barcode = Convert.ToInt64(message.Text);
                using (IProductRepository productRepository = new ProductRepository())
                {
                    var product = productRepository.GetProductByBarcode(barcode.ToString());
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Системой отправлены фото по штрикоду: {barcode}", replyMarkup: null);
                    await bot.SendPhotoAsync(message.Chat.Id, product.Camera_BackLeft, "Вид сзади слева");
                    await bot.SendPhotoAsync(message.Chat.Id, product.Camera_BackRight, "Вид сзади справа");
                    await bot.SendPhotoAsync(message.Chat.Id, product.Camera_FronView, "Вид спреди");
                }
            }
            catch (FormatException ex)
            {
                throw new Exception("формат сообщения не соотвествует штрихкоду");

            }
            finally
            {

            }
        }

        internal async Task CancelProcedure(TelegramBotClient bot, Message message)
        {
            this.productState = WorkflowState.Default;
            iterator = CheckProductByBarcode;
            await RootMenu(bot, message);
        }



    }
}
