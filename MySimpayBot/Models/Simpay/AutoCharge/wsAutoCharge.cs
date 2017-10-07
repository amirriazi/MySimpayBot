namespace Models.AutoCharge
{
    public class wsAutoCharge
    {
        #region Charge
        public class Charge_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public Charge_Input_Parameters Parameters { get; set; }
        }
        public class Charge_Input_Parameters
        {
            public int Amount { get; set; }
            public int ChargeTypeID { get; set; }
            public string MobileNumber { get; set; }
            public long SessionID { get; set; }
        }
        public class Charge_Output
        {
            public Core.wsInterface.Status Status { get; set; }
            public Charge_Output_Parameters Parameters { get; set; }
        }
        public class Charge_Output_Parameters
        {
            public string SaleKey { get; set; }
        }
        #endregion

        #region Redeem

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
            public string ChargeTypeShowName { get; set; }
            public string MobileNumber { get; set; }
        }

        #endregion




    }

}