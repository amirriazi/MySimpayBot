using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {
        public void AppsShow()
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            colK.Add(new InlineKeyboardButton()
            {
                Text = "کافه بازار",
                Url = "https://cafebazaar.ir/app/ir.altontech.newsimpay/?l=fa"

            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "Google Play",
                Url = "https://play.google.com/store/apps/details?id=ir.altontech.newsimpay"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "iOS",
                Url = "https://new.sibapp.com/applications/simpay"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "Website",
                Url = "http://simpay.ir"
            });

            inlineK.Add(colK.ToArray());
            colK.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();

            var msgToSend = "لطفا با توجه به نوع سیستم عامل، اپلیکیشین سیم پی را از مسیرهای زیر فراخوانی و نصب نمایید";

            telegramAPI.send(msgToSend, markup);


        }
    }
}