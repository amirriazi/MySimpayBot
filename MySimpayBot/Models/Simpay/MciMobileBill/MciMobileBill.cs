using Shared.WebService;

namespace Models.MciMobileBill
{
    public class MciMobileBillData
    {

        public long id { get; set; }
        public Mobile mobileNumber { get; set; }
        public int amount { get; set; }
        public bool final { get; set; }
        public string billId { get; set; }
        public string paymentId { get; set; }
        public TransactionStatusEnum status { get; set; }




    }
}