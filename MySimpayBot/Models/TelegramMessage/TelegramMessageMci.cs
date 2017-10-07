using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {

        #region MciMobileBill
        public void MciMobileBillRequestInfo(string field = "", string currentValue = "", string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var mmb = new MciMobileBill.Manager(chatId, Convert.ToInt32(currentId == "" ? "0" : currentId));
            var messagetoSend = "";
            do
            {
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "mobile"; //Default field
                    var oldList = mmb.getLastChargedMobile();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی موبایل های گذشته", cancelButton());
                        MciMobileBillFastOptions(oldList);
                        currentAction.set(SimpaySectionEnum.MciMobileBill, field, currentId);
                        break;
                    }


                }
                switch (field.ToLower())
                {
                    case "mobile":
                        messagetoSend = "لطفا شماره موبایل مورد نظرتان را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.MciMobileBill, field, currentId);
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

        private void MciMobileBillInfoUpdate(string field, string value, string passTicketId = "", bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var mmb = new MciMobileBill.Manager(chatId, Convert.ToInt32(passTicketId == "" ? "0" : passTicketId));

            do
            {

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
                        mmb.data.mobileNumber = mobile;
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
                    MciMobileBillRequestInfo(nextStepInfoName, nextStepInfoValue, mmb.data.id.ToString());
                else
                {
                    currentAction.remove();
                    MciMobileGetBillType(mmb.data.id, forceNewMessage);
                    //                    telegramAPI.send(msgToShow);
                }
            }




        }
        private void MciMobileGetBillType(long id, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var msgToSend = "لطفا مشخص نمایید کدام مورد زیر مد نظرتان است ";
                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                colKey.Add(new InlineKeyboardButton
                {
                    Text = " پایان دوره",
                    CallbackData = $"{SimpaySectionEnum.MciMobileBill}:final:{id}"
                });
                colKey.Add(new InlineKeyboardButton
                {
                    Text = " میان دوره",
                    CallbackData = $"{SimpaySectionEnum.MciMobileBill}:middle:{id}"
                });
                inlineK.Add(colKey.ToArray());
                var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                r.InlineKeyboard = inlineK.ToArray();

                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, r);
                }
                else
                {
                    telegramAPI.send(msgToSend, r);
                }


            } while (false);

        }
        private void MciMobileBillCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = "";
            do
            {
                switch (action.ToLower())
                {
                    case "final":
                        id = data.Split(':')[2];
                        MciMobilBillInquiry(Convert.ToInt32(id), true);
                        break;
                    case "middle":
                        id = data.Split(':')[2];
                        MciMobilBillInquiry(Convert.ToInt32(id), false);
                        break;
                    case "fastcharge":
                        var mobile = data.Split(':')[2];
                        MciMobileBillInfoUpdate("mobile", mobile, "", true);
                        break;
                    case "requestinfo":
                        MciMobileBillRequestInfo("mobile");
                        break;


                    default:
                        break;
                }
            } while (false);

        }

        private void MciMobilBillInquiry(long id, bool final)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var mmb = new MciMobileBill.Manager(chatId, id);
                mmb.data.final = final;
                mmb.setInfo();
                mmb.Inquiry();
                if (mmb.resultAction.hasError)
                {
                    sendMenu(message: mmb.resultAction.message);
                    break;
                }

                if (mmb.data.amount == 0)
                {
                    sendMenu(message: "هم اکنون مبلغ هزینه شما صفر میباشد.");
                    break;

                }



                var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.MciMobileBill);
                //var paymentLink = $@"{url}?BillID={mmb.data.billId}&PaymentID={mmb.data.paymentId}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $" اطلاعات قبض پراختی برای موبایل {mmb.data.mobileNumber.NationalNumber.Replace(" ", "")} بشرح زیر میباشد:";
                message += "\n  ";
                //message += $" شماره قبض: {mmb.data.billId}";
                message += "\n  ";
                //message += $" شناسه پرداخت: {mmb.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {mmb.data.amount.ToString("#,##")}";
                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";



                //Now get the saleKey from SingleBillPayment and add a new record in the BillPayment table
                var billPayment = new BillPayment.Manager(chatId, 0);
                billPayment.data = new BillPayment.BillPaymentData
                {
                    billId = mmb.data.billId,
                    paymentId = mmb.data.paymentId
                };
                var paymentLink = billPayment.SingleBillPayment();
                if (billPayment.resultAction.hasError)
                {
                    sendMenu(message: billPayment.resultAction.message);
                    break;
                }


                //PaymentStartProcess(billPayment.data.saleKey);


                sendPaymentMessage(paymentLink, message);
            } while (false);

        }
        private void MciMobileBillFastOptions(List<string> mobileList)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < mobileList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{mobileList[i]} ",
                    CallbackData = $"{SimpaySectionEnum.MciMobileBill}:fastcharge:{mobileList[i]}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.MciMobileBill}:requestinfo"
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