using System;

namespace Models.AirplaneTicket
{
    public class wsAirplane
    {
        public class GetSourceStationsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSourceAirportsList_Input_Parameters Parameters { get; set; }
        }
        public class GetSourceAirportsList_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetSourceAirportsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetSourceAirportsList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetSourceAirportsList_Output_Parameters
        {
            public string AirportCode { get; set; }
            public string AirportShowName { get; set; }
        }

        //*****************************************

        public class GetServicesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetServicesList_Input_Parameters Parameters { get; set; }
        }
        public class GetServicesList_Input_Parameters
        {
            public long SessionID { get; set; }
            public long AdultCount { get; set; }
            public long ChildCount { get; set; }
            public string DestinationAirportCode { get; set; }
            public long InfantCount { get; set; }
            public string SourceAirportCode { get; set; }
            public bool TwoWay { get; set; }
            public DateTime WayGoDateTime { get; set; }
            public DateTime WayReturnDateTime { get; set; }
        }

        public class GetServicesList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetServicesList_Output_Parameters Parameters { get; set; }

        }

        public class GetServicesList_Output_Parameters
        {
            public string SaleKey { get; set; }
            public GetServicesList_Output_Parameters_ServicesList[] WayGoServicesList { get; set; }
            public GetServicesList_Output_Parameters_ServicesList[] WayReturnServicesList { get; set; }
        }

        public class GetServicesList_Output_Parameters_ServicesList
        {
            public long ID { get; set; }
            public string SourceAirportCode { get; set; }
            public string DestinationAirportCode { get; set; }
            public string Aircraft { get; set; }
            public string AirlineCode { get; set; }
            public string FlightID { get; set; }
            public string FlightNumber { get; set; }
            public string Class { get; set; }
            public string ClassType { get; set; }
            public string ClassTypeName { get; set; }

            public int AmountAdult { get; set; }
            public int AmountChild { get; set; }
            public int AmountInfant { get; set; }
            public string ArrivalTime { get; set; }
            public string DayOfWeek { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public string Description { get; set; }

            public bool IsCharter { get; set; }
            public string SellerID { get; set; }
            public string SellerReference { get; set; }
            public string ServiceUniqueIdentifier { get; set; }

            public string Status { get; set; }
            public string StatusName { get; set; }
            public string SystemKey { get; set; }
        }

        //*************************
        public class ReserveTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public ReserveTicket_Input_Parameters Parameters { get; set; }
        }
        public class ReserveTicket_Input_Parameters
        {
            public string[] PassengersInfo { get; set; }
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
            public string WayGoServiceUniqueIdentifier { get; set; }
            public string WayReturnServiceUniqueIdentifier { get; set; }
        }

        public class ReserveTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public ReserveTicket_Output_Parameters Parameters { get; set; }

        }

        public class ReserveTicket_Output_Parameters
        {
            public int TotalAmount { get; set; }
            public bool TwoWay { get; set; }
            public ReserveTicket_Output_Parameters_PassengersList[] WayGoPassengersList { get; set; }
            public ReserveTicket_Output_Parameters_Service WayGoService { get; set; }
            public ReserveTicket_Output_Parameters_PassengersList[] WayReturnPassengersList { get; set; }
            public ReserveTicket_Output_Parameters_Service WayReturnService { get; set; }
        }

        public class ReserveTicket_Output_Parameters_PassengersList
        {
            public string AgeTypeShowName { get; set; }
            public long Amount { get; set; }
            public DateTime BirthDate { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public class ReserveTicket_Output_Parameters_Service
        {
            public string Aircraft { get; set; }
            public string AirlineCode { get; set; }
            public long AmountAdult { get; set; }
            public long AmountChild { get; set; }
            public long AmountInfant { get; set; }
            public string ArrivalTime { get; set; }
            public string Class { get; set; }
            public string ClassType { get; set; }
            public string ClassTypeName { get; set; }
            public string DayOfWeek { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public string Description { get; set; }
            public string DestinationAirportCode { get; set; }
            public string FlightID { get; set; }
            public string FlightNumber { get; set; }
            public long ID { get; set; }
            public bool IsCharter { get; set; }
            public string SellerID { get; set; }
            public string SellerReference { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string SourceAirportCode { get; set; }
            public string Status { get; set; }
            public string StatusName { get; set; }
            public string SystemKey { get; set; }
        }
        //*****************************************
        public class RedeemTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public RedeemTicket_Input_Parameters Parameters { get; set; }
        }
        public class RedeemTicket_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class RedeemTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }

        }


        //*************************
        public class PrintTickets_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public PrintTickets_Input_Parameters Parameters { get; set; }
        }
        public class PrintTickets_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class PrintTickets_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public PrintTickets_Output_Parameters Parameters { get; set; }

        }
        public class PrintTickets_Output_Parameters
        {
            public bool TwoWay { get; set; }
            public PrintTickets_Output_Parameters_TicketsList[] WayGoTicketsList { get; set; }
            public PrintTickets_Output_Parameters_TicketsList[] WayReturnTicketsList { get; set; }
        }

        public class PrintTickets_Output_Parameters_TicketsList
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
    }
}