using System;

namespace Models.TrafficFine
{
    public class wsTrafficFine
    {

        public class GetTicketsCount_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetTicketsCount_Input_Parameters Parameters { get; set; }
        }
        public class GetTicketsCount_Input_Parameters
        {
            public string BarCode { get; set; }
            public long SessionID { get; set; }
        }
        public class GetTicketsCount_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetTicketsCount_Output_Parameters Parameters { get; set; }

        }
        public class GetTicketsCount_Output_Parameters
        {
            public string SaleKey { get; set; }
            public long TicketsCount { get; set; }
            public long TotalAmount { get; set; }
        }



        public class GetTicketsDetail_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetTicketsDetail_Input_Parameters Parameters { get; set; }
        }
        public class GetTicketsDetail_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }


        public class GetTicketsDetail_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetTicketsDetail_Output_Parameters Parameters { get; set; }
        }
        public class GetTicketsDetail_Output_Parameters
        {
            public int TicketsCount { get; set; }
            public int TotalAmount { get; set; }
            public GetTicketsDetail_Output_Parameters_Detail[] TicketsDetail { get; set; }
        }

        public class GetTicketsDetail_Output_Parameters_Detail
        {
            public long Amount { get; set; }
            public string City { get; set; }
            public int Code { get; set; }
            public DateTime DateTime { get; set; }
            public string Description { get; set; }
            public long ID { get; set; }
            public string LicensePlate { get; set; }
            public string Location { get; set; }
            public string Serial { get; set; }
            public string Type { get; set; }
        }


        //*******
        public class SelectTickets_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public SelectTickets_Input_Parameters Parameters { get; set; }
        }
        public class SelectTickets_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
            public long[] TicketsList { get; set; }
        }


        public class SelectTickets_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public SelectTickets_Output_Parameters Parameters { get; set; }
        }
        public class SelectTickets_Output_Parameters
        {
            public long TicketsCount { get; set; }
            public long TotalAmount { get; set; }
        }





        public class GetPurchaseInfo_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetPurchaseInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetPurchaseInfo_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetPurchaseInfo_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetPurchaseInfo_Output_Parameters[] Parameters { get; set; }
        }
        public class GetPurchaseInfo_Output_Parameters
        {
            public long Amount { get; set; }
            public string BillID { get; set; }
            public string City { get; set; }
            public int Code { get; set; }
            public DateTime DateTime { get; set; }
            public string Description { get; set; }
            public long ID { get; set; }
            public string LicensePlate { get; set; }
            public string Location { get; set; }
            public string Serial { get; set; }
            public string Type { get; set; }
            public string PaymentID { get; set; }
            public string PaymentStatus { get; set; }
            public string PaymentTraceNumber { get; set; }
        }

        //********************************
        public class SingleTicketPayment_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public SingleTicketPayment_Input_Parameters Parameters { get; set; }
        }
        public class SingleTicketPayment_Input_Parameters
        {
            public string BillID { get; set; }
            public string PaymentID { get; set; }
            public long SessionID { get; set; }
        }
        public class SingleTicketPayment_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public SingleTicketPayment_Output_Parameters Parameters { get; set; }

        }
        public class SingleTicketPayment_Output_Parameters
        {
            public string SaleKey { get; set; }
            public long PaymentLink { get; set; }
            public long Amount { get; set; }
        }

        //****************

        public class TrafficFinesInquiry_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public TrafficFinesInquiry_Input_Parameters Parameters { get; set; }
        }
        public class TrafficFinesInquiry_Input_Parameters
        {
            public string BarCode { get; set; }
            public long SessionID { get; set; }
        }
        public class TrafficFinesInquiry_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public TrafficFinesInquiry_Output_Parameters Parameters { get; set; }

        }
        public class TrafficFinesInquiry_Output_Parameters
        {
            public string SaleKey { get; set; }
            public bool TwoPhaseInquiry { get; set; }
        }

        //****************


        public class GetCaptcha_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetCaptcha_Input_Parameters Parameters { get; set; }
        }
        public class GetCaptcha_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }
        public class GetCaptcha_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public GetCaptcha_Output_Parameters Parameters { get; set; }

        }
        public class GetCaptcha_Output_Parameters
        {
            public string CaptchaUrl { get; set; }
            public string CaptchaBase64 { get; set; }
        }

        //****************


        public class SolveCaptcha_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public SolveCaptcha_Input_Parameters Parameters { get; set; }
        }
        public class SolveCaptcha_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
            public string CaptchaText { get; set; }
        }
        public class SolveCaptcha_Output
        {
            public Core.wsInterface.Status Status { get; set; }

        }

    }
}