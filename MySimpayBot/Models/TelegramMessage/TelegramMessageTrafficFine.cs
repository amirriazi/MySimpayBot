using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Models
{
    public partial class TelegramMessage
    {

        #region TrafficFine

        private void TrafficFineCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];

            do
            {
                int row = 0;
                long ticketId = 0;

                switch (action.ToLower())
                {
                    case "pgtkt":
                        ticketId = Convert.ToInt64(data.Split(':')[2]);
                        row = Convert.ToInt32(data.Split(':')[3]);
                        TrafficFineShowTicketDetails(ticketId, row);
                        break;
                    case "select":
                        ticketId = Convert.ToInt64(data.Split(':')[2]);
                        row = Convert.ToInt32(data.Split(':')[3]);
                        TrafficFineSelectionOfTicket(ticketId, row, true);
                        break;
                    case "unselect":
                        ticketId = Convert.ToInt64(data.Split(':')[2]);
                        row = Convert.ToInt32(data.Split(':')[3]);
                        TrafficFineSelectionOfTicket(ticketId, row, false);
                        break;
                    case "payselected":
                        ticketId = Convert.ToInt64(data.Split(':')[2]);
                        TrafficFinePaySelectedTickets(ticketId);
                        break;
                    case "payall":
                        ticketId = Convert.ToInt64(data.Split(':')[2]);
                        TrafficFinePayAllTickets(ticketId);
                        break;
                    case "fastcharge":
                        var barCode = data.Split(':')[2];
                        TrafficFineInfoUpdate("barcode", barCode, "", true);
                        break;
                    case "trafficfinesrequestinfo":
                        TrafficFineRequestInfo("barcode");
                        break;


                    default:
                        break;
                }
            } while (false);



        }

        private void TrafficFineRequestInfo(string field = "", string currentValue = "", string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var traffic = new TrafficFine.Manager(chatId);
            var messagetoSend = "";
            do
            {
                if (!string.IsNullOrEmpty(currentId))
                {
                    //autoCharge.transaction = new AutoCharge.Transaction() { id = Convert.ToInt32(currentId) };
                    traffic.data = new TrafficFine.TrafficFineData.header()
                    {
                        ticketId = Convert.ToInt32(currentId),
                    };
                    traffic.getHeader();

                }
                //first look at history
                if (String.IsNullOrEmpty(field))
                {
                    field = "barcode"; //Default field
                    var oldList = traffic.getLastBarcodes();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی بارکدهای گذشته", cancelButton());
                        TrafficFineFastOptions(oldList);
                        currentAction.set(SimpaySectionEnum.TrafficFinesProduct, field, currentId);
                        break;
                    }


                }



                switch (field.ToLower())
                {
                    case "barcode":
                        TrafficFineAskForBarcode(currentId);
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

        private void TrafficFineAskForBarcode(string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var fileToSend = new FileToSend
            {
                //Url = new Uri("http://simpay.ir/images/carIdentity.jpg"),
                FileId = "AgADAQADqqcxG4DXMEwKKbDTk-E47AGH3i8ABHaC15qBNz4DssIBAAEC",
                Filename = "Card.png",

            };
            var caption = "لطفا شماره بارکد مندرج برروی کارت خودرو را وارد نمایید.";

            telegramAPI.fileToSend = fileToSend;
            telegramAPI.caption = caption;
            telegramAPI.send(MessageType.PhotoMessage);
            currentAction.set(SimpaySectionEnum.TrafficFinesProduct, "barcode", currentId);
        }
        private void TrafficFineInfoUpdate(string field, string value, string passTicketId = "", bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var hasError = false;
            var msgToShow = "Done!";
            var nextStepInfoName = "";
            var nextStepInfoValue = "";
            var traffic = new TrafficFine.Manager(chatId);

            telegramAPI.sendTypingStatus();

            do
            {

                if (Utils.isInteger(passTicketId))
                {
                    traffic.data = new TrafficFine.TrafficFineData.header
                    {
                        ticketId = Convert.ToInt32(passTicketId),
                    };
                    traffic.getHeader();
                }
                else
                {
                    traffic.data = new TrafficFine.TrafficFineData.header
                    {
                        ticketId = 0,
                        status = TransactionStatusEnum.NotCompeleted,
                    };

                }

                switch (field)
                {
                    case "barcode":
                        traffic.data.barCode = value;
                        traffic.setInfo();
                        Log.Warn($"TrafficFineInfoUpdate-SetInfo: data={Utils.ConvertClassToJson(traffic.data)}", 0);
                        nextStepInfoName = "";
                        break;
                    case "captcha":
                        traffic.data.captchaText = value;
                        traffic.setInfo();
                        Log.Warn($"TrafficFineInfoUpdate-SetInfo: data={Utils.ConvertClassToJson(traffic.data)}", 0);
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
                    TrafficFineRequestInfo(nextStepInfoName, nextStepInfoValue, traffic.data.ticketId.ToString());
                else
                {
                    //resetCurrentState();
                    TrafficGetTrafficFinesInquiry(traffic.data.ticketId, forceNewMessage);
                    //TrafficFineGetTikects(traffic.data.ticketId, forceNewMessage);
                    //                    telegramAPI.send(msgToShow);
                }
            }




        }

        private void TrafficFineFastOptions(List<string> barCodeList)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            for (var i = 0; i < barCodeList.Count; i++)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{barCodeList[i]} ",
                    CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:fastcharge:{barCodeList[i]}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"شماره جدید",
                CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:trafficfinesrequestinfo"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر شماره بارکدهای مربوطه را انتخاب نموده و یا شماره جدیدی وارد نمایید: ", r);

        }

        private void TrafficGetTrafficFinesInquiry(long ticketId = 0, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
            currentAction.remove();
            traffic.getHeader();
            do
            {
                if (String.IsNullOrEmpty(traffic.data.saleKey))
                {
                    traffic.getTrafficFinesInquiry();
                    if (traffic.resultAction.hasError)
                    {
                        sendMenu(message: traffic.resultAction.message);
                        break;
                    }
                }
                if (traffic.data.twoPhaseInquiry)
                {
                    if (string.IsNullOrEmpty(traffic.data.captchaText))
                    {
                        traffic.getCaptcha();
                        if (traffic.resultAction.hasError)
                        {
                            sendMenu(message: traffic.resultAction.message);
                            break;
                        }

                        telegramAPI.fileToSend = new FileToSend
                        {
                            Content = new System.IO.MemoryStream(Convert.FromBase64String(traffic.data.captchaBase64)),
                            Filename = traffic.data.captchaBase64
                        };
                        telegramAPI.caption = "لطفا نوشتار داخل تصویر را وارد نمایید.";
                        telegramAPI.send(MessageType.PhotoMessage);
                        currentAction.set(SimpaySectionEnum.TrafficFinesProduct, "captcha", traffic.data.ticketId.ToString());
                        break;
                    }
                    else
                    {
                        traffic.ResolveCaptcha();
                        if (traffic.resultAction.hasError)
                        {
                            telegramAPI.send(traffic.resultAction.message);
                            TrafficGetTrafficFinesInquiry(ticketId, forceNewMessage);
                            break;
                        }
                        currentAction.remove();

                    }

                }
                TrafficFineGetTikects(ticketId, forceNewMessage);
            } while (false);
        }
        private void TrafficFineGetTikects(long ticketId = 0, bool forceNewMessage = false)
        {

            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                telegramAPI.sendTypingStatus();

                var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
                traffic.getHeader();

                traffic.GetTicketDetail();

                if (traffic.resultAction.hasError)
                {
                    sendMenu(message: traffic.resultAction.message);
                    break;
                }

                var sendMessage = "";

                sendMessage += $"در مجموع {traffic.data.count} جریمه به مبلغ کل {traffic.data.amount.ToString("##,#")} ریال برای شما به شرح زیر ثبت شده است";

                telegramAPI.send(sendMessage, cancelButton());

                TrafficFineShowTicketDetails(traffic.data.ticketId, 1, forceNewMessage);
                //sendMenu();
            } while (false);

        }
        public void TrafficFineShowTicketDetails(long ticketId, int row = 0, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msgToSend = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
            traffic.getHeader();
            traffic.getDetails(row);
            var list = traffic.data.details;
            for (var i = 0; i < list.Count; i++)
            {
                var selectionMsg = list[i].selected ? " (انتخاب شده) " : "";
                msgToSend += $" ردیف {list[i].row}    {selectionMsg}";
                msgToSend += "\n ";
                msgToSend += $"تاریخ: {Utils.Shamsi(list[i].DateTime.ToString("MM/dd/yyyy  H:mm"))} ";
                msgToSend += "\n ";
                msgToSend += $" مبلغ جریمه: {list[i].Amount.ToString("##,#")} ریال ";
                msgToSend += "\n ";
                msgToSend += $" شماره پلاک خودرو: {list[i].LicensePlate} ";
                msgToSend += "\n ";
                msgToSend += $" توسط {list[i].Description} ثبت شده در {list[i].Location} ";
                msgToSend += "\n ";
                msgToSend += $" بدلیل {list[i].Type} ";
                msgToSend += "\n ";
                msgToSend += "\n -";
            }
            if (traffic.data.count > 1)
            {
                if (list[0].selected)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = " حذف این جریمه",
                        CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:unselect:{traffic.data.ticketId}:{list[0].row}"
                    });
                }
                else
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = "انتخاب این جریمه",
                        CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:select:{traffic.data.ticketId}:{list[0].row}"
                    });

                }

                inlineK.Add(colKey.ToArray());
                colKey.Clear();
                if (traffic.data.selectedCount > 0)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = "پرداخت جریمه های انتخابی",
                        CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:payselected:{traffic.data.ticketId}"
                    });
                }
                else
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = "پرداخت کل جریمه ها",
                        CallbackData = $"{SimpaySectionEnum.TrafficFinesProduct}:payall:{traffic.data.ticketId}"
                    });

                }
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

            }
            inlineK.Add(colKey.ToArray());
            var callBackExtra = $"{SimpaySectionEnum.TrafficFinesProduct}:pgtkt:{traffic.data.ticketId}";
            var paging = paginButtons(6, row, traffic.data.count, callBackExtra);

            if (paging != null)
                inlineK.Add(paging);


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

        }

        public void TrafficFineSelectionOfTicket(long ticketId, int row, bool selection)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
            traffic.RowSelection(ticketId, row, selection);
            TrafficFineShowTicketDetails(ticketId, row);

        }

        public void TrafficFinePaySelectedTickets(long ticketId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
                traffic.getHeader();
                if (!traffic.SelectTickets())
                {
                    break;
                }

                //PaymentStartProcess(traffic.data.saleKey);


                var url = SimpayCore.getRedirectPaymentUrl(SimpaySectionEnum.TrafficFinesProduct);
                var paymentLink = $@"{url}?SaleKey={traffic.data.saleKey}&SessionID={SimpayCore.getSessionId()}";
                var message = "";
                message += $"تعداد {traffic.data.selectedCount} جریمه به مبلغ {traffic.data.selectedAmount} ریال، انتخابی توسط شما آماده پرداخت است. ";
                message += "\n \n ";
                message += "لطفا جهت انتقال به صفحه بانک از دکمه زیر استفاده فرمایید.";

                sendPaymentMessage(paymentLink, message);


            } while (false);

        }

        public void TrafficFinePayAllTickets(long ticketId)
        {
            var traffic = new TrafficFine.Manager(chatId) { data = new TrafficFine.TrafficFineData.header { ticketId = ticketId } };
            traffic.getHeader();
            traffic.getDetails();
            foreach (var row in traffic.data.details)
            {
                //row.selected = true;
                //TrafficFineSelectionOfTicket(ticketId, row.row, true);
                traffic.RowSelection(ticketId, row.row, true);
            }

            TrafficFinePaySelectedTickets(ticketId);

        }

        #endregion

    }
}