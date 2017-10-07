using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {
        public enum CalendarNavigationSwitchEnum
        {
            NoNavigation = 0,
            SelectYears = 1,
            SelectMonths = 2,
            ChangeMonth = 4,
            SelectYearsMonth = SelectYears | SelectYears
        }

        public void Calendar(string theDate = null, string extraInfo = "", bool forceNewWindow = false, CalendarNavigationSwitchEnum Navigation = CalendarNavigationSwitchEnum.ChangeMonth)
        {
            string[] days = { "ش", "ی", "د", "س", "چ", "پ", "ج" };
            var date = DateTime.Now;
            if (!String.IsNullOrEmpty(theDate))
            {
                date = Convert.ToDateTime(theDate);
            }
            var fc = new FarsiCalendar(date);
            var calCells = fc.PersianCalendarGrid();

            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();
            if (((int)Navigation & (int)CalendarNavigationSwitchEnum.SelectYears) == (int)CalendarNavigationSwitchEnum.SelectYears)
            {
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"تغییر سال",
                    CallbackData = $"{SimpaySectionEnum.Calendar}:chngy:{fc.pDate.Substring(0, 4)}:{extraInfo}:{Navigation}"
                });

            }
            if (((int)Navigation & (int)CalendarNavigationSwitchEnum.SelectYears) == (int)CalendarNavigationSwitchEnum.SelectYears)
            {
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"تغییر ماه",
                    CallbackData = $"{SimpaySectionEnum.Calendar}:chngm:{fc.pDate.Substring(0, 4)}:{extraInfo}:{Navigation}"
                });
            }
            if (colK.Count > 0)
            {
                //inlineK.Add(colK.ToArray());
                inlineK.Add(colK.Reverse<InlineKeyboardButton>().ToList().ToArray());
                colK.Clear();
            }

            if (((int)Navigation & (int)CalendarNavigationSwitchEnum.ChangeMonth) == (int)CalendarNavigationSwitchEnum.ChangeMonth)
            {
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"ماه قبلی",
                    CallbackData = $"{SimpaySectionEnum.Calendar}:month:{date.AddMonths(-1).ToString("yyyy/MM/dd")}:{extraInfo}:{Navigation}"
                });
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"ماه بعدی",
                    CallbackData = $"{SimpaySectionEnum.Calendar}:month:{date.AddMonths(+1).ToString("yyyy/MM/dd")}:{extraInfo}:{Navigation}"
                });

                //inlineK.Add(colK.ToArray());
                inlineK.Add(colK.Reverse<InlineKeyboardButton>().ToList().ToArray());
                colK.Clear();


            }

            for (var i = 0; i < 7; i++)
            {
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"{days[i]}",
                    CallbackData = $"{SimpaySectionEnum.Calendar}:days:0:{extraInfo}:{Navigation}"
                });
            }

            //inlineK.Add(colK.ToArray());
            inlineK.Add(colK.Reverse<InlineKeyboardButton>().ToList().ToArray());
            colK.Clear();

            for (var r = 0; r < calCells.Length; r++)
            {
                for (var c = 0; c < calCells[r].Length; c++)
                {
                    colK.Add(new InlineKeyboardButton()
                    {
                        Text = $"{calCells[r][c].caption}",
                        CallbackData = $"{SimpaySectionEnum.Calendar}:cell:{calCells[r][c].date}:{extraInfo}:{Navigation}"
                    });

                }
                if (colK.Count < 7)
                {
                    for (var i = 0; i < 7 - calCells[r].Length; i++)
                    {
                        colK.Add(new InlineKeyboardButton()
                        {
                            Text = $".",
                            CallbackData = $"{SimpaySectionEnum.Calendar}:cell:0:{extraInfo}:{Navigation}"
                        });
                    }
                }
                //inlineK.Add(colK.ToArray());
                inlineK.Add(colK.Reverse<InlineKeyboardButton>().ToList().ToArray());
                colK.Clear();
            }


            var Cal = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            Cal.InlineKeyboard = inlineK.ToArray();

            var msgToSend = $"تقویم {fc.pMonthName} ماه سال {fc.pYear} ";

            if (callbackQuery != null && !forceNewWindow)
            {
                telegramAPI.editText(callbackQuery.Message.ID, msgToSend, Cal);
            }
            else
            {
                telegramAPI.send(msgToSend, Cal);
            }


        }
        private void CalendarShowMonths(int year, string extraInfo, CalendarNavigationSwitchEnum Navigation)
        {
            var msgToSend = "ماه مورد نظر را انتخاب کنید.";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();

            var fc = new FarsiCalendar(DateTime.Now);
            var shamsiMonthArray = fc.getPersianMonthArray();
            var col = 0;
            for (var i = 0; i < shamsiMonthArray.Length; i++)
            {
                col++;
                colK.Add(new InlineKeyboardButton
                {
                    Text = shamsiMonthArray[i],
                    CallbackData = $@"{SimpaySectionEnum.Calendar}:selm:{year}/{(i + 1).ToString("00")}/01:{extraInfo}:{Navigation}"
                });
                if (col == 3)
                {
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                    col = 0;
                }
            }


            var Cal = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            Cal.InlineKeyboard = inlineK.ToArray();

            telegramAPI.editText(callbackQuery.Message.ID, msgToSend, Cal);

        }

        private void CalendarShowYears(int thisYear, string extraInfo, CalendarNavigationSwitchEnum Navigation)
        {
            var msgToSend = "سال مورد نظر را انتخاب کنید.";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();

            colK.Add(new InlineKeyboardButton
            {
                Text = $"قبلتر",
                CallbackData = $@"{SimpaySectionEnum.Calendar}:chngy:{thisYear - 20}:{extraInfo}:{Navigation}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();



            var fc = new FarsiCalendar(DateTime.Now);
            var shamsiMonthArray = fc.getPersianMonthArray();
            var col = 0;
            for (var Y = thisYear - 10; Y < thisYear + 10; Y++)
            {
                col++;
                colK.Add(new InlineKeyboardButton
                {
                    Text = $"{Y}",
                    CallbackData = $@"{SimpaySectionEnum.Calendar}:sely:{Y}/01/01:{extraInfo}:{Navigation}"
                });
                if (col == 5)
                {
                    inlineK.Add(colK.ToArray());
                    colK.Clear();
                    col = 0;
                }
            }
            colK.Add(new InlineKeyboardButton
            {
                Text = $"جلوتر",
                CallbackData = $@"{SimpaySectionEnum.Calendar}:chngy:{thisYear + 20}:{extraInfo}:{Navigation}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();


            var Cal = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            Cal.InlineKeyboard = inlineK.ToArray();

            telegramAPI.editText(callbackQuery.Message.ID, msgToSend, Cal);
        }
        private void CalendarCallBack(string data)
        {
            var action = data.Split(':')[1];
            var Navigation = CalendarNavigationSwitchEnum.ChangeMonth;// Default value
            if (data.Split(':').Length >= 5)
            {
                //Navigation = (CalendarNavigationSwitchEnum)Convert.ToInt16(data.Split(':')[4]);
                Navigation = Converter.ToEnum<CalendarNavigationSwitchEnum>(data.Split(':')[4]);
            }

            do
            {
                switch (action.ToLower())
                {
                    case "month":
                        var newDate = data.Split(':')[2];
                        var passedExtraInfo = data.Split(':')[3];
                        Calendar(newDate, passedExtraInfo, false, Navigation);

                        break;
                    case "chngm": //change month
                        var year = Convert.ToInt32(data.Split(':')[2]);
                        var changMonthPassedExtraInfo = data.Split(':')[3];
                        CalendarShowMonths(year, changMonthPassedExtraInfo, Navigation);
                        break;
                    case "chngy":
                        var currentYear = Convert.ToInt32(data.Split(':')[2]);
                        var changYearPassedExtraInfo = data.Split(':')[3];
                        CalendarShowYears(currentYear, changYearPassedExtraInfo, Navigation);
                        break;

                    case "selm":
                        var yearMonthDay = data.Split(':')[2];
                        var selectedMonthPassedExtraInfo = data.Split(':')[3];

                        var fcNewMonth = new FarsiCalendar(yearMonthDay);


                        Calendar(fcNewMonth.gDate.ToString("yyyy/MM/dd"), selectedMonthPassedExtraInfo, false, Navigation);
                        break;
                    case "sely":
                        var selyYearMonthDay = data.Split(':')[2];
                        var selectedYearPassedExtraInfo = data.Split(':')[3];

                        var fcNewYear = new FarsiCalendar(selyYearMonthDay);


                        Calendar(fcNewYear.gDate.ToString("yyyy/MM/dd"), selectedYearPassedExtraInfo, false, Navigation);
                        break;


                    case "cell":

                        var date = data.Split(':')[2];
                        var extraInfo = data.Split(':')[3];

                        var section = extraInfo.Split('|')[0];
                        var field = extraInfo.Split('|')[1];
                        var id = Convert.ToInt32(extraInfo.Split('|')[2]);

                        if (date == "0")
                        {
                            telegramAPI.answerCallBack("فقط اعداد تاریخ را انتخاب نمایید");
                        }
                        else
                        {
                            //telegramAPI.editText(callbackQuery.Message.ID, $"تاریخ انتخابی {date}");
                            if (section == Convert.ToString(SimpaySectionEnum.BusTicket))
                            {
                                if (field.ToLower() == "datetime")
                                {
                                    BusTicketUpdateInfo(field, date, id, callbackQuery.Message.ID);
                                }

                            }
                            if (section == Convert.ToString(SimpaySectionEnum.TrainTicket))
                            {
                                if (field.ToLower() == "gdate" || field.ToLower() == "rdate")
                                {
                                    TrainTicketUpdateInfo(field, date, id);
                                }
                                if (field.ToLower() == "dob")
                                {
                                    TrainTicketUpdateInfo("dateofbirth", date, id);
                                }


                            }
                            if (section == Convert.ToString(SimpaySectionEnum.AirplaneTicket))
                            {
                                if (field.ToLower() == "gdate" || field.ToLower() == "rdate")
                                {
                                    AirplaneTicketUpdateInfo(field, date, id);
                                }
                                if (field.ToLower() == "dob")
                                {
                                    AirplaneTicketUpdateInfo("dateofbirth", date, id);
                                }


                            }


                        }

                        break;

                    default:
                        break;
                }
            } while (false);
        }

    }
}