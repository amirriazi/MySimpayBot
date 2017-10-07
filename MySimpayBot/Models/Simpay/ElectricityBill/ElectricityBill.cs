using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.ElectricityBill
{
    public class ElectricityBillData
    {
        public long id { get; set; }

        public DateTime dateEntered { get; set; }

        public string electricityBillID { get; set; }

        public int amount { get; set; }

        public int debt { get; set; }

        public string fullName { get; set; }

        public string billId { get; set; }

        public string paymentId { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }

        public DateTime? paymentDeadLineDate { get; set; }

        public TransactionStatusEnum status { get; set; }
    }
}