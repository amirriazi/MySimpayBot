using System;
using System.Collections.Generic;

namespace Models.BusTicket
{
    public class BusTicketData
    {
        public long id { get; set; }
        public int amount { get; set; }
        public int sourceStateCode { get; set; }
        public string sourceStateShowName { get; set; }
        public int sourceTerminalCode { get; set; }
        public string sourceTerminalShowName { get; set; }
        public int destinationStateCode { get; set; }
        public string destinationStateShowName { get; set; }
        public int destinationTerminalCode { get; set; }
        public string destinationTerminalShowName { get; set; }
        public DateTime dateTime { get; set; }
        public string serviceUniqueIdentifier { get; set; }
        public string fullName { get; set; }
        public int seatCount { get; set; }
        public int selectedServiceRow { get; set; }
        public string saleKey { get; set; }
        public TransactionStatusEnum status { get; set; }
        public string ticketNumber { get; set; }
    }

    public class CodeAndName
    {
        public int code { get; set; }
        public string name { get; set; }
    }

    public class SeatInfo
    {
        public int capacity { get; set; }
        public int columnNumber { get; set; }
        public int floor { get; set; }
        public int rowNumber { get; set; }
        public int space { get; set; }
        public List<SeatMap> seats { get; set; }

        public bool isCompleted()
        {
            var ok = true;
            do
            {
                if (seats == null)
                {
                    ok = false;
                    break;
                }
                foreach (var seat in seats)
                {
                    if (seat.selectedByUser)
                        ok = ok && (seat.selectedGender != 0);
                }

            } while (false);
            return ok;
        }
        public int seatCount()
        {
            var count = 0;
            foreach (var seat in seats)
            {
                if (seat.selectedByUser)
                    count++;
            }
            return count;
        }
        public string[] getSelectedSeat()
        {
            var selectedSeatList = new List<string>();
            foreach (var seat in seats)
            {
                if (seat.selectedByUser)
                {
                    selectedSeatList.Add($"{seat.seatNumber}/{seat.selectedGender}");
                }

            }
            return selectedSeatList.ToArray();
        }
        public string getSelectedSeatName()
        {
            var selectedSeatList = "";
            foreach (var seat in seats)
            {
                if (seat.selectedByUser)
                {
                    selectedSeatList += (selectedSeatList == "" ? "" : ", ");
                    selectedSeatList += $"{seat.seatNumber}/{getGenderName(seat.selectedGender)}";

                }
            }
            return selectedSeatList;
        }
        public string getGenderName(int genderId)
        {
            var ans = "";
            switch (genderId)
            {
                case 1:
                    ans = "خانم";
                    break;
                case 2:
                    ans = "آقا";
                    break;
                default:
                    ans = "نا مشخص";
                    break;
            }
            return ans;
        }

    }
    public class SeatMap
    {
        public int mapIndex { get; set; }
        public int seatNumber { get; set; }
        public int occupiedBy { get; set; }//0 empty, 1: female, 2: male

        public bool selectedByUser { get; set; }
        public int selectedGender { get; set; }

    }

    public class LastPath
    {
        public long id { get; set; }
        public string sourceTerminalShowName { get; set; }
        public string destinationTerminalShowName { get; set; }
    }


}