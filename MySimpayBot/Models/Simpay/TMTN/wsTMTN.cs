using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Models.TMTN
{
    public class wsTMTN
    {
        public class GetSimTypesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetSimTypesList_Input_Parameters Parameters { get; set; }
        }
        public class GetSimTypesList_Input_Parameters
        {
            public long SessionID { get; set; }
        }

        public class GetSimTypesList_Output
        {
            public GetSimTypesList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetSimTypesList_Output_Parameters
        {
            public int SimTypeID { get; set; }
            public string SimTypeName { get; set; }
            public string SimTypeShowName { get; set; }
            public string SimTypeThumbnail { get; set; }
        }


        //*************************************

        public class GetCategoriesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetCategoriesList_Input_Parameters Parameters { get; set; }
        }
        public class GetCategoriesList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int SimTypeID { get; set; }
        }

        public class GetCategoriesList_Output
        {
            public GetCategoriesList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetCategoriesList_Output_Parameters
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public string CategoryShowName { get; set; }
            public string CategoryThumbnail { get; set; }
        }


        //*************************************


        public class GetPackagesList_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public GetPackagesList_Input_Parameters Parameters { get; set; }
        }
        public class GetPackagesList_Input_Parameters
        {
            public long SessionID { get; set; }
            public int CategoryID { get; set; }
        }

        public class GetPackagesList_Output
        {
            public GetPackagesList_Output_Parameters[] Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class GetPackagesList_Output_Parameters
        {
            public int PackageAmount { get; set; }
            public string PackageDescription { get; set; }
            public int PackageID { get; set; }
            public string PackageName { get; set; }
            public string PackageShowName { get; set; }
        }


        //*************************************


        public class BuyService_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public BuyService_Input_Parameters Parameters { get; set; }
        }
        public class BuyService_Input_Parameters
        {
            public int Amount { get; set; }
            public string MobileNumber { get; set; }
            public int ServiceTypeID { get; set; }
            public long SessionID { get; set; }
        }

        public class BuyService_Output
        {
            public BuyService_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class BuyService_Output_Parameters
        {
            public string SaleKey { get; set; }
        }


        //*************************************


        public class RedeemService_Input
        {
            public Core.wsInterface.Identity Identity { get; set; }
            public RedeemService_Input_Parameters Parameters { get; set; }
        }
        public class RedeemService_Input_Parameters
        {
            public string SaleKey { get; set; }
            public long SessionID { get; set; }
        }

        public class RedeemService_Output
        {
            public BuyService_Output_Parameters Parameters { get; set; }
            public Core.wsInterface.Status Status { get; set; }
        }

        public class RedeemService_Output_Parameters
        {
            public int Amount { get; set; }
            public string MobileNumber { get; set; }
            public string ServiceTypeShowName { get; set; }
        }


        //*************************************

    }
}