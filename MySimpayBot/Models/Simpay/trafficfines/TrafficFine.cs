using System;
using System.Collections.Generic;

namespace Models.TrafficFine
{
    public class TrafficFineData
    {

        public class header
        {
            public long ticketId { get; set; }
            public string barCode { get; set; }
            public string saleKey { get; set; }
            public bool twoPhaseInquiry { get; set; }
            public string captchaUrl { get; set; }
            public string captchaText { get; set; }
            public string captchaBase64 { get; set; }
            public TransactionStatusEnum status { get; set; }
            public List<Detail> details { get; set; }
            public int count { get; set; }
            public int amount { get; set; }
            public int selectedCount { get; set; }
            public int selectedAmount { get; set; }
        }

        public class Detail
        {
            public long ID { get; set; }
            public long Amount { get; set; }
            public string City { get; set; }
            public int Code { get; set; }
            public DateTime DateTime { get; set; }
            public string Description { get; set; }

            public string LicensePlate { get; set; }
            public string Location { get; set; }
            public string Serial { get; set; }
            public string Type { get; set; }
            public bool selected { get; set; }
            public int row { get; set; }
        }
    }
}