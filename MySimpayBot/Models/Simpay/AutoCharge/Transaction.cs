using Shared.WebService;

namespace Models.AutoCharge
{
    public class Transaction
    {
        public long chatId { get; set; }
        public int id { get; set; }
        public Mobile mobileNumber { get; set; }
        public int chargeTypeId { get; set; }
        public int amount { get; set; }
        public string saleKey { get; set; }
        public TransactionStatusEnum status { get; set; }
        public string transactionId { get; set; }

    }
}