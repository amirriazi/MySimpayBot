using System;
namespace Models
{
    public class wsPanel
    {
        // Input
        public class SendMessage_Input
        {
            public WebServiceIdentity Identity { set; get; }
            public SendMessage_Input_Params Parameters { get; set; }
        }

        public class SendMessage_Input_Params
        {
            public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }
            public string FileID { get; set; }
            public string[] Mobile { get; set; }
            public string Message { get; set; }
            public string StartDateTime { get; set; }

        }
        //Output
        public class SendMessage_Output
        {
            public WebServiceStatus Status { get; set; }
        }



        /*******************/
        public class SendNotification_Input
        {
            public WebServiceIdentity Identity { set; get; }
            public SendNotification_Input_Params Parameters { get; set; }
        }

        public class SendNotification_Input_Params
        {
            public string Message { get; set; }
            public string StartDateTime { get; set; }
            public int UserCount { get; set; }

        }
        //Output
        public class SendNotification_Output
        {
            public WebServiceStatus Status { get; set; }
        }


        /*******************/
        public class SendNotificationToAll_Input
        {
            public WebServiceIdentity Identity { set; get; }
            public SendNotificationToAll_Input_Params Parameters { get; set; }
        }

        public class SendNotificationToAll_Input_Params
        {
            public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }
            public string FileID { get; set; }
            public string Message { get; set; }
            public int UserPerDay { get; set; }
            public string StartDateTime { get; set; }

            public int UserPerBatch { get; set; }
            public int SecondBetweenBatches { get; set; }
            public bool TestMode { get; set; }



        }
        //Output
        public class SendNotificationToAll_Output
        {
            public WebServiceStatus Status { get; set; }
        }



    }
}