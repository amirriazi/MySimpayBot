﻿
using System;

namespace Models.GasBill
{
    public class wsGasBill
    {
        public class BillInquiry_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public BillInquiry_Input_Parameters Parameters { get; set; }
        }
        public class BillInquiry_Input_Parameters
        {
            public string GasParticipateCode { get; set; }
            public long SessionID { get; set; }
        }

        public class BillInquiry_Output
        {
            public BillInquiry_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class BillInquiry_Output_Parameters
        {
            public long Amount { get; set; }
            public string BillID { get; set; }
            public DateTime FromDate { get; set; }
            public string FullName { get; set; }
            public DateTime PaymentDeadLineDate { get; set; }
            public string PaymentID { get; set; }
            public DateTime ToDate { get; set; }

        }

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
            public int Amount { get; set; }
            public string BillID { get; set; }
            public string BillType { get; set; }
            public string PaymentID { get; set; }
            public string PaymentTraceNumber { get; set; }

        }


    }
}