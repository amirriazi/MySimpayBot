using Shared.WebService;

namespace Models.FixedLineBill
{
    public class FixedLineBillData
    {

        public long id { get; set; }
        public string fixedLineNumber { get; set; }
        public int amount { get; set; }
        public string billId { get; set; }
        public string paymentId { get; set; }
        public TransactionStatusEnum status { get; set; }
    }
}