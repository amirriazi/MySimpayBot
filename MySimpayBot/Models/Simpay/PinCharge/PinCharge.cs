namespace Models.PinCharge
{
    public class PinChargeData
    {
        public long id { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
        public string operatorName { get; set; }
        public int typeId { get; set; }
        public string saleKey { get; set; }
        public string ChargeTypeShowName { get; set; }
        public string pinCode { get; set; }
        public string description { get; set; }

        public TransactionStatusEnum status { get; set; }


    }

    public class OperatorList
    {
        public string OperatorName { get; set; }
        public long TypeID { get; set; }

    }
    public class ChargesList
    {
        public long Amount { get; set; }
        public string Name { get; set; }
        public string OperatorName { get; set; }
        public long TypeID { get; set; }
    }
}