
using Artel.Telemetry.Domain.BaseClasses;
using Artel.Telemetry.Domain.Data;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Artel.Telemetry.Telegram
{
    internal class CustomerService
    {

        internal static async Task DrawKeybordContact(TelegramBotClient bot, Message message)
        {
            var contactKeyboard = new ReplyKeyboardMarkup(new[] { KeyboardButton.WithRequestContact("Контакты") })
            {
                Selective = true,
                ResizeKeyboard = true
            };
            var text = "Отправьте свои контакты для регистрации в системе";
            await bot.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: contactKeyboard);

        }

        internal static async Task Create(TelegramBotClient bot, Message message)
        {
            using (ICustomerRepository customerRepository = new CustomerRepository())
            {
                var contactName = $"{message.Contact.FirstName}  {message.Contact.LastName}";
                var contactPhone = message.Contact.PhoneNumber;
                var telegramid = (int)message.Chat.Id;
                var IsEnabled = true;
                var dateOfCreation = DateTime.Now;
                customerRepository.Create(contactName, contactPhone, telegramid, IsEnabled, dateOfCreation);

                var text = $"Уважаемый {contactName}, Вы зарегестрировались в системе по номеру {contactPhone}";
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                await bot.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Html);
            }            
        }

        internal static bool IsEnabled(int id)
        {
            var isEnabled = false;
            using (ICustomerRepository customerRepository = new CustomerRepository())
            {
                isEnabled = customerRepository.CustomerIsEnabled(id);
            }
            return isEnabled;
        }
        internal static bool NotRegistered(long chatid)
        {
            var isEnabled = true;
            using (ICustomerRepository customerRepository = new CustomerRepository())
            {
                isEnabled = !customerRepository.CustomerIsEnabled(chatid);
            }
            return isEnabled;
        }
    }
}