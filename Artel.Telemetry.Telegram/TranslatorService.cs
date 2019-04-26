using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Telegram
{
    internal class TranslatorService
    {
        public static Dictionary<string, Dictionary<string, string>> Select { get; } = SelectLanguage();

        private static Dictionary<string, Dictionary<string, string>> SelectLanguage()
        {
            return new Dictionary<string, Dictionary<string, string>>
            {
                { "ru",  RussianWord()},
                { "en", EnglishWord()},
                 { "uz", UzbekWord()}
            };
        }

        public static Dictionary<string, string> Russian { get; } = RussianWord();
        public static Dictionary<string, string> English { get; } = EnglishWord();
        public static Dictionary<string, string> Uzbek { get; } = UzbekWord();

        private static Dictionary<string, string> UzbekWord()
        {
            return new Dictionary<string, string>
            {
                { "welcome", "Shtrix kod yuboring, lekin minutiga bir martadan ortiq emas" },
                { "fault", "Shtrix kod bo’yicha rasm yo'q"},
                { "sent", "Rasmlar shtrix kod bo’yicha yuboriladi" },
                { "frontview","Old ko’rinish" },
                { "backright", "Orqa fondan ko'rinish - o'ng" },
                { "backleft", "Orqa fondan ko'rinish - chap" },
                { "Contact", "Ma’lumotlaringizni ro'yxatdan o'tkazish uchun yuboring" },
                { "language", "Siz o'zbek tilini tanladingiz, bu parametrni saqlash uchun bir daqiqa kuting." }
            };
        }

        private static Dictionary<string, string> EnglishWord()
        {
            return new Dictionary<string, string>
            {
                { "welcome", "Send a barcode, but no more than once per minute" },
                { "fault", "No photo on barcode"},
                { "sent", "Send a photo on the barcode" },
                { "frontview","View from the foreground" },
                { "backright", "View from the background - right" },
                { "backleft", "View from the background - left" },
                { "Contact", "Send your contacts to register" },
                { "language", "You have chosen English. Wait a minute to save this parameter." }
            };
        }

        private static Dictionary<string, string> RussianWord()
        {
            return new Dictionary<string, string>
            {
                { "welcome", "Отправьте штрихкод, но не чаще чем один раз в минуту" },
                { "fault", "Нет фото по штрихкоду"},
                { "sent", "Отправляются фото по штрихкоду" },
                { "frontview","Вид с переднего плана" },
                { "backright", "Вид с заднего плана - справа" },
                { "backleft", "Вид с заднего плана - слева" } ,
                { "Contact", "Отправьте свои контакты для регистрации" },
                { "language", "Вы выбрали русский язык. Подождите минуту для сохранения  этого параметра" }
            };
        }
    }
}
