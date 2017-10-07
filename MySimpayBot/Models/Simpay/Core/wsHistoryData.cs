using System;

namespace Models.Core
{
    public class wsHistoryData
    {
        //*****************

        public class History_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public History_Input_Parameters Parameters { get; set; }
        }
        public class History_Input_Parameters
        {
            public long SessionID { get; set; }
            public long ProductID { get; set; }
            public DateTime FromDateTime { get; set; }
            public DateTime ToDateTime { get; set; }
            public long Limit { get; set; }
            public long Offset { get; set; }



        }

        public class History_Output
        {
            public wsInterface.Status Status { get; set; }
            public History_Output_Parameters Parameters { get; set; }

        }
        public class History_Output_Parameters
        {
            public History_Output_Parameters_Detail[] Detail { get; set; }
            public bool HasMore { get; set; }
        }

        public class History_Output_Parameters_Detail
        {
            public long Amount { get; set; }
            public DateTime DateTime { get; set; }
            public object ExtraInfo { get; set; }
            public string HijryDateTime { get; set; }
            public long ProductID { get; set; }
            public string ProductShowName { get; set; }
            public long RecordID { get; set; }
            public string SaleKey { get; set; }
        }
        public class History_Output_Parameters_ExtralInfo_Xpin
        {
            public string __type { get; set; }
            public int Amount { get; set; }
            public string Description { get; set; }
            public string PinCode { get; set; }
        }
        public class History_Output_Parameters_ExtralInfo_AirplaneTicket
        {
            public string __type { get; set; }
            public bool TwoWay { get; set; }
            public bool HasMore { get; set; }
            public History_Output_Parameters_ExtralInfo_AirplaneTicket_TicketsList[] WayGoTicketsList { get; set; }
            public History_Output_Parameters_ExtralInfo_AirplaneTicket_TicketsList[] WayReturnTicketsList { get; set; }
        }
        public class History_Output_Parameters_ExtralInfo_AirplaneTicket_TicketsList
        {
            public string AgeType { get; set; }
            public string AirlineCode { get; set; }
            public string AirlineNumber { get; set; }
            public string FirstName { get; set; }
            public string Html { get; set; }
            public string LastName { get; set; }
            public string PNR { get; set; }
            public string TicketNumber { get; set; }

        }

        public class History_Output_Parameters_ExtralInfo_Traffic
        {
            public string __type { get; set; }
            public TrafficFine.wsTrafficFine.GetPurchaseInfo_Output_Parameters[] Tickets { get; set; }
        }

        public class History_Output_Parameters_ExtralInfo_BillPayment
        {
            public string __type { get; set; }
            public int Amount { get; set; }
            public string BillID { get; set; }
            public string BillType { get; set; }
            public string PaymentID { get; set; }
            public string PaymentTraceNumber { get; set; }

        }
        public class History_Output_Parameters_ExtralInfo_AutoCharge
        {
            public int Amount { get; set; }
            public string ChargeTypeShowName { get; set; }
            public string MobileNumber { get; set; }
        }
        public class History_Output_Parameters_ExtralInfo_PinCharge
        {
            public int Amount { get; set; }
            public string ChargeTypeShowName { get; set; }
            public string Description { get; set; }
            public string PinCode { get; set; }
        }


        public class History_Output_Parameters_ExtralInfo_WayGoTicketsList
        {
            public string AgeType { get; set; }
            public string AirlineCode { get; set; }
            public string AirlineNumber { get; set; }
            public string FirstName { get; set; }
            public string Html { get; set; }
            public string LastName { get; set; }
            public string PNR { get; set; }
            public string TicketNumber { get; set; }
        }

        public class History_Output_Parameters_ExtralInfo_WayReturnTicketsList
        {
            public string AgeType { get; set; }
            public string AirlineCode { get; set; }
            public string AirlineNumber { get; set; }
            public string FirstName { get; set; }
            public string Html { get; set; }
            public string LastName { get; set; }
            public string PNR { get; set; }
            public string TicketNumber { get; set; }
        }

        public class History_Output_Parameters_ExtralInfo_MciMobileBill
        {
            public int Amount { get; set; }
            public string BillID { get; set; }
            public string BillType { get; set; }
            public string PaymentID { get; set; }
            public string PaymentTraceNumber { get; set; }

        }
        public class History_Output_Parameters_ExtralInfo_BusTicket
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