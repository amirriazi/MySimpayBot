using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {
        private TMTN.Manager mtnManager = null;

        private void MTNRequestInfo(string fieldId = "", dynamic value = null, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msgToSend = "";
            var doSendMessage = true;
            mtnManager = new TMTN.Manager(chatId, id);
            if (String.IsNullOrEmpty(fieldId))
            {
                telegramAPI.send("در این بخش شما میتوانید با وارد نمودن شماره موبایل و مبلغ بدهی، قبوض ایرانسل خود را پرداخت نمایید", cancelButton());

                fieldId = "mobile";
            }
            switch (fieldId.ToLower())
            {
                case "mobile":
                    msgToSend = " لطفا شماره موبایل مورد نظر را وارد نمایید:";
                    currentAction.set(SimpaySectionEnum.MTNServices, "mobile", id.ToString());
                    break;
                case "amount":
                    msgToSend = "در این مرحله، لطفا مبلغ مورد نظر را به ریال وارد نمایید:";
                    currentAction.set(SimpaySectionEnum.MTNServices, "amount", id.ToString());
                    break;

                case "buyservice":
                    doSendMessage = false;
                    MTNBuyService();
                    break;

                default:
                    break;
            }
            if (doSendMessage && !String.IsNullOrEmpty(msgToSend))
                telegramAPI.send(msgToSend);


        }


        private void MTNVerifyUserEntry(string field, dynamic value, string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var id = 0;
            if (!String.IsNullOrEmpty(currentId))
            {
                id = Convert.ToInt32(currentId);
            }
            do
            {
                switch (field.ToLower())
                {
                    case "mobile":
                        var mobile = new Mobile() { Number = (string)value };
                        if (!mobile.IsNumberContentValid())
                        {
                            telegramAPI.send("شماره موبایل ورودی بنظر درست نیست لطفا مجددا وارد نمایید.");
                            break;
                        }
                        MTNUpdateInfo(field, mobile.InternationalNumber, id);
                        break;
                    case "amount":

                        if (!Utils.isInteger(value))
                        {
                            telegramAPI.send("مبلغ وارد شده درست نیست.");
                            break;
                        }
                        var amount = Convert.ToInt32(value);
                        MTNUpdateInfo(field, amount, id);
                        break;

                    default:
                        MTNUpdateInfo(field, value, id);
                        break;
                }
            } while (false);
        }
        private void MTNUpdateInfo(string field, dynamic value, long id, bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var nextStepField = "";
            mtnManager = new TMTN.Manager(chatId, id);
            var msgToSend = "";
            dynamic stepValue = null;

            do
            {
                switch (field.ToLower())
                {
                    case "mobile":
                        var mobileNumber = (string)value;
                        mtnManager.data.mobileNumber = mobileNumber;
                        mtnManager.data.packageId = 70;
                        mtnManager.setInfo();
                        if (id == 0)
                            id = mtnManager.data.id;

                        nextStepField = "amount";

                        msgToSend = $"شماره موبایل: {Utils.removeWhiteSpace(mtnManager.data.mobileNumber)}";
                        currentAction.remove();

                        break;
                    case "amount":
                        var amount = (int)value;
                        mtnManager.data.amount = amount;
                        mtnManager.data.packageId = 70;
                        mtnManager.setInfo();
                        if (id == 0)
                            id = mtnManager.data.id;

                        nextStepField = "buyservice";

                        msgToSend = $"مبلغ وارد شده: {mtnManager.data.amount.ToString("#,##")} ریال ";
                        currentAction.remove();

                        break;

                    default:
                        break;
                }


            } while (false);
            if (!String.IsNullOrEmpty(msgToSend))
            {
                if (callbackQuery == null || forceNewWindow)
                {
                    telegramAPI.send(msgToSend);
                }
                else
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend);
                }

            }
            if (!String.IsNullOrEmpty(nextStepField))
            {
                MTNRequestInfo(nextStepField, stepValue, id);
            }

        }

        private void MTNBuyService()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msg = "";
            do
            {

                mtnManager.BuyService();
                if (mtnManager.resultAction.hasError)
                {
                    sendMenu(message: mtnManager.resultAction.message);
                    break;
                }

                msg += $" مبلغ درخواستی برای شماره موبایل {Utils.removeWhiteSpace(mtnManager.data.mobileNumber)} برابر است با {mtnManager.data.amount.ToString("#,##")} ریال: \n \n";
                msg += "در صورت تایید، لطفا برای پرداخت به لینک زیر بروید. ";
                msg += "----";


                var resultLink = SimpayCore.getPaymentLink(mtnManager.data.saleKey);
                if (SimpayCore.resultAction.hasError)
                {
                    telegramAPI.send(SimpayCore.resultAction.message);
                    break;
                }
                sendPaymentMessage(resultLink, msg);

            } while (false);
        }
        private void MTNServiceRedeem(string saleKey)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msg = "";
            do
            {
                mtnManager = new TMTN.Manager(chatId);
                mtnManager.redeem(saleKey);
                if (mtnManager.resultAction.hasError)
                {
                    sendMenu(message: mtnManager.resultAction.message);
                    break;
                }

                msg += $" سرویس خریداری شده برای شماره موبایل {Utils.removeWhiteSpace(mtnManager.data.mobileNumber)} به شرح زیر میباشد: \n \n";
                msg += $" {mtnManager.data.packageDescription}: \n \n \n ";

                sendMenu(message: msg);

            } while (false);
        }
    }
}