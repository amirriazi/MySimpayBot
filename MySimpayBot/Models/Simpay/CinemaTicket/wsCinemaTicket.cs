using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CinemaTicket
{
    public class wsCinemaTicket
    {
        public class GetCinemaSansSeatsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetCinemaSansSeatsList_Input_Parameters Parameters { get; set; }
        }
        public class GetCinemaSansSeatsList_Input_Parameters
        {
            public int CinemaCode { get; set; }
            public int SansCode { get; set; }
            public long SessionID { get; set; }
        }

        public class GetCinemaSansSeatsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetCinemaSansSeatsList_Output_Parameters Parameters { get; set; }

        }

        public class GetCinemaSansSeatsList_Output_Parameters
        {
            public GetCinemaSansSeatsList_Output_Parameters_Detail[] Detail { get; set; }
            public int MaxSeatsColumnsCount { get; set; }
        }

        public class GetCinemaSansSeatsList_Output_Parameters_Detail
        {
            public string RealRowNumber { get; set; }
            public string RealSeatNumber { get; set; }
            public string RowNumber { get; set; }
            public int SeatNumber { get; set; }
            public int State { get; set; }
        }

        //*****************************************


        public class CalculateAmount_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public CalculateAmount_Input_Parameters Parameters { get; set; }
        }
        public class CalculateAmount_Input_Parameters
        {
            public int CinemaCode { get; set; }
            public int Count { get; set; }
            public int SansCode { get; set; }
            public long SessionID { get; set; }
        }

        public class CalculateAmount_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public CalculateAmount_Output_Parameters Parameters { get; set; }

        }

        public class CalculateAmount_Output_Parameters
        {

            public int TotalAmount { get; set; }
        }


        //*****************************************

        public class OrderTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public OrderTicket_Input_Parameters Parameters { get; set; }
        }
        public class OrderTicket_Input_Parameters
        {
            public int CinemaCode { get; set; }
            public int Count { get; set; }
            public int SansCode { get; set; }
            public long SessionID { get; set; }
        }

        public class OrderTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public OrderTicket_Output_Parameters Parameters { get; set; }

        }

        public class OrderTicket_Output_Parameters
        {

            public string SaleKey { get; set; }
            public int TotalAmount { get; set; }
        }


        //*****************************************


        public class GetSaleInfoForTicketBuying_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSaleInfoForTicketBuying_Input_Parameters Parameters { get; set; }
        }
        public class GetSaleInfoForTicketBuying_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetSaleInfoForTicketBuying_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetSaleInfoForTicketBuying_Output_Parameters Parameters { get; set; }

        }
        public class GetSaleInfoForTicketBuying_Output_Parameters
        {
            public int CinemaCode { get; set; }
            public int SansCode { get; set; }
            public int TicketCount { get; set; }
        }

        //*****************************************


        public class BuyTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public BuyTicket_Input_Parameters Parameters { get; set; }
        }
        public class BuyTicket_Input_Parameters
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string SaleKey { get; set; }
            public int[] SeatsNumber { get; set; }
            public long SessionID { get; set; }
        }

        public class BuyTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }


        }


        //*****************************************

        public class PrintTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public PrintTicket_Input_Parameters Parameters { get; set; }
        }
        public class PrintTicket_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class PrintTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public PrintTicket_Output_Parameters Parameters { get; set; }

        }
        public class PrintTicket_Output_Parameters
        {
            public int CinemaCode { get; set; }
            public string CinemaName { get; set; }
            public DateTime Date { get; set; }
            public string FilmName { get; set; }
            public string FullName { get; set; }
            public string ReserveCode { get; set; }
            public string SalonName { get; set; }
            public int SansCode { get; set; }
            public PrintTicket_Output_Parameters_Seats[] Seats { get; set; }
            public int TicketCount { get; set; }
            public string Time { get; set; }
            public int TotalAmount { get; set; }
        }

        public class PrintTicket_Output_Parameters_Seats
        {
            public int Number { get; set; }
            public string RowNumber { get; set; }
        }
    }
}
