using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CinemaTicketCallBackInput
    {
        public long chatId { get; set; }
        public string[] seats { get; set; }
    }
}