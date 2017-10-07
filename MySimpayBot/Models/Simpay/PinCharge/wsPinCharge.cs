namespace Models.PinCharge
{
    public class wsPinCharge
    {
        public class GetChargesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetChargesList_Input_Parameters Parameters { get; set; }
        }
        public class GetChargesList_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetChargesList_Output
        {
            public GetChargesList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetChargesList_Output_Parameters
        {
            public int Amount { get; set; }
            public string Name { get; set; }
            public string OperatorName { get; set; }
            public int TypeId { get; set; }
        }


        //*****************

        public class Charge_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public Charge_Input_Parameters Parameters { get; set; }
        }
        public class Charge_Input_Parameters
        {
            public int Amount { get; set; }
            public int ChargeTypeID { get; set; }
            public long SessionID { get; set; }
        }

        public class Charge_Output
        {
            public Charge_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class Charge_Output_Parameters
        {
            public string saleKey { get; set; }
        }


        //*****************

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
            public Redeem_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class Redeem_Output_Parameters
        {
            public int Amount { get; set; }
            public string ChargeTypeShowName { get; set; }
            public string Description { get; set; }
            public string PinCode { get; set; }
        }


        //*****************
    }
}