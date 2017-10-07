using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Shared.WebService;

namespace Models
{
    public partial class TelegramMessage
    {

        #region BillPayment
        public void BillPaymentRequestInfo(string field = "", string currentValue = "", string currentId = "")
        {
            var billPayment = new BillPayment.Manager(chatId, Convert.ToInt32(currentId == "" ? "0" : currentId));
            var messagetoSend = "";
            do
            {
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "billid"; //Default field

                }
                switch (field.ToLower())
                {
                    case "billid":
                        messagetoSend = "لطفا شناسه قبض و یا تصویر بارکد روی قبض را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.BillPaymentProduct, field, currentId);
                        break;
                    case "paymentid":
                        messagetoSend = "لطفا شناسه پرداخت را وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.BillPaymentProduct, field, currentId);
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

        private void BillPaymentInfoUpdate(string field, string value, string passTicketId = "", bool forceNewMessage = false)
        {


            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var mmb = new BillPayment.Manager(chatId, Convert.ToInt32(passTicketId == "" ? "0" : passTicketId));

            do
            {

                switch (field)
                {
                    case "barcode":

                        break;
                    case "billid":
                        if (value.Trim().Length < 13)
                        {
                            hasError = true;
                            msgToShow = "قبض ورودی بنظر درست نمیاید";
                            break;
                        }
                        if (value.Trim().Length >= 18)
                        {
                            mmb.data.billId = value.Substring(0, 13);
                            mmb.data.paymentId = value.Substring(13);
                            nextStepInfoName = "";
                        }
                        else
                        {
                            mmb.data.billId = value;
                            nextStepInfoName = "paymentid";
                        }
                        mmb.setInfo();
                        break;
                    case "paymentid":
                        mmb.data.paymentId = value;
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
                    BillPaymentRequestInfo(nextStepInfoName, nextStepInfoValue, mmb.data.id.ToString());
                else
                {
                    currentAction.remove();
                    SingleBillPayment(mmb.data.id);
                    //BillPaymentInquiry(mmb.data.id);
                    //                    telegramAPI.send(msgToShow);
                }
            }




        }
        private void BillPaymentCallBackQuery(string data)
        {
            var action = data.Split(':')[1];
            var id = "";
            do
            {
                switch (action.ToLower())
                {
                    case "fastcharge":
                        id = data.Split(':')[2];
                        var savedBill = new BillPayment.Manager(chatId, Convert.ToInt32(id));
                        var newBill = new BillPayment.Manager(chatId);
                        newBill.data = new BillPayment.BillPaymentData()
                        {
                            billId = savedBill.data.billId,
                            amount = savedBill.data.amount,
                            billType = savedBill.data.billType,
                            status = TransactionStatusEnum.NotCompeleted

                        };
                        newBill.setInfo();
                        BillPaymentRequestInfo("paymentid", "", newBill.data.id.ToString());

                        //BillPaymentInfoUpdate("mobile", mobile, "", true);
                        break;
                    case "requestinfo":
                        BillPaymentRequestInfo("mobile");
                        break;


                    default:
                        break;
                }
            } while (false);

        }
        private void SingleBillPayment(long id)
        {

            do
            {
                var billPayment = new BillPayment.Manager(chatId, id);
                var paymentLink = billPayment.SingleBillPayment();
                if (billPayment.resultAction.hasError)
                {
                    sendMenu(message: billPayment.resultAction.message);
                    break;
                }

                var message = "";
                message += $" اطلاعات قبض پراختی بشرح زیر میباشد:";
                message += "\n  ";
                message += $" شماره قبض: {billPayment.data.billId}";
                message += "\n  ";
                message += $" شناسه پرداخت: {billPayment.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {billPayment.data.amount.ToString("##,##")} ریال";
                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";

                //PaymentStartProcess(billPayment.data.saleKey);

                sendPaymentMessage(paymentLink, message);


            } while (false);
        }
        private void BillPaymentInquiry(long id)
        {
            do
            {
                var billPayment = new BillPayment.Manager(chatId, id);
                billPayment.Inquiry();
                if (billPayment.resultAction.hasError)
                {
                    sendMenu(message: billPayment.resultAction.message);
                    break;
                }

                var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.BillPaymentProduct);
                var paymentLink = $@"{url}?BillID={billPayment.data.billId}&PaymentID={billPayment.data.paymentId}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $" اطلاعات قبض پراختی بشرح زیر میباشد:";
                message += "\n  ";
                message += $" شماره قبض: {billPayment.data.billId}";
                message += "\n  ";
                message += $" شناسه پرداخت: {billPayment.data.paymentId}";
                message += "\n  ";
                message += $" مبلغ: {billPayment.data.amount.ToString("##,##")} ریال";
                message += "\n  ";
                message += $" نوع قبض: {billPayment.data.billType}";
                message += "\n \n ";
                message += "لطفا جهت پرداخت صورت حساب فوق از دکمه زیر استفاده فرمایید.";

                sendPaymentMessage(paymentLink, message);

            } while (false);

        }
        private void BillPaymentFastOptions(List<BillPayment.BillPaymentLast> BillList)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < BillList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{BillList[i].billId} - {BillList[i].billType} ",
                    CallbackData = $"{SimpaySectionEnum.BillPaymentProduct}:fastcharge:{BillList[i].id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.BillPaymentProduct}:requestinfo"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر قبوضی را  که قبلا وارد نموده اید انتخاب نموده و یا قبض جدیدی وارد نمایید: ", r);

        }

        public void BillPaymentBarcode(string currentId, string fileId)
        {
            var hasError = false;
            telegramAPI.send("در حال بررسی فایل ارسالی");
            var id = String.IsNullOrEmpty(currentId) ? 0 : Convert.ToInt64(currentId);
            do
            {

                try
                {


                    telegramAPI.fileId = fileId;

                    var fileUrl = telegramAPI.getFileUrl();
                    var localFilePath = $@"{ProjectValues.DataFolder}bill{chatId}.{Utils.FileExtention(fileUrl)}";
                    if (!Utils.FolderExists(ProjectValues.DataFolder, true))
                    {
                        Log.Error($"Could not found {ProjectValues.DataFolder} path!", 0);
                        hasError = true;
                        break;
                    }
                    var isOk = Utils.DownloadRemoteImageFile(fileUrl, localFilePath);

                    if (!isOk)
                    {
                        hasError = true;
                        break;
                    }

                    var barcode = new Barcode() { minTextLenValid = 17, maxTextLenValid = 50 };
                    var code = barcode.read(localFilePath);
                    if (String.IsNullOrEmpty(code))
                    {
                        hasError = true;
                        break;
                    }
                    telegramAPI.send($"بارکد خوانده شده:{code}");
                    var billPayment = new BillPayment.Manager(chatId, id);
                    billPayment.data.billId = code.Substring(0, 13);
                    billPayment.data.paymentId = code.Substring(13);
                    billPayment.setInfo();
                    currentAction.remove();
                    SingleBillPayment(billPayment.data.id);

                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, 0);
                    hasError = true;

                }


            } while (false);
            if (hasError)
            {
                telegramAPI.send("بارکد ارسالی شناخته نشد لطفا دوباره سعی نمایید.");
                BillPaymentRequestInfo("billid", "", id.ToString());
            }

        }



        #endregion

    }
}