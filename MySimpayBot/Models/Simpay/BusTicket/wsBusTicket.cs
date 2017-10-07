using System;

namespace Models.BusTicket
{
    public class wsBusTicket
    {
        public class GetSourceStatesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSourceStatesList_Input_Parameters Parameters { get; set; }
        }
        public class GetSourceStatesList_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetSourceStatesList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetSourceStatesList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetSourceStatesList_Output_Parameters
        {
            public int StateCode { get; set; }
            public string StateShowName { get; set; }
        }

        //*****************************************

        public class GetSourceTerminalsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSourceTerminalsList_Input_Parameters Parameters { get; set; }
        }
        public class GetSourceTerminalsList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int StateCode { get; set; }
        }

        public class GetSourceTerminalsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetSourceTerminalsList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetSourceTerminalsList_Output_Parameters
        {
            public int TerminalCode { get; set; }
            public string TerminalShowName { get; set; }

        }

        //******************************************************
        public class GetDestinationStatesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetDestinationStatesList_Input_Parameters Parameters { get; set; }
        }
        public class GetDestinationStatesList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int SourceTerminalCode { get; set; }
        }

        public class GetDestinationStatesList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetDestinationStatesList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetDestinationStatesList_Output_Parameters
        {
            public int StateCode { get; set; }
            public string StateShowName { get; set; }
        }

        //*****************************************

        public class GetDestinationTerminalsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetDestinationTerminalsList_Input_Parameters Parameters { get; set; }
        }
        public class GetDestinationTerminalsList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int SourceTerminalCode { get; set; }
            public int StateCode { get; set; }
        }

        public class GetDestinationTerminalsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetDestinationTerminalsList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetDestinationTerminalsList_Output_Parameters
        {
            public int TerminalCode { get; set; }
            public string TerminalShowName { get; set; }

        }

        //******************************************************


        public class GetServicesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetServicesList_Input_Parameters Parameters { get; set; }
        }
        public class GetServicesList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int SourceTerminalCode { get; set; }
            public int DestinationTerminalCode { get; set; }
            public DateTime DateTime { get; set; }
        }

        public class GetServicesList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetServicesList_Output_Parameters Parameters { get; set; }

        }

        public class GetServicesList_Output_Parameters
        {
            public string SaleKey { get; set; }
            public GetServicesList_Output_Parameters_Detail[] Detail { get; set; }
        }
        public class GetServicesList_Output_Parameters_Detail
        {
            public int Amount { get; set; }
            public string BusType { get; set; }
            public int Capacity { get; set; }
            public string CorporationName { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public string DestinationTerminalShowName { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string SourceTerminalShowName { get; set; }

        }

        //******************************************************



        public class GetServiceSeats_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetServiceSeats_Input_Parameters Parameters { get; set; }
        }
        public class GetServiceSeats_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
        }

        public class GetServiceSeats_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetServiceSeats_Output_Parameters Parameters { get; set; }

        }

        public class GetServiceSeats_Output_Parameters
        {

            public int Capacity { get; set; }
            public int ColumnNumber { get; set; }
            public int Floor { get; set; }
            public int RowNumber { get; set; }
            public int Space { get; set; }
            public string[] Seats { get; set; }

        }

        //******************************************************


        public class ReserveSeats_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public ReserveSeats_Input_Parameters Parameters { get; set; }
        }
        public class ReserveSeats_Input_Parameters
        {
            public long SessionID { get; set; }
            public string FullName { get; set; }
            public string SaleKey { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string[] Seats { get; set; }
        }

        public class ReserveSeats_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public ReserveSeats_Output_Parameters Parameters { get; set; }

        }

        public class ReserveSeats_Output_Parameters
        {
            public int TotalAmount { get; set; }

        }

        //******************************************************
        public class Redeem_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public Redeem_Input_Parameters Parameters { get; set; }
        }
        public class Redeem_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class Redeem_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public Redeem_Output_Parameters Parameters { get; set; }
        }
        public class Redeem_Output_Parameters
        {
            public string CorporationName { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public string DestinationTerminalShowName { get; set; }
            public string PassengerName { get; set; }
            public DateTime PurchaseDateTime { get; set; }
            public string PurchaseID { get; set; }
            public string[] Seats { get; set; }
            public long SeatsCount { get; set; }
            public string SourceTerminalShowName { get; set; }
            public string TicketNumber { get; set; }
            public int TotalAmount { get; set; }
        }


    }
}