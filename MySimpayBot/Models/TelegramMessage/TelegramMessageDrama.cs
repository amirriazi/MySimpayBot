using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {

        private void DramaRequestInfo(string fieldId = "", dynamic value = null, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var showMessage = false;
            var msg = "";
            //telegramAPI.send("در این بخش شما میتوانید بلیط نمایش یا فیلم مورد نظر خود را تهیه فرمایید. ", cancelButton());
            var drama = new EventsTicket.Manager();
            do
            {
                if (String.IsNullOrEmpty(fieldId))
                {
                    telegramAPI.send("لطفا تا دریافت اطلاعات از سرویس دهنده صبر فرمایید.", cancelButton());
                    drama = new EventsTicket.Manager(chatId, "drama");
                    //drama = new EventsTicket.Manager(chatId, "sampleEvent");
                    drama.setInfo();
                    id = drama.data.id;
                    fieldId = "event";
                    drama.GetEventsList();

                }
                else if (id != 0)
                {
                    drama = new EventsTicket.Manager(id);
                }

                switch (fieldId.ToLower())
                {
                    case "event":
                        DramaShowEvents(drama);
                        break;
                    case "instance":
                        DramaCreateEventDetail(drama);
                        break;
                    case "seatmap":
                        DramaGetEventInstanceSeatMap(drama);
                        break;

                    case "seat":
                        currentAction.set(SimpaySectionEnum.Drama, fieldId, id.ToString());
                        msg = "لطفا تعداد صندلی مورد نیاز را وارد نمایید:";
                        showMessage = true;
                        break;

                    case "fullname":
                        currentAction.set(SimpaySectionEnum.Drama, fieldId, id.ToString());
                        msg = "لطفا نام کامل خود را وارد نمایید:";
                        showMessage = true;
                        break;
                    case "email":
                        currentAction.set(SimpaySectionEnum.Drama, fieldId, id.ToString());
                        msg = "لطفا آدرس ایمیل خود را وارد نمایید:";
                        showMessage = true;
                        break;
                    case "reserveticket":
                        DramaReserveTicket(drama);
                        break;
                    case "query":
                        currentAction.set(SimpaySectionEnum.Drama, fieldId, id.ToString());
                        msg = "بخشی از نام برنامه یا موضوع آن را وارد نمایید::";
                        showMessage = true;
                        break;
                    case "print":

                        break;



                    default:
                        break;
                }

            } while (false);

            if (showMessage)
            {
                telegramAPI.send(msg);
            }

        }
        private void DramaEntry(string field, string value, long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var drama = new EventsTicket.Manager(id);
            do
            {
                switch (field.ToLower())
                {
                    case "seat":
                        var seat = (string)value;
                        if (!Utils.isInteger(seat))
                        {
                            telegramAPI.send("مقدار عددی وارد نمایید.");
                            break;
                        }
                        DramaUpdateInfo(drama, field, Convert.ToInt32(seat));
                        break;

                    case "fullname":
                        var fullName = (string)value;
                        if (fullName.Length < 5)
                        {
                            telegramAPI.send("لطفا نام خود را درست وارد نمایید.");
                            break;
                        }
                        if (fullName.Split(' ').Length < 2)
                        {
                            telegramAPI.send("لطفا نام خود را درست وارد نمایید.");
                            break;
                        }
                        DramaUpdateInfo(drama, field, (string)value);
                        break;
                    case "email":
                        var email = (string)value;
                        if (!Utils.IsValidEmail(email))
                        {
                            telegramAPI.send("آدرس ایمیل ارسالی درست نیست لطفا مجددا سعی نمایید.");
                            break;
                        }
                        DramaUpdateInfo(drama, field, (string)value);
                        break;
                    case "query":
                        var query = (string)value;
                        if (query.Length < 3)
                        {
                            telegramAPI.send("لطفا کلمات بیشتری را برای جستجو وارد نمایید!");
                            break;
                        }
                        DramaUpdateInfo(drama, field, (string)value);
                        break;
                    default:
                        break;
                }

            } while (false);

        }
        private void DramaUpdateInfo(EventsTicket.Manager drama, string field, dynamic value = null, bool forceNewWindow = false)
        {
            var nextStepField = "";
            var msgToSend = "";
            var id = drama.data.id;
            dynamic stepValue = null;

            do
            {
                switch (field.ToLower())
                {
                    case "event":
                        var eventUID = (string)value;
                        drama.data.eventUID = eventUID;
                        drama.setInfo();
                        nextStepField = "instance";

                        break;
                    case "instance":
                        var instanceUID = (string)value;
                        drama.data.instanceUID = instanceUID;
                        drama.setInfo();
                        if (drama.data.eventMethod == "event_seat")
                        {
                            nextStepField = "seatmap";
                        }
                        else if (drama.data.eventMethod == "event")
                        {
                            nextStepField = "seat";
                        }

                        break;
                    case "seatmap":
                        var seatArray = (string)value;
                        drama.data.seats = seatArray;
                        drama.setInfo();
                        nextStepField = "fullname";
                        break;

                    case "seat":
                        var seats = "";
                        for (int i = 1; i <= (int)value; i++)
                        {
                            seats += (String.IsNullOrEmpty(seats)) ? "" : ",";
                            seats += $"{i}";
                        }
                        drama.data.seats = seats;
                        drama.setInfo();
                        nextStepField = "fullname";
                        break;
                    case "fullname":
                        var fullName = (string)value;
                        drama.data.fullName = fullName;
                        drama.setInfo();
                        nextStepField = "email";
                        break;

                    case "email":
                        var email = (string)value;
                        drama.data.emailAddress = email;
                        drama.setInfo();
                        nextStepField = "reserveticket";
                        break;
                    case "query":
                        var query = (string)value;
                        drama.data.query = query;
                        drama.setInfo();
                        nextStepField = "";
                        DramaShowEvents(drama, 1);
                        break;
                    case "queryremove":
                        drama.data.query = "";
                        drama.setInfo();
                        nextStepField = "";
                        DramaShowEvents(drama, 1);
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

                DramaRequestInfo(nextStepField, stepValue, id);
            }

        }
        private void DramaCallBackQuery(string data)
        {


            var action = data.Split(':')[1];
            var id = Convert.ToInt64(data.Split(':')[2]);
            var drama = new EventsTicket.Manager(id);
            do
            {

                switch (action.ToLower())
                {
                    case "eventlist":
                        DramaShowEventList(drama);
                        break;

                    case "page":
                        var pageNumber = Convert.ToInt32(data.Split(':')[3]);
                        DramaShowEvents(drama, pageNumber);
                        break;
                    case "evntsel":
                        var eventUID = data.Split(':')[3];
                        DramaUpdateInfo(drama, "event", (string)eventUID);
                        break;
                    case "instsel":
                        var instanceUID = data.Split(':')[3];
                        DramaUpdateInfo(drama, "instance", (string)instanceUID);
                        break;


                    case "edpage":
                        var instancePage = Convert.ToInt32(data.Split(':')[3]);
                        DramaShowEventDetail(drama, instancePage, false);
                        break;
                    case "search":
                        DramaRequestInfo("query", "", id);
                        break;
                    case "showall":
                        DramaUpdateInfo(drama, "queryremove", "");
                        break;


                    default:
                        break;
                }
            } while (false);



        }


        private void DramaShowEvents(EventsTicket.Manager drama, int pageNumber = 1, bool forceNewMessage = false)
        {
            do
            {
                var id = drama.data.id;
                var eventPaging = drama.getEventInfo(pageNumber);
                if (eventPaging.maxRecord == 0)
                {
                    sendMenu(message: " متاسفانه برنامه ای در سامانه یافت نشد.");
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                if (String.IsNullOrEmpty(drama.data.query))
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = "جستجو",
                        CallbackData = $"{SimpaySectionEnum.Drama}:search:{id}"
                    });

                }
                else
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = "نمایش کل",
                        CallbackData = $"{SimpaySectionEnum.Drama}:showall:{id}"
                    });

                }
                inlineK.Add(colKey.ToArray());
                colKey.Clear();


                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب",
                    CallbackData = $"{SimpaySectionEnum.Drama}:evntsel:{id}:{eventPaging.data[0].uniqueIdentifier}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();



                var callBackExtra = $"{SimpaySectionEnum.Drama}:page:{id}";
                var paging = paginButtons(6, pageNumber, eventPaging.maxRecord, callBackExtra);

                if (paging != null)
                    inlineK.Add(paging);



                var eventMsg = "";

                var fcStartDate = new FarsiCalendar(eventPaging.data[0].startDate);
                var fcEndDate = new FarsiCalendar(eventPaging.data[0].endDate);

                eventMsg += $"عنوان نمایش: {eventPaging.data[0].title} {Environment.NewLine}";
                eventMsg += $"توضیح: {eventPaging.data[0].shortDescription} {Environment.NewLine}";
                eventMsg += $"{Environment.NewLine} {Environment.NewLine}";
                eventMsg += $"تاریخ شروع اکران: {fcStartDate.pDate.Substring(0, 10)} {Environment.NewLine}";
                eventMsg += $"تاریخ پایان اکران: {fcEndDate.pDate.Substring(0, 10)} {Environment.NewLine}";
                eventMsg += $"{Environment.NewLine} ";
                eventMsg += $"ساعت نمایش: {eventPaging.data[0].timesText} {Environment.NewLine}";
                eventMsg += $"مکان نمایش: {eventPaging.data[0].venueTitle} {Environment.NewLine}";
                eventMsg += $"{Environment.NewLine} ";
                eventMsg += $"بها: {eventPaging.data[0].amountsText} {Environment.NewLine}";
                //eventMsg += $"نوع: {eventPaging.data[0].method} {Environment.NewLine}";

                eventMsg += $"{Environment.NewLine} {Environment.NewLine}";
                eventMsg += $"<a href='{eventPaging.data[0].imageThumbnailURL}'>.</a>";


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.parseMode = Telegram.Bot.Types.Enums.ParseMode.Html;
                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, eventMsg, markup);
                }
                else
                {
                    telegramAPI.send(eventMsg, markup);
                }



            } while (false);
        }
        private void DramaCreateEventDetail(EventsTicket.Manager drama)
        {
            do
            {
                var id = drama.data.id;
                drama.GetEventDetail();
                if (drama.resultAction.hasError)
                {
                    telegramAPI.send(drama.resultAction.message);
                    break;
                }
                DramaShowEventDetail(drama);

            } while (false);

        }
        private void DramaShowEventDetail(EventsTicket.Manager drama, int instancePage = 1, bool forceNewMessage = true)
        {
            do
            {
                var id = drama.data.id;
                var instanceIdx = instancePage - 1;

                var wsEventDetail = drama.GetEventDetailFromFile();
                if (wsEventDetail == null || wsEventDetail.Instances == null)
                {
                    telegramAPI.send("متاسفانه اطلاعات تکمیلی این برنامه جهت تهیه بلیط از سمت سرویس دهنده در دسترس نیست");
                    break;
                }

                drama.data.venueTitle = wsEventDetail.VenueTitle;
                drama.data.venueAddress = wsEventDetail.VenueAddress;
                drama.data.saleKey = wsEventDetail.SaleKey;
                drama.setInfo();


                var msg = "";
                var instances = wsEventDetail.Instances;
                msg += $"{instances[instanceIdx].Title} {Environment.NewLine}";
                msg += $"ظرفیت کل: {instances[instanceIdx].Capacity} {Environment.NewLine}";
                //msg += $"ظرفیت فعلی: {instances[instanceIdx].Remained} {instances[instanceIdx].RemainedText}  {Environment.NewLine}";
                msg += $"ظرفیت فعلی: {instances[instanceIdx].RemainedText}  {Environment.NewLine}";
                msg += $"مکان:{wsEventDetail.VenueTitle}{Environment.NewLine} {wsEventDetail.VenueAddress}{Environment.NewLine}";
                msg += $"تلفن:{wsEventDetail.VenueTelPhone}{Environment.NewLine} ";
                //eventMsg += $"بها: {wsEventDetail.AmountsText} {Environment.NewLine}";
                msg += $"{Environment.NewLine} {Environment.NewLine}";
                msg += $"<a href='{wsEventDetail.ImageURL}'>.</a>";


                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب صندلی",
                    CallbackData = $"{SimpaySectionEnum.Drama}:instsel:{id}:{instances[instanceIdx].UniqueIdentifier}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();


                if (instances.Length > 1)
                {
                    var callBackExtra = $"{SimpaySectionEnum.Drama}:edpage:{id}";
                    var paging = paginButtons(6, instancePage, instances.Length, callBackExtra);

                    if (paging != null)
                        inlineK.Add(paging);


                }

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.parseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msg, markup);
                }
                else
                {
                    telegramAPI.send(msg, markup);
                }


            } while (false);
        }

        private void DramaShowEventList(EventsTicket.Manager drama)
        {
            do
            {
                var id = drama.data.id;
                var eventPaging = drama.getEventInfo(1, 9999);
                if (eventPaging.maxRecord == 0)
                {
                    sendMenu(message: " متاسفانه برنامه ای در سامانه یافت نشد.");
                    break;
                }

                var msg = ":فهرست برنامه های موجود در زیر نمایش داده شده است. لطفا برای انتخاب هر برنامه روی لینک آبی رنگ کلیک نمایید.";
                msg += Environment.NewLine + Environment.NewLine;
                foreach (var info in eventPaging.data)
                {
                    msg += $"/ردیف_{info.row} - ";
                    msg += $"عنوان: {info.title}{Environment.NewLine}";
                    //msg += $"مکان: {info.title}{Environment.NewLine}";
                    //msg += $"بها: {info.amountsText}{Environment.NewLine}";
                    //msg += "  ---  ---  ---  ---  ---  ---  ---";
                    msg += $"{Environment.NewLine}";
                }
                telegramAPI.send(msg);
            } while (false);
        }

        private void DramaReserveTicket(EventsTicket.Manager drama)
        {
            do
            {
                drama.ReserveTicket();
                if (drama.resultAction.hasError)
                {
                    telegramAPI.send(drama.resultAction.message);
                    break;
                }

                PaymentStartProcess(drama.data.saleKey);
            } while (false);
        }

        private void DramaGetEventInstanceSeatMap(EventsTicket.Manager drama)
        {
            do
            {
                if (drama.data.eventMethod == "event")
                {
                    var id = drama.data.id;
                    drama.GetEventInstanceSeatMap();
                    if (drama.resultAction.hasError)
                    {
                        telegramAPI.send(drama.resultAction.message);
                        break;
                    }
                    DramaRequestInfo("seat", "", id);
                }
                else
                {
                    DramaShowSeatMapLink(drama);
                }



            } while (false);
        }

        private void DramaShowSeatMapLink(EventsTicket.Manager drama)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            colKey.Add(new InlineKeyboardButton()
            {
                Text = "مشاهده نقشه سالن",
                Url = $@"{ProjectValues.EventSansPlanUrl}/telegram/{SimpayCore.getSessionId()}/{drama.data.saleKey}/{drama.data.instanceUID}/{thisUser.chatId}/{drama.data.id}",
            });
            inlineK.Add(colKey.ToArray());
            colKey.Clear();
            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();

            telegramAPI.send("برای ملاحظه نقشه سالن اجرا و انتخاب صندلی دکمه زیر را فشار دهید. ", markup);
        }
        public void DramaEventSeatPlan(EventSeatPlanInput ws)
        {
            Log.Trace(Utils.ConvertClassToJson(ws), 0);

            chatId = ws.chatId;
            telegramAPI = new myTelegramApplication.TelegramAPI(ws.chatId);
            thisUser = new UserModel(ws.chatId);
            currentAction = new CurrentAction(ws.chatId);


            var seats = "";
            for (int i = 0; i < ws.seats.Length; i++)
            {
                seats += (String.IsNullOrEmpty(seats)) ? "" : ",";
                seats += $"{ws.seats[i]}";
            }

            var drama = new EventsTicket.Manager(ws.eventId);

            DramaUpdateInfo(drama, "seatmap", seats);

        }


    }
}