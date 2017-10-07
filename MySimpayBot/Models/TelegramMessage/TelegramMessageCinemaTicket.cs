using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using myTelegramApplication;

namespace Models
{
    public partial class TelegramMessage
    {
        //private static Manager cinemaManager = new Manager(chatId, 0);
        private void CinemaTicketRequestInfo(string action = "", dynamic currentValue = null, string currentId = "")
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var showMessage = false;
            var msg = "";
            long id = 0;
            //telegramAPI.send("در این بخش شما میتوانید بلیط نمایش یا فیلم مورد نظر خود را تهیه فرمایید. ", cancelButton());
            if (!string.IsNullOrEmpty(currentId))
            {

                id = Convert.ToInt32(currentId);
            }

            var cinemaManager = new CinemaTicket.Manager(chatId, id);

            do
            {
                if (action == "")
                {
                    currentAction.remove();
                    cinemaManager.setQuery("");
                    telegramAPI.send("درحال اتصال به سامانه سینما تیکت. لطفا شکیبا باشید.", cancelButton());
                    action = "filmcode";
                }
                switch (action.ToLower())
                {
                    case "filmquery":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "filmquery", id.ToString());
                        showMessage = true;
                        msg = "جهت جستجوی فیلم شما میتوانید بخشی از نام فیلم، بازیگران، کارگردان، موضوع فیلم و ... را وارد نمایید:";
                        break;
                    case "filmcode":
                        if (id == 0)
                        {
                            cinemaManager.setInfo();
                            id = cinemaManager.data.id;
                        }
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "filmcode", id.ToString());
                        CinemaTicketShowMovies(id: id);
                        break;
                    case "cinemacode":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "cinemacode", id.ToString());
                        CinemaTicketShowCinemaOfMovie(cinemaManager.data.filmCode, id, true);
                        break;
                    case "sanscode":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "sanscode", id.ToString());
                        CinemaTicketInitialSansOfCinemaFilm(cinemaManager.data.filmCode, cinemaManager.data.cinemaCode, id, true);
                        break;
                    case "firstname":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "firstname", id.ToString());
                        showMessage = true;
                        msg = "لطفا نام کوچک خود را وارد نمایید";

                        break;
                    case "lastname":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "lastname", id.ToString());
                        showMessage = true;
                        msg = "لطفا نام فامیل خود را وارد نمایید";
                        break;
                    case "seatselect":
                        currentAction.set(SimpaySectionEnum.CinemaTicket, "lastname", id.ToString());
                        CinemaTicketSelectSeats(id);
                        break;
                    case "orderticket":
                        CinemaTicketOrder(id);
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

        private void CinemaTicketInitial()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var cinemaManager = new CinemaTicket.Manager(chatId);
            cinemaManager.UpdateInformation();
        }

        private void CinemaTicketFilmSearch(long id = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            currentAction.set(SimpaySectionEnum.CinemaTicket, "filmquery", id.ToString());

            var msg = "جهت جستجوی فیلم شما میتوانید بخشی از نام فیلم، بازیگران، کارگردان، موضوع فیلم و ... را وارد نمایید:";
            telegramAPI.send(msg, cancelButton());
        }


        private void CinemaTicketEntry(string field, string value)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var cinemaManager = new CinemaTicket.Manager(chatId);
            do
            {
                switch (field.ToLower())
                {
                    case "filmquery":
                        telegramAPI.send("درحال جستجو لطفا صبر نمایید.", cancelButton());
                        //currentAction.set(SimpaySectionEnum.CinemaTicket, "filmquery", value);
                        cinemaManager.setQuery(value);
                        CinemaTicketShowMovies(0, 0, true);

                        break;
                    case "cinemacode":
                        if (!Utils.isInteger(value))
                        {
                            telegramAPI.send("مقدار ورودی باید بصورت عددی و کد سینما باشد، از فهرست بالا انتخاب نمایید. ");
                            break;
                        }
                        CinemaTicketUpdateInfo(field, value, Convert.ToInt32(currentAction.parameter));
                        break;
                    case "sanscode":
                        if (!Utils.isInteger(value))
                        {
                            telegramAPI.send("مقدار ورودی باید بصورت عددی و کد سانس باشد، از فهرست بالا انتخاب نمایید. ");
                            break;
                        }
                        CinemaTicketUpdateInfo(field, value, Convert.ToInt32(currentAction.parameter));
                        break;
                    case "firstname":
                    case "lastname":
                        CinemaTicketUpdateInfo(field, value, Convert.ToInt32(currentAction.parameter));
                        break;
                    default:
                        break;
                }

            } while (false);
        }
        private void CinemaTicketUpdateInfo(string field, dynamic value = null, long id = 0, bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var cinemaManager = new CinemaTicket.Manager(chatId, id);
            var nextStepField = "";
            var msgToSend = "";
            dynamic stepValue = null;
            do
            {

                switch (field.ToLower())
                {
                    case "filmcode":
                        cinemaManager.data.filmCode = (int)value;
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        nextStepField = "cinemacode";
                        currentAction.remove();
                        break;
                    case "cinemacode":
                        cinemaManager.data.cinemaCode = (int)value;
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        nextStepField = "sanscode";


                        currentAction.remove();
                        break;
                    case "sanscode":
                        cinemaManager.data.sansCode = (int)value;
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        nextStepField = "firstname";
                        currentAction.remove();
                        break;
                    case "firstname":
                        cinemaManager.data.firstName = (string)value;
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        nextStepField = "lastname";
                        currentAction.remove();
                        break;
                    case "lastname":
                        cinemaManager.data.lastName = (string)value;
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        nextStepField = "seatselect";
                        currentAction.remove();
                        break;
                    case "seatselect":
                        var jointSeatNumbers = (string)value;

                        var oldSeatCount = cinemaManager.data.seatCount;
                        cinemaManager.data.seats = jointSeatNumbers;
                        var newSeatCount = cinemaManager.data.seatCount;

                        if (cinemaManager.data.status == "paid")// in case they paid but cannot buythe ticket, refer to the buy again!
                        {
                            if (oldSeatCount != newSeatCount)
                            {
                                telegramAPI.send($"تعداد صندلی های انتخابی باید {oldSeatCount} باشد. دوباره سعی فرمایید.");
                                CinemaTicketSelectSeats(cinemaManager.data.id);
                                break;
                            }
                            CinemaTicketBuyTicket(cinemaManager.data.id);
                            break;
                        }

                        cinemaManager.data.status = "seat";
                        cinemaManager.setInfo();
                        id = cinemaManager.data.id;
                        cinemaManager.setTotalAmount();

                        msgToSend = $" با توجه به انتخاب {cinemaManager.data.seatCount} صندلی، مبلغ نهای بلیط شما برابر است با {cinemaManager.data.amount.ToString("#,##")} ریال";
                        currentAction.remove();
                        nextStepField = "orderticket";

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

                CinemaTicketRequestInfo(nextStepField, stepValue, id.ToString());
            }

        }
        private void CinemaTicketCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var id = Convert.ToInt32(data.Split(':')[2]);
            do
            {

                switch (action.ToLower())
                {
                    case "page":
                        var pageNumber = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketShowMovies(id: id, pageNumber: pageNumber);
                        break;
                    case "film":
                        var filmCode = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketUpdateInfo("filmcode", filmCode, id);

                        break;
                    case "cinema":
                        var cinameCode = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketUpdateInfo("cinemacode", cinameCode, id);

                        break;
                    case "sans":
                        var sansCode = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketUpdateInfo("sanscode", sansCode, id);
                        break;

                    case "search":
                        CinemaTicketRequestInfo(action);
                        break;
                    case "trailer":
                        var filmCodeToShow = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketShowTrailer(filmCodeToShow);
                        break;
                    case "sanspage":
                        var sansPageNumber = Convert.ToInt32(data.Split(':')[3]);
                        CinemaTicketShowSans(id, sansPageNumber);
                        break;


                    default:
                        break;
                }
            } while (false);



        }


        private void CinemaTicketShowMovies(long id, int pageNumber = 0, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                //var moviesOnScreen = CinematicketAPI.GetOnScreen(pageNumber);
                if (pageNumber == 0)
                {

                    pageNumber = 1;
                    CinemaTicketInitial();
                }
                //if (string.IsNullOrEmpty(query))
                //{
                //    query = currentAction.parameter;
                //}
                var onScreenMovieData = cinemaManager.GetOnScreenMoviesData(id, pageNumber);
                if (onScreenMovieData.maxRecord == 0)
                {
                    sendMenu(message: " متاسفانه فیلمی در سامانه یافت نشد.");
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();


                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "سینماهای در حال اکران",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:film:{id}:{onScreenMovieData.data.filmCode}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();



                if (!String.IsNullOrEmpty(onScreenMovieData.data.filmTrailer))
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = "مشاهده تریلر",
                        //Url = onScreenMovieData.data.filmTrailer
                        CallbackData = $"{SimpaySectionEnum.CinemaTicket}:trailer:{id}:{onScreenMovieData.data.filmCode}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }
                //colKey.Add(new InlineKeyboardButton()
                //{
                //    Text = "جستجو",
                //    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:search:{id}"
                //});
                //inlineK.Add(colKey.ToArray());
                //colKey.Clear();


                //inlineK.Add(colKey.ToArray());
                var callBackExtra = $"{SimpaySectionEnum.CinemaTicket}:page:{id}";
                var paging = paginButtons(6, pageNumber, onScreenMovieData.maxRecord, callBackExtra);

                if (paging != null)
                    inlineK.Add(paging);



                //var fileToSend = new FileToSend
                //{
                //    Url = new Uri(onSCreenMovieData.data[0].filmImageUrl),
                //};
                //telegramAPI.fileToSend = fileToSend;
                //telegramAPI.caption = $"{onSCreenMovieData.data[0].filmName} ({onSCreenMovieData.data[0].categoryName})";
                //telegramAPI.send(Telegram.Bot.Types.Enums.MessageType.PhotoMessage, r);

                var movie = "";
                movie += $"عنوان فیلم: {onScreenMovieData.data.filmName} {Environment.NewLine}";
                movie += $"کارگردان: {onScreenMovieData.data.director} {Environment.NewLine}";
                movie += $"تهیه کننده: {onScreenMovieData.data.producer} {Environment.NewLine}";
                movie += $"بازیگران: {onScreenMovieData.data.casting} {Environment.NewLine}";

                movie += $"ژانر: {onScreenMovieData.data.genre} {Environment.NewLine}";
                movie += $"دسته: {onScreenMovieData.data.categoryName} {Environment.NewLine}";
                movie += $"تاریخ اکران: {onScreenMovieData.data.releaseDate} {Environment.NewLine}";
                movie += $"درجه: {onScreenMovieData.data.rating} {Environment.NewLine}";
                movie += $"خلاصه داستان: {onScreenMovieData.data.summary} {Environment.NewLine}";

                movie += $"{Environment.NewLine} {Environment.NewLine}";
                movie += $"{new Uri(CinemaTicket.CinematicketAPI.WEB_URL + onScreenMovieData.data.filmImageUrl)}";


                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, movie, markup);
                }
                else
                {
                    telegramAPI.send(movie, markup);
                }


            } while (false);
        }


        private void CinemaTicketShowTrailer(int filmCode)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var cinemaManager = new CinemaTicket.Manager(chatId);
            telegramAPI.sendUploadingVideoStatus();
            do
            {
                var filmInfo = cinemaManager.GetFilmInfo(filmCode);
                if (String.IsNullOrEmpty(filmInfo.filmTrailer))
                {
                    telegramAPI.send("متاسفانه تریلر فیلم یافت نشد!");
                    break;
                }

                telegramAPI.fileToSend = new FileToSend
                {
                    Url = new Uri(filmInfo.filmTrailer),
                    Filename = filmInfo.filmCode.ToString()

                };
                telegramAPI.caption = $"تریلر فیلم {filmInfo.filmName}";
                var sendTrailerResult = telegramAPI.send(Telegram.Bot.Types.Enums.MessageType.VideoMessage);
                if (!sendTrailerResult)
                {
                    telegramAPI.send($"برای مشاهده تریلر این فیلم لطفا به لینک زیر بروید: {Environment.NewLine} {filmInfo.filmTrailer}");

                }
            } while (false);
        }
        private void CinemaTicketShowCinemaOfMovie(int filmCode, long id = 0, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId);
                var cinemas = cinemaManager.GetCinemaOfMovie(filmCode);
                if (cinemas.Count == 0)
                {
                    sendMenu(message: " متاسفانه سینمایی با قابلیت خرید اینترنتی بلیط برای این فیلم یافت نشد.");
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colK = new List<InlineKeyboardButton>();
                var listIndex = 0;
                var colIndex = 0;
                var maxCol = 2;
                var btnText = "";
                var btnData = "";
                while (listIndex < (cinemas.Count < 70 ? cinemas.Count : 70))
                {
                    colIndex = 0;
                    while (colIndex < maxCol)
                    {
                        colIndex++;
                        if (listIndex < cinemas.Count)
                        {
                            btnText = $"{cinemas[listIndex].cinemaName}";
                            btnData = $"{SimpaySectionEnum.CinemaTicket}:cinema:{id}:{cinemas[listIndex].cinemaCode}";
                        }
                        else
                        {
                            btnText = $".";
                            btnData = $"{SimpaySectionEnum.CinemaTicket}:blank:{id}:0";

                        }
                        colK.Add(new InlineKeyboardButton()
                        {
                            Text = btnText,
                            CallbackData = btnData,
                        });


                        listIndex++;
                        if (listIndex > 150)
                            break;
                    }
                    inlineK.Add(colK.ToArray());
                    colK.Clear();

                }

                var msgToSend = "فهرست سینماهائی که فیلم فوق را بر پرده دارند: ";

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);
                }
                else
                {
                    telegramAPI.send(msgToSend, markup);
                }

            } while (false);
        }



        private void CinemaTicketInitialSansOfCinemaFilm(int filmCode, int cinemaCode, long id, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId) { data = new CinemaTicket.CinemaTicket { id = id } };
                cinemaManager.InitialSanseOfCinema(cinemaCode, filmCode);

                var msgToSend = $"فهرست سانس و زمان نمایش این فیلم در این سینما: {Environment.NewLine} {Environment.NewLine}";

                telegramAPI.send(msgToSend);

                CinemaTicketShowSans(id, 1, forceNewMessage);


            } while (false);

        }
        private void CinemaTicketShowSans(long id, int pageNumber, bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId);
                var sansPaging = cinemaManager.getSans(id, pageNumber);

                if (sansPaging.maxRecord == 0)
                {
                    telegramAPI.send("فهرستی یافت نشد.");
                    break;


                }

                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "انتخاب",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:sans:{id}:{sansPaging.data.sansCode}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "مشاهده پلان",
                    //CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatview:{id}:{sansPaging.data.cinemaCode}:{sansPaging.data.sansCode}"
                    Url = $@"{ProjectValues.CinemaTicketSansPlanUrl}/{sansPaging.data.cinemaCode}/{sansPaging.data.sansCode}/{chatId}/0"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();


                var callBackExtra = $"{SimpaySectionEnum.CinemaTicket}:sanspage:{id}";
                var paging = paginButtons(6, pageNumber, sansPaging.maxRecord, callBackExtra);

                if (paging != null)
                    inlineK.Add(paging);



                var msgToSend = "";
                msgToSend += $"تاریخ نمایش: {sansPaging.data.showDay} {sansPaging.data.showDate} {Environment.NewLine}";
                msgToSend += $"ساعت نمایش: {sansPaging.data.sansHour} {Environment.NewLine}";
                msgToSend += $"سالن نمایش: {sansPaging.data.salon}{Environment.NewLine}";
                msgToSend += $"بهای بلیط: {sansPaging.data.sansPrice.ToString("#,##")} ریال{Environment.NewLine}";
                msgToSend += $"{Environment.NewLine} {Environment.NewLine}-";



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);
                }
                else
                {
                    telegramAPI.send(msgToSend, markup);
                }

            } while (false);

        }
        private void CinemaTicketInitialSeats(long id, int cinemaCode, int sansCode)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                cinemaManager.InitialSeatsList(cinemaCode, sansCode);
                if (cinemaManager.resultAction.hasError)
                {
                    telegramAPI.send(cinemaManager.resultAction.message);
                    break;
                }

                CinemaTicketShowSeatsList(id, 1, 1, true);
            } while (false);
        }

        private void CinemaTicketShowSeatsList(long id, int row = 1, int colSection = 1, bool forceNewWindow = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {

                if (row < 1 || colSection < 1)
                {
                    break;
                }

                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                var seatsInfo = cinemaManager.getSeats(id, row, colSection);

                if (seatsInfo.rowCount == 0)
                {
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();

                //var seatAction = selectable ? "seat" : "seatblank";
                var seatAction = "";

                var index = 0;
                foreach (var seat in seatsInfo.data)
                {
                    var seatNum = "";
                    switch (seat.state)
                    {
                        case 0: // Free
                            seatNum = seat.realSeatNumber;
                            seatAction = "seat";
                            break;
                        case 1:
                            seatNum = "X";
                            seatAction = "sold";
                            break;
                        case 2:
                            seatNum = ".";
                            seatAction = "not";
                            break;

                        case 3:
                            seatNum = ".";
                            seatAction = "aisle";
                            break;
                        case 4:
                            seatNum = "X";
                            seatAction = "reserve";
                            break;
                        case 5:
                            seatNum = "X";
                            seatAction = "inactive";
                            break;

                        default:
                            seatNum = ".";
                            seatAction = "NULL";
                            break;
                    }
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = $"{seatNum }",
                        CallbackData = $"{SimpaySectionEnum.CinemaTicket}:{seatAction}:{id}:{seatsInfo.data[index].seatNumber}"
                    });
                    index++;

                }

                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                //Col serction Navigation:
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $"محدوده چپ",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatnav:{id}:{row}:{colSection - 1}"
                });
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $"محدوده راست",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatnav:{id}:{row}:{colSection + 1}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $"ردیف بالا",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatnav:{id}:{row - 1}:{colSection}"
                });
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = $"ردیف پایین",
                    CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatnav:{id}:{row + 1}:{colSection}"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();



                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();

                //var msgToSend = "نمودار پلان سالن و وضعیت صندلیها. دقت فرمایید که انتخاب صندلی پس از پرداخت بهای بلیط امکان پذیر است.";
                var msgToSend = $"نمودار پلان سانس ردیف {row} بخش {colSection}.";
                //telegramAPI.send(msgToSend, markup);
                if (callbackQuery != null && !forceNewWindow)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, markup);
                }
                else
                {
                    telegramAPI.send(msgToSend, markup);
                }



            } while (false);
        }

        private void CinemaTicketSendPic()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var font = new Font("Times New Roman", 50);
            var fontColor = Color.Aqua;
            var backColor = Color.White;
            //var img = Utils.DrawText("Xfhdsakjfhdskf hasjdkhfjkdshf jksdfhkjsad hfjsd XXXX", font, fontColor, backColor);
            var img = (new CinemaTicket.Manager(chatId)).DrawPlan();
            telegramAPI.fileToSend = new FileToSend
            {
                Content = Utils.ImageToStream(img, ImageFormat.Png),
                Filename = "textDrawing!"

            };

            telegramAPI.send(Telegram.Bot.Types.Enums.MessageType.PhotoMessage);

        }
        private void CinemaTicketSelectSeats(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var msg = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                msg += $"در این مرحله با فشار دادن دکمه زیر شما به صفحه انتخاب صندلی هدایت میشوید. {Environment.NewLine} ";
                colKey.Add(new InlineKeyboardButton()
                {
                    Text = "مشاهده پلان",
                    //CallbackData = $"{SimpaySectionEnum.CinemaTicket}:seatview:{id}:{sansPaging.data.cinemaCode}:{sansPaging.data.sansCode}"
                    Url = $@"{ProjectValues.CinemaTicketSansPlanUrl}/{cinemaManager.data.cinemaCode}/{cinemaManager.data.sansCode}/{chatId}/1"
                });
                inlineK.Add(colKey.ToArray());
                colKey.Clear();

                var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                markup.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send(msg, markup);

            } while (false);

        }

        private void CinemaTicketOrder(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send("هم اکنون سامانه در حال رزرو بلیط شما میباشد. لطفا صبر نمایید.");
            do
            {

                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                cinemaManager.OrderTicket();
                if (cinemaManager.resultAction.hasError)
                {
                    telegramAPI.send(cinemaManager.resultAction.message);
                    break;
                }

                PaymentStartProcess(cinemaManager.data.saleKey);


            } while (false);
        }




        public void CinemaTicketSeatSelectionCallBack(long chatId, string[] seats)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            try
            {
                do
                {
                    TelegramMessage.chatId = chatId;
                    SimpayCore.chatId = chatId;
                    currentAction = new CurrentAction(chatId);
                    telegramAPI = new TelegramAPI(chatId);
                    var cinemaManager = new CinemaTicket.Manager(chatId, true);

                    if (cinemaManager.data.id == 0)
                    {
                        telegramAPI.send($"متاسفانه بلیط درخواستی شما یافت نشد.");
                        break;
                    }
                    if (cinemaManager.data.status == "done")
                    {
                        telegramAPI.send($"شما قبلا این بلیط را خرداری نمودید. لطفا از ابتدا جهت خرید بلیط جدید اقدام فرمایید.");
                        break;

                    }
                    var jointSeatNumbers = "";
                    for (int i = 0; i < seats.Length; i++)
                    {
                        jointSeatNumbers += $"{seats[i]};";
                    }
                    CinemaTicketUpdateInfo("seatselect", jointSeatNumbers, cinemaManager.data.id);


                } while (false);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, 0);
                throw;

            }
        }

        public void CinemaTicketBankCallBack(long chatId, string saleKey)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                //var telegramAdmin = new TelegramAPI(ProjectValues.adminChatId);
                currentAction = new CurrentAction(chatId);
                var cinemaManager = new CinemaTicket.Manager(chatId, true);
                if (cinemaManager.data.id == 0)
                {
                    telegramAPI.send($"متاسفانه بلیط درخواستی شما یافت نشد.");
                    break;
                }
                cinemaManager.GetSaleInfoForTicketBuying();
                if (cinemaManager.resultAction.hasError)
                {
                    telegramAPI.send(cinemaManager.resultAction.message + $" {Environment.NewLine}");
                    telegramAPI.send("متاسفانه سینما تیکت بلیط شما را نهایی نکرده است. مبلغ پرداختی شما تا حداکثر یک ساعت دیگر به حساب شما بازگردانده خواهد شد. ");
                    sendMenu();
                    break;
                }

                CinemaTicketBuyTicket(cinemaManager.data.id);

            } while (false);
        }
        public void CinemaTicketBuyTicket(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var cinemaManager = new CinemaTicket.Manager(chatId, id);
                cinemaManager.BuyTicket();
                if (cinemaManager.resultAction.hasError)
                {
                    telegramAPI.send(cinemaManager.resultAction.message + $" {Environment.NewLine}");
                    telegramAPI.send(" با عرض پوزش، ظاهرا مشکلی در تایید نهایی بلیط شما بوجود آمده است بعنوان مثال صندلی مورد نظر شما قبل از شما اشغال شده است. لطفا مجددا صندلی خود را انتخاب فرمایید، در غیر اینصورت مبلغ پرداختی شما تا حداکثر یک ساعت دیگر به حساب شما بازگردانده خواهد شد.");
                    CinemaTicketSelectSeats(cinemaManager.data.id);
                    break;
                }
                cinemaManager.PrintTicket();
                if (cinemaManager.resultAction.hasError)
                {
                    telegramAPI.send(cinemaManager.resultAction.message);
                    break;
                }

                var msg = "";
                msg += $" اطلاعات بلیط شما:  {Environment.NewLine}";
                msg += $" عنوان فیلم: {cinemaManager.data.extraInfo.filmName}  {Environment.NewLine}";
                msg += $" نام سینما: {cinemaManager.data.extraInfo.cinemaName}   {Environment.NewLine}";
                msg += $"  سالن: {cinemaManager.data.extraInfo.salonName}  {Environment.NewLine}";
                msg += $"  شماره رزرو: {cinemaManager.data.extraInfo.reserveCode}  {Environment.NewLine}";
                msg += $" تاریخ: {Utils.Shamsi(cinemaManager.data.extraInfo.date.ToString("d"))}   {Environment.NewLine}";
                msg += $"  ساعت : {cinemaManager.data.extraInfo.time.FromEpochToTimeEx()}     {Environment.NewLine}";
                msg += $"  تعداد بلیط: {cinemaManager.data.extraInfo.ticketCount}     {Environment.NewLine}";
                msg += $"  بهای کل: {cinemaManager.data.extraInfo.totalAmount.ToString("#,##")} ریال    {Environment.NewLine}";
                msg += $"   {Environment.NewLine}";
                msg += $"   {Environment.NewLine}";

                telegramAPI.send(msg);
                sendMenu();


            } while (false);
        }
    }
}
