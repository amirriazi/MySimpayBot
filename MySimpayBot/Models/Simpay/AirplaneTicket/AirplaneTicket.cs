using System;

namespace Models.AirplaneTicket
{
    public class AirplaneTicketData
    {
        public long id { get; set; }

        public int userId { get; set; }

        public DateTime dateEntered { get; set; }

        public string sourceAirportCode { get; set; }

        public string sourceAirportShowName { get; set; }

        public string destinationAirportCode { get; set; }

        public string destinationAirportShowName { get; set; }

        public bool twoWay { get; set; }

        public DateTime? wayGoDateTime { get; set; }

        public DateTime? wayReturnDateTime { get; set; }

        public int adultCount { get; set; }

        public int childCount { get; set; }

        public int infantCount { get; set; }

        public int amount { get; set; }

        public string saleKey { get; set; }
        public int goRow { get; set; }
        public int returnRow { get; set; }
        public int currentPassengerRow { get; set; }
        public TransactionStatusEnum status { get; set; }
    }
}