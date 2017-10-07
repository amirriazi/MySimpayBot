using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Models
{
    public class BodyDefines
    {
        public class sendMessageToMobile
        {
            public string FileID { get; set; }
            public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }
            public string Message { get; set; }
            public string[] ChatIds { get; set; }

        }
        public class SendNotification
        {
            public string FileID { get; set; }
            public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }
            public string Message { get; set; }
            public string[] ChatIds { get; set; }
        }
        public class SendNotificationToAll
        {
            public string FileID { get; set; }
            public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }
            public string Message { get; set; }
            public string[] ChatIds { get; set; }
        }

    }
}