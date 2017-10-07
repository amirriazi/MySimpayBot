using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Models
{
    public partial class TelegramMessage
    {
        public void PaymentStartProcess(string saleKey, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            Def.MyDbLogger.playLoad = $" Caller={memberName}.{sourceLineNumber} saleKey={saleKey}";
            Def.MyDbLogger.reportLog();

            //Log.Info($"PaymentStartProcess: ChatId={chatId} caller={memberName}.{sourceLineNumber} saleKey={saleKey} ", 0);
            do
            {
                var stackTrace = new StackTrace();
                if (String.IsNullOrEmpty(saleKey))
                {
                    var adminTelegramAPI = new myTelegramApplication.TelegramAPI(ProjectValues.adminChatId);
                    if (thisUser == null)
                    {
                        thisUser = new UserModel(chatId);
                    }
                    adminTelegramAPI.send($"PaymentStartProcess: sale key is empty! Called from {stackTrace.GetFrame(1).GetMethod().DeclaringType}.{stackTrace.GetFrame(1).GetMethod().Name} userId:{thisUser.userId}");
                    //adminTelegramAPI.send($"PaymentStartProcess: sale key is empty! Called from {Utils.getLastFrame()} userId:{thisUser.userId}");
                    telegramAPI.send("با عرض پوزش، اشکالی در ارتباط با سرویس دهنده بوجود آمده است. لطفا چند دقیقه دیگر دوباره سعی نمایید.");
                    break;
                }
                var payment = new Payment.Manager(chatId);
                payment.data.saleKey = saleKey;
                payment.data.status = "start";
                payment.setInfo();

                var inlineK = new List<KeyboardButton[]>();
                var colK = new List<KeyboardButton>();
                colK.Add(new KeyboardButton()
                {
                    Text = "بله",
                    //CallbackData = $"{SimpaySectionEnum.Payment}:discount:{payment.data.id}:1"
                });
                colK.Add(new KeyboardButton()
                {
                    Text = "خیر",
                    //CallbackData = $"{SimpaySectionEnum.Payment}:discount:{payment.data.id}:0"
                });
                inlineK.Add(colK.ToArray());
                colK.Clear();

                var markup = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup();
                markup.Keyboard = inlineK.ToArray();
                markup.OneTimeKeyboard = true;
                markup.ResizeKeyboard = true;

                currentAction.set(SimpaySectionEnum.Payment, "discountask", payment.data.id.ToString());

                telegramAPI.send(" در صورتیکه برای شما کد تخفیف صادر شده است، آیا مایلید از آن در این خرید استفاده نمایید؟", markup);


            } while (false);

        }

        public void PaymentUpdateInfo(string field, string value, long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var payment = new Payment.Manager(chatId, id);
            var msgToSend = "";
            do
            {

                switch (field.ToLower())
                {
                    case "discountask":
                        if (value == "بله")
                        {
                            currentAction.remove();
                            PaymentAskForDiscount(id);
                        }
                        else if (value == "خیر")
                        {
                            currentAction.remove();
                            PaymentInputDiscount("0", id);
                        }
                        else
                        {
                            msgToSend = "لطفا فقط با بله یا خیر پاسخ دهید.";
                        }
                        break;
                    case "discountcode":
                        PaymentInputDiscount(value, id);
                        break;

                    default:
                        break;
                }
            } while (false);

            if (!String.IsNullOrEmpty(msgToSend))
            {
                telegramAPI.send(msgToSend);
            }

        }
        private void PaymentCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = Convert.ToInt32(data.Split(':')[2]);
            do
            {
                switch (action)
                {
                    case "discount":
                        var answerHasDiscount = Convert.ToInt16(data.Split(':')[3]);
                        if (answerHasDiscount == 1)
                        {
                            PaymentAskForDiscount(id);
                        }
                        else
                        {
                            PaymentInputDiscount("0", id);
                        }

                        break;

                    default:
                        break;
                }


            } while (false);

        }

        private void PaymentAskForDiscount(long paymentId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var payment = new Payment.Manager(chatId, paymentId);
                if (payment.data.status == "getPaymentLink" || payment.data.paymentFinished)
                {
                    break;
                }

                currentAction.set(SimpaySectionEnum.Payment, "discountcode", paymentId.ToString());
                telegramAPI.send("لطفا کد تخفیف را وارد نمایید. در صورت انصراف عدد 0 (صفر) را وارد نمایید.", cancelButton());

            } while (false);

        }
        private void PaymentInputDiscount(string discountCode, long paymentId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                currentAction.remove();
                if (chatId == 142711721 || chatId == 59609141 || chatId == 233616340)
                {
                    if (discountCode.ToLower() == "simpaybot9603")
                    {
                        discountCode = "0";
                    }

                }
                var payment = new Payment.Manager(chatId, paymentId);
                if (payment.data.status == "getPaymentLink" || payment.data.paymentFinished)
                {
                    break;
                }

                telegramAPI.send("درحال ایجاد پیش فاکتور شما...", cancelButton());
                if (discountCode == "0")
                {
                    payment.data.discountCode = "";
                }
                else
                {
                    payment.data.discountCode = discountCode;
                }
                payment.data.status = "discount";
                payment.setInfo();

                PaymentShowInformation(payment);
            } while (false);
        }

        private void PaymentShowInformation(Payment.Manager payment, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                Def.MyDbLogger.action = "PaymentShowInformation";
                Def.MyDbLogger.playLoad = $"ChatId={payment.chatId} Caller={memberName}.{sourceLineNumber} {Utils.ConvertClassToJson(payment)}";
                Def.MyDbLogger.reportLog();

                Log.Info($"GetSalePaymentInfo: ChatId={payment.chatId} Caller={memberName}.{sourceLineNumber} {Utils.ConvertClassToJson(payment)}", 0);
                if (payment.data.status == "getPaymentLink" || payment.data.paymentFinished)
                {
                    break;
                }

                if (String.IsNullOrEmpty(payment.data.saleKey))
                {

                    Log.Error($"GetSalePaymentInfo Sale Key Empty: ChatId={payment.chatId}  Caller={memberName}.{sourceLineNumber} {Utils.ConvertClassToJson(payment)}", 0);
                    break;

                }
                var paymentInfo = SimpayCore.GetSalePaymentInfo(payment.data.saleKey, payment.data.discountCode);
                if (SimpayCore.resultAction.hasError)
                {
                    if (payment.data.discountCode != "")
                    {
                        telegramAPI.send(SimpayCore.resultAction.message);
                        PaymentAskForDiscount(payment.data.id);
                        break;
                    }
                    else
                    {
                        if (chatId == ProjectValues.adminChatId)
                        {
                            telegramAPI.send("GetSalePaymentInfo" + SimpayCore.resultAction.message);
                        }
                        Log.Error("GetSalePaymentInfo" + SimpayCore.resultAction.message, 0);
                        sendMenu(message: "با عرض پوزش ، بعلت در دسترس نبودن صفحه بانک، عملیات متوقف گردید");
                        break;
                    }
                }

                payment.data.paymentIsPossible = paymentInfo.PaymentIsPossible;
                payment.data.discountAmount = Convert.ToInt32(paymentInfo.DiscountAmount);
                payment.data.amount = Convert.ToInt32(paymentInfo.Amount);
                payment.data.description = paymentInfo.Description;
                payment.data.productId = Convert.ToInt32(paymentInfo.ProductID);
                payment.data.productName = paymentInfo.ProductName;
                payment.data.status = "GetSalePaymentInfo";
                payment.setInfo();

                if (!payment.data.paymentIsPossible)
                {
                    sendMenu(message: payment.data.description);
                    break;
                }

                var paymentLink = SimpayCore.getPaymentLink(payment.data.saleKey);
                if (SimpayCore.resultAction.hasError)
                {
                    sendMenu(message: SimpayCore.resultAction.message);
                    break;

                }
                payment.data.status = "getPaymentLink";
                payment.setInfo();


                var msg = "";
                msg += $"عنوان محصول {Environment.NewLine }";
                msg += $"{payment.data.productName} {Environment.NewLine + Environment.NewLine}";
                msg += $"شرح محصول {Environment.NewLine }";
                msg += $"{payment.data.description} {Environment.NewLine + Environment.NewLine}";
                if (payment.data.discountAmount != 0)
                {
                    msg += $"مبلغ تخفیف{Environment.NewLine }";
                    msg += $"{payment.data.discountAmount.ToString("0,##")} ریال {Environment.NewLine + Environment.NewLine}";
                }

                msg += $"مبلغ کل{Environment.NewLine }";
                msg += $"{payment.data.amount.ToString("#,##")} ریال {Environment.NewLine + Environment.NewLine}";
                if (payment.data.discountAmount != 0)
                {
                    msg += $"مبلغ قابل پرداخت {Environment.NewLine }";

                    var totalPaid = payment.data.amount - payment.data.discountAmount;
                    if (totalPaid > 0)
                    {
                        msg += $"{totalPaid.ToString("0,##")} ریال {Environment.NewLine + Environment.NewLine}";
                    }
                    else
                    {
                        msg += $" صفر ریال {Environment.NewLine + Environment.NewLine}";
                    }
                }


                sendPaymentMessage(paymentLink, msg);
            } while (false);
        }
    }
}