using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {

        #region BusTicket
        private void BusTicketCallBack(string data, int callbackQueryId = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = Convert.ToInt32(data.Split(':')[2]);

            do
            {
                switch (action.ToLower())
                {
                    case "pgs":
                        var dayTimeIdPage = Convert.ToInt32(Convert.ToInt32(data.Split(':')[3]));
                        var servicePage = Convert.ToInt32(Convert.ToInt32(data.Split(':')[4]));
                        BusTicketShowServiceDayTimeTotal(id, dayTimeIdPage, servicePage);
                        break;
                    case "sstate":
                        var ssCode = Convert.ToInt32(data.Split(':')[3]);
                        var ssName = (new BusTicket.State(true)).getShowName(ssCode);
                        BusTicketUpdateInfo(action, new BusTicket.CodeAndName() { code = ssCode, name = ssName }, id, callbackQueryId);
                        break;
                    case "sterminal":
                        var stCode = Convert.ToInt32(data.Split(':')[3]);
                        var stName = (new BusTicket.Terminal(true)).getShowName(stCode);
                        BusTicketUpdateInfo(action, new BusTicket.CodeAndName() { code = stCode, name = stName }, id, callbackQueryId);
                        break;
                    case "dstate":
                        var dsCode = Convert.ToInt32(data.Split(':')[3]);
                        var dsName = (new BusTicket.State(true)).getShowName(dsCode);
                        BusTicketUpdateInfo(action, new BusTicket.CodeAndName() { code = dsCode, name = dsName }, id, callbackQueryId);
                        break;
                    case "dterminal":
                        var dtCode = Convert.ToInt32(data.Split(':')[3]);
                        var dtName = (new BusTicket.Terminal(true)).getShowName(dtCode);
                        BusTicketUpdateInfo(action, new BusTicket.CodeAndName() { code = dtCode, name = dtName }, id, callbackQueryId);
                        break;
                    case "daytime":
                        var dayTimeId = Convert.ToInt32(data.Split(':')[3]);
                        BusTicketShowServiceDayTimeTotal(id, dayTimeId, 1, true);
                        break;

                    case "seat"://XXX
                        //{ SimpaySectionEnum.BusTicket}:select: { id}:{ serviceData.row}
                        var serviceRow = Convert.ToInt32(data.Split(':')[3]);
                        BusTicketGetSeatMapOfService(id, serviceRow);
                        break;
                    case "selectseat"://$"{SimpaySectionEnum.BusTicket}:selectseat:{id}:{seatInfo.seats[i].seatNumber}"
                        var seatMapIndexSelected = Convert.ToInt32(data.Split(':')[3]);
                        BusTicketSeatSelection(id, seatMapIndexSelected, true);
                        break;
                    case "unselectseat"://$"{SimpaySectionEnum.BusTicket}:unselectseat:{id}:{seatInfo.seats[i].seatNumber}"
                        var seatMapIndexUnSelected = Convert.ToInt32(data.Split(':')[3]);
                        BusTicketSeatSelection(id, seatMapIndexUnSelected, false);
                        break;
                    case "backtoseat":
                        BusTicketShowSeats(id);
                        break;
                    case "seatdone":
                        BusTicketGenderSelectionMessage(id);
                        break;
                    case "selectgender"://{SimpaySectionEnum.BusTicket}:selectgender:{id}:{mapIndex}:{1}
                        var seatMapIndex = Convert.ToInt32(data.Split(':')[3]);
                        var genderIndex = Convert.ToInt32(data.Split(':')[4]);
                        BusTicketSelectGender(id, seatMapIndex, genderIndex);
                        break;
                    case "getname":
                        BusTicketGetNameMessage(id);
                        break;

                    case "backtojob":
                        var nextJob = data.Split(':')[3];
                        BusTicketProcess(nextJob, id);
                        break;
                    case "fastbuy":
                        BusTicketFastBuy(id);
                        break;

                    default:
                        telegramAPI.answerCallBack("دکمه درستی زده نشده است");
                        break;
                }
            } while (false);
        }

        private void BusTicketCurrenAction(string field, string currentTicketId, string value)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var id = Convert.ToInt32(currentTicketId);
            BusTicketUpdateInfo(field, value, id);
        }

        private void BusTicketProcess(string action = "", long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                if (String.IsNullOrEmpty(action))
                {
                    telegramAPI.send(" در حال جمع آوری اطلاعات مربوطه، لطفا صبر کنید. ", cancelButton());

                    var oldList = (new BusTicket.Manager(chatId)).getLastPath();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی مسیرهای قبلی شما", cancelButton());
                        BusTerminalFastOptions(oldList);
                        break;
                    }


                    action = "sstate";
                }
                switch (action.ToLower())
                {
                    case "sstate":
                        BusTicketShowSourceState(id);
                        break;
                    case "sterminal":
                        BusTicketShowSourceTerminal(id);
                        break;
                    case "dstate":
                        BusTicketShowDestinationState(id);
                        break;
                    case "dterminal":
                        BusTicketShowDestinationTerminal(id);
                        break;
                    case "datetime":
                        BusTicketShowDateTime(extraInfo: $"{SimpaySectionEnum.BusTicket}|datetime|{id}", forceNewWindow: true);
                        break;
                    case "service":
                        BusTicketShowServiceDayTimeSummary(id);
                        break;
                    case "done":
                        BusTicketPaymentMessage(id);
                        break;

                    default:
                        telegramAPI.send($" {action} not recognized!");
                        break;
                }

            } while (false);
        }

        private void BusTicketShowSourceState(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var bus = new BusTicket.Manager(chatId, id);
            do
            {

                var list = bus.GetListOfSourceState();
                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }

                for (var i = 0; i < list.Count; i++)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].stateShowName}",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:sstate:{id}:{list[i].stateCode}",
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = "لطفا از فهرست زیر استان مبدا را انتخاب کنید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);



        }
        private void BusTicketShowSourceTerminal(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var bus = new BusTicket.Manager(chatId, id);
            do
            {

                var list = bus.GetListOfSourceTerminal();
                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }
                if (list.Count == 1)
                {
                    BusTicketCallBack($"{SimpaySectionEnum.BusTicket}:sterminal:{id}:{list[0].terminalCode}");
                    break;
                }

                for (var i = 0; i < list.Count; i++)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].terminalShowName}",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:sterminal:{id}:{list[i].terminalCode}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = " دراین مرحله لطفا از فهرست زیر شهر / ترمینال مبدا را انتخاب کنید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);



        }

        private void BusTicketShowDestinationState(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var bus = new BusTicket.Manager(chatId, id);
            do
            {

                var list = bus.GetListOfDestinationState();
                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }

                for (var i = 0; i < list.Count; i++)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].stateShowName}",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:dstate:{id}:{list[i].stateCode}",
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = "لطفا از فهرست زیر استان مقصد را انتخاب کنید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);



        }

        private void BusTicketShowDestinationTerminal(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var bus = new BusTicket.Manager(chatId, id);
            do
            {

                var list = bus.GetListOfDestinationTerminal();
                if (bus.resultAction.hasError)
                {
                    // there is no service
                    BusTicketSendNoServiceError(id, bus.resultAction.message);

                    break;
                }

                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }
                if (list.Count == 1)
                {
                    BusTicketCallBack($"{SimpaySectionEnum.BusTicket}:dterminal:{id}:{list[0].terminalCode}");
                    break;
                }
                for (var i = 0; i < list.Count; i++)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].terminalShowName}",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:dterminal:{id}:{list[i].terminalCode}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                var msgToSend = " دراین مرحله لطفا از فهرست زیر شهر / ترمینال مقصد را انتخاب کنید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);



        }



        private void BusTicketUpdateInfo(string field, dynamic value, long id = 0, int callbackQueryId = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var messageToSend = "";
            var nextStep = "";
            var bus = new BusTicket.Manager(chatId, id);
            do
            {
                switch (field.ToLower())
                {
                    case "sstate":
                        var sourceState = (BusTicket.CodeAndName)value;
                        bus.data.sourceStateCode = sourceState.code;
                        bus.data.sourceStateShowName = sourceState.name;
                        bus.setInfo();
                        messageToSend = $"استان مبدا انتخابی: {bus.data.sourceStateShowName}";
                        nextStep = "sterminal";
                        break;
                    case "sterminal":
                        var sourceTerminal = (BusTicket.CodeAndName)value;
                        bus.data.sourceTerminalCode = sourceTerminal.code;
                        bus.data.sourceTerminalShowName = sourceTerminal.name;
                        bus.setInfo();
                        messageToSend = $"شهر / ترمینال مبدا انتخابی: {bus.data.sourceTerminalShowName}";
                        nextStep = "dstate";
                        break;
                    case "dstate":
                        var destinationState = (BusTicket.CodeAndName)value;
                        bus.data.destinationStateCode = destinationState.code;
                        bus.data.destinationStateShowName = destinationState.name;
                        bus.setInfo();
                        messageToSend = $"استان مقصد انتخابی: {bus.data.destinationStateShowName}";
                        nextStep = "dterminal";
                        break;
                    case "dterminal":
                        var destinationTerminal = (BusTicket.CodeAndName)value;
                        bus.data.destinationTerminalCode = destinationTerminal.code;
                        bus.data.destinationTerminalShowName = destinationTerminal.name;
                        bus.setInfo();
                        messageToSend = $"شهر / ترمینال مقصد انتخابی: {bus.data.destinationTerminalShowName }";
                        nextStep = "datetime";
                        break;
                    case "datetime":
                        var fc = new FarsiCalendar((string)value);
                        bus.data.dateTime = fc.gDate;
                        bus.setInfo();
                        messageToSend = $"تاریخ انتخابی: {fc.pDate}";
                        nextStep = "service";
                        break;
                    case "fullname":
                        var fullname = (string)value;
                        bus.data.fullName = fullname;
                        bus.setInfo();
                        bus.getInfo();
                        messageToSend = $"نام مسافر: {bus.data.fullName}";
                        nextStep = "done";
                        currentAction.remove();

                        break;
                    default:
                        break;
                }
            } while (false);

            if (!String.IsNullOrEmpty(messageToSend))
            {
                if (callbackQueryId == 0)
                {
                    telegramAPI.send(messageToSend);
                }
                else
                {
                    telegramAPI.editText(callbackQueryId, messageToSend);
                }
                if (!String.IsNullOrEmpty(nextStep))
                {
                    BusTicketProcess(nextStep, bus.data.id);
                }

            }

        }


        private void BusTicketShowServiceDayTimeSummary(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var bus = new BusTicket.Manager(chatId, id);
            var msgToSend = "";
            do
            {
                var serviceList = bus.GetListOfServices();
                if (bus.resultAction.hasError)
                {
                    // there is no service
                    BusTicketSendNoServiceError(id, bus.resultAction.message);

                    break;
                }

                if (serviceList.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }
                bus.setInfo();
                var service = new BusTicket.Service(id, true);

                var summary = service.getDayTimeSummary();
                if (summary.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای نشان دادن خلاصه فهرست سرویسها یافت نشد.");
                    break;

                }
                msgToSend = "لطفا از فهرست زیر طیف زمانی را انتخاب نمایید. ";

                for (var i = 0; i < summary.Count; i++)
                {

                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{summary[i].dayTimeName} ({summary[i].countOfRecords})",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:daytime:{id}:{summary[i].dayTimeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();



                telegramAPI.send(msgToSend, markup);

            } while (false);
        }
        private void BusTicketShowServiceDayTimeTotal(long id, int dayTimeId, int page = 1, bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var msgToSend = "";
            do
            {
                var maxPage = 0;
                var service = new BusTicket.Service(id);
                var serviceData = service.getByDayTime(dayTimeId, page, out maxPage);
                var fc = new FarsiCalendar(serviceData.departureDateTime);
                msgToSend += " \n ";
                //msgToSend += $" شناسه این سرویس: /bts{id}_{serviceData.row}  \n";
                msgToSend += " نام شرکت: \n ";
                msgToSend += $"{serviceData.corporationName}  \n \n";
                msgToSend += " نوع اتوبوس: \n ";
                msgToSend += $"{serviceData.busType}  \n \n";
                msgToSend += " تاریخ و ساعت حرکت: \n ";
                msgToSend += $"{fc.pDate }  \n \n ";
                msgToSend += $" بهای بلیط: {serviceData.amount.ToString("#,##")} ریال \n \n ";

                msgToSend += $" گنجایش فعلی: {serviceData.capacity}  \n";
                msgToSend += " \n ";
                msgToSend += " \n -";
                var buttonExtraInfo = $"{SimpaySectionEnum.BusTicket}:pgs:{id}:{dayTimeId}";
                var paging = paginButtons(5, page, maxPage, buttonExtraInfo);

                if (paging != null)
                    inlineK.Add(paging);


                colK.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب",
                    CallbackData = $"{SimpaySectionEnum.BusTicket}:seat:{id}:{serviceData.row}"

                });

                inlineK.Add(colK.ToArray());
                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                if (forceNewWindow)
                {
                    telegramAPI.send(msgToSend, markup);
                }
                else
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);
                }


            } while (false);

        }
        private void BusTicketGetSeatMapOfService(int id, int serviceRow)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var bus = new BusTicket.Manager(chatId, id);
                bus.setSelectedServiceInfo(serviceRow);
                var seatInfo = bus.GetServiceSeats(serviceRow);
                if (bus.resultAction.hasError)
                {
                    // there is no service
                    BusTicketSendNoServiceError(id, bus.resultAction.message);

                    break;
                }


                BusTicketShowSeats(id, true);

            } while (false);
        }
        private void BusTicketShowSeats(long id, bool forceNewWindow = false) //id|row
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var bus = new BusTicket.Manager(chatId, id);
            var selectedCount = 0;
            do
            {
                var seatInfo = bus.getSeatInfo();
                var inlineK = new List<InlineKeyboardButton[]>();
                var colK = new List<InlineKeyboardButton>();
                var msgToSend = "";


                var col = 0;
                for (var i = 0; i < seatInfo.seats.Count; i++)
                {
                    col++;
                    var txt = "";
                    var callBackdata = "";
                    if (col == seatInfo.space + 1)
                    {
                        txt = ".";
                        callBackdata = $"{SimpaySectionEnum.BusTicket}:null:{id}:0";
                        colK.Add(new InlineKeyboardButton()
                        {
                            Text = $"{txt}",
                            CallbackData = $"{callBackdata}"
                        });
                    }
                    if (seatInfo.seats[i].seatNumber == 0)
                    {
                        txt = ".";
                        callBackdata = $"{SimpaySectionEnum.BusTicket}:null:{id}:0";
                    }
                    else if (seatInfo.seats[i].occupiedBy == 1)
                    {
                        txt = $"👩";
                        callBackdata = $"{SimpaySectionEnum.BusTicket}:null:{id}:0";
                    }
                    else if (seatInfo.seats[i].occupiedBy == 2)
                    {
                        txt = $"👨";
                        callBackdata = $"{SimpaySectionEnum.BusTicket}:null:{id}:0";
                    }
                    else if (seatInfo.seats[i].occupiedBy == 0)
                    {
                        if (seatInfo.seats[i].selectedByUser)
                        {
                            selectedCount++;
                            txt = $" *{seatInfo.seats[i].seatNumber}* ";
                            callBackdata = $"{SimpaySectionEnum.BusTicket}:unselectseat:{id}:{seatInfo.seats[i].mapIndex}";
                        }
                        else
                        {
                            txt = $"{seatInfo.seats[i].seatNumber}";
                            callBackdata = $"{SimpaySectionEnum.BusTicket}:selectseat:{id}:{seatInfo.seats[i].mapIndex}";

                        }
                    }

                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{txt}",
                        CallbackData = $"{callBackdata}"
                    });
                    if (col == seatInfo.columnNumber)
                    {
                        col = 0;
                        inlineK.Add(colK.Reverse<InlineKeyboardButton>().ToList().ToArray());
                        colK.Clear();
                    }
                }
                if (selectedCount == 0)
                {
                    msgToSend = "لطفا صندلی مورد نظر را انتخاب نمایید";
                    msgToSend += " \n \n ";
                    msgToSend += "نکته: صندلی های فروش رفته با توضیح نوع جنسیت مسافر، مشخص گردیده اند.";
                }
                else
                {
                    msgToSend = $"تعداد {selectedCount} صندلی انتخاب شده است";
                    msgToSend += " \n \n ";
                    msgToSend += " پس از انتخاب صندلیها به تعداد مسافر درخواستی، به مرحله بعد بروید.";
                    msgToSend += " \n \n ";
                    colK.Clear();
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"پایان انتخاب صندلی(ها) ",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:seatdone:{id}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();


                if (forceNewWindow)
                {
                    telegramAPI.send(msgToSend, markup);
                }
                else
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);
                }

            } while (false);
        }

        private void BusTicketSeatSelection(long id, int mapIndex, bool selected)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var bus = new BusTicket.Manager(chatId, id);
                bus.setSeatSelection(mapIndex, selected);
                bus.setSelectedSeatInfo();

                if (bus.resultAction.hasError)
                {
                    // there is no service
                    BusTicketSendNoServiceError(id, bus.resultAction.message);
                    break;
                }
                BusTicketShowSeats(id);

            } while (false);
        }

        private void BusTicketGenderSelectionMessage(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var bus = new BusTicket.Manager(chatId, id);
            var seatInfo = bus.getSeatInfo();

            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var msgToSend = "";
            var noFemale = false;
            do
            {
                foreach (var seat in seatInfo.seats)
                {
                    noFemale = false;
                    var selectedGender = "";

                    if (seat.seatNumber <= seatInfo.columnNumber)
                    {
                        noFemale = true;
                    }
                    if (seat.selectedByUser)
                    {
                        if (seat.selectedGender == 1)
                        {
                            selectedGender = " (👩) ";
                        }
                        else if (seat.selectedGender == 2)
                        {
                            selectedGender = " (👨) ";
                        }
                        colK.Add(new InlineKeyboardButton()
                        {
                            Text = $"صندلی {seat.seatNumber} {selectedGender} ",
                            CallbackData = $"{SimpaySectionEnum.BusTicket}:null:{id}"
                        });
                        inlineK.Add(colK.ToArray());
                        colK.Clear();
                        if (noFemale)
                        {
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = $"X",
                                CallbackData = $"{SimpaySectionEnum.BusTicket}:null:{id}:{seat.mapIndex}:{1}"
                            });
                        }
                        else
                        {
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = $"👩",
                                CallbackData = $"{SimpaySectionEnum.BusTicket}:selectgender:{id}:{seat.mapIndex}:{1}"
                            });

                        }
                        colK.Add(new InlineKeyboardButton()
                        {
                            Text = $"👨",
                            CallbackData = $"{SimpaySectionEnum.BusTicket}:selectgender:{id}:{seat.mapIndex}:{2}"
                        });
                        inlineK.Add(colK.ToArray());
                        colK.Clear();


                    }
                }

                if (seatInfo.isCompleted())
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"مرحله بعد",
                        CallbackData = $"{SimpaySectionEnum.BusTicket}:getname:{id}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"بازگشت به صندلیها",
                    CallbackData = $"{SimpaySectionEnum.BusTicket}:backtoseat:{id}"
                });
                inlineK.Add(colK.ToArray());
                colK.Clear();

                msgToSend = "نوع جنسیت صندلی های انتخابی را مشخص نمایید.";

            } while (false);


            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();

            telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);

        }
        private void BusTicketSelectGender(long id, int mapIndex, int genderIdx)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var bus = new BusTicket.Manager(chatId, id);

            bus.setSeatGender(mapIndex, genderIdx);

            BusTicketGenderSelectionMessage(id);

        }

        private void BusTicketGetNameMessage(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var bus = new BusTicket.Manager(chatId, id);
            var msgToSend = "";
            if (bus.data.seatCount > 1)
            {
                msgToSend = "لطفا نام کامل یکی از مسافران را وارد نمایید: ";
            }
            else
            {
                msgToSend = "لطفا نام کامل مسافر را وارد نمایید: ";
            }


            currentAction.set(SimpaySectionEnum.BusTicket, "fullname", $"{id}");
            telegramAPI.send(msgToSend);
        }
        private void BusTicketPaymentMessage(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var bus = new BusTicket.Manager(chatId, id);
            do
            {
                bus.ReserveSeat();
                if (bus.resultAction.hasError)
                {
                    sendMenu(message: bus.resultAction.message);
                    break;
                }

                PaymentStartProcess(bus.data.saleKey);

                //var msgToSend = bus.getTicketInfo();
                //msgToSend += "\n \n ";
                //msgToSend += "لطفا در صورت تایید اطلاعات فوق، با زدن دکمه زیر به صفحه بانک بروید.  ";


                //var resultLink = SimpayCore.getPaymentLink(bus.data.saleKey);
                //sendPaymentMessage(resultLink, msgToSend);


            } while (false);
        }

        private void BusTicketSendNoServiceError(long id, string errorMsg)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msgToSend = errorMsg;
            msgToSend += "\n \n";
            msgToSend += "با توجه به ایراد پیش آمده، خواهشمند است مرحله بعدی فرایند را مشخص فرمایید.: ";

            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "تغییر مبدا",
                CallbackData = $"{SimpaySectionEnum.BusTicket}:backtojob:{id}:sstate"
            });
            colK.Add(new InlineKeyboardButton()
            {
                Text = "تغییر مقصد",
                CallbackData = $"{SimpaySectionEnum.BusTicket}:backtojob:{id}:dstate"
            });
            colK.Add(new InlineKeyboardButton()
            {
                Text = "تغییر تاریخ",
                CallbackData = $"{SimpaySectionEnum.BusTicket}:backtojob:{id}:datetime"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();



            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();

            telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);

        }

        private void BusTerminalFastOptions(List<BusTicket.LastPath> list)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            foreach (var item in list)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{item.sourceTerminalShowName} به {item.destinationTerminalShowName} ",
                    CallbackData = $"{SimpaySectionEnum.BusTicket}:fastbuy:{item.id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"مسیر جدید",
                CallbackData = $"{SimpaySectionEnum.BusTicket}:backtojob:{0}:sstate"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر مسیر مربوطه را انتخاب نموده و یا مسیر جدیدی را وارد نمایید: ", r);

        }
        private void BusTicketFastBuy(long oldId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var oldBus = new BusTicket.Manager(chatId, oldId);
                var newBus = new BusTicket.Manager(chatId);

                var oldData = oldBus.data;
                newBus.data = new BusTicket.BusTicketData()
                {
                    dateTime = DateTime.Now,
                    sourceStateCode = oldData.sourceStateCode,
                    sourceStateShowName = oldData.sourceStateShowName,
                    sourceTerminalCode = oldData.sourceTerminalCode,
                    sourceTerminalShowName = oldData.sourceTerminalShowName,
                    destinationStateCode = oldData.destinationStateCode,
                    destinationStateShowName = oldData.destinationStateShowName,
                    destinationTerminalCode = oldData.destinationTerminalCode,
                    destinationTerminalShowName = oldData.destinationTerminalShowName,
                    status = TransactionStatusEnum.NotCompeleted,
                };
                newBus.setInfo();
                Log.Info($"New id created {newBus.data.id}", 0);
                BusTicketProcess("datetime", newBus.data.id);



            } while (false);
        }
        private void BusTicketShowDateTime(string theDate = null, string extraInfo = "", bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send("لطفا تاریخ سفر را از طریق تقویم زیر مشخص نمایید ");
            Calendar(theDate, extraInfo, forceNewWindow);


        }

        #endregion




    }
}