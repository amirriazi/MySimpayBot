using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {

        #region FixedLineBill
        public void FixedLineBillRequestInfo(string field = "", string currentValue = "", string currentId = "")
        {
            var mmb = new FixedLineBill.Manager(chatId, Convert.ToInt32(currentId == "" ? "0" : currentId));
            var messagetoSend = "";
            do
            {
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "phone"; //Default field
                    var oldList = mmb.GetFixedLineLast();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی شماره های گذشته", cancelButton());
                        FixedLineBillFastOptions(oldList);
                        currentAction.set(SimpaySectionEnum.FixedLineBill, field, currentId);
                        break;
                    }


                }
                switch (field.ToLower())
                {
                    case "phone":
                        messagetoSend = "لطفا شماره مورد نظرتان را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.FixedLineBill, field, currentId);
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

        private void FixedLineBillInfoUpdate(string field, string value, string passedId = "", bool forceNewMessage = false)
        {


            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var mmb = new FixedLineBill.Manager(chatId, Convert.ToInt32(passedId == "" ? "0" : passedId));

            do
            {

                switch (field)
                {
                    case "phone":
                        var phone = (string)value;
                        if (phone.Length < 8)
                        {
                            hasError = true;
                            msgToShow = "شماره تلفن ورودی بنظر درست نیست لطفا با دقت وارد نمایید.";
                            break;
                        }
                        mmb.data.fixedLineNumber = phone;
                        mmb.setInfo();
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
                    FixedLineBillRequestInfo(nextStepInfoName, nextStepInfoValue, mmb.data.id.ToString());
                else
                {
                    currentAction.remove();
                    FixedLineBillInquiry(mmb);
                    //                    telegramAPI.send(msgToShow);
                }
            }




        }
        private void FixedLineBillCallBackQuery(string data)
        {
            var action = data.Split(':')[1];
            do
            {
                switch (action.ToLower())
                {
                    case "fastcharge":
                        var phone = data.Split(':')[2];
                        FixedLineBillInfoUpdate("phone", phone, "", true);
                        break;
                    case "requestinfo":
                        FixedLineBillRequestInfo("phone");
                        break;


                    default:
                        break;
                }
            } while (false);

        }

        private void FixedLineBillInquiry(FixedLineBill.Manager fixedline)
        {
            do
            {
                telegramAPI.send("در حال اتصال به سامانه مخابرات....");
                fixedline.Inquiry();
                if (fixedline.resultAction.hasError)
                {
                    sendMenu(message: fixedline.resultAction.message);
                    break;
                }
                if (fixedline.data.amount == 0)
                {
                    sendMenu(message: "در این لحظه مانده بدهی شما صفر می باشد.");
                    break;
                }
                //var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.FixedLineBill);
                //var paymentLink = $@"{url}?BillID={fixedline.data.billId}&PaymentID={fixedline.data.paymentId}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $" اطلاعات قبض پراختی برای شماره تلفن ثابت {fixedline.data.fixedLineNumber} بشرح زیر میباشد:";
                message += "\n  ";
                message += $" شماره قبض: {fixedline.data.billId}";
                message += "\n  ";
                message += $" شناسه پرداخت: {fixedline.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {fixedline.data.amount}";
                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";


                //Now get the saleKey from SingleBillPayment and add a new record in the BillPayment table
                var billPayment = new BillPayment.Manager(chatId, 0);
                billPayment.data = new BillPayment.BillPaymentData
                {
                    billId = fixedline.data.billId,
                    paymentId = fixedline.data.paymentId
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
        private void FixedLineBillFastOptions(List<string> phoneList)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < phoneList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{phoneList[i]} ",
                    CallbackData = $"{SimpaySectionEnum.FixedLineBill}:fastcharge:{phoneList[i]}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.FixedLineBill}:requestinfo"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر شماره تلفن هائی که قبلا وارد نموده اید را انتخاب نموده و یا شماره جدیدی وارد نمایید: ", r);

        }

        #endregion
    }
}