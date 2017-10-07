using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Payment
{
    public class PaymentData
    {
        public long id { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public string saleKey { get; set; }
        public string discountCode { get; set; }
        public int discountAmount { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public bool paymentIsPossible { get; set; }
        public bool paymentFinished { get; set; }
        public string status { get; set; }


    }
}
