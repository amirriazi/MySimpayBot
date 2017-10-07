namespace Models.XPIN
{
    public class XpinData
    {
        public long id { get; set; }
        public int amount { get; set; }
        public XpinCategoryEnum categoryId { get; set; }
        public int productID { get; set; }
        public string productName { get; set; }
        public string productThumbnail { get; set; }
        public int subProductID { get; set; }
        public string subProductName { get; set; }
        public string saleKey { get; set; }
        public TransactionStatusEnum status { get; set; }
    }



}