namespace Models.XPIN
{
    public class wsXpin
    {
        public class GetProductsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetProductsList_Input_Parameters Parameters { get; set; }
        }
        public class GetProductsList_Input_Parameters
        {
            public int CategoryID { get; set; }
            public long SessionID { get; set; }
        }

        public class GetProductsList_Output
        {
            public GetProductsList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetProductsList_Output_Parameters
        {

            public string ProductHintsLink { get; set; }
            public int ProductID { get; set; }
            public string ProductIcon { get; set; }
            public string ProductName { get; set; }
            public string ProductThumbnail { get; set; }
        }



        //************************
        public class GetSubProductsList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSubProductsList_Input_Parameters Parameters { get; set; }
        }
        public class GetSubProductsList_Input_Parameters
        {
            public int ProductID { get; set; }
            public long SessionID { get; set; }
        }

        public class GetSubProductsList_Output
        {
            public GetSubProductsList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetSubProductsList_Output_Parameters
        {

            public int SubProductAmount { get; set; }
            public string SubProductHints { get; set; }
            public int SubProductID { get; set; }
            public bool SubProductIsActive { get; set; }
            public string SubProductName { get; set; }
        }

        //************************
        public class ReservePin_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public ReservePin_Input_Parameters Parameters { get; set; }
        }
        public class ReservePin_Input_Parameters
        {
            public int SubProductID { get; set; }
            public long SessionID { get; set; }
        }

        public class ReservePin_Output
        {
            public ReservePin_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class ReservePin_Output_Parameters
        {

            public string SaleKey { get; set; }

        }

        //*********************
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
            public string Description { get; set; }
            public string PinCode { get; set; }
        }



    }
}