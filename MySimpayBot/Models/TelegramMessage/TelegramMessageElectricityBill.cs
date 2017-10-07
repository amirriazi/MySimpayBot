using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {

        #region ElectricityBill
        public void ElectricityBillRequestInfo(string field = "", string currentValue = "", long currentId = 0)
        {
            var el = new ElectricityBill.Manager(chatId, currentId);
            var messagetoSend = "";
            do
            {
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "code"; //Default field
                    var oldList = el.getLastBill();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی قبوض گذشته", cancelButton());
                        ElectricityBillFastOptions(oldList);
                        currentAction.set(SimpaySectionEnum.ElectricityBill, field, currentId.ToString());
                        break;
                    }


                }
                switch (field.ToLower())
                {
                    case "code":
                        messagetoSend = "لطفا شماره قبض برق را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.ElectricityBill, field, currentId.ToString());
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

        private void ElectricityBillInfoUpdate(string field, string value, string passTicketId = "", bool forceNewMessage = false)
        {


            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var ele = new ElectricityBill.Manager(chatId, Convert.ToInt32(passTicketId == "" ? "0" : passTicketId));

            do
            {

                switch (field)
                {
                    case "code":
                        var code = value;
                        if (code.Length <= 12)
                        {
                            hasError = true;
                            msgToShow = "شماره قبض وارد شده درست نیست. لطفا دوباره وارد نمایید.";
                            break;
                        }
                        ele.data.electricityBillID = code;
                        ele.setInfo();
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
                    ElectricityBillRequestInfo(nextStepInfoName, nextStepInfoValue, ele.data.id);
                else
                {
                    currentAction.remove();
                    ElectricityBillInquiry(ele);
                }
            }




        }
        private void ElectricityBillCallBackQuery(string data)
        {
            var action = data.Split(':')[1];
            do
            {
                switch (action.ToLower())
                {
                    case "fastcharge":
                        var code = data.Split(':')[2];
                        ElectricityBillInfoUpdate("code", code, "", true);
                        break;
                    case "requestinfo":
                        ElectricityBillRequestInfo("code");
                        break;


                    default:
                        break;
                }
            } while (false);

        }

        private void ElectricityBillInquiry(ElectricityBill.Manager gas)
        {
            do
            {

                gas.Inquiry();
                if (gas.resultAction.hasError)
                {
                    sendMenu(message: gas.resultAction.message);
                    break;
                }

                var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.ElectricityBill);
                var paymentLink = $@"{url}?BillID={gas.data.billId}&PaymentID={gas.data.paymentId}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $" اطلاعات پرداختی برای قبض برق شماره {gas.data.electricityBillID} بشرح زیر میباشد:";
                message += "\n  ";
                message += $" صاحب پرونده: {gas.data.fullName}";
                message += "\n  ";
                message += $" شماره قبض: {gas.data.billId}";
                message += "\n  ";
                message += $" شناسه پرداخت: {gas.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {gas.data.amount}";
                message += "\n  ";
                message += $" بدهی قبلی: {gas.data.debt}";
                message += "\n  ";

                message += $" از تاریخ: {Utils.Shamsi(Convert.ToString(gas.data.fromDate))}";
                message += "\n  ";
                message += $" تا تاریخ: {Utils.Shamsi(Convert.ToString(gas.data.toDate))}";
                message += "\n  ";
                message += $" تاریخ سررسید: {Utils.Shamsi(Convert.ToString(gas.data.paymentDeadLineDate))}";

                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";

                sendPaymentMessage(paymentLink, message);

            } while (false);

        }
        private void ElectricityBillFastOptions(List<string> mobileList)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < mobileList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{mobileList[i]} ",
                    CallbackData = $"{SimpaySectionEnum.ElectricityBill}:fastcharge:{mobileList[i]}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.ElectricityBill}:requestinfo"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر شماره موبایل هائی که قبلا وارد نموده اید را انتخاب نموده و یا شماره جدیدی وارد نمایید: ", r);

        }

        #endregion


    }
}