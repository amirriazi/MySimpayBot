using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;


namespace Models
{
    public partial class TelegramMessage
    {
        #region AutoCharge

        private void AutoChargeCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = data.Split(':')[2];
            do
            {

                switch (action.ToLower())
                {
                    case "autochargerequestinfo":
                        AutoChargeRequestInfo("mobile");
                        break;
                    case "fastcharge":
                        AutoChargeFastCharge(id);
                        break;
                    case "last":
                        AutoChargeLastPurchase(id);
                        break;
                    case "amount":
                        var amount = data.Split(':')[3];
                        AutoChargeInfoUpdate(action, amount, id);
                        break;
                    case "chargetypeid":
                        var chargetypeid = data.Split(':')[3];
                        AutoChargeInfoUpdate(action, chargetypeid, id);

                        break;
                    default:
                        break;
                }
            } while (false);

        }


        private void AutoChargeRequestInfo(string field = "", string currentValue = "", string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var autoCharge = new AutoCharge.Manager(chatId);
            var messagetoSend = "";
            do
            {
                if (!string.IsNullOrEmpty(currentId))
                {
                    //autoCharge.transaction = new AutoCharge.Transaction() { id = Convert.ToInt32(currentId) };
                    autoCharge.getTransaction(Convert.ToInt32(currentId));

                }

                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    var oldList = autoCharge.getLastChargedMobile();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی موبایل های گذشته", cancelButton());
                        AutoChargeFastChargeOptions(oldList);
                        break;
                    }
                    field = "Mobile";
                }
                //get phone number
                switch (field.ToLower())
                {
                    case "mobile":
                        messagetoSend = "لطفا شماره موبایل مورد نظرتان را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.AutoCharge, field, currentId);
                        break;
                    case "chargetypeid":
                        AutoChargeTypeList(autoCharge);
                        break;

                    //messagetoSend += "لطفا اپراتور مورد نظر را از فهرست زیر انتخاب نمایید:" + "\n\n";
                    //var operatorList = autoCharge.GetAutoChargeOperatorList();
                    //for (var i = 0; i < operatorList.Count; i++)
                    //{
                    //    //messagetoSend += $"<a href='{list[i].chargeTypeId}' > {list[i].chargeName} {list[i].operatorName} </a> " + "\n";
                    //    messagetoSend += $@"/{operatorList[i].chargeTypeId} {operatorList[i].chargeName} {operatorList[i].operatorName}" + "\n";
                    //}
                    //messagetoSend += "\n";
                    //messagetoSend += " - ";
                    //currentAction.set(SimpaySectionEnum.AutoCharge, field, currentId);
                    //break;
                    case "amount":
                        AutoChargeAmountList(autoCharge);
                        break;
                    //messagetoSend += "لطفا مبلغ شارژ مورد نظر را از فهرست زیر انتخاب نمایید:" + "\n\n";
                    //var chargeList = autoCharge.GetAutoChargeList(autoCharge.transaction.chargeTypeId);
                    //for (var i = 0; i < chargeList.Count; i++)
                    //{
                    //    messagetoSend += $@"/{chargeList[i].amount} ریالی" + "\n";
                    //}
                    //messagetoSend += "\n";
                    //messagetoSend += " - ";
                    //currentAction.set(SimpaySectionEnum.AutoCharge, field, currentId);
                    //break;

                    default:
                        break;
                }

            } while (false);

            if (!String.IsNullOrEmpty(messagetoSend))
            {

                telegramAPI.send(messagetoSend, cancelButton());
            }

        }

        private void AutoChargeAmountList(AutoCharge.Manager autoCharge)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var chargeList = autoCharge.GetAutoChargeList(autoCharge.transaction.chargeTypeId);
            for (var i = 0; i < chargeList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $"{chargeList[i].amount} ریالی ",
                    CallbackData = $"{SimpaySectionEnum.AutoCharge}:amount:{autoCharge.transaction.id}:{chargeList[i].amount}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("لطفا مبلغ شارژ مورد نظر را از فهرست زیر انتخاب نمایید:", r);

        }

        private void AutoChargeTypeList(AutoCharge.Manager autoCharge)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var operatorList = autoCharge.GetAutoChargeOperatorList();
            for (var i = 0; i < operatorList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $"{operatorList[i].chargeName} {operatorList[i].operatorName}",
                    CallbackData = $"{SimpaySectionEnum.AutoCharge}:chargetypeid:{autoCharge.transaction.id}:{operatorList[i].chargeTypeId}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("لطفا اپراتور مورد نظر را از فهرست زیر انتخاب نمایید:", r);

        }
        private void AutoChargeInfoUpdate(string field, string value, string transID = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var autoCharge = new AutoCharge.Manager(chatId);

            do
            {

                int id = 0;
                if (Utils.isInteger(transID))
                {
                    id = Convert.ToInt32(transID);
                    autoCharge.getTransaction(id);
                }
                else
                {
                    autoCharge.transaction = new AutoCharge.Transaction
                    {
                        id = 0,
                        status = TransactionStatusEnum.NotCompeleted,
                        amount = 0
                    };
                }

                switch (field)
                {
                    case "mobile":
                        var mobile = new Mobile() { Number = value };
                        if (!mobile.IsNumberContentValid())
                        {
                            hasError = true;
                            msgToShow = "شماره موبایل ورودی بنظر درست نیست لطفا با دقت وارد نمایید.";
                            break;
                        }
                        autoCharge.transaction.mobileNumber = mobile;
                        autoCharge.setTransaction(id);
                        nextStepInfoName = "chargetypeid";
                        break;
                    case "chargetypeid":

                        autoCharge.transaction.chargeTypeId = Convert.ToInt32(value);
                        autoCharge.setTransaction(id);
                        nextStepInfoName = "amount";
                        break;
                    case "amount":
                        autoCharge.transaction.amount = Convert.ToInt32(value);
                        autoCharge.setTransaction(id);
                        nextStepInfoName = "";
                        break;

                    default:
                        break;
                }

            } while (false);


            if (hasError)
            {
                telegramAPI.send(msgToShow, cancelButton());
            }
            else
            {
                if (nextStepInfoName != "")
                    AutoChargeRequestInfo(nextStepInfoName, nextStepInfoValue, autoCharge.transaction.id.ToString());
                else
                {
                    currentAction.remove();
                    AutoChargeGetLink(autoCharge.transaction.id);
                    //                    telegramAPI.send(msgToShow);
                }
            }




        }

        private void AutoChargeFastChargeOptions(List<AutoCharge.LastChargedMobiles> list)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"تکرار آخرین خرید",
                CallbackData = $"{SimpaySectionEnum.AutoCharge}:last:{list[0].id}"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            for (var i = 0; i < list.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{list[i].mobileNumber} ",
                    CallbackData = $"{SimpaySectionEnum.AutoCharge}:fastcharge:{list[i].id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }

            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.AutoCharge}:autochargerequestinfo:0"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر شماره تلفن های مربوطه را انتخاب نموده و یا شماره جدیدی وارد نمایید: ", r);

        }
        private void AutoChargeFastCharge(string id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var oldAutoCharge = new AutoCharge.Manager(chatId);
                oldAutoCharge.getTransaction(Convert.ToInt32(id));

                var newAutoCharge = new AutoCharge.Manager(chatId);
                newAutoCharge.transaction = new AutoCharge.Transaction()
                {
                    mobileNumber = oldAutoCharge.transaction.mobileNumber,
                    chargeTypeId = oldAutoCharge.transaction.chargeTypeId,
                    amount = 0//oldAutoCharge.transaction.amount
                };

                newAutoCharge.setTransaction();



                AutoChargeRequestInfo("amount", "0", id);
                //AutoChargeGetLink(newAutoCharge.transaction.id);



            } while (false);
        }
        private void AutoChargeLastPurchase(string id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var oldAutoCharge = new AutoCharge.Manager(chatId);
                oldAutoCharge.getTransaction(Convert.ToInt32(id));

                var newAutoCharge = new AutoCharge.Manager(chatId);
                newAutoCharge.transaction = new AutoCharge.Transaction()
                {
                    mobileNumber = oldAutoCharge.transaction.mobileNumber,
                    chargeTypeId = oldAutoCharge.transaction.chargeTypeId,
                    amount = oldAutoCharge.transaction.amount
                };
                newAutoCharge.setTransaction();
                var msg = "";
                msg += $"شارژ سیم کارت به شماره: {Environment.NewLine}";
                msg += $"{oldAutoCharge.transaction.mobileNumber.InternationalNumber} {Environment.NewLine}";
                msg += $"و مبلغ {Environment.NewLine}";
                msg += $"{oldAutoCharge.transaction.amount.ToString("#,##")} ریال{Environment.NewLine}";
                msg += $"{Environment.NewLine} {Environment.NewLine}";
                msg += $"لطفا چند لحظه صبر نمایید.{Environment.NewLine} ";
                telegramAPI.send(msg);

                AutoChargeGetLink(newAutoCharge.transaction.id);



            } while (false);

        }

        private void AutoChargeGetLink(int id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var autoCharge = new AutoCharge.Manager(chatId);

                autoCharge.getTransaction(id);

                var resultSaleKey = autoCharge.getSaleKey();
                if (autoCharge.actionResult.hasError)
                {
                    sendMenu(message: $"{autoCharge.actionResult.message}");
                    break;
                }

                autoCharge.transaction.saleKey = resultSaleKey;
                autoCharge.setTransaction(id);


                PaymentStartProcess(autoCharge.transaction.saleKey);

                //var resultLink = SimpayCore.getPaymentLink(autoCharge.transaction.saleKey);

                //sendPaymentMessage(resultLink, $"با زدن کلید زیر به صفحه بانک منتقل میشوید. لطفا دفت فرمایید که مبلغ پرداختی باید {autoCharge.transaction.amount} باشد.");

            } while (false);



        }


        private List<InlineKeyboardButton[]> AutoChargeGetOperatorList()
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            do
            {
                var autoCharge = new AutoCharge.Manager(chatId);
                var list = autoCharge.GetAutoChargeOperatorList();
                for (var i = 0; i < list.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = $@"{list[i].chargeName} {list[i].operatorName} ",
                        CallbackData = $"{SimpaySectionEnum.AutoCharge}:chargetypeid:{list[i].chargeTypeId}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }

            } while (false);
            return inlineK;
        }


        #endregion //AutoCharge

    }
}
