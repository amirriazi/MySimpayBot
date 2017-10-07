namespace Models.BillPayment
{
    public class BillPaymentData
    {
        public long id { get; set; }
        public int amount { get; set; }

        public string billId { get; set; }
        public string paymentId { get; set; }
        public string billType { get; set; }
        public string saleKey { get; set; }
        public TransactionStatusEnum status { get; set; }

    }

    public class BillPaymentLast
    {
        public string billId { get; set; }
        public string billType { get; set; }
        public long id { get; set; }
    }
}