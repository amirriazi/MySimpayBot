using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {
        private TMTN.Manager tmtnManager = null;

        private void TMTNRequestInfo(string fieldId = "", dynamic value = null, long id = 0)
        {
            var msgToSend = "";
            var doSendMessage = true;
            tmtnManager = new TMTN.Manager(chatId, id);
            if (String.IsNullOrEmpty(fieldId))
            {

                //var oldList = (new TMTN.Manager(chatId)).getLastPath();
                //if (oldList.Count > 0)
                //{
                //    telegramAPI.send("در حال فراخوانی مسیرهای قبلی شما", cancelButton());
                //    TMTNFastOptions(oldList);
                //    doSendMessage = false;
                //    break;
                //}


                fieldId = "simtype";
            }
            switch (fieldId.ToLower())
            {
                case "simtype":
                    doSendMessage = false;
                    TMTNGetSimType();
                    break;
                case "category":
                    doSendMessage = false;
                    TMTNGetCategory();
                    break;
                case "package":
                    doSendMessage = false;
                    TMTNGetPackage();
                    break;

                case "mobile":
                    msgToSend = "در این مرحله، لطفا شماره موبایل مورد نظر را وارد نمایید:";
                    currentAction.set(SimpaySectionEnum.TMTNServices, "mobile", id.ToString());
                    break;
                case "buyservice":
                    doSendMessage = false;
                    TMTNBuyService();
                    break;

                default:
                    break;
            }
            if (doSendMessage && !String.IsNullOrEmpty(msgToSend))
                telegramAPI.send(msgToSend);


        }

        private void TMTNCallBackQuery(string data)
        {
            var action = data.Split(':')[1].ToLower();
            var id = Convert.ToInt32(data.Split(':')[2]);
            var msgToSend = "";
            var tmtn = new TMTN.Manager(chatId, id);
            do
            {
                switch (action)
                {
                    case "simtype":
                        var simTypeId = Convert.ToInt32(data.Split(':')[3]);

                        TMTNUpdateInfo(action, simTypeId, id);

                        break;
                    case "category":
                        var categoryId = Convert.ToInt32(data.Split(':')[3]);

                        TMTNUpdateInfo(action, categoryId, id);

                        break;
                    case "package":
                        var packageId = Convert.ToInt32(data.Split(':')[3]);

                        TMTNUpdateInfo(action, packageId, id);

                        break;


                    default:
                        break;
                }

            } while (false);

            if (!String.IsNullOrEmpty(msgToSend))
            {
                telegramAPI.editText(callbackQuery.Message.ID, msgToSend);
            }

        }
        private void TMTNVerifyUserEntry(string field, dynamic value, string currentId = "")
        {
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
                        TMTNUpdateInfo(field, mobile.InternationalNumber, id);
                        break;
                    default:
                        TMTNUpdateInfo(field, value, id);
                        break;
                }
            } while (false);
        }
        private void TMTNUpdateInfo(string field, dynamic value, long id, bool forceNewWindow = false)
        {
            var nextStepField = "";
            tmtnManager = new TMTN.Manager(chatId, id);
            var msgToSend = "";
            dynamic stepValue = null;

            do
            {
                switch (field.ToLower())
                {
                    case "simtype":
                        var simTypeId = (int)value;
                        tmtnManager.data.simTypeId = simTypeId;
                        tmtnManager.data.simTypeShowName = tmtnManager.simtype.GetSimTypeShowName(tmtnManager.data.simTypeId);
                        tmtnManager.setInfo();
                        if (id == 0)
                            id = tmtnManager.data.id;

                        nextStepField = "category";

                        msgToSend = $"نوع سرویس انتخابی: {tmtnManager.data.simTypeShowName}";
                        currentAction.remove();

                        break;
                    case "category":
                        var categoryId = (int)value;
                        tmtnManager.data.categoryId = categoryId;
                        tmtnManager.data.categoryShowName = tmtnManager.category.GetCategoryShowName(tmtnManager.data.categoryId);
                        tmtnManager.setInfo();
                        if (id == 0)
                            id = tmtnManager.data.id;

                        nextStepField = "package";

                        msgToSend = $"دسته بندی انتخابی: {tmtnManager.data.categoryShowName}";
                        currentAction.remove();

                        break;
                    case "package":
                        var packageId = (int)value;
                        tmtnManager.data.packageId = packageId;
                        tmtnManager.data.packageShowName = tmtnManager.package.GetPackageShowName(tmtnManager.data.packageId);
                        tmtnManager.data.packageDescription = tmtnManager.package.GetPackageDescription(tmtnManager.data.packageId);
                        tmtnManager.data.amount = tmtnManager.package.GetPackageAmount(tmtnManager.data.packageId);
                        tmtnManager.setInfo();
                        if (id == 0)
                            id = tmtnManager.data.id;

                        nextStepField = "mobile";

                        msgToSend = $"بسته انتخابی: {tmtnManager.data.packageShowName}";
                        currentAction.remove();

                        break;
                    case "mobile":
                        var mobileNumber = (string)value;
                        tmtnManager.data.mobileNumber = mobileNumber;
                        tmtnManager.setInfo();
                        if (id == 0)
                            id = tmtnManager.data.id;

                        nextStepField = "buyservice";

                        msgToSend = $"شماره موبایل: {Utils.removeWhiteSpace(tmtnManager.data.mobileNumber)}";
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
                TMTNRequestInfo(nextStepField, stepValue, id);
            }

        }

        private void TMTNGetSimType()
        {
            do
            {
                telegramAPI.send(" در حال فراخوانی انواع سرویسهای موجود، لطفا صبر کنید. ", cancelButton());
                tmtnManager.GetSimTypesList();
                if (tmtnManager.resultAction.hasError)
                {
                    sendMenu(message: tmtnManager.resultAction.message);
                    break;
                }


                var inlineK = new List<InlineKeyboardButton[]>();
                var colK = new List<InlineKeyboardButton>();
                foreach (var list in tmtnManager.simtype.data)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $@"{list.simTypeShowName}",
                        CallbackData = $@"{SimpaySectionEnum.TMTNServices}:simtype:{tmtnManager.data.id}:{list.simTypeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = "لطفا سرویس مورد نظر را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);


            } while (false);
        }

        private void TMTNGetCategory()
        {
            do
            {
                tmtnManager.GetCategoriesList();
                if (tmtnManager.resultAction.hasError)
                {
                    sendMenu(message: tmtnManager.resultAction.message);
                    break;
                }


                var inlineK = new List<InlineKeyboardButton[]>();
                var colK = new List<InlineKeyboardButton>();
                foreach (var list in tmtnManager.category.data)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $@"{list.categoryShowName}",
                        CallbackData = $@"{SimpaySectionEnum.TMTNServices}:category:{tmtnManager.data.id}:{list.categoryId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = "لطفا دسته بندی مورد نظر را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);


            } while (false);

        }
        private void TMTNGetPackage()
        {
            do
            {
                tmtnManager.GetPackageList();
                if (tmtnManager.resultAction.hasError)
                {
                    sendMenu(message: tmtnManager.resultAction.message);
                    break;
                }


                var inlineK = new List<InlineKeyboardButton[]>();
                var colK = new List<InlineKeyboardButton>();
                foreach (var list in tmtnManager.package.data)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $@"{list.packageShowName}",
                        CallbackData = $@"{SimpaySectionEnum.TMTNServices}:package:{tmtnManager.data.id}:{list.packageId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = "لطفا بسته مورد نظر را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);


            } while (false);

        }

        private void TMTNBuyService()
        {

            do
            {
                tmtnManager.BuyService();
                if (tmtnManager.resultAction.hasError)
                {
                    sendMenu(message: tmtnManager.resultAction.message);
                    break;
                }


                PaymentStartProcess(tmtnManager.data.saleKey);
                //var msg = "";
                //msg += $" سرویس مورد نظر درخواستی به شرح زیر میباشد: \n \n";
                //msg += $" {tmtnManager.data.packageDescription}: \n \n \n ";
                //msg += "در صورت تایید، لطفا برای پرداخت به لینک زیر بروید. ";


                //var resultLink = SimpayCore.getPaymentLink(tmtnManager.data.saleKey);
                //if (SimpayCore.resultAction.hasError)
                //{
                //    telegramAPI.send(SimpayCore.resultAction.message);
                //    break;
                //}
                //sendPaymentMessage(resultLink, msg);

            } while (false);
        }
        private void TMTNServiceRedeem(string saleKey)
        {
            var msg = "";
            do
            {
                tmtnManager = new TMTN.Manager(chatId);
                tmtnManager.redeem(saleKey);
                if (tmtnManager.resultAction.hasError)
                {
                    sendMenu(message: tmtnManager.resultAction.message);
                    break;
                }

                msg += $" سرویس خریداری شده برای شماره موبایل {Utils.removeWhiteSpace(tmtnManager.data.mobileNumber)} به شرح زیر میباشد: \n \n";
                msg += $" {tmtnManager.data.packageDescription}: \n \n \n ";

                sendMenu(message: msg);

            } while (false);
        }
    }
}