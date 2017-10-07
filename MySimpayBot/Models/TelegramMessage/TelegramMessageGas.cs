using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {

        #region GasBill
        public void GasBillRequestInfo(string field = "", string currentValue = "", long currentId = 0)
        {
            var gas = new GasBill.Manager(chatId, currentId);
            var messagetoSend = "";
            do
            {
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "code"; //Default field
                    var oldList = gas.getLastChargedMobile();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی قبوض گذشته", cancelButton());
                        GasBillFastOptions(oldList);
                        currentAction.set(SimpaySectionEnum.GasBill, field, currentId.ToString());
                        break;
                    }


                }
                switch (field.ToLower())
                {
                    case "code":
                        messagetoSend = "لطفا شماره پرونده اشتراک گاز را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.GasBill, field, currentId.ToString());
                        break;
                    default:
                        break;

                }

            } while (false);
            if (!String.IsNullOrEmpty(messagetoSend))
            {
                telegramAPI.send(messagetoSend, cancelButton());
            }

        }

        private void GasBillInfoUpdate(string field, string value, string passTicketId = "", bool forceNewMessage = false)
        {


            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var gas = new GasBill.Manager(chatId, Convert.ToInt32(passTicketId == "" ? "0" : passTicketId));

            do
            {

                switch (field)
                {
                    case "code":
                        var code = value;
                        if (code.Length != 12)
                        {
                            hasError = true;
                            msgToShow = "شماره پرونده وارد شده درست نیست. لطفا دوباره وارد نمایید.";
                            break;
                        }
                        gas.data.gasParticipateCode = code;
                        gas.setInfo();
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
                    GasBillRequestInfo(nextStepInfoName, nextStepInfoValue, gas.data.id);
                else
                {
                    currentAction.remove();
                    GasBillInquiry(gas);
                }
            }




        }
        private void GasBillCallBackQuery(string data)
        {
            var action = data.Split(':')[1];
            do
            {
                switch (action.ToLower())
                {
                    case "fastcharge":
                        var code = data.Split(':')[2];
                        GasBillInfoUpdate("code", code, "", true);
                        break;
                    case "requestinfo":
                        GasBillRequestInfo("code");
                        break;


                    default:
                        break;
                }
            } while (false);

        }

        private void GasBillInquiry(GasBill.Manager gas)
        {
            do
            {

                gas.Inquiry();
                if (gas.resultAction.hasError)
                {
                    sendMenu(message: gas.resultAction.message);
                    break;
                }

                //var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.GasBill);
                //var paymentLink = $@"{url}?BillID={gas.data.billId}&PaymentID={gas.data.paymentId}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $" اطلاعات پرداختی برای قبض گاز مشترک شماره{gas.data.gasParticipateCode} بشرح زیر میباشد:";
                message += "\n  ";
                message += $" شماره قبض: {gas.data.billId}";
                message += "\n  ";
                message += $" شناسه پرداخت: {gas.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {gas.data.amount}";
                message += "\n  ";
                message += $" از تاریخ: {Utils.Shamsi(Convert.ToString(gas.data.fromDate))}";
                message += "\n  ";
                message += $" تا تاریخ: {Utils.Shamsi(Convert.ToString(gas.data.toDate))}";
                message += "\n  ";
                message += $" تاریخ سررسید: {Utils.Shamsi(Convert.ToString(gas.data.paymentDeadLineDate))}";

                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";



                //Now get the saleKey from SingleBillPayment and add a new record in the BillPayment table
                var billPayment = new BillPayment.Manager(chatId, 0);
                billPayment.data = new BillPayment.BillPaymentData
                {
                    billId = gas.data.billId,
                    paymentId = gas.data.paymentId
                };
                var paymentLink = billPayment.SingleBillPayment();
                if (billPayment.resultAction.hasError)
                {
                    sendMenu(message: billPayment.resultAction.message);
                    break;
                }
                sendPaymentMessage(paymentLink, message);



            } while (false);

        }
        private void GasBillFastOptions(List<string> mobileList)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < mobileList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{mobileList[i]} ",
                    CallbackData = $"{SimpaySectionEnum.GasBill}:fastcharge:{mobileList[i]}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.GasBill}:requestinfo"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر شماره قبوضی که قبلا وارد نموده اید را انتخاب نموده و یا شماره جدیدی وارد نمایید: ", r);

        }

        #endregion


    }
}