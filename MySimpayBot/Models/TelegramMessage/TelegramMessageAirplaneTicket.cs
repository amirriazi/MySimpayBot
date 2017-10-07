using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;


namespace Models
{
    public partial class TelegramMessage
    {
        private void AirplaneTicketRequestInfo(string fieldId = "", dynamic value = null, long id = 0)
        {
            var msgToSend = "";
            var doSendMessage = true;
            var airplane = new AirplaneTicket.Manager(chatId, id);
            do
            {

                if (String.IsNullOrEmpty(fieldId))
                {
                    telegramAPI.send(" در حال جمع آوری اطلاعات مربوطه، لطفا صبر کنید. ", cancelButton());

                    //var oldList = (new Airplane.Manager(chatId)).getLastPath();
                    //if (oldList.Count > 0)
                    //{
                    //    telegramAPI.send("در حال فراخوانی مسیرهای قبلی شما", cancelButton());
                    //    AirplaneTicketFastOptions(oldList);
                    //    doSendMessage = false;
                    //    break;
                    //}


                    fieldId = "sairport";
                }
                switch (fieldId.ToLower())
                {
                    case "sairport":
                        msgToSend = "لطفا نام فرودگاه مبدا را وارد نمایید";
                        if (!String.IsNullOrEmpty(airplane?.data.sourceAirportCode))
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {airplane?.data.sourceAirportShowName}";
                        }
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, fieldId.ToLower(), id.ToString());

                        break;
                    case "dairport":
                        msgToSend = "لطفا نام فروردگاه مقصد را وارد نمایید";
                        if (!String.IsNullOrEmpty(airplane?.data.destinationAirportCode))
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {airplane?.data.destinationAirportShowName}";
                        }
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, fieldId.ToLower(), id.ToString());

