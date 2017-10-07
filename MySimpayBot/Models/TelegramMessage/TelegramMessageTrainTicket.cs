using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {
        private void TrainTicketRequestInfo(string fieldId = "", dynamic value = null, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msgToSend = "";
            var doSendMessage = true;
            var train = new TrainTicket.Manager(chatId, id);

            do
            {
                if (String.IsNullOrEmpty(fieldId))
                {
                    telegramAPI.send(" در حال جمع آوری اطلاعات مربوطه، لطفا صبر کنید. ", cancelButton());

                    var oldList = (new TrainTicket.Manager(chatId)).getLastPath();
                    if (oldList.Count > 0)
                    {
                        telegramAPI.send("در حال فراخوانی مسیرهای قبلی شما", cancelButton());
                        TrainTicketFastOptions(oldList);
                        doSendMessage = false;
                        break;
                    }


                    fieldId = "sstation";
                }
                switch (fieldId.ToLower())
                {
                    case "sstation":
                        //TrainTicketShowSourceStation(id);
                        msgToSend = "لطفا نام شهر مبدا را وارد نمایید";
                        if (train?.data.sourceStationCode != 0)
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {train?.data.sourceStationShowName }";
                        }
                        currentAction.set(SimpaySectionEnum.TrainTicket, "sstation", id.ToString());

                        break;
                    case "dstation":
                        msgToSend = "اکنون، نام شهر مقصد را وارد نمایید";
                        if (train?.data.destinationStationCode != 0)
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {train?.data.destinationStationShowName }";
                        }
                        currentAction.set(SimpaySectionEnum.TrainTicket, "dstation", id.ToString());
                        break;
                    case "tickettype":
                        doSendMessage = false;
                        msgToSend = "لطفا نوع بلیط را از فهرست زیر انتخاب نمایید:";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "tickettype", id.ToString());
                        TrainTicketShowTicketType(msgToSend, id);
                        break;
                    case "gdate":
                        doSendMessage = false;
                        msgToSend = "لطفا تاریخ رفت را از تقویم زیر انتخاب نمایید";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "gdate", id.ToString());
                        TrainTicketShowDateTime(theDate: null, caption: msgToSend, extraInfo: $"{SimpaySectionEnum.TrainTicket}|gdate|{id}", forceNewWindow: true);
                        break;


                    case "hasreturn":
                        doSendMessage = false;
                        TrainTicketHasReturnRequest(id);
                        break;
                    case "rdate":
                        doSendMessage = false;
                        msgToSend = "لطفا تاریخ برگشت را از تقویم زیر انتخاب نمایید";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "rdate", id.ToString());
                        TrainTicketShowDateTime(theDate: null, caption: msgToSend, extraInfo: $"{SimpaySectionEnum.TrainTicket}|rdate|{id}", forceNewWindow: true);
                        break;
                    case "seatcount":
                        msgToSend = "حال، تعداد بلیط درخواستی را بصورت عددی وارد نمایید:";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "seatcount", id.ToString());

                        break;
                    case "compartment":
                        doSendMessage = false;
                        //if this is car carrier then there is no compartment!
                        if (train.data.ticketTypeCode == TrainTicket.TicketTypeEnum.Car)
                        {
                            TrainTicketUpdateInfo(fieldId, 0, train.data.id);

                        }
                        else
                        {
                            TrainTicketCompartmentRequest(id);
                        }

                        break;
                    case "getservice":
                        doSendMessage = false;
                        TrainTicketSearchServices(id);
                        break;
                    case "gdaytimesummary":
                        doSendMessage = false;
                        TrainTicketShowGoServiceDayTimeSummary(id);
                        break;
                    case "rdaytimesummary":
                        doSendMessage = false;
                        TrainTicketShowReturnServiceDayTimeSummary(id);
                        break;
                    case "servicedetail":
                        doSendMessage = false;
                        TrainTicketGetServiceDetail(id);
                        break;
                    case "passengerinfo":
                        doSendMessage = false;
                        TrainTicketSendPassengerInfoMessage(id);
                        break;

                    // Passenger OR Car info which may be called in the loop!
                    case "passtype":
                        doSendMessage = false;
                        if (train.data.ticketTypeCode == TrainTicket.TicketTypeEnum.Car)
                        {
                            TrainTicketUpdateInfo("passtype", TrainTicket.TicketPassengerTypeEnum.CarCarriers, id);
                        }
                        else
                        {
                            TrainTicketPassengerInfoGetType(id);
                        }

                        break;
                    case "carowner":
                        msgToSend = "لطفا نام و نام خانوادگی صاحب خودرو را وارد نمایید:";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "carowner", id.ToString());
                        break;
                    case "carid":
                        msgToSend = "لطفا شماره شاسی  یا پلاک خودرو را به همراه نام تجاری آن وارد نمایید.:";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "carid", id.ToString());
                        break;


                    case "personel":
                        msgToSend = "با توجه به نوع مسافر انتخابی خواهشمند است شماره کارت ایثارگری خود را وارد نمایید:";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "personel", id.ToString());
                        break;
                    case "nationalcode":
                        msgToSend = "کد ملی مسافر را وارد نمایید";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "nationalcode", id.ToString());
                        break;
                    case "dateofbirth":
                        msgToSend = "لطفا تاریخ تولد را بصورت 1347/10/28  وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.TrainTicket, "dateofbirth", id.ToString());
                        //TrainTicketShowDateOfBirth(theDate: null, caption: msgToSend, extraInfo: $"{SimpaySectionEnum.TrainTicket}|dob|{id}", forceNewWindow: true);
                        break;
                    case "opservicego":
                        doSendMessage = false;
                        TrainTicketOptionalServices(train, "go");
                        break;
                    case "opservicereturn":
                        doSendMessage = false;
                        TrainTicketOptionalServices(train, "return");
                        break;
                    case "endpassenger":
                        doSendMessage = false;
                        TrainTicketEndOfPassengerInfo(train);
                        break;
                    case "setticketinfo":
                        doSendMessage = false;
                        TrainTicketSetTicketInfo(train);
                        break;
                    default:
                        msgToSend = $" {fieldId} not recognized!";
                        break;
                }

            } while (false);
            if (doSendMessage && !String.IsNullOrEmpty(msgToSend))
                telegramAPI.send(msgToSend);
        }
        private void TrainTicketCallBack(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = Convert.ToInt32(data.Split(':')[2]);
            var msgToSend = "";
            var station = new TrainTicket.Station();
            do
            {
                switch (action)
                {
                    case "gpg":
                        var dayTimeIdPageGo = Convert.ToInt32(Convert.ToInt32(data.Split(':')[3]));
                        var servicePageGo = Convert.ToInt32(Convert.ToInt32(data.Split(':')[4]));
                        TrainTicketShowServiceDayTimeTotal("go", id, dayTimeIdPageGo, servicePageGo);
                        break;
                    case "rpg":
                        var dayTimeIdPageReturn = Convert.ToInt32(Convert.ToInt32(data.Split(':')[3]));
                        var servicePageReturn = Convert.ToInt32(Convert.ToInt32(data.Split(':')[4]));
                        TrainTicketShowServiceDayTimeTotal("return", id, dayTimeIdPageReturn, servicePageReturn);
                        break;

                    case "sstation":
                        var sstationCode = Convert.ToInt32(data.Split(':')[3]);
                        station.setDataList(code: sstationCode);

                        TrainTicketUpdateInfo(action, station.data[0], id);

                        msgToSend = $"مبدا انتخابی: {station.data[0].stationShowName} ";
                        break;
                    case "dstation":
                        var dstationCode = Convert.ToInt32(data.Split(':')[3]);
                        station.setDataList(code: dstationCode);

                        TrainTicketUpdateInfo(action, station.data[0], id);

                        msgToSend = $"مقصد انتخابی: {station.data[0].stationShowName} ";
                        break;
                    case "tickettype":

                        //var ticketType = (TrainTicket.TicketTypeEnum)Convert.ToInt32(data.Split(':')[3]);
                        var ticketType = Enum.Parse(typeof(TrainTicket.TicketTypeEnum), data.Split(':')[3]);
                        TrainTicketUpdateInfo(action, ticketType, id);

                        //msgToSend = $"مقصد انتخابی: {station.data[0].stationShowName} ";

                        break;
                    case "gdate":
                        var gdate = data.Split(':')[3];
                        telegramAPI.send($"gdate={gdate}");

                        //TrainTicketUpdateInfo(action, station.data[0], id);
                        break;
                    case "hasreturn":
                        var answerHasRetrun = Convert.ToInt16(data.Split(':')[3]);

                        if (answerHasRetrun == 1 || answerHasRetrun == 0)// YES
                        {
                            TrainTicketUpdateInfo(action, answerHasRetrun, id);
                        }
                        break;
                    case "compartment":
                        var answerCompartment = Convert.ToInt16(data.Split(':')[3]);

                        if (answerCompartment == 1 || answerCompartment == 0)// YES
                        {
                            TrainTicketUpdateInfo(action, answerCompartment, id);
                        }
                        break;

                    case "gdaytime":
                        var gdayTimeId = Convert.ToInt32(data.Split(':')[3]);
                        TrainTicketShowServiceDayTimeTotal("go", id, gdayTimeId, 1, true);
                        break;
                    case "rdaytime":
                        var rdayTimeId = Convert.ToInt32(data.Split(':')[3]);
                        TrainTicketShowServiceDayTimeTotal("return", id, rdayTimeId, 1, true);
                        break;

                    case "gselect":
                        var gRow = Convert.ToInt16(data.Split(':')[3]);
                        TrainTicketUpdateInfo(action, gRow, id);
                        break;
                    case "rselect":
                        var rRow = Convert.ToInt16(data.Split(':')[3]);
                        TrainTicketUpdateInfo(action, rRow, id);
                        break;
                    case "backtojob":
                        var nextJob = data.Split(':')[3];
                        TrainTicketRequestInfo(nextJob, null, id);
                        break;
                    case "fastbuy":
                        TrainTicketFastBuy(id);
                        break;

                    case "passtype":
                        var passtype = Enum.Parse(typeof(TrainTicket.TicketPassengerTypeEnum), data.Split(':')[3]);
                        TrainTicketUpdateInfo("passtype", passtype, id);
                        break;

                    case "opservice":
                        //$"{SimpaySectionEnum.TrainTicket}:opservice:{train.data.id}:{way}:{opservice.row}"
                        var way = data.Split(':')[3];
                        var optionalServiceRow = Convert.ToInt32(data.Split(':')[4]);
                        TrainTicketUpdateInfo($"{action}{way}", optionalServiceRow, id);
                        break;
                    case "vfyok":

                        TrainTicketRequestInfo("setticketinfo", null, id);
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
        private void TrainTicketVerifyUserEntryText(string field, dynamic value, string currentTicketId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            long id = 0;
            if (!string.IsNullOrEmpty(currentTicketId))
            {
                id = Convert.ToInt32(currentTicketId);
            }
            do
            {

                switch (field.ToLower())
                {
                    case "sstation":
                    case "dstation":
                        if (Utils.isItOnlyEnglishCharecter((string)value))
                        {
                            telegramAPI.send("خواهشمند است نام شهر ها را حتما به فارسی وارد نمایید");
                            break;
                        }

                        TrainTicketCheckStationName(field, (string)value, id);

                        break;
                    case "seatcount":
                        var seatCountValue = value;
                        if (!Utils.isInteger(seatCountValue))
                        {
                            telegramAPI.send("این مقدار باید عددی باشد");
                            break;
                        }
                        TrainTicketUpdateInfo(field, value, id);
                        break;
                    case "nationalcode":
                        var nationalCode = (string)value;
                        if (!Utils.IsValidNationalCode(nationalCode))
                        {
                            telegramAPI.send("کد ملی درست وارد نشده است. لطفا یکبار دیگه کد ملی را بصورت یک عدد 10 رقمی بدون خط فاصله وارد نمایید.");
                            break;
                        }
                        TrainTicketUpdateInfo(field, value, id);
                        break;
                    default:
                        TrainTicketUpdateInfo(field, value, id);
                        break;
                }

            } while (false);

        }
        private void TrainTicketCheckStationName(string field, string stationKeyword, long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var station = new TrainTicket.Station();
            var stationCode = 0;
            var stationName = "";
            var showList = false;
            do
            {
                station.setDataList(keyword: stationKeyword);
                var stationList = station.data;
                if (stationList.Count > 1) // if found more than one station
                {
                    showList = true;
                }
                else if (stationList.Count == 0)
                {
                    showList = true;
                    station.setDataList();
                }
                // stationList.Count ==1 ' so found the station
                stationCode = stationList[0].stationCode;
                stationName = stationList[0].stationShowName;


            } while (false);
            switch (field.ToLower())
            {
                case "sstation":
                    if (showList)
                        TrainTicketShowSourceStation(station.data, id);
                    else
                        TrainTicketUpdateInfo(field,
                                                new TrainTicket.StationData { stationCode = stationCode, stationShowName = stationName },
                                                id);

                    break;
                case "dstation":
                    if (showList)
                        TrainTicketShowDestinationStation(station.data, id);
                    else
                        TrainTicketUpdateInfo(field,
                                                new TrainTicket.StationData { stationCode = stationCode, stationShowName = stationName },
                                                id);

                    break;

                default:
                    break;
            }

        }
        private void TrainTicketUpdateInfo(string field, dynamic value, long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var nextStepField = "";
            var train = new TrainTicket.Manager(chatId, id);
            var msgToSend = "";
            dynamic stepValue = null;
            do
            {
                switch (field.ToLower())
                {
                    case "sstation":
                        var sstationData = (TrainTicket.StationData)value;
                        train.data.sourceStationCode = sstationData.stationCode;
                        train.data.sourceStationShowName = sstationData.stationShowName;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;
                        nextStepField = "dstation";

                        msgToSend = $"مبدا انتخابی: {train.data.sourceStationShowName}";
                        currentAction.remove();
                        break;
                    case "dstation":
                        var dstationData = (TrainTicket.StationData)value;
                        train.data.destinationStationCode = dstationData.stationCode;
                        train.data.destinationStationShowName = dstationData.stationShowName;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;

                        nextStepField = "tickettype";
                        msgToSend = $"مقصد انتخابی: {train.data.destinationStationShowName}";
                        currentAction.remove();
                        break;
                    case "tickettype":
                        var ticketType = (TrainTicket.TicketTypeEnum)value;
                        train.data.ticketTypeCode = ticketType;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;

                        nextStepField = "gdate";

                        msgToSend = $"نوع بلیط انتخابی: {train.getTicketTypeName(ticketType)}";

                        break;


                    case "gdate":
                        var fcGo = new FarsiCalendar((string)value);
                        if (String.Compare(fcGo.gDate.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd")) < 0)
                        {
                            msgToSend = "تاریخ باید بزرگتر از امروز باشد";
                            nextStepField = "gdate";
                            break;
                        }
                        train.data.wayGoDateTime = fcGo.gDate;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;
                        nextStepField = "hasreturn";
                        msgToSend = $"تاریخ رفت انتخابی: {fcGo.pDate}";
                        break;
                    case "hasreturn":
                        var answer = (int)value;
                        if (answer == 1)
                        {
                            train.data.twoWay = true;
                            msgToSend = "بلیط برگشتی دارد";
                            nextStepField = "rdate";
                        }
                        else
                        {
                            train.data.twoWay = false;
                            msgToSend = "بلیط برگشتی ندارد";
                            nextStepField = "seatcount";
                        }
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;


                        break;
                    case "rdate":
                        var fgReturn = new FarsiCalendar((string)value);
                        if (String.Compare(fgReturn.gDate.ToString("yyyy/MM/dd"), Convert.ToDateTime(train.data.wayGoDateTime).ToString("yyyy/MM/dd")) < 0)
                        {
                            msgToSend = "تاریخ برگشت باید مساوی یا بزرگتر از تاریخ رفت باشد. ";
                            nextStepField = "rdate";
                            break;
                        }
                        train.data.wayReturnDateTime = fgReturn.gDate;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;

                        nextStepField = "seatcount";
                        msgToSend = $"تاریخ برگشت انتخابی: {fgReturn.pDate}";
                        break;
                    case "seatcount":
                        train.data.seatCount = Convert.ToInt16(value);
                        train.setInfo();


                        if (id == 0)
                            id = train.data.id;

                        nextStepField = "compartment";
                        msgToSend = $"تعداد صندلی های انتخابی: {train.data.seatCount}";
                        currentAction.remove();
                        break;
                    case "compartment":
                        train.data.justCompartment = (value == 1) ? true : false;
                        train.setInfo();
                        if (id == 0)
                            id = train.data.id;


                        var compartmentYesNo = train.data.justCompartment ? "بله" : "خیر";
                        msgToSend = $"درخواست کوپه دربستی: {compartmentYesNo}";

                        nextStepField = "getservice";
                        break;
                    case "gselect":
                        train.data.goRow = (int)value;
                        train.setInfo();

                        if (train.data.twoWay)
                        {
                            nextStepField = "rdaytimesummary";
                        }
                        else
                        {
                            nextStepField = "servicedetail";
                        }
                        break;
                    case "rselect":
                        train.data.returnRow = (int)value;
                        train.setInfo();
                        nextStepField = "servicedetail";
                        break;

                    case "servicedetail":
                        break;
                    case "passtype":
                        var passTypeId = (TrainTicket.TicketPassengerTypeEnum)value;
                        var passTypeName = train.GetPassengerType(passTypeId);

                        train.passenger.data[train.data.currentPassengerRow - 1].passengerTypeCode = passTypeId;
                        train.passenger.data[train.data.currentPassengerRow - 1].passengerTypeShowName = passTypeName;
                        train.passenger.setInfo(train.data.currentPassengerRow);

                        msgToSend = $"نوع مسافر انتخابی: {passTypeName}";
                        if (passTypeId == TrainTicket.TicketPassengerTypeEnum.Martyr || passTypeId == TrainTicket.TicketPassengerTypeEnum.Veteran)
                        {
                            nextStepField = "personel";
                        }
                        else if (passTypeId == TrainTicket.TicketPassengerTypeEnum.CarCarriers)
                        {
                            nextStepField = "carowner";
                        }
                        else
                        {
                            nextStepField = "nationalcode";
                        }
                        currentAction.remove();
                        break;
                    case "carowner":
                        var carowner = (string)value;
                        train.passenger.data[train.data.currentPassengerRow - 1].lastName = carowner;
                        train.passenger.data[train.data.currentPassengerRow - 1].nationalCode = train.data.currentPassengerRow.ToString();

                        train.passenger.setInfo(train.data.currentPassengerRow);
                        nextStepField = "carid";
                        currentAction.remove();
                        break;
                    case "carid":
                        var carid = (string)value;
                        train.passenger.data[train.data.currentPassengerRow - 1].firstName = carid;
                        train.passenger.setInfo(train.data.currentPassengerRow);

                        nextStepField = "opservicego";
                        //nextStepField = "endpassenger";
                        stepValue = "go";
                        currentAction.remove();
                        break;

                    case "personel":
                        var personel = (string)value;

                        train.passenger.data[train.data.currentPassengerRow - 1].personel = personel;
                        train.passenger.setInfo(train.data.currentPassengerRow);
                        msgToSend = $"کد ایثار گری وارد شده: {personel}";

                        nextStepField = "nationalcode";

                        currentAction.remove();
                        break;

                    case "nationalcode":
                        var nationalCode = (string)value;

                        train.passenger.data[train.data.currentPassengerRow - 1].nationalCode = nationalCode;
                        train.passenger.setInfo(train.data.currentPassengerRow);
                        msgToSend = $"شماره کد ملی وارد شده: {nationalCode}";

                        nextStepField = "dateOfBirth";

                        currentAction.remove();
                        break;
                    case "dateofbirth":
                        var shamsiDateOfBirth = (string)value;
                        var fcdob = new FarsiCalendar(shamsiDateOfBirth);

                        train.passenger.data[train.data.currentPassengerRow - 1].dateOfBirth = fcdob.gDate;
                        train.passenger.setInfo(train.data.currentPassengerRow);
                        msgToSend = $"تاریخ تولد وارد شده: {fcdob.pDate}";

                        nextStepField = "opservicego";
                        stepValue = "go";
                        currentAction.remove();
                        break;
                    case "opservicego":

                        var optionalServiceGoRow = (int)value;

                        var optionalServiceGo = train.optionalServiceGo;

                        train.passenger.data[train.data.currentPassengerRow - 1].optionalServiceGo = optionalServiceGoRow;
                        train.passenger.setInfo(train.data.currentPassengerRow);
                        msgToSend = $"سرویس انتخابی برای رفت: {optionalServiceGo.getService(optionalServiceGoRow).name}";
                        currentAction.remove();

                        if (train.optionalServiceReturn.data.Count != 0)
                        {
                            nextStepField = "opservicereturn";
                        }
                        else
                        {
                            nextStepField = "endpassenger";
                        }

                        break;

                    case "opservicereturn":

                        var optionalServiceReturnRow = (int)value;

                        var optionalServiceReturn = train.optionalServiceReturn;


                        train.passenger.data[train.data.currentPassengerRow - 1].optionalServiceReturn = optionalServiceReturnRow;
                        train.passenger.setInfo(train.data.currentPassengerRow);
                        msgToSend = $"سرویس انتخابی برای برگشت: {optionalServiceReturn.getService(optionalServiceReturnRow).name}";
                        nextStepField = "endpassenger";
                        currentAction.remove();
                        break;


                    default:
                        break;
                }
            } while (false);
            if (!String.IsNullOrEmpty(msgToSend))
            {
                if (callbackQuery == null)
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
                TrainTicketRequestInfo(nextStepField, stepValue, id);
            }


        }

        private void TrainTicketShowSourceStation(List<TrainTicket.StationData> list, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = TrainTicketGetListOfStationButtons(list, "sstation", id);

                var msgToSend = "لطفا مبدا را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);


        }

        private void TrainTicketShowDestinationStation(List<TrainTicket.StationData> list, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = TrainTicketGetListOfStationButtons(list, "dstation", id);

                var msgToSend = "لطفا مقصد را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);


        }



        private InlineKeyboardButton[][] TrainTicketGetListOfStationButtons(List<TrainTicket.StationData> list, string action, long id)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var listIndex = 0;
            var colIndex = 0;
            var maxCol = 3;
            var btnText = "";
            var btnData = "";
            while (listIndex < list.Count)
            {
                colIndex = 0;
                while (colIndex < maxCol)
                {
                    colIndex++;
                    if (listIndex < list.Count)
                    {
                        btnText = $"{list[listIndex].stationShowName}";
                        btnData = $"{SimpaySectionEnum.TrainTicket}:{action}:{id}:{list[listIndex].stationCode}";
                    }
                    else
                    {
                        btnText = $".";
                        btnData = $"{SimpaySectionEnum.TrainTicket}:blank:{id}:0";

                    }
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = btnText,
                        CallbackData = btnData,
                    });


                    listIndex++;
                }
                inlineK.Add(colK.ToArray());
                colK.Clear();

            }
            return inlineK.ToArray(); ;

        }

        private void TrainTicketShowDateTime(string theDate = null, string caption = "", string extraInfo = "", bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            telegramAPI.send(caption);

            Calendar(theDate, extraInfo, forceNewWindow, CalendarNavigationSwitchEnum.ChangeMonth);


        }
        private void TrainTicketShowDateOfBirth(string theDate = null, string caption = "", string extraInfo = "", bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send(caption);

            Calendar(theDate, extraInfo, forceNewWindow, CalendarNavigationSwitchEnum.SelectYears | CalendarNavigationSwitchEnum.SelectMonths);


        }

        private void TrainTicketShowTicketType(string msgToSend = ".", long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            colK.Add(new InlineKeyboardButton()
            {
                Text = "عادی",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:tickettype:{id}:{TrainTicket.TicketTypeEnum.Normal}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "ویژه برادران",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:tickettype:{id}:{TrainTicket.TicketTypeEnum.Men}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "ویژه خواهران",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:tickettype:{id}:{TrainTicket.TicketTypeEnum.Women}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            colK.Add(new InlineKeyboardButton()
            {
                Text = "ویژه حمل خودرو ",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:tickettype:{id}:{TrainTicket.TicketTypeEnum.Car}"
            });
            inlineK.Add(colK.ToArray());
            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();


            telegramAPI.send(msgToSend, markup);

        }

        private void TrainTicketHasReturnRequest(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            colK.Add(new InlineKeyboardButton()
            {
                Text = "بله",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:hasreturn:{id}:1"
            });
            colK.Add(new InlineKeyboardButton()
            {
                Text = "خیر",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:hasreturn:{id}:0"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();


            telegramAPI.send("آیا مایل به خرید بلیط برگشت نیز میباشید؟", markup);

        }

        private void TrainTicketCompartmentRequest(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            colK.Add(new InlineKeyboardButton()
            {
                Text = "بله",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:compartment:{id}:1"
            });
            colK.Add(new InlineKeyboardButton()
            {
                Text = "خیر",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:compartment:{id}:0"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();


            telegramAPI.send("آیا کوپه بصورت دربستی باشد؟", markup);

        }

        private void TrainTicketSearchServices(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send(" در حال جستجوی سرویسها با توجه به اطلاعات فوق ");
            var train = new TrainTicket.Manager(chatId, id);
            do
            {

                train.getServices();
                if (train.resultAction.hasError)
                {
                    telegramAPI.send(train.resultAction.message);
                    break;
                }
                if (train.data.twoWay)
                {
                    if (!train.hasServiceGo && !train.hasServiceReturn)
                    {
                        TrainTicketChangeFieldNavigate("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.", "sstation,dstation,gdate", id);
                        break;
                    }
                    if (!train.hasServiceGo && train.hasServiceReturn)
                    {
                        TrainTicketChangeFieldNavigate("هیچ سرویسی برای مسیر رفت یافت نشد.", "sstation,dstation,gdate", id);
                        break;
                    }
                    if (train.hasServiceGo && !train.hasServiceReturn)
                    {
                        TrainTicketChangeFieldNavigate("هیچ سرویسی برای مسیر برگشت یافت نشد.", "sstation,dstation,rdate", id);
                        break;
                    }
                }
                else
                {
                    if (!train.hasServiceGo)
                    {
                        TrainTicketChangeFieldNavigate("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.", "gdate", id);
                        break;
                    }
                }
                TrainTicketRequestInfo("gdaytimesummary", null, id);

            } while (false);
        }

        private void TrainTicketShowGoServiceDayTimeSummary(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var train = new TrainTicket.Manager(chatId, id);
            var msgToSend = "";
            do
            {
                var serviceGo = train.serviceGo;

                var summary = serviceGo.getDayTimeSummary();
                if (summary.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای نشان دادن خلاصه فهرست سرویسها یافت نشد.");
                    break;

                }
                msgToSend = " برای انتخاب سرویس رفت، لطفا از فهرست زیر طیف زمانی را انتخاب نمایید ";

                for (var i = 0; i < summary.Count; i++)
                {

                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{summary[i].dayTimeName} ({summary[i].countOfRecords})",
                        CallbackData = $"{SimpaySectionEnum.TrainTicket}:gdaytime:{id}:{summary[i].dayTimeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();



                telegramAPI.send(msgToSend, markup);

            } while (false);

        }
        private void TrainTicketShowReturnServiceDayTimeSummary(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var train = new TrainTicket.Manager(chatId, id);

            var msgToSend = "";
            do
            {
                if (train.serviceReturn == null)
                {
                    telegramAPI.send("go to next level!");
                    break;
                }
                var serviceReturn = train.serviceReturn;

                var summary = serviceReturn.getDayTimeSummary();
                if (summary.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای نشان دادن خلاصه فهرست سرویسها یافت نشد.");
                    break;

                }
                msgToSend = " برای انتخاب سرویس برگشت، لطفا از فهرست زیر طیف زمانی را انتخاب نمایید ";

                for (var i = 0; i < summary.Count; i++)
                {

                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{summary[i].dayTimeName} ({summary[i].countOfRecords})",
                        CallbackData = $"{SimpaySectionEnum.TrainTicket}:rdaytime:{id}:{summary[i].dayTimeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();



                telegramAPI.send(msgToSend, markup);

            } while (false);

        }

        private void TrainTicketChangeFieldNavigate(string message, string fields, long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var messageToSend = "";
            do
            {
                var arrfields = fields.Split(',');
                foreach (var field in arrfields)
                {
                    switch (field.ToLower())
                    {
                        case "sstation":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تغییر مبدا",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;
                        case "dstation":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تغیر مقصد",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;

                        case "tickettype":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تغییر نوع بلیط",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;
                        case "gdate":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تغییر تاریخ رفت",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });

                            break;
                        case "rdate":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تغییر تاریخ برگشت",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;
                        case "seatcount":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "تعداد ",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;
                        case "compartment":
                            colK.Add(new InlineKeyboardButton()
                            {
                                Text = "دربستی",
                                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{id}:{field}"
                            });
                            break;
                        default:
                            break;
                    }
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }

                messageToSend += message + " \n \n";
                messageToSend += " لطفا برای جستجوی دوباره، از کدام مرحله مایل به تغییر میباشید؟";

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send(messageToSend, markup);

            } while (false);

        }
        private void TrainTicketShowServiceDayTimeTotal(string way, long id, int dayTimeId, int page = 1, bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var train = new TrainTicket.Manager(chatId, id);

            var msgToSend = "";

            do
            {
                var maxPage = 0;
                var callbackAction = "";
                var paginAction = "";
                var service = new TrainTicket.Service(id, way);
                if (way == "return")
                {
                    callbackAction = "rselect";
                    paginAction = "rpg";
                    service = train.serviceReturn;
                }
                else if (way == "go")
                {
                    callbackAction = "gselect";
                    paginAction = "gpg";
                    service = train.serviceGo;
                }
                var serviceData = service.getByDayTime(dayTimeId, page, out maxPage);
                var fc = new FarsiCalendar(serviceData.departureDateTime);
                var hasCompartment = serviceData.isCompartment ? "بله " : " خیر ";
                var hasMedia = serviceData.media ? "بله " : " خیر ";
                var hasAC = serviceData.airConditioning ? "بله " : " خیر ";
                msgToSend += " \n ";
                //msgToSend += $" شناسه این سرویس: /bts{id}_{serviceData.row}  \n";

                msgToSend += $"شماره قطار:  {serviceData.trainNumber}  \n \n";
                msgToSend += " نام قطار: \n ";
                msgToSend += $"{serviceData.trainName}  \n \n";
                msgToSend += " نوع قطار: \n ";
                msgToSend += $"{serviceData.trainType}  \n \n";

                msgToSend += $" کوپه دارد: {hasCompartment }  \n \n";
                msgToSend += $" صوت و تصویر: {hasCompartment }  \n \n";
                msgToSend += $" تهویه مطبوع دارد: {hasCompartment }  \n \n";


                msgToSend += " تاریخ و ساعت حرکت: \n ";
                msgToSend += $"{fc.pDate }  \n \n ";
                msgToSend += $" ساعت رسیدن {serviceData.arrivalTime}: \n \n ";
                msgToSend += $" بهای بلیط : {serviceData.realAmount.ToString("#,##")} ریال \n \n ";

                msgToSend += $" گنجایش فعلی: {serviceData.availableCapacity}  \n";
                msgToSend += " \n ";
                msgToSend += " \n -";
                var buttonExtraInfo = $"{SimpaySectionEnum.TrainTicket}:{paginAction}:{id}:{dayTimeId}";
                var paging = paginButtons(5, page, maxPage, buttonExtraInfo);

                if (paging != null)
                    inlineK.Add(paging);


                colK.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب",
                    CallbackData = $"{SimpaySectionEnum.TrainTicket}:{callbackAction}:{id}:{serviceData.row}"

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
        private void TrainTicketFastOptions(List<TrainTicket.LastPath> list)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            foreach (var item in list)
            {
                colKey.Add(new InlineKeyboardButton
                {
                    Text = $@"{item.sourceStationShowName} به {item.destinationStationShowName} ",
                    CallbackData = $"{SimpaySectionEnum.TrainTicket}:fastbuy:{item.id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

            }
            colKey.Add(new InlineKeyboardButton
            {
                Text = $@"مسیر جدید",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:backtojob:{0}:sstation"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("شما، هم میتوانید از فهرست زیر مسیر مربوطه را انتخاب نموده و یا مسیر جدیدی را وارد نمایید: ", r);

        }
        private void TrainTicketFastBuy(long oldId = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                do
                {
                    var oldPath = new TrainTicket.Manager(chatId, oldId);
                    var newPath = new TrainTicket.Manager(chatId);

                    var oldData = oldPath.data;
                    newPath.data = new TrainTicket.TrainTicketData()
                    {
                        sourceStationCode = oldData.sourceStationCode,
                        sourceStationShowName = oldData.sourceStationShowName,
                        destinationStationCode = oldData.destinationStationCode,
                        destinationStationShowName = oldData.destinationStationShowName,
                        status = TransactionStatusEnum.NotCompeleted,
                    };
                    newPath.setInfo();
                    TrainTicketRequestInfo("tickettype", null, newPath.data.id);

                } while (false);

            } while (false);

        }

        private void TrainTicketGetServiceDetail(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send(" در حال بررسی سرویسهای انتخابی ");
            var train = new TrainTicket.Manager(chatId, id);
            do
            {
                train.GetServiceDetailInfo();
                if (train.resultAction.hasError)
                {
                    telegramAPI.send(train.resultAction.message);
                    break;
                }
                TrainTicketRequestInfo("passengerinfo", null, id);

            } while (false);


        }

        private void TrainTicketSendPassengerInfoMessage(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var train = new TrainTicket.Manager(chatId, id);

            var message = "";
            do
            {
                if (train.data.ticketTypeCode == TrainTicket.TicketTypeEnum.Car)
                {
                    //message = $"در این مرحله لطفا اطلاعات شناسنامه ای {train.data.seatCount} مسافر را وارد نمایید:";
                    message += $"در این مرحله لطفا اطلاعات  {train.data.seatCount} خودرو را وارد نمایید.";
                    message += " \n \n ";

                }
                else if (train.data.seatCount > 1)
                {
                    message += $"در این مرحله لطفا اطلاعات شناسنامه ای {train.data.seatCount} مسافر را وارد نمایید.";
                    message += " \n \n ";

                }
                else if (train.data.seatCount == 1)
                {
                    message += "در این مرحله لطفا اطلاعات شناسنامه ای مسافر را وارد نمایید.";
                }
                train.data.currentPassengerRow = 1;
                train.setInfo();
                telegramAPI.send(message);
                TrainTicketRequestInfo("passtype", null, id);

            } while (false);
        }

        private void TrainTicketPassengerInfoGetType(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            colKey.Add(new InlineKeyboardButton()
            {
                Text = $"بزرگسال",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:passtype:{id}:{TrainTicket.TicketPassengerTypeEnum.Adult}"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            colKey.Add(new InlineKeyboardButton()
            {
                Text = "خردسال",
                CallbackData = $"{SimpaySectionEnum.TrainTicket}:passtype:{id}:{TrainTicket.TicketPassengerTypeEnum.Child}"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            //colKey.Add(new InlineKeyboardButton()
            //{
            //    Text = "شاهد",
            //    CallbackData = $"{SimpaySectionEnum.TrainTicket}:passtype:{id}:{TrainTicket.TicketPassengerTypeEnum.Martyr}"
            //});
            //inlineK.Add(colKey.ToArray());
            //colKey.Clear();

            //colKey.Add(new InlineKeyboardButton()
            //{
            //    Text = "جانباز",
            //    CallbackData = $"{SimpaySectionEnum.TrainTicket}:passtype:{id}:{TrainTicket.TicketPassengerTypeEnum.Veteran}"
            //});
            //inlineK.Add(colKey.ToArray());
            //colKey.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("لطفا ابتدا نوع مسافر را انتخاب کنید: ", markup);

        }

        private void TrainTicketOptionalServices(TrainTicket.Manager train, string way)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var gotoNextStep = false;
            do
            {


                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();


                var optionalService = new TrainTicket.OptionalService(train.data.id, way, true);

                //if (train.data.ticketTypeCode == TrainTicket.TicketTypeEnum.Car)
                //{
                //    var currentField = "";
                //    if (way == "go")
                //    {
                //        currentField = "opservicego";
                //    }
                //    else
                //    {
                //        currentField = "opservicereturn";
                //    }

                //    TrainTicketUpdateInfo(currentField, 0, train.data.id);
                //    break;
                //}


                if (optionalService.data.Count == 0)
                {
                    gotoNextStep = true;
                    break;
                }
                foreach (var opservice in optionalService.data)
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = $"{opservice.name} {opservice.amount.ToString("0,##")} ریال",
                        CallbackData = $"{SimpaySectionEnum.TrainTicket}:opservice:{train.data.id}:{way}:{opservice.row}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send("لطفا یکی از سرویسهای پذیرائی را انتخاب نمایید: ", markup);



            } while (false);
            if (gotoNextStep)
            {
                if (way == "go" && train.data.twoWay)
                {
                    TrainTicketOptionalServices(train, "return");
                }
                else
                {
                    TrainTicketEndOfPassengerInfo(train);
                }

            }
        }
        private void TrainTicketEndOfPassengerInfo(TrainTicket.Manager train)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            if (train.data.seatCount > train.data.currentPassengerRow)
            {
                train.data.currentPassengerRow++;
                train.setInfo();
                telegramAPI.send($"حال اطلاعات مسافر {train.data.currentPassengerRow} ام را وارد نمایید ");
                TrainTicketRequestInfo("passtype", null, train.data.id);
            }
            else
            {
                TrainTicketVerifyPassengerInfo(train);
            }

        }
        private void TrainTicketVerifyPassengerInfo(TrainTicket.Manager train)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var message = " لطفا اطلاعات وارد شده برای هر بلیط را کنترل نمایید.  ";
            var passenger = new TrainTicket.Passenger(train.data.id, true);
            foreach (var info in passenger.data)
            {
                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();
                if (train.data.ticketTypeCode == TrainTicket.TicketTypeEnum.Car)
                {
                    message += $"{Environment.NewLine} {Environment.NewLine} ";
                    message += $"خودرو شماره :  {info.row}  {Environment.NewLine} ";
                    message += $"نام مالک: {info.lastName}    {Environment.NewLine}";
                    message += $"مشخصات خودرو: {info.firstName}    {Environment.NewLine} ";

                    message += $"{Environment.NewLine} {Environment.NewLine} -----------";

                }
                else
                {
                    message += "\n \n ";
                    message += $"مسافر شماره:  {info.row}  \n ";
                    message += $"نوع مسافر: {info.passengerTypeShowName} \n ";
                    if (!String.IsNullOrEmpty(info.personel))
                    {
                        message += $"شماره ایثار گری: {info.personel}   \n ";
                    }
                    message += $"کد ملی: {info.nationalCode}    \n ";

                    var fcDob = new FarsiCalendar(Convert.ToDateTime(info.dateOfBirth));

                    message += $"تاریخ تولد: {fcDob.pDate}    \n ";
                    message += "\n \n -----------";



                }
                colKey.Add(new InlineKeyboardButton
                {
                    Text = "ادامه",
                    CallbackData = $"{SimpaySectionEnum.TrainTicket}:vfyok:{train.data.id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                colKey.Add(new InlineKeyboardButton
                {
                    Text = "ویرایش",
                    CallbackData = $"{SimpaySectionEnum.TrainTicket}:editpass:{train.data.id}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                telegramAPI.send(message, markup);

            }


        }

        private void TrainTicketSetTicketInfo(TrainTicket.Manager train)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                train.SetTicketInfo();
                if (train.resultAction.hasError)
                {
                    train.data.currentPassengerRow = 1;
                    train.setInfo();
                    telegramAPI.send(train.resultAction.message);
                    TrainTicketRequestInfo("passtype", null, train.data.id);
                    currentAction.remove();
                    break;
                }
                train.LockSeat();
                if (train.resultAction.hasError)
                {
                    currentAction.remove();
                    sendMenu(message: train.resultAction.message);
                    break;
                }



                PaymentStartProcess(train.data.saleKey);

                //var trainInfo = "";
                //var msgToSend = "";
                //trainInfo += "اطلاعات رفت: \n \n";
                //trainInfo += train.getTrainInfo("go");
                //if (train.data.twoWay)
                //{
                //    trainInfo += "اطلاعات برگشت: \n \n";
                //    trainInfo += train.getTrainInfo("return");
                //}
                //msgToSend += trainInfo;
                //msgToSend += "\n  ";
                //msgToSend += "\n  ";
                //msgToSend += "لطفا در صورت تایید اطلاعات فوق، با زدن دکمه زیر به صفحه بانک بروید.  ";


                //var resultLink = SimpayCore.getPaymentLink(train.data.saleKey);
                //if (SimpayCore.resultAction.hasError)
                //{
                //    telegramAPI.send(SimpayCore.resultAction.message);
                //    break;
                //}
                //sendPaymentMessage(resultLink, msgToSend);
                //telegramAPI.send(msgToSend);

            } while (false);
        }

        private void TrainTicketRedeemTicket(string saleKey)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var msgToSend = "";
                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                var train = new TrainTicket.Manager(chatId);
                train.Redeem(saleKey);

                if (train.resultAction.hasError)
                {
                    sendMenu(message: train.resultAction.message);
                    break;
                }

                msgToSend = "بلیط(های) شما صادر گردید. لطفا از لینکهای زیر جهت مشاهده و چاپ بلیط(ها) استفاده نمایید:";

                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $" بلیط  ",
                    //Url = $@"http://simpay.ir/train-ticket-print.aspx?param={saleKey}&sid={SimpayCore.getSessionId()}"
                    //Url = $@"http://simpay724.com/home/PrintTrainTicket/{}/{}"
                    //Url = $@"http://37.32.121.153:1026/home/PrintTrainTicket/{saleKey}/{SimpayCore.getSessionId()}"
                    Url = $@"http://tgct.simpay.ir/home/PrintTrainTicket/{saleKey}/{SimpayCore.getSessionId()}"

                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                telegramAPI.send(msgToSend, markup);

            } while (false);
        }

    }
}