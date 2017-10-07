using System;

namespace Shared.WebService
{
    public static partial class ProjectValues
    {
        public const long adminChatId = 102379130;

        public static string telegramApiToken
        {
            get
            {
                string _cs = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "S-PC":
                        _cs = @"355253393:AAFNsuNu_5n56r2ZoOajeuZ-tS0lIzhuIFs";
                        break;
                    case "MOEPC":
                        //_cs = @"362745168:AAHAU_oG1tyoOuEiQ97aC9zUQBWfqDCSN8A";
                        _cs = @"355253393:AAFNsuNu_5n56r2ZoOajeuZ-tS0lIzhuIFs";
                        break;

                    default:
                        _cs = @"133954921:AAH6iKm_QiR2wihseUoMXviewVTaFuL2F-M";
                        break;

                }
                return _cs;

            }
        }
        public static string DataFolder
        {
            get
            {
                string _cs = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "S-PC":
                        _cs = @"C:\SharifTech\mySimpayBot\data\";
                        break;
                    case "MOEPC":
                        _cs = @"C:\SharifTech\mySimpayBot\data\";
                        break;

                    default:
                        _cs = @"C:\SharifTech\mySimpayBot\data\";
                        break;

                }
                return _cs;

            }
        }

        public static string JobmManagementCoreUrl
        {
            get
            {
                string _cs = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "S-PC":
                        //_cs = @"http://JobService.simpay.ir/webservices/Core.svc";
                        _cs = @"http://jobmanagement.acx.ir/webservices/Core.svc";
                        break;
                    case "MOEPC":
                        _cs = @"http://JobService.simpay.ir/webservices/Core.svc";
                        break;

                    default:
                        _cs = @"http://jobmanagement.acx.ir/webservices/Core.svc";
                        break;

                }
                return _cs;

            }
        }

        public static string EventSansPlanUrl
        {
            get
            {
                string _cs = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "S-PC":
                        _cs = @"http://37.32.121.153:1026/EventsTicket/SeatManagement";
                        break;
                    case "MOEPC":
                        _cs = @"http://37.32.121.153:1026/EventsTicket/SeatManagement";
                        break;

                    default:
                        _cs = @"http://37.32.121.153:1026/EventsTicket/SeatManagement";
                        break;

                }
                return _cs;

            }
        }

        public static string CinemaTicketSansPlanUrl
        {
            get
            {
                string _cs = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "S-PC":
                        _cs = @"http://spc.simpay.ir/Bot/CinemaPlan";
                        break;
                    case "MOEPC":
                        _cs = @"http://spc.simpay.ir/Bot/CinemaPlan";
                        break;

                    default:
                        _cs = @"http://tgct.simpay.ir/Bot/CinemaPlan";
                        break;

                }
                return _cs;

            }
        }


        static ProjectValues()
        {
            ProjectValues.logMode = 0;
        }
        public const string Domain = "Shariftech";
        public const string ProjectName = "FileSharingBot";

        public static DataBaseConfigure dataBaseConfigure = new DataBaseConfigure();
        public static string CryptographyKey = @"1234512345678976";
        public static string Consumer = "UNKNOWN";

        // for Simpay code send sms and login;
        public static SimpayCore simpayCore = new SimpayCore();

        public const string MainDirectory = @"C:\" + Domain + @"\" + ProjectName + @"\";
        public const string LogConfigFilePath = MainDirectory + @"Configure\Log.config";
        public const string DataBaseConfigFilePath = MainDirectory + @"Configure\DataBaseProperties.config";



        public const int QuestionAnswerRecordsInPage = 3;

        //for  Cinematicket
        public static CinemaTicketDefinitions CinemaTicketDef = new CinemaTicketDefinitions();
        private static int logMode;
        public static int LogMode
        {
            get { return logMode; }
            private set { logMode = value; }
        }

        public static string PaymentLink
        {
            get
            {
                string _l = "";
                switch (Environment.MachineName.ToUpper())
                {
                    case "MOEPC":
                        _l = @"http://localhost:2001/ui/Payment.aspx";
                        break;
                    default:
                        _l = @"http://boxoffice.simpay.ir/ui/Payment.aspx";
                        break;
                }
                return _l;

            }
        }

        // Saman Defs:
        public static SamanPayment SamanPaymentDefs = new SamanPayment();

        // Define Related Classes
        public class SamanPayment
        {
            public string URL = "https://sep.shaparak.ir/Payment.aspx";
            public string MID = "10069061";
            public string PWD = "qwer1234";
            public string callBackURL
            {
                get
                {
                    string _cs = "";
                    switch (Environment.MachineName.ToUpper())
                    {
                        case "MOEPC":
                            _cs = @"http://localhost:2001/ui/ipg_callback_saman.aspx";
                            break;
                        default:
                            _cs = @"http://boxOffice.simpay.ir/ui/ipg_callback_saman.aspx";
                            break;
                    }
                    return _cs;
                }

            }






        }
        public class SimpayCore
        {
            public string username = "gisheapp";
            public string password = "gisheApp@2";
            public string Login_ProjectName = "simsendmarket";//"MCIImiServer";
            public string SubscribeInBusiness_Projectname = "simsendmarket";
            public string SMS_BusinessName = "simGames";
            public string SubscribeInBusiness_BusinessName = "simGames";
            public string Reg_UserName = "simOff";
            public string Reg_Password = "sh@rifSim%123";
            public string Reg_BussinessName = "gisheGame";
        }

        public class CinemaTicketDefinitions
        {

            public ApiURL URL = new ApiURL("http://api.cinematicket.org/mobile/v1/");
            public class ApiURL
            {
                private string Core = "";
                public string Refresh
                {
                    get { return Core + "account/refresh"; }
                }
                public string CheckToken
                {
                    get { return Core + "account/checktoken"; }
                }

                public string Calculate
                {
                    get { return Core + "order/calculate"; }
                }
                public string OrderBegin
                {
                    get { return Core + "order/begin"; }
                }
                public string OrdeEnd
                {
                    get { return Core + "order/end"; }
                }
                public string NewsTop
                {
                    get { return Core + "news/Top"; }
                }
                public string NewsGet
                {
                    get { return Core + "news/get"; }
                }


                public string SansePlane
                {
                    get { return Core + "Sanse/plan"; }
                }
                public string ReserveSeats
                {
                    get { return Core + "sanse/reserve-seat/"; }
                }
                public string OrderCheck
                {
                    get { return Core + "order/check"; }
                }


                public ApiURL(string CoreURL)
                {
                    Core = CoreURL;
                }

            }
        }

        public class DataBaseConfigure
        {
            public int CommandTimeout
            {
                get { return 120; }
            }

            public string ConnectionString
            {
                get
                {
                    string _cs = "";
                    switch (Environment.MachineName.ToUpper())
                    {
                        case "S-PC":
                            _cs = @"Server=S-PC\Alton;Database=simpayBot;User ID=amirriazi;PWD=66177602; max pool size=500";
                            break;
                        case "MOEPC":
                            _cs = @"Server=MOEPC\HOMEPC;Database=SimpayBot;User ID=amirriazi;PWD=66177602; max pool size=500";
                            break;

                        default:
                            _cs = @"Server=WIN-E8E6M34F3M5\SIMPAY,2525;Database=SimpayBot;User ID=bot_user;PWD=b0tP@ssw0rd";
                            break;

                    }
                    return _cs;
                }
            }
        }

    }

    public enum UserIdentityType
    {
        App = 0,
        Panel = 1
    }


}
