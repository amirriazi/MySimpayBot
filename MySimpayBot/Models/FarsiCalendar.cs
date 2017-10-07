using System;
using System.Collections.Generic;
using System.Globalization;
using PersianCalendarPlus;

namespace Models
{
    class FarsiCalendar
    {
        public bool hasError { get; set; }
        public string errorMessage { get; set; }
        public DateTime gDate { get; set; }
        public string pDate { get; set; }
        public string pMonthName { get; set; }
        public int pYear { get; set; }

        public FarsiCalendar(DateTime passGregorianDate)
        {
            hasError = false;
            gDate = passGregorianDate;
            pDate = $"{toSDate()} {gDate.ToString("HH:mm")}";
            pYear = Convert.ToInt32(pDate.Substring(0, 4));
            pMonthName = getPersianMonthName();
        }
        public FarsiCalendar(string passPersianDate)
        {
            hasError = false;
            try
            {
                pDate = passPersianDate;
                gDate = toGDate();
                pYear = Convert.ToInt32(pDate.Substring(0, 4));
                pMonthName = getPersianMonthName();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                hasError = true;
            }

        }

        public string toSDate()
        {
            return ConvertToPersian(gDate);
        }
        public string toSDate(DateTime passGregorianDate)
        {
            return ConvertToPersian(passGregorianDate);
        }


        private string ConvertToPersian(DateTime GregorianDate)
        {

            return PerCalPlus.GregorianDateToPersian(GregorianDate);

        }

        public DateTime toGDate()
        {
            return ConvertToGregorian(pDate);
        }
        public DateTime toGDate(string passPersianDate)
        {
            return ConvertToGregorian(passPersianDate);
        }


        private DateTime ConvertToGregorian(string passPersianDate)
        {

            var justDate = "";
            var Time = "";

            if (passPersianDate.Length > 11)
            {
                justDate = passPersianDate.Substring(0, 10);
                Time = passPersianDate.Substring(11);
            }
            else
            {
                justDate = passPersianDate;
                Time = "";
            }

            string strG = PerCalPlus.PersianDateToGregorian(justDate, PerCalPlus.DateStringType.ToShortDateString).ToShortDateString() + " " + (Time != "" ? Time : "");
            DateTime G = Convert.ToDateTime(strG);
            return G;
        }


        public CalendarCell[][] PersianCalendarGrid()
        {
            var arrRows = new List<CalendarCell[]>();
            var arrCols = new List<CalendarCell>();


            var pc = new PersianCalendar();
            int year = pc.GetYear(gDate);
            int month = pc.GetMonth(gDate);
            int day = pc.GetDayOfMonth(gDate);
            var maxDayOfMonth = PerCalPlus.GetDaysInPersianMonth(year, month);
            var StartDayOfWeek = getPersianFirstDayOfTheMonth();


            var dataDay = 0;
            var dayOfWeek = 0;
            var row = 1;
            var col = 0;

            while (dataDay < maxDayOfMonth)
            {
                col++;
                if (row == 1 && dayOfWeek < StartDayOfWeek)
                {
                    arrCols.Add(new CalendarCell()
                    {
                        caption = ".",
                        date = "0"
                    });
                }
                else
                {
                    dataDay++;
                    arrCols.Add(new CalendarCell()
                    {
                        caption = $"{dataDay}",
                        date = $@"{year}/{month.ToString("00")}/{dataDay.ToString("00")}"
                    });
                }

                if (col == 7)
                {
                    col = 0;
                    row++;
                    arrRows.Add(arrCols.ToArray());
                    arrCols.Clear();


                }
                dayOfWeek++;
            }
            if (arrCols.Count != 0)
                arrRows.Add(arrCols.ToArray());

            return arrRows.ToArray();
        }

        public int getPersianFirstDayOfTheMonth()
        {
            string PersianFirstDateMonth = pDate.Substring(0, 8) + "01";

            var gDateOfFirstDay = PerCalPlus.PersianDateToGregorian(Convert.ToInt16(pDate.Substring(0, 4)), Convert.ToInt16(pDate.Substring(5, 2)), 1);

            var pc = new PersianCalendar();
            var fday = getPersianDayWeekIndex(pc.GetDayOfWeek(gDateOfFirstDay));
            return fday;
        }

        public int PersianDayIndexOfWeek()
        {
            var pc = new PersianCalendar();
            var gIdxOfWeek = pc.GetDayOfWeek(gDate);
            return getPersianDayWeekIndex(gIdxOfWeek);

        }
        private int getPersianDayWeekIndex(DayOfWeek gIdx)
        {
            int Idx = 0;
            switch (gIdx)
            {
                case DayOfWeek.Saturday:
                    Idx = 0;
                    break;
                case DayOfWeek.Sunday:
                    Idx = 1;
                    break;
                case DayOfWeek.Monday:
                    Idx = 2;
                    break;
                case DayOfWeek.Tuesday:
                    Idx = 3;
                    break;
                case DayOfWeek.Wednesday:
                    Idx = 4;
                    break;
                case DayOfWeek.Thursday:
                    Idx = 5;
                    break;
                case DayOfWeek.Friday:
                    Idx = 6;
                    break;
                default:
                    break;
            }
            return Idx;
        }

        public string[] getPersianMonthArray()
        {
            string[] monthName = { "فروردین", "اردیبشهت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
            return monthName;
        }

        private string getPersianMonthName()
        {
            var monthName = getPersianMonthArray();
            return monthName[Convert.ToInt16(pDate.Substring(5, 2)) - 1];
        }



        public class CalendarCell
        {
            public string caption { get; set; }
            public string date { get; set; }
        }
    }
}