                        break;
                    case "adult":
                        msgToSend = "در این مرحله تعداد مسافر بزرگسال را وارد نمایید:";
                        if (airplane?.data.adultCount != 0)
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {airplane?.data.adultCount}";
                        }
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, fieldId.ToLower(), id.ToString());
                        break;
                    case "child":
                        msgToSend = "حال تعداد مسافر کودک را وارد نمایید:";
                        if (airplane?.data.childCount != 0)
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {airplane?.data.childCount}";
                        }
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, fieldId.ToLower(), id.ToString());
                        break;
                    case "infant":
                        msgToSend = "حال تعداد مسافر نوزاد را وارد نمایید:";
                        if (airplane?.data.infantCount != 0)
                        {
                            msgToSend += "\n \n";
                            msgToSend += $"مقدار قبلی: {airplane?.data.infantCount}";
                        }
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, fieldId.ToLower(), id.ToString());
                        break;
                    case "gdate":
                        doSendMessage = false;
                        msgToSend = "لطفا تاریخ رفت را از تقویم زیر انتخاب، یا با فرمت سال/ماه/روز وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "gdate", id.ToString());

                        AirplaneTicketShowDateTime(theDate: null, caption: msgToSend, exairplanefo: $"{SimpaySectionEnum.AirplaneTicket}|gdate|{id}", forceNewWindow: true);
                        break;


                    case "hasreturn":
                        doSendMessage = false;
                        AirplaneTicketHasReturnRequest(id);
                        break;
                    case "rdate":
                        doSendMessage = false;
                        msgToSend = "لطفا تاریخ برگشت را از تقویم زیر انتخاب، یا با فرمت سال/ماه/روز وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "rdate", id.ToString());
                        AirplaneTicketShowDateTime(theDate: null, caption: msgToSend, exairplanefo: $"{SimpaySectionEnum.AirplaneTicket}|rdate|{id}", forceNewWindow: true);
                        break;
                    case "search":
                        doSendMessage = false;
                        AirplaneTicketSearchServices(airplane);
                        break;
                    case "gdaytimesummary":
                        doSendMessage = false;
                        AirplaneTicketShowGoServiceDayTimeSummary(airplane);
                        break;


                    case "rdaytimesummary":
                        doSendMessage = false;
                        AirplaneTicketShowReturnServiceDayTimeSummary(airplane);
                        break;
                    case "passengerinfo":
                        doSendMessage = false;
                        AirplaneTicketSendPassengerInfoMessage(airplane);
                        break;

                    // Passenger info which may be called in the loop!
                    case "passtype":
                        doSendMessage = false;
                        AirplaneTicketPassengerInfoGetType(airplane);
                        break;
                    case "title":
                        doSendMessage = false;
                        AirplaneTicketPassengerInfoGetTitle(airplane);
                        break;
                    case "nationalcode":
                        msgToSend = "کد ملی مسافر را وارد نمایید";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "nationalcode", id.ToString());
                        break;

                    case "fname":
                        msgToSend = "لطفا نام کوچک مسافر را و بدون فاصله (به هم چسبیده) به انگلیسی وارد نمایید";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "fname", id.ToString());
                        break;
                    case "lname":
                        msgToSend = "لطفا نام فامیل مسافر را به انگلیسی و بدون فاصله (به هم چسبیده) وارد نمایید";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "lname", id.ToString());
                        break;
                    case "dateofbirth":
                        msgToSend = "لطفا تاریخ تولد را بصورت 1347/10/28  وارد نمایید.";
                        currentAction.set(SimpaySectionEnum.AirplaneTicket, "dateofbirth", id.ToString());
                        //AirplaneTicketShowDateOfBirth(theDate: null, caption: msgToSend, extraInfo: $"{SimpaySectionEnum.AirplaneTicket}|dob|{id}", forceNewWindow: true);
                        break;
                    case "endpassenger":
                        doSendMessage = false;
                        AirplaneTicketEndOfPassengerInfo(airplane);
                        break;

                    // end of passeneger info
                    case "reserveticket":
                        AirplaneReserveTicket(airplane);
                        break;



                    default:
                        break;
                }


            } while (false);
            if (doSendMessage && !String.IsNullOrEmpty(msgToSend))
                telegramAPI.send(msgToSend);

        }

        private void AirplaneTicketUpdateInfo(string field, dynamic value, long id, bool forceNewWindow = false)
        {
            var nextStepField = "";
            var airplane = new AirplaneTicket.Manager(chatId, id);
            var msgToSend = "";
            dynamic stepValue = null;


            do
            {
                switch (field.ToLower())
                {
                    case "sairport":
                        var sAirportData = (AirplaneTicket.AirportData)value;
                        airplane.data.sourceAirportCode = sAirportData.airportCode;
                        airplane.data.sourceAirportShowName = sAirportData.airportShowName;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;
                        nextStepField = "dairport";

                        msgToSend = $"مبدا انتخابی: {airplane.data.sourceAirportShowName}";
                        currentAction.remove();

                        break;
                    case "dairport":
                        var dAirportData = (AirplaneTicket.AirportData)value;
                        airplane.data.destinationAirportCode = dAirportData.airportCode;
                        airplane.data.destinationAirportShowName = dAirportData.airportShowName;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;
                        nextStepField = "adult";

                        msgToSend = $"مقصد انتخابی: {airplane.data.destinationAirportShowName}";
                        currentAction.remove();
                        break;

                    case "adult":
                        var adultCount = (int)value;
                        airplane.data.adultCount = adultCount;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;
                        nextStepField = "child";

                        msgToSend = $"تعداد مسافر(ان) بزرگسال: {airplane.data.adultCount}";
                        currentAction.remove();
                        break;
                    case "child":
                        var childCount = (int)value;
                        airplane.data.childCount = childCount;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;
                        nextStepField = "infant";

                        msgToSend = $"تعداد مسافر(ان) کودک: {airplane.data.childCount}";
                        currentAction.remove();
                        break;

                    case "infant":
                        var infantCount = (int)value;
                        airplane.data.infantCount = infantCount;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;


                        if (airplane.seatCount == 0)
                        {
                            msgToSend = "تعداد مسافرها مشخص نشده است!";
                            nextStepField = "adult";
                            break;

                        }

                        nextStepField = "gdate";

                        msgToSend = $"تعداد مسافر(ان) نوزاد: {airplane.data.infantCount}";
                        currentAction.remove();
                        break;
                    case "gdate":
                        var fcGo = new FarsiCalendar((string)value);
                        if (String.Compare(fcGo.gDate.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd")) < 0)
                        {
                            msgToSend = "تاریخ باید بزرگتر از امروز باشد";
                            nextStepField = "gdate";
                            break;
                        }
                        airplane.data.wayGoDateTime = fcGo.gDate;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;

                        nextStepField = "hasreturn";
                        msgToSend = $"تاریخ رفت انتخابی: {fcGo.pDate}";
                        break;
                    case "hasreturn":
                        var answer = (int)value;
                        if (answer == 1)
                        {
                            airplane.data.twoWay = true;
                            msgToSend = "بلیط برگشتی دارد";
                            nextStepField = "rdate";
                        }
                        else
                        {
                            airplane.data.twoWay = false;
                            msgToSend = "بلیط برگشتی ندارد";
                            nextStepField = "search";
                        }
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;

                        break;
                    case "rdate":
                        var fcReturn = new FarsiCalendar((string)value);

                        if (String.Compare(fcReturn.gDate.ToString("yyyy/MM/dd"), Convert.ToDateTime(airplane.data.wayGoDateTime).ToString("yyyy/MM/dd")) < 0)
                        {
                            //msgToSend = "تاریخ باید بزرگتر از امروز باشد";
                            msgToSend = "تاریخ برگشت باید مساوی یا بزرگتر از تاریخ رفت باشد. ";
                            nextStepField = "rdate";
                            break;
                        }

                        airplane.data.wayReturnDateTime = fcReturn.gDate;
                        airplane.setInfo();
                        if (id == 0)
                            id = airplane.data.id;

                        nextStepField = "search";

                        msgToSend = $"تاریخ برگشت انتخابی: {fcReturn.pDate}";
                        break;
                    case "search":
                        break;

                    case "gselect":
                        airplane.data.goRow = (int)value;
                        airplane.setInfo();
                        var gFlightNumber = airplane.serviceGo.data[airplane.data.goRow - 1].flightNumber;
                        var gDepartureTime = airplane.serviceGo.data[airplane.data.goRow - 1].departureDateTime;
                        msgToSend = $"پرواز انتخابی: {gFlightNumber} ساعت {Convert.ToDateTime(gDepartureTime).ToString("HH:mm")}";
                        if (airplane.data.twoWay)
                        {
                            nextStepField = "rdaytimesummary";
                        }
                        else
                        {
                            nextStepField = "passengerinfo";
                        }
                        break;
                    case "rselect":
                        airplane.data.returnRow = (int)value;
                        airplane.setInfo();
                        var rFlightNumber = airplane.serviceGo.data[airplane.data.returnRow - 1].flightNumber;
                        var rDepartureTime = airplane.serviceGo.data[airplane.data.returnRow - 1].departureDateTime;
                        msgToSend = $"پرواز انتخابی: {rFlightNumber} ساعت {Convert.ToDateTime(rDepartureTime).ToString("HH:mm")}";
                        nextStepField = "passengerinfo";
                        break;
                    case "passtype":
                        var passTypeId = (string)value;
                        var passTypeName = airplane.GetPassengerTypeShowName(passTypeId);

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].passengerTypeCode = passTypeId;
                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].passengerTypeShowName = passTypeName;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);

                        msgToSend = $"نوع مسافر انتخابی: {passTypeName}";
                        nextStepField = "title";
                        currentAction.remove();
                        break;
                    case "title":
                        var title = (string)value;

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].title = title;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);

                        msgToSend = $"جنسیت مسافر انتخابی: {airplane.GetPassengerTitleShowName(value)}";
                        nextStepField = "nationalcode";
                        currentAction.remove();
                        break;

                    case "nationalcode":
                        var nationalCode = (string)value;

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].nationalCode = nationalCode;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);
                        msgToSend = $"شماره کد ملی وارد شده: {nationalCode}";

                        nextStepField = "fname";

                        currentAction.remove();
                        break;
                    case "fname":
                        var fname = (string)value;

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].firstName = fname;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);
                        msgToSend = $"نام کوچک وارد شده: {fname}";

                        nextStepField = "lname";

                        currentAction.remove();
                        break;
                    case "lname":
                        var lname = (string)value;

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].lastName = lname;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);
                        msgToSend = $"نام فامیل وارد شده: {lname}";

                        nextStepField = "dateofbirth";

                        currentAction.remove();
                        break;

                    case "dateofbirth":
                        var shamsiDateOfBirth = (string)value;
                        var fcdob = new FarsiCalendar(shamsiDateOfBirth);

                        airplane.passenger.data[airplane.data.currentPassengerRow - 1].dateOfBirth = fcdob.gDate;
                        airplane.passenger.setInfo(airplane.data.currentPassengerRow);
                        msgToSend = $"تاریخ تولد وارد شده: {fcdob.pDate}";

                        nextStepField = "endpassenger";
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

                AirplaneTicketRequestInfo(nextStepField, stepValue, id);
            }
        }

        private void AirplaneTicketVerifyUserEntryText(string field, dynamic value, string currentTicketId = "")
        {
            long id = 0;
            if (!string.IsNullOrEmpty(currentTicketId))
            {
                id = Convert.ToInt32(currentTicketId);
            }
            do
            {

                switch (field.ToLower())
                {
                    case "sairport":
                    case "dairport":
                        if (Utils.isItOnlyEnglishCharecter((string)value))
                        {
                            telegramAPI.send("خواهشمند است نام شهر ها را حتما به فارسی وارد نمایید");
                            break;
                        }

                        AirplaneTicketCheckAirportName(field, (string)value, id);

                        break;
                    case "adult":
                        var adultCountValue = value;
                        if (!Utils.isInteger(adultCountValue))
                        {
                            telegramAPI.send("این مقدار باید عددی باشد");
                            break;
                        }
                        AirplaneTicketUpdateInfo(field, Convert.ToInt32(value), id);
                        break;
                    case "child":
                        var childCountValue = value;
                        if (!Utils.isInteger(childCountValue))
                        {
                            telegramAPI.send("این مقدار باید عددی باشد");
                            break;
                        }
                        AirplaneTicketUpdateInfo(field, Convert.ToInt32(value), id);
                        break;
                    case "infant":
                        var infantCountValue = value;
                        if (!Utils.isInteger(infantCountValue))
                        {
                            telegramAPI.send("این مقدار باید عددی باشد");
                            break;
                        }
                        AirplaneTicketUpdateInfo(field, Convert.ToInt32(value), id);
                        break;

                    case "nationalcode":
                        var nationalCode = (string)value;
                        if (!Utils.IsValidNationalCode(nationalCode))
                        {
                            telegramAPI.send("کد ملی درست وارد نشده است. لطفا یکبار دیگه کد ملی را بصورت یک عدد 10 رقمی بدون خط فاصله وارد نمایید.");
                            break;
                        }
                        AirplaneTicketUpdateInfo(field, value, id);
                        break;
                    case "gdate":
                    case "rdate":
                    case "dateofbirth":
                        var tmpFC = new FarsiCalendar(value);
                        if (tmpFC.hasError)
                        {
                            telegramAPI.send("لطفا تاریخ را بصورت مثال وارد نمایید: مثال: 1372/02/05");
                            break;

                        }
                        AirplaneTicketUpdateInfo(field, value, id);
                        break;


                    case "fname":
                    case "lname":
                        if (!Utils.isItOnlyEnglishCharecter((string)value))
                        {
                            telegramAPI.send("خواهشمند است  فقط به انگلیسی وارد نمایید");
                            break;
                        }

                        AirplaneTicketUpdateInfo(field, value, id);

                        break;

                    default:
                        AirplaneTicketUpdateInfo(field, value, id);
                        break;
                }

            } while (false);

        }

        private void AirplaneTicketCheckAirportName(string field, string stationKeyword, long id)
        {
            (new AirplaneTicket.Manager(chatId)).updateAirportList();
            var airport = new AirplaneTicket.Airport();
            var airportCode = "";
            var airportName = "";
            var showList = false;
            do
            {
                airport.setDataList(keyword: stationKeyword);
                var airportList = airport.data;
                if (airportList.Count > 1) // if found more than one station
                {
                    showList = true;
                }
                else if (airportList.Count == 0)
                {
                    showList = true;
                    airport.setDataList();
                }
                // stationList.Count ==1 ' so found the station
                airportCode = airportList[0].airportCode;
                airportName = airportList[0].airportShowName;


            } while (false);
            switch (field.ToLower())
            {
                case "sairport":
                    if (showList)
                        AirplaneTicketShowSourceAirport(airport.data, id);
                    else
                        AirplaneTicketUpdateInfo(field,
                                                new AirplaneTicket.AirportData { airportCode = airportCode, airportShowName = airportName },
                                                id);

                    break;
                case "dairport":
                    if (showList)
                        AirplaneTicketShowDestinationAirport(airport.data, id);
                    else
                        AirplaneTicketUpdateInfo(field,
                                                new AirplaneTicket.AirportData { airportCode = airportCode, airportShowName = airportName },
                                                id);

                    break;

                default:
                    break;
            }


        }

        private void AirplaneTicketShowSourceAirport(List<AirplaneTicket.AirportData> list, long id = 0)
        {
            do
            {

                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = AirplaneTicketGetListOfAirportButtons(list, "sairport", id);

                var msgToSend = "لطفا از مبدا را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);


        }
        private void AirplaneTicketShowDestinationAirport(List<AirplaneTicket.AirportData> list, long id = 0)
        {
            do
            {

                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه اطلاعاتی برای ادامه یافت نشد.");
                    break;
                }


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = AirplaneTicketGetListOfAirportButtons(list, "dairport", id);

                var msgToSend = "لطفا از مقصد را از میان فهرست زیر انتخاب نمایید.";

                telegramAPI.send(msgToSend, markup);

            } while (false);


        }

        private InlineKeyboardButton[][] AirplaneTicketGetListOfAirportButtons(List<AirplaneTicket.AirportData> list, string action, long id)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var listIndex = 0;
            var colIndex = 0;
            var maxCol = 2;
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
                        btnText = $"{list[listIndex].airportShowName}";
                        btnData = $"{SimpaySectionEnum.AirplaneTicket}:{action}:{id}:{list[listIndex].airportCode}";
                    }
                    else
                    {
                        btnText = $".";
                        btnData = $"{SimpaySectionEnum.AirplaneTicket}:blank:{id}:0";

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

        private void AirplaneTicketCallBack(string data)
        {
            var action = data.Split(':')[1];
            var id = Convert.ToInt32(data.Split(':')[2]);
            var msgToSend = "";
            var airport = new AirplaneTicket.Airport();
            do
            {
                switch (action)
                {
                    case "sairport":
                        var sAirportCode = data.Split(':')[3];
                        airport.setDataList(code: sAirportCode);

                        AirplaneTicketUpdateInfo(action, airport.data[0], id);

                        msgToSend = $"مبدا انتخابی: {airport.data[0].airportShowName} ";
                        break;
                    case "dairport":
                        var dAirportCode = data.Split(':')[3];
                        airport.setDataList(code: dAirportCode);

                        AirplaneTicketUpdateInfo(action, airport.data[0], id);

                        msgToSend = $"مقصد انتخابی: {airport.data[0].airportShowName} ";
                        break;
                    case "hasreturn":
                        var answerHasRetrun = Convert.ToInt16(data.Split(':')[3]);

                        if (answerHasRetrun == 1 || answerHasRetrun == 0)// YES
                        {
                            AirplaneTicketUpdateInfo(action, answerHasRetrun, id);
                        }
                        break;

                    case "gpg":
                        var dayTimeIdPageGo = Convert.ToInt32(Convert.ToInt32(data.Split(':')[3]));
                        var servicePageGo = Convert.ToInt32(Convert.ToInt32(data.Split(':')[4]));
                        AirplaneTicketShowServiceDayTimeTotal("go", id, dayTimeIdPageGo, servicePageGo);
                        break;
                    case "rpg":
                        var dayTimeIdPageReturn = Convert.ToInt32(Convert.ToInt32(data.Split(':')[3]));
                        var servicePageReturn = Convert.ToInt32(Convert.ToInt32(data.Split(':')[4]));
                        AirplaneTicketShowServiceDayTimeTotal("return", id, dayTimeIdPageReturn, servicePageReturn);
                        break;
                    case "gdaytime":
                        var gdayTimeId = Convert.ToInt32(data.Split(':')[3]);
                        AirplaneTicketShowServiceDayTimeTotal("go", id, gdayTimeId, 1, true);
                        break;
                    case "rdaytime":
                        var rdayTimeId = Convert.ToInt32(data.Split(':')[3]);
                        AirplaneTicketShowServiceDayTimeTotal("return", id, rdayTimeId, 1, true);
                        break;
                    case "gselect":
                        var gRow = Convert.ToInt16(data.Split(':')[3]);
                        AirplaneTicketUpdateInfo(action, gRow, id);
                        break;
                    case "rselect":
                        var rRow = Convert.ToInt16(data.Split(':')[3]);
                        AirplaneTicketUpdateInfo(action, rRow, id);
                        break;
                    case "passtype":
                        var passtype = data.Split(':')[3];
                        AirplaneTicketUpdateInfo(action, passtype, id);
                        break;
                    case "title":
                        var title = data.Split(':')[3];
                        AirplaneTicketUpdateInfo(action, title, id);
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
        private void AirplaneTicketShowDateTime(string theDate = null, string caption = "", string exairplanefo = "", bool forceNewWindow = false)
        {

            telegramAPI.send(caption);

            Calendar(theDate, exairplanefo, forceNewWindow, CalendarNavigationSwitchEnum.ChangeMonth);


        }

        private void AirplaneTicketHasReturnRequest(long id = 0)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            colK.Add(new InlineKeyboardButton()
            {
                Text = "بله",
                CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:hasreturn:{id}:1"
            });
            colK.Add(new InlineKeyboardButton()
            {
                Text = "خیر",
                CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:hasreturn:{id}:0"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();


            telegramAPI.send("آیا مایل به خرید بلیط برگشت نیز میباشید؟", markup);

        }

        public void AirplaneTicketSearchServices(AirplaneTicket.Manager airplane)
        {
            telegramAPI.send(" در حال جستجوی سرویسها با توجه به اطلاعات فوق ");
            do
            {
                airplane.getServices();
                if (airplane.resultAction.hasError)
                {
                    telegramAPI.send(airplane.resultAction.message);
                    break;
                }
                if (airplane.data.twoWay)
                {
                    if (!airplane.hasServiceGo && !airplane.hasServiceReturn)
                    {
                        //airplaneTicketChangeFieldNavigate("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.", "sstation,dstation,gdate", id);
                        telegramAPI.send("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.");
                        break;
                    }
                    if (!airplane.hasServiceGo && airplane.hasServiceReturn)
                    {
                        //airplaneTicketChangeFieldNavigate("هیچ سرویسی برای مسیر رفت یافت نشد.", "sstation,dstation,gdate", id);
                        telegramAPI.send("هیچ سرویسی برای مسیر رفت یافت نشد.");
                        break;
                    }
                    if (airplane.hasServiceGo && !airplane.hasServiceReturn)
                    {
                        //airplaneTicketChangeFieldNavigate("هیچ سرویسی برای مسیر برگشت یافت نشد.", "sstation,dstation,rdate", id);
                        telegramAPI.send("هیچ سرویسی برای مسیر برگشت یافت نشد.");
                        break;
                    }
                }
                else
                {
                    if (!airplane.hasServiceGo)
                    {
                        //airplaneTicketChangeFieldNavigate("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.", "gdate", id);
                        telegramAPI.send("هیچ سرویسی برای این مسیر و تاریخ یافت نشد.");
                        break;
                    }
                }
                AirplaneTicketRequestInfo("gdaytimesummary", null, airplane.data.id);


            } while (false);

        }
        private void AirplaneTicketShowGoServiceDayTimeSummary(AirplaneTicket.Manager airplane)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var msgToSend = "";
            do
            {
                var serviceGo = airplane.serviceGo;

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
                        CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:gdaytime:{airplane.data.id}:{summary[i].dayTimeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();



                telegramAPI.send(msgToSend, markup);

            } while (false);

        }
        private void AirplaneTicketShowReturnServiceDayTimeSummary(AirplaneTicket.Manager airplane)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var msgToSend = "";
            do
            {
                var serviceGo = airplane.serviceReturn;

                var summary = serviceGo.getDayTimeSummary();
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
                        CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:rdaytime:{airplane.data.id}:{summary[i].dayTimeId}"
                    });
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                }



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();



                telegramAPI.send(msgToSend, markup);

            } while (false);

        }

        private void AirplaneTicketShowServiceDayTimeTotal(string way, long id, int dayTimeId, int page = 1, bool forceNewWindow = false)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            var airplane = new AirplaneTicket.Manager(chatId, id);

            var msgToSend = "";

            do
            {
                var maxPage = 0;
                var callbackAction = "";
                var paginAction = "";
                var service = new AirplaneTicket.Service(id, way);
                if (way == "return")
                {
                    callbackAction = "rselect";
                    paginAction = "rpg";
                    service = airplane.serviceReturn;
                }
                else if (way == "go")
                {
                    callbackAction = "gselect";
                    paginAction = "gpg";
                    service = airplane.serviceGo;
                }
                var serviceData = service.getByDayTime(dayTimeId, page, out maxPage);
                var fc = new FarsiCalendar(Convert.ToDateTime(serviceData.departureDateTime));
                var amount = serviceData.amountAdult * airplane.data.adultCount + serviceData.amountChild * airplane.data.childCount + serviceData.amountInfant * airplane.data.infantCount;

                var isCharter = serviceData.isCharter ? "بله " : " خیر ";
                msgToSend += " \n ";
                //msgToSend += $" شناسه این سرویس: /bts{id}_{serviceData.row}  \n";

                msgToSend += $"شرکت :  {serviceData.airlineCode}  \n \n";
                msgToSend += $" نام هواپیما: {serviceData.aircraft}  \n \n";

                msgToSend += $" کلاس: {serviceData.classTypeName}  \n \n";


                msgToSend += " تاریخ و ساعت حرکت: \n ";
                msgToSend += $"{fc.pDate }  \n \n ";
                msgToSend += $" بهای بلیط : {amount.ToString("#,##")} ریال \n \n ";
                msgToSend += $" چارتر: {isCharter }  \n \n";
                msgToSend += $" موجودی سهمیه: {serviceData.statusName}  \n \n";

                msgToSend += " \n ";
                msgToSend += " \n -";
                var buttonExAirplanefo = $"{SimpaySectionEnum.AirplaneTicket}:{paginAction}:{id}:{dayTimeId}";
                var paging = paginButtons(5, page, maxPage, buttonExAirplanefo);

                if (paging != null)
                    inlineK.Add(paging);


                colK.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب",
                    CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:{callbackAction}:{id}:{serviceData.row}"

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

        private void AirplaneTicketSendPassengerInfoMessage(AirplaneTicket.Manager airplane)
        {

            var message = "";
            do
            {
                var seatCount = airplane.data.adultCount + airplane.data.childCount + airplane.data.infantCount;
                if (seatCount > 1)
                {
                    message = $"در این مرحله لطفا اطلاعات شناسنامه ای {seatCount} مسافر را وارد نمایید:";
                    message = " \n \n ";

                }
                else if (seatCount == 1)
                {
                    message = "در این مرحله لطفا اطلاعات شناسنامه ای مسافر را وارد نمایید:";
                }
                airplane.data.currentPassengerRow = 1;
                airplane.setInfo();
                telegramAPI.send(message);
                AirplaneTicketRequestInfo("passtype", null, airplane.data.id);

            } while (false);
        }

        private void AirplaneTicketPassengerInfoGetType(AirplaneTicket.Manager airplane)
        {


            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var selectionCount = 0;
            var lastSelectedType = "";


            if (airplane.data.adultCount != 0)
            {
                selectionCount++;
                lastSelectedType = "adl";
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $"بزرگسال",
                    CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:passtype:{airplane.data.id}:adl"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }


            if (airplane.data.childCount != 0)
            {
                selectionCount++;
                lastSelectedType = "chd";
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "کودک",
                    CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:passtype:{airplane.data.id}:chd"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

            }
            if (airplane.data.infantCount != 0)
            {
                selectionCount++;
                lastSelectedType = "inf";
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "نوزاد",
                    CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:passtype:{airplane.data.id}:inf"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }
            if (selectionCount == 1)
            {
                AirplaneTicketUpdateInfo("passtype", lastSelectedType, airplane.data.id, true);
            }
            else
            {
                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send("لطفا ابتدا نوع مسافر را انتخاب کنید: ", markup);
            }
        }

        private void AirplaneTicketPassengerInfoGetTitle(AirplaneTicket.Manager airplane)
        {


            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            colKey.Add(new InlineKeyboardButton()
            {
                Text = airplane.GetPassengerTitleShowName("MR"),
                CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:title:{airplane.data.id}:MR"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            colKey.Add(new InlineKeyboardButton()
            {
                Text = airplane.GetPassengerTitleShowName("MS"),
                CallbackData = $"{SimpaySectionEnum.AirplaneTicket}:title:{airplane.data.id}:MS"
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("جنسیت مسافر را انتخاب کنید: ", markup);
        }

        private void AirplaneTicketShowDateOfBirth(string theDate = null, string caption = "", string extraInfo = "", bool forceNewWindow = false)
        {

            telegramAPI.send(caption);

            Calendar(theDate, extraInfo, forceNewWindow, CalendarNavigationSwitchEnum.SelectYears | CalendarNavigationSwitchEnum.SelectMonths);


        }

        private void AirplaneTicketEndOfPassengerInfo(AirplaneTicket.Manager airplane)
        {


            if (airplane.seatCount > airplane.data.currentPassengerRow)
            {
                airplane.data.currentPassengerRow++;
                airplane.setInfo();
                telegramAPI.send($"حال اطلاعات مسافر {airplane.data.currentPassengerRow} ام را وارد نمایید ");
                AirplaneTicketRequestInfo("passtype", null, airplane.data.id);
            }
            else
            {
                AirplaneTicketRequestInfo("reserveticket", null, airplane.data.id);
            }

        }

        private void AirplaneReserveTicket(AirplaneTicket.Manager airplane)
        {
            telegramAPI.send("هم اکنون سامانه در حال رزرو بلیط(ها) میباشد. لطفا صبر نمایید.");
            do
            {
                airplane.ReserveTicket();
                if (airplane.resultAction.hasError)
                {
                    telegramAPI.send(airplane.resultAction.message);
                    currentAction.remove();
                    break;
                }

                var serviceGoRow = airplane.data.goRow;
                var serviceReturnRow = airplane.data.returnRow;


                PaymentStartProcess(airplane.data.saleKey);


                //var msg = "";
                //msg += "اطلاعات بلیط درخواستی: \n\n";
                //msg += $"مبدا: {airplane.data.sourceAirportShowName} \n ";
                //msg += $"مقصد: {airplane.data.destinationAirportShowName} \n ";
                //msg += $": مبلغ کل{airplane.data.amount.ToString("#,##")} \n ";
                //msg += "پرواز رفت: .............. \n";
                //var serviceGo = airplane.serviceGo.getService(serviceGoRow);
                //var fcG = new FarsiCalendar(Convert.ToDateTime(serviceGo.departureDateTime));
                //msg += $"شرکت هواپیمائی: {serviceGo.airlineCode} \n ";
                //msg += $"نوع هواپیما: {serviceGo.aircraft} \n ";
                //msg += $"شماره پرواز: {serviceGo.flightNumber} \n ";
                //msg += $"تاریخ: {fcG.pDate} \n ";
                //msg += $"کلاس پرواز: {serviceGo.classTypeName} \n ";

                //if (serviceReturnRow != 0)
                //{
                //    msg += "پرواز برگشت: .............. \n";
                //    var serviceReturn = airplane.serviceGo.getService(serviceGoRow);
                //    var fcR = new FarsiCalendar(Convert.ToDateTime(serviceGo.departureDateTime));
                //    msg += $"شرکت هواپیمائی: {serviceReturn.airlineCode} \n ";
                //    msg += $"نوع هواپیما: {serviceReturn.aircraft} \n ";
                //    msg += $"شماره پرواز: {serviceReturn.flightNumber} \n ";
                //    msg += $"تاریخ: {fcR.pDate} \n ";
                //    msg += $"کلاس پرواز: {serviceReturn.classTypeName} \n ";
                //}
                //msg += "اطلاعات مسافرین: .............. \n";
                //foreach (var info in airplane.passenger.data)
                //{
                //    msg += $"نام مسافر (انگلیسی): {info.title} {info.firstName.ToUpper()} {info.lastName.ToUpper()} \n ";
                //    msg += $"کد ملی: {info.nationalCode}  \n ";
                //    msg += $"نوع مسافر: {info.passengerTypeShowName}  \n ";
                //}

                //msg += "\n  ";
                //msg += "\n  ";
                //msg += "لطفا در صورت تایید اطلاعات فوق، با زدن دکمه زیر به صفحه بانک بروید.  ";


                //var resultLink = SimpayCore.getPaymentLink(airplane.data.saleKey);
                //if (SimpayCore.resultAction.hasError)
                //{
                //    telegramAPI.send(SimpayCore.resultAction.message);
                //    break;
                //}
                //sendPaymentMessage(resultLink, msg);



            } while (false);
        }
        private void AirplaneRedeemTicket(string saleKey)
        {
            var msgToSend = "";

            do
            {
                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                var airplane = new AirplaneTicket.Manager(chatId);
                airplane.Redeem(saleKey);
                if (airplane.resultAction.hasError)
                {
                    sendMenu(message: airplane.resultAction.message);
                    break;
                }

                msgToSend = "بلیط(های) شما صادر گردید. لطفا از لینکهای زیر جهت مشاهده و چاپ بلیط(ها) استفاده نمایید:";

                foreach (var item in airplane.passenger.data)
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = $" رفت {item.firstName.ToUpper()} {item.lastName.ToUpper()} ",
                        Url = item.htmlGo
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }
                if (airplane.data.twoWay)
                {
                    msgToSend += "\n\n";
                    foreach (var item in airplane.passenger.data)
                    {
                        colKey.Add(new InlineKeyboardButton()
                        {
                            Text = $" رفت {item.firstName.ToUpper()} {item.lastName.ToUpper()} ",
                            Url = item.htmlReturn
                        });
                        inlineK.Add(colKey.ToArray());
                        colKey.Clear();
                    }
                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                telegramAPI.send(msgToSend, markup);



                break;

            } while (false);
        }

    }
}