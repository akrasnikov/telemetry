

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
using System.Text;
using System.IO;

namespace Artel.Telemetry.Telegram
{

    internal class ProductService
    {
        WorkflowState productState;
        Dictionary<string, string> translator;

        Func<TelegramBotClient, Message, Task> iterator;

        public WorkflowState State { get => productState; set => productState = value; }

        public ProductService(Message message)
        {
            this.productState = WorkflowState.Default;
            translator = CustomerLanguage(message.Chat.Id);
        }

        private Dictionary<string, string> CustomerLanguage(long telegramid)
        {
            string language = string.Empty;
            using (ICustomerRepository customer = new CustomerRepository())
            {
                language = customer.Language(telegramid);
                if (language is null) language = "en";
            }
            return TranslatorService.Select[language];
        }
        internal async Task SelectLanguageAsync(TelegramBotClient bot, Message message, string language = "ru")
        {            
            using (ICustomerRepository customer = new CustomerRepository())
            {
                customer.SetLanguage(message.Chat.Id, language);
                translator = TranslatorService.Select[language];
            }
            var text = translator["language"];
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: null);
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
            ReplyKeyboardMarkup languageKeyboard = new[]
            {
                new[] { "Русский", "English", "Ўзбекча" }
            };
            languageKeyboard.Selective = true;
            languageKeyboard.ResizeKeyboard = true;
            var text = translator["welcome"];
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: languageKeyboard);
        }

        internal void CheckProductIterator(TelegramBotClient bot, Message message) => iterator(bot, message);

        bool servicebusy = false;
        internal async Task CheckProductByBarcode(TelegramBotClient bot, Message message)
        {
            try
            {
                var barcode = message.Text;


                using (IProductRepository productRepository = new ProductRepository())
                {
                    var product = productRepository.GetProductByBarcode(barcode);
                    if (product is null)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, $"{translator["fault"]}: {barcode}", replyMarkup: null);
                        return;
                    }

                    servicebusy = true;

                    await bot.SendTextMessageAsync(message.Chat.Id, $" {translator["sent"]}: {barcode}", replyMarkup: null);
                    await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                    using (var fileStream = new FileStream(product.Camera_BackLeft, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await bot.SendPhotoAsync(message.Chat.Id, fileStream, translator["backleft"]);
                    };
                    await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                    using (var fileStream = new FileStream(product.Camera_BackRight, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await bot.SendPhotoAsync(message.Chat.Id, fileStream, translator["backright"]);
                    };
                    await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                    using (var fileStream = new FileStream(product.Camera_FronView, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await bot.SendPhotoAsync(message.Chat.Id, fileStream, translator["frontview"]);
                    };

                }
            }
            catch (FormatException ex)
            {
                //throw new Exception("формат сообщения не соотвествует штрихкоду");

            }
            finally
            {
                servicebusy = false;
            }
        }

        internal async Task CancelProcedure(TelegramBotClient bot, Message message)
        {
            this.productState = WorkflowState.Default;
            iterator = CheckProductByBarcode;
            await RootMenu(bot, message);
        }

        internal bool WaitSendPhoto()
        {
            return servicebusy;
        }

        DateTime prevdateTime;
        internal bool MakeRequest(DateTime curentdata)
        {
            var result = curentdata.Subtract(prevdateTime);
            prevdateTime = curentdata;
            if (result.Minutes < 1) return true;
            else return false;
        }

        internal void ClearBarcode()
        {
            try
            {
                using (IProductRepository productRepository = new ProductRepository())
                {
                    productRepository.ClearBarcodeCRLF();
                }
            }
            finally
            {

            }
        }       
    }
}
