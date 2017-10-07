using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.EventsTicket
{
    public class wsEventsTicket
    {
        public class GetEventsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetEventsList_Input_Parameters Parameters { get; set; }
        }
        public class GetEventsList_Input_Parameters
        {
            public long SessionID { get; set; }
            public string Category { get; set; }
        }

        public class GetEventsList_Output
        {
            public GetEventsList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetEventsList_Output_Parameters
        {
            public string AmountsText { get; set; }
            public bool AvailableForSale { get; set; }
            public DateTime EndDate { get; set; }
            public string ImageThumbnailURL { get; set; }
            public string ImageURL { get; set; }
            public string Method { get; set; }
            public string ShortDescription { get; set; }
            public DateTime StartDate { get; set; }
            public string TimesText { get; set; }
            public string Title { get; set; }
            public string UniqueIdentifier { get; set; }
            public string VenueTitle { get; set; }
        }

        //************************************************************


        public class GetEventDetailInfo_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetEventDetailInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetEventDetailInfo_Input_Parameters
        {
            public long SessionID { get; set; }
            public string EventUniqueIdentifier { get; set; }
        }

        public class GetEventDetailInfo_Output
        {
            public GetEventDetailInfo_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetEventDetailInfo_Output_Parameters
        {
            public string UniqueIdentifier { get; set; }
            public bool Active { get; set; }
            public string AmountsText { get; set; }
            public string Behavior { get; set; }
            public long Code { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool HasChildPages { get; set; }
            public bool HasDiscount { get; set; }
            public bool HasSale { get; set; }
            public string ImageThumbnailURL { get; set; }
            public string ImageURL { get; set; }

            public string Method { get; set; }
            public string Name { get; set; }
            public string PromoDescription { get; set; }
            public string SaleKey { get; set; }
            public string ShortDescription { get; set; }
            public string TimesText { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }

            public string VenueAddress { get; set; }
            public long VenueCode { get; set; }
            public string VenueTelPhone { get; set; }
            public string VenueTitle { get; set; }
            public string[] Specifications { get; set; }
            public GetEventDetailInfo_Output_Parameters_Instance[] Instances { get; set; }
        }
        public class GetEventDetailInfo_Output_Parameters_Instance
        {
            public string UniqueIdentifier { get; set; }
            public DateTime DateTime { get; set; }
            public string AmountText { get; set; }
            public long Capacity { get; set; }
            public long Code { get; set; }
            public long Remained { get; set; }
            public string RemainedText { get; set; }
            public long SaleID { get; set; }
            public string Title { get; set; }
        }


        //************************************************************
        public class ReserveTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public ReserveTicket_Input_Parameters Parameters { get; set; }
        }
        public class ReserveTicket_Input_Parameters
        {
            public long SessionID { get; set; }
            public string EmailAddress { get; set; }
            public string FullName { get; set; }
            public string SaleKey { get; set; }
            public string[] Seats { get; set; }
        }

        public class ReserveTicket_Output
        {
            public ReserveTicket_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class ReserveTicket_Output_Parameters
        {
            public int Amount { get; set; }
            public string Description { get; set; }
            public string Seats { get; set; }
        }

        //************************************************************
        public class BuyTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public BuyTicket_Input_Parameters Parameters { get; set; }
        }
        public class BuyTicket_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
        }

        public class BuyTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }
        }

        //************************************************************
        public class PrintTicket_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public PrintTicket_Input_Parameters Parameters { get; set; }
        }
        public class PrintTicket_Input_Parameters
        {
            public long SessionID { get; set; }
            public string SaleKey { get; set; }
        }

        public class PrintTicket_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public PrintTicket_Output_Parameters Parameters { get; set; }
        }

        public class PrintTicket_Output_Parameters
        {
            public string AttachmentURL { get; set; }
            public string EmailAddress { get; set; }
            public string FullName { get; set; }
            public long InstanceID { get; set; }
            public string InstanceTitle { get; set; }
            public string ItemBehavior { get; set; }
            public string MobileNumber { get; set; }
            public string PaymentIdentifier { get; set; }
            public long ReserveID { get; set; }
            public string SaleDeliverType { get; set; }
            public string SaleMethod { get; set; }
            public string SaleName { get; set; }
            public string SaleTitle { get; set; }
            public int SeatCount { get; set; }
            public string Seats { get; set; }
            public string TraceNumber { get; set; }
            public string VenueAddress { get; set; }
            public long VenueCode { get; set; }
            public string VenueTelPhone { get; set; }
            public string VenueTitle { get; set; }
        }

        //************************************************************
        public class GetEventInstanceSeatMap_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetEventInstanceSeatMap_Input_Parameters Parameters { get; set; }
        }
        public class GetEventInstanceSeatMap_Input_Parameters
        {
            public string InstanceUniqueIdentifier { get; set; }
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetEventInstanceSeatMap_Output
        {
            public GetEventInstanceSeatMap_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetEventInstanceSeatMap_Output_Parameters
        {
            public string CSS { get; set; }
            public string HTML { get; set; }
            public long VenueCode { get; set; }
        }


    }
}