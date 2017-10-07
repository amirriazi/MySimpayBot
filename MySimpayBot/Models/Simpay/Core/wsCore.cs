using System;

namespace Models.Core
{
    public class wsCore
    {
        public class GetSalePaymentLink_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetSalePaymentLink_Input_Parameters Parameters { get; set; }
        }
        public class GetSalePaymentLink_Input_Parameters
        {
            public long IPGID { get; set; }
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetSalePaymentLink_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetSalePaymentLink_Output_Parameters Parameters { get; set; }

        }
        public class GetSalePaymentLink_Output_Parameters
        {
            public DateTime PaymentExpirationDateTime { get; set; }
            public string PaymentLink { get; set; }
        }


        //*****************

        public class GetSalePaymentInfo_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetSalePaymentInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetSalePaymentInfo_Input_Parameters
        {
            public string DiscountCode { get; set; }
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetSalePaymentInfo_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetSalePaymentInfo_Output_Parameters Parameters { get; set; }

        }
        public class GetSalePaymentInfo_Output_Parameters
        {
            public long Amount { get; set; }
            public string Description { get; set; }
            public long DiscountAmount { get; set; }
            public bool PaymentIsPossible { get; set; }
            public long ProductID { get; set; }
            public string ProductName { get; set; }
        }



        //*****************

        public class GetPurchaseHistoryProductsList_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetPurchaseHistoryProductsList_Input_Parameters Parameters { get; set; }
        }
        public class GetPurchaseHistoryProductsList_Input_Parameters
        {
            public DateTime FromDateTime { get; set; }
            public DateTime ToDateTime { get; set; }
            public long SessionID { get; set; }
        }

        public class GetPurchaseHistoryProductsList_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetPurchaseHistoryProductsList_Output_Parameters[] Parameters { get; set; }

        }
        public class GetPurchaseHistoryProductsList_Output_Parameters
        {
            public long ProductID { get; set; }
            public string ProductShowName { get; set; }
        }

        //*****************************

        //*****************

        public class GetUnfinishedPaymentsInfo_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetUnfinishedPaymentsInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetUnfinishedPaymentsInfo_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetUnfinishedPaymentsInfo_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetUnfinishedPaymentsInfo_Output_Parameters[] Parameters { get; set; }

        }
        public class GetUnfinishedPaymentsInfo_Output_Parameters
        {
            public long Amount { get; set; }
            public string ApplicationTypeShowName { get; set; }
            public DateTime DateTime { get; set; }
            public string Description { get; set; }
            public long ProductID { get; set; }
            public string ProductShowName { get; set; }
            public string SaleKey { get; set; }
        }



        //*****************

        public class GetIPGsList_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetIPGsList_Input_Parameters Parameters { get; set; }
        }
        public class GetIPGsList_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class GetIPGsList_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetIPGsList_Output_Parameters[] Parameters { get; set; }

        }
        public class GetIPGsList_Output_Parameters
        {
            public bool Default { get; set; }
            public int ID { get; set; }
            public string IPGThumbnail { get; set; }
            public string ShowName { get; set; }
        }

        //*********************************************



        public class TokenHeader
        {
            public string fdt { get; set; }
            public string tdt { get; set; }
            public int sea { get; set; }
            public string pjn { get; set; }
            public bool pra { get; set; }
            public int sid { get; set; }
        }



        //*****************

        public class GetInvitationInfo_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetInvitationInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetInvitationInfo_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetInvitationInfo_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetInvitationInfo_Output_Parameters Parameters { get; set; }

        }
        public class GetInvitationInfo_Output_Parameters
        {
            public string InvitationCode { get; set; }
            public string Link { get; set; }
            public string Summery { get; set; }
            public string TextToShare { get; set; }
        }

        //*********************************************
        public class ReportInvitation_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public ReportInvitation_Input_Parameters Parameters { get; set; }
        }
        public class ReportInvitation_Input_Parameters
        {
            public string InvitationCode { get; set; }
            public string MobileNumber { get; set; }
        }

        public class ReportInvitation_Output
        {
            public wsInterface.Status Status { get; set; }
            public ReportInvitation_Output_Parameters Parameters { get; set; }

        }
        public class ReportInvitation_Output_Parameters
        {
            public string Footer { get; set; }
            public string Header { get; set; }
            public ReportInvitation_Output_Parameters_Platform[] Platforms { get; set; }
        }

        public class ReportInvitation_Output_Parameters_Platform
        {
            public string Link { get; set; }
            public string Thumbnail { get; set; }
            public string Title { get; set; }
        }



        //*********************************************
        public class GetInvitationStatus_Input
        {
            public wsInterface.Identity Identity { get; set; }
            public GetInvitationStatus_Input_Parameters Parameters { get; set; }
        }
        public class GetInvitationStatus_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetInvitationStatus_Output
        {
            public wsInterface.Status Status { get; set; }
            public GetInvitationStatus_Output_Parameters Parameters { get; set; }

        }
        public class GetInvitationStatus_Output_Parameters
        {
            public long ApprovedInvitationCount { get; set; }
            public string Description { get; set; }
            public long TotalInvitationCount { get; set; }
            public long SentDiscountCount { get; set; }
        }



    }

}