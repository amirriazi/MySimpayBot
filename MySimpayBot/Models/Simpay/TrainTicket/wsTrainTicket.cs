using System;

namespace Models.TrainTicket
{
    public class wsTrainTicket
    {
        public class GetSourceStationsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSourceStationsList_Input_Parameters Parameters { get; set; }
        }
        public class GetSourceStationsList_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetSourceStationsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetSourceStationsList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetSourceStationsList_Output_Parameters
        {
            public int StationCode { get; set; }
            public string StationShowName { get; set; }
        }

        //*****************************************

        public class GetDestinationStationsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetDestinationStationsList_Input_Parameters Parameters { get; set; }
        }
        public class GetDestinationStationsList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int SourceStationCode { get; set; }
        }

        public class GetDestinationStationsList_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetDestinationStationsList_Output_Parameters[] Parameters { get; set; }

        }

        public class GetDestinationStationsList_Output_Parameters
        {
            public int StationCode { get; set; }
            public string StationShowName { get; set; }
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
            public int DestinationStationCode { get; set; }
            public bool JustCompartment { get; set; }
            public int SeatCount { get; set; }
            public int SourceStationCode { get; set; }
            public int TicketTypeCode { get; set; }
            public bool TwoWay { get; set; }
            public DateTime? WayGoDateTime { get; set; }
            public DateTime? WayReturnDateTime { get; set; }
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
            public bool AirConditioning { get; set; }
            public string ArrivalTime { get; set; }
            public int AvailableCapacity { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public int DiscountedAmount { get; set; }
            public bool IsCompartment { get; set; }
            public bool Media { get; set; }
            public int RealAmount { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string TrainName { get; set; }
            public int TrainNumber { get; set; }
            public string TrainType { get; set; }
        }

        //*********************



        public class GetServiceDetailInfo_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetServiceDetailInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetServiceDetailInfo_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
            public string WayGoServiceUniqueIdentifier { get; set; }
            public string WayReturnServiceUniqueIdentifier { get; set; }
        }

        public class GetServiceDetailInfo_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetServiceDetailInfo_Output_Parameters Parameters { get; set; }

        }

        public class GetServiceDetailInfo_Output_Parameters
        {
            public bool JustCompartment { get; set; }
            public int SeatCount { get; set; }
            public int TicketTypeCode { get; set; }
            public bool TwoWay { get; set; }
            public GetServiceDetailInfo_Output_Parameters_OptionalServicesList[] WayGoOptionalServicesList { get; set; }
            public GetServiceDetailInfo_Output_Parameters_OptionalServicesList[] WayReturnOptionalServicesList { get; set; }
            public GetServiceDetailInfo_Output_Parameters_Services WayGoService { get; set; }
            public GetServiceDetailInfo_Output_Parameters_Services WayReturnService { get; set; }

        }


        public class GetServiceDetailInfo_Output_Parameters_OptionalServicesList
        {
            public int Amount { get; set; }
            public int Code { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public string OptionalServiceUniqueIdentifier { get; set; }
        }

        public class GetServiceDetailInfo_Output_Parameters_Services
        {
            public bool AirConditioning { get; set; }
            public string ArrivalTime { get; set; }
            public int AvailableCapacity { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public int DiscountedAmount { get; set; }
            public bool IsCompartment { get; set; }
            public bool Media { get; set; }
            public int RealAmount { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string TrainName { get; set; }
            public int TrainNumber { get; set; }
            public string TrainType { get; set; }
        }


        //*********************

        //*********************



        public class SetTicketInfo_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public SetTicketInfo_Input_Parameters Parameters { get; set; }
        }
        public class SetTicketInfo_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
            public string[] WayGoPassengersInfo { get; set; }
            public string[] WayReturnPassengersInfo { get; set; }
        }

        public class SetTicketInfo_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public SetTicketInfo_Output_Parameters Parameters { get; set; }

        }

        public class SetTicketInfo_Output_Parameters
        {
            public int CompartmentAmount { get; set; }
            public bool JustCompartment { get; set; }
            public int SeatAmount { get; set; }
            public int SeatCount { get; set; }
            public int TicketTypeCode { get; set; }
            public int TotalAmount { get; set; }
            public bool TwoWay { get; set; }
            public SetTicketInfo_Output_Parameters_PassengersList[] WayGoPassengersList { get; set; }
            public SetTicketInfo_Output_Parameters_Service WayGoService { get; set; }
            public SetTicketInfo_Output_Parameters_PassengersList[] WayReturnPassengersList { get; set; }
            public SetTicketInfo_Output_Parameters_Service WayReturnService { get; set; }

        }


        public class SetTicketInfo_Output_Parameters_PassengersList
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NationalCode { get; set; }
            public string TicketPassengerTypeShowName { get; set; }
        }

        public class SetTicketInfo_Output_Parameters_Service
        {
            public bool AirConditioning { get; set; }
            public string ArrivalTime { get; set; }
            public int AvailableCapacity { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public int DiscountedAmount { get; set; }
            public bool IsCompartment { get; set; }
            public bool Media { get; set; }
            public int RealAmount { get; set; }
            public string ServiceUniqueIdentifier { get; set; }
            public string TrainName { get; set; }
            public int TrainNumber { get; set; }
            public string TrainType { get; set; }
        }

        //********************************************
        public class LockSeat_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public LockSeat_Input_Parameters Parameters { get; set; }
        }
        public class LockSeat_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
        }

        public class LockSeat_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public LockSeat_Output_Parameters Parameters { get; set; }

        }

        public class LockSeat_Output_Parameters
        {
            public LockSeat_Output_Parameters_Way WayGo { get; set; }
            public LockSeat_Output_Parameters_Way WayReturn { get; set; }
        }

        public class LockSeat_Output_Parameters_Way
        {
            public bool IsCompartment { get; set; }
            public int LockedRowNumber { get; set; }
            public int LockedWagonNumber { get; set; }
        }
        //*****************************************
        //********************************************
        public class RedeemTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public RedeemTicket_Input_Parameters Parameters { get; set; }
        }
        public class RedeemTicket_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
        }

        public class RedeemTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }

        }

        //*****************************************


        public class PrintTickets_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public PrintTickets_Input_Parameters Parameters { get; set; }
        }
        public class PrintTickets_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
        }

        public class PrintTickets_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public PrintTickets_Output_Parameters Parameters { get; set; }
        }

        public class PrintTickets_Output_Parameters
        {
            public bool TwoWay { get; set; }
            public PrintTickets_Output_Parameters_WayTicketsList[] WayGoTicketsList { get; set; }
            public PrintTickets_Output_Parameters_WayTicketsList[] WayReturnTicketsList { get; set; }
        }

        public class PrintTickets_Output_Parameters_WayTicketsList
        {
            public string ArrivalTime { get; set; }
            public string BarCodeImage { get; set; }
            public int CompartmentNumber { get; set; }
            public int Degree { get; set; }
            public DateTime DepartureDateTime { get; set; }
            public string DestinationStationShowName { get; set; }
            public string FirstName { get; set; }
            public string Food { get; set; }
            public string LastName { get; set; }
            public string NationalCode { get; set; }
            public string PassengerTypeShowName { get; set; }
            public int PersonCode { get; set; }
            public DateTime RegistrationDateTime { get; set; }
            public long SaleID { get; set; }
            public long SeatNumber { get; set; }
            public long Services { get; set; }
            public long ServicesAmount { get; set; }
            public string SourceStationShowName { get; set; }
            public long TicketNumber { get; set; }
            public string TicketSeries { get; set; }
            public string TicketTypeShowName { get; set; }
            public long TotalServicesAmount { get; set; }
            public long TrackingNumber { get; set; }
            public long TrainNumber { get; set; }
            public string WagonName { get; set; }
            public long WagonNumber { get; set; }
            public long WagonType { get; set; }
        }
        //*****************************************


    }
}