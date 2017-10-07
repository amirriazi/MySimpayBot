using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class JobSendMessageToChat_Input
    {
        public string Body { get; set; }
        public int BusinessID { get; set; }
        public string FetchExpirationDateTime { get; set; }
        public string TypeName { get; set; }
        public string UID { get; set; }
    }
    public class JobSendMessageToChat_Input_Body
    {
        public string[] ChatIds { get; set; }
        public string Message { get; set; }
        public string FileID { get; set; }
        public Telegram.Bot.Types.Enums.MessageType MessageType { get; set; }

    }

}