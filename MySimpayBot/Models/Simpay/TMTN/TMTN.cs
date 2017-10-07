using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.TMTN
{
    public class TMTNData
    {
        public long id { get; set; }

        public int simTypeId { get; set; }

        public string simTypeShowName { get; set; }

        public int categoryId { get; set; }

        public string categoryShowName { get; set; }

        public int packageId { get; set; }

        public string packageShowName { get; set; }

        public string packageDescription { get; set; }

        public int amount { get; set; }
        public string mobileNumber { get; set; }

        public string saleKey { get; set; }
        public TransactionStatusEnum status { get; set; }
    }
}