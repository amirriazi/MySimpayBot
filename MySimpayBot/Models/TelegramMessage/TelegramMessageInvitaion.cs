using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {
        private void InvitationSendLinkToFriends()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                if (thisUser.activated)
                {
                    var info = SimpayCore.GetInvitationInfo();
                    if (info == null)
                    {
                        telegramAPI.send(message: "ایراد در بخش دریافت کد دعوت!");
                        break;
                    }

                    thisUser.invitationCode = info.InvitationCode;
                    thisUser.reportUser();

                    InvitationSendMessage();
                    break;
                }
                else
                {
                    var msg = "";
                    msg = " .سامانه سیم پی برای کسانی که لینک این بات را با دیگر دوستان درمیان میگذارند، ترتیباتی جهت تشکر در نظر گرفته است. لذا خواهشمند است قبل از ارسال این بات، حتما در سامانه سیم پی ثبت نام فرمایید ";
                    telegramAPI.send(msg);
                    activateUserMessage();
                }

            } while (false);
            //telegramAPI.fileToSend = new FileToSend
            //{
            //    Url = new Uri("http://simpay.ir/images/simpay-logo-1200.png"),
            //};
            //telegramAPI.caption = caption;
            //telegramAPI.send(MessageType.PhotoMessage);

        }
        public void InvitationRuls()
        {
            do
            {
                var rules = "";
                rules += $" قوانین: {Environment.NewLine}{Environment.NewLine}";
                rules += $"  شما حتما باید در سیم‌پی عضو شده باشید.  {Environment.NewLine}";
                rules += $"  دوستان شما حتما باید از طریق لینک مخصوص شما در سیم‌پی عضو شود.  {Environment.NewLine}";
                rules += $"  دوست شما نباید قبلا از طریق دیگری در سیم‎پی عضو شده باشد. {Environment.NewLine}";
                rules += $"  به محض اولین خرید سه نفر از دوستان شما از سیم‌پی، به طور اتوماتیک یک کد تخفیف 10000 ریالی برای استفاده از یکی از محصولات سیم‌پی برای شما ارسال می‌گردد.  {Environment.NewLine}";
                rules += $"  محدودیتی در دعوت از دوستانتان نیست و به ازای دعوت از هر سه نفر، به شرط رعایت موارد بالا، یک کد تخفیف 10000 ریالی جایزه می‌گیرید. {Environment.NewLine}";
                rules += $"  {Environment.NewLine}";
                sendMenu(message: rules);
            } while (false);

        }
        private void InvitationSendMessage()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            //private void sendLinktoFriends()
            //{

            var url = "http://t.me/" + telegramAPI.getMe().username + "?start=" + Convert.ToString(chatId);
            string caption = "";
            //caption += " \n ";
            //caption += $"این ربات از طرف {message.From.FirstName} {message.From.LastName} برای شما ارسال شده است. ";
            //caption += " \n ";
            //caption += ".سیم پی ضرورت زندگی مدرن سریع،ساده،مطمئن";
            //caption += " \n \n ";
            //caption += " لطف با زدن لینک زیر به صفحه ما بیاید ";
            //caption += url;




            caption += $" سلام {Environment.NewLine}";
            caption += $" من چند وقتی هست که با سیم پی آشنا شدم و ازش استفاده می‌کنم. {Environment.NewLine}";
            caption += $" برای اولین بار می‌تونی همه خریدهات رو روی تلگرام انجام بدی، به علاوه می‌تونی با معرفی سیم‌پی به سه نفر از دوستانت 10000 ریال کد تخفیف هدیه بگیری. {Environment.NewLine}";
            caption += $" {Environment.NewLine}";
            caption += $" همه این محصولات توی یک ربات تلگرامی هست: {Environment.NewLine}";
            caption += $"استعلام ریز جرایم {Environment.NewLine}";
            caption += $"خرید شارژ {Environment.NewLine}";
            caption += $"بسته‌های اینترنت {Environment.NewLine}";
            caption += $"بلیت قطار، هواپیما، اتوبوس {Environment.NewLine}";
            caption += $"پرداخت همه قبوض {Environment.NewLine}";
            caption += $"انواع گیفت کارت {Environment.NewLine}";
            caption += $"سوابق خرید {Environment.NewLine}";
            caption += $"نماد اعتماد الکترونیکی هم داره و همه خدماتش هم کاملا رایگانه! {Environment.NewLine}";
            caption += $" با لینک زیر می تونی به ربات سیم پی وارد بشی. {Environment.NewLine}";
            caption += url;

            telegramAPI.send("لطفا پست زیر را برای دوستان ارسال فرمایید.");

            sendMenu(message: caption);

            //telegramAPI.fileToSend = new FileToSend
            //{
            //    Url = new Uri("http://simpay.ir/images/simpay-logo-1200.png"),
            //};
            //telegramAPI.caption = caption;
            //telegramAPI.send(MessageType.PhotoMessage);




        }

        private void InvitationGetStatus()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var info = SimpayCore.GetInvitationStatus();
                if (info == null)
                {
                    sendMenu(message: "این بخش هنوز آماده نیست");
                    break;
                }

                var msg = "";
                msg += $"{info.Description} {Environment.NewLine}{Environment.NewLine}";
                msg += $"تعداد ثبت نام از طریق لینک شما: {info.TotalInvitationCount} {Environment.NewLine}";
                msg += $"تعداد خرید پس از ثبت نام از طریق لینک شما : {info.SentDiscountCount} {Environment.NewLine}";
                msg += $"تعداد کد تخفیف مجاز: {info.ApprovedInvitationCount} {Environment.NewLine}{Environment.NewLine}-";


                sendMenu(message: msg);
            } while (false);
        }


    }
}