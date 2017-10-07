namespace Models.BillPayment
{
    public class wsBillPayment
    {
        public class GetBillInfo_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetBillInfo_Input_Parameters Parameters { get; set; }
        }
        public class GetBillInfo_Input_Parameters
        {
            public string BillID { get; set; }
            public string PaymentID { get; set; }
            public long SessionID { get; set; }
        }

        public class GetBillInfo_Output
        {
            public GetBillInfo_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetBillInfo_Output_Parameters
        {
            public long BillAmount { get; set; }
            public bool BillPaymentIsValid { get; set; }
            public string BillStatus { get; set; }
            public string BillType { get; set; }
        }
        //*****************************

        public class SingleBillPayment_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public SingleBillPayment_Input_Parameters Parameters { get; set; }
        }
        public class SingleBillPayment_Input_Parameters
        {
            public string BillID { get; set; }
            public string PaymentID { get; set; }
            public long SessionID { get; set; }
        }

        public class SingleBillPayment_Output
        {
            public SingleBillPayment_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class SingleBillPayment_Output_Parameters
        {
            public long Amount { get; set; }
            public string PaymentLink { get; set; }
            public string SaleKey { get; set; }
        }



        //****************************
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