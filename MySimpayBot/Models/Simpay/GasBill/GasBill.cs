using Shared.WebService;
using System;

namespace Models.GasBill
{
    public class GasBillData
    {

        public long id { get; set; }

        public string gasParticipateCode { get; set; }

        public long amount { get; set; }

        public string billId { get; set; }

        public string paymentId { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }

        public DateTime? paymentDeadLineDate { get; set; }
        public TransactionStatusEnum status { get; set; }




    }
}