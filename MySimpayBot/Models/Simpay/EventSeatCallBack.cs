using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class EventSeatPlanInput
    {
        public long eventId { get; set; }
        public long chatId { get; set; }

        public string[] seats { get; set; }
    }
}