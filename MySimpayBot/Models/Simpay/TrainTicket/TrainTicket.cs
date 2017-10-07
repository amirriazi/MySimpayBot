using System;

namespace Models.TrainTicket
{
    public class TrainTicketData
    {
        public long id { get; set; }
        public int sourceStationCode { get; set; }
        public string sourceStationShowName { get; set; }
        public int destinationStationCode { get; set; }
        public string destinationStationShowName { get; set; }
        public bool justCompartment { get; set; }
        public int seatCount { get; set; }
        public TicketTypeEnum ticketTypeCode { get; set; }
        public bool twoWay { get; set; }
        public DateTime? wayGoDateTime { get; set; }
        public DateTime? wayReturnDateTime { get; set; }
        public int amount { get; set; }
        public string saleKey { get; set; }
        public int goRow { get; set; }
        public int returnRow { get; set; }
        public int currentPassengerRow { get; set; }

        public TransactionStatusEnum status { get; set; }
        public int lockedRowNumberGo { get; set; }
        public int lockedWagonNumberGo { get; set; }

        public int lockedRowNumberReturn { get; set; }
        public int lockedWagonNumberReturn { get; set; }

    }

    public class LastPath
    {
        public long id { get; set; }
        public string sourceStationShowName { get; set; }
        public string destinationStationShowName { get; set; }
    }



}